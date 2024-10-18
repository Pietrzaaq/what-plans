import axios from "axios";
export default new class CitiesService {
    async getAll() {
        return await axios.get(`${import.meta.env.VITE_API_BASE_URL}/cities`)
            .then(response => response.data);
    }
};

