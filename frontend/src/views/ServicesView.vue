<script setup>
	import { ref, onMounted } from "vue";
	import axios from "axios";
	import CategoryCard from "@/components/CategoryCard.vue";

	const categories = ref([]);

	const fetchCategories = async () => {
		try {
			const response = await axios.get("https://localhost:7007/api/category");
			categories.value = response.data;
		} catch (error) {
			console.error("Error fetching categories:", error);
		}
	};

	onMounted(() => {
		fetchCategories();
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
				الخدمات المتوفرة لدينا
			</h1>
			<p class="text-gray-600 max-w-3xl mx-auto">
				استكشف مجموعة الخدمات التي نقدمها لتلبية احتياجاتك. اختر الفئة التي
				تناسب احتياجاتك وابدأ رحلتك معنا.
			</p>
		</header>

		<!-- Services Section -->
		<section class="container mx-auto px-4 py-4 relative">
			<!-- Categories Grid -->
			<div
				class="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-6"
				dir="rtl"
			>
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
