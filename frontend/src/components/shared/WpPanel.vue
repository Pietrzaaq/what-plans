<script setup>
import { PANEL_EVENTS, panelEmitter } from "@/emitters/panelEmitter.js";

const model = defineModel();

const props = defineProps({
    header: {
        type: String,
        required: true
    }
});

function onPanelHide() {
    console.log("Panel hide");
    panelEmitter.emit(PANEL_EVENTS.CLOSE);
}
</script>

<template>
    <Sidebar v-model:visible="model" :header="props.header" position="right" @hide="onPanelHide">
    </Sidebar>
</template>

<style scoped>

</style>