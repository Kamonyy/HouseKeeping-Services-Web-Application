<script setup>
	import { RouterLink, useRoute, useRouter } from "vue-router";
	import { useUserStore } from "@/store/userStore";

	const route = useRoute();
	const router = useRouter();
	const userStore = useUserStore();

	const isActive = (path) => route.path === path;

	const handleLogout = () => {
		userStore.logout();
		router.push("/");
	};
</script>

<template>
	<nav class="flex justify-between items-center px-6 py-4 shadow-md rtl">
		<!-- Logo Section -->
		<div class="flex items-center">
			<RouterLink to="/" class="mx-5">
				<img src="@/img/logo.png" alt="Logo" class="h-12 w-auto" />
			</RouterLink>
			<div class="group relative">
				<RouterLink to="/services" class="hover:text-blue-500"
					>خدماتنا</RouterLink
				>
			</div>
		</div>

		<!-- Navigation Links -->
		<div class="flex items-center space-x-6 rtl:space-x-reverse">
			<RouterLink to="#" class="hover:text-blue-500">مساعدة</RouterLink>
			<RouterLink
				@click="handleLogout"
				to="/login"
				v-if="userStore.isLoggedIn"
				class="hover:text-blue-500"
				>تسجيل خروج</RouterLink
			>
			<RouterLink id="un" v-if="userStore.isLoggedIn">
				{{ userStore.username }}
			</RouterLink>
			<RouterLink
				to="/login"
				v-else
				:class="{ active: isActive('/login') }"
				class="hover:text-blue-500"
				>تسجيل دخول</RouterLink
			>
		</div>
	</nav>
</template>

<style scoped>
	nav {
		max-width: 1280px;
		margin: 0 auto;
	}
	#un {
		font-size: 18px;
		color: #505050;
		margin-left: 15px;
		background-color: #ffffff;
		border-radius: 10px;
		padding: 8px 12px;
		box-shadow: 0 2px 5px rgba(0, 0, 0, 0.15);
		font-weight: 500;
		transition: background-color 0.3s, transform 0.2s;
	}

	#un:hover {
		background-color: #e0f7fa;
		transform: scale(1.05);
	}

	.rtl {
		direction: rtl;
	}
</style>
