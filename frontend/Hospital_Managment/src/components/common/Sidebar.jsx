// src/components/common/Sidebar.jsx
// Navigation links — role-based visibility
// ============================================================

import { NavLink } from 'react-router-dom';
import { useAuth }     from '../../hooks/useAuth';

// Nav items — admin field controls visibility
const NAV_ITEMS = [
  { to: '/dashboard', label: '📊 Dashboard', adminOnly: false },
  { to: '/patients',  label: '🧑‍⚕️ Patients',  adminOnly: false },
  { to: '/symptoms',  label: '🩺 Symptoms',  adminOnly: false },
  { to: '/medicines', label: '💊 Medicines', adminOnly: false },
  { to: '/users',     label: '👥 Users',     adminOnly: true  },
  { to: '/roles',     label: '🔑 Roles',     adminOnly: true  },
  { to: '/symptom-checker', label: '🩺 Symptom-checker'   , adminOnly: true  },
 
];

export default function Sidebar() {
 const { hasRole } = useAuth();
     console.log("RhasRole:", hasRole);

  return (
    <aside style={styles.aside}>
      <div style={styles.logo}>Medical<br />Admin</div>
      <nav>
        {NAV_ITEMS
          // Filter out admin-only items for non-admins
          .filter(item => !item.adminOnly || hasRole)
          .map(item => (
            <NavLink
              key={item.to}
              to={item.to}
              style={({ isActive }) => ({
                ...styles.link,
                background: isActive ? '#2563eb' : 'transparent',
              })}
            >
              {item.label}
            </NavLink>
          ))
        }
      </nav>
    </aside>
  );
}

const styles = {
  aside: { width: '220px', background: '#1a3a5c', color: '#fff',
           display: 'flex', flexDirection: 'column', minHeight: '100vh' },
  logo:  { padding: '24px 16px', fontSize: '20px', fontWeight: 800,
           borderBottom: '1px solid rgba(255,255,255,0.1)', lineHeight: 1.3 },
  link:  { display: 'block', padding: '12px 20px', color: '#e2e8f0',
           textDecoration: 'none', fontSize: '14px', fontWeight: 500,
           borderRadius: '6px', margin: '2px 8px', transition: 'background 0.2s' },
};