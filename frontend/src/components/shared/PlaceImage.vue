<script setup>
import { computed, onMounted } from "vue";
import { IMAGES_URL } from "@/models/uploadUrl.js";

const props = defineProps(['id', 'place']);
const imageUrl = computed(() => `${IMAGES_URL}/${props.id}/binary`);

onMounted(() => {
    console.log('Image of', props.place);
});

</script>

<template>
    <img :src="imageUrl" :alt="props.place.name">
</template>

<style scoped>

</style>