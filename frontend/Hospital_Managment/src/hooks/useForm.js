// ============================================================
// src/hooks/useForm.js
// WHY custom form hook?
//   - Centralizes form state, change handling, and validation
//   - Keeps component JSX clean
// ============================================================

import { useState } from 'react';

export function useForm(initialValues, validateFn) {
  const [values, setValues]   = useState(initialValues);
  const [errors, setErrors]   = useState({});
  const [touched, setTouched] = useState({});

  // Handle any input change — works with name attribute
  const handleChange = (e) => {
    const { name, value, type, checked } = e.target;
    setValues(prev => ({
      ...prev,
      [name]: type === 'checkbox' ? checked : value
    }));
    // Clear error for this field when user starts typing
    if (errors[name]) setErrors(prev => ({ ...prev, [name]: '' }));
  };

  // Mark field as touched on blur (shows validation errors)
  const handleBlur = (e) => {
    setTouched(prev => ({ ...prev, [e.target.name]: true }));
  };

  // Run validation before submit
  const validate = () => {
    if (!validateFn) return true;
    const newErrors = validateFn(values);
    setErrors(newErrors);
    // Mark all fields as touched to show all errors on submit
    setTouched(Object.keys(values).reduce((acc, key) => ({ ...acc, [key]: true }), {}));
    return Object.keys(newErrors).length === 0;
  };

  // Reset form to initial values
  const reset = () => {
    setValues(initialValues);
    setErrors({});
    setTouched({});
  };

  // Set values externally (e.g., when editing an existing record)
  const setFormValues = (newValues) => setValues(newValues);

  return { values, errors, touched, handleChange, handleBlur, validate, reset, setFormValues };
}