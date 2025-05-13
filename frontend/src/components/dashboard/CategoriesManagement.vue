<template>
	<div class="space-y-6 animate-fadeIn">
		<h1 class="text-3xl font-bold text-gray-800 mb-6">إدارة التصنيفات</h1>

		<div class="grid grid-cols-1 lg:grid-cols-2 gap-6">
			<!-- Categories List -->
			<div
				class="bg-gradient-to-br from-white to-blue-50 rounded-xl shadow-md overflow-hidden"
			>
				<div
					class="bg-gradient-to-r from-blue-600 to-blue-700 text-white p-4 flex justify-between items-center"
				>
					<p class="font-bold text-xl flex items-center text-white">
						<i class="fas fa-folder mr-2"></i>
						التصنيفات الرئيسية
					</p>
					<button
						@click="openAddCategoryModal"
						class="px-3 py-1.5 bg-white text-blue-600 rounded-lg flex items-center gap-1 hover:bg-gray-100 transition-colors shadow-sm"
					>
						<i class="fas fa-plus-circle"></i>
						<span>إضافة تصنيف</span>
					</button>
				</div>

				<div class="p-4">
					<div v-if="loadingCategories" class="flex justify-center p-6">
						<div
							class="animate-spin rounded-full h-12 w-12 border-t-2 border-b-2 border-blue-500"
						></div>
					</div>
					<div v-else-if="categories.length === 0" class="text-center p-10">
						<i class="fas fa-folder-open text-blue-300 text-5xl mb-3"></i>
						<p class="text-gray-500 font-medium">لا توجد تصنيفات بعد</p>
						<p class="text-gray-400 text-sm mt-1">
							انقر على "إضافة تصنيف" لإنشاء تصنيف جديد
						</p>
					</div>

					<ul v-else class="divide-y divide-gray-200">
						<li
							v-for="category in categories"
							:key="category.id"
							class="p-3 hover:bg-blue-50/50 rounded-lg transition-all cursor-pointer"
							:class="{
								'bg-blue-50/70':
									selectedCategory && selectedCategory.id === category.id,
							}"
							@click="selectCategory(category)"
						>
							<div class="flex items-center justify-between">
								<div class="flex items-center">
									<div
										class="w-10 h-10 rounded-full bg-gradient-to-br from-blue-100 to-blue-200 flex items-center justify-center mr-3 text-blue-600 shadow-sm"
									>
										<i class="fas fa-folder"></i>
									</div>
									<div>
										<h3 class="font-medium text-gray-800">
											{{ category.name }}
										</h3>
										<p class="text-gray-500 text-sm">
											{{
												category.subCategories
													? category.subCategories.length
													: 0
											}}
											تصنيف فرعي
										</p>
									</div>
								</div>
								<div class="flex gap-2">
									<button
										@click.stop="editCategory(category)"
										class="p-2 bg-blue-100 text-blue-600 rounded-lg hover:bg-blue-200 transition-colors"
										title="تعديل"
									>
										<i class="fas fa-edit"></i>
									</button>
									<button
										@click.stop="confirmDeleteCategory(category)"
										class="p-2 bg-red-100 text-red-600 rounded-lg hover:bg-red-200 transition-colors"
										title="حذف"
									>
										<i class="fas fa-trash-alt"></i>
									</button>
								</div>
							</div>
						</li>
					</ul>
				</div>
			</div>

			<!-- SubCategories -->
			<div
				class="bg-gradient-to-br from-white to-blue-50 rounded-xl shadow-md overflow-hidden"
			>
				<div
					class="bg-gradient-to-r from-purple-600 to-purple-700 text-white p-4 flex justify-between items-center"
				>
					<p class="font-bold text-xl flex items-center text-white">
						<i class="fas fa-sitemap mr-2"></i>
						التصنيفات الفرعية
						<span
							v-if="selectedCategory"
							class="mr-2 text-sm bg-white/20 px-2 py-1 rounded-lg"
						>
							{{ selectedCategory.name }}
						</span>
					</p>
					<button
						@click="openAddSubCategoryModal"
						:disabled="!selectedCategory"
						:class="[
							'px-3 py-1.5 rounded-lg flex items-center gap-1 transition-colors shadow-sm',
							selectedCategory
								? 'bg-white text-purple-600 hover:bg-gray-100'
								: 'bg-gray-200 text-gray-500 cursor-not-allowed',
						]"
					>
						<i class="fas fa-plus-circle"></i>
						<span>إضافة تصنيف فرعي</span>
					</button>
				</div>

				<div class="p-4">
					<div v-if="!selectedCategory" class="text-center p-10">
						<i class="fas fa-hand-pointer text-purple-300 text-5xl mb-3"></i>
						<p class="text-gray-500 font-medium">
							الرجاء اختيار تصنيف رئيسي أولاً
						</p>
						<p class="text-gray-400 text-sm mt-1">
							لعرض وإدارة التصنيفات الفرعية، يرجى النقر على تصنيف رئيسي من
							القائمة
						</p>
					</div>
					<div v-else-if="loadingSubCategories" class="flex justify-center p-6">
						<div
							class="animate-spin rounded-full h-12 w-12 border-t-2 border-b-2 border-purple-500"
						></div>
					</div>
					<div
						v-else-if="
							!selectedCategory.subCategories ||
							selectedCategory.subCategories.length === 0
						"
						class="text-center p-10"
					>
						<i class="fas fa-folder-open text-purple-300 text-5xl mb-3"></i>
						<p class="text-gray-500 font-medium">لا توجد تصنيفات فرعية بعد</p>
						<p class="text-gray-400 text-sm mt-1">
							انقر على "إضافة تصنيف فرعي" لإنشاء تصنيف فرعي جديد
						</p>
					</div>

					<ul v-else class="divide-y divide-gray-100">
						<li
							v-for="subCategory in selectedCategory.subCategories"
							:key="subCategory.id"
							class="p-3 hover:bg-purple-50/30 transition-all rounded-lg"
						>
							<div class="flex items-center justify-between">
								<div class="flex items-center">
									<div
										class="w-10 h-10 rounded-full bg-gradient-to-br from-purple-100 to-purple-200 flex items-center justify-center mr-3 text-purple-600 shadow-sm"
									>
										<i class="fas fa-layer-group"></i>
									</div>
									<div>
										<h3 class="font-medium text-gray-800">
											{{ subCategory.name }}
										</h3>
									</div>
								</div>
								<div class="flex gap-2">
									<button
										@click="editSubCategory(subCategory)"
										class="p-2 bg-purple-100 text-purple-600 rounded-lg hover:bg-purple-200 transition-colors"
										title="تعديل"
									>
										<i class="fas fa-edit"></i>
									</button>
									<button
										@click="confirmDeleteSubCategory(subCategory)"
										class="p-2 bg-red-100 text-red-600 rounded-lg hover:bg-red-200 transition-colors"
										title="حذف"
									>
										<i class="fas fa-trash-alt"></i>
									</button>
								</div>
							</div>
						</li>
					</ul>
				</div>
			</div>
		</div>

		<!-- Category Add Modal -->
		<Modal
			:is-open="showAddCategoryModal"
			title="إضافة تصنيف جديد"
			size="md"
			@close="showAddCategoryModal = false"
		>
			<template #header-icon>
				<div class="bg-white/20 p-2 rounded-full">
					<i class="fas fa-folder-plus"></i>
				</div>
			</template>

			<div class="space-y-4 text-right">
				<div>
					<label class="block text-sm font-medium text-gray-700 mb-1"
						>اسم التصنيف</label
					>
					<input
						type="text"
						v-model="newCategoryName"
						class="w-full border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 px-4 py-2"
						placeholder="أدخل اسم التصنيف"
					/>
				</div>
			</div>

			<template #actions>
				<button
					class="px-4 py-2 bg-gray-200 hover:bg-gray-300 text-gray-800 rounded transition-colors focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-gray-300"
					@click="showAddCategoryModal = false"
				>
					إلغاء
				</button>
				<button
					class="px-4 py-2 bg-blue-600 hover:bg-blue-700 text-white rounded transition-colors focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500"
					@click="addCategory"
				>
					إضافة
				</button>
			</template>
		</Modal>

		<!-- Category Edit Modal -->
		<Modal
			:is-open="showEditCategoryModal"
			title="تعديل التصنيف"
			size="md"
			@close="showEditCategoryModal = false"
		>
			<template #header-icon>
				<div class="bg-white/20 p-2 rounded-full">
					<i class="fas fa-edit"></i>
				</div>
			</template>

			<div class="space-y-4 text-right">
				<div>
					<label class="block text-sm font-medium text-gray-700 mb-1"
						>اسم التصنيف</label
					>
					<input
						type="text"
						v-model="newCategoryName"
						class="w-full border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 px-4 py-2"
						placeholder="أدخل اسم التصنيف"
					/>
				</div>
			</div>

			<template #actions>
				<button
					class="px-4 py-2 bg-gray-200 hover:bg-gray-300 text-gray-800 rounded transition-colors focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-gray-300"
					@click="showEditCategoryModal = false"
				>
					إلغاء
				</button>
				<button
					class="px-4 py-2 bg-blue-600 hover:bg-blue-700 text-white rounded transition-colors focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500"
					@click="updateCategory"
				>
					حفظ
				</button>
			</template>
		</Modal>

		<!-- Sub Category Add Modal -->
		<Modal
			:is-open="showAddSubCategoryModal"
			title="إضافة تصنيف فرعي جديد"
			size="md"
			@close="showAddSubCategoryModal = false"
		>
			<template #header-icon>
				<div class="bg-white/20 p-2 rounded-full">
					<i class="fas fa-layer-group"></i>
				</div>
			</template>

			<div class="space-y-4 text-right">
				<div>
					<label class="block text-sm font-medium text-gray-700 mb-1"
						>اسم التصنيف الفرعي</label
					>
					<input
						type="text"
						v-model="newSubCategoryName"
						class="w-full border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 px-4 py-2"
						placeholder="أدخل اسم التصنيف الفرعي"
					/>
				</div>
			</div>

			<template #actions>
				<button
					class="px-4 py-2 bg-gray-200 hover:bg-gray-300 text-gray-800 rounded transition-colors focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-gray-300"
					@click="showAddSubCategoryModal = false"
				>
					إلغاء
				</button>
				<button
					class="px-4 py-2 bg-blue-600 hover:bg-blue-700 text-white rounded transition-colors focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500"
					@click="addSubCategory"
				>
					إضافة
				</button>
			</template>
		</Modal>

		<!-- Sub Category Edit Modal -->
		<Modal
			:is-open="showEditSubCategoryModal"
			title="تعديل التصنيف الفرعي"
			size="md"
			@close="showEditSubCategoryModal = false"
		>
			<template #header-icon>
				<div class="bg-white/20 p-2 rounded-full">
					<i class="fas fa-pencil-alt"></i>
				</div>
			</template>

			<div class="space-y-4 text-right">
				<div>
					<label class="block text-sm font-medium text-gray-700 mb-1"
						>اسم التصنيف الفرعي</label
					>
					<input
						type="text"
						v-model="newSubCategoryName"
						class="w-full border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 px-4 py-2"
						placeholder="أدخل اسم التصنيف الفرعي"
					/>
				</div>
			</div>

			<template #actions>
				<button
					class="px-4 py-2 bg-gray-200 hover:bg-gray-300 text-gray-800 rounded transition-colors focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-gray-300"
					@click="showEditSubCategoryModal = false"
				>
					إلغاء
				</button>
				<button
					class="px-4 py-2 bg-blue-600 hover:bg-blue-700 text-white rounded transition-colors focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500"
					@click="updateSubCategory"
				>
					حفظ
				</button>
			</template>
		</Modal>

		<!-- Delete Confirmation Modal -->
		<Modal
			:is-open="showDeleteConfirmModal"
			title="تأكيد الحذف"
			size="sm"
			@close="showDeleteConfirmModal = false"
		>
			<template #header-icon>
				<div class="bg-white/20 p-2 rounded-full">
					<i class="fas fa-trash-alt"></i>
				</div>
			</template>

			<div class="text-right">
				<p class="text-gray-700">
					هل أنت متأكد من رغبتك في حذف
					<span v-if="deleteType === 'category'">التصنيف</span>
					<span v-else>التصنيف الفرعي</span>؟
				</p>
				<p class="text-red-600 text-sm mt-2">لا يمكن التراجع عن هذا الإجراء.</p>
			</div>

			<template #actions>
				<button
					class="px-4 py-2 bg-gray-200 hover:bg-gray-300 text-gray-800 rounded transition-colors focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-gray-300"
					@click="showDeleteConfirmModal = false"
				>
					إلغاء
				</button>
				<button
					class="px-4 py-2 bg-red-600 hover:bg-red-700 text-white rounded transition-colors focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-red-500"
					@click="confirmDelete"
				>
					حذف
				</button>
			</template>
		</Modal>
	</div>
