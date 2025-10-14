export const PLACE_CATEGORIES = {
    FOOD_AND_DRINK: 'FoodAndDrink',
    EDUCATION: 'Education',
    ARTS_AND_CULTURE: 'ArtsAndCulture',
    ENTERTAINMENT: 'Entertainment',
    TOURISM: 'Tourism',
    HISTORIC: 'Historic',
    SPORTS_AND_RECREATION: 'SportsAndRecreation',
    COMMUNITY: 'Community',
    NATURE: 'Nature',
    OTHER: 'Other'
};

export const PLACE_TYPES = {
    // Food & Drink
    CAFE: 'Cafe',
    PUB: 'Pub',
    RESTAURANT: 'Restaurant',
    FAST_FOOD: 'FastFood',
    BIERGARTEN: 'Biergarten',

    // Education & Learning
    SCHOOL: 'School',
    UNIVERSITY: 'University',
    LIBRARY: 'Library',
    TRAINING_CENTER: 'TrainingCenter',
    DANCING_SCHOOL: 'DancingSchool',
    SURF_SCHOOL: 'SurfSchool',

    // Arts & Culture
    THEATRE: 'Theatre',
    MUSIC_VENUE: 'MusicVenue',
    ARTS_CENTRE: 'ArtsCentre',
    EXHIBITION_CENTRE: 'ExhibitionCentre',
    EVENTS_VENUE: 'EventsVenue',
    CONFERENCE_CENTRE: 'ConferenceCentre',
    STAGE: 'Stage',
    PLANETARIUM: 'Planetarium',

    // Entertainment & Leisure
    NIGHTCLUB: 'Nightclub',
    CASINO: 'Casino',
    AMUSEMENT_ARCADE: 'AmusementArcade',
    DANCE: 'Dance',
    ESCAPE_GAME: 'EscapeGame',
    FITNESS_CENTRE: 'FitnessCentre',
    SWIMMING_POOL: 'SwimmingPool',
    SUMMER_CAMP: 'SummerCamp',
    BEACH_RESORT: 'BeachResort',
    PARK: 'Park',
    NATURE_RESERVE: 'NatureReserve',
    HACKERSPACE: 'Hackerspace',

    // Tourism & Attractions
    MUSEUM: 'Museum',
    ZOO: 'Zoo',
    THEME_PARK: 'ThemePark',
    ATTRACTION: 'Attraction',
    CAMP_SITE: 'CampSite',
    CARAVAN_SITE: 'CaravanSite',

    // Historic & Heritage
    CASTLE: 'Castle',
    CHURCH: 'Church',

    // Sports & Recreation
    SPORTS_CENTRE: 'SportsCentre',
    STADIUM: 'Stadium',

    // Community & Social
    COMMUNITY_CENTRE: 'CommunityCentre',
    SOCIAL_CENTRE: 'SocialCentre',

    // Miscellaneous
    OTHER: 'Other',
    MULTI: 'Multi'
};

// Category metadata
export const PLACE_CATEGORIES_DATA = {
    [PLACE_CATEGORIES.FOOD_AND_DRINK]: {
        name: 'Food & Drink',
        color: '#FF6B6B',
        icon: 'utensils'
    },
    [PLACE_CATEGORIES.EDUCATION]: {
        name: 'Education',
        color: '#4ECDC4',
        icon: 'graduation-cap'
    },
    [PLACE_CATEGORIES.ARTS_AND_CULTURE]: {
        name: 'Arts & Culture',
        color: '#9B59B6',
        icon: 'palette'
    },
    [PLACE_CATEGORIES.ENTERTAINMENT]: {
        name: 'Entertainment',
        color: '#E74C3C',
        icon: 'gamepad'
    },
    [PLACE_CATEGORIES.TOURISM]: {
        name: 'Tourism',
        color: '#3498DB',
        icon: 'camera'
    },
    [PLACE_CATEGORIES.HISTORIC]: {
        name: 'Historic',
        color: '#95A5A6',
        icon: 'landmark'
    },
    [PLACE_CATEGORIES.SPORTS_AND_RECREATION]: {
        name: 'Sports & Recreation',
        color: '#2ECC71',
        icon: 'dumbbell'
    },
    [PLACE_CATEGORIES.COMMUNITY]: {
        name: 'Community',
        color: '#F39C12',
        icon: 'users'
    },
    [PLACE_CATEGORIES.NATURE]: {
        name: 'Nature',
        color: '#27AE60',
        icon: 'tree'
    },
    [PLACE_CATEGORIES.OTHER]: {
        name: 'Other',
        color: '#7F8C8D',
        icon: 'question'
    }
};

