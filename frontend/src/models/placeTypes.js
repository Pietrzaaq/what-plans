const PLACE_TYPES = {
    THEATRE: 0,
    SPORTS_CENTRE: 1,
    SCENE: 2,
    RESTAURANT: 3,
    CLUB: 4,
    MUSEUM: 5,
    OTHER: 6,
    MULTI: 100
};

export const PLACE_TYPES_DATA = {
    [PLACE_TYPES.THEATRE]: {
        name: 'Theatre',
        icon: 'theater-masks',
        markerColor: 'purple',
    },
    [PLACE_TYPES.SPORTS_CENTRE]: {
        name: 'Sports Centre',
        icon: 'heart-pulse',
        markerColor: 'blue',
    },
    [PLACE_TYPES.SCENE]: {
        name: 'Scene',
        icon: 'music',
        markerColor: 'green',
    },
    [PLACE_TYPES.RESTAURANT]: {
        name: 'Restaurant',
        icon: 'utensils',
        markerColor: 'cadetblue',
    },
    [PLACE_TYPES.CLUB]: {
        name: 'Club',
        icon: 'glass-martini',
        markerColor: 'red',
    },
    [PLACE_TYPES.MUSEUM]: {
        name: 'Museum',
        icon: 'landmark',
        markerColor: 'red',
    },
    [PLACE_TYPES.OTHER]: {
        name: 'Other',
        icon: 'star',
        markerColor: 'orange',
    },
    [PLACE_TYPES.MULTI]: {
        name: 'Multi-purpose',
        icon: 'building',
        markerColor: 'darkblue',
    }
};

export { PLACE_TYPES };
