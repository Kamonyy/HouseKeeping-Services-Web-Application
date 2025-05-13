<template>
	<div
		class="fixed top-20 right-6 z-[200] space-y-4 max-h-screen overflow-hidden"
		dir="rtl"
	>
		<Toast
			v-for="toast in toasts"
			:key="toast.id"
			:message="toast.message"
			:type="toast.type"
			:duration="toast.duration"
			:visible="true"
			@close="removeToast(toast.id)"
		/>
	</div>
</template>

<script>
	import { useToastStore } from "../store/toastStore";
	import { storeToRefs } from "pinia";
	import Toast from "./Toast.vue";

	export default {
		name: "ToastContainer",
		components: {
			Toast,
		},
		setup() {
			const toastStore = useToastStore();
			const { toasts } = storeToRefs(toastStore);
			const { removeToast } = toastStore;

			return {
				toasts,
				removeToast,
			};
		},
	};
</script>
