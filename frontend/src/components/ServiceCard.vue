<script setup>
	import { computed } from "vue";

	const props = defineProps({
		service: {
			type: Object,
			required: true,
		},
	});

	// Function to get appropriate image based on service title/category
	const getServiceImage = computed(() => {
		const title = props.service.title?.toLowerCase() || "";
		const category = props.service.category?.toLowerCase() || "";

		// Cleaning services
		if (
			title.includes("تنظيف") ||
			title.includes("نظافة") ||
			category.includes("تنظيف") ||
			category.includes("نظافة")
		) {
			return "https://images.pexels.com/photos/4107120/pexels-photo-4107120.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1";
		}

		// Electrical services
		if (
			title.includes("كهرباء") ||
			title.includes("كهربائية") ||
			category.includes("كهرباء")
		) {
			return "https://images.pexels.com/photos/8005397/pexels-photo-8005397.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1";
		}

		// Plumbing services
		if (
			title.includes("سباكة") ||
			title.includes("مياه") ||
			category.includes("سباكة")
		) {
			return "https://images.pexels.com/photos/6419128/pexels-photo-6419128.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1";
		}

		// Carpentry services
		if (
			title.includes("نجارة") ||
			title.includes("خشب") ||
			category.includes("نجارة")
		) {
			return "https://images.pexels.com/photos/6419123/pexels-photo-6419123.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1";
		}

		// Painting services
		if (
			title.includes("دهان") ||
			title.includes("طلاء") ||
			category.includes("دهان")
		) {
			return "https://images.pexels.com/photos/6598/coffee-desk-notes-workspace.jpg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1";
		}

		// AC services
		if (
			title.includes("تكييف") ||
			title.includes("مكيف") ||
			category.includes("تكييف")
		) {
			return "https://images.pexels.com/photos/3689532/pexels-photo-3689532.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1";
		}

		// Gardening services
		if (
			title.includes("حدائق") ||
			title.includes("حديقة") ||
			category.includes("حدائق")
		) {
			return "https://images.pexels.com/photos/4503273/pexels-photo-4503273.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1";
		}

		// Moving services
		if (
			title.includes("نقل") ||
			title.includes("تحريك") ||
			category.includes("نقل")
		) {
			return "https://images.pexels.com/photos/4246091/pexels-photo-4246091.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1";
		}

		// Default image if no match
		return "https://images.pexels.com/photos/3747463/pexels-photo-3747463.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1";
	});
</script>

<template>
	<RouterLink
		:to="'/service/' + service.id"
		class="card block flex flex-col slide-up"
	>
		<div class="relative overflow-hidden group">
			<img
				:src="getServiceImage"
				:alt="service.title"
				class="w-full h-52 object-cover transition-all duration-normal group-hover:brightness-100"
			/>
			<!-- Gradient overlay -->
			<div
				class="absolute inset-0 bg-gradient-to-t from-gray-900/90 via-gray-900/40 to-transparent flex items-end p-4"
			>
				<h3 class="text-white text-xl font-bold mb-2 drop-shadow-sm">
					{{ service.title }}
				</h3>
			</div>
		</div>
		<div class="card-body flex-grow flex flex-col justify-between">
			<div class="stack-y-4">
				<div class="flex items-center justify-between">
					<p class="text-gradient font-semibold text-lg">
						{{ service.title }}
					</p>
					<span class="badge badge-primary">
						{{ service.category }}
					</span>
				</div>
				<p class="text-sm text-color-medium border-b border-gray-100 pb-3">
					بواسطة:
					<span class="text-primary font-bold">{{ service.username }}</span>
				</p>
				<p class="text-sm text-color-dark line-clamp-3">
					{{ service.description }}
				</p>
			</div>
		</div>
	</RouterLink>
</template>

<style scoped>
	.group {
		transition: all var(--transition-normal);
	}

	.drop-shadow-sm {
		filter: drop-shadow(0 1px 1px rgba(0, 0, 0, 0.4));
	}

	.line-clamp-3 {
		display: -webkit-box;
		-webkit-line-clamp: 3;
		-webkit-box-orient: vertical;
		overflow: hidden;
	}

	.text-primary {
		color: var(--color-primary) !important;
	}

	.text-color-dark {
		color: var(--color-gray-800) !important;
	}

	.text-color-medium {
		color: var(--color-gray-600) !important;
	}

	.duration-normal {
		transition-duration: var(--transition-normal);
	}

	.text-gradient {
		background-clip: text !important;
		-webkit-background-clip: text !important;
		color: transparent !important;
		background-image: linear-gradient(to right, #0066ff, #4d94ff) !important;
		font-weight: 700 !important;
		text-shadow: 0 0 1px rgba(0, 102, 255, 0.1) !important;
	}

	/* Add white text color for service titles */
	h3 {
		color: white !important;
		font-weight: 700;
		text-shadow: 0 2px 4px rgba(0, 0, 0, 0.7);
	}
</style>
