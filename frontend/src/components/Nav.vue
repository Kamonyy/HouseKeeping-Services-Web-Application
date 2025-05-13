<template>
	<div class="sticky top-0 z-50 bg-white border-b border-gray-100 font-cairo">
		<nav
			class="flex justify-between items-center py-4 px-6 rtl container mx-auto"
		>
			<div class="flex items-center gap-8">
				<RouterLink to="/" class="group">
					<img
						src="@/img/logo.png"
						alt="Logo"
						class="h-12 w-auto transition-all duration-300 group-hover:scale-105 drop-shadow-sm"
					/>
				</RouterLink>
				<div class="hidden md:flex gap-6">
					<div class="nav-link">
						<RouterLink to="/services" class="nav-item">خدماتنا</RouterLink>
					</div>

					<!-- Provider Management Link (visible only to providers) -->
					<div class="nav-link" v-if="userStore.isProvider">
						<RouterLink to="/provider-management" class="nav-item"
							>إدارة الخدمات</RouterLink
						>
					</div>

					<!-- Admin Approval Link (visible only to admins) -->
					<div class="nav-link" v-if="userStore.isAdmin">
						<RouterLink to="/admin/dashboard" class="nav-item"
							>لوحة التحكم</RouterLink
						>
					</div>

					<div class="nav-link">
						<RouterLink to="/help" class="nav-item">مساعدة</RouterLink>
					</div>
				</div>
			</div>

			<div class="flex items-center gap-4">
				<!-- Mobile menu button -->
				<button @click="toggleMobileMenu" class="md:hidden flex items-center">
					<i class="fas fa-bars text-gray-700 text-xl"></i>
				</button>

				<div class="hidden md:flex items-center gap-4">
					<div class="nav-link" v-if="userStore.isLoggedIn">
						<button @click="handleLogout" class="nav-item">تسجيل خروج</button>
					</div>
					<div v-if="userStore.isLoggedIn" class="relative">
						<span
							id="username-badge"
							class="font-medium text-black bg-gradient-to-r from-primary to-primary-light rounded-full px-4 py-1.5 shadow-md transition-all duration-300 hover:shadow-lg flex items-center gap-2"
						>
							<i class="fas fa-user-circle"></i>
							{{ userStore.displayName }}
						</span>
						<div v-if="showSessionInfo" class="session-info">
							<div class="p-4 border-b border-gray-100">
								<div class="flex items-center gap-3">
									<div
										class="w-10 h-10 rounded-full bg-blue-100 flex items-center justify-center"
									>
										<i class="fas fa-user text-primary"></i>
									</div>
									<div>
										<div class="font-medium text-gray-900">
											{{ userStore.firstName || userStore.username }}
											{{ userStore.lastName }}
										</div>
										<div class="text-sm text-gray-500">
											@{{ userStore.username }}
										</div>
									</div>
								</div>
							</div>
							<div class="p-3">
								<div class="text-sm font-medium text-gray-600">
									الجلسة تنتهي في:
								</div>
								<div class="text-primary font-medium mt-1">
									{{ formattedSessionTime }}
								</div>
							</div>
						</div>
					</div>
					<div class="nav-link" v-else>
						<div class="flex items-center gap-3">
							<RouterLink
								to="/login"
								class="auth-btn login-btn flex items-center gap-2"
							>
								<i class="fas fa-sign-in-alt"></i>
								تسجيل دخول
							</RouterLink>
							<RouterLink
								to="/register"
								class="auth-btn signup-btn flex items-center gap-2"
							>
								<i class="fas fa-user-plus"></i>
								إنشاء حساب
							</RouterLink>
						</div>
					</div>
				</div>
			</div>
		</nav>

		<!-- Mobile menu -->
		<div
			v-if="mobileMenuOpen"
			class="md:hidden bg-white border-t border-gray-100 shadow-lg animate-slideDown"
		>
			<div class="container mx-auto px-6 py-4">
				<div class="flex flex-col gap-4 rtl">
					<!-- User info (if logged in) -->
					<div
						v-if="userStore.isLoggedIn"
						class="flex items-center gap-3 p-3 bg-blue-50 rounded-lg mb-2"
					>
						<div
							class="w-10 h-10 rounded-full bg-blue-100 flex items-center justify-center"
						>
							<i class="fas fa-user text-primary"></i>
						</div>
						<div>
							<div class="font-medium text-gray-900">
								{{ userStore.firstName || userStore.username }}
								{{ userStore.lastName }}
							</div>
							<div class="text-xs text-gray-500">@{{ userStore.username }}</div>
						</div>
					</div>

					<RouterLink
						@click="closeMobileMenu"
						to="/services"
						class="mobile-nav-item"
					>
						<i class="fas fa-list-ul w-6"></i>
						خدماتنا
					</RouterLink>

					<RouterLink
						v-if="userStore.isProvider"
						@click="closeMobileMenu"
						to="/provider-management"
						class="mobile-nav-item"
					>
						<i class="fas fa-cog w-6"></i>
						إدارة الخدمات
					</RouterLink>

					<RouterLink
						v-if="userStore.isAdmin"
						@click="closeMobileMenu"
						to="/admin/dashboard"
						class="mobile-nav-item"
					>
						<i class="fas fa-tachometer-alt w-6"></i>
						لوحة التحكم
					</RouterLink>

					<RouterLink
						@click="closeMobileMenu"
						to="/help"
						class="mobile-nav-item"
					>
						<i class="fas fa-question-circle w-6"></i>
						مساعدة
					</RouterLink>

					<div class="border-t border-gray-100 my-2"></div>

					<RouterLink
						v-if="!userStore.isLoggedIn"
						@click="closeMobileMenu"
						to="/login"
						class="mobile-nav-item text-primary"
					>
						<i class="fas fa-sign-in-alt w-6"></i>
						تسجيل دخول
					</RouterLink>

					<RouterLink
						v-if="!userStore.isLoggedIn"
						@click="closeMobileMenu"
						to="/register"
						class="mobile-nav-item text-secondary"
					>
						<i class="fas fa-user-plus w-6"></i>
						إنشاء حساب
					</RouterLink>

					<button
						v-else
						@click="handleLogoutMobile"
						class="mobile-nav-item text-red-600"
					>
						<i class="fas fa-sign-out-alt w-6"></i>
						تسجيل خروج
					</button>
				</div>
			</div>
		</div>
	</div>
