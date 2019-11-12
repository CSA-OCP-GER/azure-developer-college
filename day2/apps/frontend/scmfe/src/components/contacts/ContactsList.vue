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
          <v-data-table :headers="headers" :items="contacts" class="elevation-1">
            <template v-slot:item.avatar="{item}">
              <avatar :image="item.image" :firstname="item.firstname" :lastname="item.lastname"></avatar>
            </template>
            <template v-slot:item.actions="{item}">
              <v-hover>
                <v-icon
                  slot-scope="{ hover }"
                  :color="`${hover ? 'primary' : ''}`"
                  small
                  class="mr-2"
                  @click="editItem(item)"
                >mdi-pencil</v-icon>
              </v-hover>
              <v-hover>
                <v-icon
                  slot-scope="{ hover }"
                  :color="`${hover ? 'red' : ''}`"
                  small
                  @click="deleteItem(item)"
                >mdi-delete</v-icon>
              </v-hover>
            </template>
          </v-data-table>
        </v-flex>
      </v-layout>
    </v-container>
    <contacts-create ref="contactsCreate"></contacts-create>
    <v-btn bottom color="green" dark fab fixed right @click="createContact()">
      <v-icon>mdi-plus</v-icon>
    </v-btn>
  </div>
</template>

<script>
import { mapGetters, mapActions } from "vuex";
import ContactsCreate from "./ContactsCreate";
import Avatar from "../avatar/Avatar";

export default {
  components: {
    ContactsCreate,
    Avatar
  },
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
    },
    createContact() {
      this.$refs.contactsCreate.open({}).then(newContact => {
        if (newContact != null) {
          this.refresh();
          // this.$router.push({ path: `/contacts/${newContact}` });
        }
      });
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
          text: "",
          align: "center",
          sortable: false,
          value: "avatar"
        },
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
          text: "Email",
          sortable: true,
          value: "email"
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