/**
 * Constants for image URLs
 */
const DEFAULT_IMAGE =
	"https://images.pexels.com/photos/3747463/pexels-photo-3747463.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1";

// Map of keywords to image URLs from Pexels
const CATEGORY_IMAGE_MAP = {
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

// Fallback images to use if no keyword matches
const DEFAULT_IMAGES = [
	"https://images.pexels.com/photos/4107120/pexels-photo-4107120.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
	"https://images.pexels.com/photos/8005397/pexels-photo-8005397.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
	"https://images.pexels.com/photos/6419128/pexels-photo-6419128.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
	"https://images.pexels.com/photos/6419123/pexels-photo-6419123.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
	"https://images.pexels.com/photos/6598/coffee-desk-notes-workspace.jpg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
	"https://images.pexels.com/photos/3689532/pexels-photo-3689532.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
	"https://images.pexels.com/photos/4503273/pexels-photo-4503273.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
];

/**
 * Helper function to get an appropriate Pexels image based on category name
 * @param {string} name - The category name
 * @param {number|string} id - The category ID (used for fallback)
 * @returns {string} - Image URL
 */
const getCategoryImageByName = (name, id = 0) => {
	if (!name) return DEFAULT_IMAGE;

	const lowerName = name.toLowerCase();

	// Check if any keyword matches
	for (const [keyword, imagePath] of Object.entries(CATEGORY_IMAGE_MAP)) {
		if (lowerName.includes(keyword)) {
			return imagePath;
		}
	}

	// Use ID to pick a fallback image
	if (DEFAULT_IMAGES.length > 0) {
		const numericId = typeof id === "string" ? parseInt(id, 10) || 0 : id;
		const index = Math.abs(numericId) % DEFAULT_IMAGES.length;
		return DEFAULT_IMAGES[index];
	}

	return DEFAULT_IMAGE;
};

/**
 * Gets an appropriate image URL for a category
 * @param {Object} category - The category object
 * @returns {string} - The image URL to use
 */
export const getCategoryImage = (category) => {
	if (!category) return DEFAULT_IMAGE;

	// Extract data from category with property normalization
	const imageUrl = category.imageUrl || category.ImageUrl;
	const name = category.name || category.Name || "";
	const id = category.id || category.Id || 0;

	// If the category has an image URL that's a full URL, use it
	if (imageUrl && imageUrl.startsWith("http")) {
		return imageUrl;
	}

	// Otherwise, get an image based on category name and ID
	return getCategoryImageByName(name, id);
};
