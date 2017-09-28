import Vue from 'vue'
import Vuetify from 'vuetify';
import App from './App.vue'

// import('path/to/node_modules/vuetify/dist/vuetify.min.css');

Vue.use(Vuetify);
Vue.config.productionTip = false

new Vue({
  el: '#app',
  render: h => h(App)
})