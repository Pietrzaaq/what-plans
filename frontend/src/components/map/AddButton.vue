<script setup>
import { useCurrentUserStore } from "@/stores/currentUser.js";
import { computed, ref } from "vue";

const currentUserStore = useCurrentUserStore();
const menu = ref();
const items = computed(() => {
    if (!currentUserStore.isAuthenticated) {
        return [{ label: 'Only logged users can add events and places.' }];
    }
    
    return [
        {
            label: 'Options',
            items: [
                {
                    label: 'Add event',
                    icon: 'fa fa-calendar',
                    action: addEvent
                },
                {
                    label: 'Add place',
                    icon: 'fa fa-location-dot',
                    action: addPlace
                }
            ]
        }
    ];
});

function addPlace() {
    // TODO: Option of adding place
}

function addEvent() {
    // TODO: Option of adding event
}

function toggle(event) {
    menu.value.toggle(event);
}
</script>

<template>
    <Button icon="fa fa-plus"
            :label="'Add'"
            rounded
            class="absolute map-add-button"
            @click="toggle">
        
    </Button>
    <Menu ref="menu" id="overlay_menu" :model="items" :popup="true" />
</template>

<style scoped>
.map-add-button {
    z-index: 400;
    top: 7rem;
    right: 2rem;
    width: 6rem
}
</style>