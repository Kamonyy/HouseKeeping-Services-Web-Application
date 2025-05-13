<template>
	<div class="bg-white rounded-lg shadow-md mb-6 overflow-hidden">
		<div v-if="!hideHeader" class="bg-blue-600 text-white p-4">
			<h4 class="font-bold text-xl">
				{{ isEditing ? "تعديل الخدمة" : "إضافة خدمة جديدة" }}
			</h4>
		</div>
		<div class="p-4 md:p-6">
			<form @submit.prevent="submitForm" class="rtl space-y-6">
				<div class="mb-4">
					<label for="title" class="block text-gray-700 mb-2 font-bold">
						عنوان الخدمة <span class="text-red-500">*</span>
					</label>
					<input
						type="text"
						class="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 transition-all duration-200"
						id="title"
						v-model="serviceForm.title"
						placeholder="أدخل عنوان الخدمة"
						required
					/>
				</div>

				<div class="mb-4">
					<label for="description" class="block text-gray-700 mb-2 font-bold">
						وصف الخدمة <span class="text-red-500">*</span>
					</label>
					<textarea
						class="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 transition-all duration-200"
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
							class="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 transition-all duration-200"
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
							class="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 transition-all duration-200"
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
									class="w-full px-3 py-2 pr-10 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 transition-all duration-200"
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
							class="mb-4 p-3 bg-blue-50 border border-blue-200 rounded-md shadow-sm"
						>
							<div class="flex items-center justify-between mb-2">
								<span class="font-medium text-blue-700"
									>التصنيفات المختارة ({{
										serviceForm.subCategoryIds.length
									}})</span
								>
								<div class="flex gap-2">
									<button
										v-if="activeCategoryId !== null"
										type="button"
										@click="clearActiveCategory"
										class="text-xs text-blue-600 hover:text-blue-800 transition-colors"
									>
										تغيير التصنيف
									</button>
									<button
										type="button"
										@click="clearAllSubCategories"
										class="text-xs text-red-600 hover:text-red-800 transition-colors"
									>
										إلغاء الكل
									</button>
								</div>
							</div>
							<div class="flex flex-wrap gap-2">
								<span
									v-for="subCatId in serviceForm.subCategoryIds"
									:key="subCatId"
									class="inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium bg-blue-100 text-blue-800 border border-blue-200 transition-all duration-200"
								>
									{{ getSubCategoryName(subCatId) }}
									<button
										@click="removeSubCategory(subCatId)"
										class="mr-1 text-blue-500 hover:text-blue-700 transition-colors"
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
						<div
							v-else
							class="border border-gray-200 rounded-lg divide-y shadow-sm hover:shadow-md transition-shadow duration-300"
						>
							<div
								v-for="category in filteredCategories"
								:key="category.id"
								class="bg-white"
								:class="{
									'opacity-50': isCategoryDisabled(category.id),
								}"
							>
								<div
									@click="handleCategoryClick(category)"
									class="flex justify-between items-center p-4 cursor-pointer hover:bg-gray-50 transition-all duration-200"
									:class="{
										'bg-blue-50 border-r-4 border-blue-500':
											openCategories.includes(category.id) &&
											activeCategoryId === category.id,
										'bg-gray-50':
											openCategories.includes(category.id) &&
											activeCategoryId !== category.id,
										'cursor-not-allowed': isCategoryDisabled(category.id),
									}"
								>
									<div class="flex items-center">
										<svg
											xmlns="http://www.w3.org/2000/svg"
											class="h-5 w-5 transform transition-transform duration-200"
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
										<span
											v-if="activeCategoryId === category.id"
											class="mr-2 px-2 py-0.5 bg-blue-100 text-blue-800 text-xs rounded-full"
										>
											نشط
										</span>
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
												isCategoryDisabled(category.id) ||
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
											class="flex items-center p-2 rounded-md hover:bg-gray-100 transition-all duration-150"
											:class="{
												'opacity-50 cursor-not-allowed': isCategoryDisabled(
													category.id
												),
												'bg-blue-50 border border-blue-200':
													serviceForm.subCategoryIds.includes(subCategory.id),
											}"
										>
											<input
												class="w-4 h-4 ml-2 text-blue-600 focus:ring-blue-500"
												type="checkbox"
												:id="`subcat-${subCategory.id}`"
												:value="subCategory.id"
												v-model="serviceForm.subCategoryIds"
												:disabled="isCategoryDisabled(category.id)"
												@change="
													handleSubCategoryChange(category.id, subCategory.id)
												"
											/>
											<label
												class="text-gray-700 cursor-pointer flex-grow"
												:for="`subcat-${subCategory.id}`"
												:class="{
													'cursor-not-allowed': isCategoryDisabled(category.id),
													'font-medium text-blue-700':
														serviceForm.subCategoryIds.includes(subCategory.id),
												}"
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

				<div class="flex gap-2 justify-end mt-6">
					<button
						type="button"
						class="px-5 py-2.5 bg-gray-200 text-gray-700 rounded-md hover:bg-gray-300 focus:outline-none focus:ring-2 focus:ring-gray-500 focus:ring-offset-2 transition-all duration-200"
						@click="cancelForm"
					>
						إلغاء
					</button>
					<button
						type="submit"
						class="px-5 py-2.5 bg-blue-600 text-white rounded-md hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-2 transition-all duration-200 flex items-center gap-1"
					>
						<svg
							v-if="isEditing"
							xmlns="http://www.w3.org/2000/svg"
							class="h-5 w-5"
							viewBox="0 0 20 20"
							fill="currentColor"
						>
							<path
								d="M13.586 3.586a2 2 0 112.828 2.828l-.793.793-2.828-2.828.793-.793zM11.379 5.793L3 14.172V17h2.828l8.38-8.379-2.83-2.828z"
							/>
						</svg>
						<svg
							v-else
							xmlns="http://www.w3.org/2000/svg"
							class="h-5 w-5"
							viewBox="0 0 20 20"
							fill="currentColor"
						>
							<path
								fill-rule="evenodd"
								d="M10 3a1 1 0 011 1v5h5a1 1 0 110 2h-5v5a1 1 0 11-2 0v-5H4a1 1 0 110-2h5V4a1 1 0 011-1z"
								clip-rule="evenodd"
							/>
						</svg>
						{{ isEditing ? "تحديث الخدمة" : "إضافة الخدمة" }}
					</button>
				</div>
			</form>
		</div>
	</div>
