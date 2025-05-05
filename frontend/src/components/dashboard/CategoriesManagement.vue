<template>
	<div class="space-y-6">
		<h1 class="text-3xl font-bold text-gray-800">إدارة التصنيفات</h1>
		<div class="grid grid-cols-1 md:grid-cols-2 gap-6">
			<!-- Main Categories -->
			<div class="bg-white rounded-lg shadow-md overflow-hidden">
				<div class="bg-blue-600 text-white p-4">
					<h4 class="font-bold text-xl">التصنيفات الرئيسية</h4>
				</div>
				<div class="p-4">
					<div class="flex justify-end mb-4">
						<button
							@click="$emit('add-main-category')"
							class="flex items-center gap-1 px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700 transition-colors text-sm"
						>
							<svg
								xmlns="http://www.w3.org/2000/svg"
								class="h-4 w-4"
								fill="none"
								viewBox="0 0 24 24"
								stroke="currentColor"
							>
								<path
									stroke-linecap="round"
									stroke-linejoin="round"
									stroke-width="2"
									d="M12 4v16m8-8H4"
								/>
							</svg>
							إضافة تصنيف جديد
						</button>
					</div>

					<div v-if="loading" class="text-center py-8">
						<div
							class="inline-block w-12 h-12 border-4 border-blue-600 border-t-transparent rounded-full animate-spin"
						></div>
						<p class="mt-4 text-gray-600">جاري تحميل البيانات...</p>
					</div>

					<div
						v-else-if="mainCategories.length === 0"
						class="text-center py-8 text-gray-500"
					>
						لا توجد تصنيفات رئيسية حتى الآن
					</div>

					<div v-else class="space-y-2">
						<div
							v-for="category in mainCategories"
							:key="category.id || category.Id"
							class="p-3 border rounded hover:bg-gray-50 flex justify-between items-center"
						>
							<span class="font-medium">{{
								category.Name || category.name
							}}</span>
							<div class="flex gap-2">
								<button
									@click="$emit('edit-main-category', category)"
									class="p-1 text-blue-600 hover:text-blue-800 transition-colors"
									title="تحرير"
								>
									<svg
										xmlns="http://www.w3.org/2000/svg"
										class="h-5 w-5"
										fill="none"
										viewBox="0 0 24 24"
										stroke="currentColor"
									>
										<path
											stroke-linecap="round"
											stroke-linejoin="round"
											stroke-width="2"
											d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z"
										/>
									</svg>
								</button>
								<button
									@click="$emit('delete-main-category', category)"
									class="p-1 text-red-600 hover:text-red-800 transition-colors"
									title="حذف"
								>
									<svg
										xmlns="http://www.w3.org/2000/svg"
										class="h-5 w-5"
										fill="none"
										viewBox="0 0 24 24"
										stroke="currentColor"
									>
										<path
											stroke-linecap="round"
											stroke-linejoin="round"
											stroke-width="2"
											d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16"
										/>
									</svg>
								</button>
							</div>
						</div>
					</div>
				</div>
			</div>

			<!-- Sub Categories -->
			<div class="bg-white rounded-lg shadow-md overflow-hidden">
				<div class="bg-blue-600 text-white p-4">
					<h4 class="font-bold text-xl">التصنيفات الفرعية</h4>
				</div>
				<div class="p-4">
					<div class="flex justify-end mb-4">
						<button
							@click="$emit('add-sub-category')"
							class="flex items-center gap-1 px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700 transition-colors text-sm"
						>
							<svg
								xmlns="http://www.w3.org/2000/svg"
								class="h-4 w-4"
								fill="none"
								viewBox="0 0 24 24"
								stroke="currentColor"
							>
								<path
									stroke-linecap="round"
									stroke-linejoin="round"
									stroke-width="2"
									d="M12 4v16m8-8H4"
								/>
							</svg>
							إضافة تصنيف فرعي
						</button>
					</div>

					<div v-if="loading" class="text-center py-8">
						<div
							class="inline-block w-12 h-12 border-4 border-blue-600 border-t-transparent rounded-full animate-spin"
						></div>
						<p class="mt-4 text-gray-600">جاري تحميل البيانات...</p>
					</div>

					<div
						v-else-if="subCategories.length === 0"
						class="text-center py-8 text-gray-500"
					>
						لا توجد تصنيفات فرعية حتى الآن
					</div>

					<div v-else class="space-y-2">
						<div
							v-for="subCategory in subCategories"
							:key="subCategory.id || subCategory.Id"
							class="p-3 border rounded hover:bg-gray-50 flex justify-between items-center"
						>
							<div>
								<span class="font-medium">{{
									subCategory.Name || subCategory.name
								}}</span>
								<div class="text-sm text-gray-500">
									{{
										subCategory.categoryName ||
										subCategory.CategoryName ||
										getMainCategoryName(
											subCategory.CategoryId ||
												subCategory.categoryId ||
												subCategory.mainCategoryId
										)
									}}
								</div>
							</div>
							<div class="flex gap-2">
								<button
									@click="$emit('edit-sub-category', subCategory)"
									class="p-1 text-blue-600 hover:text-blue-800 transition-colors"
									title="تحرير"
								>
									<svg
										xmlns="http://www.w3.org/2000/svg"
										class="h-5 w-5"
										fill="none"
										viewBox="0 0 24 24"
										stroke="currentColor"
									>
										<path
											stroke-linecap="round"
											stroke-linejoin="round"
											stroke-width="2"
											d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z"
										/>
									</svg>
								</button>
								<button
									@click="$emit('delete-sub-category', subCategory)"
									class="p-1 text-red-600 hover:text-red-800 transition-colors"
									title="حذف"
								>
									<svg
										xmlns="http://www.w3.org/2000/svg"
										class="h-5 w-5"
										fill="none"
										viewBox="0 0 24 24"
										stroke="currentColor"
									>
										<path
											stroke-linecap="round"
											stroke-linejoin="round"
											stroke-width="2"
											d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16"
										/>
									</svg>
								</button>
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>
	</div>
