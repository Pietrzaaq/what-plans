<script setup>
import { onMounted, ref, toRef, watch } from 'vue';
import { useFavoritesStore } from "@/stores/favorites.js";
import EventItem from "@/components/map/popups/EventItem.vue";
import LongTextLink from "@/components/shared/LongTextLink.vue";

const props = defineProps(['teleportTo', 'popupPlace']);

const teleportToRef = toRef(props.teleportTo);
const place = toRef(props.popupPlace);
const events = ref(place.value.events);
const event = ref(null);

const favoritesStore = useFavoritesStore();
const isFavorite = ref(false);
watch(teleportToRef, function () {});

onMounted(() => {
    if (events.value.length === 1)
        event.value = events.value[0];
    
    isFavorite.value = favoritesStore.isEventFavorite(events.value[0].id);
});
</script>

<template>
    <Teleport :to="teleportToRef">
        <Transition appear>
            <DataView
                v-if="events.length > 1"
                class="event-popup"
                :value="events"
                :paginator="events.length > 3"
                :rows="3">
                <template #header>
                    <div class="w-full text-center">
                        <LongTextLink
                            class="font-bold w-full text-center text-l" :to="`/?placeId=${place.id}`"
                            :text="place.name"/>
                        <div class="text-light text-sm">{{ `${events.length} Events` }}</div>
                    </div>
                </template>
                <template #list="slotProps">
                    <div class="flex flex-column w-full">
                        <EventItem v-for="item in slotProps" :key="item.id" :item="item"/>
                    </div>
                </template>
            </DataView>
        </Transition>
    </Teleport>
</template>

<style>
.event-popup {
    display: flex;
    flex-direction: column;
    padding: 0 !important;
    min-width: 10rem !important;
    max-width: 30rem !important;
    min-height: 20rem !important;
    max-height: 35rem !important;
    border-radius: 2rem !important;
    overflow: hidden;
}
</style>

<style scoped>
:deep(.p-galleria-item-container) {
    width: 100%;
}

:deep(.p-galleria-indicators) {
    padding: 0.1rem;
}
</style>
