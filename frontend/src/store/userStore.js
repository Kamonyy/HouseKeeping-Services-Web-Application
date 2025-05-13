import { defineStore } from "pinia";
import { jwtDecode } from "jwt-decode";

// Token expiration time in milliseconds (default: 24 hours)
const TOKEN_EXPIRATION_TIME = 24 * 60 * 60 * 1000;

export const useUserStore = defineStore("user", {
	state: () => {
		// Try to load user data from localStorage
		const storedUser = localStorage.getItem("user");
		const initialState = {
			username: null,
			firstName: null,
			lastName: null,
			token: null,
			roles: [],
			tokenExpiration: null,
		};

		if (storedUser) {
			try {
				const parsedUser = JSON.parse(storedUser);

				// Check if token hasn't expired
				if (
					parsedUser.tokenExpiration &&
					new Date(parsedUser.tokenExpiration) > new Date()
				) {
					return parsedUser;
				} else {
					// Token expired, remove from localStorage
					localStorage.removeItem("user");
				}
			} catch (e) {
				console.error("Error parsing stored user data:", e);
				localStorage.removeItem("user");
			}
		}

		return initialState;
	},
	actions: {
		setUser(username, token, rememberMe = true, firstName = "", lastName = "") {
			this.username = username;
			this.token = token;
			this.firstName = firstName;
			this.lastName = lastName;

			// Set token expiration date
			const expirationDate = new Date();
			expirationDate.setTime(expirationDate.getTime() + TOKEN_EXPIRATION_TIME);
			this.tokenExpiration = expirationDate.toISOString();

			// Extract roles from the token
			if (token) {
				try {
					const decodedToken = jwtDecode(token);
					const rolesClaim =
						decodedToken[
							"http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
						];
					this.roles = Array.isArray(rolesClaim) ? rolesClaim : [rolesClaim];

					// Store in localStorage if rememberMe is true
					if (rememberMe) {
						this.persistUserData();
					}
				} catch (error) {
					console.error("Error extracting roles from token:", error);
					this.roles = [];
				}
			}
		},
		logout() {
			this.username = null;
			this.firstName = null;
			this.lastName = null;
			this.token = null;
			this.roles = [];
			this.tokenExpiration = null;

			// Clear localStorage
			localStorage.removeItem("user");
		},
		hasRole(role) {
			return this.roles.includes(role);
		},
		persistUserData() {
			localStorage.setItem(
				"user",
				JSON.stringify({
					username: this.username,
					firstName: this.firstName,
					lastName: this.lastName,
					token: this.token,
					roles: this.roles,
					tokenExpiration: this.tokenExpiration,
				})
			);
		},
		checkTokenExpiration() {
			if (this.tokenExpiration) {
				const now = new Date();
				const expiration = new Date(this.tokenExpiration);

				// If token is expired, log out
				if (now >= expiration) {
					this.logout();
					return false;
				}
				return true;
			}
			return false;
		},
	},
	getters: {
		isLoggedIn: (state) => {
			if (!state.token || !state.tokenExpiration) return false;

			// Check if token is expired
			const now = new Date();
			const expiration = new Date(state.tokenExpiration);
			return now < expiration;
		},
		isAdmin: (state) => state.roles.includes("Admin"),
		isProvider: (state) => state.roles.includes("Provider"),
		isUser: (state) => state.roles.includes("User"),
		// Return time left before token expiration
		tokenExpiresIn: (state) => {
			if (!state.tokenExpiration) return 0;

			const now = new Date();
			const expiration = new Date(state.tokenExpiration);
			return Math.max(0, expiration - now);
		},
		// Return display name (firstName or username)
		displayName: (state) => {
			return state.firstName || state.username;
		},
	},
});
