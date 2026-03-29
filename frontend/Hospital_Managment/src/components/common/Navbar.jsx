 //============================================================
// src/components/common/Navbar.jsx
// Shows logged-in user name and logout button
// ============================================================

import { useAuth }     from '../../hooks/useAuth';
import { useNavigate } from 'react-router-dom';

export default function Navbar() {
  const { user, logout } = useAuth();
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  return (
    <nav style={styles.nav}>
      <span style={styles.title}>🏥 MedicalDB System</span>
      <div style={styles.right}>
        <span style={styles.user}>
          👤 {user?.fullName} ({user?.role})
        </span>
        <button onClick={handleLogout} style={styles.logoutBtn}>
          Logout
        </button>
      </div>
    </nav>
  );
}

const styles = {
  nav:       { display: 'flex', justifyContent: 'space-between', alignItems: 'center',
               padding: '12px 24px', background: '#1a3a5c', color: '#fff' },
  title:     { fontSize: '18px', fontWeight: 700 },
  right:     { display: 'flex', alignItems: 'center', gap: '16px' },
  user:      { fontSize: '14px', opacity: 0.85 },
  logoutBtn: { padding: '6px 16px', background: '#e74c3c', color: '#fff',
               border: 'none', borderRadius: '6px', cursor: 'pointer', fontWeight: 600 },
};