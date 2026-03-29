// src/pages/Login.jsx

import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../hooks/useAuth';
import '../Styles/Login.css';

export default function Login() {
  const { login, isLoggedIn } = useAuth();
  const navigate = useNavigate();

  const [form, setForm] = useState({
    loginName: '',
    password: ''
  });

  const [errors, setErrors] = useState({});
  const [loading, setLoading] = useState(false);
  const [apiErr, setApiErr] = useState('');

  // Redirect if already logged in
  useEffect(() => {
    if (isLoggedIn) {
      navigate('/dashboard');
    }
  }, [isLoggedIn, navigate]);

  const validate = () => {
    const e = {};
    if (!form.loginName.trim()) e.loginName = 'Login name is required';
    if (!form.password.trim()) e.password = 'Password is required';
    return e;
  };

  const handleChange = (e) => {
    const { name, value } = e.target;

    setForm(prev => ({
      ...prev,
      [name]: value
    }));

    if (errors[name]) {
      setErrors(prev => ({ ...prev, [name]: '' }));
    }

    if (apiErr) setApiErr('');
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    const errs = validate();
    if (Object.keys(errs).length) {
      setErrors(errs);
      return;
    }

    setLoading(true);
    try {
      await login(form);
      navigate('/dashboard');
    } catch (err) {
      setApiErr(
        err.response?.data?.message ||
        'Login failed. Please check your credentials.'
      );
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="login-page">
      <div className="login-card">

        <div className="login-logo">🏥</div>
        <h1 className="login-title">MedicalDB System</h1>
        <p className="login-subtitle">Sign in to continue</p>

        {apiErr && <div className="login-api-error">{apiErr}</div>}

        <form onSubmit={handleSubmit}>

          <div className="login-field">
            <label>Login Name</label>
            <input
              name="loginName"
              value={form.loginName}
              onChange={handleChange}
              placeholder="Enter your login name"
              className={errors.loginName ? 'input-error' : ''}
              autoFocus
            />
            {errors.loginName && (
              <span className="error-text">{errors.loginName}</span>
            )}
          </div>

          <div className="login-field">
            <label>Password</label>
            <input
              type="password"
              name="password"
              value={form.password}
              onChange={handleChange}
              placeholder="Enter your password"
              className={errors.password ? 'input-error' : ''}
            />
            {errors.password && (
              <span className="error-text">{errors.password}</span>
            )}
          </div>

          <button type="submit" disabled={loading}>
            {loading ? 'Signing in...' : 'Sign In'}
          </button>

        </form>

        <p className="login-hint">
          Contact your system administrator if you have trouble logging in.
        </p>

      </div>
    </div>
  );
}