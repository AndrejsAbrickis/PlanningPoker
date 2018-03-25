<template>
  <v-app>
    <div 
      id="app"
      class="full-height">
      <h2 v-if="!joined">Planning Poker</h2>
      <login 
        v-if="!joined" 
        :join="joinGroup" 
        :player="player" />
      <poker-table
        v-if="joined" 
        :player="player" 
        :messages="messages" 
        :players-online="playersOnline"
        :play-card="playCard"
        :is-cards-revealed="isCardsRevealed"
        :show-cards="showCards"
        :new-game="newGame"
        :games-played="gamesPlayed"/>
    </div>
  </v-app>
</template>

<script lang="ts">
import { HubConnection } from '@aspnet/signalr';
import Vue from 'vue';
import Component from 'vue-class-component'; // eslint-disable-line
import Login from './Components/Login.vue';  // eslint-disable-line
import PokerTable from './Components/PokerTable.vue';  // eslint-disable-line
import EventBus, { Events } from './Services/EventBus';
import HUB_EVENTS from './Services/HubEvents';

const HUBS = {
  POKER: '/poker',
};

@Component({
  components: {
    Login,
    PokerTable,
  },
})
export default class App extends Vue {
  private name: string = 'app';
  private pokerHub: any = {}; // eslint-disable-line no-undef
  private joined: boolean = false; // eslint-disable-line no-undef
  private messages: any[] = []; // eslint-disable-line no-undef
  private player: object = {}; // eslint-disable-line no-undef
  private playersOnline: object = {}; // eslint-disable-line no-undef
  private isCardsRevealed: boolean = false; // eslint-disable-line no-undef
  private gamesPlayed: any[] = []; // eslint-disable-line no-undef

  private mounted(): void {
    this.pokerHub = new HubConnection(HUBS.POKER);
    this.pokerHub.start();

    this.pokerHub.on(HUB_EVENTS.Connected, this.handleConnected);
    this.pokerHub.on(HUB_EVENTS.Disconnected, this.handleDisconnected);
    this.pokerHub.on(HUB_EVENTS.UpdateUser, this.handleUpdateUser);
    this.pokerHub.on(HUB_EVENTS.Send, this.handleSend);
    this.pokerHub.on(HUB_EVENTS.UsersJoined, this.handleUserJoined);
    this.pokerHub.on(HUB_EVENTS.NewGame, this.handleNewGame);
    this.pokerHub.on(HUB_EVENTS.ShowCards, this.handleShowCards);
    this.pokerHub.on(HUB_EVENTS.JoinGroup, this.handleJoinGroup);
  }

  private handleConnected(usersOnline) {
    usersOnline.forEach((user) => {
      this.$set(this.playersOnline, user.connectionId, {
        Name: user.name || '',
      });
    });
  }

  private handleDisconnected(usersOnline) {
    this.playersOnline = {};
    this.handleConnected(usersOnline);
  }

  private join(playerName) {
    this.pokerHub.invoke(HUB_EVENTS.JoinUser, playerName);
  }

  private playCard(card) {
    this.pokerHub.invoke(HUB_EVENTS.Send, card);
  }

  private handleUserJoined(user: any) {
    this.joined = true;
    this.$set(this.playersOnline, user.connectionId, { Name: user.name });
  }

  private handleSend(message: string) {
    this.messages.push(message);
  }

  private newGame() {
    this.pokerHub.invoke(HUB_EVENTS.NewGame);
  }

  private handleNewGame() {
    EventBus.$emit(Events.NEW_GAME_STARTED);
    this.gamesPlayed.push(this.messages);
    this.messages = [];
    this.isCardsRevealed = false;
  }

  private showCards() {
    this.pokerHub.invoke(HUB_EVENTS.ShowCards);
  }

  private handleShowCards() {
    this.isCardsRevealed = true;
  }

  private joinGroup(playerName, groupId) {
    const message = { playerName, groupId };
    this.pokerHub.invoke(HUB_EVENTS.JoinGroup, message);

    if (history.pushState) {
      const url = `${window.location.protocol}//${window.location.host}${
        window.location.pathname
      }?groupId=${groupId}`;
      window.history.pushState({ path: url }, '', url);
    }
  }

  private handleJoinGroup(usersOnline) {
    this.playersOnline = {};
    this.handleConnected(usersOnline);
  }

  private handleUpdateUser(user) {
    this.joined = true;
    this.player = user;
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