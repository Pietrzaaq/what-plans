import { computed, ref } from 'vue';
import { defineStore } from "pinia";
import { MAP_TYPES } from "@/models/mapTypes.js";
import mapService from "@/services/mapService.js";
import L from 'leaflet';
import { useFilterStore } from "@/stores/filter.js";

const DEFAULT_COORDINATES = [51.769406790090855, 19.43750792680422];
const DEFAULT_ZOOM = 13;
const OPEN_STREET_MAP_TILE_URL = 'https://tile.openstreetmap.org/{z}/{x}/{y}.png';
const MAP_TILER_TILE_URL = `https://api.maptiler.com/maps/basic-v2/{z}/{x}/{y}.png?key=${import.meta.env.VITE_MAP_TILER_API_KEY}`;

const filterStore = useFilterStore();

export const useMapStore = defineStore(
    'map', () => {
        const _map = ref(L.Map);
        const _tileLayer = ref(L.TileLayer);
        const _tileUrl = ref(OPEN_STREET_MAP_TILE_URL);
        const _center = ref(DEFAULT_COORDINATES);
        const _zoom = ref(DEFAULT_ZOOM);
        const _data = ref([]);
        const _loadedGeohashes = ref([]);
        const _currentGeohashes = ref([]);
        const _geohashesToLoad = ref([]);
        const _geohashPrecision = ref(4);

        const map = computed(() => _map.value);
        const tileLayer = computed(() => _tileLayer.value);
        const tileUrl = computed(() => _tileUrl.value);
        const center = computed(() => _center.value);
        const zoom = computed(() => _zoom.value);
        const data = computed(() => _data.value);
        const loadedGeohashes = computed(() => _loadedGeohashes.value);
        const currentGeohashes = computed(() => _currentGeohashes.value);
        const geohashesToLoad = computed(() => _geohashesToLoad.value);
        const geohashPrecision = computed(() => _geohashPrecision.value);

        async function initialize() {
            navigator.geolocation.getCurrentPosition((position) => {
                _center.value = [position.latitude, position.longitude];
            });

            _map.value = L.map('map', {
                center: _center.value,
                zoom: _zoom.value,
                doubleClickZoom: false,
                zoomAnimation: true
            });

            const isMapTilerValid = await checkTileProvider(MAP_TILER_TILE_URL);
            if (isMapTilerValid) {
                _tileUrl.value = MAP_TILER_TILE_URL;
            }
            
            _tileLayer.value = L.tileLayer(_tileUrl.value, {
                minZoom: 7,
                maxZoom: 19,
                closePopupOnClick: false,
                attribution: '<span>Projekt wykonany w ramach pracy magisterkiej na Uniwersytecie Łódzkim</span> &copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>'
            });

            _tileLayer.value.addTo(map.value);
        }
        
        async function loadData(mapType) {
            if (_geohashPrecision.value < 4) {
                const cities = await mapService.getCities(geohashesToLoad.value);
                _data.value.push(...cities);
            }
            else if (mapType === MAP_TYPES.EVENT) {
                const startDate= filterStore.startDate;
                const endDate = filterStore.endDate;
                const events = await mapService.getEvents(geohashesToLoad.value, startDate, endDate);
                _data.value.push(...events);
            }
            else {
                const places = await mapService.getPlaces(geohashesToLoad.value);
                _data.value.push(...places);
            }

            _loadedGeohashes.value.push(...geohashesToLoad.value);
            _geohashesToLoad.value = [];
        }
        
        function clear() {
            _data.value = [];
            _loadedGeohashes.value = [];
            _geohashesToLoad.value = [];
        }

        function destroy() {
            _map.value.remove();
            _center.value = DEFAULT_COORDINATES;
            clear();
        }

        function setCenter(center) {
            _center.value = center;
        }
        
        function setZoom(zoom) {
            _zoom.value = zoom;
        }
        
        function setGeohashPrecision(precision) {
            _geohashPrecision.value = precision;
        }

        function setCurrentGeohashes(geohashes) {
            _currentGeohashes.value = geohashes;
        }
        
        function setGeohashesToLoad(geohashesToLoad) {
            _geohashesToLoad.value = geohashesToLoad;
        }

        return {
            map,
            tileLayer,
            tileUrl,
            center,
            zoom,
            data,
            loadedGeohashes,
            currentGeohashes,
            geohashesToLoad,
            geohashPrecision,
            initialize,
            destroy,
            loadData,
            clear,
            setCenter,
            setZoom,
            setGeohashPrecision,
            setCurrentGeohashes,
            setGeohashesToLoad
        };
    });

async function checkTileProvider(url) {
    try {
        const response = await fetch(url.replace('{z}', '0').replace('{x}', '0').replace('{y}', '0'));
        return response.ok;
    } catch (error) {
        return false;
    }
}