<script setup>
import { computed, onMounted, ref, toRef, watch } from 'vue';
import { usePlacesStore } from "@/stores/places.js";
import moment from "moment";
import { FontAwesomeIcon } from "@fortawesome/vue-fontawesome";

const props = defineProps(['teleportTo', 'popupEvent']);

const teleportToRef = toRef(props.teleportTo);
const event = toRef(props.popupEvent);
const place = ref(null);
const favorites = ref([]);
const isFavorite = ref(false);
const startDate = computed(() => moment(event.value.startDate).format('LL'));
watch(teleportToRef, function () {});

function buyTicket() {
    window.open(event.value.url, '_blank');  
}

function toggleFavorite() {
    if (isFavorite.value) {
        // Remove from favorites
        const updatedFavorites = favorites.value.filter(id => id !== event.value.id);
        favorites.value = updatedFavorites;
        localStorage.setItem('favoritesEventsIds', JSON.stringify(updatedFavorites));
    } else {
        // Add to favorites
        favorites.value.push(event.value.id);
        localStorage.setItem('favoritesEventsIds', JSON.stringify(favorites.value));
    }

    isFavorite.value = !isFavorite.value;
}

onMounted(() => {
    const places = usePlacesStore().places;
    
    if (event.value.placeId) {
        place.value = places.find(p => p.id === event.value.placeId);
    }

    const favoritesFromStorage = localStorage.getItem('favoritesEventsIds');
    favorites.value = favoritesFromStorage ? JSON.parse(favoritesFromStorage) : [];
    isFavorite.value = favorites.value.includes(event.value.id);
});
</script>

<template>
    <Teleport :to="teleportToRef">
        <Transition appear>
            <Card  class="area-popup">
                <template #header>
                    <div class="relative flex flex-column">
                        <Button
                            text
                            :icon="isFavorite ? 'fas fa-heart' : 'far fa-heart'"
                            class="absolute fa-2xl align-self-end mr-3"
                            style="z-index: 100; color: white" 
                            @click="toggleFavorite"></Button>
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
                    <div class="flex flex-column w-full text-center justify-content-center  py-2">
                        <div class="font-bold">{{ event.name }}</div>
                        <a :href="place.url" v-if="place" class="font-medium">{{ place.name }}</a>
                    </div>
                </template>
                <template #content>
                    <div style="overflow-y: auto;">
                        <div v-if="place">
                            <span class="font-medium">Address:</span> 
                            <div>{{ place.location.address }}</div>
                        </div>
                        <br>
                        <div class="w-full">
                            <font-awesome-icon icon="fas fa-calendar"></font-awesome-icon>
                            <span class="font-medium ml-1">Date:</span>
                            <div class="text-lg">{{ startDate }}</div>
                        </div>
                    </div>
                </template>
                <template #footer>
                    <div class="flex w-full justify-content-center py-2">
                        <Button icon="fas fa-ticket" class="w-full" label="Buy the ticket" @click="buyTicket"></Button>
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
