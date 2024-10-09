import axios from "axios";
export default new class PlacesService {

    async getAll() {
        return await axios.get('https://localhost:5000/api/places');
    }

    async getAllWithEvents() {
        return await axios.get(`https://localhost:5000/api/places/events`);
    }

    async getPlaceWithEvents(placeId) {
        return await axios.get(`https://localhost:5000/api/places/${placeId}/events`);
    }

    async create(place) {
        return await axios.post('https://localhost:5000/api/places', place);
    }
};

