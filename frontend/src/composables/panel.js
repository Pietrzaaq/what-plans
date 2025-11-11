import { onMounted, onUnmounted, ref } from "vue";
import { PANEL_EVENTS, panelEmitter } from "@/emitters/panelEmitter.js";

export function usePanel() {
    const visible = ref(false);

    function open() {
        visible.value = true;
    }

    function close() {
        visible.value = false;
    }

    onMounted(() => {
        panelEmitter.on(PANEL_EVENTS.OPEN, open);
        panelEmitter.on(PANEL_EVENTS.CLOSE, close);
    });

    onUnmounted(() => {
        panelEmitter.off(PANEL_EVENTS.OPEN, open);
        panelEmitter.off(PANEL_EVENTS.CLOSE, close);
    });

    return {
        visible,
        open,
        close
    };
}