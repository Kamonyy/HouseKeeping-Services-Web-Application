<script setup>
	// This component doesn't need script logic
</script>

<template>
	<!-- This is an invisible component that applies global styles -->
	<div class="hidden"></div>
</template>

<style>
	/* Global Typography Enhancements */
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

	body {
		font-family: "Cairo", sans-serif;
		font-size: 16px;
		line-height: 1.6;
		color: var(--color-gray-800);
		text-shadow: 0 0 1px rgba(0, 0, 0, 0.01);
		-webkit-font-smoothing: antialiased;
		-moz-osx-font-smoothing: grayscale;
		background-color: var(--color-gray-50);
	}

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

	p {
		margin-bottom: 1rem;
		font-size: 1rem;
		line-height: 1.7;
		color: var(--color-gray-700);
	}

	/* Fancy Text Effects */
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

	.text-shadow-sm {
		text-shadow: 0 1px 2px rgba(0, 0, 0, 0.1);
	}

	.drop-shadow-text {
		filter: drop-shadow(0 1px 1px rgba(0, 0, 0, 0.1));
	}

	/* Enhanced Links */
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

	/* RTL-specific typography */
	.rtl h1,
	.rtl h2,
	.rtl h3,
	.rtl h4,
	.rtl h5,
	.rtl h6,
	.rtl p,
	.rtl span,
	.rtl a,
	.rtl div {
		font-family: "Cairo", sans-serif;
		letter-spacing: -0.01em;
	}

	/* Enhance readability for Arabic text */
	.rtl {
		line-height: 1.8;
		font-feature-settings: "kern", "liga", "calt";
	}

	/* Enhance button text */
	button,
	.btn {
		font-family: "Cairo", sans-serif;
		font-weight: 600;
		letter-spacing: 0.01em;
	}

	/* Improved form text */
	input,
	textarea,
	select {
		font-family: "Cairo", sans-serif;
		color: var(--color-gray-800);
		font-weight: 500;
	}

	/* Reduce opacity of placeholder text */
	::placeholder {
		opacity: 0.7;
		color: var(--color-gray-500);
	}

	/* Improved text contrast classes */
	.text-primary {
		color: var(--color-primary);
	}

	.text-secondary {
		color: var(--color-secondary);
	}

	.text-dark {
		color: var(--color-gray-900);
		font-weight: 600;
	}

	.text-medium {
		color: var(--color-gray-700);
	}

	.text-light {
		color: var(--color-gray-600);
	}

	/* Enhanced focus styles for accessibility */
	button:focus,
	a:focus,
	input:focus,
	select:focus,
	textarea:focus {
		outline: 2px solid var(--color-primary-light);
		outline-offset: 2px;
	}

	/* Improved visibility for labels */
	label {
		color: var(--color-gray-800);
		font-weight: 600;
		margin-bottom: 0.25rem;
		display: inline-block;
	}

	/* Global utility classes for text colors */
	.text-primary {
		color: var(--color-primary) !important;
		font-weight: 600 !important;
	}

	.text-primary-dark {
		color: var(--color-primary-dark) !important;
		font-weight: 600 !important;
	}

	.text-secondary {
		color: var(--color-secondary) !important;
		font-weight: 600 !important;
	}

	.text-color-dark {
		color: var(--color-gray-800) !important;
		font-weight: 500 !important;
	}

	.text-color-medium {
		color: var(--color-gray-700) !important;
		font-weight: 500 !important;
	}

	.text-color-light {
		color: var(--color-gray-600) !important;
	}

	/* Card title styles for better visibility */
	.card-title,
	.category-name,
	.service-title {
		color: white !important;
		font-weight: 700 !important;
		text-shadow: 0 2px 4px rgba(0, 0, 0, 0.7) !important;
	}

	/* Ensure gradient overlays provide good contrast */
	.gradient-overlay {
		background: linear-gradient(
			to top,
			rgba(0, 0, 0, 0.9) 0%,
			rgba(0, 0, 0, 0.7) 40%,
			rgba(0, 0, 0, 0.2) 100%
		);
	}
</style>
