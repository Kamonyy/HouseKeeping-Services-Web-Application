<script setup>
	import { ref, onMounted } from "vue";
	import axios from "axios";
	import CategoryCard from "./CategoryCard.vue";
	import { extractArrayFromResponse } from "../utils/apiUtils";
	import { RouterLink } from "vue-router";

	const categories = ref([]);
	const isLoading = ref(true);
	const currentTestimonialIndex = ref(0);

	// Fallback categories in case the API fails
	const fallbackCategories = [
		{
			id: 1,
			name: "تنظيف المنازل",
			description: "خدمات تنظيف شاملة للمنازل والشقق",
			icon: "fas fa-broom",
			image: "/img/categories/cleaning.jpg",
		},
		{
			id: 2,
			name: "صيانة الأجهزة",
			description: "إصلاح وصيانة الأجهزة المنزلية",
			icon: "fas fa-tools",
			image: "/img/categories/appliances.jpg",
		},
		{
			id: 3,
			name: "خدمات السباكة",
			description: "تركيب وإصلاح أنظمة السباكة",
			icon: "fas fa-faucet",
			image: "/img/categories/plumbing.jpg",
		},
		{
			id: 4,
			name: "خدمات كهربائية",
			description: "تركيب وإصلاح الأنظمة الكهربائية",
			icon: "fas fa-bolt",
			image: "/img/categories/electrical.jpg",
		},
	];

	// Fetch categories from API
	const fetchCategories = async () => {
		try {
			isLoading.value = true;
			const response = await axios.get("/api/category");

			// Use the utility function to handle different response formats
			const categoriesArray = extractArrayFromResponse(
				response.data,
				"categories"
			);

			if (categoriesArray && categoriesArray.length > 0) {
				categories.value = categoriesArray.slice(0, 4);
			} else {
				categories.value = fallbackCategories;
			}
		} catch (error) {
			console.error("Error fetching categories:", error);
			// Use fallback categories on error
			categories.value = fallbackCategories;
		} finally {
			isLoading.value = false;
		}
	};

	// Features list for the home page
	const features = [
		{
			title: "خدمات متنوعة",
			description:
				"نقدم مجموعة واسعة من الخدمات المنزلية والمكتبية لتلبية جميع احتياجاتك",
			icon: "fas fa-th-large",
		},
		{
			title: "مزودي خدمة محترفين",
			description:
				"جميع مزودي الخدمة لدينا مدربون ومؤهلون لتقديم خدمات عالية الجودة",
			icon: "fas fa-user-tie",
		},
		{
			title: "أسعار تنافسية",
			description: "نقدم أسعارًا معقولة وشفافة دون أي رسوم خفية أو إضافية",
			icon: "fas fa-tag",
		},
		{
			title: "حجز سهل وسريع",
			description: "منصة سهلة الاستخدام تمكنك من حجز الخدمة في دقائق معدودة",
			icon: "fas fa-bolt",
		},
	];

	// Testimonials for the home page
	const testimonials = [
		{
			name: "أحمد محمد",
			role: "مالك منزل",
			content:
				"استخدمت الموقع لحجز خدمة تنظيف منزلي، والنتيجة كانت رائعة! سأستخدم الخدمة مرة أخرى بالتأكيد.",
			avatar: "https://randomuser.me/api/portraits/men/32.jpg",
		},
		{
			name: "سارة علي",
			role: "مديرة مكتب",
			content:
				"وفرت علينا الكثير من الوقت والجهد في البحث عن خدمات صيانة موثوقة. الخدمة ممتازة والأسعار معقولة.",
			avatar: "https://randomuser.me/api/portraits/women/44.jpg",
		},
		{
			name: "محمد عبدالله",
			role: "صاحب شركة",
			content:
				"منصة رائعة تجمع أفضل مقدمي الخدمات في مكان واحد. سهلت علينا عملية الصيانة الدورية لمكاتبنا.",
			avatar: "https://randomuser.me/api/portraits/men/67.jpg",
		},
	];

	// Statistics for the home page
	const statistics = [
		{
			value: "1000+",
			label: "عميل سعيد",
			icon: "fas fa-smile",
		},
		{
			value: "500+",
			label: "مزود خدمة",
			icon: "fas fa-user-tie",
		},
		{
			value: "5000+",
			label: "خدمة منجزة",
			icon: "fas fa-check-circle",
		},
		{
			value: "24/7",
			label: "دعم متواصل",
			icon: "fas fa-headset",
		},
	];

	// Optimized animation setup function
	const setupScrollAnimation = () => {
		const animatedElements = document.querySelectorAll(".scroll-animate");
		const shouldSkipAnimation =
			window.matchMedia("(max-width: 768px)").matches ||
			window.matchMedia("(prefers-reduced-motion: reduce)").matches;

		// Skip animations on small screens or when reduced motion is preferred
		if (shouldSkipAnimation) {
			animatedElements.forEach((element) => {
				element.style.opacity = "1";
				element.style.transform = "none";
				element.classList.add("animated");
			});
			return;
		}

		// Create and configure Intersection Observer
		const observer = new IntersectionObserver(
			(entries) => {
				entries.forEach((entry) => {
					if (entry.isIntersecting) {
						// Add a small random delay for a more natural effect
						const delay = Math.random() * 0.3;
						entry.target.style.transitionDelay = `${delay}s`;

						// Add the animated class
						requestAnimationFrame(() => {
							entry.target.classList.add("animated");
						});

						// Stop observing after animation
						observer.unobserve(entry.target);
					}
				});
			},
			{
				threshold: 0.1,
				rootMargin: "0px 0px -50px 0px",
			}
		);

		// Start observing each element
		animatedElements.forEach((element) => observer.observe(element));
	};

	// Special animations for testimonials
	const setupTestimonialAnimations = () => {
		const testimonialElement = document.querySelector(".testimonial-card");
		if (!testimonialElement) return;

		// Create a special observer just for the testimonial
		const testimonialObserver = new IntersectionObserver(
			(entries) => {
				if (entries[0].isIntersecting) {
					// Start the testimonial animation sequence
					animateTestimonial();
					testimonialObserver.unobserve(testimonialElement);
				}
			},
			{
				threshold: 0.3,
				rootMargin: "0px",
			}
		);

		testimonialObserver.observe(testimonialElement);
	};

	// Animate the testimonial with a sequence of effects
	const animateTestimonial = () => {
		const card = document.querySelector(".testimonial-card");
		const avatar = document.querySelector(".testimonial-avatar");
		const content = document.querySelector(".testimonial-content");
		const quote = document.querySelector(".testimonial-quote");

		if (!card || !avatar || !content || !quote) return;

		// Reset initial states
		card.style.opacity = "0";
		avatar.style.opacity = "0";
		content.style.opacity = "0";
		quote.style.opacity = "0";

		avatar.style.transform = "translateY(30px)";
		content.style.transform = "translateX(30px)";
		quote.style.transform = "rotate(0deg) scale(0.5)";

		// Animate in sequence
		setTimeout(() => {
			card.style.transition = "opacity 0.8s ease-out";
			card.style.opacity = "1";
		}, 100);

		setTimeout(() => {
			quote.style.transition = "all 0.8s cubic-bezier(0.34, 1.56, 0.64, 1)";
			quote.style.opacity = "1";
			quote.style.transform = "rotate(10deg) scale(1)";
		}, 400);

		setTimeout(() => {
			avatar.style.transition = "all 0.8s cubic-bezier(0.34, 1.56, 0.64, 1)";
			avatar.style.opacity = "1";
			avatar.style.transform = "translateY(0)";
		}, 700);

		setTimeout(() => {
			content.style.transition = "all 0.8s cubic-bezier(0.34, 1.56, 0.64, 1)";
			content.style.opacity = "1";
			content.style.transform = "translateX(0)";
		}, 1000);
	};

	// Change testimonial with animation
	const nextTestimonial = () => {
		const testimonialCard = document.querySelector(".testimonial-content");
		const testimonialAvatar = document.querySelector(".testimonial-avatar");

		if (testimonialCard && testimonialAvatar) {
			// Animate out
			testimonialCard.style.opacity = "0";
			testimonialCard.style.transform = "translateX(30px)";
			testimonialAvatar.style.opacity = "0";

			setTimeout(() => {
				// Change testimonial index
				currentTestimonialIndex.value =
					(currentTestimonialIndex.value + 1) % testimonials.length;

				// Animate back in after a short delay
				setTimeout(() => {
					testimonialCard.style.transform = "translateX(-30px)";
					setTimeout(() => {
						testimonialCard.style.opacity = "1";
						testimonialCard.style.transform = "translateX(0)";
						testimonialAvatar.style.opacity = "1";
					}, 50);
				}, 300);
			}, 300);
		} else {
			// Fallback if elements not found
			currentTestimonialIndex.value =
				(currentTestimonialIndex.value + 1) % testimonials.length;
		}
	};

	const prevTestimonial = () => {
		const testimonialCard = document.querySelector(".testimonial-content");
		const testimonialAvatar = document.querySelector(".testimonial-avatar");

		if (testimonialCard && testimonialAvatar) {
			// Animate out
			testimonialCard.style.opacity = "0";
			testimonialCard.style.transform = "translateX(-30px)";
			testimonialAvatar.style.opacity = "0";

			setTimeout(() => {
				// Change testimonial index
				currentTestimonialIndex.value =
					(currentTestimonialIndex.value - 1 + testimonials.length) %
					testimonials.length;

				// Animate back in after a short delay
				setTimeout(() => {
					testimonialCard.style.transform = "translateX(30px)";
					setTimeout(() => {
						testimonialCard.style.opacity = "1";
						testimonialCard.style.transform = "translateX(0)";
						testimonialAvatar.style.opacity = "1";
					}, 50);
				}, 300);
			}, 300);
		} else {
			// Fallback if elements not found
			currentTestimonialIndex.value =
				(currentTestimonialIndex.value - 1 + testimonials.length) %
				testimonials.length;
		}
	};

	// Handle testimonial indicator clicks with animation
	const setTestimonialIndex = (index) => {
		if (index === currentTestimonialIndex.value) return;

		const testimonialCard = document.querySelector(".testimonial-content");
		const testimonialAvatar = document.querySelector(".testimonial-avatar");

		if (testimonialCard && testimonialAvatar) {
			// Determine direction for animation
			const isNext = index > currentTestimonialIndex.value;

			// Animate out
			testimonialCard.style.opacity = "0";
			testimonialCard.style.transform = isNext
				? "translateX(30px)"
				: "translateX(-30px)";
			testimonialAvatar.style.opacity = "0";

			setTimeout(() => {
				// Change testimonial index
				currentTestimonialIndex.value = index;

				// Animate back in after a short delay
				setTimeout(() => {
					testimonialCard.style.transform = isNext
						? "translateX(-30px)"
						: "translateX(30px)";
					setTimeout(() => {
						testimonialCard.style.opacity = "1";
						testimonialCard.style.transform = "translateX(0)";
						testimonialAvatar.style.opacity = "1";
					}, 50);
				}, 300);
			}, 300);
		} else {
			// Fallback if elements not found
			currentTestimonialIndex.value = index;
		}
	};

	// Fetch categories on component mount and setup animations
	onMounted(() => {
		fetchCategories();

		// Give elements time to render before setting up animations
		setTimeout(() => {
			setupScrollAnimation();

			// Refresh animations when scrolling
			window.addEventListener("scroll", () => {
				// Debounce the scroll event
				if (!window.scrollAnimationTimeout) {
					window.scrollAnimationTimeout = setTimeout(() => {
						setupScrollAnimation();
						window.scrollAnimationTimeout = null;
					}, 200);
				}
			});

			// Force refresh the animations when all images are loaded
			window.addEventListener("load", () => {
				console.log("Window fully loaded, refreshing animations");
				setupScrollAnimation();
			});
		}, 300); // Increased timeout to ensure DOM is fully ready
	});
