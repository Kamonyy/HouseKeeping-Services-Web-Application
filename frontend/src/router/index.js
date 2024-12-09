import { createRouter, createWebHistory } from "vue-router";
import HomeView from "@/views/HomeView.vue";
import LoginView from "@/views/LoginView.vue";
import ServicesView from "@/views/ServicesView.vue";
import HelpView from "@/views/HelpView.vue";
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
			name: "Help",
			component: HelpView,
		},
	],
});

export default router;
