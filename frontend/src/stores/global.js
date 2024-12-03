import { computed, ref } from 'vue';
import { defineStore } from "pinia";
import citiesService from "@/services/citiesService.js";

const DEFAULT_CITY_NAME = 'Łódź';

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

            _city.value = _cities.value.find(c => c.name === DEFAULT_CITY_NAME);
            
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