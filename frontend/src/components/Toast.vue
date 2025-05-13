<template>
	<transition name="toast-fade">
		<div
			v-if="visible"
			class="min-w-80 max-w-md rounded-xl shadow-xl backdrop-blur-sm border animate-float"
			:class="{
				'bg-gradient-to-r from-green-500 to-green-600 text-white border-green-400':
					type === 'success',
				'bg-gradient-to-r from-red-500 to-red-600 text-white border-red-400':
					type === 'error',
				'bg-gradient-to-r from-blue-500 to-blue-600 text-white border-blue-400':
					type === 'info',
				'bg-gradient-to-r from-yellow-500 to-yellow-600 text-white border-yellow-400':
					type === 'warning',
			}"
		>
			<div class="relative overflow-hidden">
				<!-- Decorative element -->
				<div class="absolute inset-0 opacity-10">
					<div
						class="absolute -right-10 -top-10 rounded-full w-24 h-24 bg-white"
					></div>
					<div
						class="absolute -left-10 -bottom-10 rounded-full w-16 h-16 bg-white"
					></div>
				</div>

				<div class="p-4 relative">
					<div class="flex items-center">
						<!-- Success Icon -->
						<div
							v-if="type === 'success'"
							class="flex-shrink-0 ml-3 p-1.5 bg-white bg-opacity-20 rounded-full"
						>
							<svg
								xmlns="http://www.w3.org/2000/svg"
								class="h-6 w-6"
								fill="none"
								viewBox="0 0 24 24"
								stroke="currentColor"
							>
								<path
									stroke-linecap="round"
									stroke-linejoin="round"
									stroke-width="2"
									d="M5 13l4 4L19 7"
								/>
							</svg>
						</div>
						<!-- Error Icon -->
						<div
							v-if="type === 'error'"
							class="flex-shrink-0 ml-3 p-1.5 bg-white bg-opacity-20 rounded-full"
						>
							<svg
								xmlns="http://www.w3.org/2000/svg"
								class="h-6 w-6"
								fill="none"
								viewBox="0 0 24 24"
								stroke="currentColor"
							>
								<path
									stroke-linecap="round"
									stroke-linejoin="round"
									stroke-width="2"
									d="M6 18L18 6M6 6l12 12"
								/>
							</svg>
						</div>
						<!-- Info Icon -->
						<div
							v-if="type === 'info'"
							class="flex-shrink-0 ml-3 p-1.5 bg-white bg-opacity-20 rounded-full"
						>
							<svg
								xmlns="http://www.w3.org/2000/svg"
								class="h-6 w-6"
								fill="none"
								viewBox="0 0 24 24"
								stroke="currentColor"
							>
								<path
									stroke-linecap="round"
									stroke-linejoin="round"
									stroke-width="2"
									d="M13 16h-1v-4h-1m1-4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z"
								/>
							</svg>
						</div>
						<!-- Warning Icon -->
						<div
							v-if="type === 'warning'"
							class="flex-shrink-0 ml-3 p-1.5 bg-white bg-opacity-20 rounded-full"
						>
							<svg
								xmlns="http://www.w3.org/2000/svg"
								class="h-6 w-6"
								fill="none"
								viewBox="0 0 24 24"
								stroke="currentColor"
							>
								<path
									stroke-linecap="round"
									stroke-linejoin="round"
									stroke-width="2"
									d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-3L13.732 4c-.77-1.333-2.694-1.333-3.464 0L3.34 16c-.77 1.333.192 3 1.732 3z"
								/>
							</svg>
						</div>

						<div class="mr-3 text-right flex-1">
							<p class="font-medium text-white text-opacity-90">
								{{ message }}
							</p>
						</div>
					</div>
				</div>

				<!-- Progress bar -->
				<div class="h-1 bg-white bg-opacity-30">
					<div
						class="h-full bg-white"
						:style="{
							width: `${progressWidth}%`,
							transition: `width ${duration}ms linear`,
						}"
					></div>
				</div>
			</div>

			<!-- Close button -->
			<button
				@click="close"
				class="absolute top-3 left-3 text-white hover:text-gray-200 bg-white bg-opacity-20 rounded-full h-6 w-6 flex items-center justify-center transition-all duration-200 hover:bg-opacity-30"
			>
				<svg
					xmlns="http://www.w3.org/2000/svg"
					class="h-4 w-4"
					fill="none"
					viewBox="0 0 24 24"
					stroke="currentColor"
				>
					<path
						stroke-linecap="round"
						stroke-linejoin="round"
						stroke-width="2"
						d="M6 18L18 6M6 6l12 12"
					/>
				</svg>
			</button>
		</div>
	</transition>
</template>

<script>
	export default {
		name: "Toast",
		props: {
			message: {
				type: String,
				required: true,
			},
			type: {
				type: String,
				default: "info",
				validator: (value) =>
					["success", "error", "info", "warning"].includes(value),
			},
			duration: {
				type: Number,
				default: 3000,
			},
			visible: {
				type: Boolean,
				default: true,
			},
		},
		emits: ["close"],
		data() {
			return {
				progressWidth: 100,
				timer: null,
			};
		},
		mounted() {
			if (this.duration > 0) {
				// Start the progress bar animation
				this.progressWidth = 100;
				setTimeout(() => {
					this.progressWidth = 0;
				}, 50);

				this.timer = setTimeout(() => {
					this.close();
				}, this.duration);
			}
		},
		beforeUnmount() {
			if (this.timer) {
				clearTimeout(this.timer);
			}
		},
		methods: {
			close() {
				this.$emit("close");
			},
		},
	};
</script>

<style scoped>
	.toast-fade-enter-active,
	.toast-fade-leave-active {
		transition: all 0.4s cubic-bezier(0.34, 1.56, 0.64, 1);
	}
	.toast-fade-enter-from,
	.toast-fade-leave-to {
		opacity: 0;
		transform: translateY(-10px) scale(0.95);
	}

	@keyframes float {
		0% {
			transform: translateY(0);
		}
		50% {
			transform: translateY(-5px);
		}
		100% {
			transform: translateY(0);
		}
	}

	.animate-float {
		animation: float 3s ease-in-out infinite;
	}
</style>
