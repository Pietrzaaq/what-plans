import { computed, ref } from 'vue';
import { defineStore } from "pinia";
import { MAP_TYPES } from "@/models/mapTypes.js";
import { EVENT_TYPES_DATA } from "@/models/eventTypes.js";

export const EVENT_TYPE_OPTIONS = Object.keys(EVENT_TYPES_DATA).map(key => ({
    value: key,
    name: EVENT_TYPES_DATA[key].name,
    icon: EVENT_TYPES_DATA[key].icon,
    color: EVENT_TYPES_DATA[key].markerColor
}));

export const useFilterStore = defineStore(
    'filter', () => {
        const _mapType = ref(MAP_TYPES.EVENT);
        const mapType = computed(() => _mapType.value);
        const _eventTypes = ref([]);
        const eventTypes = computed(() => _eventTypes.value);

        async function loadFilter() {
            const mapType = JSON.parse(localStorage.getItem('mapType'));

            if (!mapType) {
                const newValue = MAP_TYPES.EVENT;
                _mapType.value = newValue;  
                localStorage.setItem('mapType', JSON.stringify(newValue));
            }
            else 
                _mapType.value = mapType;

            const eventTypes = JSON.parse(localStorage.getItem('eventTypes'));

            if (!eventTypes) {
                const newValue = EVENT_TYPE_OPTIONS.map(t => t.value);
                _eventTypes.value = newValue;
                localStorage.setItem('eventTypes', JSON.stringify(newValue));
            }
            else
                _eventTypes.value = eventTypes;
        }
        
        function setMapType(mapType) {
            _mapType.value = mapType;
            localStorage.setItem('mapType', JSON.stringify(_mapType.value));
        }

        function setEventTypes(eventTypeIds) {
            _eventTypes.value = eventTypeIds;
            localStorage.setItem('eventTypes', JSON.stringify(_eventTypes.value));
        }

        return {
            mapType,
            eventTypes,
            loadFilter,
            setMapType,
            setEventTypes
        };
    });