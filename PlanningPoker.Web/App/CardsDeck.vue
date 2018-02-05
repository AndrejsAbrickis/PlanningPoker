<template>
    <div v-if="!hasVoted">
        <h3>Play card</h3>
        <v-container fluid grid-list-md class="u-pb-16">
            <v-layout row wrap>
                <v-flex xs4 sm3 md2 v-for="card in cards" :key="card.value">
                    <v-card class="purple white--text">
                        <v-card-text @click="vote(card.value)">
                            <h3>{{ card.label }}</h3>
                        </v-card-text>
                    </v-card>
                </v-flex>
            </v-layout>
        </v-container>
    </div>
</template>
<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import EventBus, { Events } from "./EventBus";

const CARDS = [
  { value: 0, label: "0" },
  { value: 0.5, label: "1/2" },
  { value: 1, label: "1" },
  { value: 2, label: "2" },
  { value: 3, label: "3" },
  { value: 5, label: "5" },
  { value: 8, label: "8" },
  { value: 13, label: "13" },
  { value: 20, label: "20" },
  { value: 40, label: "40" },
  { value: 80, label: "80" },
  { value: 999999, label: "?" }
];

@Component({
  props: {
    playCard: Function
  }
})
export default class GamesStats extends Vue {
  cards: any[] = CARDS;
  hasVoted: boolean = false;

  mounted(): void {
    EventBus.$on(Events.NEW_GAME_STARTED, this.newGame);
  }

  vote(card: any): void {
    this.hasVoted = true;
    this.playCard(card);
  }

  newGame(): void {
    this.hasVoted = false;
  }
}
</script>
