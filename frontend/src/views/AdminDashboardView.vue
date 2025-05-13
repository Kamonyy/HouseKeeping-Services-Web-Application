<template>
	<div class="flex h-screen bg-gray-50 animate-fadeIn" dir="rtl">
		<!-- Admin Sidebar -->
		<div
			class="w-64 bg-gradient-to-b from-blue-800 to-blue-900 text-white shadow-lg transition-all duration-300"
		>
			<div class="p-5 border-b border-blue-700 bg-blue-900/40 backdrop-blur-sm">
				<h2 class="text-xl font-bold flex items-center gap-2">
					<p class="text-xl font-bold text-white">
						<i class="fas fa-tachometer-alt text-blue-300"></i>
						لوحة التحكم
					</p>
				</h2>
				<p class="text-sm text-blue-300 mt-1">مرحباً بك في لوحة التحكم</p>
			</div>
			<nav class="p-3">
				<ul class="space-y-1">
					<li>
						<button
							@click="handleSectionChange('dashboard')"
							class="w-full flex items-center p-3 text-right rounded-lg transition-all duration-200"
							:class="
								activeSection === 'dashboard'
									? 'bg-blue-700/80 text-white shadow-md'
									: 'text-blue-200 hover:bg-blue-700/50 hover:text-white'
							"
						>
							<i class="fas fa-home text-lg mr-3"></i>
							الرئيسية
						</button>
					</li>
					<li>
						<button
							@click="handleSectionChange('services')"
							class="w-full flex items-center p-3 text-right rounded-lg transition-all duration-200"
							:class="
								activeSection === 'services'
									? 'bg-blue-700/80 text-white shadow-md'
									: 'text-blue-200 hover:bg-blue-700/50 hover:text-white'
							"
						>
							<i class="fas fa-briefcase text-lg mr-3"></i>
							إدارة الخدمات
						</button>
					</li>
					<li>
						<button
							@click="handleSectionChange('users')"
							class="w-full flex items-center p-3 text-right rounded-lg transition-all duration-200"
							:class="
								activeSection === 'users'
									? 'bg-blue-700/80 text-white shadow-md'
									: 'text-blue-200 hover:bg-blue-700/50 hover:text-white'
							"
						>
							<i class="fas fa-users text-lg mr-3"></i>
							إدارة المستخدمين
						</button>
					</li>
					<li>
						<button
							@click="handleSectionChange('categories')"
							class="w-full flex items-center p-3 text-right rounded-lg transition-all duration-200"
							:class="
								activeSection === 'categories'
									? 'bg-blue-700/80 text-white shadow-md'
									: 'text-blue-200 hover:bg-blue-700/50 hover:text-white'
							"
						>
							<i class="fas fa-tag text-lg mr-3"></i>
							إدارة التصنيفات
						</button>
					</li>
					<li>
						<button
							@click="handleSectionChange('reports')"
							class="w-full flex items-center p-3 text-right rounded-lg transition-all duration-200"
							:class="
								activeSection === 'reports'
									? 'bg-blue-700/80 text-white shadow-md'
									: 'text-blue-200 hover:bg-blue-700/50 hover:text-white'
							"
						>
							<i class="fas fa-chart-bar text-lg mr-3"></i>
							التقارير والإحصائيات
						</button>
					</li>
					<li>
						<button
							@click="handleSectionChange('settings')"
							class="w-full flex items-center p-3 text-right rounded-lg transition-all duration-200"
							:class="
								activeSection === 'settings'
									? 'bg-blue-700/80 text-white shadow-md'
									: 'text-blue-200 hover:bg-blue-700/50 hover:text-white'
							"
						>
							<i class="fas fa-cog text-lg mr-3"></i>
							الإعدادات
						</button>
					</li>
				</ul>
			</nav>
		</div>

		<!-- Main Content Area -->
		<div class="flex-1 overflow-auto">
			<div class="p-6">
				<!-- Dashboard main sections container -->
				<div class="flex-1 p-6 overflow-y-auto">
					<Transition name="fade" mode="out-in">
						<component
							:is="currentComponent"
							:key="activeSection"
							class="dashboard-section"
							:userToken="userStore.token"
						/>
					</Transition>
				</div>
			</div>
		</div>
	</div>
</template>

