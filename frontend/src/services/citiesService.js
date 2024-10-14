import axios from "axios";
export default new class CitiesService {
    async getAll() {
        return await axios.get('https://localhost:5000/api/cities')
            .then(response => response.data);
    }
};

