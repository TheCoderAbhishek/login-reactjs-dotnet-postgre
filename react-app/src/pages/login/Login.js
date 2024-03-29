import React, { useState, useEffect } from "react";
import { Link, useLocation, useNavigate } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faEye, faEyeSlash, faTimes } from "@fortawesome/free-solid-svg-icons";
import axios from "axios";
import ReCAPTCHA from "react-google-recaptcha";

const Login = () => {
  const [formData, setFormData] = useState({
    usernameOrEmail: "",
    passwordHash: "",
  });
  const [passwordVisible, setPasswordVisible] = useState(false);
  const [errors, setErrors] = useState({});
  const [successMessage, setSuccessMessage] = useState("");
  const [capVal, setCapVal] = useState("");
  const location = useLocation();
  const navigate = useNavigate();

  useEffect(() => {
    const searchParams = new URLSearchParams(location.search);
    const successMessage = searchParams.get("successMessage");
    if (successMessage) {
      setSuccessMessage(decodeURIComponent(successMessage));
    }
  }, [location]);

  const clearSuccessMessage = () => {
    setSuccessMessage("");
  };

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData({ ...formData, [name]: value });
    setErrors({ ...errors, [name]: "" });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    const validationErrors = {};
    if (!formData.usernameOrEmail.trim()) {
      validationErrors.usernameOrEmail = "Username or Email is required";
    }
    if (!formData.passwordHash.trim()) {
      validationErrors.passwordHash = "Password is required";
    }
    if (Object.keys(validationErrors).length > 0) {
      setErrors(validationErrors);
      return;
    }

    try {
      const response = await axios.post(
        "https://localhost:44354/api/Account/login",
        formData
      );

      if (response.status === 200) {
        setSuccessMessage("Login successful");
        setErrors({});
        setFormData({
          usernameOrEmail: "",
          passwordHash: "",
        });
        navigate("/dashboard");
      } else {
        setErrors({ general: "Invalid username/email or password." });
      }
    } catch (error) {
      console.error("Login error:", error);
      if (error.response) {
        console.error("Server response:", error.response.data);
      }
      setErrors({ general: "Login failed. Please try again later." });
    }
  };

  const togglePasswordVisibility = () => {
    setPasswordVisible(!passwordVisible);
  };

  return (
    <div className="flex flex-col items-center justify-center min-h-screen py-8">
      <h2 className="text-3xl font-semibold mb-8">Login Page</h2>
      <div className="w-full max-w-xs border border-gray-300 rounded-md p-2">
        {errors.general && (
          <p className="text-red-500 mb-2">{errors.general}</p>
        )}
        {successMessage && (
          <div className="flex justify-between items-center mb-4">
            <p className="text-green-500">{successMessage}</p>
            <button
              onClick={clearSuccessMessage}
              className="text-gray-500"
            >
              <FontAwesomeIcon icon={faTimes} />
            </button>
          </div>
        )}
        <form onSubmit={handleSubmit} className="space-y-4">
          <InputField
            type="text"
            name="usernameOrEmail"
            value={formData.usernameOrEmail}
            onChange={handleChange}
            placeholder="Username or Email"
            error={errors.usernameOrEmail}
          />
          <PasswordInputField
            name="passwordHash"
            value={formData.passwordHash}
            onChange={handleChange}
            placeholder="Password"
            error={errors.passwordHash}
            visible={passwordVisible}
            toggleVisibility={togglePasswordVisibility}
          />
          <ReCAPTCHA
            sitekey="6LfV0pIpAAAAAAEpWOP7zf2ytk7d1J7xHLaRiPDH"
            onChange={(val) => setCapVal(val)}
          />
          <button
            disabled={!capVal}
            type="submit"
            className="w-full bg-indigo-500 text-white py-2 rounded-md hover:bg-indigo-600 focus:outline-none focus:bg-indigo-600"
          >
            Log in
          </button>
        </form>
        <p className="text-gray-600 mt-2">
          Not registered?{" "}
          <Link to="/registration" className="text-indigo-600">
            Register here
          </Link>
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
      className={`w-full px-4 py-2 border ${
        error ? "border-red-500" : "border-gray-300"
      } rounded-md focus:outline-none focus:border-indigo-500`}
    />
    {error && <p className="text-red-500 mt-1">{error}</p>}
  </div>
);

const PasswordInputField = ({
  name,
  value,
  onChange,
  placeholder,
  error,
  visible,
  toggleVisibility,
}) => (
  <div className="relative mb-4">
    <div className="relative">
      <input
        type={visible ? "text" : "password"}
        name={name}
        value={value}
        onChange={onChange}
        placeholder={placeholder}
        className={`w-full px-4 py-2 pr-10 border ${
          error ? "border-red-500" : "border-gray-300"
        } rounded-md focus:outline-none focus:border-indigo-500`}
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

export default Login;
