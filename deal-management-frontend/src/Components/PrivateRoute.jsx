// src/Components/PrivateRoute.js
import React from 'react';
import { Navigate, Outlet } from 'react-router-dom';
import { authService } from '../Services/authService';

const PrivateRoute = () => {
  const isAuthenticated = authService.isLoggedIn();

  return isAuthenticated ? <Outlet /> : <Navigate to="/login" replace />;
};

export default PrivateRoute;
