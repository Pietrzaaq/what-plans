import { computed, ref } from 'vue';
import { defineStore } from "pinia";
import eventsService from "@/services/eventsService";

export const useEventsStore = defineStore(
    'events', () => {
    const _events = ref([]);
    const events = computed(() => _events.value);
    async function loadAll() {
        const result = await eventsService.getAll();

        _events.value = result.data;
    }

    return { 
        _events, 
        events,
        loadAll
    };
});