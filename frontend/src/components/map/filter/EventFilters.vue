<script setup>
import { EVENT_TYPE_OPTIONS, useFilterStore } from "@/stores/filter.js";
import { FontAwesomeIcon } from "@fortawesome/vue-fontawesome";
import { computed, nextTick, onMounted, ref } from "vue";
import { FILTER_DATE_TYPE } from "@/components/map/filter/filterDateType.js";
import { storeToRefs } from "pinia";
import moment from "moment";
import { getUniqueString } from "@/helpers/helpers.js";

const filterStore = useFilterStore();
const { eventTypes, startDate, endDate, dateType } = storeToRefs(filterStore);
const selectedEventTypes = ref([]);

const weekElementId = getUniqueString();
const monthElementId = getUniqueString();
const isMobileView = ref(false);
const isUpdating = ref(false);

const dates = ref([]);
const weekNumber = ref(null);
const monthNumber = ref(null);
const buttonsDirection = ref('horizontal');
const filterDateType = ref(FILTER_DATE_TYPE.WEEKEND);

const weekPlaceholder = computed(() => {
    const startOfWeek = moment(startDate.value).startOf('week').format('DD-MM');
    const endOfWeek = moment(startDate.value).endOf('week').format('DD-MM-YYYY');
    
    const isCurrentWeek = moment().startOf('week').isSame(moment(startDate.value).startOf('week'), 'week');
    
    return `${isCurrentWeek ? '(Current) ' : ''} ${startOfWeek} - ${endOfWeek}`;
});

const monthPlaceholder = computed(() => {
    const month = moment(startDate.value).startOf('month').format('MMMM YYYY');

    const isCurrentMonth = moment().startOf('month').isSame(moment(startDate.value).startOf('month'), 'month');

    return `${isCurrentMonth ? '(Current) ' : ''} ${month}`;
});

function onEventTypesChange(eventTypes) {
    const eventTypeIds = eventTypes.map(t => t.value);
    filterStore.setEventTypes(eventTypeIds);
}

function onWeekUpdate(value) {
    weekNumber.value = null;
    let el = document.getElementById(weekElementId);
    if (el) {
        el.blur();
    }

    if (isUpdating.value)
        return;
    isUpdating.value = true;
    
    const newStartOfWeek = moment(startDate.value).startOf('week').add(value, 'weeks').toDate();
    const newEndOfWeek = moment(startDate.value).endOf('week').add(value, 'weeks').toDate();

    filterStore.setDates(newStartOfWeek, newEndOfWeek);
    
    nextTick(() => { isUpdating.value = false; });
}

function onMonthUpdate(value) {
    monthNumber.value = null;
    let el = document.getElementById(monthElementId);
    if (el) {
        el.blur();
    }

    if (isUpdating.value)
        return;
    isUpdating.value = true;

    const newStartOfMonth = moment(startDate.value).startOf('month').add(value, 'month').toDate();
    const newEndOfMonth = moment(startDate.value).endOf('month').add(value, 'month').toDate();

    filterStore.setDates(newStartOfMonth, newEndOfMonth);

    nextTick(() => { isUpdating.value = false; });
}

function onSwitchToWeekend() {
    const newStartOfWeek = moment(startDate.value).startOf('week').toDate();
    const newEndOfWeek = moment(startDate.value).endOf('week').toDate();
    filterStore.setDates(newStartOfWeek, newEndOfWeek);
    dates.value = [newStartOfWeek, newEndOfWeek];
    filterStore.setDateType(FILTER_DATE_TYPE.WEEKEND);
}

function onSwitchToMonth() {
    const newStartOfMonth = moment(startDate.value).startOf('month').toDate();
    const newEndOfMonth = moment(startDate.value).endOf('month').toDate();
    filterStore.setDates(newStartOfMonth, newEndOfMonth);
    dates.value = [newStartOfMonth, newEndOfMonth];
    filterStore.setDateType(FILTER_DATE_TYPE.MONTH);
}

function onSwitchToDateRange() {
    filterStore.setDates(startDate.value, endDate.value);
    dates.value = [startDate.value, endDate.value];
    filterStore.setDateType(FILTER_DATE_TYPE.RANGE);
}

const checkViewWidth = () => {
  isMobileView.value = window.innerWidth < 990;
  if (isMobileView.value) 
      buttonsDirection.value = 'vertical';
  else 
      buttonsDirection.value = 'horizontal';
};

