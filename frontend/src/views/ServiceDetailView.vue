<script setup>
	import { ref, onMounted, computed, onUnmounted } from "vue";
	import { useRoute, useRouter } from "vue-router";
	import ServiceService from "@/services/ServiceService";
	import { useToastStore } from "@/store/toastStore";
	import { useUserStore } from "@/store/userStore";
	import axios from "axios";

	const service = ref(null);
	const loading = ref(true);
	const error = ref(null);
	const comments = ref([]);
	const newComment = ref("");
	const similarServices = ref([]);
	const activeImageIndex = ref(0);
	const userRating = ref(0);
	const bookingDate = ref("");
	const bookingTime = ref("");
	const bookingNotes = ref("");
	const showBookingForm = ref(false);
	const processingBooking = ref(false);
	const galleryImages = ref([]);
	const loadingImages = ref(true);
	const submittingComment = ref(false);
	const loadingComments = ref(false);
	// Lightbox states
	const showLightbox = ref(false);
	const lightboxIndex = ref(0);

	const route = useRoute();
	const router = useRouter();
	const toast = useToastStore();
	const userStore = useUserStore();

	// Computed properties
	const isLoggedIn = computed(() => userStore.isLoggedIn);
	const canBook = computed(() => isLoggedIn.value && service.value?.isApproved);
	const formattedPrice = computed(() => {
		if (!service.value) return "";
		return new Intl.NumberFormat("ar-IQ").format(service.value.estimatedPrice);
	});

	// Computed property for average rating
	const averageRating = computed(() => {
		// If the API provides the average rating directly, use it
		if (service.value && service.value.averageRating !== undefined) {
			return service.value.averageRating;
		}

		// Otherwise calculate from comments
		if (!comments.value || comments.value.length === 0) return 0;

		const validRatings = comments.value
			.filter((comment) => comment.rating > 0)
			.map((comment) => comment.rating);

		if (validRatings.length === 0) return 0;

		const sum = validRatings.reduce((acc, rating) => acc + rating, 0);
		return Math.round((sum / validRatings.length) * 10) / 10; // Round to 1 decimal place
	});

	// Computed property for formatted average rating display
	const formattedAvgRating = computed(() => {
		return averageRating.value.toFixed(1);
	});

	// Computed property for rating count
	const ratingCount = computed(() => {
		// If the API provides the rating count directly, use it
		if (service.value && service.value.ratingCount !== undefined) {
			return service.value.ratingCount;
		}

		// Otherwise count from comments
		return comments.value.filter((comment) => comment.rating > 0).length;
	});

	// Service images based on type
	const serviceTypeImages = {
		cleaning: [
			"https://images.pexels.com/photos/4107120/pexels-photo-4107120.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
			"https://images.pexels.com/photos/4239091/pexels-photo-4239091.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
			"https://images.pexels.com/photos/5591579/pexels-photo-5591579.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
			"https://images.pexels.com/photos/4108715/pexels-photo-4108715.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
		],
		repair: [
			"https://images.pexels.com/photos/8005397/pexels-photo-8005397.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
			"https://images.pexels.com/photos/4491881/pexels-photo-4491881.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
			"https://images.pexels.com/photos/257886/pexels-photo-257886.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
			"https://images.pexels.com/photos/8005398/pexels-photo-8005398.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
		],
		gardening: [
			"https://images.pexels.com/photos/4503273/pexels-photo-4503273.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
			"https://images.pexels.com/photos/4503751/pexels-photo-4503751.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
			"https://images.pexels.com/photos/4505168/pexels-photo-4505168.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
			"https://images.pexels.com/photos/4505477/pexels-photo-4505477.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
		],
		moving: [
			"https://images.pexels.com/photos/4246091/pexels-photo-4246091.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
			"https://images.pexels.com/photos/4246120/pexels-photo-4246120.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
			"https://images.pexels.com/photos/4246101/pexels-photo-4246101.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
			"https://images.pexels.com/photos/4246254/pexels-photo-4246254.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
		],
		plumbing: [
			"https://images.pexels.com/photos/6419128/pexels-photo-6419128.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
			"https://images.pexels.com/photos/6419146/pexels-photo-6419146.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
			"https://images.pexels.com/photos/5591630/pexels-photo-5591630.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
			"https://images.pexels.com/photos/6419135/pexels-photo-6419135.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
		],
		painting: [
			"https://images.pexels.com/photos/6598/coffee-desk-notes-workspace.jpg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
			"https://images.pexels.com/photos/5591144/pexels-photo-5591144.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
			"https://images.pexels.com/photos/6444365/pexels-photo-6444365.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
			"https://images.pexels.com/photos/6444475/pexels-photo-6444475.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
		],
		carpentry: [
			"https://images.pexels.com/photos/6419123/pexels-photo-6419123.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
			"https://images.pexels.com/photos/6969866/pexels-photo-6969866.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
			"https://images.pexels.com/photos/6969831/pexels-photo-6969831.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
			"https://images.pexels.com/photos/6969839/pexels-photo-6969839.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
		],
		default: [
			"https://images.pexels.com/photos/3747463/pexels-photo-3747463.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
			"https://images.pexels.com/photos/4108840/pexels-photo-4108840.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
			"https://images.pexels.com/photos/4108718/pexels-photo-4108718.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
			"https://images.pexels.com/photos/4108765/pexels-photo-4108765.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
		],
	};

	onMounted(async () => {
		console.log("ServiceDetailView mounted");
		console.log("Route params:", route.params);

		try {
			const serviceId = parseInt(route.params.id);
			console.log("Service ID:", serviceId);

			if (isNaN(serviceId)) {
				console.error("Invalid service ID");
				error.value = "معرف الخدمة غير صالح";
				loading.value = false;
				return;
			}

			console.log("Fetching service details...");
			const response = await ServiceService.getServiceById(serviceId);
			console.log("Service data received:", response);

			// Handle different response formats
			if (response && typeof response === "object") {
				// If the response has a nested structure with $values
				if (response.$values) {
					service.value = response.$values[0];
				}
				// If the response is directly the service object
				else {
					service.value = response;
				}
			} else {
				throw new Error("Unexpected response format");
			}

			console.log("Service value set:", service.value);

			// Apply ratings from API directly if available
			if (service.value.averageRating !== undefined) {
				console.log("Using API-provided rating:", service.value.averageRating);
			}

			// Normalize subCategories to always be an array
			if (!Array.isArray(service.value.subCategories)) {
				// Try to extract from possible alternative properties
				if (
					service.value.subcategories &&
					Array.isArray(service.value.subcategories)
				) {
					service.value.subCategories = service.value.subcategories;
				} else if (
					service.value.SubCategories &&
					Array.isArray(service.value.SubCategories)
				) {
					service.value.subCategories = service.value.SubCategories;
				} else {
					service.value.subCategories = [];
				}
			}

			// Set gallery images based on service title/category
			loadServiceImages();

			loading.value = false;

			// Fetch comments for this service
			await fetchComments();

			// Fetch similar services based on category
			await fetchSimilarServices();

			// Add keyboard event listener for lightbox
			window.addEventListener("keydown", handleKeydown);
		} catch (err) {
			console.error("Error loading service:", err);
			error.value = err.message || "حدث خطأ أثناء تحميل تفاصيل الخدمة";
			loading.value = false;
			toast.error(error.value);
		}
	});

	const loadServiceImages = () => {
		loadingImages.value = true;

		// Determine which image set to use based on service title keywords
		const serviceTitle = service.value?.title?.toLowerCase() || "";

		let imageSet;
		if (serviceTitle.includes("تنظيف") || serviceTitle.includes("نظافة")) {
			imageSet = serviceTypeImages.cleaning;
		} else if (
			serviceTitle.includes("إصلاح") ||
			serviceTitle.includes("صيانة")
		) {
			imageSet = serviceTypeImages.repair;
		} else if (
			serviceTitle.includes("حديقة") ||
			serviceTitle.includes("زراعة")
		) {
			imageSet = serviceTypeImages.gardening;
		} else if (serviceTitle.includes("نقل") || serviceTitle.includes("تحريك")) {
			imageSet = serviceTypeImages.moving;
		} else {
			imageSet = serviceTypeImages.default;
		}

		// Set the gallery images with proper alt text
		galleryImages.value = imageSet.map((src, index) => ({
			src,
			alt: `${service.value?.title || "خدمة"} - صورة ${index + 1}`,
		}));

		loadingImages.value = false;
	};

	const formatDate = (dateString) => {
		const date = new Date(dateString);
		return new Intl.DateTimeFormat("ar-IQ", {
			year: "numeric",
			month: "long",
			day: "numeric",
		}).format(date);
	};

	const fetchComments = async () => {
		if (!service.value || !service.value.id) return;

		loadingComments.value = true;
		try {
			const response = await axios.get(
				`/api/comment/service/${service.value.id}`
			);
			console.log("Comments response:", response.data);

			// Handle different response formats
			if (response.data && response.data.$values) {
				comments.value = response.data.$values || [];
			} else if (Array.isArray(response.data)) {
				comments.value = response.data;
			} else {
				comments.value = [];
			}

			// Add a safety check to ensure comments is always an array
			if (!Array.isArray(comments.value)) {
				console.warn("Comments data is not an array, resetting to empty array");
				comments.value = [];
			}

			console.log(`Loaded ${comments.value.length} comments`);
		} catch (err) {
			console.error("Error fetching comments:", err);
			comments.value = [];
			toast.error("تعذر تحميل التعليقات. يرجى المحاولة مرة أخرى.");
		} finally {
			loadingComments.value = false;
		}
	};

	const setRating = (rating) => {
		userRating.value = rating;
	};

	const isHovering = ref(false);
	const hoverRating = ref(0);

	const setHoverRating = (rating) => {
		hoverRating.value = rating;
		isHovering.value = true;
	};

	const clearHoverRating = () => {
		isHovering.value = false;
		hoverRating.value = 0;
	};

	const displayRating = computed(() => {
		return isHovering.value ? hoverRating.value : userRating.value;
	});

	const submitComment = async () => {
		if (!newComment.value.trim()) {
			toast.error("الرجاء إدخال تعليق");
			return;
		}

		// Check if rating is selected
		if (!userRating.value) {
			toast.error("الرجاء اختيار تقييم من 1 إلى 5");
			return;
		}

		if (!userStore.isLoggedIn) {
			toast.error("يجب تسجيل الدخول لإضافة تعليق");
			router.push("/login");
			return;
		}

		submittingComment.value = true;
		try {
			const commentData = {
				content: newComment.value,
				serviceId: service.value.id,
				rating: userRating.value,
			};

			console.log("Submitting comment:", commentData);

			const response = await axios.post("/api/comment", commentData, {
				headers: {
					Authorization: `Bearer ${userStore.token}`,
				},
			});

			console.log("Comment submission response:", response.data);

			// Clear the form
			newComment.value = "";
			userRating.value = 0;
			toast.success("تم إضافة التعليق بنجاح");

			// Wait a moment before refreshing comments to ensure the backend has processed it
			setTimeout(async () => {
				await fetchComments();
			}, 500);
		} catch (err) {
			console.error("Error posting comment:", err);
			if (err.response && err.response.data) {
				console.error("API error details:", err.response.data);
			}
			toast.error("فشل إضافة التعليق. حاول مرة أخرى.");
		} finally {
			submittingComment.value = false;
		}
	};

	const contactProvider = () => {
		// This could open a modal or redirect to a contact form
		toast.info(`رقم الاتصال: ${service.value.contactPhone}`);
	};

	const submitBooking = async () => {
		if (!bookingDate.value || !bookingTime.value) {
			toast.error("الرجاء تحديد التاريخ والوقت");
			return;
		}

		if (!userStore.isLoggedIn) {
			toast.error("يجب تسجيل الدخول لحجز الخدمة");
			router.push("/login");
			return;
		}

		processingBooking.value = true;

		try {
			// In a real app, this would make an API call to create a booking
			// For now, we'll just simulate a successful booking
			await new Promise((resolve) => setTimeout(resolve, 1000));

			toast.success("تم حجز الخدمة بنجاح");
			showBookingForm.value = false;
			bookingDate.value = "";
			bookingTime.value = "";
			bookingNotes.value = "";
		} catch (err) {
			console.error("Error booking service:", err);
			toast.error("فشل حجز الخدمة. حاول مرة أخرى.");
		} finally {
			processingBooking.value = false;
		}
	};

	const fetchSimilarServices = async () => {
		try {
			if (!service.value) {
				console.warn("Service not loaded yet, can't fetch similar services");
				return;
			}

			console.log("Fetching similar services for:", service.value.id);
			console.log(
				"Service structure:",
				JSON.stringify({
					id: service.value.id,
					title: service.value.title,
					hasSubCategories: !!service.value.subCategories,
					subCategoriesLength: service.value.subCategories?.length,
				})
			);

			// Try multiple approaches to extract the category ID
			let categoryId = null;
			let subCategoryId = null;

			// First dump the first subcategory to check its structure
			if (
				service.value.subCategories &&
				service.value.subCategories.length > 0
			) {
				console.log(
					"First subcategory structure:",
					JSON.stringify(service.value.subCategories[0])
				);
			}

			// Approach 1: Extract from subCategories array
			if (
				service.value.subCategories &&
				service.value.subCategories.length > 0
			) {
				const firstSubCategory = service.value.subCategories[0];
				if (firstSubCategory) {
					// Try different properties where categoryId might be found
					if (firstSubCategory.categoryId) {
						categoryId = firstSubCategory.categoryId;
						console.log(
							"Found categoryId from subCategories[0].categoryId:",
							categoryId
						);
					} else if (
						firstSubCategory.category &&
						firstSubCategory.category.id
					) {
						categoryId = firstSubCategory.category.id;
						console.log(
							"Found categoryId from subCategories[0].category.id:",
							categoryId
						);
					} else if (firstSubCategory.categoryName) {
						console.log("Found category name:", firstSubCategory.categoryName);
					}

					// Also try to extract subcategoryId as fallback
					if (firstSubCategory.id) {
						subCategoryId = firstSubCategory.id;
						console.log(
							"Found subCategoryId from subCategories[0].id:",
							subCategoryId
						);
					} else if (firstSubCategory.subCategoryId) {
						subCategoryId = firstSubCategory.subCategoryId;
						console.log(
							"Found subCategoryId from subCategories[0].subCategoryId:",
							subCategoryId
						);
					}
				}
			}

			// Approach 2: Try direct category properties on the service
			if (!categoryId && service.value.categoryId) {
				categoryId = service.value.categoryId;
				console.log("Found categoryId directly on service:", categoryId);
			}

			// Approach 3: Use the first approach that works
			let apiEndpoint = null;
			let apiParam = null;

			if (categoryId) {
				apiEndpoint = `/api/service/category/${categoryId}`;
				apiParam = categoryId;
				console.log(`Using category endpoint with ID ${categoryId}`);
			} else if (subCategoryId) {
				apiEndpoint = `/api/service/bySubCategory/${subCategoryId}`;
				apiParam = subCategoryId;
				console.log(`Using subcategory endpoint with ID ${subCategoryId}`);
			} else {
				// Fallback: Just get all services as a last resort
				apiEndpoint = `/api/service`;
				console.log("No category or subcategory found, fetching all services");
			}

			console.log(`Fetching from API endpoint: ${apiEndpoint}`);
			const response = await axios.get(apiEndpoint);

			// Process the response
			console.log(`Response received from ${apiEndpoint}:`, response.status);
			processRelatedServices(response);
		} catch (err) {
			console.error("Error fetching similar services:", err);
			similarServices.value = [];
		}
	};

	// Helper function to process related services response
	const processRelatedServices = (response) => {
		console.log("Similar services response:", response.data);

		// Process response based on different possible formats
		let services = [];
		if (response.data && response.data.$values) {
			services = response.data.$values;
		} else if (Array.isArray(response.data)) {
			services = response.data;
		}

		console.log(
			`Found ${services.length} services, filtering out current service (${service.value.id})`
		);

		// Filter out the current service and limit to 3 similar services
		const filteredServices = services
			.filter((s) => s.id !== service.value.id)
			.slice(0, 3);

		console.log(`Filtered to ${filteredServices.length} similar services`);

		// Map the services to our display format with ratings
		similarServices.value = filteredServices.map((s) => {
			console.log(
				`Processing similar service: ${s.id} - ${s.title}, rating: ${s.averageRating}`
			);
			return {
				id: s.id,
				title: s.title,
				description: s.description
					? s.description.length > 60
						? s.description.substring(0, 60) + "..."
						: s.description
					: "",
				estimatedPrice: s.estimatedPrice,
				averageRating: s.averageRating !== undefined ? s.averageRating : 0,
				ratingCount: s.ratingCount !== undefined ? s.ratingCount : 0,
				// Generate image URL based on service type
				imageUrl: getServiceImageUrl(s.title),
			};
		});
	};

	const getServiceImageUrl = (title = "") => {
		// Choose an appropriate image based on the service title
		const lowerTitle = title.toLowerCase();

		// Use service ID to get some variety in the images
		const getRandomImage = (imageArray) => {
			if (!imageArray || imageArray.length === 0)
				return serviceTypeImages.default[0];
			const randomIndex = Math.floor(Math.random() * imageArray.length);
			return imageArray[randomIndex];
		};

		if (lowerTitle.includes("تنظيف") || lowerTitle.includes("نظافة")) {
			return getRandomImage(serviceTypeImages.cleaning);
		} else if (
			lowerTitle.includes("إصلاح") ||
			lowerTitle.includes("صيانة") ||
			lowerTitle.includes("كهرباء") ||
			lowerTitle.includes("كهربائية")
		) {
			return getRandomImage(serviceTypeImages.repair);
		} else if (
			lowerTitle.includes("حديقة") ||
			lowerTitle.includes("زراعة") ||
			lowerTitle.includes("حدائق")
		) {
			return getRandomImage(serviceTypeImages.gardening);
		} else if (lowerTitle.includes("نقل") || lowerTitle.includes("تحريك")) {
			return getRandomImage(serviceTypeImages.moving);
		} else if (lowerTitle.includes("سباكة") || lowerTitle.includes("مياه")) {
			return getRandomImage(serviceTypeImages.plumbing);
		} else if (lowerTitle.includes("دهان") || lowerTitle.includes("طلاء")) {
			return getRandomImage(serviceTypeImages.painting);
		} else if (lowerTitle.includes("نجارة") || lowerTitle.includes("خشب")) {
			return getRandomImage(serviceTypeImages.carpentry);
		}

		return getRandomImage(serviceTypeImages.default);
	};

	const setActiveImage = (index) => {
		activeImageIndex.value = index;
	};

	// Lightbox functions
	const openLightbox = (index) => {
		lightboxIndex.value = index;
		showLightbox.value = true;
		// Prevent body scrolling when lightbox is open
		document.body.style.overflow = "hidden";
	};

	const closeLightbox = () => {
		showLightbox.value = false;
		// Restore body scrolling when lightbox is closed
		document.body.style.overflow = "";
	};

	const nextImage = () => {
		lightboxIndex.value =
			(lightboxIndex.value + 1) % galleryImages.value.length;
	};

	const prevImage = () => {
		lightboxIndex.value =
			(lightboxIndex.value - 1 + galleryImages.value.length) %
			galleryImages.value.length;
	};

	// Handle keyboard navigation in lightbox
	const handleKeydown = (e) => {
		if (!showLightbox.value) return;

		if (e.key === "Escape") {
			closeLightbox();
		} else if (e.key === "ArrowRight") {
			// In RTL interface, right arrow moves to previous
			prevImage();
		} else if (e.key === "ArrowLeft") {
			// In RTL interface, left arrow moves to next
			nextImage();
		}
	};

	// Clean up event listener when component is unmounted
	onUnmounted(() => {
		window.removeEventListener("keydown", handleKeydown);
	});
