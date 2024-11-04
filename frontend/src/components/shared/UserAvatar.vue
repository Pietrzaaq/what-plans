<script setup>
import User from "@/models/user.js";
import { AVATAR_SIZE } from "@/models/avatarSize.js";
import { computed, onMounted, ref, watch } from "vue";
import { getImageUrl, getUserInitials } from "@/helpers/helpers.js";
import UserLight from "@/models/userLight.js";
import LongText from "@/components/shared/LongText.vue";
import imagesService from "@/services/imagesService.js";

const props = defineProps({
    user: {
        type: User
    },
    size: {
        type: String,
        default: AVATAR_SIZE.NORMAL
    },
    showName: {
        type: Boolean,
        default: false
    }
});

const user = ref(new UserLight);
const avatarUrl = ref(null);
const userInitials = computed(() => getUserInitials(user.value));

const setAvatarUrl = async () => {
    if (!user.value.avatarId) 
        return;
    
    const image = await imagesService.getById(user.value.avatarId);
    
    if (!image)
        return;
    
    if (image.url) {
        avatarUrl.value = image.url;
        return ;
    }

    const imageBinary = await imagesService.getBinaryById(user.value.avatarId);
    const blob = new Blob([imageBinary], { type: 'image/png;base64' });
    avatarUrl.value = getImageUrl(blob);
};

const setUser = async () => {
    if (props.user) {
        user.value = props.user;
        await setAvatarUrl();
    }
};

watch(props.user, setUser);

onMounted( () => {
    setUser();
});
</script>

<template>
    <div class="flex align-items-center">
        <Avatar
            v-if="avatarUrl"
            shape="circle"
            :size="size"
            :image="avatarUrl"
            class="wp-user-avatar font-medium"></Avatar>
        <Avatar
            v-else
            shape="circle"
            :size="size"
            :label="userInitials"
            class="wp-user-avatar font-medium"></Avatar>
        <LongText v-if="props.showName" :text="user.username" class="mr-2 font-medium">
            {{ user.username }}
        </LongText>
    </div>
</template>

<style scoped>
.wp-user-avatar {
    background-color: var(--primary-300);
    color: var(--primary-color-text)
}
</style>