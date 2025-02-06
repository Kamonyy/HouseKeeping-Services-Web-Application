<script setup>
	defineProps({
		service: {
			type: Object,
			required: true,
		},
	});
</script>

<template>
	<RouterLink
		:to="'/service/' + service.id"
		class="block bg-white shadow-md rounded-lg overflow-hidden text-center transform transition-transform hover:-translate-y-1 hover:shadow-xl"
	>
		<!-- Card Image -->
		<img
			src="@/img/service.jpg"
			:alt="service.title"
			class="w-full h-40 object-cover"
		/>
		<!-- Card Content -->
		<div class="p-4">
			<p class="text-lg font-semibold">
				{{ service.title }}
			</p>
			<p class="text-sm text-gray-600">
				{{ service.username }}
			</p>
			<p class="text-sm">
				{{ service.description }}
			</p>
		</div>
	</RouterLink>
</template>

<style>
	p {
		color: rgb(45, 129, 66);
	}
</style>
