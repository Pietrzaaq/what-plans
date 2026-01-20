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
import PlacePanel from "@/components/map/panel/PlacePanel.vue";
import { useMap } from "@/composables/map/map.js";
import AddButton from "@/components/map/AddButton.vue";
import DeveloperInfo from "@/components/map/DeveloperInfo.vue";

// STORES
const mapStore = useMapStore();
const { map, zoom, center } = storeToRefs(mapStore);

const filterStore = useFilterStore();
const { mapType, eventTypes, placeTypes, startDate } = storeToRefs(filterStore);

const globalStore = useGlobalStore();
const { city, searchItem } = storeToRefs(globalStore);

// FILTER
watch(mapType, async() => {
    mapStore.clear();
    await loadData();
});

watch(eventTypes, async() => {
    loadMarkers();
});


watch(placeTypes, async() => {
    loadMarkers();
});

watch(startDate, async() => {
    mapStore.clear();
    await loadData();
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
const geohashes = ref([]);
const geohashesLayer = ref([]);
const { loadPlacesMarkers, loadEventsMarkers } = useMarker(markerLayer, map, showPlacePopup, showEventPopup, hidePopup, navigateToCity);

function navigateToCity(e) {
    console.log('Navigate', e);
}

function loadMarkers() {
    console.log('loadMarkers', markerLayer.value);
    if (markerLayer.value) {
        map.value.removeLayer(markerLayer.value);
        markerLayer.value = null;
    }
    markerLayer.value = L.layerGroup().addTo(map.value);
    
    if (mapType.value === MAP_TYPES.EVENT)
        loadEventsMarkers();
    else
        loadPlacesMarkers();

    scaleMarkers();
}

function loadGeohashes() {
    const bounds = map.value.getBounds();
    const currGeohashes = geolocationService.getBoundingBoxGeohashes(bounds, zoom.value);

    for (const geohash of currGeohashes) {
        const geohashBounds = geolocationService.getGeohashBounds(geohash);
        const geohashRect = L.rectangle(
            [[geohashBounds[0], geohashBounds[1]], [geohashBounds[2], geohashBounds[3]]],
            { color: 'red', fillOpacity: 0.1, weight: 3, fill: false }
        );
        geohashRect.id = geohash;
        if (!geohashes.value.includes(geohashRect.id)) {
            geohashRect.addTo(geohashesLayer.value);
            geohashes.value.push(geohashRect.id);
        }
    }
}

function scaleMarkers() {
    const zoom = map.value.getZoom();
    document.querySelectorAll('.place-marker').forEach(function(marker) {
        const label = marker.querySelector('.place-marker-label');

        if (zoom < 14)
            label.style.display = 'none';
        else {
            label.style.display = 'flex';
        }
    });
}

// MAP
const mapComposable = useMap();

async function OnMapMoveEnd() {
    const newCenterValue = map.value.getCenter();
    if (center.value.lat !== newCenterValue.lat && center.value.lng !== newCenterValue.lng) {
        const newCenter = [newCenterValue.lat, newCenterValue.lng];
        mapStore.setCenter(newCenter);
        
        const geohash = geolocationService.getGeohash(newCenter[0], newCenter[1], zoom.value);
        mapStore.setGeohash(geohash);

        const bounds = map.value.getBounds();
        const bbox = {
            north: bounds._northEast.lat,
            west: bounds._northEast.lng,
            south: bounds._southWest.lat,
            east: bounds._southWest.lng,
        };
        mapStore.setBbox(bbox);
    }
    
    await loadData();
    loadMarkers();
    loadGeohashes();
}

async function OnMapZoomEnd() {
    const newZoomValue = map.value.getZoom();
    if (zoom.value !== newZoomValue) {
        mapStore.setZoom(newZoomValue);
    }
}

async function loadData() {
    await mapStore.loadData();
}

function onMapLoad() {
    console.log('Map load!');
    setTimeout(() => scaleMarkers(), 100);
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
    await mapComposable.initialize();

    map.value.on('load', onMapLoad);
    map.value.on('moveend', OnMapMoveEnd);
    map.value.on('zoomend', OnMapZoomEnd);
    map.value.on('zoom', scaleMarkers);
    map.value.on('click', onMapClick);
    map.value.on('unload', onMapUnload);
    
    geohashesLayer.value = L.layerGroup().addTo(map.value);

    await loadData();
    loadMarkers();
    loadGeohashes();
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
    <AddButton></AddButton>
    <PlacePanel/>
    <DeveloperInfo/>
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
    position: absolute;
    left: 35px;
    font-size: 10px;
    height: 100%;
    align-items: center;
    text-align: left;
    max-width: 15rem;
    text-overflow: ellipsis;
    font-weight: 500;
    line-height: 10px;
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
