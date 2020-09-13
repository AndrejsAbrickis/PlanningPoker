import Vue from 'vue';
import { HubConnectionBuilder } from '@aspnet/signalr';
import vuetify from './plugins/vuetify';
import App from './App.vue';
import router from './router';

Vue.config.productionTip = false;

const HUBS = {
  GAME: '/gamehub',
};

const gameHub = new HubConnectionBuilder().withUrl(HUBS.GAME).build();
gameHub.start();

Vue.prototype.$gameHub = gameHub;

new Vue({
  vuetify,
  router,
  render: (h) => h(App),
}).$mount('#app');
