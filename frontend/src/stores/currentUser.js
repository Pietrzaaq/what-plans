import { computed, ref } from 'vue';
import { defineStore } from "pinia";
import axios from "axios";
import usersService from "@/services/usersService.js";

export const useCurrentUserStore = defineStore(
    'currentUser', () => {
        const _user = ref(null);
        const _isAuthenticated = ref(false);

        const user = computed(() => _user.value);
        const isAuthenticated = computed(() => _isAuthenticated.value);

        async function setUser(token) {
            axios.defaults.headers.common['Authorization'] = `Bearer ${token}`;

            try {
                _user.value = await usersService.me();
                _isAuthenticated.value = true;
            }
            catch (error) {
                console.error('Token is invalid or expired:', error);
                localStorage.removeItem('userToken');
                _user.value = null;
                _isAuthenticated.value = false;
            }
        }
        
        async function reload() {
            try {
                _user.value = await usersService.me();
                _isAuthenticated.value = true;
            }
            catch (error) {
                console.error('Token is invalid or expired:', error);
                localStorage.removeItem('userToken');
                _user.value = null;
                _isAuthenticated.value = false;
            }
        }
        
        function logout() {
            localStorage.removeItem('userToken');
            _user.value = null;
            _isAuthenticated.value = false;
        }
        
        return {
            user,
            isAuthenticated,
            setUser,
            reload,
            logout
        };
    });