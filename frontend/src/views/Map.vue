<script setup>
import { onBeforeMount, onMounted, ref, watch } from 'vue';
import L from 'leaflet';
import 'leaflet/dist/leaflet.css';
import { usePopup } from "../composables/map/popup";
import { usePlacesStore } from "../stores/places.js";
import EventDialog from "../components/map/EventDialog.vue";
import 'leaflet.awesome-markers/dist/leaflet.awesome-markers.css'; 
import 'leaflet.awesome-markers/dist/leaflet.awesome-markers.js';
import Filter from "../components/map/Filter.vue";
import { useGlobalStore } from "@/stores/global.js";
import { storeToRefs } from "pinia";
import { EVENT_TYPES_DATA } from "@/models/eventTypes.js";
import EventPopup from "@/components/map/popups/EventPopup.vue";
import PlacePopup from "@/components/map/popups/PlacePopup.vue";
import { PLACE_TYPES_DATA } from "@/models/placeTypes.js";
import { useFilterStore } from "@/stores/filter.js";
import { MAP_TYPES } from "@/models/mapTypes.js";
import { useEventsStore } from "@/stores/events.js";
import { groupBy } from "@/helpers/helpers.js";

// Stores
const globalStore = useGlobalStore();
const { center } = storeToRefs(globalStore);
const filterStore = useFilterStore();
const { mapType, eventTypes } = storeToRefs(filterStore);
const placesStore = usePlacesStore();
const eventsStore = useEventsStore();

watch(center, (newVal) => {
    if (newVal) {
        map.value.setView(newVal, zoom.value);
    }
});

watch(mapType, async() => {
    await loadData();
});

watch(eventTypes, async() => {
    await loadMarkers();
});

// Map
const map = ref(null);
const zoom = ref(13);
const bounds = ref(null);
const url = ref(`https://api.maptiler.com/maps/basic-v2/{z}/{x}/{y}.png?key=${import.meta.env.VITE_MAP_TILER_API_KEY}`);
const markerLayer = ref(null);

// Dialog
const isDialogVisible = ref(false);
const dialogArea = ref(null);

//Popup
const { teleportTo, isPlacePopupVisible, isEventPopupVisible, popupTargetObject, showPlacePopup, showEventPopup, hidePopup } = usePopup();

function onMapClick(e) {
    console.log('onMapClick', e);
}

// Polygon
function onDialogClosed() {
    dialogArea.value = null;
    isDialogVisible.value = false;
    
    loadMarkers();
}

function boundsUpdated(newBoundsValue) {
    if (bounds.value !== newBoundsValue) {
        bounds.value = newBoundsValue;
    }
}

