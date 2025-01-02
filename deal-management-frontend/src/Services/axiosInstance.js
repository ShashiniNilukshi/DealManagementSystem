// src/Services/axiosInstance.js
import axios from 'axios';
import config from '../config'; // Ensure this points to the correct config file

// Axios instance to automatically include the token in the request header
const axiosInstance = axios.create({
  baseURL: `${config.apiUrl}/`, // Set the API base URL
});

// Interceptor to add the token to the headers of each request
axiosInstance.interceptors.request.use((config) => {
  const user = JSON.parse(localStorage.getItem('user'));
  if (user && user.token) {
    config.headers['Authorization'] = `Bearer ${user.token}`;
  }
  return config;
}, (error) => {
  return Promise.reject(error);
});

export default axiosInstance;
