<script setup>
import { computed, onMounted } from 'vue';
import { useEventsStore } from "@/stores/events.js";
import { storeToRefs } from "pinia";
import moment from "moment";
import { usePlacesStore } from "@/stores/places.js";
import { useFavoritesStore } from "@/stores/favorites.js";
import FavoriteButton from "@/components/shared/FavoriteButton.vue";

const eventsStore = useEventsStore();
const { events } = storeToRefs(eventsStore);
const placesStore = usePlacesStore();
const { places } = storeToRefs(placesStore);
const favoritesStore = useFavoritesStore();

const favoriteEvents = computed(() => {
    return events.value.filter(event => favoritesStore?.favoritesEvents.length > 0 && favoritesStore.favoritesEvents.includes(event.id));
});

const favoritePlaces = computed(() => {
    return places.value.filter(place => favoritesStore?.favoritesPlaces.length > 0 && favoritesStore.favoritesPlaces.includes(place.id));
});

function formatDate(dateString) {
    return moment(dateString).format('LL');
}

function toggleFavoritePlace(placeId) {
    const isFavorite = favoritesStore.isPlaceFavorite(placeId);
    favoritesStore.togglePlaceFavorite(placeId, isFavorite);
}

function toggleFavoriteEvent(eventId) {
    const isFavorite = favoritesStore.isEventFavorite(eventId);
    favoritesStore.toggleEventFavorite(eventId, isFavorite);
}

onMounted(() => {
    favoritesStore.loadAll();
    eventsStore.loadAll();
    placesStore.loadAll();
});

</script>

<template>
    <Card class="favorites-view">
        <template #header>
            <TabView>
                <TabPanel header="Favorite Events">
                    <div v-if="events.length > 0 && favoriteEvents.length > 0" class="card-container w-full">
                        <Card v-for="event in favoriteEvents"
                              :key="event.id"
                              class="event-card"
                              :title="event.name">
                            <template #content>
                                <div class="relative flex flex-column">
                                    <FavoriteButton :is-favorite="favoritesStore.isEventFavorite(event.id)"
                                                    class="absolute align-self-end mr-3 mt-3"
                                                    style="z-index: 100; right: 0"
                                                    @toggle-favorite="toggleFavoriteEvent(event.id)"/>
                                    <img :src="event.imageUrls[0]" alt="Event Image" class="event-image" />
                                </div>
                                <div class="event-content">
                                    <h4>{{ event.name }}</h4>
                                    <p><strong>Start Date:</strong> {{ formatDate(event.startDate) }}</p>
                                    <a :href="event.url" target="_blank" class="event-link">View Event</a>
                                </div>
                            </template>
                        </Card>
                    </div>
                    <div v-else class="text-center font-medium text-xl pt-4">
                        No favorites found 💔
                    </div>
                </TabPanel>
                <TabPanel header="Favorite Places">
                    <div v-if="favoritePlaces.length > 0" class="card-container w-full">
                        <Card v-for="place in favoritePlaces" :key="place.id" class="p-col-12 p-md-6 p-lg-4">
                            <template #header>
                                <div class="relative">
                                    <FavoriteButton :is-favorite="favoritesStore.isPlaceFavorite(place.id)"
                                                    class="absolute align-self-end mr-3 mt-3"
                                                    style="z-index: 100; right: 0"
                                                    @toggle-favorite="toggleFavoritePlace(place.id)"/>
                                    <img v-if="place.imageUrls && place.imageUrls.length > 0" :src="place.imageUrls[0]" alt="Place Image" class="place-image max-w-30rem" />
                                    <div v-else class="relative flex flex-column">
                                        <div class="absolute h-4rem bg-primary-400" style="top: 0; width: 100%" ></div>
                                        <div class="mb-8"></div>
                                    </div>
                                </div>
                            </template>
                            <template #title>
                                <a :href="place.url" target="_blank" class="place-name">{{ place.name }}</a>
                            </template>
                            <template #subtitle>
                                <p>{{ place.location.formatedAddress }}</p>
                            </template>
                            <template #content>
                                <p>{{ place.description || 'No description available' }}</p>
                            </template>
                            <template #footer>
                                <p><strong>Mail: </strong><a :href="'mailto:' + place.mail">{{ place.mail }}</a></p>
                                <p><strong>City: </strong>{{ place.location.cityName }}</p>
                            </template>
                        </Card>
                    </div>
                    <div v-else class="text-center font-medium text-xl pt-4">
                        No favorites found 💔
                    </div>
                </TabPanel>
            </TabView>
        </template>
    </Card>
</template>

<style scoped>
.favorites-view {
    padding: 20px;
}

.card-container {
    display: flex;
    flex-wrap: wrap;
    gap: 20px;
}

.event-card {
    width: 300px;
}

.event-image {
    width: 100%;
    height: auto;
}

.event-content {
    padding: 10px;
}

.event-link {
    display: inline-block;
    margin-top: 10px;
    color: blue;
}
</style>
