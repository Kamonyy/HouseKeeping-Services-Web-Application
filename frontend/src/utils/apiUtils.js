/**
 * Utility functions for working with API responses
 */

/**
 * Extracts an array from different API response formats
 * @param {any} responseData - API response data
 * @param {string} label - Property name to look for (optional)
 * @returns {Array} - Extracted array or empty array if not found
 */
export const extractArrayFromResponse = (responseData, label = "items") => {
	// Already an array - return directly
	if (Array.isArray(responseData)) {
		return responseData;
	}

	// Handle object responses with different structures
	if (responseData && typeof responseData === "object") {
		// Check for the specified property first
		if (label && responseData[label] && Array.isArray(responseData[label])) {
			return responseData[label];
		}

		// Try common property names used in API responses
		for (const prop of ["items", "data", "results", "list"]) {
			if (responseData[prop] && Array.isArray(responseData[prop])) {
				return responseData[prop];
			}
		}

		// Look for any array property as last resort
		const arrayProps = Object.values(responseData).filter((val) =>
			Array.isArray(val)
		);
		if (arrayProps.length > 0) {
			return arrayProps[0];
		}
	}

	return [];
};

/**
 * Extracts auth data from login responses
 * @param {any} responseData - Login API response
 * @param {string} defaultUsername - Fallback username
 * @returns {Object} - Auth information
 */
export const extractAuthFromResponse = (responseData, defaultUsername) => {
	// Initialize with defaults
	const auth = {
		username: defaultUsername,
		token: null,
		firstName: "",
		lastName: "",
	};

	// Handle string token response
	if (typeof responseData === "string") {
		auth.token = responseData;
		return auth;
	}

	// Handle object response
	if (responseData && typeof responseData === "object") {
		// Extract token
		auth.token =
			responseData.token ||
			responseData.accessToken ||
			responseData.jwt ||
			responseData.idToken ||
			null;

		// Extract username
		auth.username =
			responseData.username ||
			responseData.userName ||
			responseData.user?.username ||
			defaultUsername;

		// Extract name fields using common naming variations
		const nameVariants = {
			first: [
				"firstName",
				"FirstName",
				"firstname",
				"first_name",
				"given_name",
			],
			last: [
				"lastName",
				"LastName",
				"lastname",
				"last_name",
				"family_name",
				"surname",
			],
		};

		// Find first name match
		for (const field of nameVariants.first) {
			if (responseData[field]) {
				auth.firstName = responseData[field];
				break;
			}
		}

		// Find last name match
		for (const field of nameVariants.last) {
			if (responseData[field]) {
				auth.lastName = responseData[field];
				break;
			}
		}
	}

	return auth;
};

/**
 * Normalizes property names in objects for consistent access
 * @param {Array} items - Array of objects to normalize
 * @param {Object} propertyMap - Mapping of original to normalized names
 * @returns {Array} - Normalized objects
 */
export const normalizePropertyNames = (items, propertyMap) => {
	if (!Array.isArray(items)) return [];

	return items.map((item) => {
		const normalized = { ...item };

		// Copy values from original properties to normalized names
		for (const [original, normalizedName] of Object.entries(propertyMap)) {
			if (
				item[original] !== undefined &&
				normalized[normalizedName] === undefined
			) {
				normalized[normalizedName] = item[original];
			}
		}

		return normalized;
	});
};

/**
 * Finds a category by ID in an array of categories
 * @param {Array} categories - Array of category objects
 * @param {string|number} id - ID to find
 * @returns {Object|null} - Found category or null
 */
export const findCategoryById = (categories, id) => {
	if (!categories?.length || !id) return null;

	const targetId = String(id);
	return (
		categories.find((cat) => String(cat.id || cat.Id) === targetId) || null
	);
};

/**
 * Safely parses JSON without throwing errors
 * @param {string} text - JSON text to parse
 * @param {any} fallback - Default value if parsing fails
 * @returns {any} - Parsed object or fallback
 */
export const safeJsonParse = (text, fallback = null) => {
	if (!text) return fallback;

	try {
		return JSON.parse(text);
	} catch (err) {
		return fallback;
	}
};

/**
 * Logs the details of categories and subcategories to help with debugging
 * @param {Array} categories - The array of category objects
 * @param {Array} subcategories - The array of subcategory objects
 */
export const debugCategoryRelationships = (categories, subcategories) => {
	console.log("===== DEBUGGING CATEGORY RELATIONSHIPS =====");

	console.log("CATEGORIES:");
	categories.forEach((cat) => {
		console.log(
			`Category ID: ${cat.id || cat.Id}, Name: ${cat.Name || cat.name}`
		);
		console.log(
			"All properties:",
			Object.keys(cat)
				.map((key) => `${key}: ${cat[key]}`)
				.join(", ")
		);
	});

	console.log("SUBCATEGORIES:");
	subcategories.forEach((subcat) => {
		const parentId =
			subcat.CategoryId || subcat.categoryId || subcat.mainCategoryId;
		console.log(
			`Subcategory ID: ${subcat.id || subcat.Id}, Name: ${
				subcat.Name || subcat.name
			}, Parent ID: ${parentId}`
		);
		console.log(
			"All properties:",
			Object.keys(subcat)
				.map((key) => `${key}: ${subcat[key]}`)
				.join(", ")
		);

		// Try to find the parent
		const parent = categories.find(
			(cat) => String(cat.id || cat.Id) === String(parentId)
		);

		if (parent) {
			console.log(`✅ Found parent: ${parent.Name || parent.name}`);
		} else {
			console.log(`❌ Parent not found for ID: ${parentId}`);
			console.log("Comparison with available category IDs:");
			categories.forEach((cat) => {
				const catId = cat.id || cat.Id;
				console.log(
					`Category ID: ${catId} (${typeof catId}) === Subcategory parent ID: ${parentId} (${typeof parentId}) => ${
						String(catId) === String(parentId)
					}`
				);
			});
		}
		console.log("---");
	});

	console.log("===========================================");
};
