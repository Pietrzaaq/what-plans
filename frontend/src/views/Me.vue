<script setup>
import { useCurrentUserStore } from "@/stores/currentUser.js";
import { computed, onBeforeMount, ref } from 'vue';
import UserAvatar from "@/components/shared/UserAvatar.vue";
import { AVATAR_SIZE } from "@/models/avatarSize.js";
import { useToast } from "primevue/usetoast";
import imagesService from "@/services/imagesService.js";
import { useRouter } from "vue-router";
import { getApiDateTime } from "@/helpers/helpers.js";
import usersService from "@/services/usersService.js";

const currentUserStore = useCurrentUserStore();
const toast = useToast();
const router = useRouter();

const user = computed(() => currentUserStore.user);
const uploadedImage = ref(null);

async function onAvatarSelect(event) {
    console.log('onAvatarSelect', event);
    const file = event.files[0];
    const reader = new FileReader();

    reader.onload = async (e) => {
        uploadedImage.value = e.target.result;
    };

    reader.readAsDataURL(file);

    const formData = new FormData();
    formData.append('file', file);
    formData.append('relatedObjectType', 'User');
    formData.append('relatedObjectId', user.value.id);

    try {
        user.value.avatarId = await imagesService.upload(formData);
    } catch (error) {
        uploadedImage.value = null;
        toast.add({ severity: 'error', summary: 'Login', detail: 'Error occured while uploading file', life: 1000 });
    }
}

function deleteAvatar() {
    uploadedImage.value = null;
    user.value.avatarId = null;
}

async function saveUser() {
    const request = {
        firstName: user.value.firstName,
        lastName: user.value.lastName,
        birthDate: getApiDateTime(user.value.birthDate),
        culture: user.value.culture,
        avatarId: user.value.avatarId,
    };
    
    try {
        await usersService.update(request);
        await currentUserStore.reload();
        toast.add({ severity: 'success', summary: 'Register', detail: 'Saved successfully', life: 1000 });
    } catch (error) {
        toast.add({ severity: 'error', summary: 'Register', detail: 'Error occured', life: 1000 });
    }
}

onBeforeMount(() => {
    const userToken = localStorage.getItem('userToken');
    if (!userToken) {
        router.push("auth/login");
    }
});
</script>

<template>
    <Card v-if="user" class="p-4">
        <template #header>
            <div class="text-xl font-bold text-primary pt-2">
                {{ `${user.username} user profile`}}
            </div>
        </template>
        <template #content>
            <div class="user-details max-w-30rem">
                <div v-if="user.isAdmin" class="w-full">
                    <Chip label="Admin" icon="fa fa-user-tie"></Chip>
                </div>
                <div v-if="user.isOrganizer" class="w-full">
                    <Chip label="Organizer" icon="fa fa-screwdriver-wrench"></Chip>
                </div>
                <div class="flex align-items-center gap-2 h-full">
                    <UserAvatar v-if="!uploadedImage" :user="user" :size="AVATAR_SIZE.XLARGE"></UserAvatar>
                    <Avatar v-else :image="uploadedImage" :size="AVATAR_SIZE.XLARGE" shape="circle"></Avatar>
                    <div class="flex flex-column align-items-start">
                        <FileUpload mode="basic"
                                    class="file-upload-avatar p-button-icon-only p-button-info p-button-rounded p-button-text"
                                    name="avatar"
                                    accept="image/*"
                                    upload-icon="fas fa-pen"
                                    auto
                                    customUpload
                                    :maxFileSize="5_000_000"
                                    @select="onAvatarSelect"/>
                        <Button icon="pi pi-trash" severity="danger" aria-label="Delete" text rounded @click="deleteAvatar"/>
                    </div>
                </div>
                <div class="form-field">
                    <label for="username">Username</label>
                    <InputText v-model="user.username" disabled id="username" />
                </div>
                <div class="form-field">
                    <label for="firstName">First Name</label>
                    <InputText v-model="user.firstName" id="firstName" />
                </div>
                <div class="form-field">
                    <label for="lastName">Last Name</label>
                    <InputText v-model="user.lastName" id="lastName" />
                </div>
                <div class="form-field">
                    <label for="birthDate">Birth Date</label>
                    <Calendar v-model="user.birthDate" id="birthDate" dateFormat="yy-mm-dd" showIcon />
                </div>
                <div class="form-field">
                    <label for="culture">Culture</label>
                    <InputText v-model="user.culture" id="culture" />
                </div>
                <Button label="Save Changes" class="p-mt-3" @click="saveUser" />
            </div>
        </template>
    </Card>
</template>

<style>
.file-upload-avatar .p-button-label {
    display: none;
}

.file-upload-avatar {
    min-width: 0 !important;
}

.file-upload-avatar .p-button-icon-left {
    margin-right: 0 !important;
}
</style>

<style scoped>
.user-details {
    text-align: left;
    display: flex;
    flex-direction: column;
    gap: 2rem;
}

.form-field {
    display: flex;
    flex-direction: column;
    gap: 1rem;
}
</style>
