<script setup>
import { onMounted, toRef, watch } from 'vue';
import { PLACE_TYPES_DATA } from "@/models/placeTypes.js";
import moment from "moment";
import FavoriteButton from "@/components/shared/FavoriteButton.vue";
import { useFavoritesStore } from "@/stores/favorites.js";

const emit = defineEmits(['addEvent']);
const props = defineProps(['teleportTo', 'popupPlace']);

const teleportToRef = toRef(props.teleportTo);
const place = toRef(props.popupPlace);
const favoritesStore = useFavoritesStore();

watch(teleportToRef, function () {});

function formatDate(date) {
    return moment(date).format('LLL');
}

function buyTicket(event) {
    window.open(event.url, '_blank');
}

function toggleFavoritePlace(placeId) {
    const isFavorite = favoritesStore.isPlaceFavorite(placeId);
    favoritesStore.togglePlaceFavorite(placeId, isFavorite.value);
}

function toggleFavoriteEvent(eventId) {
    const isFavorite = favoritesStore.isEventFavorite(eventId);
    favoritesStore.toggleEventFavorite(eventId, isFavorite.value);
}

function addEvent() {
    emit('addEvent', place.value);
}

onMounted(() => {
    favoritesStore.loadAll();
});
</script>

<template>
    <Teleport :to="teleportToRef">
        <Transition appear>
            <Card v-if="place" class="area-popup">
                <template #header>
                    <div class="relative">
                        <FavoriteButton :is-favorite="favoritesStore.isPlaceFavorite(place.id)" @toggle-favorite="toggleFavoritePlace(place.id)" />
                        <Galleria
                            :value="place.imageUrls"
                            :numVisible="5"
                            :circular="true"
                            :showIndicators="true"
                            :changeItemOnIndicatorHover="true"
                            :showIndicatorsOnItem="true"
                            :showItemNavigators="true"
                            :showThumbnails="false"
                            indicatorsPosition="bottom">
                            <template #item="slotProps">
                                <img
                                    :src="slotProps.item"
                                    :alt="place.name"
                                    style="min-width: 100%; width: 100%; height: 15rem; display: block" />
                            </template>
                        </Galleria>
                    </div>
                    <div class="flex flex-column w-full text-center justify-content-center  py-2">
                        <a :href="place.url" v-if="place" class="font-bold text-lg">{{ place.name }}</a>
                        <div>
                            {{ PLACE_TYPES_DATA[place.placeType].name }}
                        </div>
                        <div class="text-left px-3">
                            <span class="font-medium">Address:</span>
                            <div>{{ place.location.address }}</div>
                        </div>
                    </div>

                </template>
                <template #content>
                    <Accordion v-if="place.events.length > 0" :activeIndex="0" class="flex flex-column gap-2 pb-4" >
                        <AccordionTab v-for="event in place.events" :key="event.id">
                            <template #header>
                                <div class="flex flex-column w-full gap-2">
                                    <LongText class="text-black-50 text-sm font-medium w-10" :text="event.name">{{ event.name }}</LongText>
                                    <div class="text-xs">{{ formatDate(event.startDate) }}</div>
                                </div>
                            </template>
                            <div>
                                <div class="relative flex flex-column">
                                    <FavoriteButton :is-favorite="favoritesStore.isEventFavorite(event.id)" @toggle-favorite="toggleFavoriteEvent(event.id)"/>
                                    <Galleria
                                        v-if="event"
                                        :value="event.imageUrls"
                                        :numVisible="5"
                                        :showThumbnails="false"
                                        :showIndicators="true"
                                        :changeItemOnIndicatorHover="true"
                                        :showIndicatorsOnItem="true"
                                        indicatorsPosition="bottom">
                                        <template #item="slotProps">
                                            <img
                                                :src="slotProps.item"
                                                :alt="event.name"
                                                style="min-width: 100%; width: 100%; height: 10rem; display: block" />
                                        </template>
                                    </Galleria>
                                </div>
                                <div class="flex w-full justify-content-center py-2">
                                    <Button icon="fas fa-ticket" class="w-full" label="Buy the ticket" @click="buyTicket(event)"></Button>
                                </div>
                            </div>
                        </AccordionTab>
                    </Accordion>
                    <div v-else class="text-lg font-medium pb-2">
                        No upcoming events...
                    </div>
                </template>
            </Card>
        </Transition>
    </Teleport>
</template>

<style scoped>
:deep(.p-card) {
    border-top-left-radius: 0.5rem;
    border-top-right-radius: 0.5rem;
}

:deep(.p-galleria-item) img {
    border-top-left-radius: 0.5rem;
    border-top-right-radius: 0.5rem; 
}

:deep(.p-card-body) {
    overflow-y: auto !important;
    padding: 0 1rem !important;
}

:deep(.p-card-content) {
    padding: 0;
}

:deep(.p-galleria-item-container) {
    width: 100%;
}

:deep(.p-galleria-indicators) {
    padding: 0.2rem;
}

</style>
