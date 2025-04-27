<template>
	<nav
		class="flex justify-between items-center px-6 py-4 rtl bg-gradient-to-r from-gray-900/90 to-gray-800/95 backdrop-blur-md sticky top-0 z-10 border-b border-blue-900/30 shadow-lg animate-fadeIn"
	>
		<div class="flex items-center gap-6">
			<RouterLink to="/" class="group">
				<img
					src="@/img/logo.png"
					alt="Logo"
					class="h-14 w-auto transition-all duration-300 group-hover:scale-110 group-hover:rotate-3 drop-shadow-lg group-hover:drop-shadow-glow"
				/>
			</RouterLink>
			<div class="link-container">
				<RouterLink :class="linkStyle('/services')" to="/services"
					>خدماتنا</RouterLink
				>
			</div>
		</div>

		<div class="flex items-center gap-5 rtl:space-x-reverse">
			<div class="link-container">
				<RouterLink :class="linkStyle('/help')" to="/help"> مساعدة </RouterLink>
			</div>
			<div class="link-container" v-if="userStore.isLoggedIn">
				<RouterLink
					@click="handleLogout"
					to="/login"
					:class="linkStyle('/login')"
				>
					تسجيل خروج
				</RouterLink>
			</div>
			<span
				v-if="userStore.isLoggedIn"
				id="un"
				class="font-semibold text-white bg-gradient-to-r from-blue-600/90 to-blue-500/90 border border-blue-700/50 rounded-full px-5 py-2 shadow-glow transition-all duration-300 hover:scale-105 hover:from-blue-500/90 hover:to-blue-400/90 backdrop-blur-md animate-pulse-subtle"
			>
				{{ userStore.username }}
			</span>
			<div class="link-container" v-else>
				<RouterLink to="/login" :class="linkStyle('/login')">
					تسجيل دخول
				</RouterLink>
			</div>
		</div>
	</nav>
</template>

<script>
	import { RouterLink, useRoute, useRouter } from "vue-router";
	import { useUserStore } from "@/store/userStore";
	import { ref, computed } from "vue";
	import { defineComponent } from "vue";

	export default defineComponent({
		components: {
			RouterLink,
		},
		setup() {
			const route = useRoute();
			const router = useRouter();
			const userStore = useUserStore();

			const handleLogout = () => {
				userStore.logout();
				router.push("/");
			};

			// Computed property for dynamic link styles
			const linkStyle = computed(() => {
				return (path) => {
					return [
						"px-4", // Reduced padding
						"py-2", // Reduced padding
						"rounded-full", // Fully rounded corners
						"transition-all",
						"duration-300",
						"font-semibold", // Use a semi-bold font weight
						"text-white", // Default text color
						"focus:outline-none",
						"focus:ring-2",
						"focus:ring-blue-500/50",
						"focus:ring-offset-2",
						"bg-transparent", // Make the background transparent
						"shadow-none",
						"hover:bg-white/10", // Add a light background on hover
						"hover:text-blue-300",
						"block", // Ensure block display for proper hover area
						userStore.isLoggedIn && route.path === path
							? "bg-gradient-to-r from-blue-600/90 to-blue-500/80 text-white shadow-glow"
							: "", // Active state
						!userStore.isLoggedIn && route.path === path
							? "bg-gradient-to-r from-blue-600/90 to-blue-500/80 text-white shadow-glow"
							: "",
					];
				};
			});

			return { route, router, userStore, handleLogout, linkStyle };
		},
	});
</script>

<style scoped>
	nav {
		max-width: 1280px;
		margin: 0 auto;
		position: relative;
		overflow: hidden;
	}

	nav::before {
		content: "";
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
		opacity: 0.5;
		transform: rotate(0deg);
		animation: navGlow 15s linear infinite;
		pointer-events: none;
	}

	#un {
		box-shadow: 0 4px 15px rgba(59, 130, 246, 0.4);
		position: relative;
		overflow: hidden;
	}

	#un::after {
		content: "";
		position: absolute;
		top: -50%;
		left: -50%;
		width: 200%;
		height: 200%;
		background: linear-gradient(
			90deg,
			transparent,
			rgba(255, 255, 255, 0.2),
			transparent
		);
		transform: rotate(25deg);
		animation: shimmer 3s infinite;
	}

	.shadow-glow {
		box-shadow: 0 4px 15px rgba(59, 130, 246, 0.3);
	}

	.drop-shadow-glow {
		filter: drop-shadow(0 0 8px rgba(59, 130, 246, 0.5));
	}

	/* Link container with hover effect */
	.link-container {
		position: relative;
		display: inline-block;
	}

	.link-container a {
		position: relative;
		z-index: 1;
	}

	.link-container::after {
		content: "";
		position: absolute;
		bottom: -2px;
		left: 50%;
		width: 0;
		height: 2px;
		background-color: #3b82f6;
		transition: all 0.3s ease;
		transform: translateX(-50%);
		border-radius: 4px;
	}

	.link-container:hover::after {
		width: 80%;
	}

	@keyframes navGlow {
		0% {
			transform: rotate(0deg);
		}
		100% {
			transform: rotate(360deg);
		}
	}

	@keyframes shimmer {
		0% {
			transform: translateX(-150%) rotate(25deg);
		}
		100% {
			transform: translateX(150%) rotate(25deg);
		}
	}

	@keyframes pulse-subtle {
		0%,
		100% {
			box-shadow: 0 0 15px rgba(59, 130, 246, 0.4);
		}
		50% {
			box-shadow: 0 0 25px rgba(59, 130, 246, 0.6);
		}
	}

	.animate-pulse-subtle {
		animation: pulse-subtle 3s infinite ease-in-out;
	}

	.animate-fadeIn {
		animation: fadeIn 0.8s ease-out;
	}

	@keyframes fadeIn {
		from {
			opacity: 0;
			transform: translateY(-10px);
		}
		to {
			opacity: 1;
			transform: translateY(0);
		}
	}
</style>
