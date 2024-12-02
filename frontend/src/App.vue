<script setup>
	import { RouterView } from "vue-router";
	// import Navbar from "@/components/Navbar.vue";
	import Nav from "@/components/Nav.vue";
	import Footer from "@/components/Footer.vue";
</script>

<template>
	<Nav />
	<RouterView />
	<Footer />
</template>

<style scoped></style>
