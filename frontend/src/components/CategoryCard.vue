<script setup>
	import { getCategoryImage } from "../utils/categoryImageHelper";

	const props = defineProps({
		category: {
			type: Object,
			required: true,
		},
	});

	// Helper function to get properties with potential casing variations
	const getName = (category) => {
		return category.name || category.Name || "خدمة";
	};

	const getDescription = (category) => {
		return category.description || category.Description || "";
	};
</script>

<template>
	<RouterLink
		:to="'/category/' + (category.id || category.Id)"
		class="group block bg-white rounded-xl overflow-hidden transition-all duration-300 hover:shadow-xl hover:scale-102 shadow-md border border-gray-100 hover:border-blue-400"
	>
		<div
			class="relative overflow-hidden h-52 bg-gradient-to-br from-blue-50 to-indigo-50"
		>
			<img
				:src="getCategoryImage(category)"
				:alt="getName(category)"
				class="w-full h-full object-cover transition-all duration-500 group-hover:scale-110 group-hover:brightness-105"
				@error="
					$event.target.src =
						'https://images.unsplash.com/photo-1581578731548-c64695cc6952?q=80&w=1000'
				"
			/>
			<div
				class="absolute inset-0 bg-gradient-to-t from-blue-900/50 via-blue-800/30 to-transparent flex items-end p-5 transition-opacity duration-300"
			>
				<div class="category-name-container w-full">
					<h3 class="category-name text-white text-xl font-bold">
						{{ getName(category) }}
					</h3>
					<div class="category-underline"></div>
				</div>
			</div>
		</div>
		<div class="p-5 bg-gradient-to-br from-white to-blue-50/20">
			<div
				v-if="getDescription(category)"
				class="text-sm text-gray-700 line-clamp-2 min-h-[40px]"
			>
				{{ getDescription(category) }}
			</div>
			<div class="mt-3 flex justify-end">
				<span
					class="text-sm font-medium flex items-center gap-1.5 group-hover:text-blue-700 transition-colors"
				>
					عرض الخدمات
					<i
						class="fas fa-arrow-left text-xs transition-transform group-hover:translate-x-[-3px]"
					></i>
				</span>
			</div>
		</div>
	</RouterLink>
</template>

<style scoped>
	.drop-shadow-lg {
		filter: drop-shadow(0 4px 3px rgb(0 0 0 / 0.4));
	}

	.line-clamp-2 {
		display: -webkit-box;
		-webkit-line-clamp: 2;
		-webkit-box-orient: vertical;
		overflow: hidden;
	}

	.hover\:scale-102:hover {
		transform: scale(1.02);
	}

	/* Enhanced category name styling */
	.category-name-container {
		text-align: center;
		position: relative;
		padding-bottom: 0.5rem;
	}

	.category-name {
		text-shadow: 0 2px 4px rgba(0, 0, 0, 0.7), 0 0 10px rgba(0, 0, 0, 0.4);
		letter-spacing: 0.5px;
		display: inline-block;
		position: relative;
		transition: all 0.3s ease;
		transform-origin: right;
	}

	.category-underline {
		height: 2px;
		width: 0;
		background: linear-gradient(90deg, transparent, #ffffff, transparent);
		position: absolute;
		bottom: 0;
		right: 0;
		opacity: 0;
		transition: all 0.3s ease;
	}

	.group:hover .category-name {
		transform: translateY(-2px);
		text-shadow: 0 4px 8px rgba(0, 0, 0, 0.8), 0 0 15px rgba(0, 0, 0, 0.5);
	}

	.group:hover .category-underline {
		width: 100%;
		opacity: 0.8;
		animation: shimmer 2s infinite;
	}

	@keyframes shimmer {
		0% {
			background-position: -100% 0;
		}
		100% {
			background-position: 200% 0;
		}
	}
</style>
