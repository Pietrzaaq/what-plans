import axios from "axios";
export default new class PlacesService {

    async getAll(geohashes) {
        return await axios.get('https://localhost:5000/api/places', {
            params: {
                geohashes: geohashes
            }
        }).then(response => response.data);
    }

    async getAllWithEvents() {
        return await axios.get(`https://localhost:5000/api/places/events`)
            .then(response => response.data);
    }

    async getEventsByPlaceId(placeId) {
        return await axios.get(`https://localhost:5000/api/places/${placeId}/events`)
            .then(response => response.data);
    }

    async create(place) {
        return await axios.post('https://localhost:5000/api/places', place);
    }
};

