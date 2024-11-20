import { defineStore } from "pinia";
import axios from "axios";
import { useUserStore } from "./userStore"; // Import userStore to get the token

export const useCategoryStore = defineStore("category", {
	state: () => ({
		categories: [],
	}),
	actions: {
		setCategories(categories) {
			this.categories = categories;
		},
		async fetchCategories() {
			const userStore = useUserStore();

			try {
				const response = await axios.get(
					"https://localhost:7007/api/category",
					{
						headers: {
							Authorization: `Bearer ${userStore.token}`,
						},
					}
				);
				this.setCategories(response.data);
			} catch (error) {
				console.error("Failed to fetch categories:", error);
			}
		},
	},
});
