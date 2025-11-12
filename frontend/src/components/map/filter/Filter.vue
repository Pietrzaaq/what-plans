<script setup>
import { computed, onMounted, ref, watch } from 'vue';
import { FontAwesomeIcon } from "@fortawesome/vue-fontawesome";
import { useFilterStore } from "@/stores/filter.js";
import { MAP_TYPES } from "@/models/mapTypes.js";
import { storeToRefs } from "pinia";
import EventFilters from "@/components/map/filter/EventFilters.vue";
import PlaceFilters from "@/components/map/filter/PlaceFilters.vue";

const filterStore = useFilterStore();
const { mapType  } = storeToRefs(filterStore);

const mapTypeOptions = ref([
    { name: 'Events', value: MAP_TYPES.EVENT },
    { name: 'Places', value: MAP_TYPES.PLACE }
]);

const selectedMapType = ref();
const dates = ref([]);

const selectedMapTypeValue = computed(() => selectedMapType?.value?.value);

watch(dates, (newVal) => {
    console.log(newVal);
});

function onMapTypeChange(event) {
    selectedMapType.value = event.value;
    filterStore.setMapType(event.value.value);
}

onMounted(() => {
    if (mapType.value)
        selectedMapType.value = mapTypeOptions.value.find(o => o.value === mapType.value);
});
</script>

<template>
    <Accordion class="map-filter flex flex-column gap-2 absolute">
        <AccordionTab>
            <template #header>
                <div class="flex flex-wrap align-items-center gap-2">
                    <FontAwesomeIcon icon="fas fa-filter" size="lg"></FontAwesomeIcon>
                    <span class="font-medium">Filters</span>
                    <SelectButton v-model="selectedMapType"
                                  :options="mapTypeOptions"
                                  :allow-empty="false"
                                  optionLabel="name"
                                  aria-labelledby="multiple"
                                  @change="onMapTypeChange"
                                  @click.stop/>
                </div>
            </template>
            <EventFilters v-if="selectedMapTypeValue === MAP_TYPES.EVENT"></EventFilters>
            <PlaceFilters v-else></PlaceFilters>
        </AccordionTab>
    </Accordion>

</template>

<style>
.map-filter {
    z-index: 400;
    width: 400px;
    top: 6rem;
    left: 250px;
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

.p-selectbutton .p-button.p-highlight {
    background: var(--primary-500);
    color: white !important;
}
</style>