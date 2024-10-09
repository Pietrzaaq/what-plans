import { computed, ref } from 'vue';
import { defineStore } from "pinia";
import placesService from "@/services/placesService";

export const usePlacesStore = defineStore(
    'places', () => {
    const _places = ref([]);
    const places = computed(() => _places.value);
    async function loadAll() {
        const result = await placesService.getAll();

        _places.value = result.data;
    }
    
    async function loadAllWithEvents() {
        const result = await placesService.getAllWithEvents();

        _places.value = result.data;
    }

    return { 
        _places, 
        places,
        loadAll,
        loadAllWithEvents
    };
});