</template>

<script>
	import { RouterLink, useRoute, useRouter } from "vue-router";
	import { useUserStore } from "@/store/userStore";
	import { ref, computed, onMounted, onUnmounted } from "vue";
	import { defineComponent } from "vue";

	export default defineComponent({
		components: {
			RouterLink,
		},
		setup() {
			const route = useRoute();
			const router = useRouter();
			const userStore = useUserStore();
			const showSessionInfo = ref(false);
			const sessionTimer = ref(null);
			const formattedSessionTime = ref("");
			const mobileMenuOpen = ref(false);

			// Format remaining session time
			const updateSessionTime = () => {
				if (userStore.isLoggedIn) {
					const timeLeft = userStore.tokenExpiresIn;
					const hours = Math.floor(timeLeft / (1000 * 60 * 60));
					const minutes = Math.floor(
						(timeLeft % (1000 * 60 * 60)) / (1000 * 60)
					);

					formattedSessionTime.value = `${hours} ساعة ${minutes} دقيقة`;
					showSessionInfo.value = true;
				} else {
					showSessionInfo.value = false;
				}
			};

			// Update session time every minute
			onMounted(() => {
				updateSessionTime();
				sessionTimer.value = setInterval(updateSessionTime, 60000);
			});

			onUnmounted(() => {
				if (sessionTimer.value) {
					clearInterval(sessionTimer.value);
				}
			});

			const handleLogout = () => {
				userStore.logout();
				router.push("/");
			};

			const handleLogoutMobile = () => {
				closeMobileMenu();
				userStore.logout();
				router.push("/");
			};

			const toggleMobileMenu = () => {
				mobileMenuOpen.value = !mobileMenuOpen.value;
			};

			const closeMobileMenu = () => {
				mobileMenuOpen.value = false;
			};

			return {
				route,
				router,
				userStore,
				handleLogout,
				handleLogoutMobile,
				showSessionInfo,
				formattedSessionTime,
				mobileMenuOpen,
				toggleMobileMenu,
				closeMobileMenu,
			};
		},
	});
</script>

