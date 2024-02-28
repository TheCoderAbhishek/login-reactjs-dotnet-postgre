import React, { useState } from 'react';
import { Link } from 'react-router-dom';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faEye, faEyeSlash } from '@fortawesome/free-solid-svg-icons';

const Login = () => {
  const [formData, setFormData] = useState({
    username: '',
    password: ''
  });
  const [passwordVisible, setPasswordVisible] = useState(false);
  const [errors, setErrors] = useState({});

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData({ ...formData, [name]: value });
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    const validationErrors = validateForm(formData);
    if (Object.keys(validationErrors).length > 0) {
      setErrors(validationErrors);
      return;
    }
    // Add your submission logic here
  };

  const togglePasswordVisibility = () => {
    setPasswordVisible(!passwordVisible);
  };

  const validateForm = (data) => {
    let errors = {};
    if (!data.username.trim()) {
      errors.username = 'Username is required';
    }
    if (!data.password.trim()) {
      errors.password = 'Password is required';
    }
    return errors;
  };

  return (
    <div className="flex flex-col items-center justify-center min-h-screen py-8">
      <h2 className="text-3xl font-semibold mb-8">Login Page</h2>
      <div className="w-full max-w-xs border border-gray-300 rounded-md p-4">
        <form onSubmit={handleSubmit} className="space-y-4">
          <InputField
            type="text"
            name="username"
            value={formData.username}
            onChange={handleChange}
            placeholder="Username"
            error={errors.username}
          />
          <div className="relative mb-4">
            <input
              type={passwordVisible ? 'text' : 'password'}
              name="password"
              value={formData.password}
              onChange={handleChange}
              placeholder="Password"
              className={`w-full px-4 py-2 border ${errors.password ? 'border-red-500' : 'border-gray-300'} rounded-md focus:outline-none focus:border-indigo-500`}
            />
            <span
              className="absolute top-1/2 right-3 transform -translate-y-1/2 cursor-pointer"
              onClick={togglePasswordVisibility}
            >
              <FontAwesomeIcon icon={passwordVisible ? faEyeSlash : faEye} />
            </span>
          </div>
          {errors.password && <p className="text-red-500 mt-1">{errors.password}</p>}
          <button
            type="submit"
            className="w-full bg-indigo-500 text-white py-2 rounded-md hover:bg-indigo-600 focus:outline-none focus:bg-indigo-600"
          >
            Log in
          </button>
        </form>
        <p className="text-gray-600 mt-2">
          Not registered? <Link to="/registration" className="text-indigo-600">Register here</Link>
        </p>
      </div>
    </div>
  );
}

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

export default Login;
