// ============================================================
// SymptomInput.jsx
// Bottom input bar — handles typing, chip add/remove, analyze
// Props:
//   chips      - current symptom chips array
//   setChips   - setter for chips
//   onAnalyze  - called when Analyze button is clicked
//   loading    - disables button during API call
// ============================================================

import { useState } from 'react';
import '../Symptomcomponents/SymptomInput.css';

export default function SymptomInput({ chips, setChips, onAnalyze, loading }) {
  const [inputText, setInputText] = useState('');

  // Add current input value as a chip (avoid duplicates)
  const addChip = () => {
    const value = inputText.trim().toLowerCase();
    if (value && !chips.includes(value)) {
      setChips(prev => [...prev, value]);
    }
    setInputText('');
  };

  // Remove a specific chip by value
  const removeChip = (chipToRemove) => {
    setChips(prev => prev.filter(c => c !== chipToRemove));
  };

  // Allow Enter or comma key to add chip quickly
  const handleKeyDown = (e) => {
    if (e.key === 'Enter' || e.key === ',') {
      e.preventDefault();
      addChip();
    }
  };

  const hasChips   = chips.length > 0;
  const canAnalyze = hasChips && !loading;

  return (
    <div className="symptom-input">

      {/* Chip display row — only shown when chips exist */}
      {hasChips && (
        <div className="symptom-input__chips">
          {chips.map(chip => (
            <span key={chip} className="symptom-input__chip">
              {chip}
              <button
                className="symptom-input__chip-remove"
                onClick={() => removeChip(chip)}
                title={`Remove ${chip}`}>
                ×
              </button>
            </span>
          ))}
        </div>
      )}

      {/* Input row */}
      <div className="symptom-input__row">
        <input
          className="symptom-input__text"
          value={inputText}
          onChange={e => setInputText(e.target.value)}
          onKeyDown={handleKeyDown}
          placeholder="Type a symptom, press Enter... (fever, cough, headache)"
        />

        <button className="symptom-input__add-btn" onClick={addChip}>
          + Add
        </button>

        <button
          className={`symptom-input__analyze-btn ${canAnalyze ? 'symptom-input__analyze-btn--active' : 'symptom-input__analyze-btn--disabled'}`}
          onClick={onAnalyze}
          disabled={!canAnalyze}>
          {loading ? '⏳ Analyzing...' : '🔍 Analyze'}
        </button>
      </div>

    </div>
  );
}