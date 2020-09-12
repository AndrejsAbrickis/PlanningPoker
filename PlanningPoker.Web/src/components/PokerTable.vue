<template>
  <div class="pp-grid">
    <div class="pp-header u-ta-l">
      <button
        type="button"
        class="pp-header-menu-toggler"
        @click="ToggleSidebar"
      >
        <i class="material-icons">menu</i>
      </button>
    </div>
    <v-flex :class="{ visible: showSidebar }" class="grey lighten-4 pp-sidebar">
      <button type="button" class="pp-sidebar-close" @click="ToggleSidebar">
        <i class="material-icons">close</i>
      </button>
      <h3 class="h3">{{ player.groupId }}</h3>
      <players-online :user="player" :players="playersOnline" />
      <div class="u-mt-auto">
        <p class="text-xs-center mt-5">
          Made by
          <a
            href="https://www.abrickis.me"
            target="_blank"
            title="Andrejs Abrickis | abrickis.me"
            >Andrejs Abrickis</a
          >
        </p>
        <p class="text-xs-center">
          <a
            href="https://github.com/AndrejsAbrickis/signalR-vuejs-demo"
            target="_blank"
            ><GitHubIconSVG /></a>
        </p>
      </div>
    </v-flex>
    <div class="u-ps-16 u-d-flex u-fd-col">
      <games-stats
        v-if="gamesPlayed.length > 0"
        :games-played="gamesPlayed"
        :players="players"
      />
      <v-container fluid grid-list-md class="u-pb-16">
        <v-layout row wrap>
          <v-flex v-if="messages.length > 0" xs12>
            <h3>Played cards</h3>
          </v-flex>
          <v-flex
            v-for="messageItem in messages"
            :key="messageItem.connectionId"
            xs4
          >
            <v-card class="lime lighten-3">
              <v-card-text>
                {{ players[messageItem.connectionId].name }}
                <h4 v-show="isCardsRevealed">{{ messageItem.card }}</h4>
              </v-card-text>
            </v-card>
          </v-flex>
          <v-flex xs10 offset-xs-1>
            <v-btn
              v-if="!isCardsRevealed && messages.length > 0"
              color="warning"
              dark
              @click="showCards()"
              >Show cards</v-btn
            >
            <v-btn
              v-if="isCardsRevealed"
              color="warning"
              dark
              @click="newGame()"
              >New game</v-btn
            >
          </v-flex>
        </v-layout>
      </v-container>
      <cards-deck :play-card="playCard" class="u-mt-auto" />
    </div>
  </div>
</template>
<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator';
import CardsDeck from './CardsDeck.vue';
import GamesStats from './GamesStats.vue';
import PlayersOnline from './PlayersOnline.vue';
import GitHubIconSVG from './github-icon.svg';

@Component({
  components: {
    CardsDeck,
    GamesStats,
    PlayersOnline,
    GitHubIconSVG,
  },
  props: {
    gamesPlayed: Array,
    isCardsRevealed: Boolean,
    messages: Array,
    newGame: Function,
    playCard: Function,
    player: Object,
    showCards: Function,
  },
})
export default class PokerTable extends Vue {
  @Prop({ default: [] })
  private playersOnline!: any[];

  private showSidebar: boolean = false;

  private ToggleSidebar(): void {
    this.showSidebar = !this.showSidebar;
  }

  private get players() {
    return this.playersOnline.reduce((object, value) => {
      const newObjet = { ...object, ...{ [value.connectionId]: value } };
      return newObjet;
    }, {});
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
