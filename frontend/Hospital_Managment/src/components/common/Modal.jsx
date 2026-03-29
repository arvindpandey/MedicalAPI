// ============================================================
// src/components/common/Modal.jsx
// Reusable modal — used by all form dialogs (add/edit)
// WHY reusable modal? Consistent UX, single implementation
// ============================================================

export default function Modal({ title, children, onClose }) {
  return (
    // Backdrop: clicking outside closes modal
    <div style={styles.backdrop} onClick={onClose}>
      <div
        style={styles.box}
        onClick={e => e.stopPropagation()}  // Prevent close when clicking inside
      >
        <div style={styles.header}>
          <h3 style={styles.title}>{title}</h3>
          <button style={styles.closeBtn} onClick={onClose}>✕</button>
        </div>
        <div style={styles.body}>{children}</div>
      </div>
    </div>
  );
}

const styles = {
  backdrop: { position: 'fixed', inset: 0, background: 'rgba(0,0,0,0.5)',
              display: 'flex', alignItems: 'center', justifyContent: 'center', zIndex: 1000 },
  box:      { background: '#fff', borderRadius: '12px', width: '540px', maxWidth: '95vw',
              maxHeight: '90vh', overflowY: 'auto', boxShadow: '0 20px 60px rgba(0,0,0,0.3)' },
  header:   { display: 'flex', justifyContent: 'space-between', alignItems: 'center',
              padding: '20px 24px', borderBottom: '1px solid #e2e8f0' },
  title:    { margin: 0, fontSize: '18px', fontWeight: 700, color: '#1a3a5c' },
  closeBtn: { background: 'none', border: 'none', fontSize: '20px',
              cursor: 'pointer', color: '#64748b', padding: '4px' },
  body:     { padding: '24px' },
};
