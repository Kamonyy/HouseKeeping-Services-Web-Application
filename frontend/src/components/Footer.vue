<script setup>
	import { RouterLink } from "vue-router";
</script>

<template>
	<footer
		class="bg-gradient-to-b from-gray-800 to-gray-900 text-white py-16 relative overflow-hidden font-cairo"
	>
		<div class="footer-glow"></div>
		<!-- Blue accent bar -->
		<div
			class="absolute top-0 left-0 right-0 h-1 bg-gradient-to-r from-blue-400 to-blue-600"
		></div>
		<div class="container mx-auto px-6 sm:px-8 relative z-10">
			<div class="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-4 gap-8 mb-12">
				<div
					class="flex flex-col items-center md:items-start animate-fadeIn"
					style="animation-delay: 0.1s"
				>
					<div class="flex items-center flex-col">
						<img
							src="@/img/logo.png"
							alt="House Keeping Logo"
							class="h-40 mb-4 transition-all duration-300 hover:scale-105 drop-shadow-glow"
						/>
						<div class="flex justify-center space-x-4 rtl:space-x-reverse">
							<a
								href="#"
								class="text-gray-400 hover:text-blue-400 transition-all duration-200 transform hover:scale-110"
							>
								<i class="fab fa-facebook-f"></i>
							</a>
							<a
								href="#"
								class="text-gray-400 hover:text-blue-400 transition-all duration-200 transform hover:scale-110"
							>
								<i class="fab fa-twitter"></i>
							</a>
							<a
								href="#"
								class="text-gray-400 hover:text-blue-400 transition-all duration-200 transform hover:scale-110"
							>
								<i class="fab fa-instagram"></i>
							</a>
						</div>
					</div>
					<p class="text-gray-400 text-sm mt-4 text-center md:text-right">
						العناية بمنزلك بأيدٍ محترفة
					</p>
				</div>

				<div
					class="flex flex-col rtl animate-fadeIn"
					style="animation-delay: 0.2s"
				>
					<h3
						class="text-lg font-semibold mb-4 text-transparent bg-clip-text bg-gradient-to-r from-blue-400 to-blue-300"
					>
						خدماتنا
					</h3>
					<RouterLink
						to="/services"
						class="text-gray-400 hover:text-blue-400 transition-all duration-200 mb-2 relative group"
						>تنظيف
						<span
							class="absolute -bottom-1 right-0 w-0 h-0.5 bg-blue-500 group-hover:w-full transition-all duration-200"
						></span>
					</RouterLink>
					<RouterLink
						to="/services"
						class="text-gray-400 hover:text-blue-400 transition-all duration-200 mb-2 relative group"
						>صيانة
						<span
							class="absolute -bottom-1 right-0 w-0 h-0.5 bg-blue-500 group-hover:w-full transition-all duration-200"
						></span>
					</RouterLink>
					<RouterLink
						to="/services"
						class="text-gray-400 hover:text-blue-400 transition-all duration-200 mb-2 relative group"
						>سباكة
						<span
							class="absolute -bottom-1 right-0 w-0 h-0.5 bg-blue-500 group-hover:w-full transition-all duration-200"
						></span>
					</RouterLink>
					<RouterLink
						to="/services"
						class="text-gray-400 hover:text-blue-400 transition-all duration-200 relative group"
						>الكهرباء
						<span
							class="absolute -bottom-1 right-0 w-0 h-0.5 bg-blue-500 group-hover:w-full transition-all duration-200"
						></span>
					</RouterLink>
				</div>

				<div
					class="flex flex-col rtl animate-fadeIn"
					style="animation-delay: 0.3s"
				>
					<h3
						class="text-lg font-semibold mb-4 text-transparent bg-clip-text bg-gradient-to-r from-blue-400 to-blue-300"
					>
						روابط سريعة
					</h3>
					<RouterLink
						to="/"
						class="text-gray-400 hover:text-blue-400 transition-all duration-200 mb-2 relative group"
						>الرئيسية
						<span
							class="absolute -bottom-1 right-0 w-0 h-0.5 bg-blue-500 group-hover:w-full transition-all duration-200"
						></span>
					</RouterLink>
					<RouterLink
						to="/help"
						class="text-gray-400 hover:text-blue-400 transition-all duration-200 mb-2 relative group"
						>مساعدة
						<span
							class="absolute -bottom-1 right-0 w-0 h-0.5 bg-blue-500 group-hover:w-full transition-all duration-200"
						></span>
					</RouterLink>
					<RouterLink
						to="/login"
						class="text-gray-400 hover:text-blue-400 transition-all duration-200 mb-2 relative group"
						>تسجيل الدخول
						<span
							class="absolute -bottom-1 right-0 w-0 h-0.5 bg-blue-500 group-hover:w-full transition-all duration-200"
						></span>
					</RouterLink>
					<RouterLink
						to="/register"
						class="text-gray-400 hover:text-blue-400 transition-all duration-200 relative group"
						>إنشاء حساب
						<span
							class="absolute -bottom-1 right-0 w-0 h-0.5 bg-blue-500 group-hover:w-full transition-all duration-200"
						></span>
					</RouterLink>
				</div>

				<div
					class="flex flex-col rtl animate-fadeIn"
					style="animation-delay: 0.4s"
				>
					<h3
						class="text-lg font-semibold mb-4 text-transparent bg-clip-text bg-gradient-to-r from-blue-400 to-blue-300"
					>
						المواقع
					</h3>
					<RouterLink
						to=""
						class="text-gray-400 hover:text-blue-400 transition-all duration-200 mb-2 relative group"
						>بغداد
						<span
							class="absolute -bottom-1 right-0 w-0 h-0.5 bg-blue-500 group-hover:w-full transition-all duration-200"
						></span>
					</RouterLink>
					<RouterLink
						to=""
						class="text-gray-400 hover:text-blue-400 transition-all duration-200 mb-2 relative group"
						>اربيل
						<span
							class="absolute -bottom-1 right-0 w-0 h-0.5 bg-blue-500 group-hover:w-full transition-all duration-200"
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
	/* Import Cairo font - perfect for Arabic */
	@import url("https://fonts.googleapis.com/css2?family=Cairo:wght@200;300;400;500;600;700;800;900&display=swap");

	.rtl {
		direction: rtl;
		text-align: right;
	}

	.font-cairo {
		font-family: "Cairo", sans-serif;
	}

	.footer-glow {
		position: absolute;
		top: -50%;
		left: -50%;
		width: 200%;
		height: 200%;
		background: radial-gradient(
			circle,
			rgba(59, 130, 246, 0.15) 0%,
			transparent 70%
		);
		opacity: 0.4;
		transform: rotate(0deg);
		animation: footerGlow 20s cubic-bezier(0.445, 0.05, 0.55, 0.95) infinite;
		pointer-events: none;
	}

	.drop-shadow-glow {
		filter: drop-shadow(0 0 12px rgba(59, 130, 246, 0.5));
	}

	@keyframes footerGlow {
		0% {
			transform: rotate(0deg);
		}
		50% {
			transform: rotate(180deg) scale(1.05);
		}
		100% {
			transform: rotate(360deg);
		}
	}

	@keyframes fadeIn {
		from {
			opacity: 0;
			transform: translateY(15px);
		}
		to {
			opacity: 1;
			transform: translateY(0);
		}
	}

	.animate-fadeIn {
		animation: fadeIn 0.5s cubic-bezier(0.34, 1.56, 0.64, 1) forwards;
	}

	/* Arabic font optimization */
	h3,
	a,
	p {
		letter-spacing: -0.01em;
		line-height: 1.6;
	}

	/* Enhanced hover effect for links */
	.group:hover span {
		width: 100%;
		transform: scaleX(1);
	}
</style>
