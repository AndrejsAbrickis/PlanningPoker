<template>
    <div class="u-ta-l">
        <h4>Online ({{ Object.keys(players).length }})</h4>
        <div>
            <v-chip class="grey lighten-2">
                <v-avatar class="orange">{{ user.name.charAt(0) }}</v-avatar>
                {{ user.name }}
            </v-chip>
        </div>
        <div v-for="(player, key) in playersOnline" :key="key">
            <v-chip class="grey lighten-2">
                <v-avatar class="teal">{{ player.Name.charAt(0) }}</v-avatar>
                {{ player.Name }}
            </v-chip>
        </div>
    </div>
</template>
<script>
export default {
  props: {
    players: Object,
    user: Object
  },
  computed: {
    playersOnline() {
      return Object.keys(this.players)
        .filter(playerKey => playerKey != this.user.connectionId)
        .map(playerKey => this.players[playerKey]);
    }
  }
};
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


