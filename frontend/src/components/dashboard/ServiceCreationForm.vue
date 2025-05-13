<template>
	<div class="bg-white shadow-md rounded-lg p-6">
		<h2 class="text-2xl font-semibold text-gray-800 mb-6">إنشاء خدمة جديدة</h2>

		<form @submit.prevent="submitForm" class="space-y-6">
			<!-- Title Field -->
			<div>
				<label for="title" class="block text-sm font-medium text-gray-700 mb-1">
					عنوان الخدمة <span class="text-red-500">*</span>
				</label>
				<input
					v-model="formData.title"
					type="text"
					id="title"
					class="w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
					required
					placeholder="أدخل عنوان الخدمة"
				/>
			</div>

			<!-- Description Field -->
			<div>
				<label
					for="description"
					class="block text-sm font-medium text-gray-700 mb-1"
				>
					وصف الخدمة <span class="text-red-500">*</span>
				</label>
				<textarea
					v-model="formData.description"
					id="description"
					rows="4"
					class="w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
					required
					placeholder="أدخل وصف تفصيلي للخدمة"
				></textarea>
			</div>

			<div class="grid grid-cols-1 md:grid-cols-2 gap-4">
				<!-- Price Field -->
				<div>
					<label
						for="price"
						class="block text-sm font-medium text-gray-700 mb-1"
					>
						السعر التقريبي (دينار عراقي) <span class="text-red-500">*</span>
					</label>
					<input
						v-model.number="formData.estimatedPrice"
						type="number"
						id="price"
						min="0"
						step="1000"
						class="w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
						required
						placeholder="أدخل السعر التقريبي"
					/>
				</div>

				<!-- Contact Phone Field -->
				<div>
					<label
						for="phone"
						class="block text-sm font-medium text-gray-700 mb-1"
					>
						رقم الهاتف للتواصل <span class="text-red-500">*</span>
					</label>
					<input
						v-model="formData.contactPhone"
						type="tel"
						id="phone"
						class="w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
						required
						placeholder="07xxxxxxxxx"
					/>
				</div>
			</div>

			<!-- Subcategories Field -->
			<div class="mb-6">
				<label class="block text-sm font-medium text-gray-700 mb-2">
					التصنيفات الفرعية <span class="text-red-500">*</span>
					<span class="text-sm font-normal text-gray-500 mr-2"
						>(يجب اختيار تصنيف واحد على الأقل)</span
					>
				</label>
				<div v-if="loading" class="flex justify-center py-4">
					<div
						class="animate-spin rounded-full h-6 w-6 border-t-2 border-b-2 border-blue-500"
					></div>
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
						v-if="formData.subCategoryIds.length > 0"
						class="mb-4 p-3 bg-blue-50 border border-blue-200 rounded-md"
					>
						<div class="flex items-center justify-between mb-2">
							<span class="font-medium text-blue-700"
								>التصنيفات المختارة ({{ formData.subCategoryIds.length }})</span
							>
							<button
								type="button"
								@click="formData.subCategoryIds = []"
								class="text-xs text-red-600 hover:text-red-800"
							>
								إلغاء الكل
							</button>
						</div>
						<div class="flex flex-wrap gap-2">
							<span
								v-for="subCatId in formData.subCategoryIds"
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
											openCategories.includes(category.id) ? 'rotate-90' : ''
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
										{{ getSubCategoriesForCategory(category).length }}
									</span>
									<button
										type="button"
										@click.stop="toggleSelectAll(category)"
										class="text-xs px-2 py-1 rounded text-blue-600 hover:bg-blue-50"
										:disabled="
											getSubCategoriesForCategory(category).length === 0
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
									v-if="getSubCategoriesForCategory(category).length === 0"
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
											v-model="formData.subCategoryIds"
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

				<p
					v-if="formData.subCategoryIds.length === 0 && isFormSubmitted"
					class="text-red-500 text-sm mt-2"
				>
					يرجى اختيار تصنيف فرعي واحد على الأقل
				</p>
			</div>

			<!-- Submit Button -->
			<div class="flex justify-end space-x-3">
				<button
					type="submit"
					class="px-6 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500"
					:disabled="isSubmitting"
				>
					<span v-if="isSubmitting" class="flex items-center">
						<span
							class="animate-spin h-4 w-4 mr-2 border-t-2 border-white rounded-full"
						></span>
						جاري الإنشاء...
					</span>
					<span v-else>إنشاء الخدمة</span>
				</button>
			</div>
		</form>
	</div>
</template>

