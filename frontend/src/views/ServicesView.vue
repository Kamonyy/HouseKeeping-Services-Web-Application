<script setup>
	import { ref, onMounted } from "vue";
	import axios from "axios";
	import CategoryCard from "@/components/CategoryCard.vue";

	const categories = ref([]);

	const fetchCategories = async () => {
		try {
			const response = await axios.get("https://localhost:7007/api/category");
			categories.value = response.data.slice(0, 4);
		} catch (error) {
			console.error("Error fetching categories:", error);
		}
	};

	onMounted(() => {
		fetchCategories();
	});
</script>

<template>
	<div class="container mx-auto px-4 py-8 m">
		<!-- Header Section -->
		<header class="text-center mb-8">
			<h1 class="text-3xl font-bold mb-4">الخدمات المتوفرة لدينا</h1>
		</header>

		<!-- Services Section -->
		<section class="container mx-auto px-4 py-8">
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
