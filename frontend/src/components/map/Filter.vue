<script setup>
import { ref, onMounted, watch } from 'vue';
import moment from 'moment';
import { FontAwesomeIcon } from "@fortawesome/vue-fontawesome";
import { EVENT_TYPES_DATA } from "@/models/eventTypes.js";

const selectMapTypeOptions = ref([
    { name: 'Events', value: 1 },
    { name: 'Places', value: 2 }
]);
const selectedMapTypes = ref([selectMapTypeOptions.value[0]]);

const dates = ref([]);
const selectedEventTypes = ref([]);
const eventTypeOptions = Object.keys(EVENT_TYPES_DATA).map(key => ({
    value: key,
    name: EVENT_TYPES_DATA[key].name,
    icon: EVENT_TYPES_DATA[key].icon,
    color: EVENT_TYPES_DATA[key].markerColor
}));

watch(dates, (newVal) => {
    console.log(newVal);
});

onMounted(() => {
    const startDate = moment(new Date()).add(-5, 'days').toDate();
    const endDate = moment(new Date()).add(5, 'days').toDate();
    dates.value = [startDate, endDate];
});
</script>

<template>
    <Accordion :activeIndex="0" class="flex flex-column gap-2 absolute" style="width: 400px; top: 100px; left: 350px; border-radius: 10px">
        <AccordionTab>
            <template #header>
                <div class="flex align-items-center gap-2">
                    <FontAwesomeIcon icon="fas fa-filter" size="lg"></FontAwesomeIcon>
                    <span>Filters</span>
                    <SelectButton v-model="selectedMapTypes" :options="selectMapTypeOptions" optionLabel="name" multiple aria-labelledby="multiple" />
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
                    <MultiSelect v-model="selectedEventTypes" :options="eventTypeOptions" optionLabel="name" placeholder="Select event types" display="chip" class="w-full">
                        <template #option="{ option }">
                            <div class="flex align-items-center gap-2">
                                <font-awesome-icon :icon="`fas fa-${option.icon}`" :style="{color: option.color}"></font-awesome-icon>
                                <div>{{ option.name }}</div>
                            </div>
                        </template>
                        <template #footer>
                            <div class="py-2 px-3">
                                <b>{{ selectedEventTypes ? selectedEventTypes.length : 0 }}</b> item{{ (selectedEventTypes ? selectedEventTypes.length : 0) > 1 ? 's' : '' }} selected.
                            </div>
                        </template>
                    </MultiSelect>
                </div>
            </div>
        </AccordionTab>
    </Accordion>

</template>

<style scoped>

</style>