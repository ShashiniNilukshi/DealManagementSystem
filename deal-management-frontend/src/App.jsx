import React, { useEffect } from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import MainPage from './Components/MainPage';
import LoginForm from './Components/LoginForm';
import RegisterForm from './Components/RegisterForm';
import DealList from './Components/DealList';
import CreateDeal from './Components/CreateDeal';
import EditDealPage from './Components/EditDeal';
import { authService } from './Services/authService'; 
import AboutPage from './Components/About';
import ContactPage from './Components/Contact';

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<MainPage />} />
        <Route path="/login" element={<LoginForm />} />
        <Route path="/register" element={<RegisterForm />} />
        <Route path="/deals" element={<DealList />} />
        <Route path="/create/deals" element={<CreateDeal />} />
        <Route path="/deals/:dealId/edit" element={<EditDealPage />} />
        <Route path="/about" element={<AboutPage />} />
        <Route path ="/contact" element ={<ContactPage/>}/>
      </Routes>
    </Router>
  );
}

export default App;
