<template >
  <div>
    <v-divider />
    <v-toolbar flat>
      <v-breadcrumbs :items="items">
        <v-icon slot="divider">mdi-chevron-right</v-icon>
      </v-breadcrumbs>
    </v-toolbar>
    <v-divider />
    <v-container class="grid-list-xl">
      <v-toolbar class="px-0" flat color="transparent">
        <v-toolbar-title class="pt-0 px-0 headline">
          <span class="ml-3 hidden-sm-and-down">Overall Stats</span>
        </v-toolbar-title>
        <div class="flex-grow-1"></div>
        <v-spacer></v-spacer>
        <v-tooltip bottom>
          <template v-slot:activator="{ on }">
            <v-btn v-on="on" icon @click="refresh()">
              <v-icon>mdi-refresh</v-icon>
            </v-btn>
          </template>
          <span>Refresh</span>
        </v-tooltip>
      </v-toolbar>
      <v-layout row class="px-5">
        <v-flex xs12></v-flex>
      </v-layout>
    </v-container>
  </div>
</template>

<script>
import { mapGetters, mapActions } from "vuex";

export default {
  components: {},
  computed: {
    ...mapGetters({
      stats: "stats/stats",
      statsTimeline: "stats/statsTimeline"
    })
  },
  mounted() {
    return this.refresh();
  },
  methods: {
    ...mapActions({
      statsOverall: "stats/statsOverall",
      statsTl: "stats/statsTimeline"
    }),
    refresh() {
      this.loading = true;
      this.statsOverall()
        .then(() => {
          this.statsTl().then(() => (this.loading = false));
        })
        .catch(() => (this.loading = false));
    }
  },
  data() {
    return {
      items: [
        {
          text: "Home",
          disabled: false,
          to: "/home",
          exact: true
        },
        {
          text: "Statistics",
          disabled: true,
          to: "/stats"
        }
      ],
      loading: false
    };
  }
};
</script>