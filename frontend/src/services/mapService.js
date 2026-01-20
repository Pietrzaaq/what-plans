import axios from "axios";
import { getApiDateTime } from "@/helpers/helpers.js";
export default new class MapService {

    async getPlaces(geohash, bbox) {
        return await axios.get(`${import.meta.env.VITE_API_BASE_URL}/map/places`, {
            params: {
                geohash: geohash,
                north: bbox.north,
                west: bbox.west,
                south: bbox.south,
                east: bbox.east
            }
        }).then(response => response.data);
    }

    async getEvents(geohash, bbox, startDate, endDate) {
        let config;
        
        if (startDate && endDate) {
            config = {
                params: {
                    geohash: geohash,
                    north: bbox.north,
                    west: bbox.west,
                    south: bbox.south,
                    east: bbox.east,
                    startDate: getApiDateTime(startDate),
                    endDate: getApiDateTime(endDate)
                }
            };
        } 
        else {
            config = {
                params: {
                    geohash: geohash,
                    north: bbox.north,
                    west: bbox.west,
                    south: bbox.south,
                    east: bbox.east
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

