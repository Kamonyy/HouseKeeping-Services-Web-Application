/**
 * Handles different API response formats and extracts an array from them.
 * @param {any} responseData - The data received from an API call
 * @param {string} label - A label for logging purposes (e.g., 'categories', 'services')
 * @returns {Array} - The extracted array or an empty array if none is found
 */
export const extractArrayFromResponse = (responseData, label = "items") => {
	// Log the response for debugging
	console.log(`${label} API response:`, responseData);

	// If it's already an array, return it directly
	if (Array.isArray(responseData)) {
		return responseData;
	}

	// If it's an object, try to find an array inside it
	if (responseData && typeof responseData === "object") {
		// Try standard API response patterns
		if (responseData.items && Array.isArray(responseData.items)) {
			return responseData.items;
		}

		if (responseData.data && Array.isArray(responseData.data)) {
			return responseData.data;
		}

		// Look for any property that contains an array
		const possibleArrays = Object.values(responseData).filter((val) =>
			Array.isArray(val)
		);
		if (possibleArrays.length > 0) {
			return possibleArrays[0];
		}

		// If we get here, we couldn't find an array in the response
		console.warn(`Could not find ${label} array in API response`, responseData);
	} else {
		// Not an array or object
		console.warn(`Unexpected API response format for ${label}`, responseData);
	}

	// Return empty array as fallback
	return [];
};

/**
 * Extracts the token from different login response formats
 * @param {any} responseData - The data received from a login API call
 * @param {string} defaultUsername - The username to use if not found in the response
 * @returns {Object} - Object containing extracted username and token
 */
export const extractAuthFromResponse = (responseData, defaultUsername) => {
	let username = defaultUsername;
	let token = null;

	if (typeof responseData === "string") {
		// If the response is just a token string
		token = responseData;
	} else if (typeof responseData === "object") {
		// If response is an object, extract token and username
		token = responseData.token || responseData.accessToken || responseData.jwt;
		username =
			responseData.username || responseData.userName || defaultUsername;
	}

	return { username, token };
};

/**
 * Normalizes the property names in an array of objects to ensure consistent casing
 * @param {Array} items - The array of objects to normalize
 * @param {Object} propertyMap - Map of property names: { originalName: normalizedName }
 * @returns {Array} - The normalized array of objects
 */
export const normalizePropertyNames = (items, propertyMap) => {
	if (!Array.isArray(items)) return [];

	return items.map((item) => {
		const normalized = { ...item };

		Object.entries(propertyMap).forEach(([original, normalized]) => {
			// If the normalized property doesn't exist but the original does
			if (item[original] !== undefined && item[normalized] === undefined) {
				normalized[normalized] = item[original];
			}
		});

		return normalized;
	});
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
