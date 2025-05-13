<script setup>
	import { ref, onMounted, onUnmounted } from "vue";

	const scrollWidth = ref("0%");

	// Update the scroll progress width based on how far the user has scrolled
	const updateScrollProgress = () => {
		const windowHeight =
			document.documentElement.scrollHeight -
			document.documentElement.clientHeight;
		const scrollPosition = window.scrollY;
		const scrollPercentage = (scrollPosition / windowHeight) * 100;
		scrollWidth.value = `${scrollPercentage}%`;
	};

	// Setup and cleanup scroll event listeners
	onMounted(() => {
		window.addEventListener("scroll", updateScrollProgress);
	});

	onUnmounted(() => {
		window.removeEventListener("scroll", updateScrollProgress);
	});
</script>

<template>
	<div class="scroll-progress" :style="{ width: scrollWidth }"></div>
</template>

<style scoped>
	.scroll-progress {
		position: fixed;
		top: 0;
		left: 0;
		height: 3px;
		background: linear-gradient(to right, #3b82f6, #60a5fa);
		z-index: 999;
		transition: width 0.2s ease;
	}
</style>
