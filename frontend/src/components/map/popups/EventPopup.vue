<script setup>
import { onMounted, ref, toRef, watch } from 'vue';
import moment from "moment";
import { FontAwesomeIcon } from "@fortawesome/vue-fontawesome";
import FavoriteButton from "@/components/shared/FavoriteButton.vue";
import { useFavoritesStore } from "@/stores/favorites.js";
import EventItem from "@/components/map/popups/EventItem.vue";

const props = defineProps(['teleportTo', 'popupPlace']);

const teleportToRef = toRef(props.teleportTo);
const place = toRef(props.popupPlace);
const events = ref(place.value.events);
const event = ref(null);

const favoritesStore = useFavoritesStore();
const isFavorite = ref(false);
watch(teleportToRef, function () {});

function startDate(event) {
    return moment(event.startDate).format('LLL');
}

function buyTicket() {
    window.open(event.value.url, '_blank');  
}

function toggleFavorite(event) {
    favoritesStore.toggleEventFavorite(event.id, isFavorite.value);
    isFavorite.value = !isFavorite.value;
}

onMounted(() => {
    if (events.value.length === 1)
        event.value = events.value[0];
    
    isFavorite.value = favoritesStore.isEventFavorite(events.value[0].id);
});
</script>

<template>
    <Teleport :to="teleportToRef">
        <Transition appear>
            <div>
                <DataView v-if="events.length > 1" :value="events">
                    <template #header>
                        <div class="w-full text-center">
                            <div class="flex">
                                <LongText class="font-bold w-full text-center text-xl pt-2" :text="place.name">
                                    <RouterLink :to="`/?placeId=${place.id}`">{{ place.name}}</RouterLink>
                                </LongText>
                            </div>
                            Events
                        </div>
                    </template>
                    <template #list="slotProps">
                        <div class="flex flex-column w-full">
                            <EventItem v-for="item in slotProps" :key="item.id" :item="item"/>
                        </div>
                    </template>
                </DataView>
                <Card v-else>
                    <template #header>
                        <div class="relative flex flex-column">
                            <FavoriteButton :is-favorite="isFavorite" @toggle-favorite="toggleFavorite(event)" />
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
                                        :alt="events[0].name"
                                        style="min-width: 100%; width: 100%; height: 10rem; display: block" />
                                </template>
                            </Galleria>
                        </div>
                        <div class="flex flex-column w-full text-center justify-content-center  py-2">
                            <div class="font-bold">{{ events[0].name }}</div>
                            <span v-if="place.location" class="font-medium">{{ place.location.name }}</span>
                        </div>
                    </template>
                    <template #content>
                        <div style="overflow-y: auto;">
                            <div v-if="events[0].location">
                                <div>
                                    <font-awesome-icon icon="fas fa-map-pin"></font-awesome-icon>
                                    <span class="font-medium ml-1">Address:</span>
                                </div>
                                <div>{{events[0].location.address }}</div>
                            </div>
                            <br>
                            <div class="w-full">
                                <div>
                                    <font-awesome-icon icon="fas fa-calendar-days"></font-awesome-icon>
                                    <span class="font-medium ml-1">Date:</span>
                                </div>
                                <div class="text-lg">{{ startDate(events[0].startDate ) }}</div>
                            </div>
                        </div>
                    </template>
                    <template #footer>
                        <div class="flex w-full justify-content-center py-2">
                            <Button icon="fas fa-ticket" class="w-full" label="Buy the ticket" @click="buyTicket"></Button>
                        </div>
                    </template>
                </Card>
            </div>
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
