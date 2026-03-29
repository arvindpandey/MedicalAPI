// ============================================================
// src/components/Patient/Patient.jsx
// SAME PATTERN as User.jsx:
//   Container manages state → renders PatientList + PatientForm
// ============================================================

import { useState } from 'react';
 
import { useFetch } from '../../hooks/useFetch';
import { patientService } from '../../api/patientService';

// ── Validation for Patient Form ───────────────────────────
function validatePatient(values) {
    const errors = {};
    if (!values.patientName?.trim()) errors.patientName = 'Patient name is required';
    if (!values.patientGeneder) errors.patientGeneder = 'Gender is required';
    if (!values.bloodGroup) errors.bloodGroup = 'Blood group is required';
    if (values.age && (values.age < 0 || values.age > 130))
        errors.age = 'Enter a valid age';
    if (values.weight && (values.weight < 1 || values.weight > 500))
        errors.weight = 'Enter a valid weight (kg)';
    return errors;
}

const EMPTY_PATIENT = {
    patientName: '', patientGeneder: '', address: '',
    bloodGroup: '', age: '', weight: ''
};

const BLOOD_GROUPS = ['A+', 'A-', 'B+', 'B-', 'AB+', 'AB-', 'O+', 'O-'];
const GENDER_OPTIONS = [{ value: 'Male', label: 'Male' }, { value: 'Female', label: 'Female' }, { value: 'Other', label: 'Other' }];

