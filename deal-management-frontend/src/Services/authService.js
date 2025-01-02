// src/Services/authService.js
import axios from 'axios';
import config from '../config';

const API_URL = `${config.apiUrl}/auth`;

const axiosInstance = axios.create({
  baseURL: `${config.apiUrl}/`,
});

axiosInstance.interceptors.request.use((config) => {
  const user = JSON.parse(localStorage.getItem('user'));
  if (user && user.token) {
    config.headers['Authorization'] = `Bearer ${user.token}`;
  }
  return config;
});

export const authService = {
  async login(credentials) {
    try {
      const response = await axios.post(`${API_URL}/login`, credentials);
      if (response.data.token) {
        localStorage.setItem('user', JSON.stringify(response.data));
      }
      return response.data;
    } catch (error) {
      console.error('Login error:', error.response || error);
      throw error;
    }
  },

  async register(userData) {
    try {
      const response = await axios.post(`${API_URL}/register`, userData);
      return response.data;
    } catch (error) {
      console.error('Registration error:', error.response || error);
      throw error;
    }
  },

  logout() {
    localStorage.removeItem('user');
  },

  getCurrentUser() {
    return JSON.parse(localStorage.getItem('user'));
  },

  isLoggedIn() {
    const user = this.getCurrentUser();
    return user && user.token;
  },

  async refreshToken() {
    const user = this.getCurrentUser();
    if (user && user.token) {
      try {
        const response = await axios.post(`${API_URL}/refresh-token`, { token: user.token });
        if (response.data.token) {
          user.token = response.data.token;
          localStorage.setItem('user', JSON.stringify(user));
        }
        return response.data;
      } catch (error) {
        console.error('Token refresh error:', error.response || error);
        throw error;
      }
    }
    return null;
  },

  axiosInstance,
};
