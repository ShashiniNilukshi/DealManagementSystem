import React, { useState } from 'react';
import { Box, Typography, Container, Paper, TextField, Button, Grid } from '@mui/material';

const ContactPage = () => {
  const [name, setName] = useState('');
  const [email, setEmail] = useState('');
  const [message, setMessage] = useState('');

  const handleSubmit = (e) => {
    e.preventDefault();
    // Handle form submission logic here
    console.log('Form Submitted', { name, email, message });
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
            Contact Us
          </Typography>
          <Typography variant="h6" paragraph sx={{ color: '#5f6368' }}>
            We would love to hear from you! Please fill out the form below, and we’ll get back to
            you as soon as possible.
          </Typography>

          <form onSubmit={handleSubmit}>
            <Grid container spacing={2} direction="column">
              <Grid item>
                <TextField
                  label="Name"
                  variant="outlined"
                  fullWidth
                  value={name}
                  onChange={(e) => setName(e.target.value)}
                  required
                />
              </Grid>
              <Grid item>
                <TextField
                  label="Email"
                  variant="outlined"
                  fullWidth
                  value={email}
                  onChange={(e) => setEmail(e.target.value)}
                  required
                  type="email"
                />
              </Grid>
              <Grid item>
                <TextField
                  label="Message"
                  variant="outlined"
                  fullWidth
                  multiline
                  rows={4}
                  value={message}
                  onChange={(e) => setMessage(e.target.value)}
                  required
                />
              </Grid>
              <Grid item>
                <Button
                  variant="contained"
                  color="primary"
                  size="large"
                  type="submit"
                  sx={{
                    marginTop: 2,
                    padding: '12px 24px',
                    fontSize: '16px',
                    borderRadius: 5,
                  }}
                >
                  Send Message
                </Button>
              </Grid>
            </Grid>
          </form>
        </Paper>
      </Container>
    </Box>
  );
};

export default ContactPage;
