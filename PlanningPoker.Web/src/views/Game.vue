<template>
  <v-app>
    <div id="app" class="full-height">
      <template v-if="!player">
        <h2>Planning Poker</h2>
        <Login :player="player" @submit="joinGame" />
      </template>
      <PokerTable
        v-else
        :player="player"
        :messages="messages"
        :players-online="players"
        :play-card="playCard"
        :is-cards-revealed="isCardsRevealed"
        :show-cards="showCards"
        :new-game="startNewGame"
        :games-played="gamesPlayed"
      />
    </div>
  </v-app>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import Login from '@/components/Login.vue';
import PokerTable from '@/components/PokerTable.vue';
import {
  EventBus, Events, HUB_ACTIONS, HUB_EVENTS,
} from '@/services';

@Component({
  components: {
    Login,
    PokerTable,
  },
})
export default class App extends Vue {
  private $gameHub: any; // Dummy field to satisfy linter

  private messages: string[] = [];

  private player: any = null;

  private players: any[] = [];

  private pokerHub: any = {};

  private joined: boolean = false;

  private playersOnline: object = {};

  private isCardsRevealed: boolean = false;

  private gamesPlayed: any[] = [];

  private mounted(): void {
    this.$gameHub.on(HUB_EVENTS.GAME_JOINED, this.handleGameJoined);
    this.$gameHub.on(HUB_EVENTS.USER_JOINED, this.handleUserJoined);
    this.$gameHub.on(HUB_EVENTS.USER_LEFT, this.handleUserLeft);
    this.$gameHub.on(HUB_EVENTS.CARD_PLAYED, this.handleCardPlayed);
    this.$gameHub.on(HUB_EVENTS.CARDS_SHOWN, this.handleCardsShown);
    this.$gameHub.on(HUB_EVENTS.NEW_GAME_STARTED, this.handleNewGameStarted);
  }

  private joinGame({ name, groupId }: { name: string; groupId: string }) {
    this.$gameHub.invoke(HUB_EVENTS.JoinGroup, { name, groupId });

    if (window.history.pushState) {
      const url = `${window.location.protocol}//${window.location.host}${window.location.pathname}?groupId=${groupId}`;
      window.history.pushState({ path: url }, '', url);
    }
  }

  private handleGameJoined(player: unknown) {
    this.player = player;
  }

  private handleUserJoined(players: any) {
    this.players = players;
  }

  private handleUserLeft(players: any) {
    this.players = players;
  }

  private playCard(card: any) {
    this.$gameHub.invoke(HUB_ACTIONS.PLAY_CARD, String(card));
  }

  private handleCardPlayed(card: any) {
    this.messages.push(card);
  }

  private showCards() {
    this.$gameHub.invoke(HUB_ACTIONS.SHOW_CARDS);
  }

  private handleCardsShown() {
    this.isCardsRevealed = true;
  }

  private startNewGame() {
    this.$gameHub.invoke(HUB_ACTIONS.START_NEW_GAME);
  }

  private handleNewGameStarted() {
    EventBus.$emit(Events.NEW_GAME_STARTED);
    this.gamesPlayed.push(this.messages);
    this.messages = [];
    this.isCardsRevealed = false;
  }
}
</script>

<style>
#app {
  font-family: "Avenir", Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
}

.no-padding {
  padding: 0 !important;
}

.full-height {
  height: 100%;
}

.u-ta-l {
  text-align: left;
}

.u-pb-16 {
  padding-bottom: 16px;
}
</style>
