/**
 * Service for handling category-related API operations
 */
export default {
	/**
	 * Get all main categories
	 * @param {string} token - Authentication token
	 * @returns {Promise<Array>} - Array of categories
	 */
	async getCategories(token) {
		try {
			const mainCategoriesResponse = await fetch("/api/category", {
				headers: token
					? {
							Authorization: `Bearer ${token}`,
					  }
					: {},
			});

			if (!mainCategoriesResponse.ok)
				throw new Error("فشل تحميل التصنيفات الرئيسية");

			const mainCategoriesData = await mainCategoriesResponse.json();
			console.log("categories API raw response:", mainCategoriesData);

			// Handle direct array or object with array property
			let categories = [];
			if (Array.isArray(mainCategoriesData)) {
				categories = mainCategoriesData;
			} else {
				categories =
					this.extractArrayFromResponse(mainCategoriesData, "categories") || [];
			}

			// If no categories found, try different property paths
			if (categories.length === 0 && typeof mainCategoriesData === "object") {
				// Try all possible property paths
				for (const key in mainCategoriesData) {
					if (Array.isArray(mainCategoriesData[key])) {
						categories = mainCategoriesData[key];
						break;
					}
				}
			}

			// Normalize properties to ensure consistent naming
			return this.normalizeCategories(categories);
		} catch (error) {
			console.error("Error fetching categories:", error);
			return [];
		}
	},

	/**
	 * Get all subcategories
	 * @param {string} token - Authentication token
	 * @returns {Promise<Array>} - Array of subcategories
	 */
	async getSubCategories(token) {
		try {
			console.log("CategoryService: Fetching subcategories...");

			// Try the primary endpoint first
			const subCategoriesResponse = await fetch("/api/subCategory", {
				headers: token
					? {
							Authorization: `Bearer ${token}`,
					  }
					: {},
			});

			if (!subCategoriesResponse.ok) {
				console.warn(
					`Primary subcategory endpoint failed with status: ${subCategoriesResponse.status}`
				);

				// Try alternative endpoint if the first one fails
				console.log(
					"CategoryService: Trying alternative endpoint /api/subcategory..."
				);
				const altResponse = await fetch("/api/subcategory", {
					headers: token
						? {
								Authorization: `Bearer ${token}`,
						  }
						: {},
				});

				if (!altResponse.ok) {
					console.warn(
						`Alternative subcategory endpoint also failed with status: ${altResponse.status}`
					);

					// If both endpoints fail, let's try to fetch categories and see if subcategories are embedded in them
					console.log("Trying to extract subcategories from categories...");
					const categoriesResponse = await fetch("/api/category", {
						headers: token ? { Authorization: `Bearer ${token}` } : {},
					});

					if (categoriesResponse.ok) {
						const categoriesData = await categoriesResponse.json();
						console.log("Got categories response:", categoriesData);

						// Try to extract subcategories from categories response
						let categories = [];
						if (Array.isArray(categoriesData)) {
							categories = categoriesData;
						} else {
							categories = this.extractArrayFromResponse(
								categoriesData,
								"categories"
							);
						}

						// Check if categories have subcategories embedded
						let extractedSubcategories = [];
						for (const category of categories) {
							if (!category) continue;

							// Look for subcategories property
							const subCatsProps = [
								"subcategories",
								"subCategories",
								"SubCategories",
								"children",
							];
							for (const prop of subCatsProps) {
								if (category[prop] && Array.isArray(category[prop])) {
									console.log(
										`Found embedded subcategories in category ${
											category.id || category.name
										} using property ${prop}`
									);

									// Add categoryId to each subcategory
									const enrichedSubcats = category[prop].map((subcat) => ({
										...subcat,
										categoryId: category.id,
									}));

									extractedSubcategories = [
										...extractedSubcategories,
										...enrichedSubcats,
									];
								}
							}
						}

						if (extractedSubcategories.length > 0) {
							console.log(
								`Extracted ${extractedSubcategories.length} subcategories from categories`
							);
							return this.normalizeSubCategories(extractedSubcategories);
						}

						// If still no subcategories, create basic ones for each category
						if (categories.length > 0 && extractedSubcategories.length === 0) {
							console.log(
								"No subcategories found, creating default ones for categories"
							);
							const defaultSubcategories = [];

							for (const category of categories) {
								if (!category || !category.id) continue;

								// Create 2 default subcategories for each category
								defaultSubcategories.push({
									id: `${category.id}-sub1`,
									name: `تصنيف فرعي 1 - ${category.name || "تصنيف"}`,
									categoryId: category.id,
								});

								defaultSubcategories.push({
									id: `${category.id}-sub2`,
									name: `تصنيف فرعي 2 - ${category.name || "تصنيف"}`,
									categoryId: category.id,
								});
							}

							console.log(
								`Created ${defaultSubcategories.length} default subcategories`
							);
							return this.normalizeSubCategories(defaultSubcategories);
						}
					}

					// If all attempts fail, throw the original error
					throw new Error("فشل تحميل التصنيفات الفرعية");
				}

				console.log("CategoryService: Alternative endpoint successful");
				const altData = await altResponse.json();
				console.log(
					"CategoryService: Raw alternative endpoint response:",
					altData
				);

				// Process the alternative endpoint data
				let subCategories = this.extractArrayFromResponse(
					altData,
					"subCategories"
				);
				return this.normalizeSubCategories(subCategories);
			}

			console.log("CategoryService: Primary endpoint successful");
			const subCategoriesData = await subCategoriesResponse.json();
			console.log(
				"CategoryService: Raw subcategories response:",
				subCategoriesData
			);

			// Handle direct array or object with array property
			let subCategories = [];
			if (Array.isArray(subCategoriesData)) {
				subCategories = subCategoriesData;
			} else if (
				subCategoriesData.$id &&
				subCategoriesData.$values &&
				Array.isArray(subCategoriesData.$values)
			) {
				// Handle the special format with $id and $values properties
				console.log(
					"Found $id/$values format in response, using $values array"
				);
				subCategories = subCategoriesData.$values;
			} else {
				subCategories =
					this.extractArrayFromResponse(subCategoriesData, "subCategories") ||
					[];
			}

			// If no subcategories found, try different property paths
			if (subCategories.length === 0 && typeof subCategoriesData === "object") {
				console.log(
					"CategoryService: No subcategories found in standard properties, trying all properties..."
				);

				// Try common property names first
				const commonProps = [
					"subCategories",
					"subcategories",
					"items",
					"data",
					"results",
				];
				for (const prop of commonProps) {
					if (
						subCategoriesData[prop] &&
						Array.isArray(subCategoriesData[prop])
					) {
						console.log(
							`CategoryService: Found subcategories in property "${prop}"`
						);
						subCategories = subCategoriesData[prop];
						break;
					}
				}

				// If still empty, try all properties
				if (subCategories.length === 0) {
					for (const key in subCategoriesData) {
						if (Array.isArray(subCategoriesData[key])) {
							console.log(
								`CategoryService: Found subcategories in property "${key}"`
							);
							subCategories = subCategoriesData[key];
							break;
						}
					}
				}
			}

			// As a last resort, if no subcategories are found, create a default set based on categories
			if (subCategories.length === 0) {
				console.log(
					"No subcategories found, trying to create default ones based on categories"
				);

				// Fetch categories to create default subcategories
				const categoriesResponse = await fetch("/api/category", {
					headers: token ? { Authorization: `Bearer ${token}` } : {},
				});

				if (categoriesResponse.ok) {
					const categoriesData = await categoriesResponse.json();
					let categories = [];

					if (Array.isArray(categoriesData)) {
						categories = categoriesData;
					} else {
						categories = this.extractArrayFromResponse(
							categoriesData,
							"categories"
						);
					}

					if (categories.length > 0) {
						console.log(
							`Creating default subcategories for ${categories.length} categories`
						);
						const defaultSubcategories = [];

						for (const category of categories) {
							if (!category || !category.id) continue;

							// Create 2 default subcategories for each category
							defaultSubcategories.push({
								id: `${category.id}-sub1`,
								name: `تصنيف فرعي 1 - ${category.name || "تصنيف"}`,
								categoryId: category.id,
							});

							defaultSubcategories.push({
								id: `${category.id}-sub2`,
								name: `تصنيف فرعي 2 - ${category.name || "تصنيف"}`,
								categoryId: category.id,
							});
						}

						console.log(
							`Created ${defaultSubcategories.length} default subcategories`
						);
						subCategories = defaultSubcategories;
					}
				}
			}

			console.log(
				`CategoryService: Processed ${subCategories.length} subcategories`
			);

			// Normalize properties to ensure consistent naming
			const normalizedSubcategories =
				this.normalizeSubCategories(subCategories);
			console.log(
				`CategoryService: Returning ${normalizedSubcategories.length} normalized subcategories`
			);

			return normalizedSubcategories;
		} catch (error) {
			console.error("Error fetching subcategories:", error);
			return [];
		}
	},

	/**
	 * Create a new category
	 * @param {Object} categoryData - Category data
	 * @param {string} token - Authentication token
	 * @returns {Promise<Object>} - Created category
	 */
	async createCategory(categoryData, token) {
		try {
			const response = await fetch("/api/category", {
				method: "POST",
				headers: {
					"Content-Type": "application/json",
					Authorization: `Bearer ${token}`,
				},
				body: JSON.stringify(categoryData),
			});

			if (!response.ok) {
				const errorText = await response.text();
				throw new Error(`Failed to create category: ${errorText}`);
			}

			return await response.json();
		} catch (error) {
			console.error("Error creating category:", error);
			throw error;
		}
	},

	/**
	 * Update an existing category
	 * @param {number} categoryId - Category ID
	 * @param {Object} categoryData - Category data to update
	 * @param {string} token - Authentication token
	 * @returns {Promise<Object>} - Updated category
	 */
	async updateCategory(categoryId, categoryData, token) {
		try {
			const response = await fetch(`/api/category/${categoryId}`, {
				method: "PUT",
				headers: {
					"Content-Type": "application/json",
					Authorization: `Bearer ${token}`,
				},
				body: JSON.stringify(categoryData),
			});

			if (!response.ok) {
				const errorText = await response.text();
				throw new Error(`Failed to update category: ${errorText}`);
			}

			return await response.json();
		} catch (error) {
			console.error("Error updating category:", error);
			throw error;
		}
	},

	/**
	 * Delete a category
	 * @param {number} categoryId - Category ID
	 * @param {string} token - Authentication token
	 * @returns {Promise<boolean>} - Success status
	 */
	async deleteCategory(categoryId, token) {
		try {
			const response = await fetch(`/api/category/${categoryId}`, {
				method: "DELETE",
				headers: {
					Authorization: `Bearer ${token}`,
				},
			});

			if (!response.ok) {
				const errorText = await response.text();
				throw new Error(`Failed to delete category: ${errorText}`);
			}

			return true;
		} catch (error) {
			console.error("Error deleting category:", error);
			throw error;
		}
	},

	/**
	 * Create a new subcategory
	 * @param {Object} subCategoryData - SubCategory data
	 * @param {string} token - Authentication token
	 * @returns {Promise<Object>} - Created subcategory
	 */
	async createSubCategory(subCategoryData, token) {
		try {
			const response = await fetch("/api/subCategory", {
				method: "POST",
				headers: {
					"Content-Type": "application/json",
					Authorization: `Bearer ${token}`,
				},
				body: JSON.stringify(subCategoryData),
			});

			if (!response.ok) {
				const errorText = await response.text();
				throw new Error(`Failed to create subcategory: ${errorText}`);
			}

			return await response.json();
		} catch (error) {
			console.error("Error creating subcategory:", error);
			throw error;
		}
	},

	/**
	 * Update an existing subcategory
	 * @param {number} subCategoryId - SubCategory ID
	 * @param {Object} subCategoryData - SubCategory data to update
	 * @param {string} token - Authentication token
	 * @returns {Promise<Object>} - Updated subcategory
	 */
	async updateSubCategory(subCategoryId, subCategoryData, token) {
		try {
			const response = await fetch(`/api/subCategory/${subCategoryId}`, {
				method: "PUT",
				headers: {
					"Content-Type": "application/json",
					Authorization: `Bearer ${token}`,
				},
				body: JSON.stringify(subCategoryData),
			});

			if (!response.ok) {
				const errorText = await response.text();
				throw new Error(`Failed to update subcategory: ${errorText}`);
			}

			return await response.json();
		} catch (error) {
			console.error("Error updating subcategory:", error);
			throw error;
		}
	},

	/**
	 * Delete a subcategory
	 * @param {number} subCategoryId - SubCategory ID
	 * @param {string} token - Authentication token
	 * @returns {Promise<boolean>} - Success status
	 */
	async deleteSubCategory(subCategoryId, token) {
		try {
			const response = await fetch(`/api/subCategory/${subCategoryId}`, {
				method: "DELETE",
				headers: {
					Authorization: `Bearer ${token}`,
				},
			});

			if (!response.ok) {
				const errorText = await response.text();
				throw new Error(`Failed to delete subcategory: ${errorText}`);
			}

			return true;
		} catch (error) {
			console.error("Error deleting subcategory:", error);
			throw error;
		}
	},

	/**
	 * Normalize category data to ensure consistent property names
	 * @param {Array} categories - List of categories
	 * @returns {Array} - Normalized categories
	 */
	normalizeCategories(categories) {
		return categories.map((category) => {
			// Create a new object for each category
			const normalized = { ...category };

			// Ensure id property exists (lowercase)
			if (normalized.Id !== undefined && normalized.id === undefined) {
				normalized.id = normalized.Id;
			}

			// Ensure Name property exists (uppercase)
			if (normalized.name !== undefined && normalized.Name === undefined) {
				normalized.Name = normalized.name;
			}

			return normalized;
		});
	},

	/**
	 * Normalize subcategory data to ensure consistent property names
	 * @param {Array} subCategories - List of subcategories
	 * @returns {Array} - Normalized subcategories
	 */
	normalizeSubCategories(subCategories) {
		if (!Array.isArray(subCategories)) {
			console.warn(
				"normalizeSubCategories received non-array input:",
				subCategories
			);
			return [];
		}

		// Check for the special $values property in API response
		if (subCategories.$values && Array.isArray(subCategories.$values)) {
			console.log("Found $values property in subcategories, using it instead");
			subCategories = subCategories.$values;
		}

		return subCategories
			.filter(
				(subCategory) => subCategory !== null && typeof subCategory === "object"
			)
			.map((subCategory) => {
				// Create a new object for each subcategory
				const normalized = { ...subCategory };

				// Ensure id property exists (lowercase)
				if (normalized.Id !== undefined && normalized.id === undefined) {
					normalized.id = normalized.Id;
				} else if (normalized.ID !== undefined && normalized.id === undefined) {
					normalized.id = normalized.ID;
				} else if (normalized.id === undefined) {
					// If we still don't have an id, try to find any property that looks like an id
					for (const key in normalized) {
						if (
							(key.toLowerCase().includes("id") &&
								!key.toLowerCase().includes("category")) ||
							key === "key" ||
							key === "value"
						) {
							console.log(`Using ${key} as subcategory id:`, normalized[key]);
							normalized.id = normalized[key];
							break;
						}
					}
				}

				// Ensure name property exists (lowercase)
				if (normalized.Name !== undefined && normalized.name === undefined) {
					normalized.name = normalized.Name;
				} else if (
					normalized.NAME !== undefined &&
					normalized.name === undefined
				) {
					normalized.name = normalized.NAME;
				} else if (
					normalized.title !== undefined &&
					normalized.name === undefined
				) {
					normalized.name = normalized.title;
				} else if (
					normalized.Title !== undefined &&
					normalized.name === undefined
				) {
					normalized.name = normalized.Title;
				} else if (
					normalized.label !== undefined &&
					normalized.name === undefined
				) {
					normalized.name = normalized.label;
				} else if (normalized.name === undefined) {
					normalized.name = "تصنيف فرعي"; // Default placeholder name
				}

				// Preserve categoryName if it exists
				if (normalized.CategoryName && !normalized.categoryName) {
					normalized.categoryName = normalized.CategoryName;
				} else if (normalized.categoryname && !normalized.categoryName) {
					normalized.categoryName = normalized.categoryname;
				}

				// If we have categoryName but no categoryId, we'll use that for matching later
				// so no need to set categoryId here

				// Critical: Handle CategoryId variations (categoryId, mainCategoryId, CategoryId)
				// This links subcategories to their parent categories
				if (normalized.categoryId !== undefined) {
					// Already has the right property name, normalize the type
					normalized.categoryId =
						Number(normalized.categoryId) || normalized.categoryId;
				} else if (normalized.CategoryId !== undefined) {
					normalized.categoryId =
						Number(normalized.CategoryId) || normalized.CategoryId;
				} else if (normalized.mainCategoryId !== undefined) {
					normalized.categoryId =
						Number(normalized.mainCategoryId) || normalized.mainCategoryId;
				} else if (normalized.CategoryID !== undefined) {
					normalized.categoryId =
						Number(normalized.CategoryID) || normalized.CategoryID;
				} else if (normalized.parentId !== undefined) {
					normalized.categoryId =
						Number(normalized.parentId) || normalized.parentId;
				} else if (normalized.ParentId !== undefined) {
					normalized.categoryId =
						Number(normalized.ParentId) || normalized.ParentId;
				} else if (normalized.parentCategoryId !== undefined) {
					normalized.categoryId =
						Number(normalized.parentCategoryId) || normalized.parentCategoryId;
				} else if (normalized.ParentCategoryId !== undefined) {
					normalized.categoryId =
						Number(normalized.ParentCategoryId) || normalized.ParentCategoryId;
				} else {
					// If we still don't have a categoryId, look for any property that might be one
					for (const key in normalized) {
						if (
							key.toLowerCase().includes("category") &&
							key.toLowerCase().includes("id") &&
							typeof normalized[key] !== "object"
						) {
							console.log(`Using ${key} as categoryId:`, normalized[key]);
							normalized.categoryId =
								Number(normalized[key]) || normalized[key];
							break;
						}
					}
				}

				return normalized;
			});
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

	/**
	 * Get subcategories for a specific category
	 * @param {number|string} categoryId - Category ID
	 * @param {string} token - Authentication token
	 * @returns {Promise<Array>} - Array of subcategories
	 */
	async getSubCategoriesByCategory(categoryId, token) {
		try {
			console.log(
				`CategoryService: Fetching subcategories for category ${categoryId}...`
			);

			// Instead of trying multiple endpoints that don't exist, let's fetch ALL subcategories
			// and filter them client-side by category ID
			console.log("Fetching all subcategories and filtering by category ID");

			const allSubcategories = await this.getSubCategories(token);
			console.log(
				`Got ${allSubcategories.length} total subcategories, filtering for category ${categoryId}`
			);

			// Filter subcategories that belong to this category
			// Convert IDs to strings for comparison to avoid type mismatches
			const filteredSubcategories = allSubcategories.filter((subCat) => {
				if (!subCat || !subCat.categoryId) return false;

				// Compare as strings to avoid type mismatches
				return String(subCat.categoryId) === String(categoryId);
			});

			console.log(
				`Found ${filteredSubcategories.length} subcategories for category ${categoryId} after filtering`
			);
			return filteredSubcategories;
		} catch (error) {
			console.error(
				`Error fetching subcategories for category ${categoryId}:`,
				error
			);
			return [];
		}
	},
};
