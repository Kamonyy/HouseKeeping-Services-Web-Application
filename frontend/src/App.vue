<script setup>
	import { RouterView } from "vue-router";
	import Nav from "@/components/Nav.vue";
	import Footer from "@/components/Footer.vue";
	import ToastContainer from "@/components/ToastContainer.vue";
	import ScrollProgressBar from "@/components/ScrollProgressBar.vue";
	import GlobalStyles from "@/components/GlobalStyles.vue";
</script>

<template>
	<div class="app-layout">
		<GlobalStyles />
		<ScrollProgressBar />
		<Nav class="app-header" />
		<ToastContainer />
		<main class="app-main">
			<RouterView v-slot="{ Component }">
				<Transition name="page-transition" mode="out-in">
					<component :is="Component" :key="$route.path" />
				</Transition>
			</RouterView>
		</main>
		<Footer class="app-footer" />
	</div>
</template>

<style>
	.app-layout {
		display: flex;
		flex-direction: column;
		min-height: 100vh;
		background-color: var(--color-gray-50);
	}

	.app-header {
		width: 100%;
		position: sticky;
		top: 0;
		z-index: 100;
	}

	.app-main {
		flex: 1;
		padding: 1rem 0;
		margin-bottom: 2rem;
		position: relative;
		z-index: 50;
	}

	.app-footer {
		width: 100%;
		margin-top: auto;
	}

	/* Page transitions */
	.page-transition-enter-active,
	.page-transition-leave-active {
		transition: opacity 0.3s ease,
			transform 0.35s cubic-bezier(0.34, 1.56, 0.64, 1);
	}

	.page-transition-enter-from {
		opacity: 0;
		transform: translateY(10px);
	}

	.page-transition-leave-to {
		opacity: 0;
		transform: translateY(-10px);
	}
</style>
