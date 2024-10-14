<script setup>
import { onMounted, toRef, watch } from 'vue';
import { PLACE_TYPES_DATA } from "@/models/placeTypes.js";
import FavoriteButton from "@/components/shared/FavoriteButton.vue";
import { useFavoritesStore } from "@/stores/favorites.js";
import LongTextLink from "@/components/shared/LongTextLink.vue";
import { FontAwesomeIcon } from "@fortawesome/vue-fontawesome";
import EventItem from "@/components/map/popups/EventItem.vue";
import placesService from "@/services/placesService.js";

const emit = defineEmits(['addEvent']);
const props = defineProps(['teleportTo', 'popupPlace']);

const teleportToRef = toRef(props.teleportTo);
const place = toRef(props.popupPlace);
const placeEvents = toRef([]);
const favoritesStore = useFavoritesStore();

watch(teleportToRef, function () {});

function toggleFavoritePlace(placeId) {
    const isFavorite = favoritesStore.isPlaceFavorite(placeId);
    favoritesStore.togglePlaceFavorite(placeId, isFavorite.value);
}

function addEvent() {
    emit('addEvent', place.value);
}

async function loadEvents() {
    placeEvents.value = await placesService.getEventsByPlaceId(place.value.id);
}

onMounted( async() => {
    await loadEvents();
    console.log('On popup mounted');
});
</script>

<template>
    <Teleport :to="teleportToRef">
        <Transition appear>
            <Card v-if="place" class="area-popup">
                <template #header>
                    <div v-if="place.imageUrls.length > 0" class="relative">
                        <FavoriteButton
                            class="absolute align-self-end mr-3 mt-3"
                            style="z-index: 100; right: 0"
                            :is-favorite="favoritesStore.isPlaceFavorite(place.id)"
                            @toggle-favorite="toggleFavoritePlace(place.id)"/>
                        <Galleria
                            :value="place.imageUrls"
                            :numVisible="5"
                            :circular="true"
                            :showIndicators="true"
                            :changeItemOnIndicatorHover="true"
                            :showIndicatorsOnItem="true"
                            :showItemNavigators="place.imageUrls.length > 1"
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
                    <div class="flex flex-column w-full text-center justify-content-center py-2">
                        <LongTextLink
                            class="font-bold w-full text-center text-l" :to="`/?placeId=${place.id}`"
                            :text="place.name"/>
                        <div>
                            {{ PLACE_TYPES_DATA[place.placeType].name }}
                        </div>
                        <div class="text-left px-3">
                            <div class="flex flex-column font-medium pb-1">
                                <div class="flex align-items-center py-1">
                                    <font-awesome-icon icon="fas fa-house"></font-awesome-icon>
                                    <span class="ml-1">Address:</span>
                                </div>
                                <LongText class="flex w-full" :text="place.location.address">{{ place.location.address }}</LongText>
                            </div>
                        </div>
                    </div>
                </template>
                <template #content>
                    <DataView
                        v-if="placeEvents.length > 1"
                        :value="placeEvents"
                        :paginator="placeEvents.length > 2"
                        :rows="2">
                        <template #header>
                            <div class="w-full text-center">
                                <div class="text-light text-sm">{{ `${placeEvents.length} Events` }}</div>
                            </div>
                        </template>
                        <template #list="slotProps">
                            <div class="flex flex-column w-full">
                                <EventItem v-for="item in slotProps" :key="item.id" :item="item"/>
                            </div>
                        </template>
                    </DataView>
                    <div v-else class="text-lg text-center font-medium pb-2">
                        No upcoming events...
                    </div>
                </template>
            </Card>
        </Transition>
    </Teleport>
</template>

<style>
.place-popup {
    display: flex;
    flex-direction: column;
    padding: 0 !important;
    min-width: 25rem !important;
    max-width: 25rem !important;
    max-height: 35rem !important;
    border-radius: 2rem !important;
}
</style>

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
