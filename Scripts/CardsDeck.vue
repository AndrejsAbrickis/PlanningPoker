<template>
    <v-layout row wrap>
        <v-flex xs4 sm3 md2 v-for="card in cards" :key="card.value">
            <v-card class="purple white--text" @click="playCard(card.value)">
                <v-card-text>
                    <h3>{{ card.label }}</h3>
                </v-card-text>
            </v-card>
        </v-flex>
    </v-layout>
</template>
<script>
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
    { value: 80, label: '80' },
    { value: Infinity, label: '?' },
];

export default {
    data() {
        return {
            cards: CARDS
        }
    },
    props: {
        playCard: Function
    }
}
</script>
