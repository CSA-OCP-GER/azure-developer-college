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
      <v-layout row>
        <v-flex xs12>
          <h1>Contacts List</h1>
        </v-flex>
        <v-flex xs12>
          <v-data-table :headers="headers" :items="contacts" class="elevation-1"></v-data-table>
        </v-flex>
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
      contacts: "contacts/contacts"
    })
  },
  mounted() {
    return this.refresh();
  },
  methods: {
    ...mapActions({
      list: "contacts/list"
    }),
    refresh() {
      this.loading = true;
      this.list().then(() => (this.loading = false));
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
          text: "Contacts",
          disabled: true,
          to: "contacts"
        }
      ],
      loading: false,
      headers: [
        {
          text: "First Name",
          sortable: true,
          value: "firstname"
        },
        {
          text: "Last Name",
          sortable: true,
          value: "lastname"
        },
        {
          text: "Company",
          sortable: true,
          value: "company"
        },
        {
          text: "Actions",
          value: "actions",
          sortable: false,
          align: "end"
        }
      ]
    };
  }
};
</script>