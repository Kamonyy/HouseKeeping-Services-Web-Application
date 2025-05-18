import { fileURLToPath, URL } from "node:url";

import { defineConfig } from "vite";
import vue from "@vitejs/plugin-vue";

// https://vitejs.dev/config/
export default defineConfig(({ mode }) => {
	const isProd = mode === "production";

	return {
		plugins: [vue()],
		server: {
			port: 3000,
			proxy: {
				"/api": {
					target: "https://localhost:7007",
					changeOrigin: true,
					secure: false,
				},
			},
		},
		resolve: {
			alias: {
				"@": fileURLToPath(new URL("./src", import.meta.url)),
			},
		},
		build: {
			// Production optimizations
			minify: isProd ? "terser" : false,
			terserOptions: {
				compress: {
					drop_console: isProd,
					drop_debugger: isProd,
				},
			},
			// Split chunks for better caching
			rollupOptions: {
				output: {
					manualChunks: {
						vue: ["vue", "vue-router", "pinia"],
						vendor: ["axios", "jwt-decode"],
					},
				},
			},
			// Generate sourcemaps only in development
			sourcemap: !isProd,
		},
	};
});
