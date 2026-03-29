// ============================================================
// FILE: src/components/symptoms/SymptomChecker/Symptomcomponents/ChatMessage.jsx
//
// PURPOSE:
//   Renders ONE message in the chat history.
//
// TWO MESSAGE TYPES:
//   role = 'user' → right side, blue bubble, shows symptom chips
//   role = 'ai'   → left side, white card, shows AI analysis result
//
// PROPS:
//   message  → {
//                id, role,
//                symptoms[],          ← always present on both types
//                aiAdvice,            ← AI only
//                severity,            ← AI only: "Mild"|"Moderate"|"Severe"
//                suggestedAction,     ← AI only
//                servedFromCache      ← AI only: true/false
//              }
//   isSaved  → true = already saved to DB, show tick instead of button
//   onSave   → function(messageId) called when Save button clicked
// ============================================================

import './ChatMessage.css';

// ── Severity colour config ────────────────────────────────────
// WHY inline style? The colour is data-driven per severity value —
// cleaner than 4 separate CSS classes with identical structure.
//
// Covers all values your .NET Core API can return:
//   "Mild"     → green
//   "Moderate" → yellow/amber
//   "Severe"   → red
//   "Unknown"  → grey fallback (if API returns something unexpected)
const SEVERITY_STYLES = {
  Mild:     { background: '#e6f9f0', border: '0.5px solid #bbf7d0', color: '#0f6e56', icon: '🟢' },
  Moderate: { background: '#fff8e1', border: '0.5px solid #fde68a', color: '#854f0b', icon: '🟡' },
  Severe:   { background: '#fdecea', border: '0.5px solid #fca5a5', color: '#a32d2d', icon: '🔴' },
  Unknown:  { background: '#f1efe8', border: '0.5px solid #d3d1c7', color: '#5f5e5a', icon: '⚪' },
};

export default function ChatMessage({ message, isSaved, onSave }) {

  // Determine message type once — used throughout render
  const isUser = message.role === 'user';

  // Pick the severity style — fall back to Unknown if API returns
  // something unexpected (e.g. "High" instead of "Severe")
  const sevStyle = SEVERITY_STYLES[message.severity] || SEVERITY_STYLES.Unknown;

  return (
    // Outer wrapper: flex row
    // --user  → justify-content: flex-end  (bubble on RIGHT)
    // --ai    → justify-content: flex-start (bubble on LEFT)
    <div className={`chat-message ${isUser ? 'chat-message--user' : 'chat-message--ai'}`}>

      {/* ── AI AVATAR (left side, only for AI messages) ── */}
      {!isUser && (
        <div className="chat-message__avatar chat-message__avatar--ai">🩺</div>
      )}

      {/* ── MESSAGE BUBBLE ── */}
      <div className={`chat-message__bubble ${isUser ? 'chat-message__bubble--user' : 'chat-message__bubble--ai'}`}>

        {/* ════════════════════════════════════════════
            USER BUBBLE
            Shows the symptom chips the user entered
            ════════════════════════════════════════════ */}
        {isUser && (
          <div>
            {/* Small label above the chips */}
            <div className="chat-message__chip-label">Symptoms analyzed:</div>

            {/* Symptom chips row */}
            <div className="chat-message__chips">
              {message.symptoms.map(function (symptom) {
                return (
                  <span key={symptom} className="chat-message__chip">
                    {symptom}
                  </span>
                );
              })}
            </div>
          </div>
        )}

        {/* ════════════════════════════════════════════
            AI BUBBLE
            Shows severity, advice, action, save button
            ════════════════════════════════════════════ */}
        {!isUser && (
          <div>

            {/* SEVERITY BADGE ─────────────────────────
                Inline style applies the colour from SEVERITY_STYLES.
                Also shows ⚡ cached badge inline when served from DB. */}
            <div className="chat-message__severity" style={sevStyle}>
              {sevStyle.icon}&nbsp;{message.severity}
              {message.servedFromCache && (
                <span className="chat-message__cache-badge">⚡ cached</span>
              )}
            </div>

            {/* AI ADVICE TEXT ──────────────────────────
                white-space: pre-wrap in CSS preserves any
                line breaks in the AI response text.             */}
            <p className="chat-message__advice">{message.aiAdvice}</p>

            {/* SUGGESTED ACTION BOX ───────────────────*/}
            <div className="chat-message__action-box">
              ✅&nbsp;<strong>Suggested:</strong>&nbsp;{message.suggestedAction}
            </div>

            {/* SAVE BUTTON / SAVED TICK ───────────────
                isSaved = false → show "💾 Save to patient record"
                isSaved = true  → show "✓ Saved to patient record"  */}
            {!isSaved ? (
              <button
                className="chat-message__save-btn"
                onClick={function () { onSave(message.id); }}
              >
                💾 Save to patient record
              </button>
            ) : (
              <p className="chat-message__saved-text">✓ Saved to patient record</p>
            )}

            {/* DISCLAIMER ─────────────────────────────*/}
            <p className="chat-message__disclaimer">
              ⚕️ AI guidance only — always consult a qualified doctor.
            </p>

          </div>
        )}

      </div>{/* end bubble */}

      {/* ── USER AVATAR (right side, only for user messages) ── */}
      {isUser && (
        <div className="chat-message__avatar chat-message__avatar--user">👤</div>
      )}

    </div>
  );
}