export default function Patient() {
    const { data: patients, loading, error, refetch } = useFetch(patientService.getAll);
    const [showForm, setShowForm] = useState(false);
    const [editing, setEditing] = useState(null);
    const [saving, setSaving] = useState(false);
    const [toast, setToast] = useState(null);
    const [form, setForm] = useState(EMPTY_PATIENT);
    const [formErrors, setFormErrors] = useState({});

    const showToast = (msg, type = 'success') => { setToast({ msg, type }); setTimeout(() => setToast(null), 3000); };

    const handleAdd = () => { setEditing(null); setForm(EMPTY_PATIENT); setFormErrors({}); setShowForm(true); };
    const handleEdit = (p) => {
        setEditing(p); setForm({
            patientName: p.patientName, patientGeneder: p.patientGeneder,
            address: p.address, bloodGroup: p.bloodGroup, age: p.age || '', weight: p.weight || ''
        });
        setFormErrors({}); setShowForm(true);
    };

    const handleChange = (e) => {
        const { name, value } = e.target;
        setForm(prev => ({ ...prev, [name]: value }));
        if (formErrors[name]) setFormErrors(prev => ({ ...prev, [name]: '' }));
    };

    const handleSubmit = async () => {
        const errs = validatePatient(form);
        if (Object.keys(errs).length > 0) { setFormErrors(errs); return; }
        setSaving(true);
        try {
            const dto = {
                ...form, age: form.age ? parseInt(form.age) : null,
                weight: form.weight ? parseFloat(form.weight) : null
            };
            if (editing) { await patientService.update(editing.patientID, dto); showToast('Patient updated!'); }
            else { await patientService.create(dto); showToast('Patient added!'); }
            setShowForm(false); refetch();
        } catch (err) { showToast(err.response?.data?.message || 'Error saving patient', 'error'); }
        finally { setSaving(false); }
    };

    const handleDelete = async (id, name) => {
        if (!window.confirm(`Delete patient "${name}"? This will also remove their symptoms and medicines.`)) return;
        try { await patientService.remove(id); showToast('Patient deleted'); refetch(); }
        catch (err) { showToast(err.response?.data?.message || 'error'); }
    };

    return (
        <div>
            <div style={ph.header}>
                <div>
                    <h1 style={ph.title}>🧑‍⚕️ Patient Management</h1>
                    <p style={ph.sub}>Track and manage all patient records</p>
                </div>
                <button onClick={handleAdd} style={ph.btn}>+ Add Patient</button>
            </div>

            {toast && <div style={{ ...ph.toast, background: toast.type === 'error' ? '#e74c3c' : '#27ae60' }}>{toast.msg}</div>}
            {error && <div style={ph.errBox}>⚠️ {error}</div>}

            {loading ? <div style={ph.loading}>⏳ Loading patients...</div> : (
                <div style={tbl.wrapper}>
                    <table style={tbl.table}>
                        <thead>
                            <tr style={tbl.thead}>
                                {['#', 'Name', 'Gender', 'Blood Group', 'Age', 'Weight', 'Address', 'Created By', 'Actions'].map(h => (
                                    <th key={h} style={tbl.th}>{h}</th>
                                ))}
                            </tr>
                        </thead>
                        <tbody>
                            {patients.length === 0 ? (
                                <tr><td colSpan={9} style={tbl.empty}>No patients found.</td></tr>
                            ) : patients.map((p, i) => (
                                <tr key={p.patientID} style={i % 2 === 0 ? tbl.trE : tbl.trO}>
                                    <td style={tbl.td}>{i + 1}</td>
                                    <td style={tbl.td}><b>{p.patientName}</b></td>
                                    <td style={tbl.td}>{p.patientGeneder || '—'}</td>
                                    <td style={tbl.td}><span style={tbl.badge}>{p.bloodGroup || '—'}</span></td>
                                    <td style={tbl.td}>{p.age || '—'}</td>
                                    <td style={tbl.td}>{p.weight ? `${p.weight} kg` : '—'}</td>
                                    <td style={tbl.td}>{p.address || '—'}</td>
                                    <td style={tbl.td}>{p.createdByUser}</td>
                                    <td style={tbl.td}>
                                        <button onClick={() => handleEdit(p)} style={tbl.editBtn}>Edit</button>
                                        <button onClick={() => handleDelete(p.patientID, p.patientName)} style={tbl.delBtn}>Delete</button>
                                    </td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                    <p style={tbl.count}>{patients.length} patients total</p>
                </div>
            )}

            {showForm && (
                <div style={fm.backdrop} onClick={() => setShowForm(false)}>
                    <div style={fm.box} onClick={e => e.stopPropagation()}>
                        <div style={fm.hdr}><h3 style={fm.ttl}>{editing ? '✏️ Edit Patient' : '➕ Add Patient'}</h3>
                            <button style={fm.x} onClick={() => setShowForm(false)}>✕</button></div>
                        <div style={fm.body}>
                            {[
                                { l: 'Patient Name *', n: 'patientName', t: 'text' },
                                { l: 'Address', n: 'address', t: 'text' },
                                { l: 'Age', n: 'age', t: 'number' },
                                { l: 'Weight (kg)', n: 'weight', t: 'number' },
                            ].map(f => (
                                <div key={f.n} style={{ marginBottom: '14px' }}>
                                    <label style={fm.lbl}>{f.l}</label>
                                    <input name={f.n} type={f.t} value={form[f.n]} onChange={handleChange}
                                        style={{ ...fm.inp, borderColor: formErrors[f.n] ? '#e74c3c' : '#e2e8f0' }} />
                                    {formErrors[f.n] && <span style={fm.err}>{formErrors[f.n]}</span>}
                                </div>
                            ))}
                            <div style={{ display: 'flex', gap: '12px' }}>
                                <div style={{ flex: 1 }}>
                                    <label style={fm.lbl}>Gender *</label>
                                    <select name="patientGeneder" value={form.patientGeneder} onChange={handleChange}
                                        style={{ ...fm.inp, borderColor: formErrors.patientGeneder ? '#e74c3c' : '#e2e8f0' }}>
                                        <option value="">-- Select --</option>
                                        {GENDER_OPTIONS.map(o => <option key={o.value} value={o.value}>{o.label}</option>)}
                                    </select>
                                    {formErrors.patientGeneder && <span style={fm.err}>{formErrors.patientGeneder}</span>}
                                </div>
                                <div style={{ flex: 1 }}>
                                    <label style={fm.lbl}>Blood Group *</label>
                                    <select name="bloodGroup" value={form.bloodGroup} onChange={handleChange}
                                        style={{ ...fm.inp, borderColor: formErrors.bloodGroup ? '#e74c3c' : '#e2e8f0' }}>
                                        <option value="">-- Select --</option>
                                        {BLOOD_GROUPS.map(b => <option key={b} value={b}>{b}</option>)}
                                    </select>
                                    {formErrors.bloodGroup && <span style={fm.err}>{formErrors.bloodGroup}</span>}
                                </div>
                            </div>
                        </div>
                        <div style={fm.footer}>
                            <button onClick={() => setShowForm(false)} style={fm.cancel}>Cancel</button>
                            <button onClick={handleSubmit} style={fm.save} disabled={saving}>
                                {saving ? 'Saving...' : (editing ? 'Update' : 'Create')}
                            </button>
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
}

const ph = {
    header: { display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start', marginBottom: '24px' },
    title: { margin: 0, fontSize: '24px', fontWeight: 800, color: '#1a3a5c' },
    sub: { margin: '4px 0 0', color: '#64748b', fontSize: '14px' },
    btn: { padding: '10px 20px', background: '#2563eb', color: '#fff', border: 'none', borderRadius: '8px', cursor: 'pointer', fontWeight: 700, fontSize: '14px' },
    toast: { padding: '12px 20px', borderRadius: '8px', color: '#fff', marginBottom: '16px', fontWeight: 600 },
    loading: { textAlign: 'center', padding: '40px', color: '#64748b' },
    errBox: { padding: '12px', background: '#fee2e2', color: '#b91c1c', borderRadius: '8px', marginBottom: '16px' },
};

const tbl = {
    wrapper: { background: '#fff', borderRadius: '12px', overflow: 'hidden', boxShadow: '0 1px 8px rgba(0,0,0,0.08)' },
    table: { width: '100%', borderCollapse: 'collapse' },
    thead: { background: '#1a3a5c' },
    th: { padding: '12px 16px', textAlign: 'left', color: '#e2e8f0', fontWeight: 600, fontSize: '13px' },
    td: { padding: '12px 16px', fontSize: '14px', color: '#374151', borderBottom: '1px solid #f1f5f9' },
    trE: { background: '#fff' }, trO: { background: '#f8fafc' },
    badge: { background: '#dbeafe', color: '#1e40af', padding: '3px 10px', borderRadius: '12px', fontSize: '12px', fontWeight: 600 },
    editBtn: { padding: '5px 12px', background: '#f59e0b', color: '#fff', border: 'none', borderRadius: '5px', cursor: 'pointer', marginRight: '6px', fontWeight: 600 },
    delBtn: { padding: '5px 12px', background: '#ef4444', color: '#fff', border: 'none', borderRadius: '5px', cursor: 'pointer', fontWeight: 600 },
    empty: { padding: '40px', textAlign: 'center', color: '#94a3b8' },
    count: { padding: '12px 16px', margin: 0, color: '#94a3b8', fontSize: '13px' },
};

const fm = {
    backdrop: { position: 'fixed', inset: 0, background: 'rgba(0,0,0,0.5)', display: 'flex', alignItems: 'center', justifyContent: 'center', zIndex: 1000 },
    box: { background: '#fff', borderRadius: '12px', width: '520px', maxWidth: '95vw', maxHeight: '90vh', overflowY: 'auto', boxShadow: '0 20px 60px rgba(0,0,0,0.3)' },
    hdr: { display: 'flex', justifyContent: 'space-between', alignItems: 'center', padding: '20px 24px', borderBottom: '1px solid #e2e8f0' },
    ttl: { margin: 0, fontSize: '18px', fontWeight: 700, color: '#1a3a5c' },
    x: { background: 'none', border: 'none', fontSize: '18px', cursor: 'pointer' },
    body: { padding: '24px' },
    lbl: { display: 'block', fontSize: '12px', fontWeight: 600, color: '#374151', marginBottom: '5px' },
    inp: { width: '100%', padding: '9px 12px', border: '1.5px solid #e2e8f0', borderRadius: '7px', fontSize: '14px', boxSizing: 'border-box', outline: 'none' },
    err: { color: '#e74c3c', fontSize: '11px', display: 'block', marginTop: '3px' },
    footer: { display: 'flex', justifyContent: 'flex-end', gap: '12px', padding: '16px 24px', borderTop: '1px solid #e2e8f0' },
    cancel: { padding: '9px 20px', background: '#f1f5f9', border: 'none', borderRadius: '8px', cursor: 'pointer', fontWeight: 600 },
    save: { padding: '9px 24px', background: '#2563eb', color: '#fff', border: 'none', borderRadius: '8px', cursor: 'pointer', fontWeight: 700 },
};