<script setup>
	import { onMounted, computed } from "vue";
	import { useUserStore } from "../store/userStore";
	import { useCategoryStore } from "../store/categoryStore"; // Import the category store

	const userStore = useUserStore();
	const categoryStore = useCategoryStore();

	const isLoggedIn = computed(() => userStore.isLoggedIn);

	onMounted(() => {
		if (isLoggedIn.value) {
			categoryStore.fetchCategories(); // Fetch categories when user is logged in
		}
	});
</script>

<template>
	<h1>Welcome to Our Housekeeping Services</h1>
	<p>
		Your comfort is our priority. We offer a range of services to keep your home
		clean and tidy.
	</p>

	<ul v-if="isLoggedIn">
		<li v-for="category in categoryStore.categories" :key="category">
			{{ category.id }}- {{ category.name }}
		</li>
	</ul>
	<p v-else>Please log in to view our available services.</p>
</template>

<style scoped>
	h1 {
		text-align: center;
		color: #2c3e50;
	}
	p {
		text-align: center;
		font-size: 1.2em;
		color: #34495e;
	}
	ul {
		list-style-type: none;
		padding: 0;
		text-align: center;
	}
	li {
		margin: 10px 0;
		font-size: 1.1em;
		color: #2980b9;
	}
	button {
		margin-top: 20px;
		padding: 10px 20px;
		background-color: #27ae60;
		color: white;
		border: none;
		border-radius: 5px;
		cursor: pointer;
	}
	button:hover {
		background-color: #219653;
	}
</style>
