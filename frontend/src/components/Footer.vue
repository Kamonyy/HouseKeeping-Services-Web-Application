<script setup>
	// No specific logic needed for now
</script>

<template>
	<footer
		class="bg-gradient-to-b from-gray-900 to-gray-800 text-white py-12 relative overflow-hidden"
	>
		<div class="footer-glow"></div>
		<div class="container mx-auto px-6 sm:px-8 relative z-10">
			<div class="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-4 gap-8 mb-12">
				<div
					class="flex flex-col items-center md:items-start animate-fadeIn"
					style="animation-delay: 0.1s"
				>
					<img
						src="@/img/logo.png"
						alt="Logo"
						class="h-40 mb-4 transition-all duration-300 hover:scale-105 drop-shadow-glow"
					/>
					<p class="text-gray-400 text-sm text-center md:text-left">
						العناية بمنزلك بأيدٍ محترفة
					</p>
					<div class="flex justify-center mt-4 space-x-4">
						<a
							href="#"
							class="text-gray-400 hover:text-blue-500 transition-all duration-300 transform hover:scale-110"
						>
							<i class="fab fa-facebook-f text-xl"></i>
						</a>
						<a
							href="#"
							class="text-gray-400 hover:text-blue-500 transition-all duration-300 transform hover:scale-110"
						>
							<i class="fab fa-twitter text-xl"></i>
						</a>
						<a
							href="#"
							class="text-gray-400 hover:text-blue-500 transition-all duration-300 transform hover:scale-110"
						>
							<i class="fab fa-instagram text-xl"></i>
						</a>
					</div>
				</div>

				<div class="flex flex-col animate-fadeIn" style="animation-delay: 0.2s">
					<h3
						class="text-lg font-semibold mb-4 text-transparent bg-clip-text bg-gradient-to-r from-blue-400 to-blue-300"
					>
						خدماتنا
					</h3>
					<RouterLink
						to=""
						class="text-gray-400 hover:text-blue-500 transition-all duration-300 mb-2 relative group"
						>تنظيف
						<span
							class="absolute -bottom-1 left-0 w-0 h-0.5 bg-blue-500 group-hover:w-full transition-all duration-300"
						></span>
					</RouterLink>
					<RouterLink
						to=""
						class="text-gray-400 hover:text-blue-500 transition-all duration-300 mb-2 relative group"
						>صيانة
						<span
							class="absolute -bottom-1 left-0 w-0 h-0.5 bg-blue-500 group-hover:w-full transition-all duration-300"
						></span>
					</RouterLink>
					<RouterLink
						to=""
						class="text-gray-400 hover:text-blue-500 transition-all duration-300 mb-2 relative group"
						>سباكة
						<span
							class="absolute -bottom-1 left-0 w-0 h-0.5 bg-blue-500 group-hover:w-full transition-all duration-300"
						></span>
					</RouterLink>
					<RouterLink
						to=""
						class="text-gray-400 hover:text-blue-500 transition-all duration-300 relative group"
						>الكهرباء
						<span
							class="absolute -bottom-1 left-0 w-0 h-0.5 bg-blue-500 group-hover:w-full transition-all duration-300"
						></span>
					</RouterLink>
				</div>

				<div class="flex flex-col animate-fadeIn" style="animation-delay: 0.3s">
					<h3
						class="text-lg font-semibold mb-4 text-transparent bg-clip-text bg-gradient-to-r from-blue-400 to-blue-300"
					>
						المواقع
					</h3>
					<RouterLink
						to=""
						class="text-gray-400 hover:text-blue-500 transition-all duration-300 mb-2 relative group"
						>بغداد
						<span
							class="absolute -bottom-1 left-0 w-0 h-0.5 bg-blue-500 group-hover:w-full transition-all duration-300"
						></span>
					</RouterLink>
					<RouterLink
						to=""
						class="text-gray-400 hover:text-blue-500 transition-all duration-300 mb-2 relative group"
						>اربيل
						<span
							class="absolute -bottom-1 left-0 w-0 h-0.5 bg-blue-500 group-hover:w-full transition-all duration-300"
						></span>
					</RouterLink>
				</div>
			</div>

			<div class="text-center border-t border-gray-700/50 pt-6">
				<p class="text-gray-400 text-sm">
					&copy; {{ new Date().getFullYear() }} بيتك.
					<span
						class="text-transparent bg-clip-text bg-gradient-to-r from-blue-400 to-blue-300"
						>جميع الحقوق محفوظة.</span
					>
				</p>
			</div>
		</div>
	</footer>
</template>

<style scoped>
	/* Importing icons (Font Awesome or any other icon set) */
	@import url("https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0-beta3/css/all.min.css");

	.footer-glow {
		position: absolute;
		top: -50%;
		left: -50%;
		width: 200%;
		height: 200%;
		background: radial-gradient(
			circle,
			rgba(59, 130, 246, 0.1) 0%,
			transparent 70%
		);
		opacity: 0.3;
		transform: rotate(0deg);
		animation: footerGlow 15s linear infinite;
		pointer-events: none;
	}

	.drop-shadow-glow {
		filter: drop-shadow(0 0 8px rgba(59, 130, 246, 0.5));
	}

	@keyframes footerGlow {
		0% {
			transform: rotate(0deg);
		}
		100% {
			transform: rotate(360deg);
		}
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
		animation: fadeIn 0.8s ease-out forwards;
	}
</style>