<style scoped>
	/* Import Cairo font - perfect for Arabic */
	@import url("https://fonts.googleapis.com/css2?family=Cairo:wght@200;300;400;500;600;700;800;900&display=swap");

	.rtl {
		direction: rtl;
		text-align: right;
	}

	.font-cairo {
		font-family: "Cairo", sans-serif;
	}

	/* Add nav bar gradient and shadow */
	.sticky {
		box-shadow: 0 4px 20px rgba(0, 0, 0, 0.08);
		background: linear-gradient(to bottom, #ffffff, #f8fafc);
		animation: navReveal 0.5s ease-out forwards;
	}

	@keyframes navReveal {
		from {
			transform: translateY(-100%);
			opacity: 0;
		}
		to {
			transform: translateY(0);
			opacity: 1;
		}
	}

	.nav-link {
		position: relative;
	}

	.nav-item {
		font-weight: 600;
		color: var(--text-dark, #1e293b);
		position: relative;
		padding: 0.5rem 0;
		transition: all 0.3s ease;
		text-shadow: 0 1px 0 rgba(255, 255, 255, 0.8);
		letter-spacing: 0.01em;
	}

	.nav-item:hover {
		color: var(--primary-color, #3b82f6);
	}

	.nav-item::after {
		content: "";
		position: absolute;
		width: 0;
		height: 2.5px;
		bottom: 0;
		left: 0;
		background-color: var(--primary-color, #3b82f6);
		transition: width 0.3s ease;
	}

	.nav-item:hover::after {
		width: 100%;
	}

	.router-link-active.nav-item {
		color: var(--primary-color, #3b82f6);
		font-weight: 700;
	}

	.router-link-active.nav-item::after {
		width: 100%;
		background-color: var(--primary-color, #3b82f6);
	}

	/* Auth buttons styling */
	.auth-btn {
		padding: 0.5rem 1.25rem;
		border-radius: 9999px;
		font-weight: 700;
		transition: all 0.3s ease;
		box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
		letter-spacing: 0.02em;
	}

	.login-btn {
		background: var(--primary-color, #3b82f6);
		color: white;
		border: 2px solid var(--primary-color, #3b82f6);
	}

	.login-btn:hover {
		background: var(--primary-dark, #2563eb);
		transform: translateY(-2px);
		box-shadow: 0 4px 8px rgba(59, 130, 246, 0.3);
	}

	.signup-btn {
		background: white;
		color: var(--secondary-color, #f59e0b);
		border: 2px solid var(--secondary-color, #f59e0b);
	}

	.signup-btn:hover {
		background: var(--secondary-light, #fef3c7);
		transform: translateY(-2px);
		box-shadow: 0 4px 8px rgba(245, 158, 11, 0.2);
	}

	.session-info {
		position: absolute;
		top: 120%;
		right: 0;
		min-width: 240px;
		background-color: white;
		border-radius: 12px;
		box-shadow: 0 10px 25px -5px rgba(0, 0, 0, 0.1),
			0 10px 10px -5px rgba(0, 0, 0, 0.04);
		border: 1px solid #e5e7eb;
		opacity: 0;
		visibility: hidden;
		transform: translateY(10px);
		transition: all 0.3s ease;
		z-index: 100;
	}

	#username-badge:hover + .session-info,
	.session-info:hover {
		opacity: 1;
		visibility: visible;
		transform: translateY(0);
	}

	.mobile-nav-item {
		display: flex;
		align-items: center;
		gap: 0.5rem;
		padding: 0.75rem;
		border-radius: 0.5rem;
		font-weight: 600;
		color: var(--text-dark, #1e293b);
		transition: all 0.2s ease;
		letter-spacing: 0.01em;
	}

	.mobile-nav-item:hover {
		background-color: #f1f5f9;
		color: var(--primary-color, #3b82f6);
	}

	.mobile-nav-item.router-link-active {
		background-color: #ebf5ff;
		color: var(--primary-color, #3b82f6);
		font-weight: 700;
	}

	.drop-shadow-sm {
		filter: drop-shadow(0 1px 2px rgba(0, 0, 0, 0.1));
	}

	@keyframes slideDown {
		from {
			opacity: 0;
			transform: translateY(-10px);
		}
		to {
			opacity: 1;
			transform: translateY(0);
		}
	}

	.animate-slideDown {
		animation: slideDown 0.3s ease forwards;
	}

	/* Arabic font optimization */
	.nav-item,
	.mobile-nav-item,
	button,
	a {
		letter-spacing: -0.01em;
		line-height: 1.6;
	}

	/* CSS Variables for consistent colors */
	:root {
		--primary-color: #2563eb;
		--primary-light: #3b82f6;
		--primary-dark: #1d4ed8;
		--secondary-color: #f59e0b;
		--secondary-light: #fef3c7;
		--text-dark: #1e293b;
		--text-medium: #475569;
		--text-light: #64748b;
	}
</style>
