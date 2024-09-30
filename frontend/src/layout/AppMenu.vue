<script setup>
import { ref } from 'vue';
import AppMenuItem from './AppMenuItem.vue';
import { FontAwesomeIcon } from "@fortawesome/vue-fontawesome";
import { useGlobalStore } from "@/stores/global.js";
import { menuItems } from "@/layout/menuItems.js";
import { storeToRefs } from "pinia";

const model = ref(menuItems);
const globalStore = useGlobalStore();
const { cities, city, isLoading } = storeToRefs(globalStore);

const onCityUpdate = (value) => {
    globalStore.setCity(value);
    globalStore.setCenter(value.latitude, value.longitude);
};

</script>

<template>
    <div class="relative flex align-items-center w-full pt-2">
        <font-awesome-icon class="absolute z-5 pl-3" icon="fas fa-location-dot" size="lg"></font-awesome-icon>
        <Dropdown 
            v-if="!isLoading"
            class="w-full px-2"
            v-model="city"
            :options="cities"
            :virtual-scroller-options="{ itemSize: 50 }"
            input-class="ml-4"
            filter 
            placeholder="Select a City"
            option-label="name" 
            @update:modelValue="onCityUpdate">
            <template #value="{ value }">
                <div v-if="value" class="flex align-items-center">
                    <div>{{ value.name }}</div>
                </div>
            </template>
            <template #option="{ option }">
                <div class="flex flex-column justify-content-center gap-2">
                    <div class="font-medium text-sm">
                        <LongText :text="option.name">{{ option.name }}</LongText>
                    </div>
                    <div class="text-xs font-light">
                        <LongText :text="option.province">{{ option.province }}</LongText>
                    </div>
                </div>
            </template>
        </Dropdown>
        <Skeleton v-else class="w-full" height="3rem"></Skeleton>
    </div>
    <ul class="layout-menu">
        <template v-for="(item, i) in model" :key="item">
            <app-menu-item v-if="!item.separator" :item="item" :index="i"></app-menu-item>
            <li v-if="item.separator" class="menu-separator"></li>
        </template>
    </ul>
</template>

<style lang="scss" scoped></style>
