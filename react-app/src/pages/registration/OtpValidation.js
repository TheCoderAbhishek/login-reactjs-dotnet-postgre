import React, { useState, useEffect } from "react";
import { Link } from "react-router-dom";
import axios from "axios";
import { useLocation, useNavigate } from "react-router-dom";

const OtpValidation = () => {
  const location = useLocation();
  const navigate = useNavigate();

  const [formData, setFormData] = useState({
    username: location.state.username || "",
    otp: "",
  });

  const [errors, setErrors] = useState({});
  const [successMessage, setSuccessMessage] = useState("");
  const [timer, setTimer] = useState(60);
  const [showResendButton, setShowResendButton] = useState(false);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData({ ...formData, [name]: value });
  };

  useEffect(() => {
    const countdown = setInterval(() => {
      if (timer === 0) {
        clearInterval(countdown);
        setShowResendButton(true);
      } else {
        setTimer((prevTimer) => prevTimer - 1);
      }
    }, 1000);

    return () => clearInterval(countdown);
  }, [timer]);

  const handleSubmit = async (e) => {
    e.preventDefault();
    const validationErrors = validateForm(formData);
    if (Object.keys(validationErrors).length > 0) {
      setErrors(validationErrors);
      return;
    }

    try {
      const response = await axios.post(
        "https://localhost:44354/api/Account/verify-otp",
        {
          username: formData.username,
          otp: formData.otp,
          user: location.state.userData,
        }
      );

      if (response.status === 200) {
        setSuccessMessage("OTP validated successfully");
        setErrors({});
        navigate("/login");
      } else {
        throw new Error("OTP validation failed");
      }
    } catch (error) {
      console.error("OTP validation error:", error);
      setErrors({ general: "OTP validation failed. Please try again later." });
    }
  };

  const handleResendOTP = async () => {
    try {
      const response = await axios.post(
        "https://localhost:44354/api/Account/resend-otp",
        {
          username: formData.username,
          user: location.state.userData,
        }
      );

      if (response.status === 200) {
        setSuccessMessage("New OTP sent successfully");
        setShowResendButton(false);
        setTimer(60);
      } else {
        throw new Error("Failed to resend OTP");
      }
    } catch (error) {
      console.error("Resend OTP error:", error);
      setErrors({ general: "Failed to resend OTP. Please try again later." });
    }
  };

  const validateForm = (data) => {
    let errors = {};
    if (!data.otp.trim()) {
      errors.otp = "OTP is required";
    }
    return errors;
  };

  return (
    <div className="flex flex-col items-center justify-center min-h-screen py-8">
      <h2 className="text-3xl font-semibold mb-8">OTP Validation Page</h2>
      <div className="w-full max-w-xs border border-gray-300 rounded-md p-4">
        {errors.general && (
          <p className="text-red-500 mb-4">{errors.general}</p>
        )}
        {successMessage && (
          <p className="text-green-500 mb-4">{successMessage}</p>
        )}
        <form onSubmit={handleSubmit} className="space-y-4">
          <div className="mb-4">
            <label htmlFor="username" className="block mb-1">
              Username
            </label>
            <input
              type="text"
              id="username"
              name="username"
              value={formData.username}
              readOnly
              className="w-full px-4 py-2 border border-gray-300 rounded-md focus:outline-none focus:border-indigo-500"
              disabled
            />
          </div>
          <div className="relative mb-4">
            <label htmlFor="otp" className="block mb-1">
              OTP
            </label>
            <input
              type="text"
              id="otp"
              name="otp"
              value={formData.otp}
              onChange={handleChange}
              placeholder="Enter OTP"
              className={`w-full px-4 py-2 border ${
                errors.otp ? "border-red-500" : "border-gray-300"
              } rounded-md focus:outline-none focus:border-indigo-500`}
            />
            {errors.otp && <p className="text-red-500 mt-1">{errors.otp}</p>}
          </div>
          <button
            type="submit"
            className="w-full bg-indigo-500 text-white py-2 rounded-md hover:bg-indigo-600 focus:outline-none focus:bg-indigo-600"
          >
            Validate OTP
          </button>
          {showResendButton && (
            <button
              onClick={handleResendOTP}
              className="w-full bg-indigo-500 text-white py-2 rounded-md hover:bg-indigo-600 focus:outline-none focus:bg-indigo-600"
            >
              Resend OTP
            </button>
          )}
          {timer > 0 && (
            <p className="text-gray-600 mt-2">Resend OTP in {timer} seconds</p>
          )}
        </form>
        <p className="text-gray-600 mt-2">
          Not your account?{" "}
          <Link to="/registration" className="text-indigo-600">
            Register here
          </Link>
        </p>
      </div>
    </div>
  );
};

export default OtpValidation;
