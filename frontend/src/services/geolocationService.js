import geohash from "ngeohash";

export default new class GeolocationService {
    getGeohashPrecision(zoom) {
        if (zoom > 15)
            return 6;
        if (zoom > 13)
            return 5;
        else if (zoom > 11)
            return 4;
        else if (zoom > 7)
            return 3;
        else 
            return 2;
    }
    
    getBoundingBoxGeohashes(bounds, zoom) {
        const precision = this.getGeohashPrecision(zoom);
        const minLat = bounds._southWest.lat;
        const minLong = bounds._southWest.lng;
        const maxLat = bounds._northEast.lat;
        const maxLong = bounds._northEast.lng;

        return geohash.bboxes(minLat, minLong, maxLat, maxLong, precision);
    }
    
    getGeohash(lat, long, zoom) {
        const precision = this.getGeohashPrecision(zoom);
        return geohash.encode(lat, long, precision);
    }
    
    getGeohashBounds(value) {
        console.log('getting geohash bbox...', geohash.decode_bbox(value), value);
        return geohash.decode_bbox(value);
    }
};