<template>
  <v-app>
    <div id="app" class="full-height">
      <template v-if="!joined">
        <h2>Planning Poker</h2>
        <Login :player="player" @submit="joinGroup" />
      </template>
      <poker-table
        v-if="joined"
        :player="player"
        :messages="messages"
        :players-online="playersOnline"
        :play-card="playCard"
        :is-cards-revealed="isCardsRevealed"
        :show-cards="showCards"
        :new-game="newGame"
        :games-played="gamesPlayed"
      />
    </div>
  </v-app>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import { HubConnectionBuilder } from '@aspnet/signalr';
import Login from '@/components/Login.vue';
import PokerTable from '@/components/PokerTable.vue';
import { EventBus, Events, HUB_EVENTS } from '@/services';

const HUBS = {
  GAME: 'https://localhost:5001/gamehub',
};

@Component({
  components: {
    Login,
    PokerTable,
  },
})
export default class App extends Vue {
  private pokerHub: any = {};

  private joined: boolean = false;

  private messages: any[] = [];

  private player: object = {};

  private playersOnline: object = {};

  private isCardsRevealed: boolean = false;

  private gamesPlayed: any[] = [];

  private mounted(): void {
    this.pokerHub = new HubConnectionBuilder().withUrl(HUBS.GAME).build();
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

  private handleConnected(usersOnline: any) {
    usersOnline.forEach((user: any) => {
      this.$set(this.playersOnline, user.connectionId, {
        Name: user.name || '',
      });
    });
  }

  private handleDisconnected(usersOnline: any) {
    this.playersOnline = {};
    this.handleConnected(usersOnline);
  }

  private join(playerName: any) {
    this.pokerHub.invoke(HUB_EVENTS.JoinUser, playerName);
  }

  private playCard(card: any) {
    this.pokerHub.invoke(HUB_EVENTS.Send, card);
  }

  private handleUserJoined(user: any) {
    this.joined = true;
    this.$set(this.playersOnline, user.connectionId, { Name: user.playerName });
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

  private joinGroup({
    name,
    groupId,
  }: {
    name: string;
    groupId: string;
  }) {
    this.pokerHub.invoke(HUB_EVENTS.JoinGroup, { name, groupId });

    if (window.history.pushState) {
      const url = `${window.location.protocol}//${window.location.host}${window.location.pathname}?groupId=${groupId}`;
      window.history.pushState({ path: url }, '', url);
    }
  }

  private handleJoinGroup(usersOnline: any) {
    this.playersOnline = {};
    this.handleConnected(usersOnline);
  }

  private handleUpdateUser(user: any) {
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
