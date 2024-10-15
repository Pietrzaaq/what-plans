import { ref } from "vue";
import { useMapStore } from "@/stores/map.js";

export function usePopup(map) {
    const isAnotherPopupOpened = ref(false);
    const isPlacePopupVisible = ref(false);
    const isEventPopupVisible = ref(false);
    const teleportTo = ref(null);
    const popupTargetObject = ref(null);
    const popup = ref(null);

    const mapStore = useMapStore();

    function showPlacePopup(event) {
        popup.value = event.target?._popup;
        const placeId = popup.value.options.placeId;
        popupTargetObject.value = mapStore.data.find(p => p.id === placeId);

        if (isAnotherPopupOpened.value) {
            map.value.closePopup();
            hidePopup();
            return;
        }

        teleportTo.value = document.querySelector(`.place-popup`);
        isPlacePopupVisible.value = true;
        isAnotherPopupOpened.value = true;
    }

    function showEventPopup(event) {
        popup.value = event.target?._popup;
        const placeId = popup.value.options.placeId;
        popupTargetObject.value = mapStore.data.find(p => p.id === placeId);

        if (isAnotherPopupOpened.value) {
            map.value.closePopup();
            hidePopup();
            return;
        }

        teleportTo.value = document.querySelector(`.event-popup`);
        isEventPopupVisible.value = true;
        isAnotherPopupOpened.value = true;
    }

    function hidePopup() {
        isEventPopupVisible.value = false;
        isPlacePopupVisible.value = false;
        popup.value = null;
        teleportTo.value = null;
        popupTargetObject.value = null;
        
        setTimeout(() => {
            isAnotherPopupOpened.value = false;
        }, 50);
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
