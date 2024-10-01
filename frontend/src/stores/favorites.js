import { computed, ref } from 'vue';
import { defineStore } from "pinia";

export const useFavoritesStore = defineStore(
    'favorites', () => {
        const _favoritesPlaces = ref([]);
        const favoritesPlaces = computed(() => _favoritesPlaces.value);
        const _favoritesEvents = ref([]);
        const favoritesEvents = computed(() => _favoritesEvents.value);

        async function loadAll() {
            _favoritesEvents.value = JSON.parse(localStorage.getItem('favoritesEventsIds'));
            
            if (!_favoritesEvents.value) {
                const newCollection = [];
                localStorage.setItem('favoritesEventsIds', JSON.stringify(newCollection));
            }
            
            _favoritesPlaces.value = JSON.parse(localStorage.getItem('favoritesPlacesIds'));

            if (!_favoritesPlaces.value) {
                const newCollection = [];
                localStorage.setItem('favoritesPlacesIds', JSON.stringify(newCollection));
            }
        }
        
        function isPlaceFavorite(placeId) {
            return _favoritesPlaces.value.includes(placeId);
        }

        function isEventFavorite(eventId) {
            return _favoritesEvents.value.includes(eventId);
        }
        
        function togglePlaceFavorite(placeId, isFavorite) {
            if (isFavorite) {
                const updatedFavorites = _favoritesPlaces.value.filter(id => id !== placeId);
                _favoritesPlaces.value = updatedFavorites;
                localStorage.setItem('favoritesPlacesIds', JSON.stringify(updatedFavorites));
            } else {
                _favoritesPlaces.value.push(placeId);
                localStorage.setItem('favoritesPlacesIds', JSON.stringify(_favoritesPlaces.value));
            }
        }

        function toggleEventFavorite(eventId, isFavorite) {
            if (isFavorite) {
                const updatedFavorites = _favoritesEvents.value.filter(id => id !== eventId);
                _favoritesEvents.value = updatedFavorites;
                localStorage.setItem('favoritesEventsIds', JSON.stringify(updatedFavorites));
            } else {
                _favoritesEvents.value.push(eventId);
                localStorage.setItem('favoritesEventsIds', JSON.stringify(_favoritesEvents.value));
            }
        }

        return {
            favoritesPlaces,
            favoritesEvents,
            loadAll,
            isPlaceFavorite,
            isEventFavorite,
            togglePlaceFavorite,
            toggleEventFavorite
        };
    });