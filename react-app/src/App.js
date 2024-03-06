import React from 'react';
import './App.css';
import Header from './components/shared/Header.js';
import Footer from './components/shared/Footer.js';
import Home from './pages/home/Home.js';
import Login from './pages/login/Login.js';
import Registration from './pages/registration/Registration.js';
import OtpValidation from './pages/registration/OtpValidation.js';
import Dashboard from './pages/dashboard/Dashboard.js';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';

function App() {
  return (
    <Router>
      <div className="App">
        <Header />

        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/login" element={<Login />} />
          <Route path="/registration" element={<Registration />} />
          <Route path="/otp-validation" element={<OtpValidation />} />
          <Route path="/dashboard" element={<Dashboard />} />
        </Routes>
        
        <Footer />
      </div>
    </Router>
  );
}

export default App;
