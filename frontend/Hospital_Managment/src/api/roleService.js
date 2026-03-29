// ============================================================
// src/services/roleService.js
// ============================================================

import API from './axiosInstance';

export const roleService = {
  getAll:    ()           => API.get('/Role'),
  getById:   (id)         => API.get(`/Role/${id}`),
  create:    (data)       => API.post('/Role', data),
  update:    (id, data)   => API.put(`/Role/${id}`, data),
  remove:    (id)         => API.delete(`/Role/${id}`),
};