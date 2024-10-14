import { ref } from "vue";
import { useMapStore } from "@/stores/map.js";

export function usePopup() {
    const isPlacePopupVisible = ref(false);
    const isEventPopupVisible = ref(false);
    const teleportTo = ref(null);
    const popupTargetObject = ref(null);
    const popup = ref(null);

    const mapStore = useMapStore();

    function showPlacePopup(event) {
        popup.value = event.target?._popup;

        if (isPlacePopupVisible.value) {
            isPlacePopupVisible.value = false;
            teleportTo.value = null;
        }

        const placeId = popup.value.options.placeId;
        popupTargetObject.value = mapStore.data.find(p => p.id === placeId);

        teleportTo.value = document.querySelector(`.place-popup`);
        isPlacePopupVisible.value = true;
    }

    function showEventPopup(event) {
        popup.value = event.target?._popup;
        const placeId = popup.value.options.placeId;

        console.log(placeId);
        popupTargetObject.value = mapStore.data.find(p => p.id === placeId);

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