onMounted(() => {
    console.log('On mounted', dateType.value, startDate.value, endDate.value);
    
    checkViewWidth();
    window.addEventListener("resize", checkViewWidth);
    
    if (dateType.value)
        filterDateType.value = dateType.value;
    
    if (startDate.value && endDate.value)
        dates.value = [startDate.value, endDate.value];
    else {
        const startOfWeek = moment().startOf('week').toDate();
        const endOfWeek = moment().endOf('week').toDate();
        dates.value = [startOfWeek, endOfWeek];
        
        filterStore.setDates(startOfWeek, endOfWeek);
    }
    
    if (eventTypes.value)
        selectedEventTypes.value = EVENT_TYPE_OPTIONS.filter(t => eventTypes.value.includes(t.value));

    console.log('On mounted', dates.value);
    
});

</script>

<template>
    <div class="flex flex-column gap-2">
        <div>
            <div class="flex align-items-center gap-2 ml-2 pb-2">
                <font-awesome-icon icon="fas fa-calendar-days"></font-awesome-icon>
                <label for="dateRangePicker" class="font-bold block"> Dates </label>
            </div>
            <div class="flex flex-wrap align-items-center gap-2 py-4">
                <div class="flex items-center gap-2">
                    <RadioButton v-model="filterDateType" inputId="weekend" :value="FILTER_DATE_TYPE.WEEKEND" @click="onSwitchToWeekend" />
                    <div class="py-1">Weekend</div>
                </div>
                <div class="flex items-center gap-2">
                    <RadioButton v-model="filterDateType" inputId="month" :value="FILTER_DATE_TYPE.MONTH" @click="onSwitchToMonth"/>
                    <div class="py-1">Month</div>
                </div>
                <div class="flex items-center gap-2">
                    <RadioButton v-model="filterDateType" inputId="range" :value="FILTER_DATE_TYPE.RANGE" @click="onSwitchToDateRange"/>
                    <div class="py-1">Date Range</div>
                </div>
            </div>
            <InputNumber v-if="filterDateType === FILTER_DATE_TYPE.WEEKEND"
                         v-model="weekNumber"
                         class="w-full"
                         showButtons
                         :buttonLayout="buttonsDirection"
                         :input-id="weekElementId"
                         :placeholder="weekPlaceholder"
                         @update:modelValue="onWeekUpdate">
                <template #incrementbuttonicon>
                    <span class="pi pi-plus"/>
                </template>
                <template #decrementbuttonicon>
                    <span class="pi pi-minus"/>
                </template>
            </InputNumber>
            <InputNumber v-else-if="filterDateType === FILTER_DATE_TYPE.MONTH"
                         v-model="monthNumber"
                         class="w-full"
                         showButtons
                         :buttonLayout="buttonsDirection"
                         :input-id="monthElementId"
                         :placeholder="monthPlaceholder"
                         @update:modelValue="onMonthUpdate">
                <template #incrementbuttonicon>
                    <span class="pi pi-plus"/>
                </template>
                <template #decrementbuttonicon>
                    <span class="pi pi-minus"/>
                </template>
            </InputNumber>
            <Calendar v-else v-model="dates" showIcon selectionMode="range" iconDisplay="input" input-id="dateRangePicker" class="w-full" />
        </div>
        <div>
            <div class="flex flex-nowrap align-items-center gap-2 ml-2 pb-2">
                <font-awesome-icon icon="fas fa-hand-pointer"></font-awesome-icon>
                <label for="dateRangePicker" class="font-bold block"> Types </label>
            </div>
            <MultiSelect v-model="selectedEventTypes"
                         :options="EVENT_TYPE_OPTIONS"
                         optionLabel="name"
                         placeholder="Select event types"
                         display="chip"
                         class="w-full" 
                         @update:modelValue="onEventTypesChange">
                <template #option="{ option }">
                    <div class="flex align-items-center gap-2">
                        <font-awesome-icon :icon="`fas fa-${option.icon}`"
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
</template>

<style scoped>
.p-radiobutton:not(.p-radiobutton-disabled):hover {
    box-shadow: 0 0 1px 5px rgba(0, 0, 0, 0.04);  
}

:deep(.p-inputnumber) .p-button {
    background-color: var(--gray-300);
    color: var(--text-color);
}

:deep(.p-inputnumber-input) {
    pointer-events: none !important; 
}
</style>