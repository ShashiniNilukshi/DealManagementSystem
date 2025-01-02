import React from 'react';
import { Button, Typography, Box, Paper, Container } from '@mui/material';
import { useNavigate } from 'react-router-dom';



const MainPage = () => {
  const navigate = useNavigate();

  const handleLogin = () => {
    navigate('/login');
  };

  const handleRegister = () => {
    navigate('/register');
  };

  return (
    <Box
      sx={{
        height: '100vh',
        backgroundSize: 'cover',
        backgroundPosition: 'center',
        display: 'flex',
        justifyContent: 'center',
        alignItems: 'center',
        backdropFilter: 'blur(5px)', // Adds a blur effect to the background
      }}
    >
      <Container maxWidth="sm">
        <Paper
          sx={{
            padding: 4,
            backgroundColor: 'rgba(255, 255, 255, 0.9)', // Slightly lighter background to improve readability
            borderRadius: 4,
            boxShadow: 10,
            textAlign: 'center',
          }}
        >
          <Typography variant="h3" sx={{ fontWeight: 'bold', mb: 2 }} color="primary">
            Hospitality Deals
          </Typography>
          <Typography variant="h6" paragraph sx={{ mb: 4 }}>
            Discover exclusive deals and offers tailored just for you. Join us today to start saving!
          </Typography>
          <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
            <Button
              variant="contained"
              color="primary"
              size="large"
              onClick={handleLogin}
              sx={{
                borderRadius: '30px',
                padding: '12px 24px',
                boxShadow: 3,
                '&:hover': { backgroundColor: 'primary.dark' },
              }}
            >
              Login
            </Button>
            <Button
              variant="outlined"
              color="primary"
              size="large"
              onClick={handleRegister}
              sx={{
                borderRadius: '30px',
                padding: '12px 24px',
                boxShadow: 3,
                '&:hover': { backgroundColor: 'primary.main', color: 'white' },
              }}
            >
              Register
            </Button>
          </Box>
        </Paper>
      </Container>
    </Box>
  );
};

export default MainPage;
