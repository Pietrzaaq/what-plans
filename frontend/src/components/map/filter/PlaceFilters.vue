<script setup>

import { EVENT_TYPE_OPTIONS } from "@/stores/filter.js";
import { FontAwesomeIcon } from "@fortawesome/vue-fontawesome";
import { ref } from "vue";

const selectedPlaceTypes = ref([]);

function onPlaceTypesChange(value) {
    
}
</script>

<template>
    <div class="flex flex-column gap-2">
        <div>
            <div class="flex align-items-center gap-2 ml-2 pb-2">
                <font-awesome-icon icon="fas fa-hand-pointer"></font-awesome-icon>
                <label for="dateRangePicker" class="font-bold block"> Types </label>
            </div>
            <MultiSelect v-model="selectedPlaceTypes"
                         :options="EVENT_TYPE_OPTIONS"
                         optionLabel="name"
                         placeholder="Select place types"
                         display="chip"
                         class="w-full" 
                         @update:modelValue="onPlaceTypesChange">
                <template #option="{ option }">
                    <div class="flex align-items-center gap-2">
                        <font-awesome-icon :icon="`fas fa-${option.icon}`"
                                           :style="{color: option.color}"></font-awesome-icon>
                        <div>{{ option.name }}</div>
                    </div>
                </template>
                <template #footer>
                    <div class="py-2 px-3">
                        <b>{{ selectedPlaceTypes ? selectedPlaceTypes.length : 0 }}</b>
                        item{{ (selectedPlaceTypes ? selectedPlaceTypes.length : 0) > 1 ? 's' : '' }} selected.
                    </div>
                </template>
            </MultiSelect>
        </div>
    </div>
</template>

<style scoped>

</style>