</template>

<script>
	export default {
		name: "CategoriesManagement",
		props: {
			mainCategories: {
				type: Array,
				default: () => [],
			},
			subCategories: {
				type: Array,
				default: () => [],
			},
			loading: {
				type: Boolean,
				default: false,
			},
			categoryNameMap: {
				type: Object,
				default: () => ({}),
			},
		},
		emits: [
			"add-main-category",
			"edit-main-category",
			"delete-main-category",
			"add-sub-category",
			"edit-sub-category",
			"delete-sub-category",
		],
		methods: {
			getMainCategoryName(categoryId) {
				if (categoryId === undefined || categoryId === null) {
					console.warn("Received undefined or null categoryId");
					return "تصنيف غير معروف";
				}

				// First try the direct mapping which should be most reliable
				const idStr = String(categoryId);
				if (this.categoryNameMap[idStr]) {
					console.log(
						`Found category name in map for ID ${idStr}: ${this.categoryNameMap[idStr]}`
					);
					return this.categoryNameMap[idStr];
				}

				// If the direct mapping fails, try the original method
				// Convert to string for comparison to avoid type issues
				console.log(`Looking for category with ID: ${idStr} using find method`);

				// Try to find by id (lowercase) or Id (uppercase)
				const category = this.mainCategories.find((c) => {
					const cId =
						c.id !== undefined
							? String(c.id)
							: c.Id !== undefined
							? String(c.Id)
							: null;

					// Log the comparison for debugging
					if (cId !== null) {
						console.log(
							`Comparing category ID ${cId} with subcategory's CategoryId ${idStr}: ${
								cId === idStr
							}`
						);
					}

					return cId === idStr;
				});

				if (category) {
					// Cache this result in the map for future use
					this.categoryNameMap[idStr] = category.Name || category.name;
					return category.Name || category.name || "تصنيف غير معروف";
				}

				console.warn(`No category found with ID: ${idStr}`);
				return "تصنيف غير معروف";
			},
		},
	};
</script>
