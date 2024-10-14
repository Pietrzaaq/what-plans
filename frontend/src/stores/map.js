import { computed, ref } from 'vue';
import { defineStore } from "pinia";
import { MAP_TYPES } from "@/models/mapTypes.js";
import mapService from "@/services/mapService.js";

const DEFAULT_COORDINATES = [51.769406790090855, 19.43750792680422];
const DEFAULT_ZOOM = 13;

export const useMapStore = defineStore(
    'map', () => {
        const _center = ref(DEFAULT_COORDINATES);
        const _zoom = ref(DEFAULT_ZOOM);
        const _data = ref([]);
        const _loadedGeohashes = ref([]);
        const _currentGeohashes = ref([]);
        const _geohashesToLoad = ref([]);
        const _geohashPrecision = ref(4);
        
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
        }
        
        async function loadData(mapType) {
            if (_geohashPrecision.value < 4) {
                const cities = await mapService.getCities(geohashesToLoad.value);
                _data.value.push(...cities);
            }
            else if (mapType === MAP_TYPES.EVENT) {
                const events = await mapService.getEvents(geohashesToLoad.value);
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
            center,
            zoom,
            data,
            loadedGeohashes,
            currentGeohashes,
            geohashesToLoad,
            geohashPrecision,
            initialize,
            loadData,
            clear,
            setCenter,
            setZoom,
            setGeohashPrecision,
            setCurrentGeohashes,
            setGeohashesToLoad
        };
    });