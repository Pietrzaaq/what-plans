<script setup>
import { ref, onMounted, watch } from 'vue';
import moment from 'moment';
import { FontAwesomeIcon } from "@fortawesome/vue-fontawesome";

const selectMapTypeOptions = ref([
    { name: 'Events', value: 1 },
    { name: 'Places', value: 2 }
]);
const selectedMapTypes = ref([selectMapTypeOptions.value[0]]);

const dates = ref([]);

watch(dates, (newVal) => {
    console.log(newVal);
});

// On component mount, set the dates
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
            <div>
                
                <div>
                    <Calendar v-model="dates" showIcon selectionMode="range" iconDisplay="input" class="w-full" />
                </div>
            </div>
        </AccordionTab>
    </Accordion>

</template>

<style scoped>

</style>