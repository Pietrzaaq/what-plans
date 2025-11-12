import { onMounted } from "vue";
import { useMapStore } from "@/stores/map.js";
import L from "leaflet";
import { checkTileProvider } from "@/helpers/helpers.js";

const OPEN_STREET_MAP_TILE_URL = 'https://tile.openstreetmap.org/{z}/{x}/{y}.png';
const MAX_ZOOM = 19;
const MIN_ZOOM = 6;

export function useMap() {
    const mapStore = useMapStore();
    async function initialize() {
        loadCenter();
        
        const map = L.map('map', {
            center: mapStore.center,
            zoom: mapStore.zoom,
            doubleClickZoom: false,
            zoomAnimation: true
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
            attribution: '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>'
        });
        mapStore.setTileLayer(tileLayer);
    }
    
    function loadCenter() {
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