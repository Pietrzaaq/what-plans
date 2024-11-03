<script setup>
import User from "@/models/user.js";
import { AVATAR_SIZE } from "@/models/avatarSize.js";
import { computed, onMounted, ref, watch } from "vue";
import { getUserInitials } from "@/helpers/helpers.js";
import UserLight from "@/models/userLight.js";
import LongText from "@/components/shared/LongText.vue";

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
const userInitials = computed(() => getUserInitials(user.value));

const setUser = async () => {
    console.log('inside set user', props.user);
    if (props.user)
        user.value = props.user;
};

watch(props.user, setUser);

onMounted( () => {
    setUser();
});
</script>

<template>
    <div class="flex align-items-center">
        <Avatar
            v-if="user.avatarUrl"
            shape="circle"
            :size="size"
            :image="user.avatarUrl"
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