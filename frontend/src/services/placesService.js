import axios from "axios";
export default new class PlacesService {

    async getAll() {
        return await axios.get(`${import.meta.env.VITE_API_BASE_URL}/places`).then(response => response.data);
    }

    async getAllWithEvents() {
        return await axios.get(`${import.meta.env.VITE_API_BASE_URL}/places/events`).then(response => response.data);
    }

    async getEventsByPlaceId(placeId) {
        return await axios.get(`${import.meta.env.VITE_API_BASE_URL}/places/${placeId}/events`).then(response => response.data);
    }

    async create(place) {
        return await axios.post(`${import.meta.env.VITE_API_BASE_URL}/places`, place);
    }
};

