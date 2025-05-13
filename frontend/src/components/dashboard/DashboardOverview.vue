<template>
	<div class="space-y-6 animate-fadeIn">
		<h1 class="text-3xl font-bold text-color-dark mb-6">
			لوحة التحكم الرئيسية
		</h1>
		<div v-if="loading" class="flex justify-center items-center p-10">
			<div
				class="animate-spin rounded-full h-12 w-12 border-t-2 border-b-2 border-primary"
			></div>
		</div>
		<div v-else>
			<!-- Stats Cards -->
			<div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6 mb-8">
				<div
					class="bg-gradient-to-br from-white to-blue-50 p-6 rounded-xl shadow-md border-l-4 border-primary transform transition-all duration-300 hover:scale-105 hover:shadow-lg"
					style="animation-delay: 0.1s"
				>
					<div class="flex justify-between items-center">
						<div>
							<h3 class="text-lg font-semibold text-color-dark">
								إجمالي الخدمات
							</h3>
							<p class="text-3xl font-bold text-primary mt-2">
								{{ dashboardStats.totalServices }}
							</p>
						</div>
						<div class="bg-blue-100 p-3 rounded-full">
							<i class="fas fa-briefcase text-primary text-xl"></i>
						</div>
					</div>
				</div>
				<div
					class="bg-gradient-to-br from-white to-green-50 p-6 rounded-xl shadow-md border-l-4 border-green-500 transform transition-all duration-300 hover:scale-105 hover:shadow-lg"
					style="animation-delay: 0.2s"
				>
					<div class="flex justify-between items-center">
						<div>
							<h3 class="text-lg font-semibold text-color-dark">
								الخدمات المعتمدة
							</h3>
							<p class="text-3xl font-bold text-green-600 mt-2">
								{{ dashboardStats.approvedServices }}
							</p>
						</div>
						<div class="bg-green-100 p-3 rounded-full">
							<i class="fas fa-check-circle text-green-600 text-xl"></i>
						</div>
					</div>
				</div>
				<div
					class="bg-gradient-to-br from-white to-yellow-50 p-6 rounded-xl shadow-md border-l-4 border-yellow-500 transform transition-all duration-300 hover:scale-105 hover:shadow-lg"
					style="animation-delay: 0.3s"
				>
					<div class="flex justify-between items-center">
						<div>
							<h3 class="text-lg font-semibold text-color-dark">
								بانتظار الموافقة
							</h3>
							<p class="text-3xl font-bold text-yellow-600 mt-2">
								{{ dashboardStats.pendingServices }}
							</p>
						</div>
						<div class="bg-yellow-100 p-3 rounded-full">
							<i class="fas fa-clock text-yellow-600 text-xl"></i>
						</div>
					</div>
				</div>
				<div
					class="bg-gradient-to-br from-white to-purple-50 p-6 rounded-xl shadow-md border-l-4 border-purple-500 transform transition-all duration-300 hover:scale-105 hover:shadow-lg"
					style="animation-delay: 0.4s"
				>
					<div class="flex justify-between items-center">
						<div>
							<h3 class="text-lg font-semibold text-color-dark">
								إجمالي المستخدمين
							</h3>
							<p class="text-3xl font-bold text-purple-600 mt-2">
								{{ dashboardStats.totalUsers }}
							</p>
						</div>
						<div class="bg-purple-100 p-3 rounded-full">
							<i class="fas fa-users text-purple-600 text-xl"></i>
						</div>
					</div>
				</div>
			</div>

			<!-- Additional Dashboard Content -->
			<div class="grid grid-cols-1 md:grid-cols-2 gap-6">
				<!-- Recent Activity -->
				<div
					class="bg-gradient-to-br from-white to-blue-50 rounded-xl shadow-md p-6 animate-fadeIn"
					style="animation-delay: 0.5s"
				>
					<h3
						class="text-xl font-semibold text-color-dark mb-4 flex items-center"
					>
						<i class="fas fa-chart-line text-primary mr-2"></i>
						النشاط الأخير
					</h3>
					<div class="space-y-4">
						<div
							class="flex items-center p-3 bg-white rounded-lg shadow-sm border border-gray-100 hover:shadow-md transition-shadow"
						>
							<div class="bg-green-100 p-2 rounded-full">
								<i class="fas fa-user-plus text-green-600"></i>
							</div>
							<div class="mr-3">
								<p class="text-sm text-color-dark">انضم مستخدم جديد</p>
								<p class="text-xs text-color-medium">منذ 5 دقائق</p>
							</div>
						</div>
						<div
							class="flex items-center p-3 bg-white rounded-lg shadow-sm border border-gray-100 hover:shadow-md transition-shadow"
						>
							<div class="bg-blue-100 p-2 rounded-full">
								<i class="fas fa-plus-circle text-primary"></i>
							</div>
							<div class="mr-3">
								<p class="text-sm text-color-dark">تمت إضافة خدمة جديدة</p>
								<p class="text-xs text-color-medium">منذ ساعة</p>
							</div>
						</div>
						<div
							class="flex items-center p-3 bg-white rounded-lg shadow-sm border border-gray-100 hover:shadow-md transition-shadow"
						>
							<div class="bg-yellow-100 p-2 rounded-full">
								<i class="fas fa-star text-yellow-600"></i>
							</div>
							<div class="mr-3">
								<p class="text-sm text-color-dark">تقييم جديد لخدمة</p>
								<p class="text-xs text-color-medium">منذ 3 ساعات</p>
							</div>
						</div>
					</div>
				</div>

				<!-- Quick Actions -->
				<div
					class="bg-gradient-to-br from-white to-blue-50 rounded-xl shadow-md p-6 animate-fadeIn"
					style="animation-delay: 0.6s"
				>
					<h3
						class="text-xl font-semibold text-color-dark mb-4 flex items-center"
					>
						<i class="fas fa-bolt text-primary mr-2"></i>
						إجراءات سريعة
					</h3>
					<div class="grid grid-cols-2 gap-4">
						<button
							@click="navigateTo('services')"
							class="p-4 bg-primary text-white rounded-lg shadow-md hover:bg-primary-dark transition-colors transform hover:scale-105 hover:shadow-lg flex flex-col items-center justify-center"
						>
							<i class="fas fa-plus-circle text-2xl mb-2"></i>
							<span>إضافة خدمة</span>
						</button>
						<button
							@click="navigateTo('users')"
							class="p-4 bg-green-600 text-white rounded-lg shadow-md hover:bg-green-700 transition-colors transform hover:scale-105 hover:shadow-lg flex flex-col items-center justify-center"
						>
							<i class="fas fa-user-plus text-2xl mb-2"></i>
							<span>إضافة مستخدم</span>
						</button>
						<button
							@click="navigateTo('categories')"
							class="p-4 bg-purple-600 text-white rounded-lg shadow-md hover:bg-purple-700 transition-colors transform hover:scale-105 hover:shadow-lg flex flex-col items-center justify-center"
						>
							<i class="fas fa-folder-plus text-2xl mb-2"></i>
							<span>إضافة تصنيف</span>
						</button>
						<button
							@click="navigateTo('settings')"
							class="p-4 bg-yellow-600 text-white rounded-lg shadow-md hover:bg-yellow-700 transition-colors transform hover:scale-105 hover:shadow-lg flex flex-col items-center justify-center"
						>
							<i class="fas fa-cog text-2xl mb-2"></i>
							<span>الإعدادات</span>
						</button>
					</div>
				</div>
			</div>
		</div>
	</div>
