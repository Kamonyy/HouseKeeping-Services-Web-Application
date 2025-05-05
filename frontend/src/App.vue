<script setup>
	import { RouterView } from "vue-router";
	import Nav from "@/components/Nav.vue";
	import Footer from "@/components/Footer.vue";
</script>

<template>
	<div class="layout">
		<header>
			<Nav />
		</header>
		<main>
			<RouterView v-slot="{ Component }">
				<Transition name="scale" mode="out-in">
					<component :is="Component" />
				</Transition>
			</RouterView>
		</main>
		<Footer />
	</div>
</template>

<style scoped>
	.layout {
		display: flex;
		flex-direction: column;
		min-height: 100vh;
	}

	header {
		width: 100%;
	}

	main {
		flex: 1;
	}

	footer {
		width: 100%;
	}

	.scale-enter-active,
	.scale-leave-active {
		transition: transform 0.4s ease, opacity 0.4s ease;
	}

	.scale-enter-from,
	.scale-leave-to {
		transform: scale(1.1);
		opacity: 0;
	}
</style>
