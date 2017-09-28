<template>
    <div id="app">
        <div v-if="player.signedIn">
            <h2>Planning Poker Vue APP</h2>
            <v-text-field v-model="message" name="message" label="Message" id="message"></v-text-field>
            <v-btn outline class="indigo--text" @click="sendMessage">Send Message</v-btn>
            <div>
                <h4>Messages</h4>
                <p v-for="messageItem in messages" :key="messageItem.ConnectionId">
                    {{ messageItem.Message }}
                </p>
            </div>
        </div>
        <v-container grid-list-xl text-xs-center v-if="!player.signedIn">
            <v-layout row wrap>
                <v-flex xs8 offset-xs2>
                    <v-card class="grey lighten-4">
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

export default {
    name: 'app',
    data() {
        return {
            pokerHub: '',
            message: '',
            messages: [],
            player: {
                name: '',
                signedIn: false,
            },
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
        sendMessage() {
            this.pokerHub.invoke(HUB_EVENTS.Send, this.message);
        },
        handleUserJoined(message) {
            console.log(`JoinUser called`);
            console.log(message);
            this.player.signedIn = true;
        },
        handleSend(message) {
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
    margin-top: 60px;
}
</style>