</template>

<script>
	import { ref, onMounted, inject } from "vue";
	import { useUserStore } from "@/store/userStore";
	import { useToastStore } from "@/store/toastStore";
	import DashboardService from "@/services/DashboardService.js";

	export default {
		name: "DashboardOverview",
		setup() {
			const loading = ref(true);
			const userStore = useUserStore();
			const toastStore = useToastStore();
			// Use inject to get the changeSection function from parent component
			const handleSectionChange = inject("handleSectionChange", null);

			const dashboardStats = ref({
				totalServices: 0,
				approvedServices: 0,
				pendingServices: 0,
				totalUsers: 0,
			});

			const fetchDashboardData = async () => {
				loading.value = true;
				try {
					const data = await DashboardService.getDashboardStatistics(
						userStore.token
					);
					dashboardStats.value = data;
				} catch (error) {
					console.error("خطأ في تحميل إحصاءات لوحة التحكم:", error);
					toastStore.error(
						"فشل تحميل بيانات لوحة التحكم. يرجى المحاولة مرة أخرى."
					);
				} finally {
					loading.value = false;
				}
			};

			// Function to handle navigation between sections
			const navigateTo = (section) => {
				if (handleSectionChange) {
					handleSectionChange(section);
				} else {
					toastStore.info("تم النقر على " + section);
					console.log("Navigation to section:", section);
				}
			};

			onMounted(() => {
				fetchDashboardData();
			});

			return {
				loading,
				dashboardStats,
				navigateTo,
			};
		},
	};
</script>

<style scoped>
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
		animation: fadeIn 0.6s ease-out forwards;
	}

	.text-color-dark {
		color: var(--color-gray-800);
	}

	.text-color-medium {
		color: var(--color-gray-600);
	}

	.text-primary {
		color: var(--color-primary);
	}

	.text-primary-dark {
		color: var(--color-primary-dark);
	}

	.bg-primary {
		background-color: var(--color-primary);
	}

	.bg-primary-dark {
		background-color: var(--color-primary-dark);
	}

	.border-primary {
		border-color: var(--color-primary);
	}
</style>
