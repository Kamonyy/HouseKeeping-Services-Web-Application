<script setup>
	import { ref } from "vue";
	import { useRouter } from "vue-router";
	import axios from "axios";
	import { useUserStore } from "../store/userStore";
	import { extractAuthFromResponse } from "../utils/apiUtils";
	import { RouterLink } from "vue-router";

	const router = useRouter();
	const userStore = useUserStore();
	const username = ref("");
	const password = ref("");
	const rememberMe = ref(true);
	const errorMessage = ref("");
	const isLoading = ref(false);

	const login = async () => {
		if (username.value && password.value) {
			try {
				isLoading.value = true;
				const response = await axios.post("/api/account/login", {
					username: username.value,
					password: password.value,
				});
				console.log("Response from API:", response.data);

				// Handle different response formats and extract token
				if (response.data) {
					const {
						username: extractedUsername,
						token,
						firstName,
						lastName,
					} = extractAuthFromResponse(response.data, username.value);

					if (!token) {
						console.error("Could not find token in response:", response.data);
						errorMessage.value =
							"استجابة غير صالحة من الخادم. الرجاء المحاولة مرة أخرى";
						return;
					}

					// Store user data in the userStore with rememberMe option
					userStore.setUser(
						extractedUsername,
						token,
						rememberMe.value,
						firstName,
						lastName
					);
					errorMessage.value = "";
					router.push("/");
				} else {
					errorMessage.value =
						"استجابة غير صالحة من الخادم. الرجاء المحاولة مرة أخرى";
				}
			} catch (error) {
				if (error.response && error.response.status === 401) {
					errorMessage.value =
						"بيانات الدخول غير صحيحة. الرجاء المحاولة مرة أخرى";
				} else {
					errorMessage.value = "حدث خطأ. الرجاء المحاولة مرة أخرى لاحقاً";
				}
				console.error("Error during login:", error);
			} finally {
				isLoading.value = false;
			}
		} else {
			errorMessage.value = "الرجاء إدخال اسم المستخدم وكلمة المرور";
		}
	};
</script>

