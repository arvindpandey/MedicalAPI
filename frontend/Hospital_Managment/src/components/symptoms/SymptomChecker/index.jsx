// ============================================================
// FILE: src/components/symptoms/SymptomChecker/index.jsx
//
// PURPOSE:
//   Main page for the AI Symptom Checker feature.
//   Holds ALL state and business logic.
//   Composes:
//     SymptomInput  → bottom bar to type and add symptom chips
//     ChatMessage   → each bubble in the chat history
//
// FIXES APPLIED vs your original code:
//   1. analyzeSymptoms was used as BOTH an axios instance AND
//      a function — FIXED: patient fetch now uses 'API' directly
//      from axiosInstance (imported separately)
//   2. Import paths corrected to match your actual folder structure
//   3. patientAge parseInt guard added (NaN check)
//   4. handleSave properly reads symptoms from message.symptoms
// ============================================================

import { useState, useEffect, useRef } from 'react';

// These are the TWO things exported from symptomAIService.js
import { analyzeSymptoms, saveSymptomResult } from '../../../api/symptomAIService';

// Separate axios instance for the patient dropdown fetch
// (analyzeSymptoms is a function, not an axios instance)
import API from '../../../api/axiosInstance';

// Sub-components
import ChatMessage  from './Symptomcomponents/ChatMessage';
import SymptomInput from './Symptomcomponents/SymptomInput';

// Page styles
import './SymptomChecker.css';

