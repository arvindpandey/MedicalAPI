// ============================================================
// src/services/axiosInstance.js
// WHY a shared axios instance?
//   - Single place to configure base URL, headers, interceptors
//   - Auto-attaches JWT token to every request
//   - Handles 401 globally (redirect to login)
// ============================================================

import axios from 'axios';

const axiosInstance = axios.create({
  baseURL:  'http://localhost:5059/api',
  headers: { 'Content-Type': 'application/json' },
  timeout: 10000,  // 10s timeout — prevents hanging requests
});
 
// REQUEST INTERCEPTOR: runs before every API call
// Attaches Bearer token from localStorage (set after login)
axiosInstance.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('token');
    if (token) config.headers.Authorization = `Bearer ${token}`;
    return config;
  },
  (error) => Promise.reject(error)
);

// RESPONSE INTERCEPTOR: runs after every API response
// Catches expired/invalid tokens globally — no need to check in each service
axiosInstance.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      // Token expired or invalid — clear storage and send to login
      localStorage.removeItem('token');
      localStorage.removeItem('user');
      window.location.href = '/login';
    }
    return Promise.reject(error);
  }
);

export default axiosInstance;