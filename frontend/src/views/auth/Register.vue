<script setup>
import { ref, computed } from 'vue';
import { useLayout } from '@/layout/composables/layout';
import usersService from "@/services/usersService.js";
import { useToast } from "primevue/usetoast";
import { useRouter } from "vue-router";
import { getApiDateTime } from "@/helpers/helpers.js";

const { layoutConfig } = useLayout();
const toast = useToast();
const router = useRouter();

const username = ref('');
const firstName = ref('');
const lastName = ref('');
const email = ref('');
const password = ref('');
const birthdate = ref(null);
const checked = ref(false);

const logoUrl = computed(() => {
    return `../../public/layout/images/logo.png`;
});

async function register() {
    const request = {
        username: username.value,
        firstName: firstName.value,
        lastName: lastName.value,
        email: email.value,
        birthDate: getApiDateTime(birthdate.value),
        password: password.value,
        culture: 'pl-PL'
    };

    try {
        await usersService.register(request);
        toast.add({ severity: 'success', summary: 'Register', detail: 'Registration successful', life: 1000 });
        await router.replace('login');
    } catch (e) {
        toast.add({ severity: 'error', summary: 'Register', detail: 'Registration failed', life: 1000 });
    }
}
</script>

<template>
    <div class="surface-ground flex align-items-center justify-content-center min-h-screen overflow-hidden">
        <div class="flex flex-column align-items-center justify-content-center">
            <img :src="logoUrl" alt="WhatPlans logo" class="mb-5 w-6rem flex-shrink-0"/>
            <div style="border-radius: 56px; padding: 0.3rem; background: linear-gradient(180deg, var(--primary-color) 10%, rgba(33, 150, 243, 0) 30%)">
                <div class="w-full surface-card py-8 px-5 sm:px-8" style="border-radius: 53px">
                    <div class="text-center mb-5">
                        <span class="text-600 font-medium">Create an account</span>
                    </div>

                    <div>
                        <label for="username" class="block text-900 text-xl font-medium mb-2">Username</label>
                        <InputText
                            id="username" type="text" placeholder="Username" class="w-full md:w-30rem mb-5"
                            style="padding: 1rem" v-model="username"/>

                        <label for="email" class="block text-900 text-xl font-medium mb-2">Email</label>
                        <InputText
                            id="email" type="text" placeholder="Email address" class="w-full md:w-30rem mb-5"
                            style="padding: 1rem" v-model="email"/>

                        <label for="password" class="block text-900 font-medium text-xl mb-2">Password</label>
                        <Password
                            id="password" v-model="password" placeholder="Password" :toggleMask="true"
                            class="w-full mb-3" inputClass="w-full" :inputStyle="{ padding: '1rem' }"></Password>

                        <label for="birthdate" class="block text-900 font-medium text-xl mb-2">Birthdate</label>
                        <Calendar
                            id="birthdate" v-model="birthdate" placeholder="Birthdate" class="w-full mb-5"
                            :showIcon="true"/>

                        <div class="flex align-items-center justify-content-between mb-5 gap-5">
                            <div class="flex align-items-center">
                                <Checkbox v-model="checked" id="agreeTerms" binary class="mr-2"></Checkbox>
                                <label for="agreeTerms">I agree to the terms and conditions</label>
                            </div>
                        </div>
                        <Button label="Register" class="w-full p-3 text-xl" @click="register"></Button>
                    </div>
                </div>
            </div>
        </div>
        <Toast/>
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
