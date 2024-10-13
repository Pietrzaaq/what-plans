import { computed, ref } from 'vue';
import { defineStore } from "pinia";

const DEFAULT_COORDINATES = [51.769406790090855, 19.43750792680422];
const DEFAULT_ZOOM = 15;

export const useMapStore = defineStore(
    'global', () => {
        const _center = ref(DEFAULT_COORDINATES);
        const _zoom = ref(DEFAULT_ZOOM);

        const center = computed(() => _center.value);
        const zoom = computed(() => _zoom.value);

        async function initialize() {
        }

        return {
            center,
            zoom,
            initialize,
        };
    });