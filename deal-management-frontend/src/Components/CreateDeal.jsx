import React, { useState } from 'react';
import { TextField, Button, Grid, Typography, IconButton, Card, CardContent, CardHeader, Box } from '@mui/material';
import { Plus, Trash } from 'lucide-react';
import { useNavigate } from 'react-router-dom';
import { dealService } from '../Services/dealService';

const CreateDeal = () => {
  const [deal, setDeal] = useState({
    name: '',
    slug: '',
    video: '',
    hotels: [
      {
        name: '',
        rate: 0,
        amenities: '',
        media: [{ type: '', url: '' }]
      }
    ],
    itineraries: [{ name: '', day: 0 }],
    createdAt: new Date().toISOString(),
    updatedAt: new Date().toISOString(),
  });

  const navigate = useNavigate();

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

  const handleAddHotel = () => {
    setDeal({
      ...deal,
      hotels: [
        ...deal.hotels,
        { name: '', rate: 0, amenities: '', media: [{ type: '', url: '' }] }
      ]
    });
  };

  const handleAddItinerary = () => {
    setDeal({
      ...deal,
      itineraries: [...deal.itineraries, { name: '', day: 0 }]
    });
  };

  const handleDeleteHotel = (index) => {
    const updatedHotels = deal.hotels.filter((_, i) => i !== index);
    setDeal({ ...deal, hotels: updatedHotels });
  };

  const handleDeleteItinerary = (index) => {
    const updatedItineraries = deal.itineraries.filter((_, i) => i !== index);
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
    <div style={{ padding: '20px', backgroundColor: '#f4f7fa' }}>
      <Typography variant="h4" color="primary" style={{ fontWeight: 'bold' }}>Create New Deal</Typography>
      <form onSubmit={handleSubmit} style={{ marginTop: '20px' }}>
        {/* Deal Name and Slug */}
        <Grid container spacing={3}>
          <Grid item xs={12} sm={6}>
            <TextField
              fullWidth
              label="Deal Name"
              value={deal.name}
              onChange={(e) => setDeal({ ...deal, name: e.target.value })}
              required
            />
          </Grid>
          <Grid item xs={12} sm={6}>
            <TextField
              fullWidth
              label="Slug"
              value={deal.slug}
              onChange={(e) => setDeal({ ...deal, slug: e.target.value })}
              required
            />
          </Grid>
        </Grid>

        {/* Video URL */}
        <TextField
          fullWidth
          label="Video URL"
          value={deal.video}
          onChange={(e) => setDeal({ ...deal, video: e.target.value })}
          style={{ marginTop: '20px' }}
        />

        {/* Hotels Section */}
        <Typography variant="h6" color="primary" style={{ marginTop: '20px', fontWeight: 'bold' }}>Hotels</Typography>
        {deal.hotels.map((hotel, index) => (
          <Card key={index} sx={{ boxShadow: 3, borderRadius: '12px', marginBottom: '16px' }}>
            <CardHeader title={`Hotel ${index + 1}`} sx={{ backgroundColor: '#3f51b5', color: 'white', textAlign: 'center' }} />
            <CardContent sx={{ backgroundColor: '#f3f4f6' }}>
              <Grid container spacing={3}>
                <Grid item xs={12} sm={6}>
                  <TextField
                    fullWidth
                    label="Hotel Name"
                    value={hotel.name}
                    onChange={(e) => handleHotelChange(index, 'name', e.target.value)}
                    required
                  />
                </Grid>
                <Grid item xs={12} sm={6}>
                  <TextField
                    fullWidth
                    label="Hotel Rate"
                    value={hotel.rate}
                    onChange={(e) => handleHotelChange(index, 'rate', e.target.value)}
                    type="number"
                    required
                  />
                </Grid>
                <Grid item xs={12}>
                  <TextField
                    fullWidth
                    label="Amenities"
                    value={hotel.amenities}
                    onChange={(e) => handleHotelChange(index, 'amenities', e.target.value)}
                    required
                  />
                </Grid>
                {/* Media Section */}
                <Grid item xs={12}>
                  <Typography variant="body2" color="textPrimary" style={{ fontWeight: 'bold' }}>Hotel Media</Typography>
                  {hotel.media.map((media, mediaIndex) => (
                    <div key={mediaIndex} style={{ marginBottom: '10px' }}>
                      <TextField
                        label="Media Type (e.g., Image, Video)"
                        value={media.type}
                        onChange={(e) => {
                          const updatedMedia = [...hotel.media];
                          updatedMedia[mediaIndex].type = e.target.value;
                          const updatedHotels = [...deal.hotels];
                          updatedHotels[index].media = updatedMedia;
                          setDeal({ ...deal, hotels: updatedHotels });
                        }}
                        style={{ marginBottom: '10px' }}
                      />
                      <TextField
                        label="Media URL"
                        value={media.url}
                        onChange={(e) => {
                          const updatedMedia = [...hotel.media];
                          updatedMedia[mediaIndex].url = e.target.value;
                          const updatedHotels = [...deal.hotels];
                          updatedHotels[index].media = updatedMedia;
                          setDeal({ ...deal, hotels: updatedHotels });
                        }}
                        style={{ marginBottom: '10px' }}
                      />
                    </div>
                  ))}
                </Grid>
              </Grid>
              {/* Add/Delete Hotel Media */}
              <Button
                variant="outlined"
                color="primary"
                startIcon={<Plus />}
                onClick={() => {
                  const updatedHotels = [...deal.hotels];
                  updatedHotels[index].media.push({ type: '', url: '' });
                  setDeal({ ...deal, hotels: updatedHotels });
                }}
              >
                Add Media
              </Button>
              <IconButton onClick={() => handleDeleteHotel(index)} color="error" style={{ marginLeft: '10px' }}>
                <Trash />
              </IconButton>
            </CardContent>
          </Card>
        ))}
        <Button
          variant="outlined"
          color="primary"
          startIcon={<Plus />}
          onClick={handleAddHotel}
          style={{ marginBottom: '20px' }}
        >
          Add Hotel
        </Button>

        {/* Itineraries Section */}
        <Typography variant="h6" color="primary" style={{ marginTop: '20px', fontWeight: 'bold' }}>Itineraries</Typography>
        {deal.itineraries.map((itinerary, index) => (
          <Grid container spacing={3} key={index} style={{ marginBottom: '20px' }}>
            <Grid item xs={12} sm={6}>
              <TextField
                fullWidth
                label={`Itinerary Day ${itinerary.day}`}
                value={itinerary.name}
                onChange={(e) => handleItineraryChange(index, 'name', e.target.value)}
                required
              />
            </Grid>
            <Grid item xs={12} sm={6}>
              <TextField
                fullWidth
                label="Day"
                value={itinerary.day}
                onChange={(e) => handleItineraryChange(index, 'day', e.target.value)}
                type="number"
                required
              />
            </Grid>
            <IconButton onClick={() => handleDeleteItinerary(index)} color="error">
              <Trash />
            </IconButton>
          </Grid>
        ))}
        <Button
          variant="outlined"
          color="primary"
          startIcon={<Plus />}
          onClick={handleAddItinerary}
          style={{ marginBottom: '20px' }}
        >
          Add Itinerary
        </Button>

        {/* Submit Button */}
        <Button type="submit" variant="contained" color="primary" style={{ marginTop: '20px' }}>
          Create Deal
        </Button>
      </form>
    </div>
  );
};

export default CreateDeal;
