import { computed, ref } from 'vue';
import { defineStore } from "pinia";
import placesService from "@/services/placesService";

export const usePlacesStore = defineStore(
    'places', () => {
    const _places = ref([]);
    const places = computed(() => _places.value);
    async function loadAll() {
        _places.value = await placesService.getAll();
    }
    
    async function loadAllWithEvents() {
        const result = await placesService.getAllWithEvents();

        _places.value = result.data;
    }
    
    function clear() {
        _places.value = [];
    }

    return { 
        _places, 
        places,
        loadAll,
        loadAllWithEvents,
        clear
    };
});