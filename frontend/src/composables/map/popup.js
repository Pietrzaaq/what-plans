import { nextTick, ref } from "vue";
import { usePlacesStore } from "@/stores/places.js";

const MAX_POPUP_HEIGHT = 490;

export function usePopup() {
    let thisObj = null;
    const isPopupVisible = ref(false);
    const teleportTo = ref(null);
    const popupArea = ref(null);
    const popup = ref(null);
    const keepPopupOpen = ref(false);
    const isPopupHovered = ref(false);

    const areasStore = usePlacesStore();

    function showPopup(event) {
        console.log(event);
        thisObj = this;
        popup.value = event.target?._popup;

        console.log(popup.value);
        console.log(popup.value._wrapper);

        if (isPopupVisible.value)
            return;

        keepPopupOpen.value = true;
        this.openPopup();

        setTimeout(() => {
            const areaId = popup.value.options.areaId;
            popupArea.value = areasStore.areas.find(a => a.Id === areaId);

            const polygonPopup = document.querySelector(`[data-area-id="${popupArea.value.Id}"]`);

            if (!polygonPopup || !popupArea.value)
                thisObj.togglePopup();

            teleportTo.value = polygonPopup;
            isPopupVisible.value = true;

            popup.value._container.style.visibility = 'hidden';
            popup.value._container.classList.remove('hidden');
            
            const topBarHeight = document.querySelector('.layout-topbar')?.getBoundingClientRect().height;
            const popupHtml = document.querySelector('.leaflet-popup');
            const popupRectangle = popupHtml.getBoundingClientRect();

            const clientHeight = document.documentElement.clientHeight;
            const clientWidth = document.documentElement.clientWidth;
            console.log('clientHeight', document.documentElement.clientHeight);
            console.log('clientWidth', document.documentElement.clientWidth);
            
            console.log('popupY', popupRectangle.y);
            console.log('popupX', popupRectangle.x );
            console.log('polygonPopupScroll', popupHtml.offsetTop, popupHtml.offsetLeft);
            
            if (popupRectangle.y < 100 || popupRectangle.y < 0) {
                console.log('overflow y top');
                console.log('transform', popupHtml.style.webkitTransform);

                const regExp = /\(([^)]+)\)/;
                const transform = popupHtml.style.webkitTransform;
                const transformValue = regExp.exec(transform);
                console.log(transformValue[1]);
                console.log();
                
                popupHtml.style.webkitTransform = 'none';
                // popupHtml.style.top = '100px';
                // popupHtml.style.left =  popupRectangle.x + 'px';
                // console.log('clientHeight', clientHeight);
                // console.log('polygonPopup', popupHtml.getBoundingClientRect().y);
            }
            if (popupRectangle.x + popupRectangle.width > clientWidth) {
                console.log('overflow x right');
                console.log((clientWidth - popupRectangle.width) + 'px');
                popupHtml.style.webkitTransform = 'none';
                // popupHtml.style.top =  popupRectangle.y + 'px';
                // popupHtml.style.left =  (clientWidth - popupRectangle.width) + 'px';
            }
            
            popup.value._container.style.visibility = 'visible';
        }, 500);
    }

    function hidePopup() {
        console.log('onPolygonMouseOut');
        keepPopupOpen.value = false;

        if (!isPopupVisible.value) {
            return;
        }

        setTimeout(() => {
            console.log(keepPopupOpen.value);
            if (keepPopupOpen.value || !isPopupVisible.value)
                return;

            thisObj.closePopup();
            thisObj = null;
            isPopupVisible.value = false;
            teleportTo.value = null;
            popupArea.value = null;
        }, 250);
    }

    function onPopupEnter() {
        console.log('onPopupEnter');

        if (!isPopupVisible.value)
            return;

        keepPopupOpen.value = true;
        isPopupHovered.value = true;
    }

    function onPopupLeave() {
        console.log('onPopupLeave');
        keepPopupOpen.value = false;
        isPopupHovered.value = false;

        hidePopup();
    }
    
    return {
        isPopupVisible,
        teleportTo,
        popupArea,
        keepPopupOpen,
        isPopupHovered,
        showPopup,
        hidePopup,
        onPopupEnter,
        onPopupLeave
    };
}
