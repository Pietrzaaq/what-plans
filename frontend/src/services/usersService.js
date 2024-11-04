import axios from "axios";
import User from "@/models/user.js";
import UserLight from "@/models/userLight.js";
export default new class UsersService {
    async getUserById(userId) {
        return await axios.get(`${import.meta.env.VITE_API_BASE_URL}/users/${userId}`)
            .then(response => new UserLight(response.data));
    }

    async me() {
        return await axios.get(`${import.meta.env.VITE_API_BASE_URL}/users/me`)
            .then(response => new User(response.data ));
    }
    
    async update(request) {
        return await axios.patch(`${import.meta.env.VITE_API_BASE_URL}/users/me`, request)
            .then(response => new User(response.data));
    }

    async login(request) {
        return await axios.post(`${import.meta.env.VITE_API_BASE_URL}/users/login`, request)
            .then(response => response.data);
    }

    async register(request) {
        return await axios.post(`${import.meta.env.VITE_API_BASE_URL}/users/register`, request)
            .then(response => new User(response.data));
    }
};