function SymptomChecker() {

  // Symptom chips currently staged for analysis
  // e.g. ["fever", "headache", "cough"]
  const [chips, setChips] = useState([]);

  // Full chat history — alternating user + AI messages
  // Each message: { id, role: 'user'|'ai', symptoms, aiAdvice, ... }
  const [messages, setMessages] = useState([]);

  // Set of AI message IDs that have been saved to patient record
  // Using Set so .has() lookup is O(1)
  const [savedIds, setSavedIds] = useState(new Set());

  // Patient list for the dropdown (fetched from your existing API)
  const [patients, setPatients] = useState([]);

  // Currently selected patient ID (required to save a result)
  const [selectedPatientId, setSelectedPatientId] = useState('');

  // Optional patient age — sent to AI for better context
  const [patientAge, setPatientAge] = useState('');

  // true while the API call is running
  const [loading, setLoading] = useState(false);

  // Error string shown below chat area
  const [error, setError] = useState('');

  // Ref to the invisible div at the bottom of the chat
  // Used to auto-scroll to the latest message
  const chatBottomRef = useRef(null);

  // ── Load patient list when page first opens ─────────────
  // WHY: needed for the "Save to patient record" dropdown
  // NOTE: We use 'API' (axiosInstance) here, NOT analyzeSymptoms
  //       analyzeSymptoms is a function — it cannot call .get()
  useEffect(function () {
    API.get('/Patient')
      .then(function (res) {
        setPatients(res.data || []);
      })
      .catch(function () {
        // Non-critical — page works without patient list
        // User just won't be able to save results
        console.warn('Could not load patients for dropdown');
      });
  }, []); // empty [] = run once on mount

  // ── Auto-scroll to bottom on new message ────────────────
  // Runs whenever messages array or loading state changes
  useEffect(function () {
    if (chatBottomRef.current) {
      chatBottomRef.current.scrollIntoView({ behavior: 'smooth' });
    }
  }, [messages, loading]);

  // ── handleAnalyze: sends chips to AI for analysis ───────
  async function handleAnalyze() 
  {
    // Don't do anything if no chips staged
    if (chips.length === 0) return;

    setLoading(true);
    setError('');

    // STEP 1: Add user's symptom message to chat immediately
    // WHY "optimistic"? Shows the message before API responds
    //                    so UI feels instant
    const userMsg = {
      id:       Date.now(),
      role:     'user',
      symptoms: [...chips], // copy the array so state changes don't affect it
    };
    setMessages(function (prev) { return [...prev, userMsg]; });

    try {
       
      // STEP 2: Parse age — parseInt('') returns NaN, use null instead
      const age = patientAge !== '' ? parseInt(patientAge) : null;
      const validAge = isNaN(age) ? null : age;

      // STEP 3: Call the AI analysis API
      // analyzeSymptoms → POST /api/SymptomAI/analyze
      // Returns: { aiAdvice, severity, suggestedAction, servedFromCache }
      const result = await analyzeSymptoms(chips, validAge);

      // STEP 4: Add AI response to chat
      const aiMsg = {
        id:              Date.now() + 1,
        role:            'ai',
        symptoms:        [...chips],       // keep symptoms for save operation
        aiAdvice:        result.aiAdvice,
        severity:        result.severity,
        suggestedAction: result.suggestedAction,
        servedFromCache: result.servedFromCache,
      };
      setMessages(function (prev) { return [...prev, aiMsg]; });

      // STEP 5: Clear the chips after successful analysis
      setChips([]);

    } catch (err) {
      setError('Failed to analyze. Please check the API is running.');
      console.error('handleAnalyze error:', err);
    } finally {
      setLoading(false);
    }
  }

  // ── handleSave: saves an AI result to a patient record ──
  async function handleSave(messageId) {

    // Patient must be selected first
    if (!selectedPatientId) {
      alert('Please select a patient from the dropdown first.');
      return;
    }

    // Find the AI message by its ID
    const message = messages.find(function (m) { return m.id === messageId; });
    if (!message) return;

    try {
      // saveSymptomResult → POST /api/SymptomAI/save
      await saveSymptomResult(
        parseInt(selectedPatientId),
        message.symptoms,   // the symptom chips that were analyzed
        message             // the full AI result { aiAdvice, severity, ... }
      );
 ;
      // Mark this message as saved — button changes to ✅ badge
      setSavedIds(function (prev) {
        return new Set([...prev, messageId]);
      });

    } catch (err) {
      alert('Could not save to patient record. Please try again.');
      console.error('handleSave error:', err);
    }
  }

  // ── RENDER ───────────────────────────────────────────────
  return (
    <div className="symptom-page">

      {/* ── PAGE HEADER ── */}
      <h2 className="symptom-page__title">🩺 AI Symptom Checker</h2>
      <p className="symptom-page__subtitle">
        Enter patient symptoms · results are cached in your MedicalDB database
      </p>

      {/* ── PATIENT SELECTOR + AGE ── */}
      {/* Patient is required to use the Save button */}
      <div className="symptom-page__selectors">

        <select
          className="symptom-page__patient-select"
          value={selectedPatientId}
          onChange={function (e) { setSelectedPatientId(e.target.value); }}
        >
          <option value="">— Select patient (required to save result) —</option>
          {patients.map(function (p) {
            return (
              <option key={p.patientId} value={p.patientId}>
                {p.fullName || p.patientName}
              </option>
            );
          })}
        </select>

        <input
          className="symptom-page__age-input"
          type="number"
          value={patientAge}
          min="0"
          max="130"
          onChange={function (e) { setPatientAge(e.target.value); }}
          placeholder="Age (optional)"
        />
      </div>

      {/* ── CHAT HISTORY AREA ── */}
      <div className="symptom-page__chat-area">

        {/* Empty state: shown before first message */}
        {messages.length === 0 && !loading && (
          <div className="symptom-page__empty-state">
            <div className="symptom-page__empty-state-icon">🩺</div>
            Add symptoms below and click Analyze
          </div>
        )}

        {/* Render each message */}
        {messages.map(function (msg) {
          return (
            <ChatMessage
              key={msg.id}
              message={msg}
              isSaved={savedIds.has(msg.id)}
              onSave={handleSave}
            />
          );
        })}

        {/* Typing indicator while AI is processing */}
        {loading && (
          <div className="symptom-page__typing">
            <span>🩺</span> Analyzing symptoms...
          </div>
        )}

        {/* Invisible anchor div — scrolled into view on new messages */}
        <div ref={chatBottomRef} />
      </div>

      {/* ── ERROR MESSAGE ── */}
      {error && (
        <div className="symptom-page__error">⚠️ {error}</div>
      )}

      {/* ── BOTTOM INPUT BAR ── */}
      {/* SymptomInput manages its own text state */}
      {/* chips and setChips live here so parent controls the data */}
      <SymptomInput
        chips={chips}
        setChips={setChips}
        onAnalyze={handleAnalyze}
        loading={loading}
      />

    </div>
  );
}

export default SymptomChecker;