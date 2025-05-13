/**
 * User Service for handling user-related API operations
 */
export default {
	/**
	 * Get all users
	 * @param {string} token - Authentication token
	 * @returns {Promise<Array>} - Array of users
	 */
	async getUsers(token) {
		try {
			const response = await fetch("/api/user", {
				headers: {
					Authorization: `Bearer ${token}`,
				},
			});

			if (!response.ok) {
				const errorText = await response.text();
				throw new Error(
					`فشل تحميل المستخدمين: ${response.status} - ${errorText}`
				);
			}

			// Parse the response
			const rawResponse = await response.text();
			console.log("Raw API response text:", rawResponse);
			const data = rawResponse ? JSON.parse(rawResponse) : [];

			if (Array.isArray(data)) {
				return data;
			} else if (typeof data === "object") {
				// Try to extract array from response
				const arrayProps = Object.entries(data).find(([_, val]) =>
					Array.isArray(val)
				);
				if (arrayProps) {
					return arrayProps[1];
				} else {
					// Single user case
					return [data];
				}
			}

			return [];
		} catch (error) {
			console.error("Error fetching users:", error);
			throw error;
		}
	},

	/**
	 * Create a new user
	 * @param {Object} userData - User data
	 * @param {string} token - Authentication token
	 * @returns {Promise<Object>} - Created user
	 */
	async createUser(userData, token) {
		try {
			const response = await fetch("/api/user", {
				method: "POST",
				headers: {
					"Content-Type": "application/json",
					Authorization: `Bearer ${token}`,
				},
				body: JSON.stringify(userData),
			});

			if (!response.ok) {
				const errorText = await response.text();
				throw new Error(`Failed to create user: ${errorText}`);
			}

			return await response.text();
		} catch (error) {
			console.error("Error creating user:", error);
			throw error;
		}
	},

	/**
	 * Update an existing user
	 * @param {string} userId - User ID
	 * @param {Object} userData - User data to update
	 * @param {string} token - Authentication token
	 * @returns {Promise<Object>} - Updated user
	 */
	async updateUser(userId, userData, token) {
		try {
			const response = await fetch(`/api/user/${userId}`, {
				method: "PUT",
				headers: {
					"Content-Type": "application/json",
					Authorization: `Bearer ${token}`,
				},
				body: JSON.stringify(userData),
			});

			if (!response.ok) {
				const errorText = await response.text();
				throw new Error(`Failed to update user: ${errorText}`);
			}

			return await response.text();
		} catch (error) {
			console.error("Error updating user:", error);
			throw error;
		}
	},

	/**
	 * Delete a user
	 * @param {string} userId - User ID
	 * @param {string} token - Authentication token
	 * @returns {Promise<void>}
	 */
	async deleteUser(userId, token) {
		try {
			const response = await fetch(`/api/user/${userId}`, {
				method: "DELETE",
				headers: {
					Authorization: `Bearer ${token}`,
				},
			});

			if (!response.ok) {
				const errorText = await response.text();
				throw new Error(`Failed to delete user: ${errorText}`);
			}
		} catch (error) {
			console.error("Error deleting user:", error);
			throw error;
		}
	},

	/**
	 * Normalize user data for consistent properties
	 * @param {Object} user - User data
	 * @returns {Object} - Normalized user data
	 */
	normalizeUserData(user) {
		console.log("Processing user:", user.username);
		console.log(
			"Original UserType:",
			user.UserType,
			"- type:",
			typeof user.UserType
		);

		// Determine UserType value (enum: 0 = Customer/User, 1 = Provider, 2 = Admin)
		let userType = null;

		// 1. Try to extract UserType from available properties
		if (user.UserType !== undefined) {
			// UserType could be a number, string or enum name from backend
			if (typeof user.UserType === "number") {
				userType = user.UserType; // Already a number (0, 1, 2)
			} else if (typeof user.UserType === "string") {
				// Check if it's a numeric string
				if (!isNaN(parseInt(user.UserType))) {
					userType = parseInt(user.UserType);
				}
				// Check if it's a string enum name (backend returns UserType.ToString())
				else if (user.UserType === "Admin") {
					userType = 2;
				} else if (user.UserType === "Provider") {
					userType = 1;
				} else if (user.UserType === "Customer") {
					userType = 0;
				}
			}
		}
		// 2. Try lowercase property as fallback with the same logic
		else if (user.userType !== undefined) {
			if (typeof user.userType === "number") {
				userType = user.userType;
			} else if (typeof user.userType === "string") {
				if (!isNaN(parseInt(user.userType))) {
					userType = parseInt(user.userType);
				} else if (user.userType === "Admin") {
					userType = 2;
				} else if (user.userType === "Provider") {
					userType = 1;
				} else if (user.userType === "Customer") {
					userType = 0;
				}
			}
		}
		// 3. Try to derive from role property if no UserType found
		else if (user.role !== undefined) {
			if (user.role === "Admin") userType = 2;
			else if (user.role === "Provider") userType = 1;
			else if (user.role === "User" || user.role === "Customer") userType = 0;
		}
		// 4. Fallback to default
		else {
			userType = 0; // Default to regular user (Customer)
		}

		// Validate that userType is a number 0, 1, or 2
		if (userType !== 0 && userType !== 1 && userType !== 2) {
			console.warn(
				`Invalid UserType value for ${user.username}: ${userType}, defaulting to 0`
			);
			userType = 0;
		}

		// Determine role string based on UserType
		let primaryRole = "User";
		if (userType === 2) primaryRole = "Admin";
		else if (userType === 1) primaryRole = "Provider";

		// Extract name information
		const firstName =
			user.FirstName ||
			user.firstName ||
			user.first_name ||
			user.firstname ||
			user.givenName ||
			"";

		const lastName =
			user.LastName ||
			user.lastName ||
			user.last_name ||
			user.lastname ||
			user.surname ||
			user.familyName ||
			"";

		// Create normalized user object
		const normalizedUser = {
			id: user.id,
			username: user.username || user.userName,
			email: user.email,
			role: primaryRole,
			UserType: userType, // Store the numeric enum value
			isActive: typeof user.isActive === "boolean" ? user.isActive : true,
			createdDate: user.createdDate || user.dateRegistered,
			firstName: firstName,
			lastName: lastName,
			FirstName: firstName,
			LastName: lastName,
		};

		console.log("Normalized user:", normalizedUser);
		return normalizedUser;
	},
};