</script>

<template>
	<div class="animate-fadeIn">
		<!-- Hero Section -->
		<section
			class="relative bg-gradient-to-br from-blue-50 via-white to-blue-50 py-32 overflow-hidden"
		>
			<!-- Background decorations -->
			<div class="absolute top-0 left-0 w-full h-full overflow-hidden z-0">
				<div class="bg-pattern opacity-10 absolute inset-0"></div>
				<div
					class="absolute top-20 left-20 w-80 h-80 bg-blue-100 rounded-full filter blur-3xl opacity-40"
				></div>
				<div
					class="absolute bottom-10 right-10 w-96 h-96 bg-blue-200 rounded-full filter blur-3xl opacity-40"
				></div>
				<div
					class="absolute top-40 right-40 w-64 h-64 bg-indigo-100 rounded-full filter blur-3xl opacity-30"
				></div>
			</div>

			<div class="container mx-auto px-4 relative z-10">
				<div
					class="flex flex-col md:flex-row items-center justify-between gap-16"
				>
					<div class="md:w-1/2 text-right rtl">
						<div
							class="inline-block bg-gradient-to-r from-blue-500 to-indigo-600 text-white px-6 py-3 rounded-full text-sm font-semibold mb-6 animate-pulse shadow-lg"
						>
							الحل الأمثل للعناية بمنزلك
						</div>
						<h1
							class="text-4xl md:text-5xl lg:text-6xl font-extrabold text-transparent bg-clip-text bg-gradient-to-r from-blue-700 via-indigo-600 to-blue-500 mb-6 leading-tight drop-shadow-sm"
						>
							الطريقة الأمثل للعناية بمنزلك
						</h1>
						<p class="text-gray-700 text-lg md:text-xl mb-10 leading-relaxed">
							منصة متكاملة تجمع أفضل مزودي خدمات التنظيف والصيانة والتركيب في
							مكان واحد. احجز الآن بأسعار تنافسية وجودة مضمونة.
						</p>
						<div class="flex gap-5 justify-start">
							<RouterLink
								to="/services"
								class="px-8 py-4 bg-gradient-to-r from-blue-600 to-indigo-600 text-white rounded-xl hover:from-blue-700 hover:text-white hover:to-indigo-700 transition-all duration-300 font-medium flex items-center gap-2 shadow-lg hover:shadow-blue-200/50 hover:-translate-y-1 text-base"
							>
								<i class="fas fa-search"></i>
								استكشف الخدمات
							</RouterLink>
							<RouterLink
								to="/help"
								class="px-8 py-4 bg-white text-blue-600 border border-blue-200 rounded-xl hover:bg-blue-50 transition-all duration-300 font-medium flex items-center gap-2 hover:-translate-y-1 shadow-md hover:shadow-lg text-base"
							>
								<i class="fas fa-info-circle"></i>
								كيف الاستخدام
							</RouterLink>
						</div>
					</div>
					<div class="md:w-1/2 mt-12 md:mt-0">
						<div class="relative">
							<div
								class="absolute -top-6 -right-6 w-72 h-72 bg-blue-200 rounded-full mix-blend-multiply filter blur-2xl opacity-70 animate-blob"
							></div>
							<div
								class="absolute -bottom-6 -left-6 w-72 h-72 bg-indigo-300 rounded-full mix-blend-multiply filter blur-2xl opacity-70 animate-blob animation-delay-2000"
							></div>
							<div
								class="absolute inset-0 w-72 h-72 bg-blue-400 rounded-full mix-blend-multiply filter blur-2xl opacity-70 animate-blob animation-delay-4000"
							></div>
							<img
								src="@/img/home-hero.png"
								alt="خدمات منزلية"
								class="relative rounded-2xl shadow-2xl w-full h-auto object-cover z-10 transform hover:scale-105 transition-transform duration-500"
							/>

							<!-- Floating badges -->
							<div
								class="absolute -top-4 -right-4 bg-white p-4 rounded-xl shadow-xl z-20 flex items-center gap-3 animate-float"
							>
								<div
									class="w-10 h-10 bg-blue-100 rounded-full flex items-center justify-center"
								>
									<i class="fas fa-shield-alt text-blue-600 text-xl"></i>
								</div>
								<span class="font-bold text-gray-800">جودة مضمونة</span>
							</div>
							<div
								class="absolute -bottom-4 -left-4 bg-white p-4 rounded-xl shadow-xl z-20 flex items-center gap-3 animate-float animation-delay-1000"
							>
								<div
									class="w-10 h-10 bg-blue-100 rounded-full flex items-center justify-center"
								>
									<i class="fas fa-clock text-blue-600 text-xl"></i>
								</div>
								<span class="font-bold text-gray-800">توفير الوقت</span>
							</div>
						</div>
					</div>
				</div>

				<!-- Statistics bar -->
				<div
					class="mt-20 bg-white shadow-xl rounded-2xl p-6 grid grid-cols-2 md:grid-cols-4 gap-4 rtl transform hover:shadow-2xl transition-shadow duration-500 scroll-animate fade-up"
				>
					<div
						v-for="(stat, index) in statistics"
						:key="index"
						class="flex flex-col items-center text-center p-4 border-r last:border-0 border-gray-100 scroll-animate fade-up"
						:style="{ 'transition-delay': index * 0.1 + 's' }"
					>
						<div
							class="w-16 h-16 bg-gradient-to-br from-blue-100 to-blue-200 rounded-full flex items-center justify-center mb-3"
						>
							<i :class="[stat.icon, 'text-blue-600 text-2xl']"></i>
						</div>
						<div class="text-2xl md:text-3xl font-bold text-gray-900 mb-1">
							{{ stat.value }}
						</div>
						<div class="text-gray-600">{{ stat.label }}</div>
					</div>
				</div>
			</div>
		</section>

		<!-- Divider -->
		<div class="relative h-24 overflow-hidden">
			<div
				class="absolute inset-0 bg-white"
				style="clip-path: polygon(0 0, 100% 0, 100% 100%, 0 30%)"
			></div>
			<div
				class="absolute inset-0 bg-gray-50"
				style="clip-path: polygon(0 30%, 100% 100%, 0 100%)"
			></div>
		</div>

		<!-- Features Section -->
		<section class="py-24 bg-gray-50">
			<div class="container mx-auto px-4">
				<div class="text-center mb-16 max-w-3xl mx-auto scroll-animate fade-up">
					<div
						class="inline-block bg-gradient-to-r from-blue-500 to-indigo-600 text-white px-6 py-3 rounded-full text-sm font-medium mb-4"
					>
						مميزاتنا
					</div>
					<h2 class="text-4xl font-bold text-gray-900 mb-4">لماذا تختارنا؟</h2>
					<p class="text-gray-600 text-lg">
						نقدم لك منصة متكاملة تلبي جميع احتياجاتك من خدمات المنزل والمكتب
						بأعلى معايير الجودة
					</p>
				</div>
				<div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-8 rtl">
					<div
						v-for="(feature, index) in features"
						:key="index"
						class="bg-white rounded-xl p-8 shadow-lg hover:shadow-2xl transition-all duration-500 border border-gray-100 flex flex-col items-center text-center transform hover:-translate-y-2 scroll-animate fade-up group overflow-hidden relative"
						:style="{ 'animation-delay': index * 0.1 + 's' }"
					>
						<!-- Background decoration -->
						<div
							class="absolute -top-24 -right-24 w-48 h-48 bg-blue-50 rounded-full opacity-0 group-hover:opacity-100 transition-all duration-700"
						></div>
						<div
							class="absolute -bottom-24 -left-24 w-48 h-48 bg-indigo-50 rounded-full opacity-0 group-hover:opacity-100 transition-all duration-700"
						></div>

						<!-- Icon container with gradient -->
						<div
							class="w-24 h-24 bg-gradient-to-br from-blue-100 to-blue-200 rounded-full flex items-center justify-center mb-6 group-hover:from-blue-200 group-hover:to-indigo-200 transition-all duration-300 relative z-10"
						>
							<i :class="[feature.icon, 'text-blue-600 text-3xl']"></i>
						</div>
						<h3 class="text-xl font-bold text-gray-900 mb-3 relative z-10">
							{{ feature.title }}
						</h3>
						<p class="text-gray-600 leading-relaxed relative z-10">
							{{ feature.description }}
						</p>
					</div>
				</div>
			</div>
		</section>

		<!-- Divider -->
		<div class="relative h-24 overflow-hidden">
			<div
				class="absolute inset-0 bg-gray-50"
				style="clip-path: polygon(0 0, 100% 0, 100% 30%, 0 100%)"
			></div>
			<div
				class="absolute inset-0 bg-white"
				style="clip-path: polygon(100% 30%, 100% 100%, 0 100%)"
			></div>
		</div>

		<!-- Categories Section -->
		<section class="py-24 bg-white">
			<div class="container mx-auto px-4">
				<div
					class="flex flex-col md:flex-row justify-between items-center mb-16 rtl scroll-animate fade-right"
				>
					<div>
						<div
							class="inline-block bg-gradient-to-r from-blue-500 to-indigo-600 text-white px-6 py-3 rounded-full text-sm font-medium mb-4"
						>
							خدماتنا
						</div>
						<h2 class="text-4xl font-bold text-gray-900 mb-3">
							استكشف خدماتنا
						</h2>
						<p class="text-gray-600 text-lg">
							مجموعة متنوعة من الخدمات لتلبية جميع احتياجاتك
						</p>
					</div>
					<RouterLink
						to="/services"
						class="mt-6 md:mt-0 text-white hover:text-white transition-all duration-300 font-semibold text-lg relative group flex items-center bg-gradient-to-r from-blue-600 to-indigo-600 hover:from-blue-700 hover:to-indigo-700 px-8 py-4 rounded-xl shadow-lg hover:shadow-xl"
					>
						عرض جميع الخدمات
						<i
							class="fas fa-arrow-left mr-2 transition-transform duration-300 group-hover:-translate-x-1"
						></i>
					</RouterLink>
				</div>

				<div v-if="isLoading" class="flex justify-center items-center py-16">
					<div class="flex flex-col items-center">
						<div
							class="w-20 h-20 border-4 border-blue-500 border-t-transparent rounded-full animate-spin mb-4"
						></div>
						<p class="text-blue-600 font-medium">جاري تحميل الخدمات...</p>
					</div>
				</div>

				<div
					v-else-if="categories && categories.length > 0"
					class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-8 rtl"
				>
					<CategoryCard
						v-for="(category, index) in categories"
						:key="category.id || index"
						:category="category"
						class="scroll-animate fade-up opacity-100 transform hover:-translate-y-2 transition-all duration-500 hover:shadow-xl"
						:style="{ 'animation-delay': index * 0.1 + 's' }"
					/>
				</div>

				<!-- No categories fallback -->
				<div v-else class="text-center py-16 bg-gray-50 rounded-2xl shadow">
					<i class="fas fa-exclamation-circle text-blue-500 text-5xl mb-6"></i>
					<p class="text-xl text-gray-600 mb-2">
						لم يتم العثور على فئات خدمات حالياً
					</p>
					<p class="text-base text-gray-500 mt-2">
						سيتم إضافة المزيد من الخدمات قريبًا
					</p>
				</div>
			</div>
		</section>

		<!-- Divider with accent -->
		<div class="relative h-24 overflow-hidden">
			<div
				class="absolute inset-0 bg-white"
				style="clip-path: polygon(0 0, 100% 0, 100% 100%, 0 30%)"
			></div>
			<div
				class="absolute inset-0 bg-blue-50"
				style="clip-path: polygon(0 30%, 100% 100%, 0 100%)"
			></div>
		</div>

		<!-- How It Works Section -->
		<section class="py-24 bg-blue-50 relative overflow-hidden">
			<!-- Background elements -->
			<div class="absolute inset-0 opacity-10">
				<div
					class="absolute top-20 left-20 w-64 h-64 bg-blue-200 rounded-full"
				></div>
				<div
					class="absolute bottom-20 right-20 w-80 h-80 bg-indigo-200 rounded-full"
				></div>
			</div>

			<div class="container mx-auto px-4 relative z-10">
				<div class="text-center mb-20 max-w-3xl mx-auto scroll-animate fade-up">
					<div
						class="inline-block bg-gradient-to-r from-blue-500 to-indigo-600 text-white px-6 py-3 rounded-full text-sm font-medium mb-4"
					>
						الخطوات
					</div>
					<h2 class="text-4xl font-bold text-gray-900 mb-4">كيف تعمل المنصة</h2>
					<p class="text-gray-600 text-lg">
						عملية بسيطة وسهلة للحصول على الخدمة التي تحتاجها
					</p>
				</div>

				<div class="grid grid-cols-1 md:grid-cols-3 gap-10 rtl">
					<div
						class="relative scroll-animate slide-up"
						style="animation-delay: 0.1s"
					>
						<div
							class="bg-white rounded-2xl p-8 relative z-10 shadow-xl hover:shadow-2xl transition-all duration-500 border-b-4 border-blue-500 group hover:-translate-y-2"
						>
							<div
								class="w-20 h-20 bg-gradient-to-br from-blue-600 to-indigo-600 rounded-full flex items-center justify-center text-white font-bold text-2xl mb-8 shadow-lg group-hover:scale-110 transition-transform duration-300"
							>
								1
							</div>
							<h3 class="text-2xl font-bold text-gray-900 mb-4">اختر الخدمة</h3>
							<p class="text-gray-600 text-lg leading-relaxed">
								تصفح مجموعة متنوعة من الخدمات واختر ما يناسب احتياجاتك
							</p>
							<div class="mt-6 text-blue-600">
								<i class="fas fa-long-arrow-alt-left text-2xl"></i>
							</div>
						</div>
					</div>

					<div
						class="relative scroll-animate slide-up"
						style="animation-delay: 0.2s"
					>
						<div
							class="bg-white rounded-2xl p-8 relative z-10 shadow-xl hover:shadow-2xl transition-all duration-500 border-b-4 border-blue-500 group hover:-translate-y-2"
						>
							<div
								class="w-20 h-20 bg-gradient-to-br from-blue-600 to-indigo-600 rounded-full flex items-center justify-center text-white font-bold text-2xl mb-8 shadow-lg group-hover:scale-110 transition-transform duration-300"
							>
								2
							</div>
							<h3 class="text-2xl font-bold text-gray-900 mb-4">احجز موعدًا</h3>
							<p class="text-gray-600 text-lg leading-relaxed">
								حدد الوقت المناسب لك واملأ بيانات الحجز البسيطة
							</p>
							<div class="mt-6 text-blue-600">
								<i class="fas fa-long-arrow-alt-left text-2xl"></i>
							</div>
						</div>
					</div>

					<div class="scroll-animate slide-up" style="animation-delay: 0.3s">
						<div
							class="bg-white rounded-2xl p-8 relative z-10 shadow-xl hover:shadow-2xl transition-all duration-500 border-b-4 border-blue-500 group hover:-translate-y-2"
						>
							<div
								class="w-20 h-20 bg-gradient-to-br from-blue-600 to-indigo-600 rounded-full flex items-center justify-center text-white font-bold text-2xl mb-8 shadow-lg group-hover:scale-110 transition-transform duration-300"
							>
								3
							</div>
							<h3 class="text-2xl font-bold text-gray-900 mb-4">
								استمتع بالخدمة
							</h3>
							<p class="text-gray-600 text-lg leading-relaxed">
								سيصل إليك مزود الخدمة في الموعد المحدد لتنفيذ الخدمة باحترافية
							</p>
							<div class="mt-6 text-green-600">
								<i class="fas fa-check-circle text-2xl"></i>
							</div>
						</div>
					</div>
				</div>
			</div>
		</section>

		<!-- Divider -->
		<div class="relative h-24 overflow-hidden">
			<div
				class="absolute inset-0 bg-blue-50"
				style="clip-path: polygon(0 0, 100% 0, 100% 30%, 0 100%)"
			></div>
			<div
				class="absolute inset-0 bg-gray-50"
				style="clip-path: polygon(100% 30%, 100% 100%, 0 100%)"
			></div>
		</div>

		<!-- Testimonials Section -->
		<section class="py-24 bg-gray-50 relative overflow-hidden">
			<!-- Background elements -->
			<div class="absolute inset-0 opacity-10">
				<div
					class="absolute top-10 right-10 w-64 h-64 bg-blue-200 rounded-full"
				></div>
				<div
					class="absolute bottom-10 left-10 w-80 h-80 bg-indigo-200 rounded-full"
				></div>
			</div>

			<div class="container mx-auto px-4 relative z-10">
				<div class="text-center mb-16 max-w-3xl mx-auto scroll-animate fade-up">
					<div
						class="inline-block bg-gradient-to-r from-blue-500 to-indigo-600 text-white px-6 py-3 rounded-full text-sm font-medium mb-4"
					>
						الآراء
					</div>
					<h2 class="text-4xl font-bold text-gray-900 mb-4">آراء عملائنا</h2>
					<p class="text-gray-600 text-lg">تعرف على تجارب عملائنا مع خدماتنا</p>
				</div>

				<!-- Testimonial Carousel -->
				<div class="max-w-5xl mx-auto relative">
					<div
						class="bg-white rounded-2xl p-10 shadow-xl border border-gray-100 rtl relative testimonial-card"
					>
						<!-- Decorative elements -->
						<div
							class="absolute top-0 right-0 w-40 h-40 bg-blue-50 rounded-full -translate-y-1/2 translate-x-1/2 opacity-70"
						></div>
						<div
							class="absolute bottom-0 left-0 w-40 h-40 bg-indigo-50 rounded-full translate-y-1/2 -translate-x-1/2 opacity-70"
						></div>

						<div
							class="absolute -top-5 -right-5 w-14 h-14 bg-gradient-to-br from-blue-500 to-indigo-600 rounded-full flex items-center justify-center text-white shadow-lg testimonial-quote"
						>
							<i class="fas fa-quote-right text-xl"></i>
						</div>

						<div
							class="flex flex-col md:flex-row gap-10 items-center relative z-10"
						>
							<div
								class="md:w-1/3 flex flex-col items-center testimonial-avatar"
							>
								<div class="relative">
									<div
										class="absolute inset-0 bg-gradient-to-br from-blue-500 to-indigo-600 rounded-full opacity-20 blur-md transform scale-110"
									></div>
									<img
										:src="testimonials[currentTestimonialIndex].avatar"
										:alt="testimonials[currentTestimonialIndex].name"
										class="w-36 h-36 rounded-full object-cover border-4 border-white shadow-xl mb-6 relative"
									/>
								</div>
								<h4 class="font-bold text-2xl text-gray-900 text-center">
									{{ testimonials[currentTestimonialIndex].name }}
								</h4>
								<p class="text-blue-600 font-medium text-center text-lg mb-3">
									{{ testimonials[currentTestimonialIndex].role }}
								</p>
								<div class="flex mt-2">
									<i class="fas fa-star text-yellow-400 text-xl mx-0.5"></i>
									<i class="fas fa-star text-yellow-400 text-xl mx-0.5"></i>
									<i class="fas fa-star text-yellow-400 text-xl mx-0.5"></i>
									<i class="fas fa-star text-yellow-400 text-xl mx-0.5"></i>
									<i class="fas fa-star text-yellow-400 text-xl mx-0.5"></i>
								</div>
							</div>

							<div class="md:w-2/3 testimonial-content">
								<p class="text-gray-700 text-xl leading-relaxed italic">
									"{{ testimonials[currentTestimonialIndex].content }}"
								</p>
							</div>
						</div>
					</div>

					<!-- Navigation Buttons -->
					<div class="flex justify-center mt-10 gap-6">
						<button
							@click="prevTestimonial"
							class="w-14 h-14 rounded-full bg-white border border-gray-200 flex items-center justify-center hover:bg-blue-50 transition-colors shadow-md hover:shadow-lg"
						>
							<i class="fas fa-arrow-right text-blue-600 text-xl"></i>
						</button>
						<button
							@click="nextTestimonial"
							class="w-14 h-14 rounded-full bg-gradient-to-r from-blue-600 to-indigo-600 flex items-center justify-center hover:from-blue-700 hover:to-indigo-700 transition-colors shadow-md hover:shadow-lg"
						>
							<i class="fas fa-arrow-left text-white text-xl"></i>
						</button>
					</div>

					<!-- Indicators -->
					<div class="flex justify-center mt-6 gap-3">
						<button
							v-for="(_, index) in testimonials"
							:key="index"
							@click="setTestimonialIndex(index)"
							class="w-3 h-3 rounded-full transition-all duration-300"
							:class="
								currentTestimonialIndex === index
									? 'bg-gradient-to-r from-blue-600 to-indigo-600 w-10'
									: 'bg-gray-300'
							"
						></button>
					</div>
				</div>
			</div>
		</section>

		<!-- Divider -->
		<div class="relative h-24 overflow-hidden">
			<div
				class="absolute inset-0 bg-gray-50"
				style="clip-path: polygon(0 0, 100% 0, 100% 100%, 0 30%)"
			></div>
			<div
				class="absolute inset-0 bg-gradient-to-r from-blue-600 to-blue-800"
				style="clip-path: polygon(0 30%, 100% 100%, 0 100%)"
			></div>
		</div>

		<!-- CTA Section -->
		<section
			class="py-24 bg-gradient-to-br from-blue-600 via-indigo-600 to-blue-800 text-white relative overflow-hidden"
		>
			<!-- Background decorative elements -->
			<div class="absolute inset-0 overflow-hidden">
				<div
					class="absolute top-0 left-0 w-96 h-96 bg-white rounded-full opacity-10 -translate-y-1/2 -translate-x-1/2"
				></div>
				<div
					class="absolute bottom-0 right-0 w-96 h-96 bg-white rounded-full opacity-10 translate-y-1/2 translate-x-1/2"
				></div>
				<div
					class="absolute top-1/4 right-1/4 w-64 h-64 bg-indigo-300 rounded-full opacity-20 blur-3xl"
				></div>
				<div
					class="absolute bottom-1/4 left-1/4 w-64 h-64 bg-blue-300 rounded-full opacity-20 blur-3xl"
				></div>
			</div>

			<div class="container mx-auto px-4 text-center relative z-10">
				<div class="max-w-4xl mx-auto scroll-animate fade-up">
					<h2
						class="text-white text-4xl md:text-5xl font-bold mb-8 leading-tight"
					>
						ابدأ الآن في الحصول على أفضل الخدمات
					</h2>
					<p
						class="text-xl md:text-2xl mb-12 opacity-90 max-w-3xl mx-auto text-white"
					>
						انضم إلى آلاف العملاء الراضين واحصل على خدمات عالية الجودة بأسعار
						تنافسية
					</p>
					<div class="flex flex-col sm:flex-row justify-center gap-6">
						<RouterLink
							to="/services"
							class="px-10 py-5 bg-white text-blue-600 rounded-xl hover:bg-blue-50 transition-all duration-300 font-bold text-lg shadow-xl hover:shadow-2xl hover:-translate-y-1 flex items-center justify-center gap-3"
						>
							<i class="fas fa-search text-xl"></i>
							استكشف الخدمات
						</RouterLink>
						<RouterLink
							to="/login"
							class="px-10 py-5 bg-transparent border-2 border-white text-white rounded-xl hover:bg-white/10 hover:text-white transition-all duration-300 font-bold text-lg hover:-translate-y-1 flex items-center justify-center gap-3"
						>
							<i class="fas fa-sign-in-alt text-xl"></i>
							تسجيل الدخول
						</RouterLink>
					</div>

					<!-- Trust badges -->
					<div class="mt-16 grid grid-cols-2 md:grid-cols-4 gap-6">
						<div
							class="bg-white/10 rounded-xl p-4 backdrop-blur-sm scroll-animate fade-up"
							style="transition-delay: 0.1s"
						>
							<i class="fas fa-shield-alt text-3xl mb-3 text-yellow-300"></i>
							<p class="font-medium text-white">خدمة آمنة ومضمونة</p>
						</div>
						<div
							class="bg-white/10 rounded-xl p-4 backdrop-blur-sm scroll-animate fade-up"
							style="transition-delay: 0.2s"
						>
							<i class="fas fa-clock text-3xl mb-3 text-yellow-300"></i>
							<p class="font-medium text-white">توفير الوقت والجهد</p>
						</div>
						<div
							class="bg-white/10 rounded-xl p-4 backdrop-blur-sm scroll-animate fade-up"
							style="transition-delay: 0.3s"
						>
							<i class="fas fa-tag text-3xl mb-3 text-yellow-300"></i>
							<p class="font-medium text-white">أسعار تنافسية</p>
						</div>
						<div
							class="bg-white/10 rounded-xl p-4 backdrop-blur-sm scroll-animate fade-up"
							style="transition-delay: 0.4s"
						>
							<i class="fas fa-headset text-3xl mb-3 text-yellow-300"></i>
							<p class="font-medium text-white">دعم على مدار الساعة</p>
						</div>
					</div>
				</div>
			</div>
		</section>
	</div>
