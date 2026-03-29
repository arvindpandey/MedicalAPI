// ============================================================
// src/services/userService.js
// ============================================================

import API from './axiosInstance';

export const userService = {
  getAll:    ()           => API.get('/User'),
  getById:   (id)         => API.get(`/User/${id}`),
  create:    (data)       => API.post('/User', data),
  update:    (id, data)   => API.put(`/User/${id}`, data),
  remove:    (id)         => API.delete(`/User/${id}`),
};