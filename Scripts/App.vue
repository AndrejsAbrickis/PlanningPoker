<template>
    <div id="app">
        <h2>Planning Poker Vue APP</h2>
        <input type="text" v-model="playerName"/>
        <button type="button" @click="join">Join</button>
        <div>
            <h4>Messages</h4>
            <p v-for="message in messages" :key="message">
                {{ message }}
            </p>
        </div>
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
    UsersLeft: "UsersLeft"
}

export default {
    name: 'app',
    data () {
        return {
            playerName: '',
            pokerHub: '',
            messages: []
        };
    },
    mounted () {
        this.pokerHub = new HubConnection(HUBS.POKER);

        this.pokerHub.on(HUB_EVENTS.Send, (data) => {
            this.messages.push(data);
            this.playerName = '';
        });

        this.pokerHub.on(HUB_EVENTS.UsersJoined, (data) => {
            console.log(`Joined: ${data}`);
        });

        this.pokerHub.on(HUB_EVENTS.UsersLeft, (data) => {
            console.log(`Left: ${data}`);
        });

        this.pokerHub.start();
    },
    methods: {
        join () {
            console.log(this.playerName);
            this.pokerHub.invoke(HUB_EVENTS.Send, this.playerName);
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