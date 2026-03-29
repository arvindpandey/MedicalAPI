src/
├── api/                    # API service layer
│   ├── axiosInstance.js
│   ├── authService.js
│   ├── patientService.js
│   ├── symptomService.js
│   ├── medicineService.js
│   └── userService.js
├── components/
│   ├── common/             # Reusable UI components
│   │   ├── Navbar.jsx
│   │   ├── Sidebar.jsx
│   │   ├── DataTable.jsx
│   │   ├── Modal.jsx
│   │   └── LoadingSpinner.jsx
│   ├── auth/
│   │   └── LoginForm.jsx
│   ├── patients/
│   │   ├── PatientList.jsx
│   │   ├── PatientForm.jsx
│   │   └── PatientCard.jsx
│   ├── symptoms/
│   │   ├── SymptomList.jsx
│   │   └── SymptomForm.jsx
│   ├── medicines/
│   │   ├── MedicineList.jsx
│   │   └── MedicineForm.jsx
│   └── users/
│       ├── UserList.jsx
│       └── UserForm.jsx
├── pages/
│   ├── Dashboard.jsx
│   ├── Patients.jsx
│   ├── Symptoms.jsx
│   ├── Medicines.jsx
│   └── Users.jsx
├── context/
│   └── AuthContext.jsx
├── hooks/
│   ├── useAuth
│   └── useFetch.js
└── utils/
    └── helpers.js