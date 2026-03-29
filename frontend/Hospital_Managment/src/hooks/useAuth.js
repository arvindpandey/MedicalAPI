
import { useContext } from 'react';
import { AuthContext } from '../context/AuthContext';

export function useAuth() {
  const ctx = useContext(AuthContext);
   console.log("useAuth:", ctx);
  if (!ctx) throw new Error('useAuth must be used within <AuthProvider>');
  return ctx;
} 