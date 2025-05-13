/**
 * Service for handling service-related API operations
 */
const API_BASE_URL = "https://localhost:7007"; // Update this with your actual API URL

export default {
	/**
	 * Get services for the provider
	 * @param {string} token - Authentication token
	 * @returns {Promise<Array>} - Array of services
	 */
	async getProviderServices(token) {
		try {
			const response = await fetch(`${API_BASE_URL}/api/service/provider`, {
				headers: {
					Authorization: `Bearer ${token}`,
				},
			});

			if (!response.ok) {
				if (response.status === 404) {
					// Provider has no services, return empty array
					return [];
				}
				throw new Error("Failed to load provider services");
			}

			return await response.json();
		} catch (error) {
			console.error("Error fetching provider services:", error);
			throw error;
		}
	},

	/**
	 * Get a service by its ID
	 * @param {number} serviceId - Service ID
	 * @param {string} token - Optional authentication token
	 * @returns {Promise<Object>} - Service details
	 */
	async getServiceById(serviceId, token = null) {
		try {
			console.log(
				`Fetching service with ID: ${serviceId}, using token: ${
					token ? "Yes" : "No"
				}`
			);

			const headers = {};
			if (token) {
				headers["Authorization"] = `Bearer ${token}`;
			}

			const response = await fetch(`${API_BASE_URL}/api/service/${serviceId}`, {
				headers,
			});

			console.log("Response status:", response.status);

			if (!response.ok) {
				if (response.status === 404) {
					throw new Error("Service not found");
				}
				throw new Error(`Failed to load service details: ${response.status}`);
			}

			const data = await response.json();
			console.log("Response data structure:", Object.keys(data));

			// Log specific subcategory info
			if (data.subCategories) {
				console.log("Found subCategories:", data.subCategories.length, "items");
			} else if (data.SubCategories) {
				console.log("Found SubCategories:", data.SubCategories.length, "items");
			} else if (data.serviceSubCategory) {
				console.log(
					"Found serviceSubCategory:",
					data.serviceSubCategory.length,
					"items"
				);
			} else if (data.serviceSubCategories) {
				console.log(
					"Found serviceSubCategories:",
					data.serviceSubCategories.length,
					"items"
				);
			} else {
				console.log("No subcategories found in any standard format");
			}

			return data;
		} catch (error) {
			console.error(`Error fetching service ${serviceId}:`, error);
			throw error;
		}
	},

	/**
	 * Get all services for admin
	 * @param {string} token - Authentication token
	 * @returns {Promise<Array>} - Array of services
	 */
	async getAdminServices(token) {
		try {
			const response = await fetch(`${API_BASE_URL}/api/service/admin`, {
				headers: {
					Authorization: `Bearer ${token}`,
				},
			});

			if (!response.ok) {
				throw new Error("Failed to load services");
			}

			return await response.json();
		} catch (error) {
			console.error("Error fetching admin services:", error);
			throw error;
		}
	},

	/**
	 * Create a new service
	 * @param {Object} serviceData - Service data
	 * @param {string} token - Authentication token
	 * @returns {Promise<Object>} - Created service
	 */
	async createService(serviceData, token) {
		try {
			const response = await fetch(`${API_BASE_URL}/api/service`, {
				method: "POST",
				headers: {
					"Content-Type": "application/json",
					Authorization: `Bearer ${token}`,
				},
				body: JSON.stringify(serviceData),
			});

			if (!response.ok) {
				const errorText = await response.text();
				throw new Error(`Failed to create service: ${errorText}`);
			}

			return await response.json();
		} catch (error) {
			console.error("Error creating service:", error);
			throw error;
		}
	},

	/**
	 * Update an existing service
	 * @param {number} serviceId - Service ID
	 * @param {Object} serviceData - Service data to update
	 * @param {string} token - Authentication token
	 * @returns {Promise<Object>} - Updated service
	 */
	async updateService(serviceId, serviceData, token) {
		try {
			// Ensure all subcategory IDs are numbers
			if (serviceData.subCategoryIds) {
				serviceData.subCategoryIds = serviceData.subCategoryIds.map((id) =>
					typeof id === "string" ? Number(id) : id
				);
			}

			console.log(`Updating service ${serviceId} with data:`, serviceData);

			const response = await fetch(`${API_BASE_URL}/api/service/${serviceId}`, {
				method: "PUT",
				headers: {
					"Content-Type": "application/json",
					Authorization: `Bearer ${token}`,
				},
				body: JSON.stringify(serviceData),
			});

			if (!response.ok) {
				const errorText = await response.text();
				console.error(`Failed to update service ${serviceId}:`, errorText);
				throw new Error(`Failed to update service: ${errorText}`);
			}

			// For 204 No Content responses, return a success object
			if (response.status === 204) {
				return { success: true, serviceId };
			}

			return await response.json();
		} catch (error) {
			console.error("Error updating service:", error);
			throw error;
		}
	},

	/**
	 * Delete a service
	 * @param {number} serviceId - Service ID
	 * @param {string} token - Authentication token
	 * @returns {Promise<boolean>} - Success status
	 */
	async deleteService(serviceId, token) {
		try {
			const response = await fetch(`${API_BASE_URL}/api/service/${serviceId}`, {
				method: "DELETE",
				headers: {
					Authorization: `Bearer ${token}`,
				},
			});

			if (!response.ok) {
				const errorText = await response.text();
				throw new Error(`Failed to delete service: ${errorText}`);
			}

			return true;
		} catch (error) {
			console.error("Error deleting service:", error);
			throw error;
		}
	},

	/**
	 * Approve a service
	 * @param {number} serviceId - Service ID
	 * @param {string} token - Authentication token
	 * @returns {Promise<boolean>} - Success status
	 */
	async approveService(serviceId, token) {
		try {
			const response = await fetch(
				`${API_BASE_URL}/api/service/approve/${serviceId}`,
				{
					method: "PUT",
					headers: {
						Authorization: `Bearer ${token}`,
					},
				}
			);

			if (!response.ok) {
				const errorText = await response.text();
				throw new Error(`Failed to approve service: ${errorText}`);
			}

			return true;
		} catch (error) {
			console.error("Error approving service:", error);
			throw error;
		}
	},

	/**
	 * Revoke approval of a service
	 * @param {number} serviceId - Service ID
	 * @param {string} token - Authentication token
	 * @returns {Promise<boolean>} - Success status
	 */
	async revokeApproval(serviceId, token) {
		try {
			const response = await fetch(
				`${API_BASE_URL}/api/service/revoke/${serviceId}`,
				{
					method: "PUT",
					headers: {
						Authorization: `Bearer ${token}`,
					},
				}
			);

			if (!response.ok) {
				const errorText = await response.text();
				throw new Error(`Failed to revoke approval: ${errorText}`);
			}

			return true;
		} catch (error) {
			console.error("Error revoking approval:", error);
			throw error;
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
