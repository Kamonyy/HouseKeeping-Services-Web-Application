/**
 * Utility function to get a default image URL for a category
 * This helps ensure we always have appropriate images even if the API doesn't provide them
 *
 * @param {Object} category - The category object
 * @returns {string} - The image URL to use
 */
export const getCategoryImage = (category) => {
	if (!category)
		return "https://images.pexels.com/photos/3747463/pexels-photo-3747463.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1";

	// If the category has an image URL, use it
	if (category.imageUrl || category.ImageUrl) {
		const imageUrl = category.imageUrl || category.ImageUrl;
		// Check if it's a local path or already a URL
		if (imageUrl.startsWith("http")) {
			return imageUrl;
		}
		// If it's a local path, convert it to an appropriate Unsplash image
		return getCategoryImageByName(category.name || category.Name || "");
	}

	// Get the category name
	const name = category.name || category.Name || "";
	const id = category.id || category.Id || 0;

	return getCategoryImageByName(name, id);
};

/**
 * Helper function to get an appropriate Unsplash image based on category name
 */
const getCategoryImageByName = (name, id = 0) => {
	// Check the name for relevant keywords and return appropriate images
	const lowerName = name.toLowerCase();

	// Map of keywords to image URLs from Unsplash
	const imageMap = {
		// Cleaning related categories
		تنظيف:
			"https://images.pexels.com/photos/4107120/pexels-photo-4107120.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
		نظافة:
			"https://images.pexels.com/photos/4239091/pexels-photo-4239091.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
		clean:
			"https://images.pexels.com/photos/5591579/pexels-photo-5591579.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",

		// Electrical related categories
		كهرباء:
			"https://images.pexels.com/photos/8005397/pexels-photo-8005397.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
		كهربائية:
			"https://images.pexels.com/photos/257886/pexels-photo-257886.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
		electric:
			"https://images.pexels.com/photos/8005398/pexels-photo-8005398.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",

		// Plumbing related categories
		سباكة:
			"https://images.pexels.com/photos/6419128/pexels-photo-6419128.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
		مياه: "https://images.pexels.com/photos/6419146/pexels-photo-6419146.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
		plumb:
			"https://images.pexels.com/photos/5591630/pexels-photo-5591630.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",

		// Carpentry related categories
		نجارة:
			"https://images.pexels.com/photos/6419123/pexels-photo-6419123.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
		خشب: "https://images.pexels.com/photos/6969866/pexels-photo-6969866.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
		wood: "https://images.pexels.com/photos/6969831/pexels-photo-6969831.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",

		// Painting related categories
		دهان: "https://images.pexels.com/photos/6598/coffee-desk-notes-workspace.jpg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
		طلاء: "https://images.pexels.com/photos/5591144/pexels-photo-5591144.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
		paint:
			"https://images.pexels.com/photos/6444365/pexels-photo-6444365.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",

		// AC related categories
		تكييف:
			"https://images.pexels.com/photos/3689532/pexels-photo-3689532.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
		مكيف: "https://images.pexels.com/photos/4489732/pexels-photo-4489732.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
		air: "https://images.pexels.com/photos/4792729/pexels-photo-4792729.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",

		// Gardening related categories
		حدائق:
			"https://images.pexels.com/photos/4503273/pexels-photo-4503273.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
		حديقة:
			"https://images.pexels.com/photos/4503751/pexels-photo-4503751.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
		garden:
			"https://images.pexels.com/photos/4505168/pexels-photo-4505168.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
	};

	// Check if any keyword matches
	for (const [keyword, imagePath] of Object.entries(imageMap)) {
		if (lowerName.includes(keyword)) {
			return imagePath;
		}
	}

	// If no match by name, use ID to provide some variation or fallback to default
	const defaultImages = [
		"https://images.pexels.com/photos/4107120/pexels-photo-4107120.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1", // cleaning
		"https://images.pexels.com/photos/8005397/pexels-photo-8005397.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1", // electrical
		"https://images.pexels.com/photos/6419128/pexels-photo-6419128.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1", // plumbing
		"https://images.pexels.com/photos/6419123/pexels-photo-6419123.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1", // carpentry
		"https://images.pexels.com/photos/6598/coffee-desk-notes-workspace.jpg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1", // painting
		"https://images.pexels.com/photos/3689532/pexels-photo-3689532.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1", // AC
		"https://images.pexels.com/photos/4503273/pexels-photo-4503273.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1", // gardening
	];

	// Use ID to pick an image, or fallback to first image if we can't determine one
	if (typeof id === "number" && defaultImages.length > 0) {
		const index = id % defaultImages.length;
		return defaultImages[index];
	}

	// Final fallback to default image
	return "https://images.pexels.com/photos/3747463/pexels-photo-3747463.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1";
};
