const EVENT_TYPES = {
    CULTURAL: 0,
    MUSICAL: 1,
    SPORT: 2,
    CULINARY: 3,
    CASUAL: 4,
    PARTY: 5,
    OTHER: 6
};

export const EVENT_TYPES_DATA = {
    [EVENT_TYPES.CULTURAL]: {
        name: 'Cultural',
        icon: 'masks-theater',
        markerColor: 'purple',
    },
    [EVENT_TYPES.MUSICAL]: {
        name: 'Musical',
        icon: 'music',
        markerColor: 'blue',
    },
    [EVENT_TYPES.SPORT]: {
        name: 'Sport',
        icon: 'futbol',  
        markerColor: 'green',
    },
    [EVENT_TYPES.CULINARY]: {
        name: 'Culinary',
        icon: 'utensils',
        markerColor: 'cadetblue',
    },
    [EVENT_TYPES.PARTY]: {
        name: 'Party',
        icon: 'glass-cheers',  
        markerColor: 'red',
    },
    [EVENT_TYPES.OTHER]: {
        name: 'Other',
        icon: 'star',  
        markerColor: 'orange',
    }
};