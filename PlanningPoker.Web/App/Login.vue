<template>
  <v-content>
    <v-container>
      <v-layout row wrap>
        <v-flex xs12 sm10 offset-sm1 md6 offset-md3>
          <v-card class="grey lighten-4">
            <v-card-text>
              <v-form v-model="valid">
                <v-text-field 
                  v-model="playerName" 
                  name="playerName"
                  label="Enter your name here" 
                  id="playerName" 
                  :rules="[() => !!playerName || 'This field is required']">
                  </v-text-field>
                <v-text-field v-model="groupId" name="groupId" label="GroupId" id="groupId"></v-text-field>
                <v-btn outline required class="indigo indigo--text" @click="submit()">Join</v-btn>
              </v-form>
            </v-card-text>
          </v-card>
        </v-flex>
      </v-layout>
    </v-container>
  </v-content>
</template>
<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";

@Component({
  props: {
    join: Function,
    player: Object
  }
})
export default class Login extends Vue {
  join: any;
  player: any;
  groupId: any = this.player.groupId || "";
  valid: boolean = false;
  playerName: string = "";

  mounted() {
    this.groupId = new URLSearchParams(window.location.search).get("groupId");
  }

  submit() {
    if (this.valid) {
      this.join(this.playerName, this.groupId);
    }
  }
}
</script>
