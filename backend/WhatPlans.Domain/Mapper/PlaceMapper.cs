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
        string sport = place.OpenStreetMapSport?.ToLowerInvariant();

        // Amenity-based types
        if (amenity is not null)
        {
            return amenity switch
            {
                // Food & Drink
                "biergarten" => (PlaceTypes.Biergarten, PlaceCategory.FoodAndDrink),
                "cafe" => (PlaceTypes.Cafe, PlaceCategory.FoodAndDrink),
                "pub" or "bar" => (PlaceTypes.Pub, PlaceCategory.FoodAndDrink),
                "restaurant" => (PlaceTypes.Restaurant, PlaceCategory.FoodAndDrink),
                "fast_food" => (PlaceTypes.FastFood, PlaceCategory.FoodAndDrink),
                
                // Education & Learning
                "school" or "training" => (PlaceTypes.School, PlaceCategory.Education),
                "college" or "university" => (PlaceTypes.University, PlaceCategory.Education),
                "library" => (PlaceTypes.Library, PlaceCategory.Education),
                "dancing_school" => (PlaceTypes.DancingSchool, PlaceCategory.Education),
                
                // Arts & Culture
                "theatre" => (PlaceTypes.Theatre, PlaceCategory.ArtsAndCulture),
                "conference_centre" => (PlaceTypes.ConferenceCentre, PlaceCategory.ArtsAndCulture),
                "events_venue" => (PlaceTypes.EventsVenue, PlaceCategory.ArtsAndCulture),
                "stage" => (PlaceTypes.Stage, PlaceCategory.ArtsAndCulture),
                "exhibition_centre" => (PlaceTypes.ExhibitionCentre, PlaceCategory.ArtsAndCulture),
                "arts_centre" => (PlaceTypes.ArtsCentre, PlaceCategory.ArtsAndCulture),
                "music_venue" => (PlaceTypes.MusicVenue, PlaceCategory.ArtsAndCulture),
                "planetarium" => (PlaceTypes.Planetarium, PlaceCategory.ArtsAndCulture),
                
                // Entertainment & Leisure
                "nightclub" => (PlaceTypes.Nightclub, PlaceCategory.Entertainment),
                "casino" => (PlaceTypes.Casino, PlaceCategory.Entertainment),
                "gambling" => (PlaceTypes.Casino, PlaceCategory.Entertainment),
                "game_arcade" or "amusement_arcade" => (PlaceTypes.AmusementArcade, PlaceCategory.Entertainment),
                
                // Community & Social
                "community_centre" => (PlaceTypes.CommunityCentre, PlaceCategory.Community),
                "social_centre" or "social_facility" => (PlaceTypes.SocialCentre, PlaceCategory.Community),
                
                _ => default
            };
        }

        // Leisure-based types
        if (leisure is not null)
        {
            return leisure switch
            {
                // Nature & Parks
                "park" or "garden" or "playground" => (PlaceTypes.Park, PlaceCategory.Nature),
                "nature_reserve" => (PlaceTypes.NatureReserve, PlaceCategory.Nature),
                
                // Sports & Recreation
                "fitness_centre" or "fitness_station" => (PlaceTypes.FitnessCentre, PlaceCategory.SportsAndRecreation),
                "swimming_pool" or "swimming_area" => (PlaceTypes.SwimmingPool, PlaceCategory.SportsAndRecreation),
                "sports_centre" => (PlaceTypes.SportsCentre, PlaceCategory.SportsAndRecreation),
                "stadium" or "pitch" => (PlaceTypes.Stadium, PlaceCategory.SportsAndRecreation),
                
                // Entertainment
                "escape_game" => (PlaceTypes.EscapeGame, PlaceCategory.Entertainment),
                "beach_resort" => (PlaceTypes.BeachResort, PlaceCategory.Entertainment),
                "dance" => (PlaceTypes.Dance, PlaceCategory.Entertainment),
                "summer_camp" => (PlaceTypes.SummerCamp, PlaceCategory.Entertainment),
                "amusement_arcade" => (PlaceTypes.AmusementArcade, PlaceCategory.Entertainment),
                
                // Community
                "hackerspace" or "hacklab" => (PlaceTypes.Hackerspace, PlaceCategory.Community),
                
                _ => default
            };
        }

        // Tourism-based types
        if (tourism is not null)
        {
            return tourism switch
            {
                "zoo" or "aquarium" => (PlaceTypes.Zoo, PlaceCategory.Tourism),
                "museum" => (PlaceTypes.Museum, PlaceCategory.Tourism),
                "theme_park" => (PlaceTypes.ThemePark, PlaceCategory.Tourism),
                "attraction" => (PlaceTypes.Attraction, PlaceCategory.Tourism),
                "camp_site" => (PlaceTypes.CampSite, PlaceCategory.Tourism),
                "caravan_site" => (PlaceTypes.CaravanSite, PlaceCategory.Tourism),
                "viewpoint" or "gallery" => (PlaceTypes.Attraction, PlaceCategory.Tourism),
                _ => default
            };
        }

        // Historic-based types
        if (historic is not null)
        {
            return historic switch
            {
                "castle" or "fort" or "fortress" => (PlaceTypes.Castle, PlaceCategory.Historic),
                "church" or "cathedral" or "chapel" or "monastery" or "shrine" or "temple" => (PlaceTypes.Church, PlaceCategory.Historic),
                "monument" or "memorial" or "ruins" => (PlaceTypes.Attraction, PlaceCategory.Historic),
                _ => default
            };
        }

        // Building-based types
        if (building is not null)
        {
            return building switch
            {
                "sports_centre" or "sports_hall" => (PlaceTypes.SportsCentre, PlaceCategory.SportsAndRecreation),
                "stadium" => (PlaceTypes.Stadium, PlaceCategory.SportsAndRecreation),
                "castle" => (PlaceTypes.Castle, PlaceCategory.Historic),
                "theatre" => (PlaceTypes.Theatre, PlaceCategory.ArtsAndCulture),
                "church" or "cathedral" or "chapel" => (PlaceTypes.Church, PlaceCategory.Historic),
                "museum" => (PlaceTypes.Museum, PlaceCategory.Tourism),
                "university" or "college" => (PlaceTypes.University, PlaceCategory.Education),
                "school" => (PlaceTypes.School, PlaceCategory.Education),
                _ => default
            };
        }

        // Sport-based types (if sport tag is present)
        if (sport is not null)
        {
            // Most sport-specific tags should map to sports facilities
            return (PlaceTypes.SportsCentre, PlaceCategory.SportsAndRecreation);
        }

        return (PlaceTypes.Other, PlaceCategory.Other);
    }
}