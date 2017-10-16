<template>
    <v-container>
        <v-layout row wrap>
            <v-flex xs12 sm10 offset-sm1 md6 offset-md3>
                <v-card dark class="grey lighten-4">
                    <v-card-text>
                        <v-text-field v-model="playerName" name="playerName" label="Enter your name here" id="playerName"></v-text-field>
                        <v-text-field v-model="groupId" name="groupId" label="GroupId" id="groupId"></v-text-field>
                        <v-btn outline class="indigo--text" @click="join(playerName, groupId)">Join</v-btn>
                    </v-card-text>
                </v-card>
            </v-flex>
        </v-layout>
    </v-container>
</template>
<script>
export default {
    data() {
        return {
            playerName: '',
            groupId: this.player.groupId
        }
    },
    props: {
        join: Function,
        player: Object
    }
}
</script>
