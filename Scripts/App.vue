<template>
    <div id="app">
        <div v-if="player.signedIn">
            <h2>Planning Poker Vue APP</h2>

            <v-container fluid grid-list-sm>
                <v-layout row wrap>
                    <v-flex xs8>
                        <v-layout row wrap>
                            <v-flex xs4 sm3 md2 v-for="card in cards" :key="card.value">
                                <v-card class="purple white--text" @click="playCard(card.value)">
                                    <v-card-text>
                                        <h3>{{ card.label }}</h3>
                                    </v-card-text>
                                </v-card>
                            </v-flex>
                        </v-layout>
                    </v-flex>
                    <v-flex xs4>
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
        <v-container v-if="!player.signedIn">
            <v-layout row wrap>
                <v-flex xs12 sm10 offset-sm1 md6 offset-md3>
                    <v-card dark class="grey lighten-4">
                        <v-card-text>
                            <v-text-field v-model="player.name" name="playerName" label="Enter your name here" id="playerName"></v-text-field>
                            <v-btn outline class="indigo--text" @click="join">Join</v-btn>
                        </v-card-text>
                    </v-card>
                </v-flex>
            </v-layout>
        </v-container>
    </div>
</template>

<script>
import { HubConnection } from '@aspnet/signalr-client';
import PlayersOnline from '../Scripts/PlayersOnline.vue';

const HUBS = {
    POKER: '/poker'
}

const HUB_EVENTS = {
    Send: "Send",
    UsersJoined: "UsersJoined",
    UsersLeft: "UsersLeft",
    SetUsersOnline: "SetUsersOnline",
    JoinUser: "JoinUser"
}

const CARDS = [
    { value: 0, label: '0' },
    { value: 0.5, label: '1/2' },
    { value: 1, label: '1' },
    { value: 2, label: '2' },
    { value: 3, label: '3' },
    { value: 5, label: '5' },
    { value: 8, label: '8' },
    { value: 13, label: '13' },
    { value: 20, label: '20' },
    { value: 40, label: '40' },
    { value: 100, label: '80' },
    { value: Infinity, label: '?' },
];

export default {
    name: 'app',
    components: {
        PlayersOnline
    },
    data() {
        return {
            cards: CARDS,
            pokerHub: '',
            message: '',
            messages: [],
            player: {
                name: '',
                signedIn: false,
            },
            playersOnline: {}
        };
    },
    mounted() {
        this.pokerHub = new HubConnection(HUBS.POKER);

        this.pokerHub.on(HUB_EVENTS.Send, (message) => this.handleSend(message));

        this.pokerHub.on(HUB_EVENTS.UsersJoined, (data) => {
            console.log(`Joined`);
            console.log(data);
        });

        this.pokerHub.on(HUB_EVENTS.UsersLeft, (data) => {
            console.log(`Left`);
            console.log(data);
        });

        this.pokerHub.on(HUB_EVENTS.SetUsersOnline, (data) => {
            console.log(`SetUsersOnline`);
            console.log(data);
        });

        this.pokerHub.on(HUB_EVENTS.JoinUser, (message) => this.handleUserJoined(message));
        this.pokerHub.start();
    },
    methods: {
        join() {
            this.pokerHub.invoke(HUB_EVENTS.JoinUser, this.player.name);
        },
        playCard(card) {
            this.pokerHub.invoke(HUB_EVENTS.Send, card);
        },
        handleUserJoined(user) {
            console.log(`handleUserJoined`);
            console.log(user);
            this.player.signedIn = true;
            this.$set(this.playersOnline, user.ConnectionId, { Name: user.Name });
        },
        handleSend(message) {
            console.log(`handleSend`);
            console.log(message);
            this.messages.push(message);
            this.message = '';
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