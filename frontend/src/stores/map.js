import { computed, ref } from 'vue';
import { defineStore } from "pinia";
import { MAP_TYPES } from "@/models/mapTypes.js";
import mapService from "@/services/mapService.js";
import { useFilterStore } from "@/stores/filter.js";

const DEFAULT_COORDINATES = [51.769406790090855, 19.43750792680422];
const DEFAULT_ZOOM = 13;

const filterStore = useFilterStore();

export const useMapStore = defineStore(
    'map', () => {
        const _map = ref(null);
        const _tileLayer = ref(null);
        const _center = ref(DEFAULT_COORDINATES);
        const _zoom = ref(DEFAULT_ZOOM);
        const _geohash = ref(null);
        const _bbox = ref(null);
        const _data = ref([]);

        const map = computed(() => _map.value);
        const tileLayer = computed(() => _tileLayer.value);
        const center = computed(() => _center.value);
        const geohash = computed(() => _geohash.value);
        const bbox = computed(() => _bbox.value);
        const zoom = computed(() => _zoom.value);
        const data = computed(() => _data.value);

        async function loadData() {
            if (filterStore.mapType === MAP_TYPES.EVENT) {
                const startDate= filterStore.startDate;
                const endDate = filterStore.endDate;
                const events = await mapService.getEvents(_geohash.value, _bbox.value, startDate, endDate);
                _data.value = events;
            }
            else {
                const places = await mapService.getPlaces(_geohash.value, _bbox.value);
                _data.value = places;
            }
        }
        
        function clear() {
            _data.value = [];
            _geohash.value = null;
        }

        function destroy() {
            _map.value.remove();
            _center.value = DEFAULT_COORDINATES;
            clear();
        }

        function setMap(map) {
            _map.value = map;
        }

        function setTileLayer(tileLayer) {
            _tileLayer.value = tileLayer;
            _tileLayer.value.addTo(_map.value);
        }
        
        function setCenter(center) {
            _center.value = center;
        }
        
        function setGeohash(geohash) {
            _geohash.value = geohash;
        }

        function setBbox(bbox) {
            _bbox.value = bbox;
        }
        
        function setZoom(zoom) {
            _zoom.value = zoom;
        }

        return {
            map,
            tileLayer,
            center,
            zoom,
            data,
            geohash,
            bbox,
            destroy,
            loadData,
            clear,
            setMap,
            setTileLayer,
            setCenter,
            setGeohash,
            setZoom,
            setBbox
        };
    });