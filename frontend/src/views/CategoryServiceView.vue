<!-- CategoryServicesView.vue -->
<script setup>
	import { ref, onMounted, watch } from "vue";
	import { useRoute, useRouter } from "vue-router";
	import axios from "axios";
	import { extractArrayFromResponse } from "../utils/apiUtils";

	const route = useRoute();
	const router = useRouter();
	const categoryId = route.params.id; // Get the category ID from the route parameters
	const services = ref([]);
	const subCategories = ref([]);
	const categoryName = ref("");
	const selectedSubCategory = ref(null);
	const servicesBySubCategory = ref([]);
	const isLoading = ref(false);
	const showServicesSection = ref(false);
	const fadeOutServices = ref(false);
	const previousSubCategoryId = ref(null);
	const isFolded = ref(true);
	const searchQuery = ref("");
	const filteredServices = ref([]);

	// Filter services based on search query
	watch(searchQuery, () => {
		if (!servicesBySubCategory.value.length) return;

		if (!searchQuery.value.trim()) {
			filteredServices.value = servicesBySubCategory.value;
			return;
		}

		const query = searchQuery.value.toLowerCase().trim();
		filteredServices.value = servicesBySubCategory.value.filter((service) => {
			return (
				service.title?.toLowerCase().includes(query) ||
				service.description?.toLowerCase().includes(query)
			);
		});
	});

	// Fetch subcategories for the category
	const fetchSubCategories = async () => {
		try {
			const response = await axios.get(
				`/api/subcategory/bycategory/${categoryId}`
			);
			// Use the utility function to handle different response formats
			subCategories.value = extractArrayFromResponse(
				response.data,
				"subcategories"
			);

			// Get category name if first item has categoryName
			if (
				subCategories.value.length > 0 &&
				subCategories.value[0].categoryName
			) {
				categoryName.value = subCategories.value[0].categoryName;
			}
		} catch (error) {
			console.error("Error fetching subcategories:", error);
			subCategories.value = [];
		}
	};

	// Fetch services by subcategory ID
	const fetchServicesBySubCategory = async (subCategoryId) => {
		// If same subcategory clicked, toggle fold/unfold
		if (
			previousSubCategoryId.value === subCategoryId &&
			showServicesSection.value
		) {
			toggleFold();
			return;
		}

		// Store current subcategory for comparison
		previousSubCategoryId.value = subCategoryId;

		// If services are already showing for a different subcategory, fade them out
		if (showServicesSection.value) {
			fadeOutServices.value = true;

			// Wait for fade-out animation to complete
			await new Promise((resolve) => setTimeout(resolve, 300));
		}

		// Hide services section during loading
		showServicesSection.value = false;
		fadeOutServices.value = false;
		isFolded.value = false;
		searchQuery.value = ""; // Reset search query for new subcategory

		// Reset to top of page smoothly if not visible in viewport
		const subCategoryEl = document.querySelector(
			`.subcategory-${subCategoryId}`
		);
		if (subCategoryEl) {
			const rect = subCategoryEl.getBoundingClientRect();
			if (rect.top < 0 || rect.bottom > window.innerHeight) {
				subCategoryEl.scrollIntoView({ behavior: "smooth", block: "center" });
			}
		}

		isLoading.value = true;
		selectedSubCategory.value = subCategories.value.find(
			(sub) => sub.id === subCategoryId
		);

		try {
			const response = await axios.get(
				`/api/service/bySubCategory/${subCategoryId}`
			);
			// Use the utility function to handle different response formats
			servicesBySubCategory.value = extractArrayFromResponse(
				response.data,
				"services"
			);

			// Set filtered services initially to all services
			filteredServices.value = servicesBySubCategory.value;

			// Small delay to ensure animation feels natural
			setTimeout(() => {
				showServicesSection.value = true;
				isLoading.value = false;
			}, 300);
		} catch (error) {
			console.error(
				`Error fetching services for subcategory ID ${subCategoryId}:`,
				error
			);
			servicesBySubCategory.value = [];
			filteredServices.value = [];

			// Show empty state with animation too
			setTimeout(() => {
				showServicesSection.value = true;
				isLoading.value = false;
			}, 300);
		}
	};

	const toggleFold = () => {
		if (isFolded.value) {
			// Unfold animation
			showServicesSection.value = true;
			isFolded.value = false;
		} else {
			// Fold animation
			fadeOutServices.value = true;

			// After animation completes, hide the section
			setTimeout(() => {
				showServicesSection.value = false;
				fadeOutServices.value = false;
				isFolded.value = true;
			}, 300);
		}
	};

	const navigateToService = (serviceId) => {
		router.push(`/service/${serviceId}`);
	};

	const goBack = () => {
		router.back();
	};

	onMounted(() => {
		fetchSubCategories();
	});

	// Function to get appropriate image based on service title/category
	const getServiceImage = (service) => {
		const title = service.title?.toLowerCase() || "";
		const category = service.category?.toLowerCase() || "";

		// Cleaning services
		if (
			title.includes("تنظيف") ||
			title.includes("نظافة") ||
			category.includes("تنظيف") ||
			category.includes("نظافة")
		) {
			return "https://images.pexels.com/photos/4239091/pexels-photo-4239091.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1";
		}

		// Electrical services
		if (
			title.includes("كهرباء") ||
			title.includes("كهربائية") ||
			category.includes("كهرباء")
		) {
			return "https://images.pexels.com/photos/4491881/pexels-photo-4491881.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1";
		}

		// Plumbing services
		if (
			title.includes("سباكة") ||
			title.includes("مياه") ||
			category.includes("سباكة")
		) {
			return "https://images.pexels.com/photos/6419146/pexels-photo-6419146.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1";
		}

		// Carpentry services
		if (
			title.includes("نجارة") ||
			title.includes("خشب") ||
			category.includes("نجارة")
		) {
			return "https://images.pexels.com/photos/6969866/pexels-photo-6969866.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1";
		}

		// Painting services
		if (
			title.includes("دهان") ||
			title.includes("طلاء") ||
			category.includes("دهان")
		) {
			return "https://images.pexels.com/photos/5591144/pexels-photo-5591144.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1";
		}

		// AC services
		if (
			title.includes("تكييف") ||
			title.includes("مكيف") ||
			category.includes("تكييف")
		) {
			return "https://images.pexels.com/photos/4489732/pexels-photo-4489732.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1";
		}

		// Gardening services
		if (
			title.includes("حدائق") ||
			title.includes("حديقة") ||
			category.includes("حدائق")
		) {
			return "https://images.pexels.com/photos/4503751/pexels-photo-4503751.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1";
		}

		// Moving services
		if (
			title.includes("نقل") ||
			title.includes("تحريك") ||
			category.includes("نقل")
		) {
			return "https://images.pexels.com/photos/4246120/pexels-photo-4246120.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1";
		}

		// Default image if no match - use service ID to get some variety
		const defaultImages = [
			"https://images.pexels.com/photos/4108840/pexels-photo-4108840.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
			"https://images.pexels.com/photos/4108718/pexels-photo-4108718.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
			"https://images.pexels.com/photos/4108765/pexels-photo-4108765.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
			"https://images.pexels.com/photos/3747463/pexels-photo-3747463.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
		];

		// Use service ID to select an image, or fallback to first image
		if (service.id && defaultImages.length > 0) {
			const index = service.id % defaultImages.length;
			return defaultImages[index];
		}

		return defaultImages[0];
	};
