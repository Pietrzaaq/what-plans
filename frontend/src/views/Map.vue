<script setup>
import { onBeforeMount, onBeforeUnmount, onMounted, ref, watch } from 'vue';
import L from 'leaflet';
import 'leaflet/dist/leaflet.css';
import { usePopup } from "../composables/map/popup";
import EventDialog from "../components/map/EventDialog.vue";
import 'leaflet.awesome-markers/dist/leaflet.awesome-markers.css'; 
import 'leaflet.awesome-markers/dist/leaflet.awesome-markers.js';
import Filter from "../components/map/filter/Filter.vue";
import { useGlobalStore } from "@/stores/global.js";
import { storeToRefs } from "pinia";
import EventPopup from "@/components/map/popups/EventPopup.vue";
import PlacePopup from "@/components/map/popups/PlacePopup.vue";
import { useFilterStore } from "@/stores/filter.js";
import { MAP_TYPES } from "@/models/mapTypes.js";
import geolocationService from "@/services/geolocationService.js";
import { useMapStore } from "@/stores/map.js";
import { useMarker } from "@/composables/map/marker.js";

// STORES
const mapStore = useMapStore();
const { map, zoom, center, currentGeohashes, loadedGeohashes, geohashesToLoad, geohashPrecision } = storeToRefs(mapStore);

const filterStore = useFilterStore();
const { mapType, eventTypes, startDate } = storeToRefs(filterStore);

const globalStore = useGlobalStore();
const { city, searchItem } = storeToRefs(globalStore);

// FILTER
watch(mapType, async() => {
    mapStore.clear();
    await loadGeohashes();
});

watch(eventTypes, async() => {
    await loadMarkers();
});

watch(startDate, async() => {
    mapStore.clear();
    await loadGeohashes();
});

// CITY
watch(city, (newValue, oldValue) => {
    if (newValue && oldValue) {
        const center = [newValue.latitude, newValue.longitude];
        mapStore.clear();
        mapStore.setCenter(center);
        map.value.setView(center, zoom.value);
    }
});

// SEARCH 
watch(searchItem, newValue => {
    if (newValue) {
        globalStore.setSearchItem(null);
        const center = [newValue.latitude, newValue.longitude];
        mapStore.clear();
        mapStore.setCenter(center);
        mapStore.setZoom(16);
        map.value.setView(center, zoom.value);
    }
});

// DIALOG
const isDialogVisible = ref(false);
const dialogArea = ref(null);

function onDialogClosed() {
    dialogArea.value = null;
    isDialogVisible.value = false;

    loadMarkers();
}

function openEventDialog(area) {
    dialogArea.value = area;
    isDialogVisible.value = true;
}

// POPUP
const { teleportTo, isPlacePopupVisible, isEventPopupVisible, popupTargetObject, showPlacePopup, showEventPopup, hidePopup } = usePopup(map);

// MARKERS
const markerLayer = ref(null);
const { loadCitiesCircles, loadPlacesMarkers, loadEventsMarkers } = useMarker(markerLayer, map, showPlacePopup, showEventPopup, hidePopup, navigateToCity);

function navigateToCity(e) {
    console.log('Navigate', e);
}

async function loadMarkers() {
    if (markerLayer.value) {
        map.value.removeLayer(markerLayer.value);
        markerLayer.value = null;
    }
    markerLayer.value = L.layerGroup().addTo(map.value);

    if (geohashPrecision.value < 4) 
        loadCitiesCircles();
    else if (mapType.value === MAP_TYPES.EVENT)
        loadEventsMarkers();
    else
        loadPlacesMarkers();

    scaleIcon();
}

function scaleIcon() {
    const zoom = map.value.getZoom();
    const scale = Math.pow(0.9, 18 - zoom);

    document.querySelectorAll('.place-marker').forEach(function(marker) {
        const content = marker.querySelector('.place-marker-content');
        const label = marker.querySelector('.place-marker-label');
        const newSize = 50 * scale;
        const newFontSize = 25 * scale;
        const newLabelFontSize = 15 * scale;
        const newLineHeight = 15 * scale;

        content.style.width = newSize + 'px';
        content.style.height = newSize + 'px';
        content.style.fontSize = newFontSize + 'px';

        if (zoom < 14)
            label.style.display = 'none';
        else {
            label.style.display = 'flex';
            label.style.fontSize = newLabelFontSize + 'px';
            label.style.lineHeight = newLineHeight + 'px';
        }
    });
}

