import axios from "axios";
export default new class SportEventService {
    
    async getAll() {
        return await axios.get('https://localhost:5000/api/events').then(response => response.data);
    }

    async getAllForPlace(placeId) {
        return await axios.get(`https://localhost:5000/api/events/places/${placeId}`).then(response => response.data);
    }

    async create(event) {
        return await axios.post('https://localhost:5000/api/events', event).then(response => response.data);
    }
};

