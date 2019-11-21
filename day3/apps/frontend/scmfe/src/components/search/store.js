import { getSearchHttpClient } from "../../utils/http-client";

const BASE_PATH = "/search";

const state = {
    searchresults: [],
    metadata: {}
};

// getters
const getters = {
    searchresults: state => state.searchresults,
    metadata: state => state.metadata
};

// actions
const actions = {
    search({ commit, dispatch }) {
        dispatch("wait/start", "apicall", { root: true });
        var client = getSearchHttpClient();
        return client.get(BASE_PATH).then(response => {
            commit("setSearchResults", response.data);
            dispatch("wait/end", "apicall", { root: true });
        }).catch(err => {
            if (typeof err == "object" && err.code) {
                if (err.code == "ECONNABORTED") {
                    dispatch("notifications/addMessage", { type: "error", message: "Search Service unavailable.", read: false }, { root: true });
                }
            } else {
                if (err && err.message) {
                    dispatch("notifications/addMessage", { type: "error", message: err.message, read: false }, { root: true });
                }
            }
            dispatch("wait/end", "apicall", { root: true });
        });
    },
    clearresults({ commit }) {
        commit("clearResults");
    }
};

// mutations
const mutations = {
    setSearchResults(state, payload) {
        state.searchresults = payload;
    },
    clearResults(state) {
        state.searchresults = [];
    }
}

export default {
    namespaced: true,
    state,
    getters,
    actions,
    mutations
}