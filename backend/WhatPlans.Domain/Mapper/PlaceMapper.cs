using WhatPlans.Domain.Entities;
using WhatPlans.Domain.Enums;

namespace WhatPlans.Domain.Mapper;

public static class PlaceMapper
{
    public static (PlaceTypes type, PlaceCategory category) MapPlaceTypeAndCategory(Place place)
    {
        string amenity = place.OpenStreetMapAmenity?.ToLowerInvariant();
        string building = place.OpenStreetMapBuilding?.ToLowerInvariant();
        string historic = place.OpenStreetMapHistoric?.ToLowerInvariant();
        string leisure = place.OpenStreetMapLeisure?.ToLowerInvariant();
        string tourism = place.OpenStreetMapTourism?.ToLowerInvariant();

        // Amenity-based types
        if (amenity is not null)
        {
            return amenity switch
            {
                "biergarten" => (PlaceTypes.Biergarten, PlaceCategory.FoodAndDrink),
                "cafe" => (PlaceTypes.Cafe, PlaceCategory.FoodAndDrink),
                "pub" => (PlaceTypes.Pub, PlaceCategory.FoodAndDrink),
                "restaurant" => (PlaceTypes.Restaurant, PlaceCategory.FoodAndDrink),
                "fast_food" => (PlaceTypes.FastFood, PlaceCategory.FoodAndDrink),
                "school" or "training" => (PlaceTypes.School, PlaceCategory.Education),
                "university" => (PlaceTypes.University, PlaceCategory.Education),
                "library" => (PlaceTypes.Library, PlaceCategory.Education),
                "conference_centre" => (PlaceTypes.ConferenceCentre, PlaceCategory.ArtsAndCulture),
                "events_venue" or "stage" => (PlaceTypes.MusicVenue, PlaceCategory.ArtsAndCulture),
                "exhibition_centre" => (PlaceTypes.ExhibitionCentre, PlaceCategory.ArtsAndCulture),
                "arts_centre" => (PlaceTypes.ArtsCentre, PlaceCategory.ArtsAndCulture),
                "music_venue" => (PlaceTypes.MusicVenue, PlaceCategory.ArtsAndCulture),
                "planetarium" => (PlaceTypes.Planetarium, PlaceCategory.ArtsAndCulture),
                "nightclub" => (PlaceTypes.Nightclub, PlaceCategory.Entertainment),
                "casino" => (PlaceTypes.Casino, PlaceCategory.Entertainment),
                "community_centre" => (PlaceTypes.CommunityCentre, PlaceCategory.Community),
                "social_centre" => (PlaceTypes.SocialCentre, PlaceCategory.Community),
                "dancing_school" => (PlaceTypes.Dance, PlaceCategory.Education),
                _ => default
            };
        }

        // Leisure-based types
        if (leisure is not null)
        {
            return leisure switch
            {
                "park" => (PlaceTypes.Park, PlaceCategory.Nature),
                "fitness_centre" => (PlaceTypes.FitnessCentre, PlaceCategory.SportsAndRecreation),
                "swimming_pool" => (PlaceTypes.SwimmingPool, PlaceCategory.SportsAndRecreation),
                "escape_game" => (PlaceTypes.EscapeGame, PlaceCategory.Entertainment),
                "beach_resort" => (PlaceTypes.BeachResort, PlaceCategory.Entertainment),
                "dance" => (PlaceTypes.Dance, PlaceCategory.Entertainment),
                "hackerspace" => (PlaceTypes.SocialCentre, PlaceCategory.Community),
                "sports_centre" => (PlaceTypes.SportsCentre, PlaceCategory.SportsAndRecreation),
                "summer_camp" => (PlaceTypes.CampSite, PlaceCategory.Tourism),
                _ => default
            };
        }

        // Tourism-based types
        if (tourism is not null)
        {
            return tourism switch
            {
                "zoo" => (PlaceTypes.Zoo, PlaceCategory.Tourism),
                "museum" => (PlaceTypes.Museum, PlaceCategory.ArtsAndCulture),
                "theme_park" => (PlaceTypes.Attraction, PlaceCategory.Tourism),
                "attraction" => (PlaceTypes.Attraction, PlaceCategory.Tourism),
                "camp_site" or "caravan_site" => (PlaceTypes.CampSite, PlaceCategory.Tourism),
                _ => default
            };
        }

        // Historic-based types
        if (historic is not null)
        {
            return historic switch
            {
                "castle" => (PlaceTypes.Castle, PlaceCategory.Historic),
                "church" => (PlaceTypes.Church, PlaceCategory.Historic),
                _ => default
            };
        }

        // Building-based types
        if (building is not null)
        {
            return building switch
            {
                "sports_centre" => (PlaceTypes.SportsCentre, PlaceCategory.SportsAndRecreation),
                "stadium" => (PlaceTypes.SportsCentre, PlaceCategory.SportsAndRecreation),
                "castle" => (PlaceTypes.Castle, PlaceCategory.Historic),
                _ => default
            };
        }

        return (PlaceTypes.Other, PlaceCategory.Other);
    }
}