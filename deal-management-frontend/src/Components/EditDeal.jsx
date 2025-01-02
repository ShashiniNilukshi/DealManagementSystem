import React, { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { 
  TextField, 
  Button, 
  Grid, 
  Typography, 
  Paper,
  Box,
  Divider,
  CircularProgress,
  Alert
} from '@mui/material';
import { dealService } from '../Services/dealService';

const EditDealPage = () => {
  const { dealId } = useParams();
  const navigate = useNavigate();
  const [deal, setDeal] = useState({
    name: '',
    video: '',
    hotels: [],
    itineraries: []
  });
  const [modifiedHotels, setModifiedHotels] = useState([]);
  const [modifiedItineraries, setModifiedItineraries] = useState([]);
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState('');

  useEffect(() => {
    const fetchDeal = async () => {
      try {
        const fetchedDeal = await dealService.getDealById(dealId);
        setDeal(fetchedDeal);
        console.log("Fetched deal:", fetchedDeal);
        setError('');
      } catch (error) {
        console.error('Error fetching deal:', error);
        setError('Failed to load deal data');
      } finally {
        setLoading(false);
      }
    };

    if (dealId) {
      fetchDeal();
    }
  }, [dealId]);

  const handleSave = async () => {
    console.log("Saving deal...");
    setSaving(true);
    setError('');
    let mainDealUpdated = false;

    try {
      // Update Hotels if modified
      for (const index of modifiedHotels) {
        const hotel = deal.hotels[index];
        console.log("Hotel:", hotel);

        if (hotel.id) {
          const hotelData = {
            name: hotel.name.trim(),
            rate: parseFloat(hotel.rate),
            amenities: hotel.amenities?.trim() || '',
          };

          // Log the hotel update
          console.log("Updating hotel:", hotelData);
          const hotelResponse = await dealService.updateHotelInDeal(dealId, hotel.id, hotelData);
          console.log("Hotel updated:", hotelResponse);

          // Update media if exists
          if (hotel.media?.length > 0) {
            for (const mediaIndex in hotel.media) {
              const media = hotel.media[mediaIndex];
              if (media.id && media.url?.trim()) {
                const mediaData = {
                  type: media.type,
                  url: media.url.trim(),
                };

                // Log the media update
                console.log("Updating media:", mediaData);
                await dealService.updateHotelMedia(dealId, hotel.id, media.id, mediaData);
              } else {
                console.warn("Missing media id or url:", media);
              }
            }
          }

          mainDealUpdated = true;
        }
      }

      // Update Itineraries if modified
      for (const index of modifiedItineraries) {
        const itinerary = deal.itineraries[index];
        if (itinerary.id) {
          const itineraryData = {
            name: itinerary.name.trim(),
            day: parseInt(itinerary.day, 10),
          };
          console.log("Updating itinerary:", itineraryData);
          await dealService.updateItineraryInDeal(dealId, itinerary.id, itineraryData);
          mainDealUpdated = true;
        }
      }

      // If any updates, send the final PUT request
      if (mainDealUpdated) {
        const mainDealData = {
          name: deal.name.trim(),
          video: deal.video.trim(),
        };

        console.log("Updating main deal:", mainDealData);
        await dealService.updateDeal(dealId, mainDealData);
      }

      // Navigate after successful save
      navigate('/deals');
    } catch (error) {
      console.error('Error saving deal:', error);
      setError('Failed to save changes. Please check all required fields.');
    } finally {
      setSaving(false);
      setModifiedHotels([]);
      setModifiedItineraries([]);
    }
  };

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setDeal(prevDeal => ({
      ...prevDeal,
      [name]: value,
    }));
  };

  const handleHotelChange = (index, field, value) => {
    setDeal(prevDeal => {
      const updatedHotels = [...prevDeal.hotels];
      updatedHotels[index] = {
        ...updatedHotels[index],
        [field]: value
      };

      if (!modifiedHotels.includes(index)) {
        setModifiedHotels((prev) => [...prev, index]);
      }

      return {
        ...prevDeal,
        hotels: updatedHotels
      };
    });
  };

  const handleMediaChange = (hotelIndex, mediaIndex, field, value) => {
    setDeal(prevDeal => {
      const updatedHotels = [...prevDeal.hotels];
      updatedHotels[hotelIndex].media[mediaIndex] = {
        ...updatedHotels[hotelIndex].media[mediaIndex],
        [field]: value,
      };

      if (!modifiedHotels.includes(hotelIndex)) {
        setModifiedHotels((prev) => [...prev, hotelIndex]);
      }

      return {
        ...prevDeal,
        hotels: updatedHotels
      };
    });
  };

  const handleItineraryChange = (index, field, value) => {
    setDeal(prevDeal => {
      const updatedItineraries = [...prevDeal.itineraries];
      updatedItineraries[index] = {
        ...updatedItineraries[index],
        [field]: value
      };

      if (!modifiedItineraries.includes(index)) {
        setModifiedItineraries((prev) => [...prev, index]);
      }

      return {
        ...prevDeal,
        itineraries: updatedItineraries
      };
    });
  };

  if (loading) {
    return (
      <Box display="flex" justifyContent="center" alignItems="center" minHeight="400px">
        <CircularProgress />
      </Box>
    );
  }

  return (
    <Box sx={{ p: 3, backgroundColor: '#f4f7fa', minHeight: '100vh' }}>
      <Paper elevation={2} sx={{ p: 4, maxWidth: 1200, margin: '0 auto' }}>
        <Typography variant="h4" color="primary" sx={{ mb: 4 }}>
          Edit Deal
        </Typography>

        {error && (
          <Alert severity="error" sx={{ mb: 3 }}>
            {error}
          </Alert>
        )}

        {/* Main Deal Information */}
        <Box sx={{ mb: 4 }}>
          <Typography variant="h6" sx={{ mb: 2 }}>
            Basic Information
          </Typography>
          <Grid container spacing={3}>
            <Grid item xs={12}>
              <TextField
                label="Deal Name"
                name="name"
                value={deal.name || ''}
                onChange={handleInputChange}
                fullWidth
                required
              />
            </Grid>
            <Grid item xs={12}>
              <TextField
                label="Video URL"
                name="video"
                value={deal.video || ''}
                onChange={handleInputChange}
                fullWidth
              />
            </Grid>
          </Grid>
        </Box>

        <Divider sx={{ my: 4 }} />

        {/* Hotels Section */}
        <Box sx={{ mb: 4 }}>
          <Typography variant="h6" sx={{ mb: 2 }}>
            Hotels
          </Typography>
          {deal.hotels.map((hotel, index) => (
            <Paper key={hotel.id || index} sx={{ p: 3, mb: 2, backgroundColor: '#fafafa' }}>
              <Grid container spacing={3}>
                <Grid item xs={12} md={6}>
                  <TextField
                    label="Hotel Name"
                    value={hotel.name || ''}
                    onChange={(e) => handleHotelChange(index, 'name', e.target.value)}
                    fullWidth
                    required
                  />
                </Grid>
                <Grid item xs={12} md={6}>
                  <TextField
                    label="Rate"
                    type="number"
                    value={hotel.rate || ''}
                    onChange={(e) => handleHotelChange(index, 'rate', e.target.value)}
                    fullWidth
                    required
                    inputProps={{ step: "0.01", min: "0" }}
                  />
                </Grid>
                <Grid item xs={12}>
                  <TextField
                    label="Amenities"
                    value={hotel.amenities || ''}
                    onChange={(e) => handleHotelChange(index, 'amenities', e.target.value)}
                    fullWidth
                    multiline
                    rows={2}
                  />
                </Grid>

                {/* Media Section */}
                <Grid item xs={12}>
                  <Typography variant="body1" sx={{ mb: 1 }}>
                    Media
                  </Typography>
                  {hotel.media?.map((media, mediaIndex) => (
                    <Box key={media.id || mediaIndex} sx={{ mb: 2 }}>
                      <Grid container spacing={2}>
                        <Grid item xs={12} sm={6}>
                          <TextField
                            label="Media Type"
                            value={media.type || ''}
                            onChange={(e) => handleMediaChange(index, mediaIndex, 'type', e.target.value)}
                            fullWidth
                          />
                        </Grid>
                        <Grid item xs={12} sm={6}>
                          <TextField
                            label="Media URL"
                            value={media.url || ''}
                            onChange={(e) => handleMediaChange(index, mediaIndex, 'url', e.target.value)}
                            fullWidth
                          />
                        </Grid>
                      </Grid>
                    </Box>
                  ))}
                </Grid>
              </Grid>
            </Paper>
          ))}
        </Box>

        <Divider sx={{ my: 4 }} />

        {/* Itineraries Section */}
        <Box sx={{ mb: 4 }}>
          <Typography variant="h6" sx={{ mb: 2 }}>
            Itineraries
          </Typography>
          {deal.itineraries.map((itinerary, index) => (
            <Paper key={itinerary.id || index} sx={{ p: 3, mb: 2, backgroundColor: '#fafafa' }}>
              <Grid container spacing={3}>
                <Grid item xs={12} md={8}>
                  <TextField
                    label="Itinerary Name"
                    value={itinerary.name || ''}
                    onChange={(e) => handleItineraryChange(index, 'name', e.target.value)}
                    fullWidth
                    required
                  />
                </Grid>
                <Grid item xs={12} md={4}>
                  <TextField
                    label="Day"
                    type="number"
                    value={itinerary.day || ''}
                    onChange={(e) => handleItineraryChange(index, 'day', e.target.value)}
                    fullWidth
                    required
                    inputProps={{ min: "0", step: "1" }}
                  />
                </Grid>
              </Grid>
            </Paper>
          ))}
        </Box>

        {/* Action Buttons */}
        <Box sx={{ display: 'flex', gap: 2, justifyContent: 'flex-end' }}>
          <Button 
            variant="outlined" 
            onClick={() => navigate('/deals')}
            disabled={saving}
          >
            Cancel
          </Button>
          <Button 
            variant="contained" 
            color="primary" 
            onClick={handleSave}
            disabled={saving}
          >
            {saving ? <CircularProgress size={24} /> : 'Save Changes'}
          </Button>
        </Box>
      </Paper>
    </Box>
  );
};

export default EditDealPage;
