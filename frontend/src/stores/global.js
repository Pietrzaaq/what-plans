import { computed, ref } from 'vue';
import { defineStore } from "pinia";
import citiesService from "@/services/citiesService.js";
import { LOCAL_STORAGE_KEYS } from "@/models/localStorage.js";

const DEFAULT_CITY_NAME = 'Warszawa';

export const useGlobalStore = defineStore(
    'global', () => {
        const _isLoading = ref(true);
        const _city = ref();
        const _cities = ref([]);
        const _searchItem = ref(null);

        const isLoading = computed(() => _isLoading.value);
        const city = computed(() => _city.value);
        const cities = computed(() => _cities.value);
        const searchItem = computed(() => _searchItem.value);

        async function initialize() {
            _cities.value = await citiesService.getAll();

            const cityFromLocalStorge = JSON.parse(localStorage.getItem(LOCAL_STORAGE_KEYS.CITY));
            if (!cityFromLocalStorge) 
                _city.value = _cities.value.find(c => c.name === DEFAULT_CITY_NAME);
            else {
                const city = _cities.value.find(c => c.name === cityFromLocalStorge.name);
                if (city)
                    _city.value = city;
            }

            _isLoading.value = false;
        }

        async function setCity(city) {
            _city.value = city;
        }
        
        function setSearchItem(item) {
            _searchItem.value = item;
        }

        return {
            isLoading,
            city,
            cities,
            searchItem,
            initialize,
            setCity,
            setSearchItem
        };
    });