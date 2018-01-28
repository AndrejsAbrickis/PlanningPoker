<template>
    <v-container fluid grid-list-md class="no-padding full-height">
        <v-layout row wrap class="full-height">
            <v-flex xs10 offset-xs1 sm4 offset-sm0 class="grey lighten-4 full-height">
                <h3 class="h3">{{ player.groupId }}</h3>
                <players-online :user="player" :players="playersOnline" />
                <p class="text-xs-center mt-5">
                    Made by <a href="https://www.abrickis.me" target="_blank" title="Andrejs Abrickis | abrickis.me">Andrejs Abrickis</a>
                </p>
                <p class="text-xs-center">
                    <a href="https://github.com/AndrejsAbrickis/signalR-vuejs-demo" target="_blank"><svg width="32px" aria-labelledby="simpleicons-github-icon" role="img" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><title id="simpleicons-github-icon">GitHub icon</title><path d="M12 .297c-6.63 0-12 5.373-12 12 0 5.303 3.438 9.8 8.205 11.385.6.113.82-.258.82-.577 0-.285-.01-1.04-.015-2.04-3.338.724-4.042-1.61-4.042-1.61C4.422 18.07 3.633 17.7 3.633 17.7c-1.087-.744.084-.729.084-.729 1.205.084 1.838 1.236 1.838 1.236 1.07 1.835 2.809 1.305 3.495.998.108-.776.417-1.305.76-1.605-2.665-.3-5.466-1.332-5.466-5.93 0-1.31.465-2.38 1.235-3.22-.135-.303-.54-1.523.105-3.176 0 0 1.005-.322 3.3 1.23.96-.267 1.98-.399 3-.405 1.02.006 2.04.138 3 .405 2.28-1.552 3.285-1.23 3.285-1.23.645 1.653.24 2.873.12 3.176.765.84 1.23 1.91 1.23 3.22 0 4.61-2.805 5.625-5.475 5.92.42.36.81 1.096.81 2.22 0 1.606-.015 2.896-.015 3.286 0 .315.21.69.825.57C20.565 22.092 24 17.592 24 12.297c0-6.627-5.373-12-12-12"></path></svg></a>
                </p>
            </v-flex>
            <v-flex xs10 offset-xs1 sm8 offset-sm0>
                <cards-deck :playCard="playCard" />
                <v-layout row wrap>
                    <v-flex xs12 v-if="messages.length > 0">
                        <h3>Played cards</h3>
                    </v-flex>
                    <v-flex xs4 v-for="messageItem in messages" :key="messageItem.connectionId">
                        <v-card class="lime lighten-3">
                            <v-card-text>
                                {{ playersOnline[messageItem.connectionId].Name }}
                                <h4 v-show="isCardsRevealed">{{ messageItem.message }}</h4>
                            </v-card-text>
                        </v-card>
                    </v-flex>
                    <v-flex xs10 offset-xs-1>
                        <v-btn color="warning" dark @click="showCards()" v-if="!isCardsRevealed && messages.length > 0">Show cards</v-btn>
                        <v-btn color="warning" dark @click="newGame()" v-if="isCardsRevealed">New game</v-btn>
                    </v-flex>
                </v-layout>
            </v-flex>
        </v-layout>
    </v-container>
</template>
<script>
// https://www.behance.net/gallery/54695697/FinDox-Accounting-Dashboard
import PlayersOnline from "./PlayersOnline.vue";
import CardsDeck from "./CardsDeck.vue";

export default {
  components: {
    PlayersOnline,
    CardsDeck
  },
  props: {
    player: Object,
    messages: Array,
    playersOnline: Object,
    playCard: Function,
    isCardsRevealed: Boolean,
    showCards: Function,
    newGame: Function
  },
  data() {
    return {
      title: " Hello Table!"
    };
  }
};
</script>

<style scoped>
.sidebar {
  background: #2c2f42;
}

.h3 {
  text-transform: uppercase;
  font-size: 24px;
}
</style>
