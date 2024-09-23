import { computed, ref } from 'vue';
import { defineStore } from "pinia";
import placesService from "../services/placesService";

export const usePlacesStore = defineStore(
    'areas', () => {
    const _areas = ref([]);
    const areas = computed(() => _areas.value);
    async function loadAll() {
        const result = await placesService.getAll();

        if (result.data && result.data.length > 0) {
            result.data.forEach((a) => {
                a.Polygon = JSON.parse(a.Polygon);
                a.Coordinates = { latitude: a.Latitude, longitude: a.Longitude };
            });
        }

        _areas.value = result.data;
    }

    return { 
        _areas, 
        areas,
        loadAll
    };
});