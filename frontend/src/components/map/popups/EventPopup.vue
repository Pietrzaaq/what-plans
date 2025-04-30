<script setup>
import { computed, onMounted, ref, toRef, watch } from 'vue';
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

const eventPopupClasses = computed(() => {
    if (events.value.length < 2)
        return 'event-popup-short';

    if (events.value.length < 3)
        return 'event-popup-medium';
    
    return '';
});

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
            <DataView class="event-popup"
                      :class="eventPopupClasses"
                      :value="events"
                      :paginator="events.length > 3"
                      :rows="3">
                <template #header>
                    <div class="w-full text-center">
                        <LongTextLink class="font-bold w-full text-center text-l" :to="`/?placeId=${place.id}`"
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
    justify-content: center;
    padding: 0 !important;
    min-width: 30rem !important;
    max-width: 40rem !important;
    min-height: 20rem !important;
    border-radius: 2rem !important;
}

@media (max-width: 990px) {
    .place-popup {
        min-width: 25rem !important;
        max-width: 25rem !important;
    }
}

.event-popup-short {
    min-height: 12rem !important;
    max-height: 12rem !important;
}

.event-popup-medium {
    min-height: 22rem !important;
    max-height: 22rem !important;
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
