import axios from "axios";
import { IMAGES_URL } from "@/models/uploadUrl.js";
export default new class ImagesService {
    async upload(data) {
        return await axios.post(`${IMAGES_URL}`, data, {
            headers: {
                'Content-Type': 'multipart/form-data'
            } })
            .then(response => response.data);
    }

    async getById(avatarId) {
        return await axios.get(`${IMAGES_URL}/${avatarId}`)
            .then(response => response.data);
    }

    async getBinaryById(avatarId) {
        return await axios.get(`${IMAGES_URL}/${avatarId}/binary`, {
            responseType: 'arraybuffer'
            })
            .then(response => response.data);
    }
};

