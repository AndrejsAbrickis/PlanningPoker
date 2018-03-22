<template>
  <v-content>
    <v-container>
      <v-layout 
        row 
        wrap>
        <v-flex 
          xs12 
          sm10 
          offset-sm1 
          md6 
          offset-md3>
          <v-card class="grey lighten-4">
            <v-card-text>
              <v-form v-model="valid">
                <v-text-field 
                  id="playerName" 
                  v-model="playerName"
                  :rules="[() => !!playerName || 'This field is required']"
                  name="playerName"
                  label="Enter your name here"/>
                <v-text-field 
                  id="groupId"
                  v-model="groupId" 
                  name="groupId"  
                  label="GroupId"/>
                <v-btn 
                  outline 
                  required 
                  class="indigo indigo--text" 
                  @click="submit()">Join</v-btn>
              </v-form>
            </v-card-text>
          </v-card>
        </v-flex>
      </v-layout>
    </v-container>
  </v-content>
</template>
<script lang="ts">
import Vue from 'vue';
/* eslint-disable-next-line no-unused-vars */
import Component from 'vue-class-component';

@Component({
  props: {
    join: Function,
    player: Object,
  },
})
export default class Login extends Vue {
  /* eslint-disable-next-line no-undef */
  private join: any;
  /* eslint-disable-next-line no-undef */
  private player: any;
  /* eslint-disable-next-line no-undef */
  private groupId: any = this.player.groupId || '';
  /* eslint-disable-next-line no-undef */
  private valid: boolean = false;
  /* eslint-disable-next-line no-undef */
  private playerName: string = '';

  private mounted() {
    this.groupId = new URLSearchParams(window.location.search).get('groupId');
  }

  private submit() {
    if (this.valid) {
      this.join(this.playerName, this.groupId);
    }
  }
}
</script>
