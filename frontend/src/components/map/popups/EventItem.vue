<script setup>
import FavoriteButton from "@/components/shared/FavoriteButton.vue";
import { useFavoritesStore } from "@/stores/favorites.js";
import { computed, ref, toRef } from "vue";
import moment from 'moment';
import { FontAwesomeIcon } from "@fortawesome/vue-fontawesome";
import LongTextLink from "@/components/shared/LongTextLink.vue";

const favoritesStore = useFavoritesStore();

const props = defineProps(['item']);
const item = toRef(props.item);
const isFavorite = ref(favoritesStore.isEventFavorite(item.value.id));
const startDate = computed(() => moment(item.value.startDate).format('LLL'));
const imageUrls = computed(() => {
    console.log(item.value);
    const itemImageUrls = item.value.imageUrls;
    if (item.value.creatorId !== 'Ticketmaster') {
        return itemImageUrls;
    }

    const first = itemImageUrls[0];
    return [first];
});


function toggleFavorite() {
    favoritesStore.toggleEventFavorite(item.value.id, isFavorite.value);
    isFavorite.value = !isFavorite.value;
}

function buyTicket() {
    window.open(item.value.url, '_blank');
}
</script>

<template>
    <div v-if="item && item.id" class="flex flex-column justify-content-between md:flex-row p-2 gap-2">
        <div class="flex justify-content-center align-items-center w-full md:w-3">
            <Galleria v-if="item"
                      :value="imageUrls"
                      :numVisible="5"
                      :showThumbnails="false"
                      :showIndicators="true"
                      :changeItemOnIndicatorHover="true"
                      :showIndicatorsOnItem="true"
                      indicatorsPosition="bottom">
                <template #item="slotProps">
                    <img class="block xl:block"
                         :src="slotProps.item"
                         :alt="item.name"
                         style="width: 6rem; height: 6rem; border-radius: 1rem; display: block" />
                </template>
            </Galleria>
        </div>
        <div class="flex flex-column md:flex gap-2 max-w-full md:w-9">
            <LongTextLink class="font-bold text-l w-full" :to="`/?eventId=${item.id}`" :text="item.name" @click=""/>
            <div class="flex align-items-center gap-2 pb-1">
                <div>
                    <font-awesome-icon icon="fas fa-calendar-days"></font-awesome-icon>
                    <span class="text-sm ml-1">Date:</span>
                </div>
                <div class="text-sm">{{ startDate }}</div>
            </div>
            <div class="flex align-items-center justify-content-end gap-2">
                <Button v-if="item.url" icon="fas fa-ticket" :label="item.price" @click="buyTicket"></Button>
                <FavoriteButton  :is-favorite="isFavorite" :absolute="false" @toggle-favorite="toggleFavorite"/>
            </div>
        </div>
    </div>
</template>

<style scoped>
:deep(.p-galleria) .p-galleria-indicators .p-galleria-indicator button {
    width: 0.7rem;
    height: 0.7rem;
}
</style>