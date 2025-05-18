/**
 * Service for handling service-related API operations
 */
const API_URL = "https://localhost:7007";

class ServiceService {
	/**
	 * Gets all services for a provider
	 * @param {string} token - Auth token
	 * @returns {Promise<Array>} Provider's services
	 */
	async getProviderServices(token) {
		try {
			const response = await fetch(`${API_URL}/api/service/provider`, {
				headers: { Authorization: `Bearer ${token}` },
			});

			// Return empty array for 404 (no services yet)
			if (response.status === 404) {
				return [];
			}

			// Handle other errors
			if (!response.ok) {
				const text = await response.text();
				throw new Error(`Failed to load provider services: ${text}`);
			}

			return await response.json();
		} catch (error) {
			console.warn("Error fetching provider services:", error.message);
			throw error;
		}
	}

	/**
	 * Gets a service by its ID
	 * @param {number} serviceId - Service ID
	 * @param {string} token - Optional auth token
	 * @returns {Promise<Object>} Service details
	 */
	async getServiceById(serviceId, token = null) {
		try {
			const headers = {};
			if (token) {
				headers.Authorization = `Bearer ${token}`;
			}

			const response = await fetch(`${API_URL}/api/service/${serviceId}`, {
				headers,
			});

			if (response.status === 404) {
				throw new Error("Service not found");
			}

			if (!response.ok) {
				throw new Error(`Failed to load service details: ${response.status}`);
			}

			return await response.json();
		} catch (error) {
			console.warn(`Error fetching service ${serviceId}:`, error.message);
			throw error;
		}
	}

	/**
	 * Gets all services for admin
	 * @param {string} token - Auth token
	 * @returns {Promise<Array>} All services
	 */
	async getAdminServices(token) {
		try {
			const response = await fetch(`${API_URL}/api/service/admin`, {
				headers: { Authorization: `Bearer ${token}` },
			});

			if (!response.ok) {
				const text = await response.text();
				throw new Error(`Failed to load admin services: ${text}`);
			}

			return await response.json();
		} catch (error) {
			console.warn("Error fetching admin services:", error.message);
			throw error;
		}
	}

	/**
	 * Creates a new service
	 * @param {Object} serviceData - Service data
	 * @param {string} token - Auth token
	 * @returns {Promise<Object>} Created service
	 */
	async createService(serviceData, token) {
		try {
			const response = await fetch(`${API_URL}/api/service`, {
				method: "POST",
				headers: this.getJsonHeaders(token),
				body: JSON.stringify(serviceData),
			});

			if (!response.ok) {
				const text = await response.text();
				throw new Error(`Failed to create service: ${text}`);
			}

			return await response.json();
		} catch (error) {
			console.warn("Error creating service:", error.message);
			throw error;
		}
	}

	/**
	 * Updates an existing service
	 * @param {number} serviceId - Service ID
	 * @param {Object} serviceData - Updated service data
	 * @param {string} token - Auth token
	 * @returns {Promise<Object>} Updated service
	 */
	async updateService(serviceId, serviceData, token) {
		try {
			// Ensure subcategory IDs are numbers
			if (serviceData.subCategoryIds) {
				serviceData.subCategoryIds = serviceData.subCategoryIds.map((id) =>
					typeof id === "string" ? Number(id) : id
				);
			}

			const response = await fetch(`${API_URL}/api/service/${serviceId}`, {
				method: "PUT",
				headers: this.getJsonHeaders(token),
				body: JSON.stringify(serviceData),
			});

			if (!response.ok) {
				const text = await response.text();
				throw new Error(`Failed to update service: ${text}`);
			}

			// Handle 204 No Content response
			if (response.status === 204) {
				return { success: true, serviceId };
			}

			return await response.json();
		} catch (error) {
			console.warn(`Error updating service ${serviceId}:`, error.message);
			throw error;
		}
	}

	/**
	 * Deletes a service
	 * @param {number} serviceId - Service ID
	 * @param {string} token - Auth token
	 * @returns {Promise<boolean>} Success status
	 */
	async deleteService(serviceId, token) {
		try {
			const response = await fetch(`${API_URL}/api/service/${serviceId}`, {
				method: "DELETE",
				headers: { Authorization: `Bearer ${token}` },
			});

			if (!response.ok) {
				const text = await response.text();
				throw new Error(`Failed to delete service: ${text}`);
			}

			return true;
		} catch (error) {
			console.warn(`Error deleting service ${serviceId}:`, error.message);
			throw error;
		}
	}

	/**
	 * Approves a service (admin only)
	 * @param {number} serviceId - Service ID
	 * @param {string} token - Admin auth token
	 * @returns {Promise<boolean>} Success status
	 */
	async approveService(serviceId, token) {
		try {
			const response = await fetch(
				`${API_URL}/api/service/${serviceId}/approve`,
				{
					method: "POST",
					headers: { Authorization: `Bearer ${token}` },
				}
			);

			if (!response.ok) {
				const text = await response.text();
				throw new Error(`Failed to approve service: ${text}`);
			}

			return true;
		} catch (error) {
			console.warn(`Error approving service ${serviceId}:`, error.message);
			throw error;
		}
	}

	/**
	 * Revokes approval for a service (admin only)
	 * @param {number} serviceId - Service ID
	 * @param {string} token - Admin auth token
	 * @returns {Promise<boolean>} Success status
	 */
	async revokeApproval(serviceId, token) {
		try {
			const response = await fetch(
				`${API_URL}/api/service/${serviceId}/revoke`,
				{
					method: "POST",
					headers: { Authorization: `Bearer ${token}` },
				}
			);

			if (!response.ok) {
				const text = await response.text();
				throw new Error(`Failed to revoke service approval: ${text}`);
			}

			return true;
		} catch (error) {
			console.warn(
				`Error revoking approval for service ${serviceId}:`,
				error.message
			);
			throw error;
		}
	}

	/**
	 * Extracts array data from different response formats
	 * @param {Object|Array} data - Response data
	 * @param {string} defaultArrayName - Default array property name
	 * @returns {Array} Extracted array
	 */
	extractArrayFromResponse(data, defaultArrayName = "items") {
		// Return if already an array
		if (Array.isArray(data)) {
			return data;
		}

		// Handle object responses
		if (data && typeof data === "object") {
			// Check specific property first
			if (data[defaultArrayName] && Array.isArray(data[defaultArrayName])) {
				return data[defaultArrayName];
			}

			// Check common property names
			const commonProps = ["items", "results", "data", "list"];
			for (const prop of commonProps) {
				if (data[prop] && Array.isArray(data[prop])) {
					return data[prop];
				}
			}

			// Find first array property
			const arrayProps = Object.values(data).filter((val) =>
				Array.isArray(val)
			);
			if (arrayProps.length > 0) {
				return arrayProps[0];
			}
		}

		return [];
	}

	// Helper methods

	/**
	 * Gets headers for JSON requests
	 */
	getJsonHeaders(token) {
		return {
			"Content-Type": "application/json",
			Authorization: `Bearer ${token}`,
		};
	}
}

// Export a singleton instance
export default new ServiceService();
