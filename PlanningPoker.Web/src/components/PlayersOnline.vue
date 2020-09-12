<template>
  <div class="u-ta-l">
    <h4>Online ({{ Object.keys(players).length }})</h4>
    <div>
      <v-chip class="grey lighten-2 pl-1">
        <v-avatar class="orange mr-2">{{ user.name.charAt(0) }}</v-avatar>
        {{ user.name }}
      </v-chip>
    </div>
    <div v-for="(player, key) in playersOnline" :key="key">
      <v-chip class="grey lighten-2 pl-1">
        <v-avatar class="teal lighten-2 mr-2" v-if="player.name">
          {{ player.name.charAt(0) }}
        </v-avatar>
        {{ player.name }}
      </v-chip>
    </div>
  </div>
</template>
<script lang="ts">
import Vue from 'vue';
import { Component, Prop } from 'vue-property-decorator';

@Component({})
export default class PlayersOnline extends Vue {
  @Prop()
  private players: any;

  @Prop()
  private user: any;

  get playersOnline(): any[] {
    return this.players.filter(
      ({ connectionId }: { connectionId: string }) => connectionId !== this.user.connectionId,
    );
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
