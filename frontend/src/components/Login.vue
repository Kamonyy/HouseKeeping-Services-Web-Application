<script setup>
	import { ref } from "vue";
	import { useRouter } from "vue-router";
	import axios from "axios";
	import { useUserStore } from "../store/userStore";

	const router = useRouter();
	const userStore = useUserStore();
	const username = ref("");
	const password = ref("");
	const errorMessage = ref("");

	const login = async () => {
		if (username.value && password.value) {
			try {
				const response = await axios.post(
					"https://localhost:7007/api/account/login",
					{
						username: username.value,
						password: password.value,
					}
				);
				console.log("Response from API:", response.data);

				// Store username and token in the userStore
				userStore.setUser(
					response.data.username || username.value,
					response.data.token
				);

				errorMessage.value = "";
				router.push("/");
			} catch (error) {
				if (error.response && error.response.status === 401) {
					errorMessage.value = "Invalid credentials. Please try again.";
				} else {
					errorMessage.value = "An error occurred. Please try again later.";
				}
				console.error("Error during login:", error);
			}
		} else {
			errorMessage.value = "Please enter both username and password.";
		}
	};
</script>

<template>
	<div class="flex items-center justify-center py-12 bg-white animate-fadeIn">
		<form
			@submit.prevent="login"
			class="bg-white rounded-xl shadow-glow p-6 max-w-sm w-full relative overflow-hidden mx-auto"
		>
			<div class="static-glow"></div>
			<h2
				class="text-2xl font-semibold mb-5 text-center text-transparent bg-clip-text bg-gradient-to-r from-blue-700 to-blue-500"
			>
				تسجيل الدخول
			</h2>
			<div class="mb-4">
				<label for="username" class="block text-gray-700 text-sm font-bold mb-2"
					>اسم المستخدم:</label
				>
				<input
					type="text"
					id="username"
					v-model="username"
					required
					class="shadow appearance-none border rounded-lg w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:ring-2 focus:ring-blue-500 transition-all duration-300 border-gray-300"
					placeholder="ادخل اسم المستخدم"
				/>
			</div>
			<div class="mb-5">
				<label for="password" class="block text-gray-700 text-sm font-bold mb-2"
					>كلمة المرور:</label
				>
				<input
					type="password"
					id="password"
					v-model="password"
					required
					class="shadow appearance-none border rounded-lg w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:ring-2 focus:ring-blue-500 transition-all duration-300 border-gray-300"
					placeholder="ادخل كلمة المرور"
				/>
			</div>
			<button
				type="submit"
				class="bg-gradient-to-r from-blue-600 to-blue-500 hover:from-blue-700 hover:to-blue-600 text-white font-bold py-2 px-4 rounded-full focus:outline-none focus:ring-2 focus:ring-blue-500 w-full transition-all duration-300 shadow-md hover:shadow-lg transform hover:scale-[1.02]"
			>
				تسجيل دخول
			</button>
			<div
				v-if="errorMessage"
				class="text-red-500 text-sm mt-4 text-center bg-red-50 p-2 rounded-lg border border-red-200"
			>
				{{ errorMessage }}
			</div>
		</form>
	</div>
</template>

<style scoped>
	.shadow-glow {
		box-shadow: 0 8px 20px rgba(59, 130, 246, 0.15);
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
		opacity: 0.8;
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

	input:focus {
		border-color: #3b82f6;
	}
</style>