</script>

<template>
	<div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8" dir="rtl">
		<!-- Loading State -->
		<div v-if="loading" class="flex justify-center items-center h-64">
			<div class="flex flex-col items-center">
				<div
					class="w-16 h-16 border-4 border-blue-500 border-t-transparent rounded-full animate-spin"
				></div>
				<p class="mt-4 text-blue-600 font-medium">
					جاري تحميل تفاصيل الخدمة...
				</p>
			</div>
		</div>

		<!-- Error State -->
		<div
			v-else-if="error"
			class="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded relative mt-4"
			role="alert"
		>
			<strong class="font-bold">خطأ! </strong>
			<span class="block sm:inline">{{ error }}</span>
			<div class="mt-4">
				<p class="mb-2">معلومات التصحيح:</p>
				<pre class="bg-gray-100 p-2 text-xs overflow-auto">
Route params: {{ JSON.stringify(route.params) }}</pre
				>
			</div>
			<button
				class="bg-blue-500 hover:bg-blue-600 text-white px-4 py-2 rounded mt-4"
				@click="router.push('/services')"
			>
				العودة إلى الخدمات
			</button>
		</div>

		<!-- Empty Service -->
		<div
			v-else-if="!service"
			class="bg-yellow-100 border border-yellow-400 text-yellow-700 px-4 py-3 rounded relative mt-4"
			role="alert"
		>
			<strong class="font-bold">تنبيه! </strong>
			<span class="block sm:inline">لم يتم العثور على معلومات الخدمة</span>
			<button
				class="bg-blue-500 hover:bg-blue-600 text-white px-4 py-2 rounded mt-4"
				@click="router.push('/services')"
			>
				العودة إلى الخدمات
			</button>
		</div>

		<!-- Service Details -->
		<div v-else>
			<!-- Back Button and Breadcrumbs -->
			<div
				class="flex flex-col md:flex-row items-start md:items-center justify-between mb-6"
			>
				<button
					@click="router.go(-1)"
					class="px-4 py-2 rounded-lg flex items-center gap-2 bg-blue-600 text-white hover:bg-blue-700 transition-all duration-300 shadow-sm"
				>
					<i class="fas fa-arrow-left rtl:rotate-180"></i>
					<span>العودة</span>
				</button>

				<div class="flex items-center text-sm text-gray-500">
					<RouterLink to="/" class="hover:text-blue-600 transition-colors"
						>الرئيسية</RouterLink
					>
					<span class="mx-2">/</span>
					<RouterLink
						to="/services"
						class="hover:text-blue-600 transition-colors"
						>الخدمات</RouterLink
					>
					<span class="mx-2">/</span>
					<span class="text-gray-900 font-medium">{{ service.title }}</span>
				</div>
			</div>

			<div
				class="bg-white shadow-xl rounded-2xl overflow-hidden animate-fadeIn"
			>
				<!-- Service Header -->
				<div
					class="relative h-80 md:h-96 bg-gradient-to-r from-blue-600 to-blue-800 overflow-hidden rounded-t-2xl"
				>
					<img
						v-if="!loadingImages && galleryImages.length > 0"
						:src="galleryImages[activeImageIndex].src"
						:alt="galleryImages[activeImageIndex].alt"
						class="w-full h-full object-cover opacity-90 transition-opacity duration-300 cursor-pointer"
						@click="openLightbox(activeImageIndex)"
					/>
					<div v-else class="w-full h-full flex items-center justify-center">
						<div
							class="w-12 h-12 border-4 border-white border-t-transparent rounded-full animate-spin"
						></div>
					</div>

					<!-- Image overlay with gradient -->
					<div
						class="absolute inset-0 bg-gradient-to-t from-black/90 via-black/50 to-transparent flex flex-col justify-end p-8"
					>
						<!-- Rating badge - keep only this one main rating display -->
						<div class="flex items-center mb-3">
							<span
								v-if="ratingCount > 0"
								class="mr-2 bg-blue-600/90 backdrop-blur-sm text-white px-3 py-1.5 rounded-full text-sm font-semibold inline-flex items-center gap-1 shadow-lg"
							>
								<div class="flex items-center">
									<span class="font-bold">{{ formattedAvgRating }}</span>
									<i class="fas fa-star text-yellow-300 mx-1"></i>
									<span class="text-xs text-white/80">({{ ratingCount }})</span>
								</div>
							</span>
						</div>

						<h1
							class="text-4xl md:text-5xl font-bold drop-shadow-lg mb-3 animate-fadeIn"
							style="text-shadow: 0 2px 4px rgba(0, 0, 0, 0.5)"
						>
							<p
								class="text-white text-4xl md:text-5xl font-bold drop-shadow-lg mb-3 animate-fadeIn"
							>
								{{ service.title }}
							</p>
						</h1>

						<div class="flex flex-wrap items-center gap-4 text-white">
							<span
								class="bg-blue-600/80 backdrop-blur-sm px-4 py-2 rounded-lg text-lg font-semibold inline-flex items-center gap-2 shadow-md"
							>
								<i class="fas fa-tag"></i>
								{{ formattedPrice }} دينار عراقي
							</span>

							<span class="inline-flex items-center gap-1 text-white/90">
								<i class="fas fa-calendar-alt"></i>
								{{ formatDate(service.createdDate) }}
							</span>
						</div>
					</div>
				</div>

				<!-- Main content with enhanced styling -->
				<div class="p-0 md:p-8">
					<div class="flex flex-col lg:flex-row gap-8">
						<!-- Left Column - Service Details -->
						<div class="w-full lg:w-2/3 space-y-6">
							<!-- Gallery Thumbnails - already enhanced -->
							<div
								class="mb-6"
								v-if="!loadingImages && galleryImages.length > 0"
							>
								<div
									class="flex gap-2 overflow-x-auto pb-3 scrollbar-thin scrollbar-thumb-blue-500 scrollbar-track-gray-100"
								>
									<button
										v-for="(image, index) in galleryImages"
										:key="index"
										@click="setActiveImage(index)"
										class="w-24 h-24 rounded-lg overflow-hidden flex-shrink-0 border-2 transition-all duration-300 hover:opacity-90 hover:scale-105 focus:outline-none"
										:class="
											activeImageIndex === index
												? 'border-blue-500 shadow-xl ring-2 ring-blue-300 ring-offset-2'
												: 'border-transparent hover:border-blue-300'
										"
									>
										<img
											:src="image.src"
											:alt="image.alt"
											class="w-full h-full object-cover cursor-pointer"
											@click.stop="openLightbox(index)"
										/>
									</button>
								</div>
							</div>

							<!-- Service Description - already enhanced -->
							<div
								class="bg-gradient-to-br from-white to-blue-50 rounded-2xl p-6 mb-6 shadow-sm border border-blue-100/50"
							>
								<h2
									class="text-2xl font-bold text-gray-800 mb-4 flex items-center gap-2"
								>
									<i class="fas fa-info-circle text-blue-600"></i>
									وصف الخدمة
								</h2>
								<p
									class="text-gray-800 leading-relaxed text-lg whitespace-pre-line font-medium"
								>
									{{ service.description }}
								</p>

								<!-- Service Tags/Categories -->
								<div
									class="mt-6 flex flex-wrap gap-2"
									v-if="
										service.subCategories && service.subCategories.length > 0
									"
								>
									<span
										v-for="(subCategory, index) in service.subCategories"
										:key="index"
										class="bg-blue-50 text-blue-700 text-sm px-3 py-1 rounded-full border border-blue-200 inline-flex items-center gap-1"
									>
										<i class="fas fa-tag text-xs"></i>
										{{ subCategory.name || subCategory.subCategoryName }}
									</span>
								</div>
							</div>

							<!-- Booking Form - with enhanced styling -->
							<div>
								<button
									v-if="!showBookingForm && canBook"
									@click="showBookingForm = true"
									class="w-full bg-gradient-to-r from-blue-600 to-blue-500 hover:from-blue-700 hover:to-blue-600 text-white py-3 px-6 rounded-xl font-medium transition-all duration-300 flex items-center justify-center gap-2 shadow-md hover:shadow-lg"
								>
									<i class="fas fa-calendar-check"></i>
									احجز هذه الخدمة الآن
								</button>

								<div
									v-if="showBookingForm"
									class="bg-gradient-to-br from-white to-blue-50 rounded-2xl p-6 border border-blue-200 animate-fadeIn"
								>
									<h3
										class="text-xl font-bold text-gray-800 mb-4 flex items-center gap-2"
									>
										<i class="fas fa-calendar-alt text-blue-600"></i>
										حجز الخدمة
									</h3>

									<div class="grid grid-cols-1 md:grid-cols-2 gap-4 mb-4">
										<div>
											<label class="block text-gray-700 mb-2"
												>تاريخ الحجز</label
											>
											<input
												v-model="bookingDate"
												type="date"
												class="w-full p-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
											/>
										</div>
										<div>
											<label class="block text-gray-700 mb-2">وقت الحجز</label>
											<input
												v-model="bookingTime"
												type="time"
												class="w-full p-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
											/>
										</div>
									</div>

									<div class="mb-4">
										<label class="block text-gray-700 mb-2"
											>ملاحظات إضافية</label
										>
										<textarea
											v-model="bookingNotes"
											class="w-full p-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
											rows="3"
											placeholder="أي تفاصيل إضافية تود إضافتها..."
										></textarea>
									</div>

									<div class="flex gap-3">
										<button
											@click="submitBooking"
											:disabled="processingBooking"
											class="bg-blue-600 hover:bg-blue-700 text-white py-2 px-4 rounded-lg transition-colors flex items-center gap-2 disabled:opacity-70"
										>
											<i
												v-if="processingBooking"
												class="fas fa-spinner fa-spin"
											></i>
											<i v-else class="fas fa-check"></i>
											تأكيد الحجز
										</button>
										<button
											@click="showBookingForm = false"
											class="bg-gray-200 hover:bg-gray-300 text-gray-800 py-2 px-4 rounded-lg transition-colors"
										>
											إلغاء
										</button>
									</div>
								</div>
							</div>

							<!-- Comments Section -->
							<div
								class="bg-gradient-to-br from-white to-blue-50 rounded-2xl p-6 shadow-sm border border-blue-100/50"
							>
								<h2
									class="text-2xl font-bold text-gray-800 mb-4 flex items-center gap-2"
								>
									<i class="fas fa-comments text-blue-600"></i>
									التعليقات
									<span
										class="text-sm font-normal bg-blue-100 text-blue-800 px-2.5 py-1 rounded-full"
										>{{ comments.length }}</span
									>
								</h2>

								<!-- Average Rating Display -->
								<div
									v-if="ratingCount > 0"
									class="mb-6 bg-white p-5 rounded-xl border border-gray-100 shadow-sm flex flex-col md:flex-row items-center md:justify-between"
								>
									<div class="flex items-center gap-4 mb-4 md:mb-0">
										<div
											class="w-16 h-16 bg-gradient-to-br from-blue-500 to-blue-600 rounded-full flex items-center justify-center shadow-lg"
										>
											<span class="text-3xl font-bold text-white">{{
												formattedAvgRating
											}}</span>
										</div>
										<div>
											<div class="flex mb-1">
												<i
													v-for="star in 5"
													:key="star"
													class="fas fa-star text-lg"
													:class="
														star <= Math.round(averageRating)
															? 'text-yellow-400'
															: 'text-gray-200'
													"
												></i>
											</div>
											<p class="text-sm text-gray-600">
												{{ ratingCount }} تقييم
											</p>
										</div>
									</div>

									<div class="rating-bars w-full md:w-auto md:min-w-[200px]">
										<div
											v-for="rating in 5"
											:key="rating"
											class="flex items-center my-1 gap-2"
										>
											<span class="text-sm text-gray-600 w-3">{{
												6 - rating
											}}</span>
											<div
												class="w-32 bg-gray-200 rounded-full h-2.5 overflow-hidden"
											>
												<div
													class="h-full bg-yellow-400"
													:style="{
														width:
															(comments.filter((c) => c.rating === 6 - rating)
																.length /
																(ratingCount || 1)) *
																100 +
															'%',
													}"
												></div>
											</div>
											<span class="text-xs text-gray-500">
												{{
													comments.filter((c) => c.rating === 6 - rating).length
												}}
											</span>
										</div>
									</div>
								</div>

								<!-- Comments Loading State -->
								<div
									v-if="loadingComments"
									class="flex justify-center items-center py-8"
								>
									<div class="flex flex-col items-center">
										<div
											class="w-10 h-10 border-3 border-blue-500 border-t-transparent rounded-full animate-spin"
										></div>
										<p class="mt-3 text-blue-600 text-sm">
											جاري تحميل التعليقات...
										</p>
									</div>
								</div>

								<!-- Comments list -->
								<div
									v-else-if="comments && comments.length > 0"
									class="space-y-4"
								>
									<div
										v-for="(comment, index) in comments"
										:key="index"
										class="bg-white p-4 rounded-xl shadow-sm border border-gray-100 hover:border-blue-100 transition-all hover:shadow-md animate-fadeIn"
										:style="`animation-delay: ${index * 0.1}s`"
									>
										<div class="flex justify-between items-start">
											<div class="flex items-center gap-3">
												<div
													class="w-10 h-10 bg-gradient-to-br from-blue-100 to-blue-200 rounded-full flex items-center justify-center text-blue-600"
												>
													<i class="fas fa-user"></i>
												</div>
												<div>
													<div class="flex items-center gap-2">
														<p class="font-medium text-gray-800">
															{{
																comment.userFirstName || comment.userLastName
																	? `${comment.userFirstName || ""} ${
																			comment.userLastName || ""
																	  }`.trim()
																	: comment.userName || "مستخدم"
															}}
														</p>

														<!-- Comment Rating Inline -->
														<div v-if="comment.rating" class="rating-pill">
															<i
																class="fas fa-star text-yellow-400 text-xs"
															></i>
															<span>{{ comment.rating }}</span>
														</div>
													</div>
													<p class="text-xs text-gray-500 mt-0.5">
														{{ formatDate(comment.createdDate) }}
													</p>
												</div>
											</div>
										</div>
										<p class="mt-3 text-gray-700">{{ comment.content }}</p>
									</div>
								</div>

								<!-- No comments placeholder -->
								<div
									v-else
									class="bg-white p-6 rounded-xl text-gray-500 text-center border border-gray-100"
								>
									<i
										class="fas fa-comment-slash text-4xl text-gray-300 mb-2"
									></i>
									<p>لا توجد تعليقات بعد. كن أول من يعلق!</p>
								</div>

								<!-- Add Comment Form -->
								<div
									class="mt-6 bg-white p-4 rounded-xl shadow-sm border border-gray-100"
								>
									<h3
										class="text-lg font-semibold text-gray-800 mb-3 flex items-center gap-2"
									>
										<i class="fas fa-pen text-blue-600"></i>
										أضف تعليقاً
									</h3>
									<textarea
										v-model="newComment"
										class="w-full p-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
										rows="3"
										placeholder="اكتب تعليقك هنا..."
										:disabled="submittingComment"
									></textarea>
									<div
										class="mt-3 flex items-center justify-between flex-wrap gap-y-3"
									>
										<div class="flex items-center">
											<span class="text-sm text-gray-700 ml-2">
												تقييمك:
												<span class="text-red-500">*</span>
											</span>
											<div class="rating-stars" @mouseleave="clearHoverRating">
												<button
													v-for="star in 5"
													:key="star"
													@click="setRating(star)"
													@mouseenter="setHoverRating(star)"
													class="relative text-xl focus:outline-none rounded-full p-1 transition-all duration-200"
													:class="[
														displayRating >= star
															? 'text-yellow-400 transform scale-110'
															: 'text-gray-300 hover:text-yellow-300',
														star === displayRating
															? 'after:ring-2 after:ring-yellow-400 after:ring-offset-2 after:absolute after:inset-0 after:rounded-full'
															: '',
													]"
													:disabled="submittingComment"
												>
													<i class="fas fa-star"></i>
												</button>
											</div>
											<span
												class="text-sm ml-2"
												:class="userRating ? 'text-green-600' : 'text-gray-400'"
											>
												{{
													userRating
														? userRating + " نجوم"
														: "لم يتم التحديد بعد"
												}}
											</span>
										</div>
										<button
											@click="submitComment"
											:disabled="submittingComment"
											class="btn-primary px-4 py-1.5 rounded-full flex items-center gap-2 bg-gradient-to-r from-blue-600 to-blue-500 text-white hover:shadow-md transition-all duration-300 disabled:opacity-70"
										>
											<i
												v-if="submittingComment"
												class="fas fa-spinner fa-spin"
											></i>
											<i v-else class="fas fa-paper-plane"></i>
											{{ submittingComment ? "جاري الإرسال..." : "إرسال" }}
										</button>
									</div>
								</div>
							</div>
						</div>

						<!-- Right Column - Sidebar -->
						<div class="w-full lg:w-1/3 space-y-6">
							<!-- Provider Info -->
							<div
								class="bg-gradient-to-br from-white to-blue-50 rounded-2xl p-6 shadow-md border border-blue-100/50"
							>
								<h3
									class="text-xl font-bold text-gray-800 mb-4 flex items-center gap-2"
								>
									<i class="fas fa-user-tie text-blue-600"></i>
									معلومات مقدم الخدمة
								</h3>

								<div
									class="flex items-center mb-4 pb-4 border-b border-blue-100/50"
								>
									<div
										class="w-14 h-14 bg-blue-100 rounded-full flex items-center justify-center text-blue-600 ml-3 shadow-inner"
									>
										<i class="fas fa-user text-xl"></i>
									</div>
									<div>
										<p class="font-medium text-gray-800">
											{{
												service.providerFirstName || service.providerLastName
													? `${service.providerFirstName} ${service.providerLastName}`.trim()
													: service.providerUsername || "غير متوفر"
											}}
										</p>
										<p class="text-sm text-blue-600">مزود خدمة</p>
									</div>
								</div>

								<div class="space-y-4">
									<div class="flex items-center gap-3">
										<div
											class="w-10 h-10 bg-blue-50 rounded-lg flex items-center justify-center text-blue-600"
										>
											<i class="fas fa-phone-alt"></i>
										</div>
										<div>
											<p class="text-sm text-gray-500">رقم الاتصال:</p>
											<p class="font-medium text-gray-800">
												{{ service.contactPhone }}
											</p>
										</div>
									</div>

									<div class="flex items-center gap-3">
										<div
											class="w-10 h-10 bg-blue-50 rounded-lg flex items-center justify-center text-blue-600"
										>
											<i class="fas fa-calendar-alt"></i>
										</div>
										<div>
											<p class="text-sm text-gray-500">تاريخ النشر:</p>
											<p class="font-medium text-gray-800">
												{{ formatDate(service.createdDate) }}
											</p>
										</div>
									</div>
								</div>

								<button
									@click="contactProvider"
									class="w-full mt-6 bg-blue-600 hover:bg-blue-700 text-white px-4 py-3 rounded-lg font-medium transition-colors flex items-center justify-center gap-2 shadow-sm hover:shadow-md"
								>
									<i class="fas fa-phone-alt"></i>
									اتصل بمقدم الخدمة
								</button>
							</div>

							<!-- Similar Services - Enhanced visual appearance -->
							<div
								v-if="similarServices && similarServices.length > 0"
								class="bg-gradient-to-br from-white to-blue-50 rounded-2xl p-6 shadow-md border border-blue-100/50"
							>
								<h3
									class="text-xl font-bold text-gray-800 mb-4 flex items-center gap-2"
								>
									<i class="fas fa-th-list text-blue-600"></i>
									خدمات مشابهة
								</h3>

								<div class="space-y-3">
									<div
										v-for="(similar, index) in similarServices"
										:key="index"
										class="flex items-start bg-white p-3 rounded-xl transition-all duration-300 hover:shadow-md group border border-gray-100 hover:border-blue-200"
									>
										<div class="w-20 h-20 rounded-lg overflow-hidden">
											<img
												:src="similar.imageUrl"
												:alt="similar.title"
												class="w-full h-full object-cover group-hover:scale-110 transition-transform duration-300"
											/>
										</div>
										<div class="mr-3 flex-grow">
											<router-link
												:to="`/service/${similar.id}`"
												class="font-medium text-gray-800 hover:text-blue-600 transition-colors block group-hover:text-blue-600"
											>
												{{ similar.title }}
											</router-link>

											<!-- Description Preview -->
											<p
												v-if="similar.description"
												class="text-xs text-gray-500 mt-0.5 mb-1 line-clamp-2"
											>
												{{ similar.description }}
											</p>

											<div class="flex justify-between items-center mt-1">
												<p class="text-sm text-blue-600 font-semibold">
													{{
														new Intl.NumberFormat("ar-IQ").format(
															similar.estimatedPrice
														)
													}}
													دينار عراقي
												</p>
												<div
													class="flex items-center bg-yellow-50 px-2 py-0.5 rounded-full"
													:class="{ 'opacity-50': !similar.ratingCount }"
												>
													<span class="text-yellow-600 text-xs font-bold mr-1">
														{{
															similar.ratingCount
																? similar.averageRating.toFixed(1)
																: "-"
														}}
													</span>
													<i class="fas fa-star text-yellow-500 text-xs"></i>
													<span class="text-xs text-gray-500 mr-0.5"
														>({{ similar.ratingCount || 0 }})</span
													>
												</div>
											</div>
											<div class="mt-1">
												<button
													class="text-xs text-blue-600 hover:text-blue-700 font-medium rounded-full px-2 py-0.5 border border-blue-100 hover:bg-blue-50 transition-colors"
													@click="router.push(`/service/${similar.id}`)"
												>
													عرض التفاصيل
													<i class="fas fa-chevron-left ml-1 text-xs"></i>
												</button>
											</div>
										</div>
									</div>
								</div>
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>

		<!-- Lightbox Modal -->
		<div
			v-if="showLightbox"
			class="fixed inset-0 z-50 bg-black/90 flex items-center justify-center transition-opacity duration-300"
			@click="closeLightbox"
		>
			<div class="relative w-full h-full flex items-center justify-center p-4">
				<!-- Close button -->
				<button
					class="absolute top-4 right-4 z-10 text-white hover:text-gray-300 transition-colors"
					@click="closeLightbox"
				>
					<i class="fas fa-times text-2xl"></i>
				</button>

				<!-- Previous button -->
				<button
					class="absolute right-4 top-1/2 -translate-y-1/2 z-10 bg-black/50 hover:bg-black/70 text-white p-3 rounded-full transition-all transform hover:scale-110"
					@click.stop="prevImage"
				>
					<i class="fas fa-chevron-right"></i>
				</button>

				<!-- Next button -->
				<button
					class="absolute left-4 top-1/2 -translate-y-1/2 z-10 bg-black/50 hover:bg-black/70 text-white p-3 rounded-full transition-all transform hover:scale-110"
					@click.stop="nextImage"
				>
					<i class="fas fa-chevron-left"></i>
				</button>

				<!-- Image counter -->
				<div
					class="absolute bottom-6 left-1/2 -translate-x-1/2 bg-black/50 text-white px-4 py-2 rounded-full text-sm"
				>
					{{ lightboxIndex + 1 }} / {{ galleryImages.length }}
				</div>

				<!-- Image container -->
				<div class="max-w-5xl max-h-[80vh] relative" @click.stop="">
					<img
						v-if="galleryImages[lightboxIndex]"
						:src="galleryImages[lightboxIndex].src"
						:alt="galleryImages[lightboxIndex].alt"
						class="max-w-full max-h-[80vh] object-contain lightbox-image"
					/>
				</div>
			</div>
		</div>
	</div>
