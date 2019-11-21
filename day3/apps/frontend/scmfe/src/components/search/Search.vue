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
          <span class="ml-3 hidden-sm-and-down">Search Results</span>
        </v-toolbar-title>
        <div class="flex-grow-1"></div>
        <v-spacer></v-spacer>
        <v-tooltip bottom>
          <template v-slot:activator="{ on }">
            <v-btn v-on="on" icon @click="clear()">
              <v-icon>mdi-close</v-icon>
            </v-btn>
          </template>
          <span>Clear</span>
        </v-tooltip>
      </v-toolbar>
      <v-layout row class="px-5">
        <v-flex xs12>{{phrase}}</v-flex>
      </v-layout>
    </v-container>
  </div>
</template>

<script>
import { mapGetters, mapActions } from "vuex";
// import Avatar from "../avatar/Avatar";

export default {
  components: {
    // Avatar
  },
  computed: {
    ...mapGetters({
      contacts: "search/searchresults"
    })
  },
  beforeRouteUpdate(to, from, next) {
    this.searchInternal(decodeURIComponent(to.query.phrase));
    next();
  },
  mounted() {
    return this.searchInternal(decodeURIComponent(this.$route.query.phrase));
  },
  methods: {
    ...mapActions({
      search: "search/search",
      clear: "search/clearresults"
    }),
    searchInternal(phrase) {
      this.loading = true;
      this.phrase = phrase;
      this.search().then(() => (this.loading = false));
    }
  },
  data() {
    return {
      phrase: "",
      items: [
        {
          text: "Home",
          disabled: false,
          to: "/home",
          exact: true
        },
        {
          text: "Search Results",
          disabled: true,
          to: "search"
        }
      ],
      loading: false
    };
  }
};
</script>