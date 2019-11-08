import Vue from "vue";
import Router from "vue-router";
import home from "./components/home";
import goTo from "vuetify/lib/services/goto";

Vue.use(Router);

export default new Router({
  mode: "history",
  base: process.env.BASE_URL,
  routes: [
    {
      path: "/",
      name: "home",
      component: home
    },
    {
      path: "*",
      redirect: "/"
    }
  ],
  scrollBehavior: () => {
    let scrollTo = 0
    return goTo(scrollTo);
  }
})
