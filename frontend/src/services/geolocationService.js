import geohash from "ngeohash";

export default new class GeolocationService {
    getGeohashPrecision(zoom) {
        if (zoom > 13)
            return 5;
        else if (zoom > 11)
            return 4;
        else if (zoom > 7)
            return 3;
        else 
            return 2;
    }
    
    getBoundingBoxGeohashes(bounds, precision = 4) {
        const minLat = bounds._southWest.lat;
        const minLong = bounds._southWest.lng;
        const maxLat = bounds._northEast.lat;
        const maxLong = bounds._northEast.lng;

        return geohash.bboxes(minLat, minLong, maxLat, maxLong, precision);
    }
    
    getGeohash(lat, long) {
        return geohash.encode(lat, long, 9);
    }
};