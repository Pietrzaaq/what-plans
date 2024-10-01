<script setup>
import { computed, onMounted, ref } from 'vue';
import { useEventsStore } from "@/stores/events.js";
import { storeToRefs } from "pinia";
import moment from "moment";

const eventsStore = useEventsStore();
const { events } = storeToRefs(eventsStore);
const favoritesFromLocalStorage = ref([]);
const favoriteEvents = computed(() => {
    console.log('getFavoriteEvents', favoritesFromLocalStorage.value, events.value);
    
    return events.value.filter(event => favoritesFromLocalStorage.value.length > 0 && favoritesFromLocalStorage.value.includes(event.id));
});

// Function to format the date
function formatDate(dateString) {
    return moment(dateString).format('LL');
}

function deleteFavorite(eventId) {
    const updatedFavorites = favoriteEvents.value.filter(id => id !== eventId);
    favoriteEvents.value = updatedFavorites;
    localStorage.setItem('favoritesEventsIds', JSON.stringify(updatedFavorites));
}

onMounted(() => {
    favoritesFromLocalStorage.value = JSON.parse(localStorage.getItem('favoritesEventsIds'));
    
    eventsStore.loadAll();
});

</script>

<template>
    <div class="favorites-view">
        <h2>Favorite Events</h2>
        <div v-if="events.length > 0 && favoriteEvents.length > 0" class="card-container w-full">
            <Card
                v-for="event in favoriteEvents"
                :key="event.id"
                class="event-card"
                :title="event.name">
                <template #content>
                    <div class="relative flex flex-column">
                        <Button
                            text
                            icon="fas fa-heart"
                            class="absolute fa-2xl align-self-end mr-3"
                            style="z-index: 100; color: white"
                            @click="deleteFavorite(event.id)"></Button>
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
        <div class="text-center font-medium text-xl pt-4">
            No favorites found 💔
        </div>
    </div>
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