<template>
	<div class="min-h-screen animate-fadeIn">
		<!-- Hero Section -->
		<section
			class="relative bg-gradient-to-b from-blue-50 to-white py-16 overflow-hidden"
		>
			<!-- Background decorations -->
			<div class="absolute top-0 left-0 w-full h-full overflow-hidden z-0">
				<div class="bg-pattern opacity-5 absolute inset-0"></div>
				<div
					class="absolute top-20 left-20 w-64 h-64 bg-blue-100 rounded-full filter blur-3xl opacity-30"
				></div>
				<div
					class="absolute bottom-10 right-10 w-80 h-80 bg-blue-200 rounded-full filter blur-3xl opacity-30"
				></div>
			</div>

			<div class="container mx-auto px-4 relative z-10">
				<div class="flex flex-col lg:flex-row items-center gap-12">
					<!-- Left side: Login form -->
					<div class="w-full lg:w-1/2 lg:order-2">
						<div class="max-w-md mx-auto">
							<div
								class="bg-white shadow-2xl rounded-2xl p-8 border border-gray-100"
							>
								<div class="text-center mb-8">
									<div
										class="inline-block bg-blue-100 text-blue-700 px-4 py-2 rounded-full text-sm font-medium mb-4"
									>
										مرحباً بك مجدداً
									</div>
									<h2
										class="text-3xl font-bold text-transparent bg-clip-text bg-gradient-to-r from-blue-700 to-blue-500 mb-2"
									>
										تسجيل الدخول
									</h2>
									<p class="text-gray-600">
										قم بتسجيل الدخول للاستفادة من جميع خدماتنا
									</p>
								</div>

								<form @submit.prevent="login" class="rtl">
									<div class="mb-6">
										<label
											for="username"
											class="block text-gray-700 font-medium mb-2"
											>اسم المستخدم</label
										>
										<div class="relative">
											<div
												class="absolute inset-y-0 right-0 flex items-center pr-3 pointer-events-none"
											>
												<i class="fas fa-user text-gray-400"></i>
											</div>
											<input
												type="text"
												id="username"
												v-model="username"
												required
												class="w-full bg-gray-50 border border-gray-300 text-gray-900 text-lg rounded-lg focus:ring-blue-500 focus:border-blue-500 block p-3 pr-10"
												placeholder="أدخل اسم المستخدم"
											/>
										</div>
									</div>

									<div class="mb-6">
										<label
											for="password"
											class="block text-gray-700 font-medium mb-2"
											>كلمة المرور</label
										>
										<div class="relative">
											<div
												class="absolute inset-y-0 right-0 flex items-center pr-3 pointer-events-none"
											>
												<i class="fas fa-lock text-gray-400"></i>
											</div>
											<input
												type="password"
												id="password"
												v-model="password"
												required
												class="w-full bg-gray-50 border border-gray-300 text-gray-900 text-lg rounded-lg focus:ring-blue-500 focus:border-blue-500 block p-3 pr-10"
												placeholder="أدخل كلمة المرور"
											/>
										</div>
									</div>

									<div class="flex items-center justify-between mb-8">
										<div class="flex items-center">
											<input
												type="checkbox"
												id="rememberMe"
												v-model="rememberMe"
												class="w-5 h-5 text-blue-600 bg-gray-100 border-gray-300 rounded focus:ring-blue-500 ml-2"
											/>
											<label for="rememberMe" class="text-gray-700"
												>تذكرني</label
											>
										</div>
										<a
											href="#"
											class="text-blue-600 hover:text-blue-800 font-medium"
											>نسيت كلمة المرور؟</a
										>
									</div>

									<button
										type="submit"
										class="w-full flex items-center justify-center bg-blue-600 hover:bg-blue-700 text-white font-bold py-3 px-6 rounded-lg shadow-lg hover:shadow-xl transition-all duration-300 transform hover:-translate-y-1"
										:disabled="isLoading"
									>
										<span v-if="isLoading" class="mr-2">
											<div
												class="w-5 h-5 border-2 border-white border-t-transparent rounded-full animate-spin"
											></div>
										</span>
										{{ isLoading ? "جاري تسجيل الدخول..." : "تسجيل الدخول" }}
									</button>

									<div
										v-if="errorMessage"
										class="mt-6 p-4 text-sm text-center text-red-700 bg-red-50 rounded-lg border border-red-200 animate-fadeIn"
									>
										<i class="fas fa-exclamation-circle mr-2"></i>
										{{ errorMessage }}
									</div>
								</form>

								<div class="mt-8 text-center">
									<p class="text-gray-600">
										ليس لديك حساب؟
										<RouterLink
											to="/register"
											class="text-blue-600 hover:text-blue-800 font-medium"
										>
											إنشاء حساب جديد
										</RouterLink>
									</p>
								</div>
							</div>
						</div>
					</div>

					<!-- Right side: Content -->
					<div class="w-full lg:w-1/2 lg:order-1">
						<div class="rtl">
							<h1 class="text-4xl md:text-5xl font-bold text-gray-900 mb-6">
								ابدأ رحلتك معنا الآن
							</h1>
							<p class="text-xl text-gray-700 mb-8 leading-relaxed">
								قم بتسجيل الدخول للوصول إلى جميع الخدمات وإدارة حجوزاتك بسهولة
								وسرعة
							</p>

							<div class="space-y-6 mb-8">
								<div class="flex items-start gap-4">
									<div
										class="w-12 h-12 bg-blue-100 rounded-full flex items-center justify-center flex-shrink-0"
									>
										<i class="fas fa-check-circle text-blue-600 text-xl"></i>
									</div>
									<div>
										<h3 class="text-lg font-bold text-gray-900 mb-1">
											إدارة الحجوزات
										</h3>
										<p class="text-gray-700">
											يمكنك إدارة حجوزاتك ومتابعة تفاصيلها بشكل سهل
										</p>
									</div>
								</div>

								<div class="flex items-start gap-4">
									<div
										class="w-12 h-12 bg-blue-100 rounded-full flex items-center justify-center flex-shrink-0"
									>
										<i class="fas fa-history text-blue-600 text-xl"></i>
									</div>
									<div>
										<h3 class="text-lg font-bold text-gray-900 mb-1">
											سجل الخدمات
										</h3>
										<p class="text-gray-700">
											اطلع على سجل الخدمات السابقة وتفاصيلها
										</p>
									</div>
								</div>

								<div class="flex items-start gap-4">
									<div
										class="w-12 h-12 bg-blue-100 rounded-full flex items-center justify-center flex-shrink-0"
									>
										<i class="fas fa-user-shield text-blue-600 text-xl"></i>
									</div>
									<div>
										<h3 class="text-lg font-bold text-gray-900 mb-1">
											أمان وخصوصية
										</h3>
										<p class="text-gray-700">
											بياناتك الشخصية محمية ومشفرة بالكامل
										</p>
									</div>
								</div>
							</div>

							<RouterLink
								to="/"
								class="inline-flex items-center text-blue-600 hover:text-blue-800 font-medium"
							>
								<i class="fas fa-arrow-right ml-2"></i>
								العودة إلى الصفحة الرئيسية
							</RouterLink>
						</div>
					</div>
				</div>
			</div>
		</section>
	</div>
</template>

<style scoped>
	.rtl {
		direction: rtl;
		text-align: right;
	}

	.bg-pattern {
		background-image: url("data:image/svg+xml,%3Csvg width='60' height='60' viewBox='0 0 60 60' xmlns='http://www.w3.org/2000/svg'%3E%3Cg fill='none' fill-rule='evenodd'%3E%3Cg fill='%233b82f6' fill-opacity='0.1'%3E%3Cpath d='M36 34v-4h-2v4h-4v2h4v4h2v-4h4v-2h-4zm0-30V0h-2v4h-4v2h4v4h2V6h4V4h-4zM6 34v-4H4v4H0v2h4v4h2v-4h4v-2H6zM6 4V0H4v4H0v2h4v4h2V6h4V4H6z'/%3E%3C/g%3E%3C/g%3E%3C/svg%3E");
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
		animation: fadeIn 0.6s ease-out;
	}
</style>
