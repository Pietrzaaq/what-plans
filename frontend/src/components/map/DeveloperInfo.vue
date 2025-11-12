<script setup>
import { useMapStore } from "@/stores/map.js";
import { computed } from "vue";
import geolocationService from "@/services/geolocationService.js";
import { useGlobalStore } from "@/stores/global.js";

const mapStore = useMapStore();
const globalStore = useGlobalStore();

const isProductionEnvironment = import.meta.env.NODE_ENV === 'production';

const center = computed(() => {
    const lat = mapStore.center[0]?.toString().slice(0, 5);
    const long = mapStore.center[1]?.toString().slice(0, 5);
    return `${lat}, ${long}`; 
});

const geohash = computed(() => {
    const lat = mapStore.center[0];
    const long = mapStore.center[1];
    return geolocationService.getGeohash(lat, long);
});

const bounds = computed(() => {
    if (!mapStore.map || !mapStore.center)
        return;
    
    const bounds = mapStore.map.getBounds();
    return [
        bounds.getNorthEast().lat.toString().slice(0, 4),
        bounds.getNorthEast().lng.toString().slice(0, 4),
        bounds.getSouthWest().lat.toString().slice(0, 4),
        bounds.getSouthWest().lng.toString().slice(0, 4),
    ];
    
});
</script>

<template>
    <div v-if="!isProductionEnvironment" class="wp-developer-info absolute">
        <div class="flex flex-column gap-2 mt-2 p-2">
            <span class="text-center font-bold">INFO</span>
            <div class="flex">
                <div class="w-6">
                    <div>Center:</div>
                    <div class="font-bold">{{ center }}</div>
                </div>
                <div class="w-6">
                    <div>Zoom:</div>
                    <div class="font-bold">{{ mapStore.zoom }}</div>
                </div>
            </div>
            <div>
                <div>Geohash:</div>
                <div class="font-bold">{{ geohash }}</div>
            </div>
            <div>
                <div>Bounds:</div>
                <div class="font-bold">{{ bounds }}</div>
            </div>
            <div>
                <div>City:</div>
                <div class="font-bold">{{ globalStore.city.name }}</div>
            </div>
        </div>
    </div>
</template>

<style scoped>
.wp-developer-info {
    z-index: 400;
    right: 0rem;
    bottom: 0rem;
    height: 15rem;
    width: 15rem;
    background-color: rgba(128, 128, 128, 0.8);
}
</style>