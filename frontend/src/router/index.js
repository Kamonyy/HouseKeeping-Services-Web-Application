import { createRouter, createWebHistory } from "vue-router";
import HomeView from "@/views/HomeView.vue";
import LoginView from "@/views/LoginView.vue";
import ServicesView from "@/views/ServicesView.vue";
import HelpView from "@/views/HelpView.vue";
import CategoryServicesView from "@/views/CategoryServiceView.vue"; // Import the new view
import ProviderManagementView from "@/views/ProviderManagementView.vue";
import AdminDashboardView from "@/views/AdminDashboardView.vue";
import { useUserStore } from "@/store/userStore";

const router = createRouter({
	history: createWebHistory(import.meta.env.BASE_URL),
	routes: [
		{
			path: "/",
			name: "home",
			component: HomeView,
		},
		{
			path: "/login",
			name: "login",
			component: LoginView,
		},
		{
			path: "/services",
			name: "services",
			component: ServicesView,
		},
		{
			path: "/help",
			name: "help",
			component: HelpView,
		},
		{
			path: "/category/:id",
			name: "category",
			component: CategoryServicesView,
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

	// Check if route requires authentication
	if (to.matched.some((record) => record.meta.requiresAuth)) {
		// Check if user is logged in
		if (!userStore.isLoggedIn) {
			next({ name: "login" });
		} else if (to.meta.roles && to.meta.roles.length > 0) {
			// Check if user has required role
			const hasRequiredRole = to.meta.roles.some((role) =>
				userStore.hasRole(role)
			);
			if (!hasRequiredRole) {
				// User doesn't have the required role, redirect to home
				next({ name: "home" });
			} else {
				// User has the required role, proceed
				next();
			}
		} else {
			// No role requirement, proceed
			next();
		}
	} else {
		next();
	}
});

export default router;
