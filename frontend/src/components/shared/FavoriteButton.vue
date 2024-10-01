<script setup>
import { toRef } from "vue";

const props = defineProps(['isFavorite']);
const isFavorite = toRef(props, 'isFavorite');

const emit = defineEmits(['toggleFavorite']);
</script>

<template>
    <Button
        :icon="isFavorite ? 'fas fa-heart' : 'far fa-heart'"
        :class="{ 'text-red-500': isFavorite }"
        text
        raised
        rounded
        class="absolute fa-xl align-self-end bg-gray-50 mr-3 mt-3"
        style="z-index: 100; right: 0"
        @click="emit('toggleFavorite')"/>
</template>

<style scoped lang="scss">

</style>