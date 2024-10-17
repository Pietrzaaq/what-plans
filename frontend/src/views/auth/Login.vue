<script setup>
import { useLayout } from '@/layout/composables/layout';
import { ref, computed } from 'vue';
import usersService from "@/services/usersService.js";
import { useToast } from "primevue/usetoast";
import { useRouter } from "vue-router";
import { useCurrentUserStore } from "@/stores/currentUser.js";

const { layoutConfig } = useLayout();
const toast = useToast();
const router = useRouter();
const currentUserStore = useCurrentUserStore();

const email = ref('');
const password = ref('');
const checked = ref(false);

const logoUrl = computed(() => {
    return `../../public/layout/images/logo.png`;
});

async function login() {
    const request = {
        email: email.value,
        password: password.value,
    };

    console.log('before request', request);
    try {
        const token = await usersService.login(request);
        localStorage.setItem('userToken', token.toString());

        await currentUserStore.setUser(token);
        
        toast.add({ severity: 'success', summary: 'Login', detail: 'Logged successfully', life: 1000 });
        await router.replace('/');
        
    } catch (e) {
        toast.add({ severity: 'error', summary: 'Login', detail: 'Invalid credentials', life: 1000 });
    }
}
</script>

<template>
    <div class="surface-ground flex align-items-center justify-content-center min-h-screen min-w-screen overflow-hidden">
        <div class="flex flex-column align-items-center justify-content-center">
            <img :src="logoUrl" alt="WhatPlans logo" class="mb-5 w-6rem flex-shrink-0" />
            <div style="border-radius: 56px; padding: 0.3rem; background: linear-gradient(180deg, var(--primary-color) 10%, rgba(33, 150, 243, 0) 30%)">
                <div class="w-full surface-card py-8 px-5 sm:px-8" style="border-radius: 53px">
                    <div class="text-center mb-5">
                        <span class="text-600 font-medium">Sign in to continue</span>
                    </div>

                    <div>
                        <label for="email1" class="block text-900 text-xl font-medium mb-2">Email</label>
                        <InputText id="email1" type="text" placeholder="Email address" class="w-full md:w-30rem mb-5" style="padding: 1rem" v-model="email" />

                        <label for="password1" class="block text-900 font-medium text-xl mb-2">Password</label>
                        <Password id="password1" v-model="password" placeholder="Password" :toggleMask="true" class="w-full mb-3" inputClass="w-full" :inputStyle="{ padding: '1rem' }"></Password>

                        <div class="flex align-items-center justify-content-between mb-5 gap-5">
                            <div class="flex align-items-center">
                                <Checkbox v-model="checked" id="rememberme1" binary class="mr-2"></Checkbox>
                                <label for="rememberme1">Remember me</label>
                            </div>
                            <a class="font-medium no-underline ml-2 text-right cursor-pointer" style="color: var(--primary-color)">Forgot password?</a>
                        </div>
                        <Button label="Sign In" class="w-full p-3 text-xl" @click="login"></Button>
                    </div>
                </div>
            </div>
        </div>
        <Toast />
    </div>
</template>

<style scoped>
.pi-eye {
    transform: scale(1.6);
    margin-right: 1rem;
}

.pi-eye-slash {
    transform: scale(1.6);
    margin-right: 1rem;
}
</style>
