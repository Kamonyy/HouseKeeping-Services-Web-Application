<script setup>
	import { ref, onMounted, onUnmounted } from "vue";

	const scrollWidth = ref("0%");
	let animationFrameId = null;

	// Update the scroll progress width using requestAnimationFrame for better performance
	const updateScrollProgress = () => {
		animationFrameId = null;
		const windowHeight =
			document.documentElement.scrollHeight -
			document.documentElement.clientHeight;
		if (windowHeight <= 0) return; // Avoid division by zero

		const scrollPosition = window.scrollY;
		const scrollPercentage = Math.min(
			100,
			Math.max(0, (scrollPosition / windowHeight) * 100)
		);
		scrollWidth.value = `${scrollPercentage}%`;
	};

	// Debounced scroll handler using requestAnimationFrame
	const handleScroll = () => {
		if (animationFrameId === null) {
			animationFrameId = requestAnimationFrame(updateScrollProgress);
		}
	};

	// Setup and cleanup
	onMounted(() => {
		window.addEventListener("scroll", handleScroll, { passive: true });
		updateScrollProgress(); // Initial calculation
	});

	onUnmounted(() => {
		window.removeEventListener("scroll", handleScroll);
		if (animationFrameId !== null) {
			cancelAnimationFrame(animationFrameId);
		}
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
		transition: width 0.1s ease;
	}
</style>
