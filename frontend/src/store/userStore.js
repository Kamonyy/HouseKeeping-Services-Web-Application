import { defineStore } from "pinia";
import { jwtDecode } from "jwt-decode";

export const useUserStore = defineStore("user", {
	state: () => ({
		username: null,
		token: null, // Store the token here
		roles: [], // Store user roles
	}),
	actions: {
		setUser(username, token) {
			this.username = username;
			this.token = token;

			// Extract roles from the token
			if (token) {
				try {
					const decodedToken = jwtDecode(token);
					const rolesClaim =
						decodedToken[
							"http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
						];
					this.roles = Array.isArray(rolesClaim) ? rolesClaim : [rolesClaim];
				} catch (error) {
					console.error("Error extracting roles from token:", error);
					this.roles = [];
				}
			}
		},
		logout() {
			this.username = null;
			this.token = null;
			this.roles = [];
		},
		hasRole(role) {
			return this.roles.includes(role);
		},
	},
	getters: {
		isLoggedIn: (state) => !!state.token, // Check if token exists
		isAdmin: (state) => state.roles.includes("Admin"),
		isProvider: (state) => state.roles.includes("Provider"),
		isUser: (state) => state.roles.includes("User"),
	},
});
