<script setup>
import { onMounted, toRef, watch } from 'vue';

const emit = defineEmits(['popupEnter', 'popupLeave']);
const props = defineProps(['teleportTo', 'popupArea']);
console.log(props);
const teleportToRef = toRef(props.teleportTo);

watch(teleportToRef, function () {});

function onMouseEnter(event) {
    emit('popupEnter');
}

function onMouseLeave(event) {
    emit('popupLeave');
}

onMounted(() => {});
</script>

<template>
    <Teleport :to="teleportToRef">
        <div id="ap-popup" class="ap-popup position-absolute flex flex-column w-100" @mouseenter="onMouseEnter" @mouseleave="onMouseLeave">
            <div class="ap-popup-header">
                <slot name="header"></slot>
            </div>
            <div class="ap-popup-body">
                <slot></slot>
            </div>
        </div>
    </Teleport>
</template>

<style scoped lang="scss"></style>
