import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { Card, CardContent, CardHeader, Typography, Button, IconButton, Grid, Pagination } from '@mui/material';
import { Plus, Edit, Trash } from 'lucide-react';
import { dealService } from '../Services/dealService';
import { Navbar } from './Navbar';
import { authService } from '../Services/authService';

const DealList = () => {
  const [deals, setDeals] = useState([]);
  const [loading, setLoading] = useState(false);
  const [currentPage, setCurrentPage] = useState(1);  
  const [itemsPerPage] = useState(9);  
  const navigate = useNavigate();
  const user = authService.getCurrentUser();
  const isAdminOrSuperAdmin = user && (user.role === 'Admin' || user.role === 'SuperAdmin');
  const isSuperAdmin = user && user.role === 'SuperAdmin';

  useEffect(() => {
    const fetchData = async () => {
      setLoading(true);
      try {
        const dealsData = await dealService.getAllDeals();
        setDeals(dealsData);
      } catch (error) {
        console.error('Error fetching deals:', error);
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, []);

  // Handle pagination change
  const handlePageChange = (event, value) => {
    setCurrentPage(value);
  };

  // Calculate index range for the current page
  const indexOfLastDeal = currentPage * itemsPerPage;
  const indexOfFirstDeal = indexOfLastDeal - itemsPerPage;
  const currentDeals = deals.slice(indexOfFirstDeal, indexOfLastDeal);

  const handleDelete = async (id) => {
    try {
      await dealService.deleteDeal(id);
      setDeals(deals.filter(deal => deal.id !== id));  
    } catch (error) {
      console.error('Error deleting deal:', error);
    }
  };

  const handleAddDeal = () => {
    navigate('/create/deals');
  };

  if (loading) {
    return <div className="text-center p-4">Loading...</div>;
  }

  return (
    <>
      <div><Navbar /></div>
      <div style={{ padding: '30px', backgroundColor: '#f4f7fa' }}>
        <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: '30px' }}>
          <Typography variant="h4" color="primary" style={{ fontWeight: 'bold' }}>Hospitality Deals</Typography>

          {/* Conditional Rendering of the 'New Deal' button */}
          {isAdminOrSuperAdmin && (
            <Button
              onClick={handleAddDeal}
              variant="contained"
              color="primary"
              startIcon={<Plus />}
              sx={{ borderRadius: '12px' }}
            >
              New Deal
            </Button>
          )}
        </div>

        <Grid container spacing={4} style={{ marginTop: '30px' }}>
          {currentDeals.map(deal => (
            <Grid item xs={12} sm={6} md={4} key={deal.id}>
              <Card sx={{
                boxShadow: 2,
                borderRadius: '8px',
                overflow: 'hidden',
                padding: '10px',
                display: 'flex',
                flexDirection: 'column',
                height: '100%',  
                backgroundColor: '#fff',
                position: 'relative',  
              }}>
                {/* Deal Name Header */}
                <CardHeader
                  title={deal.name}
                  subheader={new Date(deal.createdAt).toLocaleDateString('en-US', { year: 'numeric', month: 'long', day: 'numeric' })}  // Format Date properly
                  sx={{
                    backgroundColor: '#3f51b5',
                    color: 'white',
                    textAlign: 'center',
                    padding: '12px',
                    borderBottom: '2px solid #fff',
                    flex: '0 1 auto',  
                  }} />

                <CardContent sx={{
                  backgroundColor: '#ffffff',
                  padding: '15px',
                  flex: '1 1 auto',  
                }}>
                  {/* Deal Image Section */}
                  {deal.image && (
                    <div style={{
                      marginBottom: '20px',
                      height: '200px', 
                      overflow: 'hidden', 
                    }}>
                      <img
                        src={deal.image} 
                        alt={deal.name}
                        style={{
                          width: '100%',
                          height: '100%',
                          objectFit: 'cover',  
                        }}
                      />
                    </div>
                  )}

                  {/* Hotels Section */}
                  <div style={{ marginBottom: '20px' }}>
                    <Typography variant="subtitle1" color="textPrimary" fontWeight="bold" gutterBottom>Hotels</Typography>
                    <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '15px' }}>
                      {deal.hotels.map(hotel => (
                        <div key={hotel.id} style={{
                          backgroundColor: '#f3f4f6',
                          padding: '12px',
                          borderRadius: '8px',
                          boxShadow: '0 2px 4px rgba(0, 0, 0, 0.1)',
                        }}>
                          <Typography variant="body1" color="textPrimary" fontWeight="bold">{hotel.name}</Typography>
                          <Typography variant="body2" color="textSecondary">Rating: {hotel.rate}</Typography>
                          <Typography variant="body2" color="textSecondary" style={{ marginTop: '8px' }}>{hotel.amenities}</Typography>
                        </div>
                      ))}
                    </div>
                  </div>

                  {/* Itineraries Section */}
                  <div style={{ marginBottom: '20px' }}>
                    <Typography variant="subtitle1" color="textPrimary" fontWeight="bold" gutterBottom>Itineraries</Typography>
                    <div style={{ marginTop: '10px', display: 'flex', flexDirection: 'column', gap: '12px' }}>
                      {deal.itineraries.map(itinerary => (
                        <div key={itinerary.id} style={{
                          padding: '12px',
                          backgroundColor: '#f9fafb',
                          borderRadius: '8px',
                          boxShadow: '0 2px 6px rgba(0, 0, 0, 0.05)',
                        }}>
                          <Typography variant="body2" color="textPrimary" fontWeight="bold">
                            Day {itinerary.day}:
                          </Typography>
                          <Typography variant="body2" color="textSecondary">{itinerary.name}</Typography>
                        </div>
                      ))}
                    </div>
                  </div>

                  {/* Video Section */}
                  <div style={{ marginBottom: '20px' }}>
                    <Typography variant="body2" color="primary" fontWeight="bold">
                      Video: <a href={deal.video} target="_blank" rel="noopener noreferrer" style={{
                        color: '#3f51b5',
                        textDecoration: 'underline',
                        fontWeight: 'bold',
                      }}>Watch Video</a>
                    </Typography>
                  </div>
                </CardContent>

                {/* Media Section - Top Right Corner */}
                {deal.media && deal.media.length > 0 && (
                  <div style={{
                    position: 'absolute',
                    top: '10px',
                    right: '10px',
                    width: '80px',
                    height: '80px',
                    backgroundColor: 'rgba(255, 255, 255, 0.7)',
                    borderRadius: '8px',
                    overflow: 'hidden',
                    boxShadow: '0 2px 6px rgba(0, 0, 0, 0.1)',
                  }}>
                    <img
                      src={deal.media[0].url}
                      alt="Media"
                      style={{
                        width: '100%',
                        height: '100%',
                        objectFit: 'cover',
                      }}
                    />
                  </div>
                )}

                {/* Card Action Buttons */}
                <div style={{
                  display: 'flex',
                  justifyContent: 'flex-end',
                  padding: '10px',
                  backgroundColor: '#f9f9f9',
                }}>
                  <IconButton onClick={() => navigate(`/deals/${deal.id}/edit`)} color="primary">
                    <Edit />
                  </IconButton>
                  {/* Only allow deletion for SuperAdmin */}
                  {isSuperAdmin && (
                    <IconButton onClick={() => handleDelete(deal.id)} color="error">
                      <Trash />
                    </IconButton>
                  )}
                </div>
              </Card>
            </Grid>
          ))}
        </Grid>

        {/* Pagination */}
        <div style={{ display: 'flex', justifyContent: 'center', marginTop: '30px' }}>
          <Pagination
            count={Math.ceil(deals.length / itemsPerPage)}
            page={currentPage}
            onChange={handlePageChange}
            color="primary"
          />
        </div>
      </div>
    </>
  );
};

export default DealList;
