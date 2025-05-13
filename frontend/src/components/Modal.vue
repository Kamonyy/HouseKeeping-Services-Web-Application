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
						<i class="fas fa-times"></i>
					</button>
					<h3 class="modal-title">
						<slot name="header-icon"></slot>
						{{ title }}
					</h3>
				</div>

				<div class="modal-body">
					<slot></slot>
				</div>

				<div class="modal-footer" v-if="$slots.footer">
					<slot name="footer"></slot>
				</div>
				<div class="modal-footer" v-else>
					<slot name="actions"></slot>
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
		backdrop-filter: blur(4px);
	}

	.modal-container {
		@apply bg-white rounded-xl shadow-xl w-full mx-auto overflow-hidden;
		animation: modal-appear 0.3s cubic-bezier(0.16, 1, 0.3, 1);
		max-height: 90vh;
		margin-top: 5vh;
	}

	.modal-lg {
		@apply max-w-4xl;
	}

	.modal-sm {
		@apply max-w-sm;
	}

	/* Default modal size (md) */
	.modal-container:not(.modal-lg):not(.modal-sm) {
		@apply max-w-2xl;
	}

	.modal-header {
		@apply flex justify-between items-center p-5 border-b border-gray-200 bg-gradient-to-r from-blue-600 via-blue-500 to-blue-700;
		flex-direction: row-reverse;
	}

	.modal-title {
		@apply text-xl font-bold text-white flex items-center gap-3;
	}

	.modal-close-btn {
		@apply text-white hover:text-gray-200 focus:outline-none transition-colors duration-200 bg-white/20 rounded-full w-8 h-8 flex items-center justify-center hover:bg-white/30;
	}

	.modal-body {
		@apply p-6 max-h-[calc(90vh-8rem)] overflow-y-auto;
	}

	.modal-footer {
		@apply p-5 border-t border-gray-200 flex justify-start space-x-reverse space-x-2 bg-gray-50;
	}

	@keyframes modal-appear {
		from {
			opacity: 0;
			transform: scale(0.98) translateY(-10px);
		}
		to {
			opacity: 1;
			transform: scale(1) translateY(0);
		}
	}
</style>
