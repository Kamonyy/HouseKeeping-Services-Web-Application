<!-- CategoryServicesView.vue -->
<script setup>
	import { ref, onMounted } from "vue";
	import { useRoute } from "vue-router";
	import axios from "axios";

	const route = useRoute();
	const categoryId = route.params.id; // Get the category ID from the route parameters
	const services = ref([]);

	const fetchServices = async () => {
		try {
			const response = await axios.get(
				`https://localhost:7007/api/subcategory/bycategory/${categoryId}`
			);
			services.value = response.data;
		} catch (error) {
			console.error("Error fetching services:", error);
		}
	};

	onMounted(() => {
		fetchServices();
	});
</script>

<template>
	<div class="container mx-auto px-4 py-8">
		<!-- Header Section -->
		<header class="text-center mb-8">
			<h1 class="text-3xl font-bold mb-4">الخدمات المتوفرة</h1>
		</header>

		<!-- Services Section -->
		<section>
			<div class="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-6 rtl">
				<div
					v-for="service in services"
					:key="service.id"
					class="bg-white shadow-md rounded-lg overflow-hidden"
				>
					<!-- Service Image -->
					<img
						src="@/img/cleaning.jpg"
						:alt="service.name"
						class="w-full h-40 object-cover"
					/>
					<!-- Service Content -->
					<div class="p-4">
						<p class="text-lg font-semibold">{{ service.name }}</p>
						<p class="text-gray-600">{{ service.categoryName }}</p>
						<p class="text-green-600 font-bold"></p>
					</div>
				</div>
			</div>
		</section>
	</div>
</template>

<style scoped></style>
