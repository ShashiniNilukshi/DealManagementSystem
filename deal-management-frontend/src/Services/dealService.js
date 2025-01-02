import axios from 'axios';


const API_URL = 'https://localhost:7216/api/deals'; // hardcoded

// Create axios instance with auth header
const axiosInstance = axios.create();
axiosInstance.interceptors.request.use((config) => {
  const user = JSON.parse(localStorage.getItem('user'));
  if (user && user.token) {
    config.headers.Authorization = `Bearer ${user.token}`;
  }
  return config;
});


export const dealService = {
  // Get all deals
  async getAllDeals() {
    const response = await axiosInstance.get(API_URL);
    return response.data;
  },

  // Get a specific deal by ID
  async getDealById(id) {
    const response = await axiosInstance.get(`${API_URL}/${id}`);
    return response.data;
  },

  // Create a new deal
  async createDeal(dealData) {
    const response = await axiosInstance.post(API_URL, dealData);
    return response.data;
  },

  // Update an existing deal
  async updateDeal(id, dealData) {
    const response = await axiosInstance.put(`${API_URL}/${id}`, dealData);
    return response.data;
  },

  // Delete a deal (only for SuperAdmin)
  async deleteDeal(id) {
    const response = await axiosInstance.delete(`${API_URL}/${id}`);
    return response.data;
  },

  // Add a hotel to a deal
  async addHotelToDeal(dealId, hotelData) {
    const response = await axiosInstance.post(`${API_URL}/${dealId}/hotels`, hotelData);
    return response.data;
  },

  // Update an existing hotel in a deal
  async updateHotelInDeal(dealId, hotelId, hotelData) {
    const response = await axiosInstance.put(`${API_URL}/${dealId}/hotels/${hotelId}`, hotelData);
    return response.data;
  },

  // Delete a hotel from a deal
  async deleteHotelFromDeal(dealId, hotelId) {
    const response = await axiosInstance.delete(`${API_URL}/${dealId}/hotels/${hotelId}`);
    return response.data;
  },

  // Add an itinerary to a deal
  async addItineraryToDeal(dealId, itineraryData) {
    const response = await axiosInstance.post(`${API_URL}/${dealId}/itineraries`, itineraryData);
    return response.data;
  },

  // Update an existing itinerary in a deal
  async updateItineraryInDeal(dealId, itineraryId, itineraryData) {
    const response = await axiosInstance.put(`${API_URL}/${dealId}/itineraries/${itineraryId}`, itineraryData);
    return response.data;
  },

  // Delete an itinerary from a deal
  async deleteItineraryFromDeal(dealId, itineraryId) {
    const response = await axiosInstance.delete(`${API_URL}/${dealId}/itineraries/${itineraryId}`);
    return response.data;
  },

  // Add media (image/video) to a hotel
  async addHotelMedia(dealId, hotelId, mediaData) {
    const response = await axiosInstance.post(`${API_URL}/${dealId}/hotels/${hotelId}/media`, mediaData);
    return response.data;
  },

  // Update media for a hotel
  async updateHotelMedia(dealId, hotelId, mediaId, mediaData) {
    const response = await axiosInstance.put(`${API_URL}/${dealId}/hotels/${hotelId}/media/${mediaId}`, mediaData);
    return response.data;
  },

  // Delete media for a hotel
  async deleteHotelMedia(dealId, hotelId, mediaId) {
    const response = await axiosInstance.delete(`${API_URL}/${dealId}/hotels/${hotelId}/media/${mediaId}`);
    return response.data;
  },

  // Get all hotels for a specific deal
  async getHotelsForDeal(dealId) {
    const response = await axiosInstance.get(`${API_URL}/${dealId}/hotels`);
    return response.data;
  },

  // Get all itineraries for a specific deal
  async getItinerariesForDeal(dealId) {
    const response = await axiosInstance.get(`${API_URL}/${dealId}/itineraries`);
    return response.data;
  },

  // Get all media for a specific hotel in a deal
  async getHotelMedia(dealId, hotelId) {
    const response = await axiosInstance.get(`${API_URL}/${dealId}/hotels/${hotelId}/media`);
    return response.data;
  },

  // Get all media for a specific deal
  async getAllMedia(dealId) {
    const response = await axiosInstance.get(`${API_URL}/${dealId}/media`);
    return response.data;
  },

  // Delete all media for a specific deal
  async deleteAllMedia(dealId) {
    const response = await axiosInstance.delete(`${API_URL}/${dealId}/media`);
    return response.data;
  },
};
