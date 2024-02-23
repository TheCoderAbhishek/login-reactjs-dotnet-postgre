import React, { useState } from 'react';
import { Link } from 'react-router-dom';
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

  // eslint-disable-next-line
  const [error, setError] = useState('');

  const handleChange = (e) => {
    if (e.target.type === 'date') {
      const date = new Date(e.target.value);
      const utcDate = new Date(Date.UTC(date.getFullYear(), date.getMonth(), date.getDate()));
      setFormData({ ...formData, [e.target.name]: utcDate.toISOString().split('T')[0] });
    } else {
      setFormData({ ...formData, [e.target.name]: e.target.value });
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (formData.passwordHash !== formData.confirmPasswordHash) {
      setError("Passwords don't match");
      return;
    }
    try {
      const response = await axios.post('https://localhost:44354/api/Account/create', formData);

      if (response.status === 200 || response.status === 201) {
        console.log('Registration successful');
      } else {
        throw new Error('Registration failed');
      }
    } catch (error) {
      console.error('Registration error:', error);
      setError('Registration failed. Please try again later.');
    }
  };

  return (
    <div className="flex flex-col items-center justify-center h-screen">
      <h2 className="text-3xl font-semibold mb-8">Registration Page</h2>
      <form onSubmit={handleSubmit} className="flex flex-col space-y-4">
        <input
          type="text"
          name="username"
          value={formData.username}
          onChange={handleChange}
          placeholder="Username"
          className="w-64 px-4 py-2 border border-gray-300 rounded-md focus:outline-none focus:border-indigo-500"
        />
        <input
          type="email"
          name="email"
          value={formData.email}
          onChange={handleChange}
          placeholder="Email"
          className="w-64 px-4 py-2 border border-gray-300 rounded-md focus:outline-none focus:border-indigo-500"
        />
        <input
          type="password"
          name="passwordHash"
          value={formData.passwordHash}
          onChange={handleChange}
          placeholder="Password"
          className="w-64 px-4 py-2 border border-gray-300 rounded-md focus:outline-none focus:border-indigo-500"
        />
        <input
          type="password"
          name="confirmPasswordHash"
          value={formData.confirmPasswordHash}
          onChange={handleChange}
          placeholder="Confirm Password"
          className="w-64 px-4 py-2 border border-gray-300 rounded-md focus:outline-none focus:border-indigo-500"
        />
        <input
          type="text"
          name="fullName"
          value={formData.fullName}
          onChange={handleChange}
          placeholder="Full Name"
          className="w-64 px-4 py-2 border border-gray-300 rounded-md focus:outline-none focus:border-indigo-500"
        />
        <input
          type="text"
          name="mobileNumber"
          value={formData.mobileNumber}
          onChange={handleChange}
          placeholder="Mobile Number"
          className="w-64 px-4 py-2 border border-gray-300 rounded-md focus:outline-none focus:border-indigo-500"
        />
        <select
          name="gender"
          value={formData.gender}
          onChange={handleChange}
          className="w-64 px-4 py-2 border border-gray-300 rounded-md focus:outline-none focus:border-indigo-500"
        >
          <option value="">Select Gender</option>
          <option value="male">Male</option>
          <option value="female">Female</option>
          <option value="other">Other</option>
        </select>
        <input
          type="date"
          name="dateOfBirth"
          value={formData.dateOfBirth}
          onChange={handleChange}
          placeholder="Date of Birth"
          className="w-64 px-4 py-2 border border-gray-300 rounded-md focus:outline-none focus:border-indigo-500"
        />
        <button
          type="submit"
          className="w-64 bg-indigo-500 text-white py-2 rounded-md hover:bg-indigo-600 focus:outline-none focus:bg-indigo-600"
        >
          Register
        </button>
      </form>
      <p className="text-gray-600 mt-2">
        Already registered? <Link to="/login" className="text-indigo-600">Log in here</Link>
      </p>
    </div>
  );
}

export default Registration;
