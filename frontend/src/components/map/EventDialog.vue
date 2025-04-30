<script setup>
import { ref, computed, watch, defineEmits, toRef } from "vue";
import Dialog from 'primevue/dialog';
import InputText from 'primevue/inputtext';
import Dropdown from 'primevue/dropdown';
import Calendar from 'primevue/calendar';
import Button from 'primevue/button';
import Checkbox from 'primevue/checkbox';
import sportEventService from "../../services/eventsService.js";
import { useCurrentUserStore } from "@/stores/currentUser.js";

const emit = defineEmits(['close-dialog']);
const props = defineProps(['visible', 'area']);

const currentUserStore = useCurrentUserStore();
const user = computed(() => currentUserStore.user);

// Visibility handling
const dialogVisible = ref(false);
const visibleFromProp = toRef(props, 'visible');

// Form values for Request data
const eventName = ref("");
const eventType = ref(null);
const placeId = ref(null);
const locationId = ref(props.area?.LocationId || null);  // Assuming location ID comes from area prop if available
const creatorId = ref("");
const eventUrl = ref("");
const startDate = ref(new Date());
const endDate = ref(null);
const imageUrls = ref('');

// Additional form fields
const shouldAddDuration = ref(false);
const duration = ref(null);

// Available options for event type
const availableEventTypes = computed(() => [
    { id: 0, label: 'Sport', value: 'Sport' },
    { id: 1, label: 'Music', value: 'Music' },
    { id: 2, label: 'Cultural', value: 'Cultural' },
    // Add more event types here as needed
]);

// Handles form submission
function addEvent() {
    const type = eventType.value.id;
    let images;
    if (imageUrls.value) 
        images = imageUrls.value.split(";");
    else 
        images = [];
    
    const requestData = {
        Name: eventName.value,
        Type: type,
        PlaceId: placeId.value,
        LocationId: locationId.value,
        CreatorId: creatorId.value,
        Url: eventUrl.value,
        StartDate: startDate.value,
        EndDate: endDate.value,
        ImageUrls: images
    };

    // Submit request
    sportEventService.create(requestData)
        .then(() => closeDialog())
        .catch(err => console.error("Failed to create event:", err));
}

// Close dialog function
function closeDialog() {
    console.log('closeDialog');
    emit('close-dialog');
}

// Watch for visibility changes
watch(visibleFromProp, (newVal) => {
    dialogVisible.value = newVal;
    if (!newVal) 
        resetForm();
    else {

        console.log('Visiblity changed', props.area);

        placeId.value = props.area.id;
        locationId.value = props.area.location.id;
        creatorId.value = user.value.id;
    }
});

// Reset form fields
function resetForm() {
    eventName.value = "";
    eventType.value = null;
    placeId.value = null;
    creatorId.value = "";
    eventUrl.value = "";
    startDate.value = new Date();
    endDate.value = null;
    imageUrls.value = [];
    shouldAddDuration.value = false;
    duration.value = null;
}

</script>

<template>
    <Dialog :visible="dialogVisible"
            modal
            header="Add Event"
            class="w-30rem"
            @update:visible="closeDialog">
        <div class="flex flex-column gap-4">
            <!-- Event Name -->
            <div class="flex flex-column gap-2">
                <label for="eventName">Event Name</label>
                <InputText v-model="eventName" id="eventName" placeholder="Enter event name" />
            </div>

            <!-- Event Type -->
            <div class="flex flex-column gap-2">
                <label for="eventType">Event Type</label>
                <Dropdown v-model="eventType" :options="availableEventTypes" id="eventType" optionLabel="label" placeholder="Select type" />
            </div>
            
            <!-- Event URL -->
            <div class="flex flex-column gap-2">
                <label for="eventUrl">Event URL</label>
                <InputText v-model="eventUrl" id="eventUrl" placeholder="Enter event URL" />
            </div>

            <!-- Start Date -->
            <div class="flex flex-column gap-2">
                <label for="startDate">Start Date</label>
                <Calendar v-model="startDate" show-time hour-format="24" id="startDate" placeholder="Select start date and time" />
            </div>

            <!-- Duration Checkbox and End Date -->
            <div class="flex align-items-center justify-content-between gap-2 pt-1">
                <Checkbox v-model="shouldAddDuration" inputId="shouldAddDuration" />
                <label for="shouldAddDuration">Add duration (hours)</label>
                <Calendar v-if="shouldAddDuration" v-model="duration" show-time hour-format="24" />
            </div>

            <!-- Image URLs -->
            <div class="flex flex-column gap-2">
                <label for="imageUrls">Image URLs</label>
                <InputText v-model="imageUrls" id="imageUrls" placeholder="Enter image URLs (comma-separated)" />
            </div>

            <!-- Submit Button -->
            <div class="flex w-full justify-content-end pt-3">
                <Button label="Submit" icon="pi pi-check" iconPos="right" @click.prevent="addEvent" />
            </div>
        </div>
    </Dialog>
</template>

<style scoped>
/* Add custom styles here if needed */
</style>
