import axios from "axios";
export default new class MapService {

    async getPlaces(geohashes) {
        return await axios.get(`${import.meta.env.VITE_API_BASE_URL}/map/places`, {
            params: {
                geohashes: geohashes
            }
        }).then(response => response.data);
    }

    async getEvents(geohashes) {
        return await axios.get(`${import.meta.env.VITE_API_BASE_URL}/map/events`, {
            params: {
                geohashes: geohashes
            }
        }).then(response => response.data);
    }

    async getCities(geohashes) {
        return await axios.get(`${import.meta.env.VITE_API_BASE_URL}/map/cities`, {
            params: {
                geohashes: geohashes
            }
        }).then(response => response.data);
    }
};

