<script setup>
	import { RouterLink, useRoute, useRouter } from "vue-router";
	import { useUserStore } from "@/store/userStore";

	const route = useRoute();
	const router = useRouter();
	const userStore = useUserStore();

	const isActive = (path) => route.path === path;

	const handleLogout = () => {
		userStore.logout();
		router.push("/login");
	};
</script>

<template>
	<header>
		<div class="logo" style="font-weight: bolder">
			<RouterLink to="/">Housey</RouterLink>
		</div>
		<nav>
			<ul>
				<li>
					<RouterLink to="/" :class="{ active: isActive('/') }"
						>Home</RouterLink
					>
				</li>
				<li class="logout-button" v-if="userStore.isLoggedIn">
					<button @click="handleLogout">Logout</button>
				</li>
				<li id="username" v-if="userStore.isLoggedIn">
					{{ userStore.username }}
				</li>
				<li v-else>
					<RouterLink to="/login" :class="{ active: isActive('/login') }"
						>Login</RouterLink
					>
				</li>
			</ul>
		</nav>
	</header>
	<hr />
</template>

<style scoped>
	header {
		display: flex;
		justify-content: space-between;
		align-items: center;
		background-color: #2073f0;
		color: #fff;
		padding: 10px 10%;
	}
	ul {
		list-style: none;
		padding: 0;
		margin: 0;
	}
	li {
		display: inline-block;
		margin-right: 10px;
	}

	a,
	button {
		padding: 10px 15px;
		color: #fff;
		text-decoration: none;
		border: none;
		background: none;
		cursor: pointer;
		border-radius: 5px;
		transition: 0.1s ease;
	}
	a:hover,
	button:hover {
		background-color: #022b69;
	}
	.active {
		background-color: #0245aa;
	}

	#username {
		font-size: 18px;
		color: #2c3e50;
		margin-right: 15px;
		background-color: #f0f8ff;
		border-radius: 10px;
		padding: 8px 12px;
		box-shadow: 0 2px 5px rgba(0, 0, 0, 0.15);
		font-weight: 500;
		transition: background-color 0.3s, transform 0.2s;
	}

	#username:hover {
		background-color: #e0f7fa;
		transform: scale(1.05);
	}
</style>
