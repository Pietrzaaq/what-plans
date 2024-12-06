<script setup>
import { onMounted } from "vue";
import { useEventsStore } from "@/stores/events.js";
import { storeToRefs } from "pinia";
import { useFavoritesStore } from "@/stores/favorites.js";
import EventItem from "@/components/map/popups/EventItem.vue";

const eventsStore = useEventsStore();
const { events } = storeToRefs(eventsStore);
const favoritesStore = useFavoritesStore();

onMounted(() => {
    favoritesStore.loadAll();
    eventsStore.loadAll();
});
</script>

<template>
    <Card>
        <template #content>
            <DataView class="w-full"
                      :value="events"
                      :paginator="events.length > 3"
                      :rows="10">
                <template #list="slotProps">
                    <div class="flex flex-column w-full">
                        <EventItem v-for="item in slotProps" :key="item.id" :item="item"/>
                    </div>
                </template>
            </DataView>
        </template>
    </Card>
</template>

<style scoped>

</style>