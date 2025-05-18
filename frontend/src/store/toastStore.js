import { defineStore } from "pinia";

const TOAST_DURATIONS = {
	success: 3000,
	error: 4000,
	info: 3000,
	warning: 3500,
};

export const useToastStore = defineStore("toast", {
	state: () => ({
		toasts: [],
	}),

	actions: {
		addToast({ message, type = "info", duration }) {
			const id = Date.now().toString();
			this.toasts.push({
				id,
				message,
				type,
				duration: duration || TOAST_DURATIONS[type] || 3000,
			});
			return id;
		},

		removeToast(id) {
			const index = this.toasts.findIndex((toast) => toast.id === id);
			if (index !== -1) {
				this.toasts.splice(index, 1);
			}
		},

		success(message, duration) {
			return this.addToast({ message, type: "success", duration });
		},

		error(message, duration) {
			return this.addToast({ message, type: "error", duration });
		},

		info(message, duration) {
			return this.addToast({ message, type: "info", duration });
		},

		warning(message, duration) {
			return this.addToast({ message, type: "warning", duration });
		},
	},
});