</script>

<template>
	<div class="container mx-auto px-4 py-6">
		<!-- Back button and heading -->
		<div class="flex items-center justify-between mb-6 rtl">
			<button
				@click="goBack"
				class="px-4 py-2 rounded-lg flex items-center gap-2 bg-blue-600 text-white hover:bg-blue-700 transition-all duration-300 shadow-sm"
			>
				<i class="fas fa-arrow-left rtl:rotate-180"></i>
				<span>العودة</span>
			</button>

			<h1
				class="text-2xl font-bold text-primary drop-shadow-sm"
				style="text-shadow: 0 1px 1px rgba(0, 0, 0, 0.1)"
			>
				{{ categoryName || "الخدمات المتوفرة" }}
			</h1>
		</div>

		<!-- SubCategories Section -->
		<section class="mb-10">
			<div class="flex items-center justify-between mb-5 rtl">
				<h2 class="text-xl font-bold text-primary">التصنيفات الفرعية</h2>
				<span
					class="bg-primary/10 text-primary px-3 py-1 rounded-full text-sm font-bold"
					>{{ subCategories.length }}</span
				>
			</div>

			<!-- SubCategory Cards -->
			<div class="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-4 rtl">
				<div
					v-for="(subCategory, index) in subCategories"
					:key="subCategory.id"
					@click="fetchServicesBySubCategory(subCategory.id)"
					class="category-card rounded-lg shadow-md overflow-hidden cursor-pointer border transition-all duration-200 hover:shadow-lg"
					:class="{
						'border-blue-500 ring-2 ring-blue-200':
							selectedSubCategory && selectedSubCategory.id === subCategory.id,
						'border-gray-200':
							!selectedSubCategory || selectedSubCategory.id !== subCategory.id,
						[`subcategory-${subCategory.id}`]: true,
					}"
				>
					<!-- SubCategory Image -->
					<div class="relative h-28">
						<img
							src="@/img/cleaning.jpg"
							:alt="subCategory.name"
							class="w-full h-full object-cover"
						/>
						<div
							class="absolute inset-0 bg-gradient-to-t from-blue-900/95 via-blue-800/70 to-transparent"
						></div>

						<!-- SubCategory Name -->
						<div class="absolute bottom-0 left-0 right-0 p-3">
							<h3
								class="text-white font-bold text-lg drop-shadow-lg"
								style="text-shadow: 0 2px 4px rgba(0, 0, 0, 0.5)"
							>
								{{ subCategory.name }}
							</h3>
						</div>

						<!-- Selection indicator -->
						<div
							v-if="
								selectedSubCategory && selectedSubCategory.id === subCategory.id
							"
							class="absolute top-2 left-2 bg-blue-500 h-6 w-6 rounded-full flex items-center justify-center"
						>
							<i class="fas fa-check text-white text-xs"></i>
						</div>
					</div>
				</div>
			</div>

			<div
				v-if="subCategories.length === 0"
				class="text-center py-8 bg-gray-50 rounded-lg border mt-4"
			>
				<p class="text-gray-600">لا توجد تصنيفات فرعية متاحة حالياً</p>
			</div>
		</section>

		<!-- Selected SubCategory Services Section -->
		<section
			v-if="selectedSubCategory"
			class="services-section"
			:class="{
				'show-services': showServicesSection,
				'fade-out-services': fadeOutServices,
			}"
		>
			<!-- Services header with search -->
			<div
				class="bg-white rounded-lg shadow-sm border border-gray-200 p-4 mb-6"
			>
				<div
					class="flex flex-col sm:flex-row gap-4 items-start sm:items-center justify-between rtl"
				>
					<div class="flex items-center gap-2">
						<i class="fas fa-layer-group text-primary"></i>
						<div>
							<h2 class="text-xl font-bold text-primary">
								{{ selectedSubCategory.name }}
							</h2>
							<p class="text-color-medium text-sm">
								{{ servicesBySubCategory.length }} خدمة متاحة
							</p>
						</div>
					</div>

					<!-- Search Bar -->
					<div class="relative w-full sm:w-auto">
						<input
							type="text"
							v-model="searchQuery"
							class="w-full sm:w-60 bg-gray-50 border border-gray-300 text-gray-900 rounded-lg pl-10 pr-4 py-2 focus:ring-blue-500 focus:border-blue-500"
							placeholder="ابحث عن خدمة..."
						/>
						<div
							class="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none"
						>
							<i class="fas fa-search text-gray-400"></i>
						</div>
					</div>
				</div>
			</div>

			<!-- Loading indicator -->
			<div v-if="isLoading" class="flex justify-center py-8">
				<div class="spinner"></div>
			</div>

			<!-- Service Cards -->
			<div
				v-else
				class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-4 rtl"
			>
				<div
					v-for="(service, index) in filteredServices"
					:key="service.id"
					@click="navigateToService(service.id)"
					class="service-card rounded-lg shadow-md overflow-hidden cursor-pointer border border-gray-200 bg-white hover:shadow-lg transition-all duration-300 hover:-translate-y-1"
				>
					<!-- Service Image -->
					<div class="relative h-36">
						<img
							:src="getServiceImage(service)"
							:alt="service.title"
							class="w-full h-full object-cover"
						/>

						<!-- Price badge if available -->
						<div
							v-if="service.estimatedPrice"
							class="absolute bottom-2 left-2 bg-green-500 text-white px-2 py-1 rounded-md text-sm font-medium"
						>
							{{ service.estimatedPrice }} IQD
						</div>

						<!-- Rating badge -->
						<div
							v-if="service.averageRating > 0"
							class="absolute top-2 left-2 bg-blue-600/90 backdrop-blur-sm text-white px-2 py-1 rounded-full text-xs font-semibold inline-flex items-center gap-1 shadow-sm"
						>
							<span class="font-bold">{{
								service.averageRating?.toFixed(1)
							}}</span>
							<i class="fas fa-star text-yellow-300 text-xs"></i>
							<span class="text-xs text-white/80"
								>({{ service.ratingCount || 0 }})</span
							>
						</div>
					</div>

					<!-- Service Content -->
					<div class="p-4">
						<h3
							class="text-lg font-bold text-primary mb-2 line-clamp-2 hover:text-primary-dark transition-colors"
							style="text-shadow: 0 0.5px 0 rgba(0, 0, 0, 0.05)"
						>
							{{ service.title }}
						</h3>

						<div class="flex items-center justify-between mb-2">
							<div class="text-sm text-color-medium flex items-center">
								<i class="fas fa-user text-primary mr-2"></i>
								{{
									service.providerFirstName || service.providerLastName
										? `${service.providerFirstName} ${service.providerLastName}`.trim()
										: service.providerUsername
								}}
							</div>
						</div>

						<p
							v-if="service.description"
							class="text-color-dark text-sm mb-3 line-clamp-2 font-medium"
						>
							{{ service.description }}
						</p>

						<button
							class="w-full py-2 bg-blue-600 hover:bg-blue-700 text-white rounded-md transition-colors flex items-center justify-center gap-2"
						>
							<span>عرض التفاصيل</span>
							<i class="fas fa-arrow-left text-xs"></i>
						</button>
					</div>
				</div>

				<!-- Empty search results -->
				<div
					v-if="
						searchQuery &&
						filteredServices.length === 0 &&
						servicesBySubCategory.length > 0
					"
					class="col-span-full text-center py-8 bg-gray-50 rounded-lg border"
				>
					<i class="fas fa-search text-blue-500 text-2xl mb-2"></i>
					<p class="text-gray-600">
						لم يتم العثور على نتائج لـ "{{ searchQuery }}"
					</p>
				</div>

				<!-- No services -->
				<div
					v-if="servicesBySubCategory.length === 0"
					class="col-span-full text-center py-8 bg-gray-50 rounded-lg border"
				>
					<i class="fas fa-info-circle text-blue-500 text-2xl mb-2"></i>
					<p class="text-gray-600">لا توجد خدمات متاحة في هذا التصنيف حالياً</p>
				</div>
			</div>
		</section>

		<!-- Back to top button (only show when scrolled down) -->
		<button
			@click="window.scrollTo({ top: 0, behavior: 'smooth' })"
			class="fixed bottom-4 right-4 z-30 bg-blue-600 text-white w-10 h-10 rounded-full shadow flex items-center justify-center hover:bg-blue-700"
		>
			<i class="fas fa-arrow-up"></i>
		</button>
	</div>
