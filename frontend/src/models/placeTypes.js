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
        icon: 'theater-masks',  // FontAwesome icon for theatre
        markerColor: 'purple',
    },
    [PLACE_TYPES.SPORTS_CENTRE]: {
        name: 'Sports Centre',
        icon: 'futbol',  // FontAwesome icon for sports centre
        markerColor: 'green',
    },
    [PLACE_TYPES.SCENE]: {
        name: 'Scene',
        icon: 'stage',  // Custom or similar icon for a scene
        markerColor: 'blue',
    },
    [PLACE_TYPES.RESTAURANT]: {
        name: 'Restaurant',
        icon: 'utensils',  // FontAwesome icon for restaurants
        markerColor: 'cadetblue',
    },
    [PLACE_TYPES.CLUB]: {
        name: 'Club',
        icon: 'glass-martini',  // FontAwesome icon for clubs
        markerColor: 'red',
    },
    [PLACE_TYPES.MUSEUM]: {
        name: 'Museum',
        icon: 'landmark',  // FontAwesome icon for museums
        markerColor: 'orange',
    },
    [PLACE_TYPES.OTHER]: {
        name: 'Other',
        icon: 'question',  // FontAwesome icon for other places
        markerColor: 'gray',
    },
    [PLACE_TYPES.MULTI]: {
        name: 'Multi-purpose',
        icon: 'building',  // FontAwesome icon for multi-purpose buildings
        markerColor: 'darkblue',
    }
};

export { PLACE_TYPES };
