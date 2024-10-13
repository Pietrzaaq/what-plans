import axios from "axios";
export default new class PlacesService {

    async getAll(geohashes) {
        return await axios.get('https://localhost:5000/api/places', {
            params: {
                geohashes: geohashes
            }
        });
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

