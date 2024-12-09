<script setup>
	import { ref, onMounted } from "vue";
	import axios from "axios";
	import CategoryCard from "./CategoryCard.vue";

	const categories = ref([]);

	// Fetch categories from API
	const fetchCategories = async () => {
		try {
			const response = await axios.get("https://localhost:7007/api/category");
			categories.value = response.data.slice(0, 4);
		} catch (error) {
			console.error("Error fetching categories:", error);
		}
	};

	// Fetch categories on component mount
	onMounted(() => {
		fetchCategories();
	});
</script>

<template>
	<div class="container mx-auto px-4 py-8 m">
		<!-- Header Section -->
		<header class="text-center mb-8">
			<h1 class="text-3xl font-bold mb-4">
				الطريقة السهلة والموثوقة للعناية بمنزلك
			</h1>
		</header>

		<!-- Services Section -->
		<section class="container mx-auto px-4 py-8">
			<!-- Section Header -->
			<div class="flex justify-between items-center mb-6 rtl">
				<div>
					<h2 class="text-2xl font-bold mb-2">مهام التنظيف والصيانة</h2>
					<p class="text-gray-600">
						احجز فوراً مع محترفين ذوي تقييم عالٍ للقيام بمهام التنظيف والصيانة
						بأسعار واضحة مسبقاً.
					</p>
				</div>
				<RouterLink to="/services" class="font-semibold">
					عرض الكل >
				</RouterLink>
			</div>

			<!-- Categories Grid -->
			<div class="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-4 gap-6 rtl">
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
	.m {
		max-width: 1280px;
		margin: 0 auto;
	}
</style>
