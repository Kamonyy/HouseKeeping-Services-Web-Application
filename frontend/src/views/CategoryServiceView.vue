<!-- CategoryServicesView.vue -->
<script setup>
	import { ref, onMounted } from "vue";
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

	onMounted(() => {
		fetchSubCategories();
	});
</script>

<template>
	<div class="container mx-auto px-4 py-8 animate-fadeIn">
		<!-- Header Section -->
		<header class="text-center mb-8 relative">
			<div class="static-glow"></div>
			<h1
				class="text-3xl font-extrabold text-transparent bg-clip-text bg-gradient-to-r from-blue-700 to-blue-500 mb-4"
			>
				{{ categoryName || "الخدمات المتوفرة" }}
			</h1>
			<p class="text-gray-600 max-w-3xl mx-auto">
				اختر الخدمة الفرعية التي تناسب احتياجاتك من القائمة أدناه
			</p>
		</header>

		<!-- SubCategories Section -->
		<section class="relative mb-12">
			<h2
				class="text-2xl font-semibold text-transparent bg-clip-text bg-gradient-to-r from-blue-600 to-blue-400 mb-6 text-right subcategories-header"
			>
				التصنيفات الفرعية
			</h2>
			<div class="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-8 rtl">
				<div
					v-for="subCategory in subCategories"
					:key="subCategory.id"
					@click="fetchServicesBySubCategory(subCategory.id)"
					class="fancy-card subcategory h-72 rounded-xl overflow-hidden transition-all duration-500 shadow-fancy hover:shadow-fancy-hover cursor-pointer flex flex-col"
					:class="{
						'selected-subcategory':
							selectedSubCategory && selectedSubCategory.id === subCategory.id,
						folded:
							selectedSubCategory &&
							selectedSubCategory.id === subCategory.id &&
							isFolded,
						[`subcategory-${subCategory.id}`]: true,
					}"
					:data-id="subCategory.id"
				>
					<!-- SubCategory Image -->
					<div class="relative overflow-hidden h-44">
						<!-- Glass border overlay -->
						<div class="fancy-glass-border"></div>

						<img
							src="@/img/cleaning.jpg"
							:alt="subCategory.name"
							class="w-full h-full object-cover transition-all duration-700 hover:scale-110 fancy-image"
						/>
						<div
							class="absolute inset-0 bg-gradient-to-t from-blue-900/80 via-black/30 to-transparent"
						></div>

						<!-- Fold/Unfold indicator -->
						<div
							v-if="
								selectedSubCategory && selectedSubCategory.id === subCategory.id
							"
							class="absolute bottom-3 right-3 w-9 h-9 fancy-button flex items-center justify-center transition-all duration-300 z-10"
							:class="{ 'rotate-180': !isFolded }"
						>
							<svg
								xmlns="http://www.w3.org/2000/svg"
								fill="none"
								viewBox="0 0 24 24"
								stroke-width="2.5"
								stroke="currentColor"
								class="w-5 h-5 text-white"
							>
								<path
									stroke-linecap="round"
									stroke-linejoin="round"
									d="M19.5 8.25l-7.5 7.5-7.5-7.5"
								/>
							</svg>
						</div>
					</div>
					<!-- SubCategory Content -->
					<div class="p-5 relative bg-white flex-grow flex flex-col">
						<div class="flex-grow">
							<p
								class="text-xl font-bold text-transparent bg-clip-text bg-gradient-to-r from-blue-700 to-blue-500 text-center mb-3"
							>
								{{ subCategory.name }}
							</p>
							<div class="flex justify-center">
								<span
									class="inline-block bg-gradient-to-r from-blue-100 to-blue-50 text-blue-700 px-3 py-1 rounded-full text-xs font-semibold shadow-sm"
								>
									عرض الخدمات
								</span>
							</div>
						</div>
						<div class="fancy-glow"></div>
					</div>
				</div>
			</div>

			<div v-if="subCategories.length === 0" class="text-center py-8">
				<p class="text-gray-500 text-lg">لا توجد تصنيفات فرعية متاحة حالياً</p>
			</div>
		</section>

		<!-- Selected SubCategory Services Section -->
		<section
			v-if="selectedSubCategory"
			class="relative mt-12 services-section"
			:class="{
				'show-services': showServicesSection,
				'fade-out-services': fadeOutServices,
			}"
		>
			<h2
				class="text-2xl font-semibold text-transparent bg-clip-text bg-gradient-to-r from-blue-600 to-blue-400 mb-6 text-right flex items-center justify-between"
			>
				<span>{{ selectedSubCategory.name }}</span>
				<span class="text-sm text-gray-500"
					>{{ servicesBySubCategory.length }} خدمة متاحة</span
				>
			</h2>

			<!-- Loading indicator -->
			<div v-if="isLoading" class="flex justify-center py-12 animate-fadeIn">
				<div class="fancy-spinner"></div>
			</div>

			<div
				v-else
				class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-8 rtl services-grid"
			>
				<div
					v-for="(service, index) in servicesBySubCategory"
					:key="service.id"
					@click="navigateToService(service.id)"
					class="fancy-service-card h-[430px] rounded-xl overflow-hidden transition-all duration-500 cursor-pointer shadow-fancy hover:shadow-fancy-hover flex flex-col transform perspective-card"
					:style="{ 'animation-delay': `${index * 0.1}s` }"
				>
					<!-- Card inner container with 3D effect -->
					<div class="card-inner w-full h-full">
						<!-- Glass border overlay -->
						<div class="fancy-glass-border"></div>
						<!-- Service Image -->
						<div class="relative overflow-hidden h-44">
							<img
								src="@/img/cleaning.jpg"
								:alt="service.title"
								class="w-full h-full object-cover transition-all duration-700 hover:scale-110 fancy-image"
							/>
							<div
								class="absolute inset-0 bg-gradient-to-t from-blue-900/80 via-black/30 to-transparent"
							></div>

							<!-- Service badge -->
							<div
								class="absolute top-3 right-3 bg-gradient-to-r from-blue-600 to-blue-400 text-white text-xs font-bold px-3 py-1 rounded-full shadow-lg"
							>
								خدمة
							</div>
						</div>

						<!-- Service Content -->
						<div class="p-5 relative flex-grow flex flex-col bg-white/95">
							<div class="flex-grow">
								<p
									class="text-xl font-bold text-transparent bg-clip-text bg-gradient-to-r from-blue-700 to-blue-500 mb-3 line-clamp-2"
								>
									{{ service.title }}
								</p>
								<div class="flex items-center mb-3">
									<svg
										xmlns="http://www.w3.org/2000/svg"
										class="h-4 w-4 text-blue-500 ml-1"
										viewBox="0 0 20 20"
										fill="currentColor"
									>
										<path
											fill-rule="evenodd"
											d="M10 9a3 3 0 100-6 3 3 0 000 6zm-7 9a7 7 0 1114 0H3z"
											clip-rule="evenodd"
										/>
									</svg>
									<span class="text-gray-600 text-sm ml-1">بواسطة:</span>
									<span class="font-medium text-blue-500 text-sm">{{
										service.providerUsername
									}}</span>
								</div>
								<p
									v-if="service.description"
									class="text-gray-700 text-sm mb-4 line-clamp-3"
								>
									{{ service.description }}
								</p>
							</div>

							<div class="mt-auto">
								<div
									v-if="service.estimatedPrice"
									class="flex justify-between items-center mb-4"
								>
									<span class="text-sm text-gray-600">السعر التقريبي:</span>
									<span
										class="font-bold text-lg text-transparent bg-clip-text bg-gradient-to-r from-green-600 to-green-400"
									>
										{{ service.estimatedPrice }} IQD
									</span>
								</div>

								<button
									class="fancy-button-gradient w-full py-2.5 px-4 rounded-lg transition-all duration-300 flex items-center justify-center"
								>
									<span>عرض التفاصيل</span>
									<svg
										xmlns="http://www.w3.org/2000/svg"
										class="h-5 w-5 mr-2 rtl:rotate-180"
										fill="none"
										viewBox="0 0 24 24"
										stroke="currentColor"
									>
										<path
											stroke-linecap="round"
											stroke-linejoin="round"
											stroke-width="2"
											d="M9 5l7 7-7 7"
										/>
									</svg>
								</button>
							</div>
							<div class="fancy-glow"></div>
						</div>
					</div>
				</div>

				<div
					v-if="servicesBySubCategory.length === 0"
					class="col-span-full text-center py-8 animate-fadeIn"
				>
					<p class="text-gray-500 text-lg">
						لا توجد خدمات متاحة في هذا التصنيف حالياً
					</p>
				</div>
			</div>
		</section>
	</div>
