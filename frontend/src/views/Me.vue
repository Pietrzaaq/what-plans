<script setup>
import { useCurrentUserStore } from "@/stores/currentUser.js";
import { computed } from 'vue';
import UserAvatar from "@/components/shared/UserAvatar.vue";
import { AVATAR_SIZE } from "@/models/avatarSize.js";
import { UPLOAD_URL } from "@/models/uploadUrl.js";

const currentUserStore = useCurrentUserStore();
const user = computed(() => currentUserStore.user);

function onAvatarUpload(event) {
    console.log('Avatar uploaded:', event);
}

function deleteAvatar() {
    user.value.avatarUrl = null;
}

function saveUser() {
    console.log('User saved:', user.value);
}
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
                <div  class="flex align-items-center gap-2 h-full">
                    <UserAvatar :user="user" :size="AVATAR_SIZE.XLARGE"></UserAvatar>
                    <div class="flex flex-column align-items-start">
                        <FileUpload
                            mode="basic"
                            class="file-upload-avatar p-button-icon-only p-button-info p-button-rounded p-button-text"
                            name="avatar"
                            accept="image/*"
                            upload-icon="fas fa-pen"
                            auto
                            :maxFileSize="5_000_000"
                            :url="UPLOAD_URL"
                            @upload="onAvatarUpload"/>
                        <Button icon="pi pi-trash" severity="danger" aria-label="Delete" text rounded @click="deleteAvatar"/>
                    </div>
                </div>
                <FileUpload
                    v-model="user.avatarUrl"
                    name="avatar"
                    mode="basic"
                    accept="image/*"
                    auto
                    :maxFileSize="5_000_000"
                    :url="UPLOAD_URL"
                    @upload="onAvatarUpload"/>
                <div class="form-field">
                    <label for="firstName">First Name</label>
                    <InputText v-model="user.firstName" id="firstName" />
                </div>
                <div class="form-field">
                    <label for="lastName">Last Name</label>
                    <InputText v-model="user.lastName" id="lastName" />
                </div>
                <div class="form-field">
                    <label for="username">Username</label>
                    <InputText v-model="user.username" id="username" />
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