</template>

<style scoped>
	.rtl {
		direction: rtl;
		text-align: right;
	}

	.bg-pattern {
		background-image: url("data:image/svg+xml,%3Csvg width='60' height='60' viewBox='0 0 60 60' xmlns='http://www.w3.org/2000/svg'%3E%3Cg fill='none' fill-rule='evenodd'%3E%3Cg fill='%233b82f6' fill-opacity='0.1'%3E%3Cpath d='M36 34v-4h-2v4h-4v2h4v4h2v-4h4v-2h-4zm0-30V0h-2v4h-4v2h4v4h2V6h4V4h-4zM6 34v-4H4v4H0v2h4v4h2v-4h4v-2H6zM6 4V0H4v4H0v2h4v4h2V6h4V4H6z'/%3E%3C/g%3E%3C/g%3E%3C/svg%3E");
	}

	@keyframes fadeIn {
		from {
			opacity: 0;
			transform: translateY(20px);
		}
		to {
			opacity: 1;
			transform: translateY(0);
		}
	}

	.animate-fadeIn {
		animation: fadeIn 1s cubic-bezier(0.25, 0.46, 0.45, 0.94);
	}

	@keyframes blob {
		0% {
			transform: translate(0px, 0px) scale(1);
		}
		33% {
			transform: translate(30px, -50px) scale(1.1);
		}
		66% {
			transform: translate(-20px, 20px) scale(0.9);
		}
		100% {
			transform: translate(0px, 0px) scale(1);
		}
	}

	.animate-blob {
		animation: blob 10s infinite ease-in-out;
	}

	@keyframes float {
		0% {
			transform: translateY(0px);
		}
		50% {
			transform: translateY(-10px);
		}
		100% {
			transform: translateY(0px);
		}
	}

	.animate-float {
		animation: float 5s ease-in-out infinite;
	}

	.animation-delay-1000 {
		animation-delay: 1s;
	}

	.animation-delay-2000 {
		animation-delay: 2s;
	}

	.animation-delay-4000 {
		animation-delay: 4s;
	}

	/* Scroll Animation Classes - Enhanced for better visibility */
	.scroll-animate {
		opacity: 0;
		transition: all 1s cubic-bezier(0.25, 0.46, 0.45, 0.94);
		will-change: opacity, transform;
		transform-style: preserve-3d;
		backface-visibility: hidden;
	}

	.scroll-animate.animated {
		opacity: 1 !important;
		transform: none !important;
	}

	/* Force animated elements to be visible regardless of scroll position on smaller screens */
	@media (max-width: 768px) {
		.scroll-animate {
			opacity: 1 !important;
			transform: none !important;
		}
	}

	/* Different animation types with improved distances */
	.fade-up {
		transform: translateY(40px);
	}

	.fade-right {
		transform: translateX(-40px);
	}

	.fade-left {
		transform: translateX(40px);
	}

	.zoom-in {
		transform: scale(0.9);
	}

	.slide-up {
		transform: translateY(50px);
	}

	/* Testimonial specific animations */
	.testimonial-card {
		transition: opacity 0.8s ease-out;
	}

	.testimonial-avatar,
	.testimonial-content,
	.testimonial-quote {
		transition: all 0.8s cubic-bezier(0.34, 1.56, 0.64, 1);
	}

	/* Staggered animation for sections */
	.scroll-animate:nth-child(1) {
		transition-delay: 0.1s;
	}
	.scroll-animate:nth-child(2) {
		transition-delay: 0.2s;
	}
	.scroll-animate:nth-child(3) {
		transition-delay: 0.3s;
	}
	.scroll-animate:nth-child(4) {
		transition-delay: 0.4s;
	}
</style>
