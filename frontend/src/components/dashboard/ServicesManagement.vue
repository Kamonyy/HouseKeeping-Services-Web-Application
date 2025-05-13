<template>
	<div class="space-y-6 animate-fadeIn">
		<div class="flex justify-between items-center">
			<h1 class="text-3xl font-bold text-gray-800">إدارة الخدمات</h1>
		</div>

		<!-- Services Filter Controls -->
		<div
			class="bg-gradient-to-br from-white to-blue-50 rounded-xl shadow-md p-4"
		>
			<div class="flex flex-wrap gap-4 items-center justify-between">
				<div class="flex gap-4 flex-1">
					<div class="flex-1">
						<label
							for="status-filter"
							class="block text-sm font-medium text-gray-700 mb-1"
							>تصفية حسب الحالة</label
						>
						<select
							id="status-filter"
							v-model="statusFilter"
							class="w-full border border-gray-300 rounded-lg p-2.5 focus:ring-blue-500 focus:border-blue-500 transition-all"
						>
							<option value="all">جميع الخدمات</option>
							<option value="pending">بانتظار الموافقة</option>
							<option value="approved">تمت الموافقة</option>
						</select>
					</div>
					<div class="flex-1">
						<label
							for="search"
							class="block text-sm font-medium text-gray-700 mb-1"
							>بحث</label
						>
						<input
							type="text"
							id="search"
							v-model="searchQuery"
							placeholder="ابحث عن عنوان الخدمة..."
							class="w-full border border-gray-300 rounded-lg p-2.5 focus:ring-blue-500 focus:border-blue-500 transition-all"
						/>
					</div>
				</div>
			</div>
		</div>

		<!-- Services List -->
		<div
			class="bg-gradient-to-br from-white to-blue-50 rounded-xl shadow-md overflow-hidden"
		>
			<div
				class="bg-gradient-to-r from-blue-600 to-blue-700 text-white p-4 flex justify-between items-center"
			>
				<p class="flex items-center gap-2 text-2xl text-white font-bold">
					<i class="fas fa-list mr-2"></i>
					قائمة الخدمات
				</p>

				<button
					@click="openAddServiceModal"
					class="px-4 py-2.5 bg-gradient-to-r from-blue-600 to-blue-700 text-white rounded-lg hover:from-blue-700 hover:to-blue-800 flex items-center gap-2 transition-all shadow-md hover:shadow-lg transform hover:scale-105"
				>
					<span>إضافة خدمة جديدة</span>
					<i class="fas fa-plus-circle"></i>
				</button>
			</div>

			<div class="overflow-x-auto">
				<table class="min-w-full divide-y divide-gray-200">
					<thead class="bg-blue-50">
						<tr>
							<th
								class="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider"
							>
								عنوان الخدمة
							</th>
							<th
								class="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider"
							>
								مقدم الخدمة
							</th>
							<th
								class="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider"
							>
								السعر
							</th>
							<th
								class="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider"
							>
								الحالة
							</th>
							<th
								class="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider"
							>
								تاريخ الإنشاء
							</th>
							<th
								class="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider"
							>
								خيارات
							</th>
						</tr>
					</thead>
					<tbody class="bg-white divide-y divide-gray-200">
						<template v-if="filteredServices.length > 0">
							<tr
								v-for="service in filteredServices"
								:key="service.id"
								class="hover:bg-blue-50/30 transition-colors"
							>
								<td class="px-6 py-4 whitespace-nowrap text-right text-sm">
									<div class="font-medium text-gray-900">
										{{ service.title }}
									</div>
								</td>
								<td
									class="px-6 py-4 whitespace-nowrap text-right text-sm text-gray-500"
								>
									{{ service.providerUsername }}
								</td>
								<td
									class="px-6 py-4 whitespace-nowrap text-right text-sm text-gray-500"
								>
									{{
										service.estimatedPrice
											? service.estimatedPrice.toLocaleString()
											: "0"
									}}
									د.ع
								</td>
								<td class="px-6 py-4 whitespace-nowrap text-right text-sm">
									<span
										class="px-2 py-1 inline-flex text-xs leading-5 font-semibold rounded-full"
										:class="{
											'bg-green-100 text-green-800': service.isApproved,
											'bg-yellow-100 text-yellow-800': !service.isApproved,
										}"
									>
										{{ service.isApproved ? "معتمدة" : "بانتظار الموافقة" }}
									</span>
								</td>
								<td
									class="px-6 py-4 whitespace-nowrap text-right text-sm text-gray-500"
								>
									{{ formatDate(service.createdDate) }}
								</td>
								<td class="px-6 py-4 whitespace-nowrap text-right text-sm">
									<div class="flex gap-2 justify-end">
										<button
											@click.prevent="viewDetails(service)"
											class="px-3 py-1.5 bg-blue-600 text-white text-sm rounded-lg hover:bg-blue-700 transition-colors"
										>
											<i class="fas fa-eye mr-1"></i>
											عرض
										</button>
										<button
											v-if="!service.isApproved"
											@click="approveService(service.id)"
											class="px-3 py-1.5 bg-green-600 text-white text-sm rounded-lg hover:bg-green-700 transition-colors"
										>
											<i class="fas fa-check mr-1"></i>
											الموافقة
										</button>
										<button
											v-if="service.isApproved"
											@click="revokeApproval(service.id)"
											class="px-3 py-1.5 bg-orange-400 text-white text-sm rounded-lg hover:bg-orange-500 transition-colors"
										>
											<i class="fas fa-ban mr-1"></i>
											إلغاء
										</button>
										<button
											v-if="service.isApproved"
											@click="confirmDeleteService(service)"
											class="px-3 py-1.5 bg-red-600 text-white text-sm rounded-lg hover:bg-red-700 transition-colors"
										>
											<i class="fas fa-trash mr-1"></i>
											حذف
										</button>
									</div>
								</td>
							</tr>
						</template>
						<tr v-else>
							<td
								colspan="6"
								class="px-6 py-10 text-center text-gray-500 whitespace-nowrap"
							>
								<div v-if="loading">
									<div
										class="inline-block w-8 h-8 border-4 border-blue-600 border-t-transparent rounded-full animate-spin"
									></div>
									<p class="mt-2">جاري تحميل البيانات...</p>
								</div>
								<div
									v-else
									class="py-8 bg-blue-50/50 rounded-lg border border-blue-100 mx-4"
								>
									<i class="fas fa-search text-blue-300 text-5xl mb-4"></i>
									<p class="text-lg font-medium">
										لا توجد خدمات متطابقة مع معايير البحث
									</p>
									<p class="text-gray-400 mt-2">
										جرب تغيير معايير البحث أو إضافة خدمات جديدة
									</p>
								</div>
							</td>
						</tr>
					</tbody>
				</table>
			</div>
		</div>

		<!-- Add Service Modal -->
		<Modal
			:is-open="showServiceForm"
			title="إضافة خدمة جديدة"
			size="lg"
			@close="closeServiceForm"
		>
			<template #header-icon>
				<div class="bg-white/20 p-2 rounded-full">
					<i class="fas fa-briefcase"></i>
				</div>
			</template>

			<div class="space-y-6">
				<ServiceForm
					:categories="categories"
					:is-editing="false"
					:initial-data="initialServiceData"
					:hide-header="true"
					@submit="handleServiceSubmit"
					@cancel="closeServiceForm"
					@reload-categories="handleReloadCategories"
				/>
			</div>
		</Modal>

		<!-- Service Details Modal -->
		<div
			v-if="selectedService"
			class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50 px-4"
			style="backdrop-filter: blur(4px)"
		>
			<div
				class="bg-white rounded-xl shadow-xl w-full max-w-2xl mx-auto overflow-hidden animate-fadeIn"
				style="
					margin-top: 5vh;
					animation: modal-appear 0.3s cubic-bezier(0.16, 1, 0.3, 1);
				"
			>
				<div
					class="flex justify-between items-center p-5 border-b border-gray-200 bg-gradient-to-r from-blue-600 via-blue-500 to-blue-700"
					style="flex-direction: row-reverse"
				>
					<button
						@click="closeDetailsModal"
						class="text-white hover:text-gray-200 w-8 h-8 flex items-center justify-center rounded-full bg-white/20 hover:bg-white/30 transition-colors"
					>
						<i class="fas fa-times"></i>
					</button>
					<h3 class="text-xl font-bold text-white flex items-center gap-3">
						<div class="bg-white/20 p-2 rounded-full">
							<i class="fas fa-info-circle"></i>
						</div>
						تفاصيل الخدمة
					</h3>
				</div>

				<div class="p-6 max-h-[calc(90vh-8rem)] overflow-y-auto">
					<!-- Loading indicator -->
					<div
						v-if="loading"
						class="flex flex-col items-center justify-center py-10"
					>
						<div
							class="w-12 h-12 border-4 border-blue-600 border-t-transparent rounded-full animate-spin"
						></div>
						<p class="mt-4 text-gray-600">جاري تحميل التفاصيل...</p>
					</div>

					<!-- Service details -->
					<div v-else class="space-y-6 text-right">
						<!-- Service ID -->
						<div
							class="grid grid-cols-1 md:grid-cols-3 gap-3 border-b border-gray-200 pb-4"
						>
							<div class="font-semibold text-gray-700">رقم الخدمة:</div>
							<div class="md:col-span-2 font-medium">
								{{ selectedService.id }}
							</div>
						</div>

						<!-- Title -->
						<div
							class="grid grid-cols-1 md:grid-cols-3 gap-3 border-b border-gray-200 pb-4"
						>
							<div class="font-semibold text-gray-700">العنوان:</div>
							<div class="md:col-span-2 font-medium">
								{{ selectedService.title }}
							</div>
						</div>

						<!-- Description -->
						<div
							class="grid grid-cols-1 md:grid-cols-3 gap-3 border-b border-gray-200 pb-4"
						>
							<div class="font-semibold text-gray-700">الوصف:</div>
							<div class="md:col-span-2">{{ selectedService.description }}</div>
						</div>

						<!-- Provider Username -->
						<div
							class="grid grid-cols-1 md:grid-cols-3 gap-3 border-b border-gray-200 pb-4"
						>
							<div class="font-semibold text-gray-700">مقدم الخدمة:</div>
							<div class="md:col-span-2">
								{{ selectedService.providerUsername }}
							</div>
						</div>

						<!-- Provider Full Name -->
						<div
							class="grid grid-cols-1 md:grid-cols-3 gap-3 border-b border-gray-200 pb-4"
						>
							<div class="font-semibold text-gray-700">اسم مقدم الخدمة:</div>
							<div class="md:col-span-2">
								{{ selectedService.providerFirstName }}
								{{ selectedService.providerLastName }}
							</div>
						</div>

						<!-- Status -->
						<div
							class="grid grid-cols-1 md:grid-cols-3 gap-3 border-b border-gray-200 pb-4"
						>
							<div class="font-semibold text-gray-700">الحالة:</div>
							<div class="md:col-span-2">
								<span
									class="px-3 py-1 inline-flex text-sm leading-5 font-semibold rounded-full"
									:class="{
										'bg-green-100 text-green-800':
											isServiceApproved(selectedService),
										'bg-yellow-100 text-yellow-800':
											!isServiceApproved(selectedService),
									}"
								>
									{{
										isServiceApproved(selectedService)
											? "معتمدة"
											: "بانتظار الموافقة"
									}}
								</span>
							</div>
						</div>

						<!-- Rating -->
						<div
							class="grid grid-cols-1 md:grid-cols-3 gap-3 border-b border-gray-200 pb-4"
						>
							<div class="font-semibold text-gray-700">التقييم:</div>
							<div class="md:col-span-2">
								<div class="flex items-center">
									<div class="flex">
										<template v-for="i in 5" :key="i">
											<i
												class="fas fa-star"
												:class="{
													'text-yellow-400':
														i <= Math.round(selectedService.averageRating || 0),
													'text-gray-300':
														i > Math.round(selectedService.averageRating || 0),
												}"
											></i>
										</template>
									</div>
									<span class="mr-2">
										{{
											selectedService.averageRating
												? selectedService.averageRating.toFixed(1)
												: "0.0"
										}}
										({{ selectedService.ratingCount || 0 }} تقييم)
									</span>
								</div>
							</div>
						</div>

						<!-- Price -->
						<div
							class="grid grid-cols-1 md:grid-cols-3 gap-3 border-b border-gray-200 pb-4"
						>
							<div class="font-semibold text-gray-700">السعر التقديري:</div>
							<div class="md:col-span-2">
								{{
									selectedService.estimatedPrice
										? selectedService.estimatedPrice.toLocaleString()
										: "0"
								}}
								د.ع
							</div>
						</div>

						<!-- Contact Phone -->
						<div
							class="grid grid-cols-1 md:grid-cols-3 gap-3 border-b border-gray-200 pb-4"
						>
							<div class="font-semibold text-gray-700">هاتف الاتصال:</div>
							<div class="md:col-span-2">
								{{ selectedService.contactPhone || "غير متوفر" }}
							</div>
						</div>

						<!-- Created Date -->
						<div
							class="grid grid-cols-1 md:grid-cols-3 gap-3 border-b border-gray-200 pb-4"
						>
							<div class="font-semibold text-gray-700">تاريخ الإنشاء:</div>
							<div class="md:col-span-2">
								{{ formatDate(selectedService.createdDate) }}
							</div>
						</div>

						<!-- Subcategories -->
						<div class="grid grid-cols-1 md:grid-cols-3 gap-3">
							<div class="font-semibold text-gray-700">التصنيفات الفرعية:</div>
							<div class="md:col-span-2">
								<div class="flex flex-wrap gap-2">
									<template
										v-for="(
											subCategory, index
										) in exploreServiceForSubcategories(selectedService)"
										:key="index"
									>
										<span
											class="px-3 py-1 bg-blue-100 text-blue-700 text-sm rounded-full"
										>
											{{ getSubCategoryName(subCategory) }}
										</span>
									</template>
									<span
										v-if="
											exploreServiceForSubcategories(selectedService).length ===
											0
										"
										class="text-gray-500 text-sm"
									>
										لا توجد تصنيفات فرعية
									</span>
								</div>
							</div>
						</div>
					</div>
				</div>

				<div
					class="p-5 border-t border-gray-200 flex justify-start space-x-reverse space-x-2 bg-gray-50"
				>
					<button
						@click="closeDetailsModal"
						class="px-4 py-2 bg-gray-500 text-white rounded-md hover:bg-gray-600 transition-colors"
					>
						إغلاق
					</button>
				</div>
			</div>
		</div>

		<!-- Delete Confirmation Modal -->
		<Modal
			:is-open="serviceToDelete !== null"
			title="تأكيد الحذف"
			size="md"
			@close="serviceToDelete = null"
		>
			<template #header-icon>
				<div class="bg-white/20 p-2 rounded-full">
					<i class="fas fa-trash-alt"></i>
				</div>
			</template>

			<div class="space-y-4">
				<p class="text-gray-700">
					هل أنت متأكد من رغبتك في حذف هذه الخدمة؟ لا يمكن التراجع عن هذا
					الإجراء.
				</p>
				<div
					class="font-bold text-red-600 p-3 bg-red-50 border border-red-100 rounded-lg text-center"
				>
					{{ serviceToDelete?.title }}
				</div>
			</div>

			<template #actions>
				<button
					@click="deleteService"
					class="px-4 py-2 bg-red-600 text-white rounded-md hover:bg-red-700 transition-colors"
				>
					تأكيد الحذف
				</button>
				<button
					@click="serviceToDelete = null"
					class="px-4 py-2 bg-gray-500 text-white rounded-md hover:bg-gray-600 transition-colors"
				>
					إلغاء
				</button>
			</template>
		</Modal>
	</div>
