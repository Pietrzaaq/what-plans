import axios from "axios";
export default new class UsersService {

    async me() {
        return await axios.get('https://localhost:5000/api/users/me')
            .then(response => response.data);
    }

    async login(request) {
        return await axios.post('https://localhost:5000/api/users/login', request)
            .then(response => response.data);
    }

    async register(request) {
        return await axios.post('https://localhost:5000/api/users/register', request)
            .then(response => response.data);
    }
};