</template>

<script>
	import { ref, onMounted, watch } from "vue";
	import { useUserStore } from "@/store/userStore";
	import { useToastStore } from "@/store/toastStore";
	import Modal from "@/components/Modal.vue";

	export default {
		name: "CategoriesManagement",
		components: {
			Modal,
		},
		props: {
			userToken: {
				type: String,
				required: true,
			},
		},
		setup(props) {
			const userStore = useUserStore();
			const toastStore = useToastStore();

			// State variables
			// These are the reactive variables that hold the state of our categories and subcategories.
			const categories = ref([]);
			const selectedCategory = ref(null);
			const selectedSubCategory = ref(null);
			const loadingCategories = ref(false);
			const loadingSubCategories = ref(false);
			const showAddCategoryModal = ref(false);
			const showAddSubCategoryModal = ref(false);
			const showEditCategoryModal = ref(false);
			const showEditSubCategoryModal = ref(false);
			const newCategoryName = ref("");
			const newSubCategoryName = ref("");
			const itemToDelete = ref(null);
			const deleteType = ref("");
			const showDeleteConfirmModal = ref(false);

			// Helper function to get API URL
			// This function constructs the full API URL using the base URL from environment variables.
			const getApiUrl = (endpoint) => {
				const baseUrl = import.meta.env.VITE_API_URL || "";
				return `${baseUrl}${endpoint}`;
			};

			// Format date function
			// Converts a date string into a more readable format in Arabic.
			const formatDate = (dateString) => {
				if (!dateString) return "غير متوفر";
				const date = new Date(dateString);
				return date.toLocaleDateString("ar-SA", {
					year: "numeric",
					month: "long",
					day: "numeric",
				});
			};

			// Load all categories with subcategories
			// Fetches all categories and their subcategories from the server.
			const loadCategories = async () => {
				loadingCategories.value = true;
				try {
					// Use our helper to get the API URL
					const apiUrl = getApiUrl("/api/category");

					console.log("Fetching categories from:", apiUrl);
					const response = await fetch(apiUrl);

					// Check if response is OK (status in the range 200-299)
					// Verifies that the server response is successful.
					if (response.ok) {
						// Check for content type to prevent HTML parsing errors
						// Ensures the response is in JSON format to avoid parsing issues.
						const contentType = response.headers.get("content-type");
						if (contentType && contentType.includes("application/json")) {
							const data = await response.json();
							console.log("Categories loaded:", data);

							// Handle different response formats
							// Processes the response data to extract categories, accommodating various API response structures.
							if (Array.isArray(data)) {
								categories.value = data;
							} else if (data && typeof data === "object") {
								// Try to extract categories array from common response formats
								// Attempts to find the categories array in the response, checking several common properties.
								if (data.categories && Array.isArray(data.categories)) {
									categories.value = data.categories;
								} else if (data.data && Array.isArray(data.data)) {
									categories.value = data.data;
								} else if (data.items && Array.isArray(data.items)) {
									categories.value = data.items;
								} else if (data.results && Array.isArray(data.results)) {
									categories.value = data.results;
								} else if (data.$values && Array.isArray(data.$values)) {
									categories.value = data.$values;
								} else {
									// If we can't extract an array, try to use the object as is
									// If no array is found, we treat the response as a single category object.
									categories.value = Array.isArray(data) ? data : [data];
								}
							} else {
								categories.value = [];
								console.error("Unexpected categories data format:", data);
							}
						} else {
							// Handle non-JSON response
							// Logs an error if the response isn't JSON and shows a toast notification.
							console.error(
								"API returned non-JSON content:",
								await response.text()
							);
							toastStore.error("خطأ في استجابة الخادم - تنسيق غير صالح");
							categories.value = [];
						}
					} else {
						// Handle HTTP error
						// Logs the error and shows a toast notification if the HTTP request fails.
						const errorText = await response.text();
						console.error(
							`Failed to load categories. Status: ${response.status}`,
							errorText
						);
						toastStore.error(`فشل تحميل التصنيفات (${response.status})`);
						categories.value = [];
					}
				} catch (error) {
					console.error("Error loading categories:", error);
					toastStore.error("حدث خطأ أثناء تحميل التصنيفات");
					categories.value = [];

					// If we're in development mode, create dummy categories for testing
					// Adds some dummy categories to the list for testing purposes during development.
					if (import.meta.env.DEV) {
						console.log("Creating dummy categories for development");
						categories.value = [
							{ id: "dummy1", name: "تصنيف تجريبي 1", subCategories: [] },
							{ id: "dummy2", name: "تصنيف تجريبي 2", subCategories: [] },
						];
					}
				} finally {
					loadingCategories.value = false;
				}
			};

			// Select a category to show its subcategories
			// Sets the selected category and loads its subcategories.
			const selectCategory = (category) => {
				selectedCategory.value = category;
				loadSubCategories(category.id);
			};

			// Load subcategories for a specific category
			// Fetches subcategories for the given category ID from the server.
			const loadSubCategories = async (categoryId) => {
				if (!categoryId) return;

				loadingSubCategories.value = true;
				try {
					// Try different API endpoint patterns
					const endpoints = [
						`/api/subcategory/category/${categoryId}`,
						`/api/subcategory/bycategory/${categoryId}`,
						`/api/category/${categoryId}/subcategories`,
					];

					let subCategories = [];
					let success = false;

					// Try each endpoint until one works
					for (const endpoint of endpoints) {
						try {
							const apiUrl = getApiUrl(endpoint);
							console.log(`Trying to fetch subcategories from: ${apiUrl}`);

							const response = await fetch(apiUrl, {
								headers: {
									Authorization: `Bearer ${props.userToken}`,
								},
							});

							if (response.ok) {
								const contentType = response.headers.get("content-type");
								if (contentType && contentType.includes("application/json")) {
									const data = await response.json();

									// Find the subcategory array in the response
									// Searches for the subcategories array in the response data.
									if (Array.isArray(data)) {
										subCategories = data;
									} else if (data && typeof data === "object") {
										// Try different possible array properties
										// Checks multiple properties to locate the subcategories array.
										if (
											data.subCategories &&
											Array.isArray(data.subCategories)
										) {
											subCategories = data.subCategories;
										} else if (data.data && Array.isArray(data.data)) {
											subCategories = data.data;
										} else if (data.items && Array.isArray(data.items)) {
											subCategories = data.items;
										} else if (data.results && Array.isArray(data.results)) {
											subCategories = data.results;
										} else if (data.$values && Array.isArray(data.$values)) {
											subCategories = data.$values;
										}
									}

									if (subCategories.length > 0) {
										// Update the category with subcategories
										// Updates the category object with its subcategories once fetched.
										const categoryIndex = categories.value.findIndex(
											(c) => c.id === categoryId
										);
										if (categoryIndex !== -1) {
											categories.value[categoryIndex].subCategories =
												subCategories;
											console.log(
												`Loaded ${subCategories.length} subcategories for category ${categoryId} from ${endpoint}`
											);

											// If we're working with the selected category, update it
											// Ensures the selected category is updated with new subcategory data.
											if (
												selectedCategory.value &&
												selectedCategory.value.id === categoryId
											) {
												selectedCategory.value =
													categories.value[categoryIndex];
											}

											success = true;
											break; // Exit the loop if we got data
										}
									}
								}
							}
						} catch (error) {
							console.error(`Error trying endpoint ${endpoint}:`, error);
						}
					}

					if (!success) {
						console.warn(
							`Failed to load subcategories for category ID ${categoryId} from any endpoint`
						);
					}
				} catch (error) {
					console.error("Error loading subcategories:", error);
					toastStore.error("حدث خطأ أثناء تحميل التصنيفات الفرعية");
				} finally {
					loadingSubCategories.value = false;
				}
			};

			// Open modal to add new category
			// Resets the new category name and opens the modal for adding a category.
			const openAddCategoryModal = () => {
				newCategoryName.value = "";
				showAddCategoryModal.value = true;
			};

			// Add a new main category
			// Sends a request to add a new category and updates the list upon success.
			const addCategory = async () => {
				if (!newCategoryName.value.trim()) {
					toastStore.error("الرجاء إدخال اسم التصنيف");
					return;
				}

				try {
					// Make sure we have an API base URL, with fallback to relative URL
					const baseUrl = import.meta.env.VITE_API_URL || "";
					const apiUrl = `${baseUrl}/api/category`;

					const response = await fetch(apiUrl, {
						method: "POST",
						headers: {
							"Content-Type": "application/json",
							Authorization: `Bearer ${props.userToken}`,
						},
						body: JSON.stringify({ name: newCategoryName.value.trim() }),
					});

					if (response.ok) {
						// Check for content type
						const contentType = response.headers.get("content-type");
						if (contentType && contentType.includes("application/json")) {
							const newCategory = await response.json();
							categories.value.push(newCategory);
							showAddCategoryModal.value = false;
							newCategoryName.value = "";
							toastStore.success("تمت إضافة التصنيف بنجاح");
						} else {
							console.error(
								"API returned non-JSON content:",
								await response.text()
							);
							toastStore.error(
								"تمت إضافة التصنيف ولكن هناك مشكلة في استجابة الخادم"
							);
							showAddCategoryModal.value = false;
							newCategoryName.value = "";
							// Reload categories to ensure we have the latest data
							await loadCategories();
						}
					} else {
						const errorText = await response.text();
						console.error(
							`Failed to add category. Status: ${response.status}`,
							errorText
						);
						toastStore.error(`فشل إضافة التصنيف (${response.status})`);
					}
				} catch (error) {
					console.error("Error adding category:", error);
					toastStore.error("حدث خطأ أثناء إضافة التصنيف");
				}
			};

			// Edit category
			// Opens the modal to edit the selected category's details.
			const editCategory = (category) => {
				selectedCategory.value = { ...category };
				newCategoryName.value = category.name;
				showEditCategoryModal.value = true;
			};

			// Update category
			// Sends a request to update the category and refreshes the list upon success.
			const updateCategory = async () => {
				if (!newCategoryName.value.trim()) {
					toastStore.error("الرجاء إدخال اسم التصنيف");
					return;
				}

				try {
					const apiUrl = getApiUrl(
						`/api/category/${selectedCategory.value.id}`
					);

					const response = await fetch(apiUrl, {
						method: "PUT",
						headers: {
							"Content-Type": "application/json",
							Authorization: `Bearer ${props.userToken}`,
						},
						body: JSON.stringify({ name: newCategoryName.value.trim() }),
					});

					if (response.ok) {
						// Check for content type
						const contentType = response.headers.get("content-type");
						if (contentType && contentType.includes("application/json")) {
							const updatedCategory = await response.json();
							const index = categories.value.findIndex(
								(c) => c.id === updatedCategory.id
							);
							if (index !== -1) {
								categories.value[index] = updatedCategory;
								if (
									selectedCategory.value &&
									selectedCategory.value.id === updatedCategory.id
								) {
									selectedCategory.value = updatedCategory;
								}
							}
							showEditCategoryModal.value = false;
							newCategoryName.value = "";
							toastStore.success("تم تحديث التصنيف بنجاح");
						} else {
							console.error(
								"API returned non-JSON content:",
								await response.text()
							);
							toastStore.success(
								"تم تحديث التصنيف ولكن هناك مشكلة في استجابة الخادم"
							);
							showEditCategoryModal.value = false;
							newCategoryName.value = "";
							// Reload categories to ensure we have the latest data
							await loadCategories();
						}
					} else {
						const errorText = await response.text();
						console.error(
							`Failed to update category. Status: ${response.status}`,
							errorText
						);
						toastStore.error(`فشل تحديث التصنيف (${response.status})`);
					}
				} catch (error) {
					console.error("Error updating category:", error);
					toastStore.error("حدث خطأ أثناء تحديث التصنيف");
				}
			};

			// Confirm delete category
			// Prepares to delete a category by setting the item to delete and showing the confirmation modal.
			const confirmDeleteCategory = (category) => {
				itemToDelete.value = category;
				deleteType.value = "category";
				showDeleteConfirmModal.value = true;
			};

			// Delete category
			// Sends a request to delete the category and updates the list upon success.
			const deleteCategory = async () => {
				if (!itemToDelete.value) return;

				try {
					const apiUrl = getApiUrl(`/api/category/${itemToDelete.value.id}`);

					const response = await fetch(apiUrl, {
						method: "DELETE",
						headers: {
							Authorization: `Bearer ${props.userToken}`,
						},
					});

					if (response.ok) {
						categories.value = categories.value.filter(
							(c) => c.id !== itemToDelete.value.id
						);
						if (
							selectedCategory.value &&
							selectedCategory.value.id === itemToDelete.value.id
						) {
							selectedCategory.value = null;
						}
						showDeleteConfirmModal.value = false;
						itemToDelete.value = null;
						toastStore.success("تم حذف التصنيف بنجاح");
					} else {
						const errorText = await response.text();
						console.error(
							`Failed to delete category. Status: ${response.status}`,
							errorText
						);
						toastStore.error(`فشل حذف التصنيف (${response.status})`);
					}
				} catch (error) {
					console.error("Error deleting category:", error);
					toastStore.error("حدث خطأ أثناء حذف التصنيف");
				}
			};

			// Open modal to add new subcategory
			// Resets the new subcategory name and opens the modal for adding a subcategory.
			const openAddSubCategoryModal = () => {
				if (!selectedCategory.value) {
					toastStore.info("الرجاء اختيار تصنيف رئيسي أولاً");
					return;
				}
				newSubCategoryName.value = "";
				showAddSubCategoryModal.value = true;
			};

			// Add a new subcategory
			// Sends a request to add a new subcategory and updates the list upon success.
			const addSubCategory = async () => {
				if (!newSubCategoryName.value.trim()) {
					toastStore.error("الرجاء إدخال اسم التصنيف الفرعي");
					return;
				}

				try {
					const apiUrl = getApiUrl("/api/subcategory");

					const response = await fetch(apiUrl, {
						method: "POST",
						headers: {
							"Content-Type": "application/json",
							Authorization: `Bearer ${props.userToken}`,
						},
						body: JSON.stringify({
							name: newSubCategoryName.value.trim(),
							categoryId: selectedCategory.value.id,
						}),
					});

					if (response.ok) {
						// Check for content type
						const contentType = response.headers.get("content-type");
						if (contentType && contentType.includes("application/json")) {
							const newSubCategory = await response.json();
							if (!selectedCategory.value.subCategories) {
								selectedCategory.value.subCategories = [];
							}
							selectedCategory.value.subCategories.push(newSubCategory);
							showAddSubCategoryModal.value = false;
							newSubCategoryName.value = "";
							toastStore.success("تمت إضافة التصنيف الفرعي بنجاح");
						} else {
							console.error(
								"API returned non-JSON content:",
								await response.text()
							);
							toastStore.success(
								"تمت إضافة التصنيف الفرعي ولكن هناك مشكلة في استجابة الخادم"
							);
							showAddSubCategoryModal.value = false;
							newSubCategoryName.value = "";
							// Reload categories to get the updated subcategories
							await loadCategories();
						}
					} else {
						const errorText = await response.text();
						console.error(
							`Failed to add subcategory. Status: ${response.status}`,
							errorText
						);
						toastStore.error(`فشل إضافة التصنيف الفرعي (${response.status})`);
					}
				} catch (error) {
					console.error("Error adding subcategory:", error);
					toastStore.error("حدث خطأ أثناء إضافة التصنيف الفرعي");
				}
			};

			// Edit subcategory
			// Opens the modal to edit the selected subcategory's details.
			const editSubCategory = (subCategory) => {
				selectedSubCategory.value = { ...subCategory };
				newSubCategoryName.value = subCategory.name;
				showEditSubCategoryModal.value = true;
			};

			// Update subcategory
			// Sends a request to update the subcategory and refreshes the list upon success.
			const updateSubCategory = async () => {
				if (!newSubCategoryName.value.trim()) {
					toastStore.error("الرجاء إدخال اسم التصنيف الفرعي");
					return;
				}

				try {
					const apiUrl = getApiUrl(
						`/api/subcategory/${selectedSubCategory.value.id}`
					);

					const response = await fetch(apiUrl, {
						method: "PUT",
						headers: {
							"Content-Type": "application/json",
							Authorization: `Bearer ${props.userToken}`,
						},
						body: JSON.stringify({
							name: newSubCategoryName.value.trim(),
							categoryId: selectedCategory.value.id,
						}),
					});

					if (response.ok) {
						// Check for content type
						const contentType = response.headers.get("content-type");
						if (contentType && contentType.includes("application/json")) {
							const updatedSubCategory = await response.json();
							const index = selectedCategory.value.subCategories.findIndex(
								(sc) => sc.id === updatedSubCategory.id
							);
							if (index !== -1) {
								selectedCategory.value.subCategories[index] =
									updatedSubCategory;
							}
							showEditSubCategoryModal.value = false;
							newSubCategoryName.value = "";
							toastStore.success("تم تحديث التصنيف الفرعي بنجاح");
						} else {
							console.error(
								"API returned non-JSON content:",
								await response.text()
							);
							toastStore.success(
								"تم تحديث التصنيف الفرعي ولكن هناك مشكلة في استجابة الخادم"
							);
							showEditSubCategoryModal.value = false;
							newSubCategoryName.value = "";
							// Reload the category to get updated subcategories
							if (selectedCategory.value) {
								const catId = selectedCategory.value.id;
								await loadCategories();
								selectedCategory.value =
									categories.value.find((c) => c.id === catId) || null;
							}
						}
					} else {
						const errorText = await response.text();
						console.error(
							`Failed to update subcategory. Status: ${response.status}`,
							errorText
						);
						toastStore.error(`فشل تحديث التصنيف الفرعي (${response.status})`);
					}
				} catch (error) {
					console.error("Error updating subcategory:", error);
					toastStore.error("حدث خطأ أثناء تحديث التصنيف الفرعي");
				}
			};

			// Confirm delete subcategory
			// Prepares to delete a subcategory by setting the item to delete and showing the confirmation modal.
			const confirmDeleteSubCategory = (subCategory) => {
				itemToDelete.value = subCategory;
				deleteType.value = "subcategory";
				showDeleteConfirmModal.value = true;
			};

			// Delete subcategory
			// Sends a request to delete the subcategory and updates the list upon success.
			const deleteSubCategory = async () => {
				if (!itemToDelete.value) return;

				try {
					const apiUrl = getApiUrl(`/api/subcategory/${itemToDelete.value.id}`);

					const response = await fetch(apiUrl, {
						method: "DELETE",
						headers: {
							Authorization: `Bearer ${props.userToken}`,
						},
					});

					if (response.ok) {
						selectedCategory.value.subCategories =
							selectedCategory.value.subCategories.filter(
								(sc) => sc.id !== itemToDelete.value.id
							);
						showDeleteConfirmModal.value = false;
						itemToDelete.value = null;
						toastStore.success("تم حذف التصنيف الفرعي بنجاح");
					} else {
						const errorText = await response.text();
						console.error(
							`Failed to delete subcategory. Status: ${response.status}`,
							errorText
						);
						toastStore.error(`فشل حذف التصنيف الفرعي (${response.status})`);
					}
				} catch (error) {
					console.error("Error deleting subcategory:", error);
					toastStore.error("حدث خطأ أثناء حذف التصنيف الفرعي");
				}
			};

			// Handle confirmation for deleting items
			// Determines whether to delete a category or subcategory based on the deleteType.
			const confirmDelete = () => {
				if (deleteType.value === "category") {
					deleteCategory();
				} else if (deleteType.value === "subcategory") {
					deleteSubCategory();
				}
			};

			// Watch for categories changes to load subcategories if needed
			// Observes changes in the categories list and loads subcategories if necessary.
			watch(
				categories,
				async (newCategories) => {
					if (newCategories.length > 0) {
						// Load subcategories for all categories
						// Iterates over all categories to load their subcategories.
						for (const category of newCategories) {
							if (
								!category.subCategories ||
								category.subCategories.length === 0
							) {
								await loadSubCategories(category.id);
							}
						}
					}
				},
				{ immediate: false }
			);

			onMounted(async () => {
				await loadCategories();

				// After categories are loaded, try to load subcategories for each
				// Once categories are fetched, attempts to load subcategories for each one.
				if (categories.value.length > 0) {
					for (const category of categories.value) {
						await loadSubCategories(category.id);
					}
				}
			});

			return {
				categories,
				selectedCategory,
				selectedSubCategory,
				loadingCategories,
				loadingSubCategories,
				showAddCategoryModal,
				showAddSubCategoryModal,
				showEditCategoryModal,
				showEditSubCategoryModal,
				showDeleteConfirmModal,
				newCategoryName,
				newSubCategoryName,
				itemToDelete,
				deleteType,

				// Methods
				formatDate,
				selectCategory,
				openAddCategoryModal,
				addCategory,
				editCategory,
				updateCategory,
				confirmDeleteCategory,
				openAddSubCategoryModal,
				addSubCategory,
				editSubCategory,
				updateSubCategory,
				confirmDeleteSubCategory,
				confirmDelete,
			};
		},
	};
</script>

<style scoped>
	/* Improved styling for modals */
	:deep(.modal-container.modal-md) {
		max-width: 30rem; /* Ensure consistent width for medium modals */
	}

	:deep(.modal-container.modal-sm) {
		max-width: 24rem; /* Ensure consistent width for small modals */
	}

	:deep(.modal-header) {
		background: linear-gradient(135deg, #3b82f6, #2563eb, #1d4ed8);
	}

	/* Animation for the categories */
	.category-item-enter-active,
	.category-item-leave-active {
		transition: all 0.3s ease;
	}

	.category-item-enter-from,
	.category-item-leave-to {
		opacity: 0;
		transform: translateY(10px);
	}

	/* Make sure inputs look good at all widths */
	@media (max-width: 640px) {
		:deep(.modal-container) {
			margin: 0;
			max-height: 100vh;
			border-radius: 0;
		}
	}

	/* Loading spinner animations */
	@keyframes spin {
		to {
			transform: rotate(360deg);
		}
	}

	.animate-spin {
		animation: spin 1s linear infinite;
	}

	.transition-all {
		transition-property: all;
		transition-timing-function: cubic-bezier(0.4, 0, 0.2, 1);
		transition-duration: 300ms;
	}
</style>
