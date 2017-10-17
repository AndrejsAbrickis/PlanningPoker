<template>
    <div id="app">
        <div v-if="joined">
            <h2>Planning Poker Vue APP</h2>
            <p>GroupId: {{ player.GroupId }}</p>

            <v-container fluid grid-list-md>
                <v-layout row wrap>
                    <v-flex xs10 offset-xs1>
                        <players-online :players="playersOnline" />
                    </v-flex>
                    <v-flex xs10 offset-xs1>
                        <cards-deck :playCard="playCard" />
                        <v-layout row wrap>
                            <v-flex xs12 v-if="messages.length > 0">
                                <h3>Played cards</h3>
                            </v-flex>
                            <v-flex xs4 v-for="messageItem in messages" :key="messageItem.ConnectionId">
                                <v-card class="lime lighten-3" @click="vote(card.value)">
                                    <v-card-text>
                                        {{ playersOnline[messageItem.ConnectionId].Name }}
                                        <h4 v-show="isCardsRevealed">{{ messageItem.Message }}</h4>
                                    </v-card-text>
                                </v-card>
                            </v-flex>
                        </v-layout>
                    </v-flex>
                </v-layout>
                <v-layout row wrap>
                    <v-flex xs10 offset-xs-1>
                        <v-btn color="warning" dark @click="showCards()" v-if="!isCardsRevealed && messages.length > 0">Show cards</v-btn>
                        <v-btn color="warning" dark @click="newGame()" v-if="isCardsRevealed">New game</v-btn>
                    </v-flex>
                </v-layout>
            </v-container>
        </div>

        <login v-if="!joined" :join="joinGroup" :player="player" />
    </div>
</template>

<script>
import { HubConnection } from '@aspnet/signalr-client';
import EventBus, { Events } from './EventBus';
import Login from '../Scripts/Login.vue';
import PlayersOnline from '../Scripts/PlayersOnline.vue';
import CardsDeck from '../Scripts/CardsDeck.vue';


const HUBS = {
    POKER: '/poker'
}

const HUB_EVENTS = {
    Send: "Send",
    UsersJoined: "UsersJoined",
    Disconnected: "Disconnected",
    Connected: "Connected",
    JoinUser: "JoinUser",
    NewGame: 'NewGame',
    ShowCards: 'ShowCards',
    JoinGroup: "JoinGroup",
    LeaveGroup: "LeaveGroup",
    UpdateUser: "UpdateUser"
}

export default {
    name: 'app',
    components: {
        Login,
        PlayersOnline,
        CardsDeck
    },
    data() {
        return {
            pokerHub: '',
            joined: false,
            messages: [],
            player: {},
            playersOnline: {},
            isCardsRevealed: false,
        };
    },
    mounted() {
        this.pokerHub = new HubConnection(HUBS.POKER);

        this.pokerHub.on(HUB_EVENTS.Connected, this.handleConnected);
        this.pokerHub.on(HUB_EVENTS.Disconnected, this.handleDisconnected);
        this.pokerHub.on(HUB_EVENTS.UpdateUser, this.handleUpdateUser);
        this.pokerHub.on(HUB_EVENTS.Send, this.handleSend);
        this.pokerHub.on(HUB_EVENTS.UsersJoined, this.handleUserJoined);
        this.pokerHub.on(HUB_EVENTS.NewGame, this.handleNewGame);
        this.pokerHub.on(HUB_EVENTS.ShowCards, this.handleShowCards);
        this.pokerHub.on(HUB_EVENTS.JoinGroup, this.handleJoinGroup);
        this.pokerHub.on(HUB_EVENTS.LeaveGroup, this.handleLeaveGroup);

        this.pokerHub.start();
    },
    methods: {
        handleConnected(usersOnline) {
            console.warn(HUB_EVENTS.Connected);
            console.log(usersOnline);
            usersOnline.forEach(user => {
                this.$set(this.playersOnline, user.ConnectionId, { Name: user.Name || '' });
            })
        },
        handleDisconnected(usersOnline) {
            console.warn(HUB_EVENTS.Disconnected);
            console.log(HUB_EVENTS.Disconnected);
            this.playersOnline = {};

            usersOnline.forEach(user => {
                this.$set(this.playersOnline, user.ConnectionId, { Name: user.Name || '' });
            })
        },
        join(playerName) {
            this.pokerHub.invoke(HUB_EVENTS.JoinUser, playerName);
        },
        playCard(card) {
            this.pokerHub.invoke(HUB_EVENTS.Send, card);
        },
        handleUserJoined(user) {
            console.warn(HUB_EVENTS.JoinUser);
            console.log(user);
            this.joined = true;
            this.$set(this.playersOnline, user.ConnectionId, { Name: user.Name });
        },
        handleSend(message) {
            console.warn(HUB_EVENTS.Send);
            console.log(message);
            this.messages.push(message);
            this.message = '';
        },
        newGame() {
            this.pokerHub.invoke(HUB_EVENTS.NewGame);
        },
        handleNewGame() {
            console.warn(HUB_EVENTS.NewGame);
            EventBus.$emit(Events.NEW_GAME_STARTED);
            this.messages = [];
            this.isCardsRevealed = false;
        },
        showCards() {
            this.pokerHub.invoke(HUB_EVENTS.ShowCards);
        },
        handleShowCards() {
            console.warn(HUB_EVENTS.ShowCards)
            this.isCardsRevealed = true;
        },
        joinGroup(playerName, groupId) {
            const message = { playerName, groupId }
            this.pokerHub.invoke(HUB_EVENTS.JoinGroup, message);
        },
        handleJoinGroup(usersOnline) {
            console.warn(HUB_EVENTS.JoinGroup);
            console.log(usersOnline);
            // this.joined = true;
            // this.$set(this.playersOnline, user.ConnectionId, { Name: user.Name });
            this.playersOnline = {};

            usersOnline.forEach(user => {
                this.$set(this.playersOnline, user.ConnectionId, { Name: user.Name || '' });
            })
        },
        handleLeaveGroup() {
            console.warn(HUB_EVENTS.LeaveGroup);
        },
        handleUpdateUser(user) {
            console.warn(HUB_EVENTS.UpdateUser);
            console.log(user);
            this.joined = true;
            this.player = user;
        }
    }
};
</script>

<style>
#app {
    font-family: 'Avenir', Helvetica, Arial, sans-serif;
    -webkit-font-smoothing: antialiased;
    -moz-osx-font-smoothing: grayscale;
    text-align: center;
    color: #2c3e50;
    margin-top: 24px;
}
</style>