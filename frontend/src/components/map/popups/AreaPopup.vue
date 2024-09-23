<script setup>
import { computed, onMounted, toRef, watch } from 'vue';

const emit = defineEmits(['popupEnter', 'popupLeave', 'addEvent']);
const props = defineProps(['teleportTo', 'popupArea']);

const teleportToRef = toRef(props.teleportTo);
const popupAreaRef = toRef(props.popupArea);

const images = [
    {
        itemImageSrc: '../../../../public/map/court.jpg',
        alt: 'Description for Image 1',
        title: 'Title 1'
    }, 
    {
        itemImageSrc: '../../../../public/map/court2.jpg',
        alt: 'Description for Image 1',
        title: 'Title 1'
    }
];

const sportTypes = computed(() => {
    return popupAreaRef.value.sportType.split(';');
});

watch(teleportToRef, function () {});

function addEvent() {
    emit('addEvent', popupAreaRef.value);
}

function onMouseEnter() {
    emit('popupEnter');
}

function onMouseLeave() {
    emit('popupLeave');
}

onMounted(() => {});
</script>

<template>
    <Teleport :to="teleportToRef">
        <Transition appear>
            <Card
                class="area-popup"
                @mouseenter="onMouseEnter"
                @mouseleave="onMouseLeave">
                <template #header>
                    <Galleria
                        :value="images"
                        :numVisible="5"
                        :showThumbnails="false"
                        :showIndicators="true"
                        :changeItemOnIndicatorHover="true"
                        :showIndicatorsOnItem="true"
                        indicatorsPosition="bottom">
                        <template #item="slotProps">
                            <img
                                :src="slotProps.item.itemImageSrc"
                                :alt="slotProps.item.alt"
                                style="min-width: 100%; width: 100%; height: 10rem; display: block" />
                        </template>
                    </Galleria>
                    <div class="flex flex-column w-full text-center justify-content-center font-bold py-2">
                        {{ popupAreaRef.StreetAddress }}
                    </div>
                </template>
                <template #content>
                    <div style="overflow-y: auto;">
                        <div>Address: {{ popupAreaRef.FormattedAddress }}</div>
                        <br>
                        Events:
                        <div class="w-full">
                            <div v-if="!popupAreaRef.SportEvents || popupAreaRef.SportEvents.length === 0">
                                No upcoming events...
                            </div>
                            <div v-else class="flex align-items-center justify-content-between">
                                <div>21-04-2024</div>
                                <div>5 people</div>
                                <font-awesome-icon icon="fas fa-plus"></font-awesome-icon>
                            </div>
                        </div>
                    </div>
                </template>
                <template #footer>
                    <div class="flex w-full justify-content-center py-2">
                        <Button icon="far fa-plus" class="w-full" label="Add new event" @click="addEvent"></Button>
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
