import axios from "axios";
export default new class MapService {

    async getPlaces(geohashes) {
        return await axios.get('https://localhost:5000/api/map/places', {
            params: {
                geohashes: geohashes
            }
        }).then(response => response.data);
    }

    async getEvents(geohashes) {
        return await axios.get('https://localhost:5000/api/map/events', {
            params: {
                geohashes: geohashes
            }
        }).then(response => response.data);
    }

    async getCities(geohashes) {
        return await axios.get('https://localhost:5000/api/map/cities', {
            params: {
                geohashes: geohashes
            }
        }).then(response => response.data);
    }
};

