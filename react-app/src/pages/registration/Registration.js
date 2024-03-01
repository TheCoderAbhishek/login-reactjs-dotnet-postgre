import React, { useState } from 'react';
import { Link } from 'react-router-dom';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faEye, faEyeSlash } from '@fortawesome/free-solid-svg-icons';
import axios from 'axios';

const Registration = () => {
  const [formData, setFormData] = useState({
    username: '',
    email: '',
    passwordHash: '',
    confirmPasswordHash: '',
    fullName: '',
    mobileNumber: '',
    gender: '',
    dateOfBirth: ''
  });
  const [passwordVisible, setPasswordVisible] = useState(false);
  const [confirmPasswordVisible, setConfirmPasswordVisible] = useState(false);
  const [errors, setErrors] = useState({});
  const [successMessage, setSuccessMessage] = useState('');

  const handleChange = (e) => {
    const { name, value } = e.target;
    let error = '';
  
    switch (name) {
      case 'username':
        error = value.trim() ? (value.length < 3 ? 'Username must be at least 3 characters long' : '') : 'Username is required';
        break;
      case 'email':
        error = value.trim() ? (/^\S+@\S+\.\S+$/.test(value) ? '' : 'Invalid email address') : 'Email is required';
        break;
      case 'passwordHash':
        error = value.trim() ? (value.length < 6 ? 'Password must be at least 6 characters long' : '') : 'Password is required';
        
        setErrors({ ...errors, confirmPassword: value !== formData.confirmPasswordHash ? "Passwords don't match" : '' });
        break;
      case 'confirmPasswordHash':
        error = value.trim() ? (value !== formData.passwordHash ? "Passwords don't match" : '') : 'Confirm Password is required';
        break;
      case 'fullName':
        error = value.trim() ? '' : 'Full Name is required';
        break;
      case 'mobileNumber':
        error = value.trim() ? (/^\d{10}$/.test(value) ? '' : 'Invalid mobile number') : 'Mobile Number is required';
        break;
      case 'gender':
        error = value.trim() ? '' : 'Gender is required';
        break;
      case 'dateOfBirth':
        error = value.trim() ? '' : 'Date of Birth is required';
        break;
      default:
        break;
    }
  
    setErrors({ ...errors, [name]: error });
    setFormData({ ...formData, [name]: value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    const validationErrors = validateForm(formData);
    if (Object.keys(validationErrors).length > 0) {
      setErrors(validationErrors);
      return;
    }
    if (formData.passwordHash !== formData.confirmPasswordHash) {
      setErrors({ confirmPassword: "Passwords don't match" });
      return;
    }
    try {
      const response = await axios.post('https://localhost:44354/api/Account/register', formData);
  
      if (response.status === 200 || response.status === 201) {
        setSuccessMessage('Registration successful');
        setErrors({});
        setFormData({
          username: '',
          email: '',
          passwordHash: '',
          confirmPasswordHash: '',
          fullName: '',
          mobileNumber: '',
          gender: '',
          dateOfBirth: ''
        });
        <Link to="/login">Login</Link>
      } else {
        throw new Error('Registration failed');
      }
    } catch (error) {
        console.error('Registration error:', error);
      if (error.response) {
        console.error('Server response:', error.response.data);
      }
      setErrors({ general: 'Registration failed. Please try again later.' });
    }
  };
  
  const togglePasswordVisibility = () => {
    setPasswordVisible(!passwordVisible);
  };

  const toggleConfirmPasswordVisibility = () => {
    setConfirmPasswordVisible(!confirmPasswordVisible);
  };

  const validateForm = (data) => {
    let errors = {};
    if (!data.username.trim()) {
      errors.username = 'Username is required';
    }
    if (!data.email.trim()) {
      errors.email = 'Email is required';
    }
    if (!data.passwordHash.trim()) {
      errors.password = 'Password is required';
    }
    if (!data.confirmPasswordHash.trim()) {
      errors.confirmPassword = 'Confirm Password is required';
    }
    if (!data.fullName.trim()) {
      errors.fullName = 'Full Name is required';
    }
    if (!data.mobileNumber.trim()) {
      errors.mobileNumber = 'Mobile Number is required';
    }
    if (!data.gender.trim()) {
      errors.gender = 'Gender is required';
    }
    if (!data.dateOfBirth.trim()) {
      errors.dateOfBirth = 'Date of Birth is required';
    }
    return errors;
  };

  return (
    <div className="flex flex-col items-center justify-center min-h-screen py-8">
      <h2 className="text-3xl font-semibold mb-8">Registration Page</h2>
      <div className="w-full max-w-xs border border-gray-300 rounded-md p-4">
        {errors.general && <p className="text-red-500 mb-4">{errors.general}</p>}
        {successMessage && <p className="text-green-500 mb-4">{successMessage}</p>}
        <form onSubmit={handleSubmit} className="space-y-4">
          <InputField
            type="text"
            name="username"
            value={formData.username}
            onChange={handleChange}
            placeholder="Username"
            error={errors.username}
          />
          <InputField
            type="email"
            name="email"
            value={formData.email}
            onChange={handleChange}
            placeholder="Email"
            error={errors.email}
          />
          <PasswordInputField
            name="passwordHash"
            value={formData.passwordHash}
            onChange={handleChange}
            placeholder="Password"
            error={errors.password}
            visible={passwordVisible}
            toggleVisibility={togglePasswordVisibility}
          />
          <PasswordInputField
            name="confirmPasswordHash"
            value={formData.confirmPasswordHash}
            onChange={handleChange}
            placeholder="Confirm Password"
            error={errors.confirmPassword}
            visible={confirmPasswordVisible}
            toggleVisibility={toggleConfirmPasswordVisibility}
          />
          <InputField
            type="text"
            name="fullName"
            value={formData.fullName}
            onChange={handleChange}
            placeholder="Full Name"
            error={errors.fullName}
          />
          <InputField
            type="text"
            name="mobileNumber"
            value={formData.mobileNumber}
            onChange={handleChange}
            placeholder="Mobile Number"
            error={errors.mobileNumber}
          />
          <div className="relative mb-8">
            <select
              name="gender"
              value={formData.gender}
              onChange={handleChange}
              className={`w-full px-4 py-2 border ${errors.gender ? 'border-red-500' : 'border-gray-300'} rounded-md focus:outline-none focus:border-indigo-500`}
            >
              <option value="">Select Gender</option>
              <option value="male">Male</option>
              <option value="female">Female</option>
              <option value="other">Other</option>
            </select>
            {errors.gender && (
              <p className="text-red-500 mt-1">{errors.gender}</p>
            )}
          </div>
          <InputField
            type="date"
            name="dateOfBirth"
            value={formData.dateOfBirth}
            onChange={handleChange}
            placeholder="Date of Birth"
            error={errors.dateOfBirth}
          />
          <button
            type="submit"
            className="w-full bg-indigo-500 text-white py-2 rounded-md hover:bg-indigo-600 focus:outline-none focus:bg-indigo-600"
          >
            Register
          </button>
        </form>
        <p className="text-gray-600 mt-2">
          Already registered? <Link to="/login" className="text-indigo-600">Log in here</Link>
        </p>
      </div>
    </div>
  );
};

const InputField = ({ type, name, value, onChange, placeholder, error }) => (
  <div className="relative mb-4">
    <input
      type={type}
      name={name}
      value={value}
      onChange={onChange}
      placeholder={placeholder}
      className={`w-full px-4 py-2 border ${error ? 'border-red-500' : 'border-gray-300'} rounded-md focus:outline-none focus:border-indigo-500`}
    />
    {error && <p className="text-red-500 mt-1">{error}</p>}
  </div>
);

const PasswordInputField = ({ name, value, onChange, placeholder, error, visible, toggleVisibility }) => (
  <div className="relative mb-4">
    <div className="relative">
      <input
        type={visible ? 'text' : 'password'}
        name={name}
        value={value}
        onChange={onChange}
        placeholder={placeholder}
        className={`w-full px-4 py-2 pr-10 border ${error ? 'border-red-500' : 'border-gray-300'} rounded-md focus:outline-none focus:border-indigo-500`}
      />
      <span
        className="absolute top-1/2 right-3 transform -translate-y-1/2 cursor-pointer"
        onClick={toggleVisibility}
      >
        <FontAwesomeIcon icon={visible ? faEyeSlash : faEye} />
      </span>
    </div>
    {error && <p className="text-red-500 mt-1">{error}</p>}
  </div>
);

export default Registration;