</template>

<script>
	import { ref, computed, watch, nextTick } from "vue";
	import axios from "axios";
	import { useUserStore } from "@/store/userStore";
	import { extractArrayFromResponse } from "@/utils/apiUtils";

	export default {
		name: "ServiceForm",
		props: {
			isEditing: {
				type: Boolean,
				default: false,
			},
			initialData: {
				type: Object,
				default: () => ({
					title: "",
					description: "",
					estimatedPrice: 0,
					contactPhone: "",
					subCategoryIds: [],
				}),
			},
			categories: {
				type: Array,
				default: () => [],
			},
			hideHeader: {
				type: Boolean,
				default: false,
			},
		},
		emits: ["submit", "cancel", "reload-categories"],
		setup(props, { emit }) {
			const userStore = useUserStore();
			const serviceForm = ref({ ...props.initialData });
			const categorySearch = ref("");
			const openCategories = ref([]);
			const activeCategoryId = ref(null);

			// Watch for changes in initialData to update the form
			watch(
				() => props.initialData,
				(newVal) => {
					// Convert subCategoryIds to numbers for consistency
					const formData = { ...newVal };
					if (formData.subCategoryIds) {
						formData.subCategoryIds = formData.subCategoryIds.map((id) =>
							Number(id)
						);
					}
					serviceForm.value = formData;
					console.log("Form updated with data:", serviceForm.value);
					// Check if we need to set an active category based on selected subcategories
					detectActiveCategory();
				},
				{ deep: true }
			);

			// Watch for changes in categories to ensure we have the latest data
			watch(
				() => props.categories,
				(newCategories) => {
					console.log("Categories updated in ServiceForm:", newCategories);

					// Validate if categories contain subcategories
					const totalSubcategories = newCategories.reduce((count, cat) => {
						if (!cat || !cat.subCategories) return count;
						const validSubcats = cat.subCategories.filter(
							(subCat) => subCat && subCat.id && (subCat.name || subCat.Name)
						);
						return count + validSubcats.length;
					}, 0);

					console.log(
						`ServiceForm received ${newCategories.length} categories with ${totalSubcategories} total subcategories`
					);

					// Expand categories if we're in edit mode
					if (props.isEditing && newCategories.length > 0) {
						console.log(
							"In edit mode, expanding categories that contain selected subcategories"
						);
						nextTick(() => {
							expandSelectedCategories();
						});
					}

					// Check if we need to set an active category based on selected subcategories
					detectActiveCategory();

					// Open all categories by default for better UX if there aren't many
					if (newCategories.length <= 5 && openCategories.value.length === 0) {
						console.log("Auto-opening all categories for better UX");
						openCategories.value = newCategories.map((c) => c.id);
					}
				},
				{ deep: true }
			);

			// Watch for changes in isEditing to expand categories that contain selected subcategories
			watch(
				() => props.isEditing,
				(newVal) => {
					if (newVal && props.categories.length > 0) {
						expandSelectedCategories();
						// Check if we need to set an active category based on selected subcategories
						detectActiveCategory();
					}
				}
			);

			// Detect which category should be active based on selected subcategories
			const detectActiveCategory = () => {
				if (serviceForm.value.subCategoryIds.length === 0) {
					activeCategoryId.value = null;
					return;
				}

				// Find which category contains the selected subcategories
				const categoryMap = new Map();

				props.categories.forEach((category) => {
					if (category.subCategories && category.subCategories.length > 0) {
						category.subCategories.forEach((subCat) => {
							if (subCat && subCat.id) {
								categoryMap.set(Number(subCat.id), Number(category.id));
							}
						});
					}
				});

				// Check if all selected subcategories belong to the same category
				let commonCategoryId = null;
				let allSameCategory = true;

				for (const subCatId of serviceForm.value.subCategoryIds) {
					const numSubCatId = Number(subCatId);
					const categoryId = categoryMap.get(numSubCatId);

					if (!commonCategoryId) {
						commonCategoryId = categoryId;
					} else if (categoryId !== commonCategoryId) {
						allSameCategory = false;
						break;
					}
				}

				activeCategoryId.value = allSameCategory ? commonCategoryId : null;
			};

			// Get the name of the active category
			const getActiveCategoryName = () => {
				if (activeCategoryId.value === null) return "";
				const category = props.categories.find(
					(cat) => Number(cat.id) === Number(activeCategoryId.value)
				);
				return category ? category.name : "";
			};

			// Clear the active category and all selected subcategories
			const clearActiveCategory = () => {
				activeCategoryId.value = null;
				serviceForm.value.subCategoryIds = [];
			};

			// Clear all selected subcategories
			const clearAllSubCategories = () => {
				serviceForm.value.subCategoryIds = [];
				activeCategoryId.value = null;
			};

			// Check if a category should be disabled
			const isCategoryDisabled = (categoryId) => {
				return (
					activeCategoryId.value !== null &&
					Number(activeCategoryId.value) !== Number(categoryId)
				);
			};

			// Handle subcategory selection change
			const handleSubCategoryChange = (categoryId, subCategoryId) => {
				// Convert IDs to numbers for consistent comparison
				const numCategoryId = Number(categoryId);
				const numSubCategoryId = Number(subCategoryId);

				// If this is the first subcategory being selected, set the active category
				if (
					serviceForm.value.subCategoryIds.length === 1 &&
					serviceForm.value.subCategoryIds.includes(numSubCategoryId)
				) {
					activeCategoryId.value = numCategoryId;
				}

				// If all subcategories from the active category are deselected, clear the active category
				if (activeCategoryId.value === numCategoryId) {
					const categorySubCatIds = getCategorySubCategoryIds(numCategoryId);
					const hasSelectedSubCats = serviceForm.value.subCategoryIds.some(
						(id) => categorySubCatIds.includes(Number(id))
					);

					if (!hasSelectedSubCats) {
						activeCategoryId.value = null;
					}
				}
			};

			// Get all subcategory IDs for a given category
			const getCategorySubCategoryIds = (categoryId) => {
				const numCategoryId = Number(categoryId);
				const category = props.categories.find(
					(cat) => Number(cat.id) === numCategoryId
				);
				if (!category || !category.subCategories) return [];
				return category.subCategories.map((subCat) => Number(subCat.id));
			};

			// Handle category click
			const handleCategoryClick = (category) => {
				// If category is disabled, don't allow toggling
				if (isCategoryDisabled(category.id)) {
					return;
				}

				toggleCategory(category.id);
			};

			// Expand categories that contain selected subcategories
			const expandSelectedCategories = () => {
				openCategories.value = [];
				const selectedSubCatIds = serviceForm.value.subCategoryIds.map((id) =>
					Number(id)
				);

				props.categories.forEach((category) => {
					if (
						category.subCategories &&
						category.subCategories.some(
							(subCat) =>
								subCat &&
								subCat.id &&
								selectedSubCatIds.includes(Number(subCat.id))
						)
					) {
						openCategories.value.push(category.id);
					}
				});
			};

			// Calculate total subcategories across all categories
			const totalSubcategoriesCount = computed(() => {
				console.log(
					"Calculating total subcategories count from categories:",
					props.categories
				);

				// Count subcategories from valid categories
				const count = props.categories.reduce((total, cat) => {
					if (!cat) return total;

					// Check if subcategories exist and are properly formed
					const validSubcategories =
						cat.subCategories && Array.isArray(cat.subCategories)
							? cat.subCategories.filter(
									(subCat) => subCat && subCat.id && subCat.name
							  )
							: [];

					return total + validSubcategories.length;
				}, 0);

				console.log(
					`Found ${count} total valid subcategories across ${props.categories.length} categories`
				);
				return count;
			});

			// Filter categories based on search term
			const filteredCategories = computed(() => {
				if (!props.categories || props.categories.length === 0) return [];
				if (!categorySearch.value.trim()) return props.categories;

				const searchTerm = categorySearch.value.toLowerCase();
				return props.categories.filter((category) => {
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
				const numSubCategoryId = Number(subCategoryId);
				console.log(
					`Looking for subcategory name with ID: ${numSubCategoryId}`
				);

				// First try to find it in the current categories
				for (const category of props.categories) {
					if (!category || !category.subCategories) continue;

					for (const subCategory of category.subCategories) {
						if (subCategory && Number(subCategory.id) === numSubCategoryId) {
							console.log(
								`Found subcategory ${subCategory.name} with ID ${numSubCategoryId}`
							);
							return subCategory.name;
						}
					}
				}

				// If not found, check if it's a string ID like "category-sub1"
				if (
					typeof subCategoryId === "string" &&
					subCategoryId.includes("-sub")
				) {
					const parts = subCategoryId.split("-sub");
					if (parts.length === 2) {
						const categoryId = parts[0];
						const subNumber = parts[1];
						const category = props.categories.find(
							(c) => String(c.id) === categoryId
						);
						if (category) {
							return `${category.name} - تصنيف فرعي ${subNumber}`;
						}
					}
				}

				console.log(
					`Could not find subcategory name for ID: ${numSubCategoryId}`
				);
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
						serviceForm.value.subCategoryIds.some(
							(id) => Number(id) === Number(subCat.id)
						)
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
				if (
					!category ||
					!category.subCategories ||
					isCategoryDisabled(category.id)
				) {
					return;
				}

				if (isAllSelected(category)) {
					// Deselect all subcategories in this category
					serviceForm.value.subCategoryIds =
						serviceForm.value.subCategoryIds.filter(
							(id) =>
								!category.subCategories.some(
									(subCat) => Number(subCat.id) === Number(id)
								)
						);

					// If this was the active category, clear it
					if (Number(activeCategoryId.value) === Number(category.id)) {
						activeCategoryId.value = null;
					}
				} else {
					// If selecting all in a category and we don't have an active category yet,
					// set this as the active category
					if (activeCategoryId.value === null) {
						activeCategoryId.value = Number(category.id);
					}

					// Select all subcategories in this category
					const subCategoryIds = category.subCategories.map((subCat) =>
						Number(subCat.id)
					);
					const uniqueIds = new Set([
						...serviceForm.value.subCategoryIds.map((id) => Number(id)),
						...subCategoryIds,
					]);
					serviceForm.value.subCategoryIds = Array.from(uniqueIds);
				}
			};

			// Remove a specific subcategory from selection
			const removeSubCategory = (subCategoryId) => {
				const numSubCategoryId = Number(subCategoryId);
				const index = serviceForm.value.subCategoryIds.findIndex(
					(id) => Number(id) === numSubCategoryId
				);

				if (index !== -1) {
					serviceForm.value.subCategoryIds.splice(index, 1);

					// Check if we need to clear the active category
					if (activeCategoryId.value !== null) {
						const categorySubCatIds = getCategorySubCategoryIds(
							activeCategoryId.value
						);
						const hasSelectedSubCats = serviceForm.value.subCategoryIds.some(
							(id) => categorySubCatIds.includes(Number(id))
						);

						if (!hasSelectedSubCats) {
							activeCategoryId.value = null;
						}
					}
				}
			};

			// Filter subcategories based on search term
			const filteredSubCategories = (category) => {
				if (!category) {
					console.log(
						"Attempted to filter subcategories for null/undefined category"
					);
					return [];
				}

				if (!category.subCategories) {
					console.log(
						`Category ${category.id} (${category.name}) has no subcategories property`
					);
					return [];
				}

				if (!Array.isArray(category.subCategories)) {
					console.log(
						`Category ${category.id} (${category.name}) subcategories is not an array:`,
						category.subCategories
					);
					// Try to convert to array if possible
					if (
						category.subCategories &&
						typeof category.subCategories === "object"
					) {
						// Check if it's an object with numeric keys (like an array-like object)
						const keys = Object.keys(category.subCategories);
						if (keys.length > 0 && keys.every((key) => !isNaN(parseInt(key)))) {
							console.log("Converting array-like object to array");
							const arrayVersion = [];
							keys.forEach((key) => {
								arrayVersion.push(category.subCategories[key]);
							});
							if (arrayVersion.length > 0) {
								console.log(
									`Successfully converted to array with ${arrayVersion.length} items`
								);
								return filterValidSubcategories(arrayVersion);
							}
						}
					}
					return [];
				}

				// Log the raw subcategory data to help debug
				console.log(
					`Raw subcategories for category ${category.name}:`,
					category.subCategories.length > 5
						? [
								...category.subCategories.slice(0, 5),
								`...and ${category.subCategories.length - 5} more`,
						  ]
						: category.subCategories
				);

				return filterValidSubcategories(category.subCategories);
			};

			// Helper to filter and validate subcategories
			const filterValidSubcategories = (subcategories) => {
				// Filter out any invalid subcategories first
				const validSubCategories = subcategories.filter((subCat) => {
					const isValid =
						subCat && subCat.id !== undefined && (subCat.name || subCat.Name);

					if (!isValid) {
						console.log("Found invalid subcategory:", subCat);
					}
					return isValid;
				});

				// Apply search filter if needed
				if (!categorySearch.value.trim()) {
					return validSubCategories;
				}

				const searchTerm = categorySearch.value.toLowerCase();
				return validSubCategories.filter((subCat) =>
					String(subCat.name || subCat.Name || "")
						.toLowerCase()
						.includes(searchTerm)
				);
			};

			const reloadCategories = () => {
				emit("reload-categories");
			};

			const submitForm = () => {
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

				// Validate that all subcategory IDs are valid
				const validSubcategoryIds = new Set();
				let hasInvalidIds = false;
				let missingSubcategories = [];

				// Build a set of all valid subcategory IDs from all categories
				props.categories.forEach((category) => {
					if (
						category?.subCategories &&
						Array.isArray(category.subCategories)
					) {
						category.subCategories.forEach((sub) => {
							if (sub && sub.id !== undefined) {
								validSubcategoryIds.add(String(sub.id));
							}
						});
					}
				});

				// Check if each selected subcategory ID is in the valid set
				serviceForm.value.subCategoryIds.forEach((id) => {
					if (!validSubcategoryIds.has(String(id))) {
						hasInvalidIds = true;
						missingSubcategories.push({
							id: id,
							name: getSubCategoryName(id),
						});
					}
				});

				// Warn about invalid subcategory IDs but allow submission
				if (hasInvalidIds) {
					console.warn(
						`Found ${missingSubcategories.length} potentially invalid subcategory IDs:`,
						missingSubcategories
					);

					const confirmSubmit = confirm(
						`تحذير: تم العثور على ${missingSubcategories.length} تصنيفات فرعية غير معروفة. هل ترغب في المتابعة على أي حال؟`
					);

					if (!confirmSubmit) {
						return;
					}
				}

				// Create payload matching backend CreateServiceDto structure
				const formData = {
					title: serviceForm.value.title,
					description: serviceForm.value.description,
					estimatedPrice: Number(serviceForm.value.estimatedPrice),
					contactPhone: serviceForm.value.contactPhone,
					// Ensure all IDs are numbers
					subCategoryIds: serviceForm.value.subCategoryIds.map((id) => {
						// Convert to number if possible, otherwise keep as is
						const numericId = Number(id);
						return isNaN(numericId) ? id : numericId;
					}),
				};

				console.log(
					"Submitting form data with subcategory IDs:",
					formData.subCategoryIds
				);
				emit("submit", formData);
			};

			const cancelForm = () => {
				emit("cancel");
			};

			return {
				serviceForm,
				categorySearch,
				openCategories,
				activeCategoryId,
				totalSubcategoriesCount,
				filteredCategories,
				toggleCategory,
				getSubCategoryName,
				selectedSubCategoriesCount,
				isAllSelected,
				toggleSelectAll,
				removeSubCategory,
				filteredSubCategories,
				reloadCategories,
				submitForm,
				cancelForm,
				isCategoryDisabled,
				handleSubCategoryChange,
				clearActiveCategory,
				getActiveCategoryName,
				clearAllSubCategories,
				handleCategoryClick,
				expandSelectedCategories,
			};
		},
	};
</script>

<style scoped>
	.rtl {
		direction: rtl;
		text-align: right;
	}

	input:focus,
	textarea:focus {
		border-color: #3b82f6;
		box-shadow: 0 0 0 2px rgba(59, 130, 246, 0.2);
	}

	/* Smooth transitions for all interactive elements */
	button,
	input,
	textarea,
	select,
	.rounded-md,
	.rounded-full {
		transition: all 0.2s ease;
	}

	/* Improve checkbox appearance */
	input[type="checkbox"] {
		cursor: pointer;
		border-radius: 3px;
	}

	/* Disable transitions for disabled elements */
	button:disabled,
	input:disabled,
	.cursor-not-allowed {
		transition: none;
	}
</style>
