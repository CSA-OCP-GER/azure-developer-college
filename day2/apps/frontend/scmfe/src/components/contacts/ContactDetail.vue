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
          <span class="ml-3 hidden-sm-and-down">Contact Details</span>
        </v-toolbar-title>
        <div class="flex-grow-1"></div>
        <v-spacer></v-spacer>
        <v-tooltip bottom>
          <template v-slot:activator="{ on }">
            <v-btn v-on="on" icon @click="updateContact()">
              <v-icon>mdi-content-save</v-icon>
            </v-btn>
          </template>
          <span>Save</span>
        </v-tooltip>
        <v-icon disabled>mdi-circle-small</v-icon>
        <v-menu bottom left>
          <template v-slot:activator="{ on }">
            <v-btn v-on="on" icon>
              <v-icon>mdi-dots-vertical</v-icon>
            </v-btn>
          </template>
          <v-list>
            <v-list-item @click="deleteContact()">
              <v-list-item-avatar>
                <v-icon color="red">mdi-delete</v-icon>
              </v-list-item-avatar>
              <v-list-item-title>Delete</v-list-item-title>
            </v-list-item>
          </v-list>
        </v-menu>
      </v-toolbar>
      <v-layout row class="px-5">
        <v-flex xs12 md8 sm7>
          <v-card>
            <v-subheader>Contact Details</v-subheader>
            <v-card-text>
              <v-text-field
                :error-messages="errors.collect('firstname')"
                data-vv-as="First Name"
                name="firstname"
                v-validate="'required|max:255'"
                v-model="contactFields.firstname"
                label="First Name"
              ></v-text-field>
              <v-text-field
                :error-messages="errors.collect('lastname')"
                data-vv-as="Last Name"
                name="lastname"
                v-validate="'required|max:255'"
                v-model="contactFields.lastname"
                label="Last Name"
              ></v-text-field>
              <v-text-field
                :error-messages="errors.collect('email')"
                data-vv-as="Email Address"
                name="email"
                v-validate="'required|max:255'"
                v-model="contactFields.email"
                label="Email Address"
              ></v-text-field>
            </v-card-text>
            <v-subheader>Company Details</v-subheader>
            <v-card-text>
              <v-text-field
                :error-messages="errors.collect('company')"
                data-vv-as="Company"
                name="company"
                v-validate="'required|max:255'"
                v-model="contactFields.company"
                label="Company"
              ></v-text-field>
            </v-card-text>
          </v-card>
        </v-flex>
        <v-flex xs12 md4 sm5>
          <v-card>
            <v-subheader>Contact Avatar</v-subheader>
            <v-card-text class="text-center">
              <avatar
                :size="140"
                :image="contactFields.image"
                :firstname="contactFields.firstname"
                :lastname="contactFields.lastname"
              ></avatar>
            </v-card-text>
            <v-divider />
            <v-card-actions class="justify-center">
              <v-btn color="primary" text @click="changeImage()">Change Avatar</v-btn>
            </v-card-actions>
          </v-card>
        </v-flex>
      </v-layout>
    </v-container>
    <confirmation-dialog ref="confirm"></confirmation-dialog>
    <image-upload-dialog ref="logoupload"></image-upload-dialog>
  </div>
</template>
<script>
import { mapGetters, mapActions } from "vuex";
import _ from "lodash";
import Avatar from "../avatar/Avatar";
import ConfirmationDialog from "../dialog/ConfirmationDialog";
import ImageUploadDialog from "../resources/ImageUploadDialog";

export default {
  components: {
    Avatar,
    ConfirmationDialog,
    ImageUploadDialog
  },
  computed: {
    ...mapGetters({
      contact: "contacts/contact"
    }),
    isFormDirty() {
      return Object.keys(this.fields).some(key => this.fields[key].dirty);
    }
  },
  created() {
    return this.read(this.$route.params.id);
  },
  beforeRouteLeave(to, from, next) {
    if (this.isFormDirty) {
      this.$refs.confirm
        .open("You've got unsaved changes", "Leave anyway?", {
          color: "orange"
        })
        .then(confirm => {
          if (confirm) next();
        });
    } else {
      next();
    }
  },
  methods: {
    ...mapActions({
      read: "contacts/read",
      update: "contacts/update",
      delete: "contacts/delete"
    }),
    updateContact() {
      this.update(this.contactFields).then(() => this.$validator.reset());
    },
    deleteContact() {
      this.$refs.confirm
        .open(
          "Delete Contact",
          `Are you sure you want to delete contact ${this.contactFields.firstname} ${this.contactFields.lastname}?`,
          {
            color: "red"
          }
        )
        .then(confirm => {
          if (confirm)
            this.delete(this.contactFields.id).then(() =>
              this.$router.push({ name: "contacts" })
            );
        });
    },
    changeImage() {
      this.$refs.logoupload.open().then(url => {
        if (url) {
          this.contactFields.image = url;
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
          disabled: false,
          to: "/contacts",
          exact: true
        },
        {
          text: "Contact Detail",
          disabled: true
        }
      ],
      loading: false,
      contactFields: {}
    };
  },
  watch: {
    contact: {
      handler() {
        this.contactFields = _.cloneDeep(this.contact);
        this.contactFields.image = "";
      },
      deep: true
    }
  }
};
</script>