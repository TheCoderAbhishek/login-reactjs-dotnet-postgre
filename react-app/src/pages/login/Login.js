import React from 'react';
import { Link } from 'react-router-dom';

const Login = () => {
  return (
    <div className="flex flex-col items-center justify-center h-screen">
      <h2 className="text-3xl font-semibold mb-8">Login Page</h2>
      <div className="flex flex-col space-y-4">
        <input
          type="text"
          placeholder="Username"
          className="w-64 px-4 py-2 border border-gray-300 rounded-md focus:outline-none focus:border-indigo-500"
        />
        <input
          type="password"
          placeholder="Password"
          className="w-64 px-4 py-2 border border-gray-300 rounded-md focus:outline-none focus:border-indigo-500"
        />
        <button
          className="w-64 bg-indigo-500 text-white py-2 rounded-md hover:bg-indigo-600 focus:outline-none focus:bg-indigo-600"
        >
          Log in
        </button>
        <p className="text-gray-600 mt-2">
          Not registered? <Link to="/registration" className="text-indigo-600">Register here</Link>
        </p>
      </div>
    </div>
  );
}

export default Login;