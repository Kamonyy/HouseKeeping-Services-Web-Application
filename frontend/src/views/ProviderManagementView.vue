<template>
	<div class="container mx-auto px-4 py-8">
		<!-- Loading indicator -->
		<div v-if="loading" class="text-center py-8">
			<div
				class="inline-block w-12 h-12 border-4 border-blue-600 border-t-transparent rounded-full animate-spin"
			></div>
			<p class="mt-4 text-gray-600">جاري تحميل البيانات...</p>
		</div>

		<!-- Main content -->
		<div v-else>
			<div class="flex justify-between items-center mb-6">
				<h1
					class="text-center text-3xl font-bold text-transparent bg-clip-text bg-gradient-to-r from-blue-700 to-blue-500"
				>
					إدارة الخدمات
				</h1>
				<button
					@click="showServiceForm ? hideForm() : showForm()"
					class="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 flex items-center gap-2 transition-all"
				>
					<span v-if="!showServiceForm">إضافة خدمة جديدة</span>
					<span v-else>إغلاق النموذج</span>
					<svg
						xmlns="http://www.w3.org/2000/svg"
						fill="none"
						viewBox="0 0 24 24"
						stroke-width="1.5"
						stroke="currentColor"
						class="w-5 h-5"
						v-if="!showServiceForm"
					>
						<path
							stroke-linecap="round"
							stroke-linejoin="round"
							d="M12 4.5v15m7.5-7.5h-15"
						/>
					</svg>
					<svg
						xmlns="http://www.w3.org/2000/svg"
						fill="none"
						viewBox="0 0 24 24"
						stroke-width="1.5"
						stroke="currentColor"
						class="w-5 h-5"
						v-else
					>
						<path
							stroke-linecap="round"
							stroke-linejoin="round"
							d="M6 18L18 6M6 6l12 12"
						/>
					</svg>
				</button>
			</div>

			<!-- Services List - Now displayed first -->
			<div class="bg-white rounded-lg shadow-md overflow-hidden mb-8">
				<div class="bg-blue-600 text-white p-4">
					<h4 class="font-bold text-xl">الخدمات المقدمة</h4>
				</div>
				<div class="p-4">
					<div v-if="serverError" class="text-center py-8">
						<div
							class="bg-red-50 border border-red-200 rounded-md p-4 max-w-md mx-auto"
						>
							<div class="flex flex-col items-center">
								<div class="flex-shrink-0 mb-3">
									<svg
										class="h-10 w-10 text-red-400"
										xmlns="http://www.w3.org/2000/svg"
										viewBox="0 0 20 20"
										fill="currentColor"
									>
										<path
											fill-rule="evenodd"
											d="M10 18a8 8 0 100-16 8 8 0 000 16zM8.707 7.293a1 1 0 00-1.414 1.414L8.586 10l-1.293 1.293a1 1 0 101.414 1.414L10 11.414l1.293 1.293a1 1 0 001.414-1.414L11.414 10l1.293-1.293a1 1 0 00-1.414-1.414L10 8.586 8.707 7.293z"
											clip-rule="evenodd"
										/>
									</svg>
								</div>
								<h3 class="text-lg font-medium text-red-800 mb-2">
									خطأ في خادم البيانات
								</h3>
								<div class="text-sm text-red-700 mb-4 text-center">
									<p>
										لا يمكن تحميل الخدمات الخاصة بك حالياً بسبب مشكلة في الخادم.
										هذه المشكلة مؤقتة، يرجى المحاولة مرة أخرى لاحقاً.
									</p>
								</div>
								<button
									type="button"
									@click="loadProviderServices"
									class="inline-flex items-center px-4 py-2 border border-transparent text-sm font-medium rounded-md text-white bg-red-600 hover:bg-red-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-red-500"
								>
									إعادة المحاولة
								</button>
							</div>
						</div>
					</div>

					<div
						v-else-if="providerServices.length === 0"
						class="text-center py-8"
					>
						<p class="text-gray-500 text-lg">
							لم تقم بإضافة أي خدمات حتى الآن.
						</p>
						<button
							@click="showServiceForm ? hideForm() : showForm()"
							class="mt-4 px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-all"
						>
							إضافة خدمة الآن
						</button>
					</div>

					<div v-else class="overflow-x-auto">
						<table class="min-w-full divide-y divide-gray-200 rtl">
							<thead class="bg-gray-50">
								<tr>
									<th
										class="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider"
									>
										عنوان الخدمة
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
								<tr
									v-for="service in providerServices"
									:key="service.id"
									class="hover:bg-gray-50"
								>
									<td class="px-6 py-4 whitespace-nowrap">
										{{ service.title }}
									</td>
									<td class="px-6 py-4 whitespace-nowrap">
										{{ service.estimatedPrice.toLocaleString() }} د.ع
									</td>
									<td class="px-6 py-4 whitespace-nowrap">
										<span
											class="px-2 inline-flex text-xs leading-5 font-semibold rounded-full"
											:class="{
												'bg-green-100 text-green-800': service.isApproved,
												'bg-yellow-100 text-yellow-800': !service.isApproved,
											}"
										>
											{{ service.isApproved ? "معتمدة" : "بانتظار الموافقة" }}
										</span>
									</td>
									<td class="px-6 py-4 whitespace-nowrap">
										{{
											new Date(service.createdDate).toLocaleDateString("ar-IQ")
										}}
									</td>
									<td class="px-6 py-4 whitespace-nowrap">
										<div class="flex gap-2">
											<button
												class="px-3 py-1 bg-blue-600 text-white text-sm font-medium rounded hover:bg-blue-700"
												@click="editService(service)"
												:disabled="!service.isApproved"
												:class="{
													'opacity-50 cursor-not-allowed': !service.isApproved,
												}"
												:title="
													!service.isApproved
														? 'لا يمكن التعديل حتى تتم الموافقة'
														: 'تعديل الخدمة'
												"
											>
												تعديل
											</button>
											<button
												class="px-3 py-1 bg-red-600 text-white text-sm font-medium rounded hover:bg-red-700"
												@click="confirmDelete(service.id)"
												title="حذف الخدمة"
											>
												حذف
											</button>
										</div>
									</td>
								</tr>
							</tbody>
						</table>
					</div>
				</div>
			</div>

			<!-- Service Form - Now collapsible and appears after the list -->
			<div
				class="bg-white rounded-lg shadow-md mb-6 overflow-hidden"
				v-if="showServiceForm || isEditing"
			>
				<div class="bg-blue-600 text-white p-4">
					<h4 class="font-bold text-xl">
						{{ isEditing ? "تعديل الخدمة" : "إضافة خدمة جديدة" }}
					</h4>
				</div>
				<div class="p-4">
					<form @submit.prevent="handleSubmit" class="rtl">
						<div class="mb-4">
							<label for="title" class="block text-gray-700 mb-2 font-bold">
								عنوان الخدمة <span class="text-red-500">*</span>
							</label>
							<input
								type="text"
								class="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
								id="title"
								v-model="serviceForm.title"
								placeholder="أدخل عنوان الخدمة"
								required
							/>
						</div>

						<div class="mb-4">
							<label
								for="description"
								class="block text-gray-700 mb-2 font-bold"
							>
								وصف الخدمة <span class="text-red-500">*</span>
							</label>
							<textarea
								class="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
								id="description"
								v-model="serviceForm.description"
								rows="3"
								placeholder="أدخل وصفاً تفصيلياً للخدمة"
								required
							></textarea>
						</div>

						<div class="grid grid-cols-1 md:grid-cols-2 gap-4">
							<div class="mb-4">
								<label for="price" class="block text-gray-700 mb-2 font-bold">
									السعر التقريبي (د.ع) <span class="text-red-500">*</span>
								</label>
								<input
									type="number"
									class="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
									id="price"
									v-model="serviceForm.estimatedPrice"
									min="0"
									step="1000"
									placeholder="أدخل السعر التقريبي"
									required
								/>
							</div>

							<div class="mb-4">
								<label for="phone" class="block text-gray-700 mb-2 font-bold">
									رقم الهاتف للتواصل <span class="text-red-500">*</span>
								</label>
								<input
									type="tel"
									class="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
									id="phone"
									v-model="serviceForm.contactPhone"
									placeholder="07xxxxxxxxx"
									required
								/>
							</div>
						</div>

						<div class="mb-6">
							<label class="block text-gray-700 mb-2 font-bold">
								التصنيفات الفرعية <span class="text-red-500">*</span>
								<span class="text-sm font-normal text-gray-500 mr-2"
									>(يجب اختيار تصنيف واحد على الأقل)</span
								>
							</label>
							<div
								v-if="categories.length === 0"
								class="text-gray-500 p-4 bg-gray-50 rounded-lg"
							>
								جاري تحميل التصنيفات...
							</div>
							<div v-else>
								<!-- Search box for categories -->
								<div class="mb-3">
									<div class="relative">
										<input
											type="text"
											v-model="categorySearch"
											placeholder="ابحث عن تصنيف..."
											class="w-full px-3 py-2 pr-10 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
										/>
										<div
											class="absolute inset-y-0 right-0 flex items-center pr-3 pointer-events-none"
										>
											<svg
												xmlns="http://www.w3.org/2000/svg"
												class="h-5 w-5 text-gray-400"
												fill="none"
												viewBox="0 0 24 24"
												stroke="currentColor"
											>
												<path
													stroke-linecap="round"
													stroke-linejoin="round"
													stroke-width="2"
													d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z"
												/>
											</svg>
										</div>
									</div>
								</div>

								<!-- Selected categories summary -->
								<div
									v-if="serviceForm.subCategoryIds.length > 0"
									class="mb-4 p-3 bg-blue-50 border border-blue-200 rounded-md"
								>
									<div class="flex items-center justify-between mb-2">
										<span class="font-medium text-blue-700"
											>التصنيفات المختارة ({{
												serviceForm.subCategoryIds.length
											}})</span
										>
										<button
											type="button"
											@click="serviceForm.subCategoryIds = []"
											class="text-xs text-red-600 hover:text-red-800"
										>
											إلغاء الكل
										</button>
									</div>
									<div class="flex flex-wrap gap-2">
										<span
											v-for="subCatId in serviceForm.subCategoryIds"
											:key="subCatId"
											class="inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium bg-blue-100 text-blue-800"
										>
											{{ getSubCategoryName(subCatId) }}
											<button
												@click="removeSubCategory(subCatId)"
												class="mr-1 text-blue-500 hover:text-blue-700"
											>
												<svg
													xmlns="http://www.w3.org/2000/svg"
													class="h-3 w-3"
													viewBox="0 0 20 20"
													fill="currentColor"
												>
													<path
														fill-rule="evenodd"
														d="M4.293 4.293a1 1 0 011.414 0L10 8.586l4.293-4.293a1 1 0 111.414 1.414L11.414 10l4.293 4.293a1 1 0 01-1.414 1.414L10 11.414l-4.293 4.293a1 1 0 01-1.414-1.414L8.586 10 4.293 5.707a1 1 0 010-1.414z"
														clip-rule="evenodd"
													/>
												</svg>
											</button>
										</span>
									</div>
								</div>

								<!-- Check if any categories have subcategories -->
								<div
									v-if="totalSubcategoriesCount === 0"
									class="p-4 bg-yellow-50 border border-yellow-200 rounded-lg mb-4 text-center"
								>
									<div class="flex flex-col items-center">
										<svg
											xmlns="http://www.w3.org/2000/svg"
											class="h-10 w-10 text-yellow-400 mb-3"
											fill="none"
											viewBox="0 0 24 24"
											stroke="currentColor"
										>
											<path
												stroke-linecap="round"
												stroke-linejoin="round"
												stroke-width="2"
												d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-3L13.732 4c-.77-1.333-2.694-1.333-3.464 0L3.34 16c-.77 1.333.192 3 1.732 3z"
											/>
										</svg>
										<p class="text-yellow-700 font-medium">
											لم يتم العثور على أي تصنيفات فرعية
										</p>
										<p class="text-yellow-600 text-sm mt-1">
											جاري محاولة تحميل التصنيفات الفرعية...
										</p>
										<button
											type="button"
											@click="reloadCategories"
											class="mt-3 px-4 py-2 bg-yellow-600 text-white rounded hover:bg-yellow-700 transition-colors"
										>
											إعادة تحميل التصنيفات
										</button>
									</div>
								</div>

								<!-- Categories accordion -->
								<div v-else class="border border-gray-200 rounded-lg divide-y">
									<div
										v-for="category in filteredCategories"
										:key="category.id"
										class="bg-white"
									>
										<div
											@click="toggleCategory(category.id)"
											class="flex justify-between items-center p-4 cursor-pointer hover:bg-gray-50"
										>
											<div class="flex items-center">
												<svg
													xmlns="http://www.w3.org/2000/svg"
													class="h-5 w-5 transform transition-transform"
													:class="
														openCategories.includes(category.id)
															? 'rotate-90'
															: ''
													"
													fill="none"
													viewBox="0 0 24 24"
													stroke="currentColor"
												>
													<path
														stroke-linecap="round"
														stroke-linejoin="round"
														stroke-width="2"
														d="M9 5l7 7-7 7"
													/>
												</svg>
												<h6 class="font-bold text-gray-700 mr-2">
													{{ category.name }}
												</h6>
											</div>
											<div class="flex items-center gap-3">
												<span class="text-sm text-gray-500">
													{{ selectedSubCategoriesCount(category) }} /
													{{
														category.subCategories
															? category.subCategories.length
															: 0
													}}
												</span>
												<button
													type="button"
													@click.stop="toggleSelectAll(category)"
													class="text-xs px-2 py-1 rounded text-blue-600 hover:bg-blue-50"
													:disabled="
														!category.subCategories ||
														category.subCategories.length === 0
													"
												>
													{{
														isAllSelected(category)
															? "إلغاء تحديد الكل"
															: "تحديد الكل"
													}}
												</button>
											</div>
										</div>

										<div
											v-if="openCategories.includes(category.id)"
											class="p-4 pt-0 bg-gray-50 border-t border-gray-200"
										>
											<!-- Display message if no subcategories -->
											<div
												v-if="
													!category.subCategories ||
													category.subCategories.length === 0
												"
												class="p-3 text-center text-gray-500"
											>
												لا توجد تصنيفات فرعية متاحة لهذا التصنيف
											</div>

											<div
												v-else
												class="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-3 mt-3"
											>
												<div
													v-for="subCategory in filteredSubCategories(category)"
													:key="subCategory.id"
													class="flex items-center p-2 rounded-md hover:bg-gray-100"
												>
													<input
														class="w-4 h-4 ml-2 text-blue-600 focus:ring-blue-500"
														type="checkbox"
														:id="`subcat-${subCategory.id}`"
														:value="subCategory.id"
														v-model="serviceForm.subCategoryIds"
													/>
													<label
														class="text-gray-700 cursor-pointer flex-grow"
														:for="`subcat-${subCategory.id}`"
													>
														{{ subCategory.name }}
													</label>
												</div>
											</div>
										</div>
									</div>
								</div>
							</div>
						</div>

						<div class="flex gap-2">
							<button
								type="submit"
								class="px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-2"
							>
								{{ isEditing ? "تحديث الخدمة" : "إضافة الخدمة" }}
							</button>
							<button
								type="button"
								class="px-4 py-2 bg-gray-500 text-white rounded-md hover:bg-gray-600 focus:outline-none focus:ring-2 focus:ring-gray-500 focus:ring-offset-2"
								@click="cancelForm"
							>
								إلغاء
							</button>
						</div>
					</form>
				</div>
			</div>

			<!-- Delete Confirmation Modal -->
			<Modal
				:is-open="!!serviceToDeleteId"
				title="تأكيد الحذف"
				@close="serviceToDeleteId = null"
			>
				<p class="text-gray-700">
					هل أنت متأكد من رغبتك في حذف هذه الخدمة؟ لا يمكن التراجع عن هذا
					الإجراء.
				</p>
				<template #actions>
					<button
						class="px-4 py-2 bg-red-600 hover:bg-red-700 text-white rounded transition-colors focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-red-500"
						@click="deleteService"
					>
						حذف
					</button>
				</template>
			</Modal>
		</div>
	</div>
