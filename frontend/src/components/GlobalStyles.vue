<script setup>
	// This component doesn't need script logic
</script>

<template>
	<!-- Empty template - this component only provides global styles -->
</template>

<style>
	/* Design tokens */
	:root {
		--color-primary: #0066ff;
		--color-primary-light: #4d94ff;
		--color-primary-dark: #0052cc;
		--color-secondary: #ff8c00;
		--color-secondary-light: #ffb74d;
		--color-gray-50: #f8fafc;
		--color-gray-100: #f1f5f9;
		--color-gray-200: #e2e8f0;
		--color-gray-300: #cbd5e1;
		--color-gray-400: #94a3b8;
		--color-gray-500: #64748b;
		--color-gray-600: #475569;
		--color-gray-700: #334155;
		--color-gray-800: #1e293b;
		--color-gray-900: #0f172a;
	}

	/* Base typography */
	body {
		font-family: "Cairo", sans-serif;
		font-size: 16px;
		line-height: 1.6;
		color: var(--color-gray-800);
		-webkit-font-smoothing: antialiased;
		-moz-osx-font-smoothing: grayscale;
		background-color: var(--color-gray-50);
	}

	/* Headings */
	h1,
	h2,
	h3,
	h4,
	h5,
	h6 {
		font-family: "Cairo", sans-serif;
		font-weight: 700;
		line-height: 1.3;
		margin-bottom: 0.5em;
		color: var(--color-gray-900);
	}

	h1 {
		font-size: 2.5rem;
		font-weight: 800;
	}

	h2 {
		font-size: 2rem;
		font-weight: 700;
	}

	h3 {
		font-size: 1.5rem;
		font-weight: 600;
	}

	h4 {
		font-size: 1.25rem;
		font-weight: 600;
	}

	/* Paragraph */
	p {
		margin-bottom: 1rem;
		line-height: 1.7;
		color: var(--color-gray-700);
	}

	/* Links */
	a {
		color: var(--color-primary);
		text-decoration: none;
		transition: all 0.2s ease;
		font-weight: 500;
	}

	a:hover,
	a:focus {
		color: var(--color-primary-dark);
	}

	/* RTL support */
	.rtl {
		line-height: 1.8;
		font-feature-settings: "kern", "liga", "calt";
	}

	/* Form elements */
	button,
	.btn {
		font-family: "Cairo", sans-serif;
		font-weight: 600;
	}

	input,
	textarea,
	select {
		font-family: "Cairo", sans-serif;
		color: var(--color-gray-800);
	}

	::placeholder {
		opacity: 0.7;
		color: var(--color-gray-500);
	}

	/* Focus states for accessibility */
	button:focus,
	a:focus,
	input:focus,
	select:focus,
	textarea:focus {
		outline: 2px solid var(--color-primary-light);
		outline-offset: 2px;
	}

	/* Labels */
	label {
		color: var(--color-gray-800);
		font-weight: 600;
		margin-bottom: 0.25rem;
		display: inline-block;
	}

	/* Utility classes */
	.text-primary {
		color: var(--color-primary) !important;
	}

	.text-secondary {
		color: var(--color-secondary) !important;
	}

	.text-dark {
		color: var(--color-gray-900) !important;
	}

	.text-medium {
		color: var(--color-gray-700) !important;
	}

	.text-light {
		color: var(--color-gray-600) !important;
	}

	/* Card and overlay styles */
	.card-title,
	.category-name,
	.service-title {
		color: white !important;
		font-weight: 700 !important;
		text-shadow: 0 2px 4px rgba(0, 0, 0, 0.7) !important;
	}

	.gradient-overlay {
		background: linear-gradient(
			to top,
			rgba(0, 0, 0, 0.9) 0%,
			rgba(0, 0, 0, 0.7) 40%,
			rgba(0, 0, 0, 0.2) 100%
		);
	}

	/* Special text effects */
	.text-gradient {
		background-clip: text;
		-webkit-background-clip: text;
		color: transparent;
		background-image: linear-gradient(
			to right,
			var(--color-primary),
			var(--color-primary-light)
		);
	}
</style>
