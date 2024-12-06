import { createRouter, createWebHistory } from 'vue-router';
import AppLayout from '@/layout/AppLayout.vue';

const router = createRouter({
    mode: 'history',
    history: createWebHistory(),
    routes: [
        {
            path: '/',
            component: AppLayout,
            redirect: '/map',
            children: [
                {
                    path: '/map',
                    name: 'map',
                    component: () => import('@/views/Map.vue')
                },
                {
                    path: '/favorites',
                    name: 'favorites',
                    component: () => import('@/views/Favorites.vue')
                },
                {
                    path: '/events',
                    name: 'events',
                    component: () => import('@/views/Events.vue')
                },
                {
                    path: '/community',
                    name: 'community',
                    component: () => import('@/views/Community.vue')
                },
                {
                    path: '/me',
                    name: 'me',
                    component: () => import('@/views/Me.vue')
                }
            ]
        },
        {
            path: '/auth/login',
            name: 'login',
            component: () => import('@/views/auth/Login.vue')
        },
        {
            path: '/auth/register',
            name: 'register',
            component: () => import('@/views/auth/Register.vue')
        },
        {
            path: '/auth/access',
            name: 'accessDenied',
            component: () => import('@/views/auth/Access.vue')
        },
        {
            path: '/auth/error',
            name: 'error',
            component: () => import('@/views/auth/Error.vue')
        }
    ]
});

export default router;
