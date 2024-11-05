import { computed, ref } from 'vue';
import { defineStore } from "pinia";
import { MAP_TYPES } from "@/models/mapTypes.js";
import { EVENT_TYPES_DATA } from "@/models/eventTypes.js";
import { FILTER_DATE_TYPE } from "@/components/map/filter/filterDateType.js";
import moment from "moment";

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
        const _dateType = ref(FILTER_DATE_TYPE.WEEKEND);
        const dateType = computed(() => _dateType.value);
        const _startDate = ref(null);
        const startDate = computed(() => _startDate.value);
        const _endDate = ref(null);
        const endDate = computed(() => _endDate.value);

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

            const dateType = JSON.parse(localStorage.getItem('dateType'));
            if (!dateType) {
                const newValue = FILTER_DATE_TYPE.WEEKEND;
                _dateType.value = newValue;
                localStorage.setItem('dateType', JSON.stringify(newValue));
            }
            else
                _dateType.value = dateType;
            
            const startDate = JSON.parse(localStorage.getItem('startDate'));
            const endDate = JSON.parse(localStorage.getItem('startDate'));
            if (!startDate) {
                const startOfWeek = moment().startOf('week').toDate();
                const endOfWeek = moment().endOf('week').toDate();
                _startDate.value = startOfWeek;
                _endDate.value = endOfWeek;
                localStorage.setItem('startDate', JSON.stringify(startOfWeek));
                localStorage.setItem('endDate', JSON.stringify(endOfWeek));
            }
            else {
                _startDate.value = startDate;
                _endDate.value = endDate;
            }
                _dateType.value = dateType;
        }
        
        function setMapType(mapType) {
            _mapType.value = mapType;
            localStorage.setItem('mapType', JSON.stringify(_mapType.value));
        }

        function setEventTypes(eventTypeIds) {
            _eventTypes.value = eventTypeIds;
            localStorage.setItem('eventTypes', JSON.stringify(_eventTypes.value));
        }
        
        function setDateType(type) {
            _dateType.value = type;
            localStorage.setItem('dateType', JSON.stringify(_dateType.value));
        }
        
        function setDates(startDate, endDate) {
            _startDate.value = startDate;
            _endDate.value = endDate;

            localStorage.setItem('startDate', JSON.stringify(_startDate.value));
            localStorage.setItem('endDate', JSON.stringify(_endDate.value));
        }

        return {
            mapType,
            eventTypes,
            dateType,
            startDate,
            endDate,
            loadFilter,
            setMapType,
            setEventTypes,
            setDateType,
            setDates
        };
    });