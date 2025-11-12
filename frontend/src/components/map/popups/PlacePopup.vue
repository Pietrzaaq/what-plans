<script setup>
import { onMounted, ref, toRef, watch } from 'vue';
import { PLACE_TYPES_DATA } from "@/models/placeTypes.js";
import FavoriteButton from "@/components/shared/FavoriteButton.vue";
import { useFavoritesStore } from "@/stores/favorites.js";
import LongTextLink from "@/components/shared/LongTextLink.vue";
import { FontAwesomeIcon } from "@fortawesome/vue-fontawesome";
import EventItem from "@/components/map/popups/EventItem.vue";
import placesService from "@/services/placesService.js";
import { useCurrentUserStore } from "@/stores/currentUser.js";
import { storeToRefs } from "pinia";
import PlaceImage from "@/components/shared/PlaceImage.vue";
import { PANEL_EVENTS, panelEmitter } from "@/emitters/panelEmitter.js";

const emit = defineEmits(['addEvent']);
const props = defineProps(['teleportTo', 'popupPlace']);

const currentUserStore = useCurrentUserStore();
const { user } = storeToRefs(currentUserStore) ;

const teleportToRef = toRef(props.teleportTo);
const place = toRef(props.popupPlace);
const placeEvents = toRef([]);
const favoritesStore = useFavoritesStore();
const isFavorite = ref(favoritesStore.isPlaceFavorite(place.value.id));

watch(teleportToRef, function () {});

function toggleFavoritePlace(placeId) {
    isFavorite.value = favoritesStore.isPlaceFavorite(placeId);
    favoritesStore.togglePlaceFavorite(placeId, isFavorite.value);
}

function addEvent() {
    emit('addEvent', place.value);
}

async function loadEvents() {
    placeEvents.value = await placesService.getEventsByPlaceId(place.value.id);
    console.log(placeEvents.value);
}

async function navigateToPlaceUrl() {
    window.open(place.value.url, '_blank');
}


async function navigateToGoogleMaps() {
    const location = place.value.location;
    const googleMapsLink = `https://www.google.com/maps/search/?api=1&query=${location.latitude},${location.longitude}`;

    window.open(googleMapsLink, '_blank');
}

async function openPlacePanel() {
    panelEmitter.emit(PANEL_EVENTS.OPEN);
}

function close() {
    // TODO: Add popup close from button
}

onMounted( async() => {
    await loadEvents();
});
</script>

<template>
    <Teleport :to="teleportToRef">
        <Transition appear>
            <Card v-if="place" class="area-popup">
                <template #header>
                    <div v-if="place.imageIds && place.imageIds.length > 0" class="relative">
                        <div class="absolute flex align-items-center gap-2" style="right: 1rem; top: 1rem; z-index: 100">
                            <FavoriteButton :is-favorite="favoritesStore.isPlaceFavorite(place.id)"
                                            @toggle-favorite="toggleFavoritePlace(place.id)"/>
                            <Button icon="fa fa-times" class="fa-xl bg-gray-50" text rounded @click="close"/>
                        </div>
                        <Galleria :value="place.imageIds"
                                  :numVisible="5"
                                  :circular="true"
                                  :showIndicators="true"
                                  :changeItemOnIndicatorHover="true"
                                  :showIndicatorsOnItem="true"
                                  :showItemNavigators="place.imageIds.length > 1 && place.imageIds.length < 10"
                                  :showThumbnails="false"
                                  indicatorsPosition="bottom">
                            <template #item="slotProps">
                                <PlaceImage :id="slotProps.item" :place="place" style="min-width: 100%; width: 100%; height: 15rem; display: block"></PlaceImage>
                            </template>
                        </Galleria>
                    </div>
                    <div class="flex flex-column relative w-full text-center justify-content-center py-2">
                        <FavoriteButton v-if="!place.imageIds || place.imageIds.length === 0"
                                        class="absolute align-self-end mr-4 mt-2"
                                        style="z-index: 100; right: 0; top: 0"
                                        :is-favorite="favoritesStore.isPlaceFavorite(place.id)"
                                        @toggle-favorite="toggleFavoritePlace(place.id)"/>
                        <LongTextLink class="font-bold w-full text-center text-l" :to="`/?placeId=${place.id}`"
                                      :text="place.name" @click="openPlacePanel"/>
                        <div>
                            {{ PLACE_TYPES_DATA[place.placeType].name }}
                        </div>
                        <div class="flex text-left px-3">
                            <div v-if="place.location.address && place.location.address !== ' '" class="flex w-9 flex-column font-medium pb-1">
                                <div class="flex align-items-center py-1">
                                    <font-awesome-icon icon="fas fa-house"></font-awesome-icon>
                                    <span class="ml-1">Address:</span>
                                </div>
                                <LongText class="flex w-full" :text="place.location.address">{{ place.location.address }}</LongText>
                            </div>
                            <div v-else class="flex flex-column font-medium pb-1 w-11">
                                <div class="flex align-items-center py-1">
                                    <font-awesome-icon icon="fas fa-house"></font-awesome-icon>
                                    <span class="ml-1">Coordinates:</span>
                                </div>
                                <LongText class="flex w-full" :text="`${place.location.latitude}, ${place.location.longitude}`">{{ `${place.location.latitude}, ${place.location.longitude}` }}</LongText>
                            </div>
                            <div class="flex w-3 align-items-center justify-content-end gap-2">
                                <Button v-if="user && (user.isOrganizer || user.isAdmin)" icon="fa fa-plus" @click="addEvent"></Button>
                                <Button v-if="place.url" icon="fa fa-globe" @click="navigateToPlaceUrl"></Button>
                                <Button icon="fa fa-compass" @click="navigateToGoogleMaps"></Button>
                            </div>
                        </div>
                    </div>
                </template>
                <template #content>
                    <DataView v-if="placeEvents.length > 0"
                              :value="placeEvents"
                              :paginator="placeEvents.length > 3"
                              :rows="3">
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
    min-width: 40rem !important;
    max-width: 40rem !important;
    max-height: 35rem !important;
    border-radius: 2rem !important;
}

@media (max-width: 990px) {
    .place-popup {
        min-width: 25rem !important;
        max-width: 25rem !important;
        overflow-y: auto;
    }
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