</template>

<style scoped>
	/* Basic styles for cards */
	.category-card:hover {
		transform: translateY(-2px);
	}

	.service-card:hover {
		transform: translateY(-2px);
	}

	/* Services section animations - simplified */
	.services-section {
		opacity: 0;
		transition: opacity 0.3s ease;
	}

	.services-section.show-services {
		opacity: 1;
	}

	.services-section.fade-out-services {
		opacity: 0;
	}

	/* Spinner */
	.spinner {
		width: 40px;
		height: 40px;
		border-radius: 50%;
		border: 3px solid rgba(59, 130, 246, 0.1);
		border-top-color: #3b82f6;
		animation: spin 1s linear infinite;
	}

	@keyframes spin {
		to {
			transform: rotate(360deg);
		}
	}

	/* Text truncation */
	.line-clamp-2 {
		display: -webkit-box;
		-webkit-line-clamp: 2;
		-webkit-box-orient: vertical;
		overflow: hidden;
	}

	.drop-shadow-lg {
		filter: drop-shadow(0 4px 3px rgb(0 0 0 / 0.3));
	}

	.rtl {
		direction: rtl;
		text-align: right;
	}

	@keyframes fadeIn {
		from {
			opacity: 0;
			transform: translateY(10px);
		}
		to {
			opacity: 1;
			transform: translateY(0);
		}
	}

	.animate-fadeIn {
		animation: fadeIn 0.8s ease-out;
	}

	/* Color variables */
	.text-primary {
		color: var(--color-primary) !important;
	}

	.text-primary-dark {
		color: var(--color-primary-dark) !important;
	}

	.text-color-medium {
		color: var(--color-gray-600) !important;
	}

	.text-color-dark {
		color: var(--color-gray-800) !important;
	}

	/* Make subcategory and service titles white */
	h3 {
		color: white !important;
		font-weight: 700;
		text-shadow: 0 2px 4px rgba(0, 0, 0, 0.7);
	}
</style>
