<script setup>
	defineProps({
		title: {
			type: String,
			default: "",
		},
		subtitle: {
			type: String,
			default: "",
		},
		narrow: {
			type: Boolean,
			default: false,
		},
	});
</script>

<template>
	<div class="content-container slide-up">
		<div :class="narrow ? 'content-narrow' : ''">
			<!-- Page header with title and optional subtitle -->
			<header v-if="title" class="page-header stack-y-4">
				<h1 class="page-heading">{{ title }}</h1>
				<p v-if="subtitle" class="page-subtitle">{{ subtitle }}</p>
			</header>

			<!-- Main content -->
			<div class="page-content stack-y-6">
				<slot></slot>
			</div>
		</div>
	</div>
</template>

<style scoped>
	.page-header {
		margin-bottom: 2rem;
		position: relative;
	}

	.page-subtitle {
		color: var(--color-gray-600);
		font-size: var(--font-size-lg);
	}

	/* Add a subtle accent line to the header */
	.page-header::after {
		content: "";
		position: absolute;
		bottom: -1rem;
		left: 0;
		width: 5rem;
		height: 3px;
		background: linear-gradient(
			to right,
			var(--color-primary),
			var(--color-primary-light)
		);
		border-radius: 2px;
	}

	.rtl .page-header::after {
		left: auto;
		right: 0;
	}

	.page-content {
		margin-bottom: 2rem;
	}
</style>
