import { useMapStore } from "@/stores/map.js";
import { useFilterStore } from "@/stores/filter.js";
import { storeToRefs } from "pinia";
import { PLACE_TYPES_DATA } from "@/models/placeTypes.js";
import L from "leaflet";
import { EVENT_TYPES_DATA } from "@/models/eventTypes.js";

export function useMarker(markerLayer, map, showPlacePopup, showEventPopup, hidePopup) {
    const mapStore = useMapStore();
    const filterStore = useFilterStore();
    const { eventTypes } = storeToRefs(filterStore);
    
    function loadCitiesCircles() {
        mapStore.data.forEach(city => {
            const circle = L.circle([city.latitude, city.longitude], { 
                radius: city.radius,
                className: 'city-circle'
            });

            circle.addTo(markerLayer.value);
        });
    }

    function loadEventsMarkers() {
        mapStore.data.forEach(place => {
            let marker;

            if (!eventTypes.value.includes(place.placeType.toString()))
                return;

            marker = getEventMarker(place);

            marker.getPopup().on('remove', hidePopup);
            marker.addTo(markerLayer.value);
        });
    }

    function loadPlacesMarkers() {
        mapStore.data.forEach(place => {
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
            autoClose: false,
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
    
    return {
        loadPlacesMarkers,
        loadEventsMarkers,
        loadCitiesCircles
    };
}