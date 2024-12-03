import axios from "axios";
export default new class SportEventService {
    
    async getAll() {
        return await axios.get(`${import.meta.env.VITE_API_BASE_URL}/events`).then(response => response.data);
    }

    async getAllForPlace(placeId) {
        return await axios.get(`${import.meta.env.VITE_API_BASE_URL}/events/places/${placeId}`).then(response => response.data);
    }

    async create(event) {
        return await axios.post(`${import.meta.env.VITE_API_BASE_URL}/events`, event).then(response => response.data);
    }
};

