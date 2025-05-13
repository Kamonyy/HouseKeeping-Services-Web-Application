import { defineStore } from "pinia";

export const useToastStore = defineStore("toast", {
	state: () => ({
		toasts: [],
	}),

	actions: {
		addToast(toast) {
			const id = Date.now().toString();
			this.toasts.push({
				id,
				message: toast.message,
				type: toast.type || "info",
				duration: toast.duration || 3000,
			});

			return id;
		},

		removeToast(id) {
			const index = this.toasts.findIndex((toast) => toast.id === id);
			if (index !== -1) {
				this.toasts.splice(index, 1);
			}
		},

		success(message, duration = 3000) {
			return this.addToast({ message, type: "success", duration });
		},

		error(message, duration = 4000) {
			return this.addToast({ message, type: "error", duration });
		},

		info(message, duration = 3000) {
			return this.addToast({ message, type: "info", duration });
		},

		warning(message, duration = 3500) {
			return this.addToast({ message, type: "warning", duration });
		},
	},
});
