import React from 'react';
import { Box, Typography, Container, Paper, Button } from '@mui/material';
import { useNavigate } from 'react-router-dom';

const AboutPage = () => {
  const navigate = useNavigate();

  const handleRedirect = () => {
    navigate('/deals');
  };

  return (
    <Box
      sx={{
        height: '100vh',
        display: 'flex',
        justifyContent: 'center',
        alignItems: 'center',
        backgroundColor: '#f5f5f5',
        padding: 2,
      }}
    >
      <Container maxWidth="sm">
        <Paper
          sx={{
            padding: 4,
            backgroundColor: '#ffffff',
            borderRadius: 3,
            boxShadow: 3,
            textAlign: 'center',
          }}
        >
          <Typography variant="h3" sx={{ fontWeight: 'bold' }} gutterBottom>
            About DealHub
          </Typography>
          <Typography variant="h6" paragraph sx={{ color: '#5f6368' }}>
            Dealux is a powerful platform designed to help you manage and discover exclusive deals
            in various categories. Whether you’re looking for product discounts, service offers, or
            special experiences, we make it easy to find the best deals tailored just for you.
          </Typography>
          <Typography variant="body1" paragraph sx={{ color: '#5f6368' }}>
            Our mission is to provide businesses and individuals with a simple, user-friendly
            platform for deal management. From deal creation to discovery, we ensure the best
            offers are just a click away.
          </Typography>
          <Button
            variant="contained"
            color="primary"
            size="large"
            onClick={handleRedirect}
            sx={{
              marginTop: 2,
              padding: '12px 24px',
              fontSize: '16px',
              borderRadius: 5,
            }}
          >
            Browse Deals
          </Button>
        </Paper>
      </Container>
    </Box>
  );
};

export default AboutPage;
