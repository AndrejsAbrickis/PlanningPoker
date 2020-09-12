import Vue from "vue";
import vuetify from "./plugins/vuetify";
import App from "./App.vue";
import router from "./router";
import { HubConnectionBuilder } from '@aspnet/signalr';

Vue.config.productionTip = false;

const HUBS = {
  GAME: "https://localhost:5001/gamehub",
};

const gameHub = new HubConnectionBuilder().withUrl(HUBS.GAME).build();
gameHub.start();

Vue.prototype.$gameHub = gameHub;

new Vue({
  vuetify,
  router,
  render: (h) => h(App),
}).$mount("#app");
