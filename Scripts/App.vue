<template>
    <div id="app">
        <div v-if="joined">
            <h2>Planning Poker Vue APP</h2>

            <v-container fluid grid-list-md>
                <v-layout row wrap>
                    <v-flex xs6 offset-xs1>
                        <cards-deck :playCard="playCard" />
                    </v-flex>
                    <v-flex xs3 offset-xs1>
                        <players-online :players="playersOnline" />
                    </v-flex>
                </v-layout>
            </v-container>

            <div>
                <h4>Cards</h4>
                <p v-for="messageItem in messages" :key="messageItem.ConnectionId">
                    {{ messageItem.Message }}
                </p>
            </div>

        </div>
        <login v-if="!joined" :join="join" />
    </div>
</template>

<script>
import { HubConnection } from '@aspnet/signalr-client';
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
    JoinUser: "JoinUser"
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
            joined: false,
            pokerHub: '',
            message: '',
            messages: [],
            player: {
                name: '',
            },
            playersOnline: {}
        };
    },
    mounted() {
        this.pokerHub = new HubConnection(HUBS.POKER);

        this.pokerHub.on(HUB_EVENTS.Send, this.handleSend);
        this.pokerHub.on(HUB_EVENTS.Connected, this.handleConnected);
        this.pokerHub.on(HUB_EVENTS.Disconnected, this.handleDisconnected);
        this.pokerHub.on(HUB_EVENTS.JoinUser, this.handleUserJoined);

        this.pokerHub.start();
    },
    methods: {
        handleConnected(usersOnline) {
            usersOnline.forEach(user => {
                this.$set(this.playersOnline, user.ConnectionId, { Name: user.Name || '' });
            })
        },
        handleDisconnected(usersOnline) {
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
            console.log(`handleUserJoined`);
            console.log(user);
            this.joined = true;
            this.$set(this.playersOnline, user.ConnectionId, { Name: user.Name });
        },
        handleSend(message) {
            console.log(`handleSend`);
            console.log(message);
            this.messages.push(message);
            this.message = '';
        },
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