</template>

<style scoped>
	@keyframes fadeIn {
		from {
			opacity: 0;
			transform: translateY(15px);
		}
		to {
			opacity: 1;
			transform: translateY(0);
		}
	}

	.animate-fadeIn {
		animation: fadeIn 0.5s cubic-bezier(0.34, 1.56, 0.64, 1);
	}

	/* Rating stars styling */
	.rating-stars {
		display: flex;
		align-items: center;
		gap: 0.15rem;
	}

	.rating-stars button {
		outline: none !important;
		position: relative;
	}

	.rating-display {
		background: linear-gradient(
			to right,
			rgba(249, 250, 251, 0.5),
			rgba(243, 244, 246, 0.5)
		);
		padding: 0.25rem 0.5rem;
		border-radius: 1rem;
		display: flex;
		align-items: center;
		gap: 0.15rem;
	}

	.rating-pill {
		display: inline-flex;
		align-items: center;
		background: linear-gradient(to right, #fde68a, #fcd34d);
		padding: 0.15rem 0.4rem;
		border-radius: 1rem;
		font-size: 0.75rem;
		font-weight: 600;
		color: #92400e;
		gap: 2px;
	}

	.rating-bars {
		display: flex;
		flex-direction: column;
	}

	/* Animation delay utility classes */
	.animation-delay-100 {
		animation-delay: 0.1s;
	}

	.animation-delay-200 {
		animation-delay: 0.2s;
	}

	.animation-delay-300 {
		animation-delay: 0.3s;
	}

	/* Lightbox animation */
	.lightbox-image {
		animation: zoom-in 0.3s ease-out;
	}

	@keyframes zoom-in {
		from {
			opacity: 0;
			transform: scale(0.95);
		}
		to {
			opacity: 1;
			transform: scale(1);
		}
	}
</style>
