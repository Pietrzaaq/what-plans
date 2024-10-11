<script setup>
import { toRef } from "vue";

const props = defineProps({
    isFavorite: { type: Boolean }
});
const isFavorite = toRef(props, 'isFavorite');

const emit = defineEmits(['toggleFavorite']);
</script>

<template>
    <Button
        :icon="isFavorite ? 'fas fa-heart' : 'far fa-heart'"
        :class="{'text-red-500': isFavorite }"
        text
        raised
        rounded
        class="fa-xl bg-gray-50"
        @click="emit('toggleFavorite')"/>
</template>

<style scoped lang="scss">

</style>