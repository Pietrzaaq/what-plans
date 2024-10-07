import { ref } from "vue";
import { usePlacesStore } from "@/stores/places.js";
import { useEventsStore } from "@/stores/events.js";


export function usePopup() {
    const isPlacePopupVisible = ref(false);
    const isEventPopupVisible = ref(false);
    const teleportTo = ref(null);
    const popupTargetObject = ref(null);
    const popup = ref(null);

    const placesStore = usePlacesStore();
    const eventStore = useEventsStore();

    function showPlacePopup(event) {
        popup.value = event.target?._popup;

        if (isPlacePopupVisible.value) {
            isPlacePopupVisible.value = false;
            teleportTo.value = null;
        }

        const placeId = popup.value.options.placeId;
        popupTargetObject.value = placesStore.places.find(a => a.id === placeId);

        teleportTo.value = document.querySelector(`.place-popup`);
        isPlacePopupVisible.value = true;
    }

    function showEventPopup(event) {
        popup.value = event.target?._popup;
        const eventIds = popup.value.options.eventIds;

        console.log(eventIds);
        const events = eventStore.events.filter(e => eventIds.includes(e.id));
        popupTargetObject.value = events;

        teleportTo.value = document.querySelector(`.event-popup`);
        isEventPopupVisible.value = true;
    }

    function hidePopup() {
        teleportTo.value = null;
        isEventPopupVisible.value = false;
        isPlacePopupVisible.value = false;
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
