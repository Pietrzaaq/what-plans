<script setup>
import { computed, onBeforeUnmount, onMounted, ref, watch } from 'vue';
import { useLayout } from '@/layout/composables/layout';
import { useCurrentUserStore } from "@/stores/currentUser.js";
import { storeToRefs } from "pinia";
import { useToast } from "primevue/usetoast";
import { useRouter } from "vue-router";
import UserAvatar from "@/components/shared/UserAvatar.vue";

const { onMenuToggle } = useLayout();
const router = useRouter();
const toast = useToast();
const currentUserStore = useCurrentUserStore();
const { user } = storeToRefs(currentUserStore);

const outsideClickListener = ref(null);
const topbarMenuActive = ref(false);
const menu = ref();

const userMenuItems = ref([]);

watch(user, () => {
    setUserItems();
});

const logoUrl = computed(() => {
    return `layout/images/logo-purple-dark.png`;
});

const onTopBarMenuButton = () => {
    topbarMenuActive.value = !topbarMenuActive.value;
};

const topbarMenuClasses = computed(() => {
    return topbarMenuActive.value ? 'layout-topbar-menu-mobile-active' : 'layout-topbar-menu-hidden'; 
});

const bindOutsideClickListener = () => {
    if (!outsideClickListener.value) {
        outsideClickListener.value = (event) => {
            if (isOutsideClicked(event)) {
                topbarMenuActive.value = false;
            }
        };
        document.addEventListener('click', outsideClickListener.value);
    }
};
const unbindOutsideClickListener = () => {
    if (outsideClickListener.value) {
        document.removeEventListener('click', outsideClickListener.value);
        outsideClickListener.value = null;
    }
};
const isOutsideClicked = (event) => {
    if (!topbarMenuActive.value) return;

    const sidebarEl = document.querySelector('.layout-topbar-menu');
    const topbarEl = document.querySelector('.layout-topbar-menu-button');

    return !(sidebarEl.isSameNode(event.target) || sidebarEl.contains(event.target) || topbarEl.isSameNode(event.target) || topbarEl.contains(event.target));
};

const toggle = (event) => {
    menu.value.toggle(event);
};

const setUserItems = () => {
    if (user) {
        userMenuItems.value = [
            {
                label: 'Information',
                icon: 'fa fa-circle-info',
                iconColor: 'var(--blue-500)',
                route: '/me'
            },
            {
                label: 'Logout',
                icon: 'fa fa-circle-xmark',
                iconColor: 'var(--red-500)',
                command: () => {
                    currentUserStore.logout();
                    toast.add({ severity: 'success', summary: 'Success', detail: 'User logged out successfully', life: 2000 });
                    
                    setTimeout(() => {
                        window.location.reload();
                    }, 200);
                }
            }
        ];
    }
};

const login = async () => {
    await router.replace('auth/login');
};

const register = async () => {
    await router.replace('auth/register');
};

onMounted(() => {
    bindOutsideClickListener();
    setUserItems();
});

onBeforeUnmount(() => {
    unbindOutsideClickListener();
});
</script>

<template>
    <div class="layout-topbar">
        <router-link to="/" class="layout-topbar-logo">
            <img :src="logoUrl" alt="logo" />
            <span>What Plans</span>
        </router-link>

        <button class="p-link layout-menu-button layout-topbar-button" @click="onMenuToggle">
            <i class="pi pi-bars"></i>
        </button>

        <div class="layout-topbar-search p-fluid pl-2 w-4">
            <auto-complete style="display:flex; width: 100%" placeholder="Search for events..."></auto-complete>
        </div>

        <button class="p-link layout-topbar-menu-button layout-topbar-button" @click="onTopBarMenuButton()">
            <i class="pi pi-ellipsis-v"></i>
        </button>

        <div class="layout-topbar-menu flex align-items-center" :class="topbarMenuClasses">
            <button class="p-link layout-topbar-button">
                <i class="pi pi-calendar"></i>
                <span>Calendar</span>
            </button>
            <Button
                v-if="user"
                class="flex align-items-center justify-content-center p-2 p-link"
                style="border-radius: 50% !important; min-width: 2rem;"
                text
                rounded
                @click="toggle">
                <UserAvatar :user="user" style="border: 2px solid var(--text-color-secondary);; border-radius: 50%;"></UserAvatar>
            </Button>
            <div v-else class="flex gap-2 ml-2">
                <Button label="Log in" @click="login" outlined></Button>
                <Button label="Sign up" @click="register"></Button>
            </div>
            <Menu ref="menu" :model="userMenuItems" :popup="true" style="border-top: 40px solid var(--primary-color)">
                <template #start>
                    <div class="font-bold text-xl text-white text-center" style="margin-top: -35px; margin-bottom: 10px;">
                        <long-text :text="user.username"></long-text>
                    </div>
                </template>
                <template #item="{ item, props }">
                    <router-link v-if="item.route" v-slot="{ href, navigate }" :to="item.route" custom>
                        <a v-ripple :href="href" v-bind="props.action" @click="navigate">
                            <span :class="item.icon" :style="`color: ${item.iconColor}`" />
                            <span class="ml-2">{{ item.label }}</span>
                        </a>
                    </router-link>
                    <a v-else v-ripple :href="item.url" :target="item.target" v-bind="props.action">
                        <span :class="item.icon" :style="`color: ${item.iconColor}`" />
                        <span class="ml-2">{{ item.label }}</span>
                    </a>
                </template>
            </Menu>
        </div>
    </div>
</template>

<style lang="scss" scoped>
@media (max-width: 991px) {
    .layout-topbar-menu-hidden {
        visibility: hidden !important;
    }

    .layout-topbar-menu {
        flex-direction: column-reverse !important;
    }
}

</style>
