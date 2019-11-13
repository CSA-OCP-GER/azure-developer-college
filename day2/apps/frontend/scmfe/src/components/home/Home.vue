<template>
  <div>
    <v-divider />
    <v-toolbar flat>
      <v-breadcrumbs :items="items">
        <v-icon slot="divider">mdi-chevron-right</v-icon>
      </v-breadcrumbs>
    </v-toolbar>
    <v-divider />
    <v-container>
      <v-layout text-center wrap>
        <v-flex mb-4>
          <h1 class="display-2 font-weight-bold mb-3">Welcome to SCM Contacts App</h1>
        </v-flex>
      </v-layout>
    </v-container>
  </div>
</template>

<script>

export default {
  data() {
    return {
      items: [
        {
          text: "Home",
          disabled: true,
          to: "/home",
          exact: true
        }
      ],
      loading: false
    };
  }
};
</script>