import React from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { LogOut, User } from 'lucide-react';
import { Button, AppBar, Toolbar, Typography, Box, Menu, MenuItem, Avatar } from '@mui/material';
import { authService } from '../Services/authService';

export const Navbar = () => {
  const navigate = useNavigate();
  const user = authService.getCurrentUser();
  const [anchorEl, setAnchorEl] = React.useState(null);

  const handleMenuClick = (event) => {
    setAnchorEl(event.currentTarget);
  };

  const handleMenuClose = () => {
    setAnchorEl(null);
  };

  const handleLogout = () => {
    authService.logout();
    navigate('/login');
  };

  return (
    <AppBar position="sticky" sx={{ bgcolor: 'primary.main' }}>
      <Toolbar sx={{ justifyContent: 'space-between' }}>
        {/* Logo */}
        <Typography
          variant="h6"
          component={Link}
          to="/"
          sx={{
            fontWeight: 'bold',
            fontSize: '1.5rem',
            color: 'black',
            textDecoration: 'none',
            '&:hover': { color: 'secondary.main' },
          }}
        >
          Dealuxe
        </Typography>

        {/* Navigation Links */}
        <Box sx={{ display: 'flex', gap: 4 }}>
          <Button
            component={Link}
            to="/"
            sx={{
              color: 'white',
              '&:hover': { color: 'secondary.main' },
            }}
          >
            Home
          </Button>
          <Button
            component={Link}
            to="/about"
            sx={{
              color: 'white',
              '&:hover': { color: 'secondary.main' },
            }}
          >
            About
          </Button>
          <Button
            component={Link}
            to="/contact"
            sx={{
              color: 'white',
              '&:hover': { color: 'secondary.main' },
            }}
          >
            Contact
          </Button>
        </Box>

        {/* User Section */}
        <Box sx={{ display: 'flex', alignItems: 'center' }}>
          {user ? (
            <>
              <Button
                sx={{
                  color: 'white',
                  display: 'flex',
                  alignItems: 'center',
                  gap: 1,
                  '&:hover': { color: 'secondary.main' },
                }}
                onClick={handleMenuClick}
              >
                <Avatar sx={{ width: 32, height: 32 }} />
                <Typography variant="body1">{user.username}</Typography>
              </Button>
              <Menu
                anchorEl={anchorEl}
                open={Boolean(anchorEl)}
                onClose={handleMenuClose}
                sx={{ mt: 1 }}
              >
                <MenuItem onClick={handleLogout}>
                  <LogOut size={18} style={{ marginRight: 8 }} />
                  Logout
                </MenuItem>
              </Menu>
            </>
          ) : (
            <Button
              component={Link}
              to="/login"
              sx={{
                bgcolor: 'white',
                color: 'primary.main',
                '&:hover': { bgcolor: 'secondary.main', color: 'white' },
              }}
            >
              Login
            </Button>
          )}
        </Box>
      </Toolbar>
    </AppBar>
  );
};

export default Navbar;
