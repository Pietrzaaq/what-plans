<script setup>
import { onBeforeMount, onMounted, ref } from 'vue';
import L from 'leaflet';
import 'leaflet/dist/leaflet.css';
import AreaPopup from '@/components/map/popups/AreaPopup.vue';
import { usePopup } from "../composables/map/popup";
import { usePlacesStore } from "../stores/places.js";
import EventDialog from "../components/map/EventDialog.vue";
import 'leaflet.awesome-markers/dist/leaflet.awesome-markers.css'; 
import 'leaflet.awesome-markers/dist/leaflet.awesome-markers.js'; 

// Areas
const placesStore = usePlacesStore();

// Map
const map = ref(null);
let center = ref([51.769406790090855, 19.43750792680422]);
const zoom = ref(14);
const bounds = ref(null);
const url = ref(`https://api.maptiler.com/maps/basic-v2/{z}/{x}/{y}.png?key=${import.meta.env.VITE_MAP_TILER_API_KEY}`);

// Dialog
const isDialogVisible = ref(false);
const dialogArea = ref(null);

//Popup
const { teleportTo, isPopupVisible, popupArea, showPopup, hidePopup, onPopupEnter, onPopupLeave } = usePopup();

function onMapClick(e) {
    console.log('onMapClick', e);
}

// Polygon
function onPolygonClick(event) {
    console.log('onPolygonClick');
}
function onPopupMouseEnter() {
    console.log('onPopupMouseEnter');
}

function onPopupMouseLeave() {
    console.log('onPopupMouseLeave');
}

function onDialogClosed() {
    dialogArea.value = null;
    isDialogVisible.value = false;
    
    loadAreas();
}

function onMarkerClicked(event) {
    console.log('On marker clicked', event);
}

async function loadAreas() {
    await placesStore.loadAll();

    placesStore.areas.forEach((area) => {
        const polygon = L.geoJSON(area.Polygon).addTo(map.value);

        polygon.on('click', onPolygonClick);
        polygon.on('mouseover', showPopup);
        polygon.on('mouseout', hidePopup);
        polygon.bindPopup(`<div class="area-popup" data-area-id="${area.Id}"></div>`, {
            areaId: area.Id,
            autoClose: false,
            autoPan: false,
            closeButton: false,
            keepInView: true,
            className: 'hidden'
        });
    });
}

function boundsUpdated(newBoundsValue) {
    if (bounds.value !== newBoundsValue) {
        bounds.value = newBoundsValue;
    }
}

function openEventDialog(area) {
    dialogArea.value = area;
    isDialogVisible.value = true;
    console.log(area, isDialogVisible.value);
}

function initializeMap() {
    map.value = L.map('map', {
        center: L.latLng(center.value[0], center.value[1]),
        zoom: zoom.value
    });

    map.value.on('load', onMapLoad);
    map.value.on('zoomend', onZoomEnd);
    map.value.on('moveend', onMoveEnd);
    map.value.on('click', onMapClick);
    map.value.on('unload', onMapUnload);

    L.tileLayer(url.value, {
        minZoom: 4,
        maxZoom: 19,
        closePopupOnClick: false,
        attribution: '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>'
    }).addTo(map.value);
}

function onMapLoad() {
    console.log('Map load!');
}

function onMapUnload() {}

function onZoomEnd(event) {
    console.log(event);
    console.log(zoom.value);
    const newZoomValue = event.target._zoom;
    console.log(newZoomValue);

    if (zoom.value !== newZoomValue) {
        console.log('New zoom value', newZoomValue);
        zoom.value = newZoomValue;
    }
}

function onMoveEnd() {
    console.log('onMoveEnd');
    const newCenterValue = map.value.getCenter();

    if (center.value.lat !== newCenterValue.lat && center.value.lng !== newCenterValue.lng) {
        console.log('New center value', newCenterValue);
        center.value = newCenterValue;
    }
}

onBeforeMount(async () => {
    await loadAreas();

    navigator.geolocation.getCurrentPosition((position) => {
        center.value = [position.latitude, position.longitude];
    });
});

onMounted(() => {
    initializeMap();
});
</script>

<template>
    <main style="width: 100vw; height: 100vh">
        <div id="map" style="height: 100%" @load="onMapLoad"></div>
    </main>
    <AreaPopup
        v-if="isPopupVisible" 
        :teleportTo="teleportTo" 
        :popup-area="popupArea" 
        @addEvent="openEventDialog"
        @popupEnter="onPopupEnter" 
        @popupLeave="onPopupLeave"></AreaPopup>
    <EventDialog
        :visible="isDialogVisible"
        :area="dialogArea"
        @close-dialog="onDialogClosed"></EventDialog>
</template>

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
