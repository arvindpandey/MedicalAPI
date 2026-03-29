 
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom'; 
import Login from './pages/Login';
import Layout from './components/common/Layout';
import Dashboard from '././pages/Dashboard';
import Patient from './components/Patient/Patient';
import { useAuth }      from './hooks/useAuth';
import { AuthProvider } from './context/AuthProvider';
import SymptomChecker from './components/symptoms/SymptomChecker';

const PrivateRoute = ({ children }) => 
{ 
  // localStorage.removeItem('token');
  // localStorage.removeItem('user');
  const { user } = useAuth();   
  return user ? children : <Navigate to="/login" />;
};

export default function App() {
  return (
    <AuthProvider>
      <BrowserRouter>
        <Routes>
          <Route path="/login" element={<Login />} />
          <Route path="/" element={<PrivateRoute><Layout /></PrivateRoute>}>
            <Route path="dashboard" element={<Dashboard />} />
            <Route path="patients"  element={<Patient />} />
            <Route path="/symptom-checker" element={<SymptomChecker />} />
            {/* <Route path="symptoms"  element={<Symptoms />} />
            <Route path="medicines" element={<Medicines />} /> */}
            {/* <Route path="users"     element={<Users />} /> */}
          </Route>
        </Routes>
      </BrowserRouter>
    </AuthProvider>
  );
}