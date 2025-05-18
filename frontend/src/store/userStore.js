import { defineStore } from "pinia";
import { jwtDecode } from "jwt-decode";

// Token expiration time in milliseconds (default: 24 hours)
const TOKEN_EXPIRATION_TIME = 24 * 60 * 60 * 1000;
const USER_STORAGE_KEY = "user";

export const useUserStore = defineStore("user", {
	state: () => {
		// Initialize with default state
		const initialState = {
			username: null,
			firstName: null,
			lastName: null,
			token: null,
			roles: [],
			tokenExpiration: null,
		};

		// Try to load user data from localStorage
		try {
			const storedUser = localStorage.getItem(USER_STORAGE_KEY);
			if (storedUser) {
				const parsedUser = JSON.parse(storedUser);

				// Check if token is still valid
				if (
					parsedUser.tokenExpiration &&
					new Date(parsedUser.tokenExpiration) > new Date()
				) {
					return parsedUser;
				}

				// Clear expired token
				localStorage.removeItem(USER_STORAGE_KEY);
			}
		} catch (e) {
			localStorage.removeItem(USER_STORAGE_KEY);
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
			const expirationDate = new Date(Date.now() + TOKEN_EXPIRATION_TIME);
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
				} catch (error) {
					this.roles = [];
				}

				// Store in localStorage if rememberMe is true
				if (rememberMe) {
					this.persistUserData();
				}
			}
		},
		logout() {
			// Reset state
			this.username = null;
			this.firstName = null;
			this.lastName = null;
			this.token = null;
			this.roles = [];
			this.tokenExpiration = null;

			// Clear localStorage
			localStorage.removeItem(USER_STORAGE_KEY);
		},
		hasRole(role) {
			return this.roles.includes(role);
		},
		persistUserData() {
			localStorage.setItem(
				USER_STORAGE_KEY,
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
			if (!this.tokenExpiration) return false;

			const now = new Date();
			const expiration = new Date(this.tokenExpiration);

			if (now >= expiration) {
				this.logout();
				return false;
			}

			return true;
		},
	},
	getters: {
		isLoggedIn: (state) => {
			if (!state.token || !state.tokenExpiration) return false;
			return new Date() < new Date(state.tokenExpiration);
		},
		isAdmin: (state) => state.roles.includes("Admin"),
		isProvider: (state) => state.roles.includes("Provider"),
		isUser: (state) => state.roles.includes("User"),
		tokenExpiresIn: (state) => {
			if (!state.tokenExpiration) return 0;
			return Math.max(0, new Date(state.tokenExpiration) - new Date());
		},
		displayName: (state) => state.firstName || state.username,
	},
});
