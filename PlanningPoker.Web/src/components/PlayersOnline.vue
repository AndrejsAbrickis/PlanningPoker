<template>
  <div class="u-ta-l">
    <h4>Online ({{ Object.keys(players).length }})</h4>
    <div>
      <v-chip class="grey lighten-2">
        <v-avatar class="orange">{{ user.name.charAt(0) }}</v-avatar>
        {{ user.name }}
      </v-chip>
    </div>
    <div
      v-for="(player, key) in playersOnline"
      :key="key">
      <v-chip class="grey lighten-2">
        <v-avatar class="teal">{{ player.Name.charAt(0) }}</v-avatar>
        {{ player.Name }}
      </v-chip>
    </div>
  </div>
</template>
<script lang="ts">
import Vue from 'vue';
/* eslint-disable-next-line no-unused-vars */
import Component from 'vue-class-component';

@Component({
  props: {
  players: Object,
  user: Object,
  },
  })
export default class PlayersOnline extends Vue {
  /* eslint-disable-next-line no-undef */
  private players: any;

  /* eslint-disable-next-line no-undef */
  private user: any;

  get playersOnline(): any[] {
    return Object.keys(this.players)
      .filter(playerKey => playerKey !== this.user.connectionId)
      .map(playerKey => this.players[playerKey]);
  }
}
</script>


<style scoped>
h4 {
  font-size: 18px;
  margin: 0;
  padding-left: 6px;
}
.u-ta-l {
  text-align: left;
}
</style>
