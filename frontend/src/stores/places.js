import { computed, ref } from 'vue';
import { defineStore } from "pinia";
import placesService from "@/services/placesService";

export const usePlacesStore = defineStore(
    'places', () => {
    const _places = ref([]);
    const places = computed(() => _places.value);
    async function loadAll(includeEvents) {
        const result = await placesService.getAllWithEvents(includeEvents);

        _places.value = result.data;
    }

    return { 
        _places, 
        places,
        loadAll
    };
});