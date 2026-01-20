import { onMounted } from "vue";
import { useMapStore } from "@/stores/map.js";
import L from "leaflet";
import { checkTileProvider } from "@/helpers/helpers.js";
import { useGlobalStore } from "@/stores/global.js";
import geolocationService from "@/services/geolocationService.js";

const OPEN_STREET_MAP_TILE_URL = 'https://tile.openstreetmap.org/{z}/{x}/{y}.png';
const MAX_ZOOM = 19;
const MIN_ZOOM = 6;

export function useMap() {
    const mapStore = useMapStore();
    const globalStore = useGlobalStore();
    async function initialize() {
        loadCenter();
        
        const map = L.map('map', {
            center: mapStore.center,
            zoom: mapStore.zoom,
            doubleClickZoom: false,
            zoomAnimation: true,
            zoomSnap: 1
        });
        mapStore.setMap(map);

        let tileProviderUrl = import.meta.env.VITE_TILE_PROVIDER_URL;
        const isMapTilerValid = await checkTileProvider(tileProviderUrl);
        if (!isMapTilerValid) 
            tileProviderUrl = OPEN_STREET_MAP_TILE_URL;

        const tileLayer = L.tileLayer(tileProviderUrl, {
            minZoom: MIN_ZOOM,
            maxZoom: MAX_ZOOM,
            closePopupOnClick: false,
            attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a>'
        });
        mapStore.setTileLayer(tileLayer);

        const geohash = geolocationService.getGeohash(mapStore.center[0], mapStore.center[1], mapStore.zoom);
        mapStore.setGeohash(geohash);

        const bounds = mapStore.map.getBounds();
        const bbox = {
            north: bounds._northEast.lat,
            west: bounds._northEast.lng,
            south: bounds._southWest.lat,
            east: bounds._southWest.lng,
        };
        mapStore.setBbox(bbox);
    }
    
    function loadCenter() {
        if (globalStore.city)
        navigator.geolocation.getCurrentPosition((position) => {
            if (!position.latitude || !position.longitude )
                return;

            mapStore.setCenter([position.latitude, position.longitude]) ;
        });
    }
    
    onMounted(() => {
    });
    
    return {
        initialize
    };
}