<script setup>
import Dialog from 'primevue/dialog';
import { ref, toRef, watch, defineEmits, computed } from "vue";
import Button from 'primevue/button';
import Calendar from 'primevue/calendar';
import sportEventService from "../../services/eventsService.js";

const emit = defineEmits(['close-dialog']);

const props = defineProps(['visible', 'area']);
const visibleFromProp = toRef(props, 'visible');
const dialogVisible = ref(false);

// Form values
const sportType = ref(null);
const createdBy = ref();
const creationDate = ref();
const shouldAddDuration = ref(false);
const duration = ref();
const eventStartDate = ref(new Date());
const eventEndDate = ref();

const availableSportTypes = computed(() =>  {
    if (!props.area)
        return;
    
    return [props.area.Sport];
});

function addEvent() {
  creationDate.value = new Date();
  
  // const event = new SportEvent(props.area.Id, sportType.value, createdBy.value, creationDate.value, eventStartDate.value, eventEndDate.value);
  // sportEventService.create(event);

  closeDialog();
}

function closeDialog() {
  emit('close-dialog');
}

watch(visibleFromProp, function () {
  dialogVisible.value = visibleFromProp.value;
  
  if (visibleFromProp.value) 
      sportType.value = props.area.Sport;
  else 
      sportType.value = null;
});
</script>

<template>
    <Dialog
        :visible="visibleFromProp"
        modal
        header="Add event"
        class="w-30rem"
        @update:visible="closeDialog">
        <div class="flex flex-column gap-4">
            <div class="flex flex-column gap-2">
                <label for="eventName">Sport type</label>
<!--                <SportTypeDropdown v-model="sportType" :available-types="availableSportTypes"></SportTypeDropdown>-->
            </div>
            <div class="flex flex-column gap-2">
                <label for="eventStartDate">Start date</label>
                <Calendar
                    id="eventStartDate"
                    v-model="duration"
                    show-time
                    hour-format="24"
                    name="eventStartDate" />
            </div>
            <div class="flex align-items-center justify-content-between gap-2 pt-1">
                <div class="flex align-items-center gap-2">
                    <Checkbox :value="shouldAddDuration" name="shouldAddDuration"></Checkbox>
                    <label for="shouldAddDuration">Add duration (hours)</label> 
                </div>
                <Calendar
                    v-if="shouldAddDuration"
                    id="eventStartDate"
                    class="h-2"
                    v-model="duration"
                    show-time
                    hour-format="24"
                    name="eventStartDate" />
            </div>
            <div class="flex flex-column gap-2">
                <label for="eventStartDate">End date</label>
                <Calendar
                    id="eventStartDate"
                    v-model="eventEndDate"
                    name="eventStartDate"
                    show-time
                    hour-format="24" />
            </div>
            <div class="flex w-full justify-content-flex-end pt-3">
                <Button
                    label="Submit"
                    icon="pi pi-check"
                    icon-pos="right" 
                    @click.prevent="addEvent" />
            </div>
        </div>
    </Dialog>
</template>


<style scoped>

</style>