// Place Types metadata with icons and colors
export const PLACE_TYPES_DATA = {
    // Food & Drink
    [PLACE_TYPES.CAFE]: {
        name: 'Cafe',
        icon: 'coffee',
        markerColor: '#8B4513',
        category: PLACE_CATEGORIES.FOOD_AND_DRINK
    },
    [PLACE_TYPES.PUB]: {
        name: 'Pub',
        icon: 'beer',
        markerColor: '#DAA520',
        category: PLACE_CATEGORIES.FOOD_AND_DRINK
    },
    [PLACE_TYPES.RESTAURANT]: {
        name: 'Restaurant',
        icon: 'utensils',
        markerColor: '#FF6B6B',
        category: PLACE_CATEGORIES.FOOD_AND_DRINK
    },
    [PLACE_TYPES.FAST_FOOD]: {
        name: 'Fast Food',
        icon: 'burger',
        markerColor: '#FF4444',
        category: PLACE_CATEGORIES.FOOD_AND_DRINK
    },
    [PLACE_TYPES.BIERGARTEN]: {
        name: 'Biergarten',
        icon: 'beer-mug',
        markerColor: '#FFA500',
        category: PLACE_CATEGORIES.FOOD_AND_DRINK
    },

    // Education & Learning
    [PLACE_TYPES.SCHOOL]: {
        name: 'School',
        icon: 'school',
        markerColor: '#4ECDC4',
        category: PLACE_CATEGORIES.EDUCATION
    },
    [PLACE_TYPES.UNIVERSITY]: {
        name: 'University',
        icon: 'graduation-cap',
        markerColor: '#2980B9',
        category: PLACE_CATEGORIES.EDUCATION
    },
    [PLACE_TYPES.LIBRARY]: {
        name: 'Library',
        icon: 'book',
        markerColor: '#5DADE2',
        category: PLACE_CATEGORIES.EDUCATION
    },
    [PLACE_TYPES.TRAINING_CENTER]: {
        name: 'Training Center',
        icon: 'chalkboard-teacher',
        markerColor: '#48C9B0',
        category: PLACE_CATEGORIES.EDUCATION
    },
    [PLACE_TYPES.DANCING_SCHOOL]: {
        name: 'Dancing School',
        icon: 'person-dancing',
        markerColor: '#AF7AC5',
        category: PLACE_CATEGORIES.EDUCATION
    },
    [PLACE_TYPES.SURF_SCHOOL]: {
        name: 'Surf School',
        icon: 'water',
        markerColor: '#3498DB',
        category: PLACE_CATEGORIES.EDUCATION
    },

    // Arts & Culture
    [PLACE_TYPES.THEATRE]: {
        name: 'Theatre',
        icon: 'theater-masks',
        markerColor: '#9B59B6',
        category: PLACE_CATEGORIES.ARTS_AND_CULTURE
    },
    [PLACE_TYPES.MUSIC_VENUE]: {
        name: 'Music Venue',
        icon: 'music',
        markerColor: '#8E44AD',
        category: PLACE_CATEGORIES.ARTS_AND_CULTURE
    },
    [PLACE_TYPES.ARTS_CENTRE]: {
        name: 'Arts Centre',
        icon: 'palette',
        markerColor: '#A569BD',
        category: PLACE_CATEGORIES.ARTS_AND_CULTURE
    },
    [PLACE_TYPES.EXHIBITION_CENTRE]: {
        name: 'Exhibition Centre',
        icon: 'image',
        markerColor: '#BB8FCE',
        category: PLACE_CATEGORIES.ARTS_AND_CULTURE
    },
    [PLACE_TYPES.EVENTS_VENUE]: {
        name: 'Events Venue',
        icon: 'calendar-days',
        markerColor: '#7D3C98',
        category: PLACE_CATEGORIES.ARTS_AND_CULTURE
    },
    [PLACE_TYPES.CONFERENCE_CENTRE]: {
        name: 'Conference Centre',
        icon: 'building',
        markerColor: '#6C3483',
        category: PLACE_CATEGORIES.ARTS_AND_CULTURE
    },
    [PLACE_TYPES.STAGE]: {
        name: 'Stage',
        icon: 'film',
        markerColor: '#9B59B6',
        category: PLACE_CATEGORIES.ARTS_AND_CULTURE
    },
    [PLACE_TYPES.PLANETARIUM]: {
        name: 'Planetarium',
        icon: 'globe',
        markerColor: '#5B2C6F',
        category: PLACE_CATEGORIES.ARTS_AND_CULTURE
    },

    // Entertainment & Leisure
    [PLACE_TYPES.NIGHTCLUB]: {
        name: 'Nightclub',
        icon: 'glass-martini',
        markerColor: '#E74C3C',
        category: PLACE_CATEGORIES.ENTERTAINMENT
    },
    [PLACE_TYPES.CASINO]: {
        name: 'Casino',
        icon: 'dice',
        markerColor: '#C0392B',
        category: PLACE_CATEGORIES.ENTERTAINMENT
    },
    [PLACE_TYPES.AMUSEMENT_ARCADE]: {
        name: 'Amusement Arcade',
        icon: 'gamepad',
        markerColor: '#EC7063',
        category: PLACE_CATEGORIES.ENTERTAINMENT
    },
    [PLACE_TYPES.DANCE]: {
        name: 'Dance',
        icon: 'person-dancing',
        markerColor: '#E67E22',
        category: PLACE_CATEGORIES.ENTERTAINMENT
    },
    [PLACE_TYPES.ESCAPE_GAME]: {
        name: 'Escape Game',
        icon: 'key',
        markerColor: '#D35400',
        category: PLACE_CATEGORIES.ENTERTAINMENT
    },
    [PLACE_TYPES.FITNESS_CENTRE]: {
        name: 'Fitness Centre',
        icon: 'dumbbell',
        markerColor: '#16A085',
        category: PLACE_CATEGORIES.ENTERTAINMENT
    },
    [PLACE_TYPES.SWIMMING_POOL]: {
        name: 'Swimming Pool',
        icon: 'person-swimming',
        markerColor: '#3498DB',
        category: PLACE_CATEGORIES.ENTERTAINMENT
    },
    [PLACE_TYPES.SUMMER_CAMP]: {
        name: 'Summer Camp',
        icon: 'campground',
        markerColor: '#F39C12',
        category: PLACE_CATEGORIES.ENTERTAINMENT
    },
    [PLACE_TYPES.BEACH_RESORT]: {
        name: 'Beach Resort',
        icon: 'umbrella-beach',
        markerColor: '#F4D03F',
        category: PLACE_CATEGORIES.ENTERTAINMENT
    },
    [PLACE_TYPES.PARK]: {
        name: 'Park',
        icon: 'tree',
        markerColor: '#27AE60',
        category: PLACE_CATEGORIES.NATURE
    },
    [PLACE_TYPES.NATURE_RESERVE]: {
        name: 'Nature Reserve',
        icon: 'leaf',
        markerColor: '#229954',
        category: PLACE_CATEGORIES.NATURE
    },
    [PLACE_TYPES.HACKERSPACE]: {
        name: 'Hackerspace',
        icon: 'laptop-code',
        markerColor: '#34495E',
        category: PLACE_CATEGORIES.COMMUNITY
    },

    // Tourism & Attractions
    [PLACE_TYPES.MUSEUM]: {
        name: 'Museum',
        icon: 'landmark',
        markerColor: '#3498DB',
        category: PLACE_CATEGORIES.TOURISM
    },
    [PLACE_TYPES.ZOO]: {
        name: 'Zoo',
        icon: 'paw',
        markerColor: '#28B463',
        category: PLACE_CATEGORIES.TOURISM
    },
    [PLACE_TYPES.THEME_PARK]: {
        name: 'Theme Park',
        icon: 'ferris-wheel',
        markerColor: '#E91E63',
        category: PLACE_CATEGORIES.TOURISM
    },
    [PLACE_TYPES.ATTRACTION]: {
        name: 'Attraction',
        icon: 'camera',
        markerColor: '#5DADE2',
        category: PLACE_CATEGORIES.TOURISM
    },
    [PLACE_TYPES.CAMP_SITE]: {
        name: 'Camp Site',
        icon: 'tent',
        markerColor: '#A04000',
        category: PLACE_CATEGORIES.TOURISM
    },
    [PLACE_TYPES.CARAVAN_SITE]: {
        name: 'Caravan Site',
        icon: 'caravan',
        markerColor: '#85929E',
        category: PLACE_CATEGORIES.TOURISM
    },

    // Historic & Heritage
    [PLACE_TYPES.CASTLE]: {
        name: 'Castle',
        icon: 'chess-rook',
        markerColor: '#95A5A6',
        category: PLACE_CATEGORIES.HISTORIC
    },
    [PLACE_TYPES.CHURCH]: {
        name: 'Church',
        icon: 'church',
        markerColor: '#AAB7B8',
        category: PLACE_CATEGORIES.HISTORIC
    },

    // Sports & Recreation
    [PLACE_TYPES.SPORTS_CENTRE]: {
        name: 'Sports Centre',
        icon: 'heart-pulse',
        markerColor: '#2ECC71',
        category: PLACE_CATEGORIES.SPORTS_AND_RECREATION
    },
    [PLACE_TYPES.STADIUM]: {
        name: 'Stadium',
        icon: 'stadium',
        markerColor: '#27AE60',
        category: PLACE_CATEGORIES.SPORTS_AND_RECREATION
    },

    // Community & Social
    [PLACE_TYPES.COMMUNITY_CENTRE]: {
        name: 'Community Centre',
        icon: 'users',
        markerColor: '#F39C12',
        category: PLACE_CATEGORIES.COMMUNITY
    },
    [PLACE_TYPES.SOCIAL_CENTRE]: {
        name: 'Social Centre',
        icon: 'handshake',
        markerColor: '#E67E22',
        category: PLACE_CATEGORIES.COMMUNITY
    },

    // Miscellaneous
    [PLACE_TYPES.OTHER]: {
        name: 'Other',
        icon: 'star',
        markerColor: '#7F8C8D',
        category: PLACE_CATEGORIES.OTHER
    },
    [PLACE_TYPES.MULTI]: {
        name: 'Multi-purpose',
        icon: 'building',
        markerColor: '#34495E',
        category: PLACE_CATEGORIES.OTHER
    }
};

export function getPlaceTypeData(placeType) {
    return PLACE_TYPES_DATA[placeType] || PLACE_TYPES_DATA[PLACE_TYPES.OTHER];
}

export function getCategoryData(category) {
    return PLACE_CATEGORIES_DATA[category] || PLACE_CATEGORIES_DATA[PLACE_CATEGORIES.OTHER];
}

export function getPlaceTypesByCategory(category) {
    return Object.entries(PLACE_TYPES_DATA)
        .filter(([, data]) => data.category === category)
        .map(([type]) => type);
}

export function getMarkerColor(placeType) {
    return PLACE_TYPES_DATA[placeType]?.markerColor || '#7F8C8D';
}

export function getPlaceIcon(placeType) {
    return PLACE_TYPES_DATA[placeType]?.icon || 'star';
}