</template>

<script>
	import { useUserStore } from "@/store/userStore";
	import { ref, computed, onMounted, watch, nextTick } from "vue";
	import { useRouter } from "vue-router";
	import Modal from "@/components/Modal.vue";
	import axios from "axios";
	import { extractArrayFromResponse } from "@/utils/apiUtils";

	export default {
		name: "ProviderManagementView",
		components: {
			Modal,
		},
		setup() {
			const loading = ref(true);
			const categories = ref([]);
			const providerServices = ref([]);
			const serviceForm = ref({
				title: "",
				description: "",
				estimatedPrice: 0,
				contactPhone: "",
				subCategoryIds: [],
			});
			const isEditing = ref(false);
			const currentServiceId = ref(null);
			const serviceToDeleteId = ref(null);
			const showServiceForm = ref(false);
			const userStore = useUserStore();
			const router = useRouter();
			const serverError = ref(false);

			// New refs for enhanced category selection
			const categorySearch = ref("");
			const openCategories = ref([]);

			const token = computed(() => userStore.token);
			const isProvider = computed(() => userStore.isProvider);

			// Calculate total subcategories across all categories
			const totalSubcategoriesCount = computed(() => {
				return categories.value.reduce(
					(total, cat) =>
						total +
						(cat.subCategories && Array.isArray(cat.subCategories)
							? cat.subCategories.length
							: 0),
					0
				);
			});

			// Filter categories based on search term
			const filteredCategories = computed(() => {
				if (!categories.value || categories.value.length === 0) return [];
				if (!categorySearch.value.trim()) return categories.value;

				const searchTerm = categorySearch.value.toLowerCase();
				return categories.value.filter((category) => {
					if (!category) return false;

					// Check if category name matches
					if (category.name && category.name.toLowerCase().includes(searchTerm))
						return true;

					// Check if any subcategory matches
					return (
						category.subCategories &&
						category.subCategories.some(
							(subCat) =>
								subCat &&
								subCat.name &&
								subCat.name.toLowerCase().includes(searchTerm)
						)
					);
				});
			});

			// Toggle category expansion
			const toggleCategory = (categoryId) => {
				const index = openCategories.value.indexOf(categoryId);
				if (index === -1) {
					openCategories.value.push(categoryId);
				} else {
					openCategories.value.splice(index, 1);
				}
			};

			// Get subcategory name by ID
			const getSubCategoryName = (subCategoryId) => {
				for (const category of categories.value) {
					for (const subCategory of category.subCategories) {
						if (subCategory.id === subCategoryId) {
							return subCategory.name;
						}
					}
				}
				return "التصنيف الفرعي";
			};

			// Count selected subcategories in a category
			const selectedSubCategoriesCount = (category) => {
				if (!category || !category.subCategories) {
					return 0;
				}
				return category.subCategories.filter(
					(subCat) =>
						subCat &&
						subCat.id &&
						serviceForm.value.subCategoryIds.includes(subCat.id)
				).length;
			};

			// Check if all subcategories in a category are selected
			const isAllSelected = (category) => {
				if (
					!category ||
					!category.subCategories ||
					category.subCategories.length === 0
				) {
					return false;
				}
				return (
					selectedSubCategoriesCount(category) === category.subCategories.length
				);
			};

			// Toggle select/deselect all subcategories in a category
			const toggleSelectAll = (category) => {
				if (!category || !category.subCategories) {
					return;
				}

				if (isAllSelected(category)) {
					// Deselect all subcategories in this category
					serviceForm.value.subCategoryIds =
						serviceForm.value.subCategoryIds.filter(
							(id) => !category.subCategories.some((subCat) => subCat.id === id)
						);
				} else {
					// Select all subcategories in this category
					const subCategoryIds = category.subCategories.map(
						(subCat) => subCat.id
					);
					const uniqueIds = new Set([
						...serviceForm.value.subCategoryIds,
						...subCategoryIds,
					]);
					serviceForm.value.subCategoryIds = Array.from(uniqueIds);
				}
			};

			// Remove a specific subcategory from selection
			const removeSubCategory = (subCategoryId) => {
				const index = serviceForm.value.subCategoryIds.indexOf(subCategoryId);
				if (index !== -1) {
					serviceForm.value.subCategoryIds.splice(index, 1);
				}
			};

			// Filter subcategories based on search term
			const filteredSubCategories = (category) => {
				if (!category || !category.subCategories) {
					return [];
				}

				// Filter out any invalid subcategories first
				const validSubCategories = category.subCategories.filter(
					(subCat) => subCat && subCat.id && subCat.name
				);

				if (!categorySearch.value.trim()) return validSubCategories;

				const searchTerm = categorySearch.value.toLowerCase();
				return validSubCategories.filter((subCat) =>
					subCat.name.toLowerCase().includes(searchTerm)
				);
			};

			// Add a method to reload categories
			const reloadCategories = async () => {
				console.log("Manually reloading categories and subcategories...");
				// Clear existing categories first
				categories.value = [];
				// Then reload
				await loadCategories();
			};

			onMounted(async () => {
				console.log("ProviderManagementView mounted");

				// Check if user is a provider, otherwise redirect
				console.log("Is provider:", isProvider.value);

				// Temporarily disabled provider check for debugging
				// if (!isProvider.value) {
				// 	router.push("/");
				// 	return;
				// }

				// Log token status for debugging
				console.log("Authentication token exists:", !!token.value);
				if (token.value) {
					// Get token expiration time (if possible)
					try {
						const tokenData = JSON.parse(atob(token.value.split(".")[1]));
						console.log(
							"Token expiration:",
							new Date(tokenData.exp * 1000).toLocaleString()
						);
						console.log(
							"Token roles:",
							tokenData[
								"http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
							]
						);
					} catch (e) {
						console.error("Error parsing token:", e);
					}
				}

				// Set loading state
				loading.value = true;

				// Load data sequentially
				await loadCategories();
				await loadProviderServices();
			});

			// Watch for changes to showServiceForm to expand categories when form is shown
			watch(showServiceForm, (newValue) => {
				if (
					newValue &&
					categories.value &&
					categories.value.length > 0 &&
					openCategories.value.length === 0
				) {
					// When form is shown and no categories are expanded yet, expand them all
					openCategories.value = categories.value
						.filter((category) => category && category.id)
						.map((c) => c.id);
				}
			});

			const loadCategories = async () => {
				// Don't set loading to false here since we also need to load services
				try {
					console.log("Starting to load categories...");
					const response = await fetch("/api/category");

					if (!response.ok) {
						console.error(
							"Failed to load categories:",
							response.status,
							response.statusText
						);
						categories.value = [];
						return;
					}

					const data = await response.json();

					// Use our utility function to extract the array
					let processedCategories = extractArrayFromResponse(
						data,
						"categories"
					);

					console.log("Processing categories:", processedCategories.length);

					// Always fetch subcategories separately to ensure we have the latest data
					// Initialize empty subcategories array for each category
					processedCategories = processedCategories.map((cat) => ({
						...cat,
						subCategories: [],
					}));

					// Fetch subcategories for each category
					console.log("Fetching subcategories for each category...");

					for (const category of processedCategories) {
						console.log(
							`Fetching subcategories for category ${category.id} (${category.name})...`
						);
						try {
							// Try first API endpoint pattern
							let subCatUrl = `/api/subcategory/category/${category.id}`;
							console.log("Trying endpoint:", subCatUrl);

							let subCatResponse = await fetch(subCatUrl);

							// If first endpoint fails, try alternative endpoint
							if (!subCatResponse.ok) {
								console.log("First endpoint failed, trying alternative...");
								subCatUrl = `/api/subcategory/bycategory/${category.id}`;
								console.log("Trying alternative endpoint:", subCatUrl);
								subCatResponse = await fetch(subCatUrl);
							}

							// If that also fails, try another alternative
							if (!subCatResponse.ok) {
								console.log(
									"Second endpoint failed, trying another alternative..."
								);
								subCatUrl = `/api/category/${category.id}/subcategories`;
								console.log("Trying another alternative endpoint:", subCatUrl);
								subCatResponse = await fetch(subCatUrl);
							}

							if (subCatResponse.ok) {
								const subCatData = await subCatResponse.json();
								console.log(
									`Subcategories for category ${category.id}:`,
									subCatData
								);

								// Use our utility function to extract the array
								category.subCategories = extractArrayFromResponse(
									subCatData,
									`subcategories_for_category_${category.id}`
								);

								console.log(
									`Loaded ${category.subCategories.length} subcategories for category ${category.id}`
								);
							} else {
								console.error(
									`Failed to load subcategories for category ${category.id}:`,
									subCatResponse.status,
									subCatResponse.statusText
								);
							}
						} catch (error) {
							console.error(
								`Error fetching subcategories for category ${category.id}:`,
								error
							);
						}
					}

					// If we still don't have subcategories, try one more approach - fetch all subcategories and assign them
					let anySubcategoriesLoaded = processedCategories.some(
						(cat) => cat.subCategories && cat.subCategories.length > 0
					);

					if (!anySubcategoriesLoaded) {
						console.log(
							"No subcategories loaded with category-specific endpoints, trying to load all subcategories..."
						);
						try {
							const allSubCatResponse = await fetch("/api/subcategory");
							if (allSubCatResponse.ok) {
								const allSubCatData = await allSubCatResponse.json();

								// Use our utility function to extract the array
								const allSubCategories = extractArrayFromResponse(
									allSubCatData,
									"all_subcategories"
								);

								if (allSubCategories.length > 0) {
									// Group subcategories by category ID
									for (const subCat of allSubCategories) {
										if (subCat && subCat.categoryId) {
											const category = processedCategories.find(
												(c) => c.id === subCat.categoryId
											);
											if (category) {
												if (!category.subCategories) {
													category.subCategories = [];
												}
												category.subCategories.push(subCat);
											}
										}
									}
									console.log("Assigned subcategories from global list");
								}
							}
						} catch (error) {
							console.error("Error fetching all subcategories:", error);
						}
					}

					categories.value = processedCategories;
					console.log("Categories loaded:", categories.value.length);
					console.log(
						"Total subcategories:",
						categories.value.reduce(
							(total, cat) =>
								total + (cat.subCategories ? cat.subCategories.length : 0),
							0
						)
					);

					// Check if any subcategories were loaded
					if (
						categories.value.every(
							(cat) => !cat.subCategories || cat.subCategories.length === 0
						)
					) {
						console.error("No subcategories were loaded for any category!");
						// Add some dummy subcategories for testing if nothing was loaded
						if (categories.value.length > 0) {
							console.log(
								"Adding temporary test subcategories for development"
							);
							categories.value.forEach((cat, index) => {
								cat.subCategories = [
									{
										id: `${cat.id}-sub1`,
										name: `Test SubCategory 1 for ${cat.name}`,
									},
									{
										id: `${cat.id}-sub2`,
										name: `Test SubCategory 2 for ${cat.name}`,
									},
								];
							});
						}
					}

					// When categories are loaded, expand all by default if showing form
					if (showServiceForm.value && openCategories.value.length === 0) {
						openCategories.value = categories.value
							.filter((category) => category && category.id)
							.map((c) => c.id);
					}
				} catch (error) {
					console.error("Error loading categories:", error);
					categories.value = [];
					alert("فشل تحميل التصنيفات. يرجى المحاولة مرة أخرى.");
				}
				// Do not set loading to false here, that should be handled by loadProviderServices
			};

			const loadProviderServices = async () => {
				console.log("Loading provider services...");
				loading.value = true; // Set loading state at the beginning
				try {
					console.log("Making API request with token:", !!token.value);

					// Only proceed if we have a valid token
					if (!token.value) {
						console.error("No authentication token available");
						providerServices.value = [];
						return;
					}

					// Use fetch with more detailed logging
					console.log("Fetching from URL:", "/api/service/provider");
					const response = await fetch("/api/service/provider", {
						headers: {
							Authorization: `Bearer ${token.value}`,
						},
					});

					console.log("Response status:", response.status);
					console.log(
						"Response headers:",
						Object.fromEntries([...response.headers.entries()])
					);

					if (response.status === 401 || response.status === 403) {
						console.error("Authentication error:", response.status);
						providerServices.value = [];
						return;
					}

					if (response.status === 404) {
						// Provider has no services yet, this is a normal case
						console.log("No services found for this provider");
						providerServices.value = [];
						return;
					}

					if (response.status === 500) {
						// Backend server error - handle gracefully
						console.error("Backend server error. Status:", response.status);
						providerServices.value = [];
						// Set server error flag
						serverError.value = true;
						// Show a helpful message but don't use an alert
						console.warn(
							"Services could not be loaded due to a server issue. This might be a temporary problem."
						);
						return;
					}

					if (!response.ok) {
						throw new Error(
							`Failed to load services: ${response.status} ${response.statusText}`
						);
					}

					// Clear server error if successful
					serverError.value = false;

					// Parse the response data
					const rawText = await response.text();
					let data;
					try {
						data = JSON.parse(rawText);
						console.log("Parsed JSON data:", data);
					} catch (e) {
						console.error("Failed to parse response as JSON:", e);
						console.log("Raw response was:", rawText);
						serverError.value = true;
						providerServices.value = [];
						return;
					}

					// Use our utility function to extract the array
					providerServices.value = extractArrayFromResponse(data, "services");
					console.log("Services loaded:", providerServices.value.length);
				} catch (error) {
					console.error("Error loading services:", error);
					// Set server error flag for other types of errors too
					if (error.message && error.message.includes("500")) {
						serverError.value = true;
					}
					// Only show alert for actual errors, not for empty services or server errors
					if (
						error.message &&
						!error.message.includes("404") &&
						!error.message.includes("500")
					) {
						alert("فشل تحميل الخدمات الخاصة بك. يرجى المحاولة مرة أخرى.");
					}
					providerServices.value = [];
				} finally {
					loading.value = false;
				}
			};

			const editService = (service) => {
				isEditing.value = true;
				showForm();
				currentServiceId.value = service.id;

				// Handle both formats - direct subCategories array or subCategories via ServiceSubCategory
				let subCategoryIds = [];
				if (service.subCategories && Array.isArray(service.subCategories)) {
					// Direct reference to subcategories (standard DTO format)
					subCategoryIds = service.subCategories.map((sc) => sc.id);
				} else if (
					service.serviceSubCategory &&
					Array.isArray(service.serviceSubCategory)
				) {
					// Expanded reference via ServiceSubCategory (Entity format with reference handler)
					subCategoryIds = service.serviceSubCategory
						.filter((ssc) => ssc && ssc.subCategory)
						.map((ssc) => ssc.subCategory.id);
				}

				serviceForm.value = {
					title: service.title,
					description: service.description,
					estimatedPrice: service.estimatedPrice,
					contactPhone: service.contactPhone,
					subCategoryIds: subCategoryIds,
				};

				// Expand categories with selected subcategories
				if (categories.value.length > 0) {
					openCategories.value = [];
					const selectedSubCatIds = new Set(serviceForm.value.subCategoryIds);

					// Find categories containing selected subcategories and expand them
					categories.value.forEach((category) => {
						if (
							category.subCategories &&
							category.subCategories.some(
								(subCat) =>
									subCat && subCat.id && selectedSubCatIds.has(subCat.id)
							)
						) {
							openCategories.value.push(category.id);
						}
					});
				}

				// Scroll to the form
				window.scrollTo({ top: 0, behavior: "smooth" });
			};

			const cancelForm = () => {
				isEditing.value = false;
				hideForm();
				currentServiceId.value = null;
				resetForm();
			};

			const resetForm = () => {
				serviceForm.value = {
					title: "",
					description: "",
					estimatedPrice: 0,
					contactPhone: "",
					subCategoryIds: [],
				};
			};

			const handleSubmit = async () => {
				try {
					console.log("Submitting service form:", serviceForm.value);

					// Validate required fields
					if (!serviceForm.value.title) {
						alert("يرجى إدخال عنوان الخدمة");
						return;
					}

					if (!serviceForm.value.description) {
						alert("يرجى إدخال وصف الخدمة");
						return;
					}

					if (serviceForm.value.subCategoryIds.length === 0) {
						alert("يرجى اختيار تصنيف فرعي واحد على الأقل");
						return;
					}

					const url = isEditing.value
						? `/api/service/${currentServiceId.value}`
						: "/api/service";

					// Debug API endpoint
					console.log(
						`Making ${isEditing.value ? "PUT" : "POST"} request to: ${url}`
					);

					// Create payload exactly matching backend CreateServiceDto structure
					// Using a string representation for SubCategoryIds to avoid complex object references
					const requestPayload = {
						title: serviceForm.value.title,
						description: serviceForm.value.description,
						estimatedPrice: Number(serviceForm.value.estimatedPrice),
						contactPhone: serviceForm.value.contactPhone,
						// Converting IDs to strings to potentially avoid serialization issues
						subCategoryIds: serviceForm.value.subCategoryIds.map((id) =>
							Number(id)
						),
					};

					console.log(
						"Sending payload matching backend DTO format:",
						requestPayload
					);

					try {
						// Try direct fetch first for debugging
						console.log("Attempting direct fetch first for debugging...");
						const directResponse = await fetch(url, {
							method: isEditing.value ? "PUT" : "POST",
							headers: {
								"Content-Type": "application/json",
								Authorization: `Bearer ${token.value}`,
							},
							body: JSON.stringify(requestPayload),
						});

						console.log("Direct fetch response status:", directResponse.status);
						try {
							const responseText = await directResponse.text();
							console.log("Direct fetch response text:", responseText);
							try {
								const responseData = JSON.parse(responseText);
								console.log("Direct fetch parsed data:", responseData);
							} catch (e) {
								console.warn("Could not parse response as JSON", e);
							}
						} catch (e) {
							console.error("Could not read response text", e);
						}

						// Continue with Axios for actual implementation
						console.log("Now using Axios for the actual request...");
						const response = await axios({
							method: isEditing.value ? "put" : "post",
							url: url,
							headers: {
								"Content-Type": "application/json",
								Authorization: `Bearer ${token.value}`,
							},
							data: requestPayload,
							// Adding transformer to simplify response data and avoid circular references
							transformResponse: [
								(data) => {
									try {
										// Try to parse, but return raw data if parsing fails
										console.log("Raw Axios response:", data);
										return typeof data === "string" ? JSON.parse(data) : data;
									} catch (e) {
										console.log(
											"Could not parse response, returning raw data",
											e
										);
										return data;
									}
								},
							],
						});

						console.log("Axios response:", response);
						console.log("Axios response data:", response.data);
						console.log("Response data type:", typeof response.data);

						if (typeof response.data === "object") {
							console.log("Response data keys:", Object.keys(response.data));
						}

						// Reset form and refresh services
						resetForm();
						isEditing.value = false;
						hideForm();
						currentServiceId.value = null;
						console.log("About to reload provider services...");
						await loadProviderServices();

						// Success message
						alert(
							isEditing.value
								? "تم تحديث الخدمة بنجاح!"
								: "تم إنشاء الخدمة بنجاح! ستكون متاحة بعد موافقة المشرف."
						);
					} catch (axiosError) {
						console.error("Axios error:", axiosError);
						if (axiosError.response) {
							console.error("Response status:", axiosError.response.status);
							console.error("Response headers:", axiosError.response.headers);

							// Get detailed error information
							let errorMessage = "فشل في إنشاء الخدمة";
							let isCycleError = false;

							if (axiosError.response.data) {
								console.error(
									"Detailed error response:",
									axiosError.response.data
								);

								// Check if this is a circular reference error
								if (
									typeof axiosError.response.data === "string" &&
									axiosError.response.data.includes("object cycle was detected")
								) {
									isCycleError = true;
									console.warn(
										"Detected circular reference error - this requires a backend fix"
									);
								}

								if (typeof axiosError.response.data === "string") {
									errorMessage = axiosError.response.data;
								} else if (axiosError.response.data.message) {
									errorMessage = axiosError.response.data.message;
								} else if (axiosError.response.data.error) {
									errorMessage = axiosError.response.data.error;
								} else {
									try {
										errorMessage = JSON.stringify(axiosError.response.data);
									} catch (e) {
										errorMessage = "خطأ غير معروف";
									}
								}
							}

							// If it's a circular reference error, show a more specific message
							if (isCycleError) {
								alert(
									`يوجد مشكلة في خادم البيانات يتعلق بمراجع دائرية. يرجى الاتصال بمسؤول النظام.`
								);
								return;
							}

							throw new Error(
								`فشل ${
									isEditing.value ? "تحديث" : "إنشاء"
								} الخدمة: ${errorMessage}`
							);
						} else {
							throw axiosError;
						}
					}
				} catch (error) {
					console.error(
						`Error ${isEditing.value ? "updating" : "creating"} service:`,
						error
					);
					alert(
						`فشل ${isEditing.value ? "تحديث" : "إنشاء"} الخدمة: ${
							error.message
						}`
					);
				}
			};

			const confirmDelete = (serviceId) => {
				serviceToDeleteId.value = serviceId;
			};

			const deleteService = async () => {
				try {
					const response = await fetch(
						`/api/service/${serviceToDeleteId.value}`,
						{
							method: "DELETE",
							headers: {
								Authorization: `Bearer ${token.value}`,
							},
						}
					);

					if (!response.ok) throw new Error("Failed to delete service");

					// Close modal and refresh services
					serviceToDeleteId.value = null;
					await loadProviderServices();

					// Success message
					alert("تم حذف الخدمة بنجاح!");
				} catch (error) {
					console.error("Error deleting service:", error);
					alert("فشل حذف الخدمة. يرجى المحاولة مرة أخرى.");
				}
			};

			const toggleForm = () => {
				showServiceForm.value = !showServiceForm.value;
			};

			const showForm = () => {
				showServiceForm.value = true;

				// Safely initialize categories if needed
				if (
					categories.value &&
					categories.value.length > 0 &&
					openCategories.value.length === 0
				) {
					openCategories.value = categories.value
						.filter((category) => category && category.id)
						.map((c) => c.id);
				}
			};

			const hideForm = () => {
				showServiceForm.value = false;
			};

			return {
				loading,
				categories,
				providerServices,
				serviceForm,
				isEditing,
				currentServiceId,
				serviceToDeleteId,
				showServiceForm,
				editService,
				cancelForm,
				resetForm,
				handleSubmit,
				confirmDelete,
				deleteService,
				categorySearch,
				openCategories,
				filteredCategories,
				toggleCategory,
				getSubCategoryName,
				selectedSubCategoriesCount,
				isAllSelected,
				toggleSelectAll,
				removeSubCategory,
				filteredSubCategories,
				toggleForm,
				showForm,
				hideForm,
				serverError,
				totalSubcategoriesCount,
				reloadCategories,
			};
		},
	};
</script>

<style scoped>
	.rtl {
		direction: rtl;
		text-align: right;
	}

	/* Add any additional styling here */
	input:focus,
	textarea:focus {
		border-color: #3b82f6;
	}

	/* Animation for success feedback */
	@keyframes highlight {
		0% {
			background-color: rgba(59, 130, 246, 0.1);
		}
		100% {
			background-color: transparent;
		}
	}

	.highlight {
		animation: highlight 2s ease-out;
	}
</style>