function openEventDialog(area) {
    dialogArea.value = area;
    isDialogVisible.value = true;
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

async function initializeMap() {
    map.value = L.map('map', {
        center: L.latLng(center.value[0], center.value[1]),
        zoom: zoom.value
    });

    map.value.on('load', onMapLoad);
    map.value.on('zoom', scaleIcon);
    map.value.on('zoomend', onZoomEnd);
    map.value.on('moveend', onMoveEnd);
    map.value.on('click', onMapClick);
    map.value.on('unload', onMapUnload);

    L.tileLayer(url.value, {
        minZoom: 4,
        maxZoom: 19,
        closePopupOnClick: false,
        attribution: '<span>Projekt wykonany w ramach pracy magisterkiej na Uniwersytecie Łódzkim</span> &copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>'
    }).addTo(map.value);
    
    await loadData();
}

async function loadData() {
    if (mapType.value === MAP_TYPES.EVENT) {
        await placesStore.loadAllWithEvents();
    } else {
        await placesStore.loadAll();
    }

    await loadMarkers();
}

async function loadMarkers() {
    if (markerLayer.value) {
        map.value.removeLayer(markerLayer.value);
        markerLayer.value = null;
    }
    markerLayer.value = L.layerGroup().addTo(map.value);
    
    if (mapType.value === MAP_TYPES.EVENT) 
        loadEventsMarkers();
    else 
        loadPlacesMarkers();
    
    scaleIcon();
}

function loadEventsMarkers() {
    placesStore.places.forEach(place => {
        let marker;

        if (!eventTypes.value.includes(place.placeType.toString()))
            return;

        marker = getEventMarker(place);

        marker.getPopup().on('remove', hidePopup);
        marker.addTo(markerLayer.value);
    });
}

function loadPlacesMarkers() {
    placesStore.places.forEach(place => {
        let marker;

        if (!eventTypes.value.includes(place.placeType.toString()))
            return;

        marker = getPlaceMarker(place);

        marker.getPopup().on('remove', hidePopup);
        marker.addTo(markerLayer.value);
    });
}


function getPlaceMarker(place) {
    const latLng = [place.location.latitude, place.location.longitude];

    const typeData = PLACE_TYPES_DATA[place.placeType];
    const html = `
            <div class="place-marker-content" style="background-color: ${typeData.markerColor}">
                <i class="fa fa-${typeData.icon}"></i>
            </div>
            <div class="place-marker-label">${place.name}</div>`;

    const overlay = L.divIcon({
        className: 'place-marker',
        html: html,
        iconSize: [50, 50], 
        iconAnchor: [25, 25] 
    });

    const marker = L.marker(latLng, { icon: overlay });

    marker.bindPopup(`<div class="place-popup"></div>`, {
        placeId: place.id,
        autoClose: true,
        autoPan: false,
        closeButton: false,
        keepInView: true,
    });
    marker.addTo(map.value);
    marker.on('click', showPlacePopup);

    return marker;
}

function getEventMarker(place) {
    let marker;
    const event = place.events[0];
    const latLng = [place.location.latitude, place.location.longitude];
    
    const markerOptions = {
        placeId: place.id,
        autoClose: true,
        autoPan: false,
        closeButton: false,
        keepInView: true,
    };
    
    const typeData = EVENT_TYPES_DATA[event.eventType];
    const icon = L.AwesomeMarkers.icon({
        icon: typeData.icon,
        markerColor: typeData.markerColor,
        prefix: 'fa'
    });
    marker = L.marker(latLng, { icon: icon });
    marker.bindPopup(`<div class="event-popup"></div>`, markerOptions);
    marker.on('click', showEventPopup);
    
    return marker;
}

function scaleIcon() {
    const zoom = map.value.getZoom(); 
    const scale = Math.pow(0.9, 18 - zoom); 

    document.querySelectorAll('.place-marker').forEach(function(marker) {
        const content = marker.querySelector('.place-marker-content');
        const label = marker.querySelector('.place-marker-label');
        const newSize = 50 * scale; 
        const newFontSize = 25 * scale; 
        const newLineHeight = 25 * scale;

        content.style.width = newSize + 'px';
        content.style.height = newSize + 'px';
        content.style.fontSize = newFontSize + 'px';
        
        if (zoom < 11)
            label.style.display = 'none';
        else {
            label.style.display = 'flex';
            label.style.fontSize = newFontSize + 'px';
            label.style.lineHeight = newLineHeight + 'px';
        }
    });
}


onBeforeMount(async () => {
    navigator.geolocation.getCurrentPosition((position) => {
        center.value = [position.latitude, position.longitude];
    });
});

onMounted(() => {
    initializeMap();

    setTimeout(() => scaleIcon(), 100);
});
</script>

<template>
    <main style="width: 100vw; height: 100vh">
        <div id="map" style="height: 100%" @load="onMapLoad"></div>
    </main>
    <Filter class="absolute"></Filter>
    <PlacePopup
        v-if="isPlacePopupVisible" 
        :teleportTo="teleportTo" 
        :popup-place="popupTargetObject" 
        @addEvent="openEventDialog"></PlacePopup>
    <EventPopup
        v-if="isEventPopupVisible"
        :teleportTo="teleportTo"
        :popup-place="popupTargetObject"
        @addEvent="openEventDialog"></EventPopup>
    <EventDialog
        :visible="isDialogVisible"
        :area="dialogArea"
        @close-dialog="onDialogClosed"></EventDialog>
</template>

<style>
.leaflet-container a {
    color: black !important;
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