// MAP
async function OnMapChanged() {
    const newZoomValue = map.value.getZoom();
    if (zoom.value !== newZoomValue) {
        mapStore.setZoom(newZoomValue);
    }

    const newCenterValue = map.value.getCenter();
    if (center.value.lat !== newCenterValue.lat && center.value.lng !== newCenterValue.lng) {
        const newCenter = [newCenterValue.lat, newCenterValue.lng];
        mapStore.setCenter(newCenter);
    }
    
    await loadGeohashes();
}

async function loadGeohashes() {
    const precision = geolocationService.getGeohashPrecision(zoom.value);

    if (geohashPrecision.value !== precision) {
        mapStore.setGeohashPrecision(precision);
        mapStore.clear();
    }

    const bounds = map.value.getBounds();
    const currGeohashes = geolocationService.getBoundingBoxGeohashes(bounds, geohashPrecision.value);
    mapStore.setCurrentGeohashes(currGeohashes);

    const geoToLoad = currentGeohashes.value.filter(g => !loadedGeohashes.value.includes(g));
    mapStore.setGeohashesToLoad(geoToLoad);
    
    if (geohashesToLoad.value.length > 0) {
        await loadData();
    }
}

async function loadData() {
    let isModified;
    const dataCount = mapStore.data.length;

    await mapStore.loadData(mapType.value);

    isModified = dataCount !== mapStore.data.length;
    if (isModified)
        await loadMarkers();
}

function onMapLoad() {
    console.log('Map load!');
}

function onMapUnload() {
    console.log('Map unload!');
}

function onMapClick(e) {
    console.log('onMapClick', e.latlng.lat, e.latlng.lng);
}

onBeforeMount(async () => {
});

onMounted(async () => {
    await mapStore.initialize();
    
    map.value.on('load', onMapLoad);
    map.value.on('moveend', OnMapChanged);
    map.value.on('zoom', scaleIcon);
    map.value.on('click', onMapClick);
    map.value.on('unload', onMapUnload);

    await loadGeohashes();

    setTimeout(() => scaleIcon(), 100);
});

onBeforeUnmount(() => {
    mapStore.destroy();
});
</script>

<template>
    <main style="width: 100vw; height: 100vh">
        <div id="map" style="height: 100%"></div>
    </main>
    <Filter></Filter>
    <PlacePopup v-if="isPlacePopupVisible" 
                :teleportTo="teleportTo" 
                :popup-place="popupTargetObject" 
                @addEvent="openEventDialog"></PlacePopup>
    <EventPopup v-if="isEventPopupVisible"
                :teleportTo="teleportTo"
                :popup-place="popupTargetObject"
                @addEvent="openEventDialog"></EventPopup>
    <EventDialog :visible="isDialogVisible"
                 :area="dialogArea"
                 @close-dialog="onDialogClosed"></EventDialog>
</template>

<style>
.leaflet-container a {
    color: var(--text-color) !important;
}

.place-marker-content {
    position: absolute;
    display: flex;
    align-items: center;
    justify-content: center;
    top: 50%;
    left: 50%;
    width: 100%;
    height: 100%;
    transform: translate(-50%, -50%);
    border-radius: 50%;
    opacity: 0.9;
    border: 3px solid var(--gray-500);
    font-size: 25px;
    color: white;
    transition: width 100ms, height 100ms, font-size 50ms;
}

.place-marker:hover .place-marker-content {
    opacity: 1;
    border: 3px solid var(--primary-400);
}

.place-marker:hover .place-marker-label {
    color: var(--primary-400);
}

.place-marker-label {
    display: flex;
    font-size: 10px;
    height: 100%;
    align-items: center;
    text-align: left;
    margin-left: 4.5em;
    max-width: 10rem;
    font-weight: 500;
    line-height: 20px;
    text-shadow: 1px 1px 1px var(--gray-200);
    transition: font-size 100ms;
}
</style>

<style scoped>
:deep(.leaflet-control-zoom) {
    display: none;
}

:deep(.leaflet-div-icon) {
    background-color: transparent;
    border: none;
}

.leaflet-control-zoom .control-zoom-bottom-right {
    display: block !important;
}
</style>
