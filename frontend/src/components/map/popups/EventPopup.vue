<script setup>
import { computed, onMounted, ref, toRef, watch } from 'vue';
import { usePlacesStore } from "@/stores/places.js";
import moment from "moment";
import { FontAwesomeIcon } from "@fortawesome/vue-fontawesome";
import FavoriteButton from "@/components/shared/FavoriteButton.vue";
import { useFavoritesStore } from "@/stores/favorites.js";

const props = defineProps(['teleportTo', 'popupEvent']);

const teleportToRef = toRef(props.teleportTo);
const event = toRef(props.popupEvent);
const place = ref(null);
const startDate = computed(() => moment(event.value.startDate).format('LLL'));

const favoritesStore = useFavoritesStore();
const isFavorite = ref(false);
watch(teleportToRef, function () {});

function buyTicket() {
    window.open(event.value.url, '_blank');  
}

function toggleFavorite() {
    favoritesStore.toggleEventFavorite(event.value.id, isFavorite.value);
    isFavorite.value = !isFavorite.value;
}

onMounted(() => {
    const places = usePlacesStore().places;
    
    if (event.value.placeId) {
        place.value = places.find(p => p.id === event.value.placeId);
    }

    isFavorite.value = favoritesStore.isEventFavorite(event.value.id);
});
</script>

<template>
    <Teleport :to="teleportToRef">
        <Transition appear>
            <Card  class="area-popup">
                <template #header>
                    <div class="relative flex flex-column">
                        <FavoriteButton :is-favorite="isFavorite" @toggle-favorite="toggleFavorite" />
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
                            <div>
                                <font-awesome-icon icon="fas fa-map-pin"></font-awesome-icon>
                                <span class="font-medium ml-1">Address:</span>
                            </div>
                            <div>{{ place.location.address }}</div>
                        </div>
                        <br>
                        <div class="w-full">
                            <div>
                                <font-awesome-icon icon="fas fa-calendar-days"></font-awesome-icon>
                                <span class="font-medium ml-1">Date:</span>
                            </div>
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
