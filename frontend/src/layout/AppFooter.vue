<script setup>
import { useLayout } from '@/layout/composables/layout';
import { computed } from 'vue';

const { layoutConfig } = useLayout();

const logoUrl = computed(() => {
    return `layout/images/logo.png`;
});
</script>

<template>
    <div class="layout-footer">
        <img :src="logoUrl" alt="Logo" height="20" class="mr-2" />
        by
        <span class="font-medium ml-2">Dawid Pietrzak</span>
    </div>
    <div class="text-center">
        Projekt wykonany w ramach pracy magisterkiej na Uniwersytecie Łódzkim
    </div>
</template>
<style lang="scss" scoped></style>
