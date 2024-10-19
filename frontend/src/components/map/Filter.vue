<script setup>
import { onMounted, ref, watch } from 'vue';
import moment from 'moment';
import { FontAwesomeIcon } from "@fortawesome/vue-fontawesome";
import { EVENT_TYPE_OPTIONS, useFilterStore } from "@/stores/filter.js";
import { MAP_TYPES } from "@/models/mapTypes.js";
import { storeToRefs } from "pinia";

const filterStore = useFilterStore();
const { mapType, eventTypes } = storeToRefs(filterStore);

const mapTypeOptions = ref([
    { name: 'Events', value: MAP_TYPES.EVENT },
    { name: 'Places', value: MAP_TYPES.PLACE }
]);

const selectedMapTypes = ref();
const dates = ref([]);
const selectedEventTypes = ref(EVENT_TYPE_OPTIONS);

watch(dates, (newVal) => {
    console.log(newVal);
});

function onMapTypeChange(event) {
    selectedMapTypes.value = event.value;
    filterStore.setMapType(event.value.value);
}

function onEventTypesChange(eventTypes) {
    const eventTypeIds = eventTypes.map(t => t.value);
    filterStore.setEventTypes(eventTypeIds);
}

onMounted(() => {
    const startDate = moment(new Date()).add(-5, 'days').toDate();
    const endDate = moment(new Date()).add(5, 'days').toDate();
    dates.value = [startDate, endDate];
    
    if (mapType.value)
        selectedMapTypes.value = mapTypeOptions.value.find(o => o.value === mapType.value); 
    if (eventTypes.value) {
        selectedEventTypes.value = EVENT_TYPE_OPTIONS.filter(t => eventTypes.value.includes(t.value));
    }
});
</script>

<template>
    <Accordion class="map-filter flex flex-column gap-2 absolute ">
        <AccordionTab>
            <template #header>
                <div class="flex flex-wrap align-items-center gap-2">
                    <FontAwesomeIcon icon="fas fa-filter" size="lg"></FontAwesomeIcon>
                    <span class="font-medium">Filters</span>
                    <SelectButton
                        v-model="selectedMapTypes"
                        :options="mapTypeOptions"
                        :allow-empty="false"
                        optionLabel="name"
                        aria-labelledby="multiple"
                        @change="onMapTypeChange"
                        @click.stop/>
                </div>
            </template>
            <div class="flex flex-column gap-2">
                <div>
                    <div class="flex align-items-center gap-2 ml-2 pb-2">
                        <font-awesome-icon icon="fas fa-calendar-days"></font-awesome-icon>
                        <label for="dateRangePicker" class="font-bold block"> Dates </label>
                    </div>
                    <Calendar v-model="dates" showIcon selectionMode="range" iconDisplay="input" input-id="dateRangePicker" class="w-full" />
                </div>
                <div>
                    <div class="flex align-items-center gap-2 ml-2 pb-2">
                        <font-awesome-icon icon="fas fa-hand-pointer"></font-awesome-icon>
                        <label for="dateRangePicker" class="font-bold block"> Types </label>
                    </div>
                    <MultiSelect
                        v-model="selectedEventTypes"
                        :options="EVENT_TYPE_OPTIONS"
                        optionLabel="name"
                        placeholder="Select event types"
                        display="chip"
                        class="w-full" @update:modelValue="onEventTypesChange">
                        <template #option="{ option }">
                            <div class="flex align-items-center gap-2">
                                <font-awesome-icon
                                    :icon="`fas fa-${option.icon}`"
                                    :style="{color: option.color}"></font-awesome-icon>
                                <div>{{ option.name }}</div>
                            </div>
                        </template>
                        <template #footer>
                            <div class="py-2 px-3">
                                <b>{{ selectedEventTypes ? selectedEventTypes.length : 0 }}</b>
                                item{{ (selectedEventTypes ? selectedEventTypes.length : 0) > 1 ? 's' : '' }} selected.
                            </div>
                        </template>
                    </MultiSelect>
                </div>
            </div>
        </AccordionTab>
    </Accordion>

</template>

<style>
.map-filter {
    z-index: 400;
    width: 400px;
    top: 6rem;
    left: 350px;
}

@media (max-width: 990px) {
    .map-filter {
        left: 1rem;
        max-width: 15rem;
    }
}

.map-filter .p-accordion-tab {
    overflow: hidden;
    border-radius: 1rem;
}

.map-filter .p-accordion-tab .p-accordion-header .p-accordion-header-link {
    padding: 0.75rem 1rem;
}
</style>