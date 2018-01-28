<template>
    <div id="app" class="full-height">
        <h2 v-if="!joined">Planning Poker</h2>
        <login v-if="!joined" :join="joinGroup" :player="player" />
        <poker-table v-if="joined" 
          :player="player" 
          :messages="messages" 
          :playersOnline="playersOnline"
          :playCard="playCard"
          :isCardsRevealed="isCardsRevealed"
          :showCards="showCards"
          :newGame="newGame"
          :gamesPlayed="gamesPlayed">
        </poker-table>
    </div>
</template>

<script>
import { HubConnection } from "@aspnet/signalr-client";
import EventBus, { Events } from "./EventBus";
import HUB_EVENTS from "./HubEvents";
import Login from "./Login.vue";
import PokerTable from "./PokerTable.vue";

const HUBS = {
  POKER: "/poker"
};

export default {
  name: "app",
  components: {
    Login,
    PokerTable
  },
  data() {
    return {
      pokerHub: "",
      joined: false,
      messages: [],
      player: {},
      playersOnline: {},
      isCardsRevealed: false,
      gamesPlayed: []
    };
  },
  mounted() {
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
    this.pokerHub.on(HUB_EVENTS.LeaveGroup, this.handleLeaveGroup);
  },
  methods: {
    handleConnected(usersOnline) {
      usersOnline.forEach(user => {
        this.$set(this.playersOnline, user.connectionId, {
          Name: user.name || ""
        });
      });
    },
    handleDisconnected(usersOnline) {
      this.playersOnline = {};

      usersOnline.forEach(user => {
        this.$set(this.playersOnline, user.connectionId, {
          Name: user.name || ""
        });
      });
    },
    join(playerName) {
      this.pokerHub.invoke(HUB_EVENTS.JoinUser, playerName);
    },
    playCard(card) {
      this.pokerHub.invoke(HUB_EVENTS.Send, card);
    },
    handleUserJoined(user) {
      this.joined = true;
      this.$set(this.playersOnline, user.connectionId, { Name: user.name });
    },
    handleSend(message) {
      this.messages.push(message);
      this.message = "";
    },
    newGame() {
      this.pokerHub.invoke(HUB_EVENTS.NewGame);
    },
    handleNewGame() {
      EventBus.$emit(Events.NEW_GAME_STARTED);
      this.gamesPlayed.push(this.messages);
      this.messages = [];
      this.isCardsRevealed = false;
    },
    showCards() {
      this.pokerHub.invoke(HUB_EVENTS.ShowCards);
    },
    handleShowCards() {
      this.isCardsRevealed = true;
    },
    joinGroup(playerName, groupId) {
      const message = { playerName, groupId };
      this.pokerHub.invoke(HUB_EVENTS.JoinGroup, message);

      if (history.pushState) {
        const url = `${window.location.protocol}//${window.location.host}${
          window.location.pathname
        }?groupId=${groupId}`;
        window.history.pushState({ path: url }, "", url);
      }
    },
    handleJoinGroup(usersOnline) {
      this.playersOnline = {};

      usersOnline.forEach(user => {
        this.$set(this.playersOnline, user.connectionId, {
          Name: user.name || ""
        });
      });
    },
    handleLeaveGroup() {
      console.warn(HUB_EVENTS.LeaveGroup);
    },
    handleUpdateUser(user) {
      this.joined = true;
      this.player = user;
    }
  }
};
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
  padding: 0!important;
}

.full-height {
  height: 100%;
}

.u-ta-l {
  text-align: left;
}
</style>