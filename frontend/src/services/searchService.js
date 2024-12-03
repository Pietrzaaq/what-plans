import axios from "axios";
export default new class SearchService {

    async search(query) {
        return await axios.get(`${import.meta.env.VITE_API_BASE_URL}/search`, {
            params: {
                query: query
            }
        }).then(response => response.data);
    }
};

