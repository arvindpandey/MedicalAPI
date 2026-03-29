// ============================================================
// symptomAIService.js
// Handles all API calls related to AI symptom analysis
// Connects to: MedicalAPI → SymptomAIController
// Author: Your Name
// ============================================================

import API from './axiosInstance';

/**
 * Send symptoms to AI for analysis
 * First checks DB cache, calls Groq only on cache miss
 * @param {string[]} symptoms - Array of symptom strings
 * @param {number|null} patientAge - Optional patient age
 */
export const analyzeSymptoms = async (symptoms, patientAge = null) => {
  const response = await API.post('/SymptomAI/analyze', {
    
    patientId: 0,
    symptoms,
    patientAge,
  });
  return response.data;
};

/*
 * Save AI analysis result linked to a specific patient
 * Stores in Tbl_SymptomDetails in MedicalDB
 * @param {number} patientId
 * @param {string[]} symptoms
 * @param {object} aiResult - Result returned from analyzeSymptoms
 */
export const saveSymptomResult = async (patientId, symptoms, aiResult) => {
  const response = await API.post('/SymptomAI/save', {
    patientId,
    symptoms,
    aiAdvice:        aiResult.aiAdvice,
    severity:        aiResult.severity,
    suggestedAction: aiResult.suggestedAction,
  });
  return response.data;
};