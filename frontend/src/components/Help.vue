<template>
	<div class="flex items-center justify-center py-12 bg-white animate-fadeIn">
		<div
			class="bg-white rounded-xl shadow-glow p-8 max-w-md w-full text-center relative overflow-hidden mx-auto"
		>
			<div class="static-glow"></div>
			<h1
				class="text-3xl font-extrabold mb-4 text-transparent bg-clip-text bg-gradient-to-r from-blue-700 to-blue-500"
			>
				Coming Soon
			</h1>
			<p class="text-gray-600 text-base mb-6">
				نحن نعمل جاهدين على تطوير هذه الصفحة لتوفير أفضل تجربة لكم. يرجى العودة
				لاحقاً.
			</p>
			<div class="flex justify-center">
				<i
					class="fas fa-tools text-5xl text-transparent bg-clip-text bg-gradient-to-r from-blue-600 to-blue-400 opacity-80 animate-pulse-subtle"
				></i>
			</div>
		</div>
	</div>
</template>

<style scoped>
	.shadow-glow {
		box-shadow: 0 8px 30px rgba(59, 130, 246, 0.15);
	}

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
		opacity: 0.5;
		pointer-events: none;
		z-index: 0;
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

	@keyframes pulse-subtle {
		0%,
		100% {
			opacity: 0.7;
			transform: scale(1);
		}
		50% {
			opacity: 1;
			transform: scale(1.05);
		}
	}

	.animate-pulse-subtle {
		animation: pulse-subtle 3s infinite ease-in-out;
	}
</style>
