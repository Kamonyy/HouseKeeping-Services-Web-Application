/**
 * User service for managing user-related API operations
 */
class UserService {
	/**
	 * Fetches all users from the API
	 * @param {string} token - Authentication token
	 * @returns {Array} List of users
	 */
	async getUsers(token) {
		try {
			const response = await fetch("/api/user", {
				headers: { Authorization: `Bearer ${token}` },
			});

			if (!response.ok) {
				const errorText = await response.text();
				throw new Error(
					`Failed to load users: ${response.status} - ${errorText}`
				);
			}

			// Parse response text to handle empty responses
			const rawResponse = await response.text();
			if (!rawResponse) return [];

			const data = JSON.parse(rawResponse);

			// Handle different response formats
			if (Array.isArray(data)) {
				return data;
			} else if (typeof data === "object") {
				// Look for arrays in the response
				const arrayProp = Object.entries(data).find(([_, val]) =>
					Array.isArray(val)
				);
				if (arrayProp) {
					return arrayProp[1];
				}
				// Single user case - wrap in array
				return [data];
			}

			return [];
		} catch (error) {
			console.warn(`Error fetching users: ${error.message}`);
			throw error;
		}
	}

	/**
	 * Creates a new user
	 * @param {Object} userData - User data
	 * @param {string} token - Authentication token
	 * @returns {Object} Created user
	 */
	async createUser(userData, token) {
		return this.sendRequest("/api/user", "POST", userData, token);
	}

	/**
	 * Updates an existing user
	 * @param {string} userId - User ID
	 * @param {Object} userData - User data to update
	 * @param {string} token - Authentication token
	 * @returns {Object} Updated user
	 */
	async updateUser(userId, userData, token) {
		return this.sendRequest(`/api/user/${userId}`, "PUT", userData, token);
	}

	/**
	 * Deletes a user
	 * @param {string} userId - User ID
	 * @param {string} token - Authentication token
	 * @returns {boolean} Success status
	 */
	async deleteUser(userId, token) {
		try {
			const response = await fetch(`/api/user/${userId}`, {
				method: "DELETE",
				headers: { Authorization: `Bearer ${token}` },
			});

			if (!response.ok) {
				const errorText = await response.text();
				throw new Error(`Failed to delete user: ${errorText}`);
			}

			return true;
		} catch (error) {
			console.warn(`Error deleting user: ${error.message}`);
			throw error;
		}
	}

	/**
	 * Normalizes user data for consistent property access
	 * @param {Object} user - User data
	 * @returns {Object} Normalized user object
	 */
	normalizeUserData(user) {
		// Get user type from available properties
		const userType = this.determineUserType(user);

		// Create a normalized user object with consistent properties
		return {
			id: user.id || user.Id || user.userId || user.UserId || "",
			username:
				user.username || user.Username || user.userName || user.UserName || "",
			email: user.email || user.Email || "",
			firstName: user.firstName || user.FirstName || "",
			lastName: user.lastName || user.LastName || "",
			fullName: this.getFullName(user),
			userType: userType,
			userTypeName: this.getUserTypeName(userType),
			isActive: user.isActive ?? user.IsActive ?? true,
			createdAt: user.createdAt || user.CreatedAt || new Date().toISOString(),
			role: user.role || user.Role || this.getUserTypeName(userType),
		};
	}

	// Helper methods

	/**
	 * Determines the user type from available properties
	 */
	determineUserType(user) {
		// Check UserType property (case-sensitive)
		if (user.UserType !== undefined) {
			if (typeof user.UserType === "number") {
				return user.UserType;
			} else if (typeof user.UserType === "string") {
				return !isNaN(parseInt(user.UserType))
					? parseInt(user.UserType)
					: this.getUserTypeFromString(user.UserType);
			}
		}

		// Check userType property (lowercase)
		else if (user.userType !== undefined) {
			if (typeof user.userType === "number") {
				return user.userType;
			} else if (typeof user.userType === "string") {
				return !isNaN(parseInt(user.userType))
					? parseInt(user.userType)
					: this.getUserTypeFromString(user.userType);
			}
		}

		// Check role property
		else if (user.role) {
			return this.getUserTypeFromString(user.role);
		}

		// Default to regular customer (0)
		return 0;
	}

	/**
	 * Converts a string user type to numeric enum
	 */
	getUserTypeFromString(typeString) {
		const normalized = String(typeString).toLowerCase();

		if (normalized === "admin") return 2;
		if (normalized === "provider") return 1;

		// Customer, User, or unknown types
		return 0;
	}

	/**
	 * Gets the user type name based on numeric value
	 */
	getUserTypeName(typeValue) {
		const type = parseInt(typeValue);

		switch (type) {
			case 2:
				return "Admin";
			case 1:
				return "Provider";
			case 0:
			default:
				return "Customer";
		}
	}

	/**
	 * Gets the user's full name from available properties
	 */
	getFullName(user) {
		const firstName = user.firstName || user.FirstName || "";
		const lastName = user.lastName || user.LastName || "";

		if (firstName || lastName) {
			return `${firstName} ${lastName}`.trim();
		}

		return user.fullName || user.FullName || user.username || "";
	}

	/**
	 * Sends an API request with proper JSON formatting
	 */
	async sendRequest(url, method, data, token) {
		try {
			const response = await fetch(url, {
				method,
				headers: {
					"Content-Type": "application/json",
					Authorization: `Bearer ${token}`,
				},
				body: JSON.stringify(data),
			});

			if (!response.ok) {
				const errorText = await response.text();
				throw new Error(`Request failed: ${errorText}`);
			}

			// Handle empty responses (like 204 No Content)
			const responseText = await response.text();
			return responseText ? JSON.parse(responseText) : { success: true };
		} catch (error) {
			console.warn(`API error (${method} ${url}): ${error.message}`);
			throw error;
		}
	}
}

// Create and export a singleton instance
export default new UserService();
