import { FontAwesomeIcon } from "@fortawesome/vue-fontawesome";
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
    faCoffee
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
    faCoffee);

export function initFontAwesome(app) {
    app.component('font-awesome-icon', FontAwesomeIcon);
}