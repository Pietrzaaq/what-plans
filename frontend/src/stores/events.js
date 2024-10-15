import { computed, ref } from 'vue';
import { defineStore } from "pinia";
import eventsService from "@/services/eventsService";

export const useEventsStore = defineStore(
    'events', () => {
    const _events = ref([]);
    const events = computed(() => _events.value);
    async function loadAll() {
        _events.value = await eventsService.getAll();
    }

    return { 
        _events, 
        events,
        loadAll
    };
});