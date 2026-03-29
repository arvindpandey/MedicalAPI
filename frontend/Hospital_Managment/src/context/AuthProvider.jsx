// src/context/AuthProvider.jsx
import { useState } from 'react';
import { AuthContext } from './AuthContext';
import { authService } from '../api/authService';

export const AuthProvider = ({ children }) => {
   
  const [user, setUser] = useState(() => {
    const stored = localStorage.getItem('user');
    if (!stored)
      return null;
    try {
          return JSON.parse(stored);
        } 
    catch (err) 
    {
      console.error('Invalid user in localStorage',err);
      localStorage.removeItem('user');
      return null;
    }
  });
  
  const [hasRole,SetHasRole] = useState(()=>{
    const StoredHasRole = localStorage.getItem("hasRole");
    if(!StoredHasRole)
       return null;
      try {
          return JSON.parse(StoredHasRole);
        } 
    catch (err) 
    {
      console.error('Invalid hasRole in localStorage',err);
      localStorage.removeItem('user');
      return null;
    }
  })
 

  const login = async (credentials) => 
  {

   localStorage.removeItem('user', '');

    const response = await authService.login(credentials);

    console.log("Full API Response:", response);
    console.log("Response Data:", response.data);

    const { data } = response;

    localStorage.setItem('token', data.token);
    localStorage.setItem('user', JSON.stringify(data.fullName));
    localStorage.setItem('hasRole',JSON.stringify(data.role));
    console.log("Stored User:", data.fullName);

    setUser(data.fullName);

    SetHasRole(data.role);
  };

  const logout = () => {
    localStorage.clear();
    setUser(null);
  };

  return (
    <AuthContext.Provider value={{ hasRole,user, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
};