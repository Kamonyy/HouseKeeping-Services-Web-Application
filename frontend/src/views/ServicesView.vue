<script setup>
	import { ref, onMounted } from "vue";
	import axios from "axios";
	import CategoryCard from "@/components/CategoryCard.vue";
	import { extractArrayFromResponse } from "@/utils/apiUtils";
	import { RouterLink } from "vue-router";

	const categories = ref([]);
	const isLoading = ref(true);

	// Statistics for the services page
	const statistics = [
		{
			value: "20+",
			label: "فئة خدمية",
			icon: "fas fa-th-large",
		},
		{
			value: "100+",
			label: "خدمة متنوعة",
			icon: "fas fa-tasks",
		},
	];

	const fetchCategories = async () => {
		try {
			isLoading.value = true;
			const response = await axios.get("/api/category");
			categories.value = extractArrayFromResponse(response.data, "categories");
		} catch (error) {
			console.error("Error fetching categories:", error);
			categories.value = [];
		} finally {
			isLoading.value = false;
		}
	};

	onMounted(() => {
		fetchCategories();
	});
</script>

<template>
	<div>
		<!-- Hero Section -->
		<section
			class="relative bg-gradient-to-b from-blue-50 to-white py-16 overflow-hidden"
		>
			<!-- Background decorations -->
			<div class="absolute top-0 left-0 w-full h-full overflow-hidden z-0">
				<div class="bg-pattern opacity-5 absolute inset-0"></div>
				<div
					class="absolute top-20 left-20 w-64 h-64 bg-blue-100 rounded-full filter blur-3xl opacity-30"
				></div>
				<div
					class="absolute bottom-10 right-10 w-80 h-80 bg-blue-200 rounded-full filter blur-3xl opacity-30"
				></div>
			</div>

			<div class="container mx-auto px-4 relative z-10">
				<!-- Header Section -->
				<header class="text-center mb-12 relative max-w-3xl mx-auto">
					<div
						class="inline-block bg-blue-100 text-blue-700 px-4 py-2 rounded-full text-sm font-medium mb-4"
					>
						استكشف خدماتنا
					</div>
					<h1
						class="text-4xl md:text-5xl font-extrabold text-transparent bg-clip-text bg-gradient-to-r from-blue-700 to-blue-500 mb-6 drop-shadow-sm"
					>
						الخدمات المتوفرة لدينا
					</h1>
					<p class="text-gray-700 text-lg max-w-3xl mx-auto">
						استكشف مجموعة الخدمات التي نقدمها لتلبية احتياجاتك. اختر الفئة التي
						تناسب احتياجاتك وابدأ رحلتك معنا.
					</p>
				</header>

				<!-- Statistics -->
				<div class="max-w-xl mx-auto grid grid-cols-2 gap-8 mb-16">
					<div
						v-for="(stat, index) in statistics"
						:key="index"
						class="bg-white rounded-xl p-6 shadow-lg flex items-center gap-5 rtl transform transition-all duration-300 hover:shadow-xl hover:-translate-y-1"
					>
						<div
							class="w-16 h-16 bg-blue-100 rounded-full flex items-center justify-center flex-shrink-0"
						>
							<i :class="[stat.icon, 'text-blue-600 text-3xl']"></i>
						</div>
						<div>
							<div class="text-3xl font-bold text-gray-900">
								{{ stat.value }}
							</div>
							<div class="text-gray-600">{{ stat.label }}</div>
						</div>
					</div>
				</div>
			</div>
		</section>

		<!-- Divider -->
		<div class="relative h-24 overflow-hidden">
			<div
				class="absolute inset-0 bg-white"
				style="clip-path: polygon(0 0, 100% 0, 100% 100%, 0 30%)"
			></div>
			<div
				class="absolute inset-0 bg-gray-50"
				style="clip-path: polygon(0 30%, 100% 100%, 0 100%)"
			></div>
		</div>

		<!-- Services Section -->
		<section class="py-16 bg-gray-50">
			<div class="container mx-auto px-4">
				<div class="flex justify-between items-center mb-10 rtl">
					<div>
						<div
							class="inline-block bg-blue-100 text-blue-700 px-4 py-2 rounded-full text-sm font-medium mb-4"
						>
							فئات الخدمات
						</div>
						<h2 class="text-3xl font-bold text-gray-900 mb-3">
							اختر الفئة المناسبة لاحتياجاتك
						</h2>
						<p class="text-gray-600 text-lg">
							تصفح مجموعة متنوعة من فئات الخدمات لتجد ما تبحث عنه بسهولة
						</p>
					</div>
					<RouterLink
						to="/"
						class="text-blue-600 hover:text-blue-800 transition-all duration-300 font-semibold text-lg relative group flex items-center bg-blue-50 px-5 py-2.5 rounded-xl hover:bg-blue-100"
					>
						<i class="fas fa-home mr-2"></i>
						العودة للرئيسية
					</RouterLink>
				</div>

				<!-- Loading Indicator -->
				<div v-if="isLoading" class="flex justify-center items-center py-16">
					<div
						class="w-16 h-16 border-4 border-blue-600 border-t-transparent rounded-full animate-spin"
					></div>
				</div>

				<!-- Categories Grid -->
				<div
					v-else
					class="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-8 rtl"
				>
					<CategoryCard
						v-for="(category, index) in categories"
						:key="category.id"
						:category="category"
					/>
				</div>
			</div>
		</section>

		<!-- Divider with accent -->
		<div class="relative h-24 overflow-hidden">
			<div
				class="absolute inset-0 bg-gray-50"
				style="clip-path: polygon(0 0, 100% 0, 100% 100%, 0 30%)"
			></div>
			<div
				class="absolute inset-0 bg-gradient-to-r from-blue-600 to-blue-800"
				style="clip-path: polygon(0 30%, 100% 100%, 0 100%)"
			></div>
		</div>

		<!-- CTA Section -->
		<section
			class="py-16 bg-gradient-to-r from-blue-600 to-blue-800 text-white"
		>
			<div class="container mx-auto px-4 text-center">
				<div class="max-w-3xl mx-auto">
					<p class="text-white text-3xl font-bold mb-6">لم تجد ما تبحث عنه؟</p>
					<p class="text-xl mb-8 opacity-90 text-white">
						تواصل معنا لطلب خدمة مخصصة تناسب احتياجاتك الخاصة
					</p>
					<RouterLink
						to="/help"
						class="inline-block px-8 py-4 bg-white text-blue-600 rounded-xl hover:bg-gray-100 transition-all duration-300 font-bold text-lg shadow-xl hover:shadow-2xl hover:-translate-y-1"
					>
						تواصل معنا
					</RouterLink>
				</div>
			</div>
		</section>
	</div>
</template>

<style scoped>
	.rtl {
		direction: rtl;
		text-align: right;
	}

	.bg-pattern {
		background-image: url("data:image/svg+xml,%3Csvg width='60' height='60' viewBox='0 0 60 60' xmlns='http://www.w3.org/2000/svg'%3E%3Cg fill='none' fill-rule='evenodd'%3E%3Cg fill='%233b82f6' fill-opacity='0.1'%3E%3Cpath d='M36 34v-4h-2v4h-4v2h4v4h2v-4h4v-2h-4zm0-30V0h-2v4h-4v2h4v4h2V6h4V4h-4zM6 34v-4H4v4H0v2h4v4h2v-4h4v-2H6zM6 4V0H4v4H0v2h4v4h2V6h4V4H6z'/%3E%3C/g%3E%3C/g%3E%3C/svg%3E");
	}
</style>