<script>
	import { useUserStore } from "@/store/userStore";
	import { useToastStore } from "@/store/toastStore";
	import { ref, computed, onMounted, provide, defineComponent, h } from "vue";
	import { useRouter } from "vue-router";
	import Modal from "@/components/Modal.vue";
	import DashboardOverview from "@/components/dashboard/DashboardOverview.vue";
	import ServicesManagement from "@/components/dashboard/ServicesManagement.vue";
	import UsersManagement from "@/components/dashboard/UsersManagement.vue";
	import CategoriesManagement from "@/components/dashboard/CategoriesManagement.vue";

	// Define Reports component
	const ReportsComponent = defineComponent({
		name: "ReportsComponent",
		setup() {
			return () =>
				h("div", { class: "space-y-6 animate-fadeIn" }, [
					h(
						"h1",
						{ class: "text-3xl font-bold text-gray-800 mb-6" },
						"التقارير والإحصائيات"
					),
					h("div", { class: "bg-white rounded-xl shadow-md p-8 text-center" }, [
						h("i", { class: "fas fa-chart-bar text-blue-500 text-5xl mb-4" }),
						h(
							"h2",
							{ class: "text-2xl font-semibold text-gray-700 mb-2" },
							"قريباً"
						),
						h(
							"p",
							{ class: "text-gray-600" },
							"هذه الميزة قيد التطوير وستكون متاحة في المستقبل القريب."
						),
					]),
				]);
		},
	});

	// Define Settings component
	const SettingsComponent = defineComponent({
		name: "SettingsComponent",
		setup() {
			return () =>
				h("div", { class: "space-y-6 animate-fadeIn" }, [
					h(
						"h1",
						{ class: "text-3xl font-bold text-gray-800 mb-6" },
						"إعدادات النظام"
					),
					h("div", { class: "bg-white rounded-xl shadow-md p-8 text-center" }, [
						h("i", { class: "fas fa-cog text-blue-500 text-5xl mb-4" }),
						h(
							"h2",
							{ class: "text-2xl font-semibold text-gray-700 mb-2" },
							"قريباً"
						),
						h(
							"p",
							{ class: "text-gray-600" },
							"إعدادات النظام قيد التطوير وستكون متاحة قريباً."
						),
					]),
				]);
		},
	});

	export default {
		name: "AdminDashboardView",
		components: {
			Modal,
			DashboardOverview,
			ServicesManagement,
			UsersManagement,
			CategoriesManagement,
			ReportsComponent,
			SettingsComponent,
		},
		setup() {
			const userStore = useUserStore();
			const toastStore = useToastStore();
			const router = useRouter();
			const activeSection = ref("dashboard");

			const isAdmin = computed(() => userStore.isAdmin);

			// Computed property to determine which component to render
			const currentComponent = computed(() => {
				switch (activeSection.value) {
					case "dashboard":
						return DashboardOverview;
					case "services":
						return ServicesManagement;
					case "users":
						return UsersManagement;
					case "categories":
						return CategoriesManagement;
					case "reports":
						return ReportsComponent;
					case "settings":
						return SettingsComponent;
					default:
						return DashboardOverview;
				}
			});

			onMounted(() => {
				if (!isAdmin.value) {
					router.push("/");
					return;
				}
			});

			// Section switching
			const handleSectionChange = (section) => {
				activeSection.value = section;
			};

			// Provide the section change function to child components
			provide("handleSectionChange", handleSectionChange);

			return {
				userStore,
				toastStore,
				activeSection,
				currentComponent,
				handleSectionChange,
			};
		},
	};
</script>

<style>
	.fade-enter-active,
	.fade-leave-active {
		transition: opacity 0.3s ease;
	}

	.fade-enter-from,
	.fade-leave-to {
		opacity: 0;
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
		animation: fadeIn 0.5s ease-out forwards;
	}

	.dashboard-section {
		animation: fadeIn 0.5s ease-out forwards;
	}

	/* Improved text contrast */
	.text-gray-500 {
		color: #64748b !important; /* Darker gray for better visibility */
	}

	.text-gray-700 {
		color: #334155 !important; /* Darker for better readability */
	}

	.text-blue-200 {
		color: #bfdbfe !important; /* Brighter blue for better visibility on dark backgrounds */
	}

	h1,
	h2,
	h3,
	h4,
	h5,
	h6 {
		color: #1e293b !important; /* Darker heading color for better contrast */
	}

	.text-sm {
		font-weight: 500; /* Slightly bolder font weight for small text */
	}
</style>
