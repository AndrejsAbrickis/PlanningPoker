<template>
    <div id="app">
        <div v-if="player.signedIn">
            <h2>Planning Poker Vue APP</h2>

            <v-container fluid grid-list-sm>
                <v-layout row wrap>
                    <v-flex xs2 v-for="card in ['0', '1/2', '1', '2', '3', '5', '8', '10', '20', '40']" :key="card">
                        <v-card class="purple white--text" @click="playCard(card)">
                            <v-card-text>
                                <h3>{{card}}</h3>
                            </v-card-text>
                        </v-card>
                    </v-flex>
                </v-layout>
            </v-container>


            <div>
                <h4>Cards</h4>
                <p v-for="messageItem in messages" :key="messageItem.ConnectionId">
                    {{ messageItem.Message }}
                </p>
            </div>
            <div>
                <v-chip v-for="(user, key) in playersOnline" :key="key" class="grey lighten-2">
                    <v-avatar class="teal">{{ user.Name.charAt(0) }}</v-avatar>
                    {{ user.Name }}
                </v-chip>
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
    margin-top: 60px;
}
</style>