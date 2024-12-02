<script setup>
	// No specific logic needed for now
</script>

<template>
	<footer class="bg-gray-900 text-white py-12 w-full">
		<div class="container mx-auto px-6 sm:px-8">
			<!-- Footer Content Section -->
			<div class="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-4 gap-8 mb-12">
				<!-- Logo and Description Section -->
				<div class="flex flex-col items-center md:items-start">
					<img src="@/img/logo.png" alt="Logo" class="h-40 mb-4" />
					<p class="text-gray-400 text-sm text-center md:text-left">
						العناية بمنزلك بأيدٍ محترفة
					</p>
					<div class="flex justify-between mt-4 w-40 max-w-xs">
						<a href="#" class="text-gray-400 hover:text-blue-500">
							<i class="fab fa-facebook-f"></i>
						</a>
						<a href="#" class="text-gray-400 hover:text-blue-500">
							<i class="fab fa-twitter"></i>
						</a>
						<a href="#" class="text-gray-400 hover:text-blue-500">
							<i class="fab fa-instagram"></i>
						</a>
					</div>
				</div>

				<!-- Links Section 1 -->
				<div class="flex flex-col">
					<h3 class="text-lg font-semibold mb-4">روابط سريعة</h3>
					<RouterLink to="" class="text-gray-400 hover:text-blue-500 mb-2"
						>من نحن</RouterLink
					>
					<RouterLink to="" class="text-gray-400 hover:text-blue-500 mb-2"
						>خدماتنا</RouterLink
					>
					<RouterLink to="" class="text-gray-400 hover:text-blue-500"
						>اتصل بنا</RouterLink
					>
				</div>

				<!-- Links Section 2 -->
				<div class="flex flex-col">
					<h3 class="text-lg font-semibold mb-4">خدمات شائعة</h3>
					<RouterLink to="" class="text-gray-400 hover:text-blue-500 mb-2"
						>تنظيف</RouterLink
					>
					<RouterLink to="" class="text-gray-400 hover:text-blue-500 mb-2"
						>أعمال الصيانة</RouterLink
					>
					<RouterLink to="" class="text-gray-400 hover:text-blue-500 mb-2"
						>السباكة</RouterLink
					>
					<RouterLink to="" class="text-gray-400 hover:text-blue-500"
						>الكهرباء</RouterLink
					>
				</div>

				<!-- Location Section -->
				<div class="flex flex-col">
					<h3 class="text-lg font-semibold mb-4">المواقع</h3>
					<RouterLink to="" class="text-gray-400 hover:text-blue-500 mb-2"
						>بغداد</RouterLink
					>
					<RouterLink to="" class="text-gray-400 hover:text-blue-500 mb-2"
						>اربيل</RouterLink
					>
				</div>
			</div>

			<!-- App Download Section -->
			<div class="flex justify-center gap-4 mb-8">
				<a
					href="https://www.apple.com/app-store/"
					target="_blank"
					class="text-gray-400 hover:text-blue-500"
				>
					<img
						src="@\img\iosStore.svg"
						alt="Download on the App Store"
						class="h-10"
					/>
				</a>
				<a
					href="https://play.google.com/store/apps"
					target="_blank"
					class="text-gray-400 hover:text-blue-500"
				>
					<img
						src="@\img\androidStore.svg"
						alt="Get it on Google Play"
						class="h-10"
					/>
				</a>
			</div>

			<!-- Footer Bottom Section -->
			<div class="border-t border-gray-700 pt-6 text-center">
				<p class="text-sm text-gray-500">© 2024 جميع الحقوق محفوظة.</p>
			</div>
		</div>
	</footer>
</template>

<style scoped>
	/* Importing icons (Font Awesome or any other icon set) */
	@import url("https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0-beta3/css/all.min.css");
</style>
