﻿import { FontAwesomeIcon } from "@fortawesome/vue-fontawesome";
import { library } from '@fortawesome/fontawesome-svg-core';
import {
    faGear,
    faUser,
    faMagnifyingGlass,
    faCalendar,
    faStar,
    faPeopleGroup,
    faClockRotateLeft,
    faPlus,
    faBasketball,
    faFutbol,
    faVolleyball,
    faCircleQuestion,
    faInfinity,
    faTableTennisPaddleBall,
    faCoffee,
    faLocationDot,
    faFilter,
    faCalendarDays,
    faMapPin,
    faHandPointer,
    faMasksTheater,
    faMusic,
    faUtensils,
    faGlassCheers,
    faClock,
    faHouse,
    faCircleXmark, 
    faBuilding,
    faHeartPulse,
    faGlassMartini,
    faLandmark
} from '@fortawesome/free-solid-svg-icons';

library.add(
    faGear,
    faUser,
    faMagnifyingGlass,
    faCalendar,
    faStar,
    faPeopleGroup,
    faClockRotateLeft,
    faPlus,
    faBasketball,
    faFutbol,
    faVolleyball,
    faCircleQuestion, 
    faInfinity, 
    faTableTennisPaddleBall,
    faCoffee,
    faLocationDot,
    faFilter,
    faCalendarDays,
    faMapPin,
    faHandPointer,    
    faMasksTheater,
    faMusic,
    faUtensils,
    faGlassCheers, 
    faClock,
    faHouse,
    faCircleXmark,
    faBuilding,
    faHeartPulse,
    faGlassMartini,
    faLandmark);

export function initFontAwesome(app) {
    app.component('font-awesome-icon', FontAwesomeIcon);
}