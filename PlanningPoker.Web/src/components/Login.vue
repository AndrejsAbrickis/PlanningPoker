<template>
  <v-main>
    <v-container>
      <v-layout row wrap>
        <v-flex xs12 sm10 offset-sm1 md6 offset-md3>
          <v-card class="grey lighten-4">
            <v-card-text>
              <v-form v-model="valid">
                <v-text-field
                  id="name"
                  v-model="name"
                  :rules="[() => !!name || 'This field is required']"
                  name="name"
                  label="Enter your name here"
                />
                <v-text-field
                  id="groupId"
                  v-model="groupId"
                  name="groupId"
                  label="GroupId"
                />
                <v-btn outlined color="indigo" required @click="submit()" :disabled="!valid"
                  >Join</v-btn
                >
              </v-form>
            </v-card-text>
          </v-card>
        </v-flex>
      </v-layout>
    </v-container>
  </v-main>
</template>
<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';

@Component
export default class Login extends Vue {
  private groupId: string = '';

  private valid: boolean = false;

  private name: string = '';

  private mounted() {
    this.groupId = new URLSearchParams(window.location.search).get('groupId') || '';
  }

  private submit() {
    if (!this.valid) {
      return;
    }

    this.$emit('submit', {
      name: this.name,
      groupId: this.groupId,
    });
  }
}
</script>
