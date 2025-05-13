/**
 * Dashboard Service for handling dashboard statistics-related API operations
 */
import UserService from "./UserService.js";

export default {
	/**
	 * Get dashboard overview statistics
	 * @param {string} token - Authentication token
	 * @returns {Promise<Object>} - Dashboard statistics
	 */
	async getDashboardStatistics(token) {
		try {
			// Get services data from API
			const servicesResponse = await fetch("/api/service/admin", {
				headers: {
					Authorization: `Bearer ${token}`,
				},
			});

			if (!servicesResponse.ok) throw new Error("فشل تحميل بيانات الخدمات");

			const servicesData = await servicesResponse.json();
			const services = this.extractArrayFromResponse(servicesData, "services");

			// Get total users count from the database
			let totalUsers = 0;
			try {
				const users = await UserService.getUsers(token);
				totalUsers = users.length;
			} catch (error) {
				console.error("Error fetching users count:", error);
			}

			return {
				totalServices: services.length,
				approvedServices: services.filter((service) => service.isApproved)
					.length,
				pendingServices: services.filter((service) => !service.isApproved)
					.length,
				totalUsers: totalUsers,
			};
		} catch (error) {
			console.error("Error fetching dashboard statistics:", error);
			// Return default values in case of error
			return {
				totalServices: 0,
				approvedServices: 0,
				pendingServices: 0,
				totalUsers: 0,
			};
		}
	},

	/**
	 * Helper function to extract array from API response
	 * @param {Object|Array} data - API response data
	 * @param {string} defaultArrayName - Default property name to look for
	 * @returns {Array} - Extracted array
	 */
	extractArrayFromResponse(data, defaultArrayName) {
		// If it's already an array, return it
		if (Array.isArray(data)) {
			return data;
		}

		// If it's an object, try to find an array property
		if (typeof data === "object" && data !== null) {
			// First check the specific property name
			if (data[defaultArrayName] && Array.isArray(data[defaultArrayName])) {
				return data[defaultArrayName];
			}

			// Then look for any array property
			const arrayProp = Object.entries(data).find(([_, val]) =>
				Array.isArray(val)
			);
			if (arrayProp) {
				return arrayProp[1];
			}
		}

		// Default to empty array
		return [];
	},
};
