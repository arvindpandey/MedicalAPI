// ============================================================
// src/services/symptomService.js
// ============================================================

import API from './axiosInstance';

export const symptomService = {
  getAll:       (patientId)   => API.get('/Symptom', { params: { patientId } }),
  getById:      (id)          => API.get(`/Symptom/${id}`),
  create:       (data)        => API.post('/Symptom', data),
  update:       (id, details) => API.put(`/Symptom/${id}`, { details }),
  remove:       (id)          => API.delete(`/Symptom/${id}`),
};