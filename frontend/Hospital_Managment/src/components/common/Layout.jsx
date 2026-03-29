// ============================================================
// src/components/common/Layout.jsx
// WHY Layout?
//   - Shared shell (sidebar + navbar) across all protected pages
//   - <Outlet /> is where react-router renders the current page
//   - Child routes inherit Layout without repeating the shell
// ============================================================

import { Outlet } from 'react-router-dom';
import Navbar  from './Navbar';
import Sidebar from './Sidebar';

export default function Layout() {
  return (
    <div style={styles.shell}>
      {/* Fixed left sidebar */}
      <Sidebar />

      {/* Main content area */}
      <div style={styles.main}>
        <Navbar />

        {/* react-router renders matched child route here */}
        <div style={styles.content}>
          <Outlet />
        </div>
      </div>
    </div>
  );
}

const styles = {
  shell:   { display: 'flex', minHeight: '100vh', background: '#f0f4f8' },
  main:    { flex: 1, display: 'flex', flexDirection: 'column' },
  content: { flex: 1, padding: '24px' },
};