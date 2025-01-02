import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { 
  TextField, Button, Grid, Typography, IconButton, Card, 
  CardContent, CardHeader, Box, Container, Paper, ThemeProvider, createTheme
} from '@mui/material';
import { Plus, Trash } from 'lucide-react';
import { dealService } from '../Services/dealService';
const theme = createTheme({
  palette: {
    primary: {
      main: '#1976d2',
    },
  },
});

const CreateDeal = () => {
  const navigate = useNavigate();
  const [deal, setDeal] = useState({
    name: '',
    slug: '',
    video: '',
    hotels: [{
      name: '',
      rate: 0,
      amenities: '',
      media: [{ type: '', url: '' }]
    }],
    itineraries: [{ name: '', day: 0 }],
    createdAt: new Date().toISOString(),
    updatedAt: new Date().toISOString(),
  });

  const handleHotelChange = (index, field, value) => {
    const updatedHotels = [...deal.hotels];
    updatedHotels[index][field] = value;
    setDeal({ ...deal, hotels: updatedHotels });
  };

  const handleItineraryChange = (index, field, value) => {
    const updatedItineraries = [...deal.itineraries];
    updatedItineraries[index][field] = value;
    setDeal({ ...deal, itineraries: updatedItineraries });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      await dealService.createDeal(deal);
      navigate('/deals');
    } catch (error) {
      console.error('Error creating deal:', error);
    }
  };

  return (
    <ThemeProvider theme={theme}>
      <Container maxWidth="lg" sx={{ py: 4 }}>
        <Paper elevation={0} sx={{ p: 4, bgcolor: '#f8fafc' }}>
          <Typography variant="h4" gutterBottom sx={{ fontWeight: 'bold', color: 'primary.main' }}>
            Create New Deal
          </Typography>

          <Box component="form" onSubmit={handleSubmit} sx={{ mt: 3 }}>
            <Grid container spacing={3}>
              <Grid item xs={12} md={6}>
                <TextField
                  fullWidth
                  label="Deal Name"
                  value={deal.name}
                  onChange={(e) => setDeal({ ...deal, name: e.target.value })}
                  required
                  variant="outlined"
                />
              </Grid>
              <Grid item xs={12} md={6}>
                <TextField
                  fullWidth
                  label="Slug"
                  value={deal.slug}
                  onChange={(e) => setDeal({ ...deal, slug: e.target.value })}
                  required
                  variant="outlined"
                />
              </Grid>
              <Grid item xs={12}>
                <TextField
                  fullWidth
                  label="Video URL"
                  value={deal.video}
                  onChange={(e) => setDeal({ ...deal, video: e.target.value })}
                  variant="outlined"
                />
              </Grid>
            </Grid>

            <Box sx={{ mt: 4, mb: 2 }}>
              <Typography variant="h5" sx={{ fontWeight: 600, color: 'primary.main', mb: 2 }}>
                Hotels
              </Typography>
              {deal.hotels.map((hotel, index) => (
                <Card key={index} sx={{ mb: 3, borderRadius: 2, overflow: 'visible' }}>
                  <CardHeader
                    title={`Hotel ${index + 1}`}
                    sx={{
                      bgcolor: 'primary.main',
                      color: 'white',
                      '& .MuiTypography-root': { fontSize: '1.1rem' }
                    }}
                    action={
                      <IconButton onClick={() => {
                        setDeal({ ...deal, hotels: deal.hotels.filter((_, i) => i !== index) });
                      }} sx={{ color: 'white' }}>
                        <Trash />
                      </IconButton>
                    }
                  />
                  <CardContent sx={{ bgcolor: '#fff' }}>
                    <Grid container spacing={3}>
                      <Grid item xs={12} md={6}>
                        <TextField
                          fullWidth
                          label="Hotel Name"
                          value={hotel.name}
                          onChange={(e) => handleHotelChange(index, 'name', e.target.value)}
                          required
                          variant="outlined"
                        />
                      </Grid>
                      <Grid item xs={12} md={6}>
                        <TextField
                          fullWidth
                          label="Rate"
                          type="number"
                          value={hotel.rate}
                          onChange={(e) => handleHotelChange(index, 'rate', e.target.value)}
                          required
                          variant="outlined"
                        />
                      </Grid>
                      <Grid item xs={12}>
                        <TextField
                          fullWidth
                          label="Amenities"
                          value={hotel.amenities}
                          onChange={(e) => handleHotelChange(index, 'amenities', e.target.value)}
                          required
                          variant="outlined"
                          multiline
                          rows={2}
                          placeholder="Enter hotel amenities..."
                        />
                      </Grid>
                      {hotel.media.map((media, mediaIndex) => (
                        <Grid item xs={12} key={mediaIndex}>
                          <Box sx={{ mb: 2 }}>
                            <TextField
                              fullWidth
                              label="Media Type"
                              value={media.type}
                              onChange={(e) => {
                                const updatedMedia = [...hotel.media];
                                updatedMedia[mediaIndex].type = e.target.value;
                                const updatedHotels = [...deal.hotels];
                                updatedHotels[index].media = updatedMedia;
                                setDeal({ ...deal, hotels: updatedHotels });
                              }}
                              variant="outlined"
                              sx={{ mb: 2 }}
                              select
                              SelectProps={{
                                native: true,
                              }}
                            >
                              <option value="">Select type</option>
                              <option value="image">Image</option>
                              <option value="video">Video</option>
                            </TextField>
                            <TextField
                              fullWidth
                              label="Media URL"
                              value={media.url}
                              onChange={(e) => {
                                const updatedMedia = [...hotel.media];
                                updatedMedia[mediaIndex].url = e.target.value;
                                const updatedHotels = [...deal.hotels];
                                updatedHotels[index].media = updatedMedia;
                                setDeal({ ...deal, hotels: updatedHotels });
                              }}
                              variant="outlined"
                              placeholder="Enter media URL..."
                            />
                          </Box>
                        </Grid>
                      ))}
                    </Grid>
                    <Button
                      variant="outlined"
                      startIcon={<Plus />}
                      onClick={() => {
                        const updatedHotels = [...deal.hotels];
                        updatedHotels[index].media.push({ type: '', url: '' });
                        setDeal({ ...deal, hotels: updatedHotels });
                      }}
                      sx={{ mt: 2 }}
                    >
                      Add Media
                    </Button>
                  </CardContent>
                </Card>
              ))}
              <Button
                variant="outlined"
                startIcon={<Plus />}
                onClick={() => setDeal({
                  ...deal,
                  hotels: [...deal.hotels, { name: '', rate: 0, amenities: '', media: [{ type: '', url: '' }] }]
                })}
                sx={{ mb: 4 }}
              >
                Add Hotel
              </Button>
            </Box>

            <Box sx={{ mt: 4 }}>
              <Typography variant="h5" sx={{ fontWeight: 600, color: 'primary.main', mb: 2 }}>
                Itineraries
              </Typography>
              {deal.itineraries.map((itinerary, index) => (
                <Box key={index} sx={{ mb: 2, display: 'flex', alignItems: 'center', gap: 2 }}>
                  <Grid container spacing={3} alignItems="center">
                    <Grid item xs={12} md={5}>
                      <TextField
                        fullWidth
                        label={`Itinerary Day ${itinerary.day}`}
                        value={itinerary.name}
                        onChange={(e) => handleItineraryChange(index, 'name', e.target.value)}
                        required
                        variant="outlined"
                        multiline
                        rows={2}
                        placeholder="Enter itinerary details..."
                      />
                    </Grid>
                    <Grid item xs={12} md={5}>
                      <TextField
                        fullWidth
                        label="Day"
                        type="number"
                        value={itinerary.day}
                        onChange={(e) => handleItineraryChange(index, 'day', e.target.value)}
                        required
                        variant="outlined"
                        InputProps={{
                          inputProps: { min: 0 }
                        }}
                      />
                    </Grid>
                    <Grid item xs={12} md={2}>
                      <IconButton
                        onClick={() => setDeal({ 
                          ...deal, 
                          itineraries: deal.itineraries.filter((_, i) => i !== index) 
                        })}
                        color="error"
                      >
                        <Trash />
                      </IconButton>
                    </Grid>
                  </Grid>
                </Box>
              ))}
              <Button
                variant="outlined"
                startIcon={<Plus />}
                onClick={() => setDeal({
                  ...deal,
                  itineraries: [...deal.itineraries, { name: '', day: 0 }]
                })}
                sx={{ mt: 2, mb: 4 }}
              >
                Add Itinerary
              </Button>
            </Box>

            <Button
              type="submit"
              variant="contained"
              size="large"
              sx={{ mt: 4 }}
            >
              Create Deal
            </Button>
          </Box>
        </Paper>
      </Container>
    </ThemeProvider>
  );
};

export default CreateDeal;