<script>
	import { ref, onMounted, computed } from "vue";
	import { useUserStore } from "@/store/userStore";
	import { useToastStore } from "@/store/toastStore";
	import CategoryService from "@/services/CategoryService.js";
	import ServiceService from "@/services/ServiceService.js";

	export default {
		name: "ServiceCreationForm",
		emits: ["service-created"],
		setup(props, { emit }) {
			const userStore = useUserStore();
			const toastStore = useToastStore();
			const loading = ref(true);
			const isSubmitting = ref(false);
			const isFormSubmitted = ref(false);
			const categories = ref([]);
			const categorySearch = ref("");
			const openCategories = ref([]);

			const formData = ref({
				title: "",
				description: "",
				estimatedPrice: 0,
				contactPhone: "",
				subCategoryIds: [],
			});

			// Calculate total subcategories across all categories
			const totalSubcategoriesCount = computed(() => {
				return categories.value.reduce((total, cat) => {
					return total + getSubCategoriesForCategory(cat).length;
				}, 0);
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
					const subCategories = getSubCategoriesForCategory(category);
					return subCategories.some(
						(subCat) =>
							subCat &&
							subCat.name &&
							subCat.name.toLowerCase().includes(searchTerm)
					);
				});
			});

			const getSubCategoriesForCategory = (category) => {
				if (!category) return [];
				return category.subCategories || [];
			};

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
					const subCategories = getSubCategoriesForCategory(category);
					for (const subCategory of subCategories) {
						if (subCategory.id === subCategoryId) {
							return subCategory.name;
						}
					}
				}
				return "التصنيف الفرعي";
			};

			// Count selected subcategories in a category
			const selectedSubCategoriesCount = (category) => {
				if (!category) return 0;
				const subCategories = getSubCategoriesForCategory(category);
				return subCategories.filter(
					(subCat) =>
						subCat &&
						subCat.id &&
						formData.value.subCategoryIds.includes(subCat.id)
				).length;
			};

			// Check if all subcategories in a category are selected
			const isAllSelected = (category) => {
				if (!category) return false;
				const subCategories = getSubCategoriesForCategory(category);
				if (subCategories.length === 0) return false;
				return selectedSubCategoriesCount(category) === subCategories.length;
			};

			// Toggle select/deselect all subcategories in a category
			const toggleSelectAll = (category) => {
				if (!category) return;
				const subCategories = getSubCategoriesForCategory(category);
				if (subCategories.length === 0) return;

				if (isAllSelected(category)) {
					// Deselect all subcategories in this category
					formData.value.subCategoryIds = formData.value.subCategoryIds.filter(
						(id) => !subCategories.some((subCat) => subCat.id === id)
					);
				} else {
					// Select all subcategories in this category
					const subCategoryIds = subCategories.map((subCat) => subCat.id);
					const uniqueIds = new Set([
						...formData.value.subCategoryIds,
						...subCategoryIds,
					]);
					formData.value.subCategoryIds = Array.from(uniqueIds);
				}
			};

			// Remove a specific subcategory from selection
			const removeSubCategory = (subCategoryId) => {
				const index = formData.value.subCategoryIds.indexOf(subCategoryId);
				if (index !== -1) {
					formData.value.subCategoryIds.splice(index, 1);
				}
			};

			// Filter subcategories based on search term
			const filteredSubCategories = (category) => {
				if (!category) return [];
				const subCategories = getSubCategoriesForCategory(category);
				// Filter out any invalid subcategories first
				const validSubCategories = subCategories.filter(
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
				toastStore.info("جاري إعادة تحميل التصنيفات...");
				// Clear existing categories first
				categories.value = [];
				// Then reload
				await loadCategoriesAndSubCategories();
			};

			const loadCategoriesAndSubCategories = async () => {
				loading.value = true;
				try {
					const [categoriesData, subCategoriesData] = await Promise.all([
						CategoryService.getCategories(userStore.token),
						CategoryService.getSubCategories(userStore.token),
					]);

					// Process categories to include subcategories
					categories.value = categoriesData.map((category) => {
						const categorySubcategories = subCategoriesData.filter(
							(subCategory) => subCategory.categoryId === category.id
						);
						return {
							...category,
							subCategories: categorySubcategories,
						};
					});

					// Open all categories by default for better UX
					openCategories.value = categories.value.map((cat) => cat.id);
				} catch (error) {
					console.error("Error loading categories and subcategories:", error);
					toastStore.error("فشل في تحميل التصنيفات. يرجى المحاولة مرة أخرى.");
				} finally {
					loading.value = false;
				}
			};

			const resetForm = () => {
				formData.value = {
					title: "",
					description: "",
					estimatedPrice: 0,
					contactPhone: "",
					subCategoryIds: [],
				};
				isFormSubmitted.value = false;
				categorySearch.value = "";
			};

			const submitForm = async () => {
				isFormSubmitted.value = true;

				// Validate that at least one subcategory is selected
				if (formData.value.subCategoryIds.length === 0) {
					return;
				}

				isSubmitting.value = true;
				try {
					const result = await ServiceService.createService(
						formData.value,
						userStore.token
					);
					toastStore.success(
						"تم إنشاء الخدمة بنجاح! ستكون متاحة بعد الموافقة عليها."
					);
					resetForm();
					emit("service-created", result);
				} catch (error) {
					console.error("Error creating service:", error);
					toastStore.error(
						error.message || "فشل في إنشاء الخدمة. يرجى المحاولة مرة أخرى."
					);
				} finally {
					isSubmitting.value = false;
				}
			};

			onMounted(() => {
				loadCategoriesAndSubCategories();
			});

			return {
				formData,
				categories,
				loading,
				isSubmitting,
				isFormSubmitted,
				categorySearch,
				openCategories,
				filteredCategories,
				totalSubcategoriesCount,
				getSubCategoriesForCategory,
				getSubCategoryName,
				selectedSubCategoriesCount,
				isAllSelected,
				toggleCategory,
				toggleSelectAll,
				removeSubCategory,
				filteredSubCategories,
				reloadCategories,
				submitForm,
			};
		},
	};
</script>

<style scoped>
	.rtl {
		direction: rtl;
		text-align: right;
	}
</style>
