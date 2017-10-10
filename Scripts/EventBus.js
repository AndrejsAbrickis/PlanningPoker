// event-bus.js
import Vue from 'vue';

const EventBus = new Vue();

const Events = {
    NEW_GAME_STARTED: 'NEW_GAME_STARTED',
};

export { EventBus as default, Events };