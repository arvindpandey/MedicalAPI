// ============================================================
// src/services/medicineService.js
// ============================================================

import API from './axiosInstance';

export const medicineService = {
  getAll:    (patientId)   => API.get('/Medicine', { params: { patientId } }),
  getById:   (id)          => API.get(`/Medicine/${id}`),
  create:    (data)        => API.post('/Medicine', data),
  update:    (id, details) => API.put(`/Medicine/${id}`, { details }),
  remove:    (id)          => API.delete(`/Medicine/${id}`),
};