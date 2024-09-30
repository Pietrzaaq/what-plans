import { computed, ref } from 'vue';
import { defineStore } from "pinia";
import axios from "axios";

const DEFAULT_COORDINATES = [51.769406790090855, 19.43750792680422];
const DEFAULT_CITY_ID = '19677';

export const useGlobalStore = defineStore(
    'global', () => {
        const _isLoading = ref(true);
        const _city = ref();
        const _cities = ref([]);
        const _center = ref(DEFAULT_COORDINATES);

        const isLoading = computed(() => _isLoading.value);
        const city = computed(() => _city.value);
        const cities = computed(() => _cities.value);
        const center = computed(() => _center.value);

        async function initialize() {
            const result = await axios.get('/data/cities.json', { baseURL: '/' });
            _cities.value = result.data;

            _city.value = _cities.value.find(c => c.id === DEFAULT_CITY_ID);
            
            _isLoading.value = false;
        }

        async function setCity(city) {
            _city.value = city;
        }
        
        async function setCenter(lat, long) {
            _center.value = [lat, long];
        }
        
        return {
            isLoading,
            city,
            cities,
            center,
            initialize,
            setCenter,
            setCity
        };
    });