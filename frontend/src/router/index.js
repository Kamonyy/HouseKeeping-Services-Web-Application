import { createRouter, createWebHistory } from "vue-router";
import HomeView from "@/views/HomeView.vue";
import LoginView from "@/views/LoginView.vue";
import RegisterView from "@/views/RegisterView.vue";
import ServicesView from "@/views/ServicesView.vue";
import HelpView from "@/views/HelpView.vue";
import CategoryServicesView from "@/views/CategoryServiceView.vue";
import ProviderManagementView from "@/views/ProviderManagementView.vue";
import AdminDashboardView from "@/views/AdminDashboardView.vue";
import ServiceDetailView from "@/views/ServiceDetailView.vue";
import { useUserStore } from "@/store/userStore";

const router = createRouter({
	history: createWebHistory(import.meta.env.BASE_URL),
	routes: [
		{ path: "/", name: "home", component: HomeView },
		{ path: "/login", name: "login", component: LoginView },
		{ path: "/register", name: "register", component: RegisterView },
		{ path: "/services", name: "services", component: ServicesView },
		{ path: "/help", name: "help", component: HelpView },
		{
			path: "/category/:id",
			name: "category",
			component: CategoryServicesView,
			props: true,
		},
		{
			path: "/service/:id",
			name: "service-detail",
			component: ServiceDetailView,
			props: true,
		},
		{
			path: "/provider-management",
			name: "provider-management",
			component: ProviderManagementView,
			meta: { requiresAuth: true, roles: ["Provider"] },
		},
		{
			path: "/admin/dashboard",
			name: "admin-dashboard",
			component: AdminDashboardView,
			meta: { requiresAuth: true, roles: ["Admin"] },
		},
	],
});

// Navigation guard for protected routes
router.beforeEach((to, from, next) => {
	const userStore = useUserStore();
	userStore.checkTokenExpiration();

	if (to.matched.some((record) => record.meta.requiresAuth)) {
		if (!userStore.isLoggedIn) {
			return next({ name: "login" });
		}

		if (to.meta.roles?.length > 0) {
			const hasRequiredRole = to.meta.roles.some((role) =>
				userStore.hasRole(role)
			);
			if (!hasRequiredRole) {
				return next({ name: "home" });
			}
		}
	}

	next();
});

export default router;
