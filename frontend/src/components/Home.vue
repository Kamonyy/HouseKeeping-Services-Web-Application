<script setup>
	import { ref, onMounted } from "vue";
	import axios from "axios";
	import CategoryCard from "./CategoryCard.vue";
	import { extractArrayFromResponse } from "../utils/apiUtils";

	const categories = ref([]);

	// Fetch categories from API
	const fetchCategories = async () => {
		try {
			const response = await axios.get("/api/category");
			// Use the utility function to handle different response formats
			const categoriesArray = extractArrayFromResponse(
				response.data,
				"categories"
			);
			categories.value = categoriesArray.slice(0, 4);
		} catch (error) {
			console.error("Error fetching categories:", error);
			categories.value = [];
		}
	};

	// Fetch categories on component mount
	onMounted(() => {
		fetchCategories();
	});
</script>

<template>
	<div class="container mx-auto px-4 py-8 animate-fadeIn">
		<header class="text-center mb-8 relative">
			<div class="static-glow"></div>
			<h1
				class="text-4xl font-extrabold text-transparent bg-clip-text bg-gradient-to-r from-blue-700 to-blue-500 mb-4"
			>
				الطريقة السهلة والموثوقة للعناية بمنزلك
			</h1>
			<p class="text-gray-600 text-lg max-w-3xl mx-auto">
				احجز فوراً مع محترفين ذوي تقييم عالٍ للقيام بمهام التنظيف والصيانة
				بأسعار واضحة مسبقاً.
			</p>
		</header>

		<section class="container mx-auto px-4 py-8 relative">
			<div class="static-glow"></div>
			<div class="flex justify-between items-center mb-6 rtl">
				<div>
					<h2
						class="text-3xl font-semibold text-transparent bg-clip-text bg-gradient-to-r from-blue-700 to-blue-500 mb-2"
					>
						مهام التنظيف والصيانة
					</h2>
					<p class="text-gray-600 text-lg">
						احجز فوراً مع محترفين ذوي تقييم عالٍ للقيام بمهام التنظيف والصيانة
						بأسعار واضحة مسبقاً.
					</p>
				</div>
				<RouterLink
					to="/services"
					class="text-blue-500 hover:text-blue-700 transition-all duration-300 font-semibold text-lg relative group"
				>
					عرض الكل
					<i
						class="fas fa-arrow-left ml-1 transition-transform duration-300 group-hover:translate-x-1"
					></i>
					<span
						class="absolute -bottom-1 left-0 w-0 h-0.5 bg-blue-500 group-hover:w-full transition-all duration-300"
					></span>
				</RouterLink>
			</div>

			<div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-6">
				<CategoryCard
					v-for="category in categories"
					:key="category.id"
					:category="category"
				/>
			</div>
		</section>
	</div>
</template>

<style scoped>
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
</style>
