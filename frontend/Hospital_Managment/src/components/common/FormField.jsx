// ============================================================
// src/components/common/FormField.jsx
// Reusable input field with label + validation error display
// WHY? Consistent form UI across all 5 feature modules
// ============================================================

export default function FormField({ label, name, type = 'text', value, onChange, onBlur, error, options }) {
  const inputStyle = {
    ...styles.input,
    borderColor: error ? '#e74c3c' : '#cbd5e1',
  };

  return (
    <div style={styles.field}>
      <label style={styles.label}>{label}</label>

      {/* Render select or input based on whether options are provided */}
      {options ? (
        <select name={name} value={value} onChange={onChange} onBlur={onBlur} style={inputStyle}>
          <option value="">-- Select {label} --</option>
          {options.map(opt => (
            <option key={opt.value} value={opt.value}>{opt.label}</option>
          ))}
        </select>
      ) : (
        <input
          type={type}
          name={name}
          value={value}
          onChange={onChange}
          onBlur={onBlur}
          style={inputStyle}
        />
      )}

      {/* Show validation error only if field was touched */}
      {error && <span style={styles.error}>{error}</span>}
    </div>
  );
}

const styles = {
  field: { marginBottom: '16px' },
  label: { display: 'block', marginBottom: '6px', fontSize: '13px',
           fontWeight: 600, color: '#374151' },
  input: { width: '100%', padding: '10px 12px', border: '1.5px solid #cbd5e1',
           borderRadius: '8px', fontSize: '14px', outline: 'none',
           transition: 'border-color 0.2s', boxSizing: 'border-box' },
  error: { color: '#e74c3c', fontSize: '12px', marginTop: '4px', display: 'block' },
};