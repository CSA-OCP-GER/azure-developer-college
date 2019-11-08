import Vue from "vue";
import Vuex from "vuex";
import createPersistedState from "vuex-persistedstate";
Vue.use(Vuex);

export default new Vuex.Store({
  plugins: [createPersistedState({
    key: "$scm__store",
    reducer: (persistedState) => {
      const stateFilter = Object.assign({}, persistedState);
      const blackList = ["wait"];

      blackList.forEach((item) => {
        delete stateFilter[item];
      });
      return stateFilter;
    }

  })],
  strict: true,
  modules: {

  },
  state: {

  },
  mutations: {

  },
  actions: {

  }
})
