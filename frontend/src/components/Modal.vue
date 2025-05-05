<template>
	<Teleport to="body">
		<div v-if="isOpen" class="modal-overlay">
			<div
				class="modal-container rtl"
				@click.stop
				:class="{ 'modal-lg': size === 'lg', 'modal-sm': size === 'sm' }"
			>
				<div class="modal-header">
					<button @click="close" class="modal-close-btn" title="إغلاق">
						&times;
					</button>
					<h3 class="modal-title">{{ title }}</h3>
				</div>

				<div class="modal-body">
					<slot></slot>
				</div>

				<div class="modal-footer" v-if="$slots.footer">
					<slot name="footer"></slot>
				</div>
				<div class="modal-footer" v-else>
					<slot name="actions"></slot>
					<button @click="close" class="btn-secondary">إغلاق</button>
				</div>
			</div>
		</div>
	</Teleport>
</template>

<script>
	export default {
		name: "Modal",
		props: {
			isOpen: {
				type: Boolean,
				required: true,
			},
			title: {
				type: String,
				default: "عنوان النافذة",
			},
			size: {
				type: String,
				default: "md",
				validator: (value) => ["sm", "md", "lg"].includes(value),
			},
		},
		emits: ["close"],
		setup(props, { emit }) {
			const close = () => {
				emit("close");
			};

			return {
				close,
			};
		},
		watch: {
			isOpen(newVal) {
				if (newVal) {
					document.body.style.overflow = "hidden";
				} else {
					document.body.style.overflow = "";
				}
			},
		},
		unmounted() {
			document.body.style.overflow = "";
		},
	};
</script>

<style scoped>
	.rtl {
		direction: rtl;
		text-align: right;
	}

	.modal-overlay {
		@apply fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50 px-4;
		backdrop-filter: blur(2px);
	}

	.modal-container {
		@apply bg-white rounded-lg shadow-lg w-full max-w-md mx-auto overflow-hidden;
		animation: modal-appear 0.3s ease-out;
	}

	.modal-lg {
		@apply max-w-4xl;
	}

	.modal-sm {
		@apply max-w-sm;
	}

	.modal-header {
		@apply flex justify-between items-center p-4 border-b border-gray-200;
		flex-direction: row-reverse;
	}

	.modal-title {
		@apply text-lg font-semibold text-gray-800;
	}

	.modal-close-btn {
		@apply text-2xl text-gray-500 hover:text-gray-800 transition-colors focus:outline-none;
	}

	.modal-body {
		@apply p-4 max-h-[70vh] overflow-y-auto;
	}

	.modal-footer {
		@apply p-4 border-t border-gray-200 flex justify-start space-x-reverse space-x-2;
	}

	.btn-secondary {
		@apply px-4 py-2 bg-gray-500 hover:bg-gray-600 text-white rounded-md transition-colors focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-gray-500;
	}

	@keyframes modal-appear {
		from {
			opacity: 0;
			transform: translateY(-20px);
		}
		to {
			opacity: 1;
			transform: translateY(0);
		}
	}
</style>
