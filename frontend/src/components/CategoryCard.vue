<script setup>
	defineProps({
		category: {
			type: Object,
			required: true,
		},
	});
</script>

<template>
	<RouterLink
		:to="'/category/' + category.id"
		class="block bg-white shadow-md rounded-lg overflow-hidden text-center transform transition-transform hover:-translate-y-1 hover:shadow-xl"
	>
		<!-- Card Image -->
		<img
			src="@/img/cleaning.jpg"
			:alt="category.name"
			class="w-full h-40 object-cover"
		/>
		<!-- Card Content -->
		<div class="p-4">
			<p class="text-lg font-semibold">
				{{ category.name }}
			</p>
		</div>
	</RouterLink>
</template>

<style>
	p {
		color: rgb(45, 129, 66);
	}

	p {
		color: rgb(45, 129, 66);
	}
</style>
