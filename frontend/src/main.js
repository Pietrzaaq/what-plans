import { createApp } from 'vue';
import App from './App.vue';
import router from './router';

import '@/assets/styles.scss';
import '@/assets/app.css';

import { initFontAwesome } from '@/plugins/fontAwesome.js';
import '@fortawesome/fontawesome-free/css/all.css';
import { createPinia } from "pinia";
import LongText from "@/components/shared/LongText.vue";
import { initPrimeVue } from "@/plugins/primeVue.js";
import { initLeaflet } from "@/plugins/leaflet.js";

const app = createApp(App);
const pinia = createPinia();

app.use(pinia);
app.use(router);

initPrimeVue(app);
initFontAwesome(app);
initLeaflet();

app.component('LongText', LongText);

app.mount('#app');