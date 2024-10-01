import { nextTick, ref } from "vue";
import { usePlacesStore } from "@/stores/places.js";
import { useEventsStore } from "@/stores/events.js";



export function usePopup() {
    const isPlacePopupVisible = ref(false);
    const isEventPopupVisible = ref(false);
    const teleportTo = ref(null);
    const popupTargetObject = ref(null);
    const popup = ref(null);

    const placesStore = usePlacesStore();

    function showPlacePopup(event) {
        popup.value = event.target?._popup;

        const placeId = popup.value.options.placeId;
        popupTargetObject.value = placesStore.places.find(a => a.id === placeId);

        const markerPopup = document.querySelector(`.place-popup`);

        console.log('markerPopup', markerPopup, popup.value, popupTargetObject.value);
        
        teleportTo.value = markerPopup;
        isPlacePopupVisible.value = true;
    }

    function showEventPopup(event) {
        popup.value = event.target?._popup;

        const placeId = popup.value.options.placeId;
        const eventId = popup.value.options.eventId;
        const place = placesStore.places.find(p => p.id === placeId);
        popupTargetObject.value = place.events.find(a => a.id === eventId);

        console.log(place, eventId);
        const markerPopup = document.querySelector(`.event-popup`);

        console.log('markerPopup', markerPopup, popup.value, popupTargetObject.value);

        teleportTo.value = markerPopup;
        isEventPopupVisible.value = true;
    }

    function hidePopup() {
        console.log('hidePopup');
        isEventPopupVisible.value = false;
        isPlacePopupVisible.value = false;
        teleportTo.value = null;
        popupTargetObject.value = null;
    }
    
    
    return {
        isPlacePopupVisible,
        isEventPopupVisible,
        teleportTo,
        popupTargetObject,
        showPlacePopup,
        showEventPopup,
        hidePopup
    };
}
