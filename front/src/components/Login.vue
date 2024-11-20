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
	<form @submit.prevent="login">
		<div class="form-group">
			<label for="username">Username:</label>
			<input type="text" id="username" v-model="username" required />
		</div>
		<div class="form-group">
			<label for="password">Password:</label>
			<input type="password" id="password" v-model="password" required />
		</div>
		<button type="submit">Login</button>
		<button type="button" @click="logout" v-if="isLoggedIn">Logout</button>
		<div v-if="errorMessage" class="error-message">{{ errorMessage }}</div>
	</form>
</template>

<style scoped>
	form {
		background-color: #f9f9f9;
		border-radius: 8px;
		padding: 20px;
		box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
		max-width: 400px;
		margin: 50px auto;
	}

	.form-group {
		margin-bottom: 15px;
	}

	label {
		color: black;
		display: block;
		margin-bottom: 5px;
		font-weight: bold;
	}

	input {
		width: 100%;
		padding: 10px;
		border: 1px solid #ccc;
		border-radius: 4px;
	}

	button {
		background-color: #007bff;
		color: white;
		border: none;
		padding: 10px;
		border-radius: 4px;
		cursor: pointer;
		width: 100%;
	}

	button:hover {
		background-color: #0056b3;
	}

	.error-message {
		color: red;
		margin-top: 10px;
	}
</style>
