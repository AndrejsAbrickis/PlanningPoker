<template>
  <div class="pp-grid">
    <div class="pp-header u-ta-l">
      <button 
        type="button" 
        class="pp-header-menu-toggler" 
        @click="ToggleSidebar">
        <i class="material-icons">menu</i>
      </button>
    </div>
    <v-flex 
      :class="{ visible: showSidebar }"
      class="grey lighten-4 pp-sidebar">
      <button 
        type="button" 
        class="pp-sidebar-close" 
        @click="ToggleSidebar">
        <i class="material-icons">close</i>
      </button>
      <h3 class="h3">{{ player.groupId }}</h3>
      <players-online 
        :user="player" 
        :players="playersOnline" />
      <div class="u-mt-auto">
        <p class="text-xs-center mt-5">
          Made by <a 
            href="https://www.abrickis.me" 
            target="_blank" 
            title="Andrejs Abrickis | abrickis.me">Andrejs Abrickis</a>
        </p>
        <p class="text-xs-center">
          <a 
            href="https://github.com/AndrejsAbrickis/signalR-vuejs-demo" 
            target="_blank"><svg 
              width="32px" 
              aria-labelledby="simpleicons-github-icon" 
              role="img" 
              viewBox="0 0 24 24" 
              xmlns="http://www.w3.org/2000/svg"><title id="simpleicons-github-icon">GitHub icon</title><path d="M12 .297c-6.63 0-12 5.373-12 12 0 5.303 3.438 9.8 8.205 11.385.6.113.82-.258.82-.577 0-.285-.01-1.04-.015-2.04-3.338.724-4.042-1.61-4.042-1.61C4.422 18.07 3.633 17.7 3.633 17.7c-1.087-.744.084-.729.084-.729 1.205.084 1.838 1.236 1.838 1.236 1.07 1.835 2.809 1.305 3.495.998.108-.776.417-1.305.76-1.605-2.665-.3-5.466-1.332-5.466-5.93 0-1.31.465-2.38 1.235-3.22-.135-.303-.54-1.523.105-3.176 0 0 1.005-.322 3.3 1.23.96-.267 1.98-.399 3-.405 1.02.006 2.04.138 3 .405 2.28-1.552 3.285-1.23 3.285-1.23.645 1.653.24 2.873.12 3.176.765.84 1.23 1.91 1.23 3.22 0 4.61-2.805 5.625-5.475 5.92.42.36.81 1.096.81 2.22 0 1.606-.015 2.896-.015 3.286 0 .315.21.69.825.57C20.565 22.092 24 17.592 24 12.297c0-6.627-5.373-12-12-12"/></svg></a>
        </p>
      </div>
    </v-flex>
    <div class="u-ps-16 u-d-flex u-fd-col">
      <games-stats 
        v-if="gamesPlayed.length > 0"
        :games-played="gamesPlayed"
        :players="playersOnline" /> 
      <v-container 
        fluid 
        grid-list-md 
        class="u-pb-16">
        <v-layout 
          row 
          wrap>
          <v-flex 
            v-if="messages.length > 0"
            xs12 >
            <h3>Played cards</h3>
          </v-flex>
          <v-flex  
            v-for="messageItem in messages" 
            :key="messageItem.connectionId"
            xs4>
            <v-card class="lime lighten-3">
              <v-card-text>
                {{ GetName(messageItem.connectionId) }}
                <h4 v-show="isCardsRevealed">{{ messageItem.message }}</h4>
              </v-card-text>
            </v-card>
          </v-flex>
          <v-flex 
            xs10 
            offset-xs-1>
            <v-btn 
              v-if="!isCardsRevealed && messages.length > 0" 
              color="warning" 
              dark
              @click="showCards()">Show cards</v-btn>
            <v-btn
              v-if="isCardsRevealed" 
              color="warning" 
              dark 
              @click="newGame()">New game</v-btn>
          </v-flex>
        </v-layout>
      </v-container>
      <cards-deck 
        :play-card="playCard" 
        class="u-mt-auto" />
    </div>
  </div>
</template>
<script lang="ts">
import Vue from 'vue';
/* eslint-disable-next-line no-unused-vars */
import Component from 'vue-class-component';
/* eslint-disable-next-line no-unused-vars */
import CardsDeck from './CardsDeck.vue';
/* eslint-disable-next-line no-unused-vars */
import GamesStats from './GamesStats.vue';
/* eslint-disable-next-line no-unused-vars */
import PlayersOnline from './PlayersOnline.vue';

@Component({
  components: {
    CardsDeck,
    GamesStats,
    PlayersOnline,
  },
  props: {
    gamesPlayed: Array,
    isCardsRevealed: Boolean,
    messages: Array,
    newGame: Function,
    playCard: Function,
    player: Object,
    playersOnline: Object,
    showCards: Function,
  },
})
export default class PokerTable extends Vue {
  /* eslint-disable-next-line no-undef */
  private showSidebar: boolean = false;
  /* eslint-disable-next-line no-undef */
  private playersOnline: any = this.playersOnline;

  private ToggleSidebar(): void {
  this.showSidebar = !this.showSidebar;
  }

  private GetName(connectionId: string): string {
      return this.playersOnline[connectionId] !== null ? this.playersOnline[connectionId].Name : 'NoName';
  }
}
</script>

<style scoped lang="scss">
$break-small: 576px;

.pp-grid {
  @media screen and (min-width: $break-small) {
    display: grid;
    grid-template-columns: 240px auto;
    height: 100%;
  }
}

.pp-header {
  @media screen and (min-width: $break-small) {
    display: none;
  }

  &-menu-toggler {
    margin: 0;
    padding: 12px;
  }
}

.pp-sidebar {
  bottom: 0;
  display: flex;
  flex-direction: column;
  left: -240px;
  position: absolute;
  top: 0;
  width: 240px;
  z-index: 100;

  &.visible {
    left: 0;
  }

  @media screen and (min-width: $break-small) {
    left: 0;
    position: relative;
    width: 100%;
  }

  &-close {
    padding: 11px;
    position: absolute;
    right: 0;
    @media screen and (min-width: $break-small) {
      display: none;
    }
  }
}

.h3 {
  text-transform: uppercase;
  font-size: 24px;
}

.u {
  &-mt-auto {
    margin-top: auto;
  }
  &-ps-16 {
    padding-left: 16px;
    padding-right: 16px;
  }
  &-d-flex {
    display: flex;
  }
  &-fd-col {
    flex-direction: column;
  }
  &-ta-l {
    text-align: left;
  }
}
</style>
