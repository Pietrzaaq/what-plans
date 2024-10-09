<script setup>
import FavoriteButton from "@/components/shared/FavoriteButton.vue";
import { useFavoritesStore } from "@/stores/favorites.js";
import { computed, ref, toRef } from "vue";
import moment from "moment/moment.js";
import { FontAwesomeIcon } from "@fortawesome/vue-fontawesome";

const favoritesStore = useFavoritesStore();

const props = defineProps(['item']);
const item = toRef(props.item);
const isFavorite = ref(favoritesStore.isEventFavorite(item.value.id));
const startDate = computed(() => moment(item.value.startDate).format('LLL'));

function toggleFavorite() {
    favoritesStore.toggleEventFavorite(item.value.id, isFavorite.value);
    isFavorite.value = !isFavorite.value;
}
</script>

<template>
    <div v-if="item && item.id" class="flex w-full p-2 gap-2">
        <div class="flex w-10rem">
            <Galleria
                v-if="item"
                class="h-full"
                :value="item.imageUrls"
                :numVisible="5"
                :showThumbnails="false"
                :showIndicators="true"
                :changeItemOnIndicatorHover="true"
                :showIndicatorsOnItem="true"
                indicatorsPosition="bottom">
                <template #item="slotProps">
                    <img
                        :src="slotProps.item"
                        :alt="item.name"
                        style="width: 8rem; height: 8rem; border-radius: 1rem; display: block" />
                </template>
            </Galleria>
        </div>
        <div class="flex flex-column justify-content-between gap-2 w-full">
            <div class="flex flex-column justify-content-center w-full gap-2">
                <LongText class="font-bold text-xl pt-2" :text="item.name">
                    <RouterLink :to="`/?eventId=${item.id}`">{{ item.name}}</RouterLink>
                </LongText>
                <div class="w-full">
                    <div>
                        <font-awesome-icon icon="fas fa-calendar-days"></font-awesome-icon>
                        <span class="font-medium ml-1">Date:</span>
                    </div>
                    <div class="text-lg">{{ startDate }}</div>
                </div>
            </div>
            <FavoriteButton class="align-self-end" :is-favorite="isFavorite" :absolute="false" @toggle-favorite="toggleFavorite" />
        </div>
    </div>
</template>

<style scoped>
:deep(.p-galleria) .p-galleria-indicators .p-galleria-indicator button {
    width: 1rem;
    height: 1rem;
}
</style>