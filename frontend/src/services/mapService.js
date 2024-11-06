import axios from "axios";
import { getApiDateTime } from "@/helpers/helpers.js";
export default new class MapService {

    async getPlaces(geohashes) {
        return await axios.get(`${import.meta.env.VITE_API_BASE_URL}/map/places`, {
            params: {
                geohashes: geohashes
            }
        }).then(response => response.data);
    }

    async getEvents(geohashes, startDate, endDate) {
        let config;
        
        if (startDate && endDate) {
            config = {
                params: {
                    geohashes: geohashes,
                    startDate: getApiDateTime(startDate),
                    endDate: getApiDateTime(endDate)
                }
            };
        } 
        else {
            config = {
                params: {
                    geohashes: geohashes
                }
            };
        }
        
        return await axios.get(`${import.meta.env.VITE_API_BASE_URL}/map/events`, config)
            .then(response => response.data);
    }

    async getCities(geohashes) {
        return await axios.get(`${import.meta.env.VITE_API_BASE_URL}/map/cities`, {
            params: {
                geohashes: geohashes
            }
        }).then(response => response.data);
    }
};

