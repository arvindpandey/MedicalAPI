// ============================================================
// src/pages/Dashboard.jsx
// WHY a dashboard?
//   - First thing user sees after login
//   - Shows summary stats (from vw_DashboardStats view via API)
//   - Quick navigation to key modules
// ============================================================

import { useNavigate as uN } from 'react-router-dom';
import { useAuth as uA }     from '../hooks/useAuth';
import { useFetch as uF }    from '../hooks/useFetch';
import axiosInstance from '../api/axiosInstance';
 
 

export default function  Dashboard() 
{
  const { user }   = uA();
  const navigate   = uN();

  // Fetch dashboard stats from the vw_DashboardStats view
  const { data: stats, loading } = uF(() => axiosInstance.get('/Dashboard/stats'));

  const STAT_CARDS = [
    { label: 'Total Patients',    value: stats?.totalPatients    || 0, icon: '🧑‍⚕️', color: '#dbeafe', route: '/patients'  },
    { label: "Today's Admissions",value: stats?.todayAdmissions  || 0, icon: '📅', color: '#dcfce7', route: '/patients'  },
    { label: 'Active Users',      value: stats?.totalActiveUsers || 0, icon: '👥', color: '#fef9c3', route: '/users'     },
    { label: 'Total Symptoms',    value: stats?.totalSymptoms    || 0, icon: '🩺', color: '#fce7f3', route: '/symptoms'  },
    { label: 'Medicines Logged',  value: stats?.totalMedicines   || 0, icon: '💊', color: '#f3e8ff', route: '/medicines' },
  ];

  return (
    <div>
      {/* Greeting */}
      <div style={ds.greeting}>
        <h1 style={ds.greetTitle}>
          Good {getTimeOfDay()}, {user?.fullName?.split(' ')[0]} 👋
        </h1>
        <p style={ds.greetSub}>Here's what's happening in your system today.</p>
      </div>

      {/* Stats grid */}
      {loading ? (
        <div style={{ padding: '40px', textAlign: 'center', color: '#64748b' }}>
          ⏳ Loading statistics...
        </div>
      ) : (
        <div style={ds.grid}>
          {STAT_CARDS.map(card => (
            <div
              key={card.label}
              style={{ ...ds.card, background: card.color }}
              onClick={() => navigate(card.route)}
            >
              <div style={ds.cardIcon}>{card.icon}</div>
              <div style={ds.cardValue}>{card.value.toLocaleString()}</div>
              <div style={ds.cardLabel}>{card.label}</div>
            </div>
          ))}
        </div>
      )}

      {/* Quick actions */}
      <div style={ds.section}>
        <h2 style={ds.secTitle}>Quick Actions</h2>
        <div style={ds.actionGrid}>
          {[
            { label: 'Register New Patient', icon: '➕', route: '/patients', color: '#2563eb' },
            { label: 'Log Symptoms',         icon: '🩺', route: '/symptoms', color: '#16a34a' },
            { label: 'Prescribe Medicine',   icon: '💊', route: '/medicines',color: '#7c3aed' },
          ].map(a => (
            <button key={a.label} onClick={() => navigate(a.route)}
              style={{ ...ds.actionBtn, background: a.color }}>
              {a.icon} {a.label}
            </button>
          ))}
        </div>
      </div>
    </div>
  );
}
// Helper: returns morning/afternoon/evening based on current time
function getTimeOfDay() {
  const h = new Date().getHours();
  if (h < 12) return 'morning';
  if (h < 17) return 'afternoon';
  return 'evening';
}

const ds = {
  greeting:    { marginBottom: '28px' },
  greetTitle:  { margin: 0, fontSize: '26px', fontWeight: 800, color: '#1a3a5c' },
  greetSub:    { margin: '6px 0 0', color: '#64748b', fontSize: '15px' },
  grid:        { display: 'grid', gridTemplateColumns: 'repeat(auto-fill, minmax(180px, 1fr))',
                 gap: '16px', marginBottom: '32px' },
  card:        { padding: '24px 20px', borderRadius: '12px', cursor: 'pointer',
                 transition: 'transform 0.2s, box-shadow 0.2s', textAlign: 'center',
                 boxShadow: '0 2px 8px rgba(0,0,0,0.06)' },
  cardIcon:    { fontSize: '32px', marginBottom: '8px' },
  cardValue:   { fontSize: '28px', fontWeight: 800, color: '#1a3a5c', lineHeight: 1 },
  cardLabel:   { fontSize: '13px', color: '#64748b', marginTop: '4px', fontWeight: 500 },
  section:     { background: '#fff', borderRadius: '12px', padding: '24px',
                 boxShadow: '0 1px 8px rgba(0,0,0,0.06)' },
  secTitle:    { margin: '0 0 16px', fontSize: '18px', fontWeight: 700, color: '#1a3a5c' },
  actionGrid:  { display: 'flex', gap: '12px', flexWrap: 'wrap' },
  actionBtn:   { padding: '12px 24px', color: '#fff', border: 'none', borderRadius: '10px',
                 cursor: 'pointer', fontWeight: 700, fontSize: '15px' },
};