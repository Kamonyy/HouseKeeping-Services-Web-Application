import { defineStore } from "pinia";

export const useUserStore = defineStore("user", {
	state: () => ({
		username: null,
		token: null, // Store the token here
	}),
	actions: {
		setUser(username, token) {
			this.username = username;
			this.token = token;
		},
		logout() {
			this.username = null;
			this.token = null;
		},
	},
	getters: {
		isLoggedIn: (state) => !!state.token, // Check if token exists
	},
});