</template>

<style scoped>
	.shadow-fancy {
		box-shadow: 0 10px 25px -5px rgba(59, 130, 246, 0.15),
			0 8px 10px -6px rgba(59, 130, 246, 0.1), 0 0 0 1px rgba(59, 130, 246, 0.1);
	}

	.shadow-fancy-hover {
		box-shadow: 0 20px 35px -10px rgba(59, 130, 246, 0.25),
			0 10px 15px -3px rgba(59, 130, 246, 0.15),
			0 0 0 2px rgba(59, 130, 246, 0.2);
	}

	.fancy-card {
		position: relative;
		transform: translateY(0);
		border: none;
		background: linear-gradient(to bottom, #ffffff, #f9fafb);
	}

	.fancy-card:hover {
		transform: translateY(-6px) scale(1.02);
	}

	.fancy-service-card {
		position: relative;
		transform: translateY(0);
		border: none;
		background: linear-gradient(to bottom, #ffffff, #f9fafb);
	}

	.fancy-service-card:hover {
		transform: translateY(-6px) scale(1.02);
	}

	.perspective-card {
		transform-style: preserve-3d;
		perspective: 1000px;
	}

	.card-inner {
		position: relative;
		transition: transform 0.6s;
	}

	.fancy-card:hover .fancy-image,
	.fancy-service-card:hover .fancy-image {
		transform: scale(1.1);
	}

	.fancy-glass-border {
		position: absolute;
		top: 0;
		left: 0;
		right: 0;
		bottom: 0;
		z-index: 5;
		pointer-events: none;
		border-radius: inherit;
		box-shadow: inset 0 0 0 1px rgba(255, 255, 255, 0.15);
	}

	.fancy-overlay {
		z-index: 2;
		opacity: 1;
		transition: all 0.5s ease;
	}

	.fancy-card:hover .fancy-overlay,
	.fancy-service-card:hover .fancy-overlay {
		background: linear-gradient(
			to top,
			rgba(37, 99, 235, 0.85),
			rgba(0, 0, 0, 0.4),
			rgba(0, 0, 0, 0.2)
		);
	}

	.fancy-title {
		transform: translateY(0);
		transition: transform 0.5s ease;
	}

	.fancy-card:hover .fancy-title,
	.fancy-service-card:hover .fancy-title {
		transform: translateY(-5px) scale(1.05);
	}

	.fancy-button {
		background: linear-gradient(145deg, #3b82f6, #2563eb);
		border-radius: 50%;
		box-shadow: 0 4px 10px rgba(37, 99, 235, 0.4),
			0 0 0 2px rgba(255, 255, 255, 0.2);
		transition: all 0.3s ease;
	}

	.fancy-button:hover {
		box-shadow: 0 6px 15px rgba(37, 99, 235, 0.5),
			0 0 0 3px rgba(255, 255, 255, 0.3);
		transform: translateY(-2px);
	}

	.fancy-button-gradient {
		background: linear-gradient(135deg, #3b82f6, #2563eb);
		color: white;
		font-weight: 600;
		border: none;
		position: relative;
		overflow: hidden;
	}

	.fancy-button-gradient::before {
		content: "";
		position: absolute;
		top: 0;
		left: 0;
		width: 100%;
		height: 100%;
		background: linear-gradient(
			135deg,
			transparent,
			rgba(255, 255, 255, 0.2),
			transparent
		);
		transition: transform 0.5s ease;
		transform: translateX(-100%);
	}

	.fancy-button-gradient:hover::before {
		transform: translateX(100%);
	}

	.drop-shadow-glow {
		filter: drop-shadow(0 0 8px rgba(59, 130, 246, 0.5));
	}

	.fancy-glow {
		position: absolute;
		top: 0;
		left: 0;
		width: 100%;
		height: 100%;
		background: radial-gradient(
			circle at center,
			rgba(59, 130, 246, 0.08) 0%,
			transparent 70%
		);
		opacity: 0;
		pointer-events: none;
		z-index: 0;
		transition: opacity 0.5s ease;
	}

	div:hover .fancy-glow {
		opacity: 1;
	}

	.static-glow {
		position: absolute;
		top: 0;
		left: 0;
		width: 100%;
		height: 100%;
		background: radial-gradient(
			circle at center,
			rgba(59, 130, 246, 0.05) 0%,
			transparent 70%
		);
		opacity: 0.8;
		pointer-events: none;
		z-index: -1;
	}

	.selected-subcategory {
		border-color: #3b82f6;
		box-shadow: 0 15px 35px rgba(59, 130, 246, 0.25),
			0 10px 15px rgba(59, 130, 246, 0.15), 0 0 0 2px rgba(59, 130, 246, 0.3);
		position: relative;
		z-index: 1;
		transform: translateY(-5px) scale(1.02);
	}

	.selected-subcategory::after {
		content: "";
		position: absolute;
		bottom: -12px;
		left: 50%;
		transform: translateX(-50%);
		width: 0;
		height: 0;
		border-left: 12px solid transparent;
		border-right: 12px solid transparent;
		border-top: 12px solid #3b82f6;
		opacity: 0;
		transition: opacity 0.3s ease;
		filter: drop-shadow(0 5px 5px rgba(59, 130, 246, 0.2));
	}

	.selected-subcategory:not(.folded)::after {
		opacity: 1;
	}

	.fancy-spinner {
		width: 50px;
		height: 50px;
		border-radius: 50%;
		background: conic-gradient(#0000 10%, #3b82f6);
		-webkit-mask: radial-gradient(
			farthest-side,
			#0000 calc(100% - 8px),
			#000 0
		);
		animation: fancy-spin 1s infinite linear;
	}

	@keyframes fancy-spin {
		to {
			transform: rotate(1turn);
		}
	}

	/* Services section animations */
	.services-section {
		opacity: 0;
		transform: translateY(20px);
		transition: opacity 0.5s ease, transform 0.5s ease;
		overflow: hidden;
		max-height: 0;
	}

	.services-section.show-services {
		opacity: 1;
		transform: translateY(0);
		max-height: 5000px;
		transition: opacity 0.5s ease, transform 0.5s ease, max-height 0.8s ease;
	}

	.services-section.fade-out-services {
		opacity: 0;
		transform: translateY(10px);
		transition: opacity 0.3s ease, transform 0.3s ease;
	}

	.service-card {
		opacity: 0;
		transform: translateY(20px);
		animation: slideUpFadeIn 0.5s forwards;
	}

	.services-grid {
		perspective: 1200px;
	}

	@keyframes slideUpFadeIn {
		from {
			opacity: 0;
			transform: translateY(20px) scale(0.95);
		}
		to {
			opacity: 1;
			transform: translateY(0) scale(1);
		}
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

	.line-clamp-2 {
		display: -webkit-box;
		-webkit-line-clamp: 2;
		-webkit-box-orient: vertical;
		overflow: hidden;
	}

	.line-clamp-3 {
		display: -webkit-box;
		-webkit-line-clamp: 3;
		-webkit-box-orient: vertical;
		overflow: hidden;
	}
</style>
