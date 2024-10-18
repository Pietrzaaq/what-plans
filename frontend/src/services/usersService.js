import axios from "axios";
export default new class UsersService {

    async me() {
        return await axios.get(`${import.meta.env.VITE_API_BASE_URL}/users/me`)
            .then(response => response.data);
    }

    async login(request) {
        return await axios.post(`${import.meta.env.VITE_API_BASE_URL}/users/login`, request)
            .then(response => response.data);
    }

    async register(request) {
        return await axios.post(`${import.meta.env.VITE_API_BASE_URL}/users/register`, request)
            .then(response => response.data);
    }
};