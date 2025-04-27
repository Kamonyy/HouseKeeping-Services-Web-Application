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
		:to="'/service/' + service.id'"
		class="block bg-gradient-to-b from-white to-gray-50 rounded-lg overflow-hidden text-left transform transition-all duration-300 hover:shadow-glow hover:scale-105 shadow-md border border-gray-200 hover:border-blue-500 flex flex-col animate-fadeIn"
	>
		<div class="relative overflow-hidden group">
			<img
				src="@/img/service.jpg"
				:alt="service.title"
				class="w-full h-48 object-cover rounded-t-lg transition-transform duration-500 group-hover:scale-110"
			/>
			<div
				class="absolute inset-0 bg-gradient-to-t from-gray-900/80 to-black/20 rounded-t-lg flex items-center justify-start pl-4 transition-opacity duration-300"
			>
				<h3 class="text-white text-xl font-semibold drop-shadow-glow">
					{{ service.title }}
				</h3>
			</div>
		</div>
		<div
			class="p-4 flex-grow flex flex-col justify-between relative overflow-hidden"
		>
			<div>
				<p
					class="text-lg font-semibold text-transparent bg-clip-text bg-gradient-to-r from-blue-600 to-blue-400 mb-2"
				>
					{{ service.title }}
				</p>
				<p class="text-sm text-gray-600 mb-1">
					بواسطة:
					<span class="font-medium text-blue-500">{{ service.username }}</span>
				</p>
				<p class="text-sm text-gray-700">
					{{ service.description }}
				</p>
			</div>
			<div class="mt-2">
				<span
					class="inline-block bg-gradient-to-r from-blue-200 to-blue-100 text-blue-700 px-3 py-1 rounded-full text-xs font-semibold shadow-sm transition-all duration-300 hover:shadow-md"
				>
					{{ service.category }}
				</span>
			</div>
			<div class="static-glow"></div>
		</div>
	</RouterLink>
</template>

<style scoped>
	.shadow-glow {
		box-shadow: 0 8px 20px rgba(59, 130, 246, 0.3);
	}

	.drop-shadow-glow {
		filter: drop-shadow(0 0 6px rgba(59, 130, 246, 0.5));
	}

	.static-glow {
		position: absolute;
		top: 0;
		left: 0;
		width: 100%;
		height: 100%;
		background: radial-gradient(
			circle at center,
			rgba(59, 130, 246, 0.1) 0%,
			transparent 70%
		);
		opacity: 0;
		pointer-events: none;
		z-index: 0;
		transition: opacity 0.5s ease;
	}

	a:hover .static-glow {
		opacity: 0.7;
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
