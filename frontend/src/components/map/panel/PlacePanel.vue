<script setup>
import WpPanel from "@/components/shared/WpPanel.vue";
import { usePanel } from "@/composables/panel.js";

const panel = usePanel();
</script>

<template>
    <WpPanel v-model="panel.visible.value" :header="'Place panel'"></WpPanel>
</template>

<style scoped>

</style>