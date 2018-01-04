<template>
    <div>
        <p>Players Online ({{ Object.keys(players).length }})</p>
        <v-chip v-for="(player, key) in players" :key="key" class="grey lighten-2">
            <v-avatar class="teal">{{ player.Name.charAt(0) }}</v-avatar>
            {{ player.Name }}
        </v-chip>
    </div>
</template>
<script>
export default {
    props: {
        players: Object
    }
}
</script>