</template>

<script>
	import { ref, computed, onMounted } from "vue";
	import { useToastStore } from "@/store/toastStore";
	import ServiceForm from "@/components/ServiceForm.vue";
	import CategoryService from "@/services/CategoryService";
	import ServiceService from "@/services/ServiceService";
	import Modal from "@/components/Modal.vue";

	export default {
		name: "ServicesManagement",
		components: {
			ServiceForm,
			Modal,
		},
		props: {
			userToken: {
				type: String,
				required: true,
			},
		},
		setup(props) {
			const toastStore = useToastStore();
			const loading = ref(true);
			const services = ref([]);
			const selectedService = ref(null);
			const statusFilter = ref("all");
			const searchQuery = ref("");
			const showServiceForm = ref(false);
			const categories = ref([]);
			const serviceToDelete = ref(null);

			const initialServiceData = {
				title: "",
				description: "",
				estimatedPrice: 0,
				contactPhone: "",
				subCategoryIds: [],
			};

			// Computed property to filter services by status and search query
			const filteredServices = computed(() => {
				let result = [...services.value];
				if (statusFilter.value !== "all") {
					result = result.filter((service) =>
						statusFilter.value === "approved"
							? service.isApproved
							: !service.isApproved
					);
				}
				if (searchQuery.value.trim()) {
					const query = searchQuery.value.toLowerCase();
					result = result.filter(
						(service) =>
							(service.title && service.title.toLowerCase().includes(query)) ||
							(service.description &&
								service.description.toLowerCase().includes(query)) ||
							(service.providerUsername &&
								service.providerUsername.toLowerCase().includes(query))
					);
				}
				return result;
			});

			onMounted(async () => {
				await loadDashboardData();
			});

			const loadDashboardData = async () => {
				loading.value = true;
				try {
					await loadServices();
					await loadCategories();
				} catch (error) {
					toastStore.error("فشل تحميل البيانات. يرجى المحاولة مرة أخرى.");
				} finally {
					loading.value = false;
				}
			};

			const loadServices = async () => {
				try {
					const data = await ServiceService.getAdminServices(props.userToken);
					if (Array.isArray(data)) {
						services.value = data;
					} else if (data && typeof data === "object") {
						const possibleArrayProps = [
							"services",
							"data",
							"items",
							"results",
							"$values",
						];
						for (const prop of possibleArrayProps) {
							if (Array.isArray(data[prop])) {
								services.value = data[prop];
								break;
							}
						}
						if (!Array.isArray(services.value)) {
							services.value = [];
						}
					} else {
						services.value = [];
					}
				} catch (error) {
					services.value = [];
				}
			};

			const loadCategories = async () => {
				try {
					console.log(
						"Loading categories and subcategories for services management..."
					);

					// First, load the categories
					const categoriesResponse = await CategoryService.getCategories(
						props.userToken
					);
					console.log(
						`Loaded ${categoriesResponse.length} categories:`,
						categoriesResponse
					);

					// Then load ALL subcategories directly
					const allSubCategories = await CategoryService.getSubCategories(
						props.userToken
					);
					console.log(
						`Loaded ${allSubCategories.length} subcategories:`,
						allSubCategories
					);

					// Log specific details about the IDs to help debugging
					if (allSubCategories.length > 0) {
						console.log("SubCategory categoryIds (first 5 shown):");
						allSubCategories.slice(0, 5).forEach((sub) => {
							console.log(
								`SubCategory ${sub.id} (${sub.name}): categoryId = ${
									sub.categoryId
								}, type: ${typeof sub.categoryId}`
							);
						});
					}

					if (categoriesResponse.length > 0) {
						console.log("Category IDs (first 5 shown):");
						categoriesResponse.slice(0, 5).forEach((cat) => {
							console.log(
								`Category ${cat.id} (${cat.name}): id type: ${typeof cat.id}`
							);
						});
					}

					// Create a robust mapping structure that will track which subcategories matched
					const mappedSubcategories = new Set();

					// Map subcategories to their respective categories with multiple matching strategies
					const categoriesWithSubs = categoriesResponse.map((category) => {
						// Try multiple ways to match subcategories to categories
						const categorySubcategories = allSubCategories.filter(
							(subcategory) => {
								// Skip already mapped subcategories to prevent duplicates
								if (mappedSubcategories.has(subcategory.id)) {
									return false;
								}

								let matches = false;

								// Strategy 1: Direct string comparison of IDs
								if (String(subcategory.categoryId) === String(category.id)) {
									matches = true;
								}
								// Strategy 2: Number comparison of IDs
								else if (
									Number(subcategory.categoryId) === Number(category.id)
								) {
									matches = true;
								}
								// Strategy 3: Check for different property name casing
								else if (
									subcategory.CategoryId &&
									String(subcategory.CategoryId) === String(category.id)
								) {
									matches = true;
								}
								// Strategy 4: Check for name-based matching if IDs don't match
								else if (
									subcategory.categoryName &&
									category.name &&
									subcategory.categoryName.toLowerCase() ===
										category.name.toLowerCase()
								) {
									matches = true;
								}

								// If we got a match, add to the tracked set
								if (matches) {
									mappedSubcategories.add(subcategory.id);
								}

								return matches;
							}
						);

						console.log(
							`Category ${category.name} (${category.id}): found ${categorySubcategories.length} subcategories`
						);

						// Return category with its matched subcategories
						return {
							...category,
							subCategories: categorySubcategories.map((sub) => ({
								id: sub.id,
								name: sub.name || sub.Name,
								categoryId: category.id, // Explicitly set the categoryId to ensure consistency
							})),
						};
					});

					// Check if we have subcategories for each category
					let totalSubcategories = categoriesWithSubs.reduce(
						(count, category) => count + (category.subCategories?.length || 0),
						0
					);

					console.log(
						`Categories with subcategories: ${categoriesWithSubs.length}, total mapped subcategories: ${totalSubcategories}`
					);

					// If no subcategories were matched but we have subcategories, use fallback distribution
					if (totalSubcategories === 0 && allSubCategories.length > 0) {
						console.log(
							"WARNING: No subcategories were matched to categories using IDs!"
						);
						console.log("Creating distribution based on available data...");

						// Mark unmapped subcategories
						const unmappedSubcategories = allSubCategories.filter(
							(sub) => !mappedSubcategories.has(sub.id)
						);
						console.log(
							`Found ${unmappedSubcategories.length} unmapped subcategories to distribute`
						);

						// Distribute unmapped subcategories evenly across categories
						const categoriesWithAssignedSubs = categoriesWithSubs.map(
							(category, index) => {
								// Calculate how many subcategories to assign to each category
								const subcategoriesPerCategory = Math.ceil(
									unmappedSubcategories.length / categoriesWithSubs.length
								);

								// Determine the slice of subcategories for this category
								const startIndex = index * subcategoriesPerCategory;
								const endIndex = Math.min(
									startIndex + subcategoriesPerCategory,
									unmappedSubcategories.length
								);
								const assignedSubs = unmappedSubcategories.slice(
									startIndex,
									endIndex
								);

								console.log(
									`Assigning ${assignedSubs.length} subcategories to category ${category.name}`
								);

								return {
									...category,
									subCategories: [
										...(category.subCategories || []),
										...assignedSubs.map((sub) => ({
											id: sub.id,
											name: sub.name || sub.Name || "تصنيف فرعي",
											categoryId: category.id, // Force the categoryId to match
										})),
									],
								};
							}
						);

						categories.value = categoriesWithAssignedSubs;

						// Log final count after distribution
						const finalCount = categoriesWithAssignedSubs.reduce(
							(count, category) =>
								count + (category.subCategories?.length || 0),
							0
						);
						console.log(
							`Final distribution: ${categoriesWithAssignedSubs.length} categories, ${finalCount} total subcategories`
						);
						return;
					}

					// If everything worked normally, assign the result
					categories.value = categoriesWithSubs;
				} catch (error) {
					console.error("Error loading categories:", error);
					toastStore.error(
						"حدث خطأ أثناء تحميل التصنيفات. يرجى المحاولة مرة أخرى."
					);
					categories.value = [];
				}
			};

			// Format date for display
			const formatDate = (dateString) => {
				if (!dateString) return "N/A";
				try {
					return new Date(dateString).toLocaleDateString("ar-IQ");
				} catch (e) {
					return dateString;
				}
			};

			// Service details modal logic
			const viewDetails = async (service) => {
				loading.value = true;
				try {
					// Fetch complete service details
					console.log("Fetching service details for ID:", service.id);
					const fullServiceDetails = await ServiceService.getServiceById(
						service.id,
						props.userToken
					);
					console.log("API response for service details:", fullServiceDetails);

					// Log specifically about subcategories
					console.log(
						"Subcategories data from API:",
						fullServiceDetails.subCategories ||
							fullServiceDetails.SubCategories ||
							fullServiceDetails.serviceSubCategories ||
							fullServiceDetails.serviceSubCategory ||
							"No subcategories found in response"
					);

					selectedService.value = fullServiceDetails;
				} catch (error) {
					console.error("Error fetching service details:", error);
					toastStore.error("فشل تحميل تفاصيل الخدمة. يرجى المحاولة مرة أخرى.");
					// Fallback to the basic service data we already have
					selectedService.value = JSON.parse(JSON.stringify(service));
				} finally {
					loading.value = false;
				}
			};
			const closeDetailsModal = () => {
				selectedService.value = null;
			};

			// Approve/revoke logic
			const approveService = async (serviceId) => {
				try {
					const success = await ServiceService.approveService(
						serviceId,
						props.userToken
					);
					if (!success) throw new Error();
					const idx = services.value.findIndex((s) => s.id === serviceId);
					if (idx !== -1) services.value[idx].isApproved = true;
					toastStore.success("تمت الموافقة على الخدمة بنجاح!");
				} catch {
					toastStore.error("فشل الموافقة على الخدمة. يرجى المحاولة مرة أخرى.");
				}
			};
			const revokeApproval = async (serviceId) => {
				try {
					const success = await ServiceService.revokeApproval(
						serviceId,
						props.userToken
					);
					if (!success) throw new Error();
					const idx = services.value.findIndex((s) => s.id === serviceId);
					if (idx !== -1) services.value[idx].isApproved = false;
					toastStore.success("تم إلغاء الموافقة على الخدمة بنجاح!");
				} catch {
					toastStore.error("فشل إلغاء الموافقة. يرجى المحاولة مرة أخرى.");
				}
			};

			// Open service form modal
			const openAddServiceModal = async () => {
				// Show loading state
				loading.value = true;
				toastStore.info("جاري تحميل بيانات نموذج إضافة الخدمة...");

				try {
					// Force reload categories every time to ensure fresh data
					console.log(
						"Loading fresh category and subcategory data for service form"
					);
					await loadCategories();

					// Validate subcategory data
					const totalSubcategories = categories.value.reduce(
						(count, category) => count + (category.subCategories?.length || 0),
						0
					);

					// Log what we found
					console.log(
						`Form ready with ${categories.value.length} categories and ${totalSubcategories} subcategories`
					);

					// If we don't have any subcategories, show warning
					if (totalSubcategories === 0) {
						toastStore.warning(
							"لم يتم العثور على تصنيفات فرعية. قد لا تعمل وظيفة إضافة الخدمة بشكل صحيح."
						);
					} else {
						// Show success message with count
						toastStore.success(
							`تم تحميل ${categories.value.length} تصنيف و ${totalSubcategories} تصنيف فرعي`
						);
					}

					// Log detailed category info for debugging
					categories.value.forEach((category) => {
						const subcatCount = category.subCategories?.length || 0;
						console.log(
							`Category ${category.name} (${category.id}): ${subcatCount} subcategories`
						);

						// List first few subcategories for each category
						if (subcatCount > 0) {
							const samplesToShow = Math.min(3, subcatCount);
							console.log(
								`Sample subcategories for ${category.name}:`,
								category.subCategories
									.slice(0, samplesToShow)
									.map((s) => `${s.name} (${s.id})`)
							);
						}
					});
				} catch (error) {
					console.error("Error preparing service form data:", error);
					toastStore.error("فشل في تحميل التصنيفات. يرجى المحاولة مرة أخرى.");
				} finally {
					// Open the modal
					showServiceForm.value = true;
					loading.value = false;
				}
			};

			// Close service form modal
			const closeServiceForm = () => {
				showServiceForm.value = false;
			};

			// Handle service creation
			const handleServiceSubmit = async (formData) => {
				try {
					const createdService = await ServiceService.createService(
						formData,
						props.userToken
					);
					showServiceForm.value = false;
					toastStore.success("تم إنشاء الخدمة بنجاح!");
					services.value.push(createdService);
				} catch (error) {
					toastStore.error("فشل إنشاء الخدمة. يرجى المحاولة مرة أخرى.");
				}
			};

			// Subcategories helper
			const getSubCategories = (service) => {
				if (!service) return [];

				// For debugging - log the service object to see its structure
				console.log("Service object for subcategories:", service);

				// Case 1: Direct subCategories array (lowercase)
				if (
					service.subCategories &&
					Array.isArray(service.subCategories) &&
					service.subCategories.length > 0
				) {
					console.log(
						"Found subcategories in service.subCategories:",
						service.subCategories
					);
					return service.subCategories;
				}

				// Case 2: SubCategories array (capitalized first letter)
				if (
					service.SubCategories &&
					Array.isArray(service.SubCategories) &&
					service.SubCategories.length > 0
				) {
					console.log(
						"Found subcategories in service.SubCategories:",
						service.SubCategories
					);
					return service.SubCategories;
				}

				// Case 3: ServiceSubCategory array with subCategory objects
				if (
					service.serviceSubCategory &&
					Array.isArray(service.serviceSubCategory) &&
					service.serviceSubCategory.length > 0
				) {
					console.log(
						"Found serviceSubCategory array:",
						service.serviceSubCategory
					);
					const result = service.serviceSubCategory
						.map((ssc) => ssc.subCategory || ssc)
						.filter(Boolean);
					console.log(
						"Extracted subcategories from serviceSubCategory:",
						result
					);
					return result;
				}

				// Case 4: ServiceSubCategories array with subCategory objects
				if (
					service.serviceSubCategories &&
					Array.isArray(service.serviceSubCategories) &&
					service.serviceSubCategories.length > 0
				) {
					console.log(
						"Found serviceSubCategories array:",
						service.serviceSubCategories
					);
					const result = service.serviceSubCategories
						.map((ssc) => ssc.subCategory || ssc)
						.filter(Boolean);
					console.log(
						"Extracted subcategories from serviceSubCategories:",
						result
					);
					return result;
				}

				// Case 5: Direct array of subcategory objects
				if (Array.isArray(service) && service.length > 0 && service[0].name) {
					console.log(
						"Service is directly an array of subcategories:",
						service
					);
					return service;
				}

				console.log("No subcategories found in any expected format");
				return [];
			};

			// Reload categories
			const handleReloadCategories = async () => {
				loading.value = true;
				try {
					// Show a loading toast
					toastStore.info("جاري تحميل التصنيفات والتصنيفات الفرعية...");

					// Force reload categories and their subcategories
					await loadCategories();

					// Check if we successfully loaded subcategories
					const totalSubcategories = categories.value.reduce(
						(count, category) => count + (category.subCategories?.length || 0),
						0
					);

					if (totalSubcategories > 0) {
						toastStore.success(
							`تم تحميل ${categories.value.length} تصنيف و ${totalSubcategories} تصنيف فرعي بنجاح`
						);

						// Log success information
						console.log(
							`Successfully loaded ${categories.value.length} categories with ${totalSubcategories} subcategories`
						);
						console.log("Categories with subcategory counts:");
						categories.value.forEach((cat) => {
							console.log(
								`- ${cat.name}: ${cat.subCategories?.length || 0} subcategories`
							);
						});
					} else {
						toastStore.warning(
							"لم يتم العثور على تصنيفات فرعية. قد لا تعمل وظيفة إضافة الخدمة بشكل صحيح."
						);
						console.warn("No subcategories were found after reload attempt");
					}
				} catch (error) {
					console.error("Error reloading categories:", error);
					toastStore.error("فشل في تحميل التصنيفات. يرجى المحاولة مرة أخرى.");
				} finally {
					loading.value = false;
				}
			};

			const confirmDeleteService = (service) => {
				serviceToDelete.value = service;
			};
			const deleteService = async () => {
				if (!serviceToDelete.value) return;
				try {
					await ServiceService.deleteService(
						serviceToDelete.value.id,
						props.userToken
					);
					services.value = services.value.filter(
						(s) => s.id !== serviceToDelete.value.id
					);
					toastStore.success("تم حذف الخدمة بنجاح!");
				} catch (error) {
					toastStore.error("فشل حذف الخدمة. يرجى المحاولة مرة أخرى.");
				} finally {
					serviceToDelete.value = null;
				}
			};

			// Service approval status helper
			const isServiceApproved = (service) => {
				if (!service) return false;

				// Handle different property names for approval status
				if (typeof service.isApproved === "boolean") {
					return service.isApproved;
				}

				if (typeof service.IsApproved === "boolean") {
					return service.IsApproved;
				}

				return false;
			};

			// Helper to get subcategory name from various possible formats
			const getSubCategoryName = (subCategory) => {
				if (!subCategory) return "غير معروف";

				// Handle direct string value
				if (typeof subCategory === "string") {
					return subCategory;
				}

				// Handle object with name property
				if (subCategory.name) {
					return subCategory.name;
				}

				// Handle object with Name property (capitalized)
				if (subCategory.Name) {
					return subCategory.Name;
				}

				// Handle object with subCategoryName property
				if (subCategory.subCategoryName) {
					return subCategory.subCategoryName;
				}

				// Handle object with SubCategoryName property
				if (subCategory.SubCategoryName) {
					return subCategory.SubCategoryName;
				}

				// Handle object mapped from ServiceSubCategory relationship
				if (subCategory.subCategory && subCategory.subCategory.name) {
					return subCategory.subCategory.name;
				}

				return "غير معروف";
			};

			// Deeply explore service object to find subcategories
			const exploreServiceForSubcategories = (service) => {
				// Try standard paths first
				const subcategories = getSubCategories(service);
				if (subcategories && subcategories.length > 0) {
					return subcategories;
				}

				// If service has no subcategories but we have the service in the list with subcategories
				if (service && service.id) {
					// Find the same service in our services list, which might have more data
					const matchingService = services.value.find(
						(s) => s.id === service.id
					);
					if (matchingService) {
						const matchingSubcategories = getSubCategories(matchingService);
						if (matchingSubcategories && matchingSubcategories.length > 0) {
							return matchingSubcategories;
						}
					}
				}

				// Look in every property for things that might be subcategories
				if (service && typeof service === "object") {
					for (const key in service) {
						// Skip specific properties we know aren't subcategories
						if (
							[
								"id",
								"title",
								"description",
								"createdDate",
								"estimatedPrice",
							].includes(key)
						) {
							continue;
						}

						const value = service[key];

						// Check if property is an array that might contain subcategories
						if (Array.isArray(value)) {
							// Look for objects that might be subcategories (having name or categoryId)
							const possibleSubcategories = value.filter(
								(item) =>
									item &&
									typeof item === "object" &&
									(item.name || item.Name || item.categoryId || item.CategoryId)
							);

							if (possibleSubcategories.length > 0) {
								console.log(
									`Found possible subcategories in property ${key}:`,
									possibleSubcategories
								);
								return possibleSubcategories;
							}
						}

						// Check for nested objects that might contain subcategory arrays
						if (value && typeof value === "object" && !Array.isArray(value)) {
							for (const nestedKey in value) {
								const nestedValue = value[nestedKey];
								if (
									Array.isArray(nestedValue) &&
									nestedValue.some(
										(item) =>
											item &&
											typeof item === "object" &&
											(item.name ||
												item.Name ||
												item.categoryId ||
												item.CategoryId)
									)
								) {
									console.log(
										`Found possible subcategories in nested property ${key}.${nestedKey}:`,
										nestedValue
									);
									return nestedValue;
								}
							}
						}
					}
				}

				return [];
			};

			return {
				loading,
				services,
				statusFilter,
				searchQuery,
				filteredServices,
				showServiceForm,
				categories,
				initialServiceData,
				handleServiceSubmit,
				formatDate,
				viewDetails,
				approveService,
				revokeApproval,
				handleReloadCategories,
				selectedService,
				closeDetailsModal,
				getSubCategories,
				serviceToDelete,
				confirmDeleteService,
				deleteService,
				openAddServiceModal,
				closeServiceForm,
				isServiceApproved,
				getSubCategoryName,
				exploreServiceForSubcategories,
			};
		},
	};
</script>

<style scoped>
	@keyframes modal-appear {
		from {
			opacity: 0;
			transform: scale(0.98) translateY(-10px);
		}
		to {
			opacity: 1;
			transform: scale(1) translateY(0);
		}
	}

	@keyframes fadeIn {
		from {
			opacity: 0;
		}
		to {
			opacity: 1;
		}
	}

	.animate-fadeIn {
		animation: fadeIn 0.3s ease-in-out;
	}

	/* Custom scrollbar for modal bodies */
	.modal-body::-webkit-scrollbar {
		width: 8px;
	}

	.modal-body::-webkit-scrollbar-track {
		background: #f1f5f9;
		border-radius: 8px;
	}

	.modal-body::-webkit-scrollbar-thumb {
		background-color: #cbd5e1;
		border-radius: 8px;
		border: 2px solid #f1f5f9;
	}

	.modal-body::-webkit-scrollbar-thumb:hover {
		background-color: #94a3b8;
	}

	@media (max-width: 640px) {
		.modal-container {
			margin-top: 0;
			border-radius: 0;
			height: 100vh;
			max-height: 100vh;
		}
	}
</style>
