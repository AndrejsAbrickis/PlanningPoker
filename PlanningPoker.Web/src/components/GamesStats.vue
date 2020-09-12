<template>
  <div>
    <h3>Games stats</h3>
    <v-container fluid grid-list-sm>
      <v-layout v-for="(game, index) in gamesPlayed" :key="index" row wrap>
        <v-flex xs1>
          <span>{{ index + 1 }}.</span>
        </v-flex>
        <v-flex xs11 class="u-ta-l">
          <v-chip
            v-for="(vote, index) in game"
            :key="index"
            outlined
            color="teal lighten-2 pl-1"
          >
            <v-avatar class="teal lighten-2 black--text mr-2">{{
              vote.card
            }}</v-avatar>
            <span class="black--text" v-if="players[vote.connectionId]">{{
              players[vote.connectionId].name
            }}</span>
            <span class="black--text" v-else>{{ vote.connectionId }}</span>
          </v-chip>
        </v-flex>
      </v-layout>
    </v-container>
  </div>
</template>

<script lang="ts">
import Vue from 'vue';
import { Component, Prop } from 'vue-property-decorator';

@Component({})
export default class GamesStats extends Vue {
  @Prop()
  private gamesPlayed!: any;

  @Prop()
  private players!: any;
}
</script>
