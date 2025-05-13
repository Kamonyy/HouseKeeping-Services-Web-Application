<template>
	<div class="container mx-auto px-4 py-8" dir="rtl">
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
				<h1 class="text-3xl font-bold text-gray-800">إدارة الخدمات</h1>
				<button
					@click="showAddServiceModal"
					class="px-4 py-2.5 bg-gradient-to-r from-blue-600 to-blue-700 text-white rounded-lg hover:from-blue-700 hover:to-blue-800 flex items-center gap-2 transition-all shadow-md hover:shadow-lg transform hover:scale-105 hover:translate-y-[-2px]"
				>
					<i class="fas fa-plus-circle"></i>
					<span>إضافة خدمة جديدة</span>
				</button>
			</div>

			<!-- Services List -->
			<div class="bg-white rounded-lg shadow-md overflow-hidden mb-8">
				<div class="bg-blue-600 text-white p-4">
					<h4 class="font-bold text-xl">الخدمات المقدمة</h4>
				</div>
				<div class="p-4">
					<!-- Server Error Message -->
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
									@click="fetchProviderServices"
									class="inline-flex items-center px-4 py-2 border border-transparent text-sm font-medium rounded-md text-white bg-red-600 hover:bg-red-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-red-500"
								>
									إعادة المحاولة
								</button>
							</div>
						</div>
					</div>

					<!-- Empty State -->
					<div
						v-else-if="providerServices.length === 0"
						class="text-center py-12"
					>
						<svg
							xmlns="http://www.w3.org/2000/svg"
							class="h-16 w-16 mx-auto text-gray-400 mb-4"
							fill="none"
							viewBox="0 0 24 24"
							stroke="currentColor"
						>
							<path
								stroke-linecap="round"
								stroke-linejoin="round"
								stroke-width="2"
								d="M19 11H5m14 0a2 2 0 012 2v6a2 2 0 01-2 2H5a2 2 0 01-2-2v-6a2 2 0 012-2m14 0V9a2 2 0 00-2-2M5 11V9a2 2 0 012-2m0 0V5a2 2 0 012-2h6a2 2 0 012 2v2M7 7h10"
							/>
						</svg>
						<p class="text-gray-500 text-lg mb-4">
							لم تقم بإضافة أي خدمات حتى الآن.
						</p>
						<button
							@click="showAddServiceModal"
							class="px-4 py-2.5 bg-gradient-to-r from-blue-600 to-blue-700 text-white rounded-lg hover:from-blue-700 hover:to-blue-800 flex items-center gap-2 transition-all shadow-md hover:shadow-lg transform hover:scale-105 hover:translate-y-[-2px] mx-auto"
						>
							<i class="fas fa-plus-circle"></i>
							<span>إضافة خدمة الآن</span>
						</button>
					</div>

					<!-- Services Table -->
					<div v-else class="overflow-x-auto">
						<!-- Search Filter -->
						<div class="mb-4">
							<div class="relative">
								<input
									type="text"
									v-model="searchQuery"
									placeholder="ابحث عن عنوان الخدمة..."
									class="w-full border border-gray-300 rounded-md p-2 pr-10 focus:ring-blue-500 focus:border-blue-500"
								/>
								<div
									class="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none"
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

						<table class="min-w-full divide-y divide-gray-200">
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
									v-for="service in filteredServices"
									:key="service.id"
									class="hover:bg-gray-50 transition-colors"
								>
									<td class="px-6 py-4 whitespace-nowrap">
										<div class="font-medium text-gray-900">
											{{ service.title }}
										</div>
									</td>
									<td class="px-6 py-4 whitespace-nowrap">
										<div class="text-gray-900">
											{{ service.estimatedPrice.toLocaleString() }} د.ع
										</div>
									</td>
									<td class="px-6 py-4 whitespace-nowrap">
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
									<td class="px-6 py-4 whitespace-nowrap">
										<div class="text-gray-500">
											{{ formatDate(service.createdDate) }}
										</div>
									</td>
									<td class="px-6 py-4 whitespace-nowrap">
										<div class="flex gap-2">
											<button
												class="px-3 py-1 bg-blue-600 text-white text-sm font-medium rounded hover:bg-blue-700 transition-colors"
												@click="viewServiceDetails(service)"
												title="عرض التفاصيل"
											>
												عرض
											</button>
											<button
												class="px-3 py-1 bg-green-600 text-white text-sm font-medium rounded hover:bg-green-700 transition-colors"
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
												class="px-3 py-1 bg-red-600 text-white text-sm font-medium rounded hover:bg-red-700 transition-colors"
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

			<!-- Service Details Modal -->
			<div
				v-if="selectedService"
				class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50 px-4 backdrop-blur-sm transition-all duration-300"
				dir="rtl"
			>
				<div
					class="bg-white rounded-lg shadow-xl w-full max-w-2xl mx-auto overflow-hidden animate-modal-appear"
				>
					<div
						class="bg-blue-600 text-white p-4 flex justify-between items-center"
					>
						<h3 class="text-xl font-semibold">تفاصيل الخدمة</h3>
						<button
							@click="closeDetailsModal"
							class="text-white hover:text-gray-200 focus:outline-none transition-colors duration-200"
						>
							<svg
								xmlns="http://www.w3.org/2000/svg"
								class="h-6 w-6"
								fill="none"
								viewBox="0 0 24 24"
								stroke="currentColor"
							>
								<path
									stroke-linecap="round"
									stroke-linejoin="round"
									stroke-width="2"
									d="M6 18L18 6M6 6l12 12"
								/>
							</svg>
						</button>
					</div>

					<div class="p-6 max-h-[70vh] overflow-y-auto">
						<div class="space-y-4">
							<div
								class="grid grid-cols-1 md:grid-cols-3 gap-2 border-b border-gray-200 pb-3 hover:bg-gray-50 p-2 rounded transition-colors duration-200"
							>
								<div class="font-semibold text-gray-700">العنوان:</div>
								<div class="md:col-span-2 text-gray-900">
									{{ selectedService.title }}
								</div>
							</div>
							<div
								class="grid grid-cols-1 md:grid-cols-3 gap-2 border-b border-gray-200 pb-3 hover:bg-gray-50 p-2 rounded transition-colors duration-200"
							>
								<div class="font-semibold text-gray-700">الوصف:</div>
								<div class="md:col-span-2 text-gray-900">
									{{ selectedService.description }}
								</div>
							</div>
							<div
								class="grid grid-cols-1 md:grid-cols-3 gap-2 border-b border-gray-200 pb-3 hover:bg-gray-50 p-2 rounded transition-colors duration-200"
							>
								<div class="font-semibold text-gray-700">الحالة:</div>
								<div class="md:col-span-2">
									<span
										class="px-2 py-1 inline-flex text-xs leading-5 font-semibold rounded-full"
										:class="{
											'bg-green-100 text-green-800': selectedService.isApproved,
											'bg-yellow-100 text-yellow-800':
												!selectedService.isApproved,
										}"
									>
										{{
											selectedService.isApproved ? "معتمدة" : "بانتظار الموافقة"
										}}
									</span>
								</div>
							</div>
							<div
								class="grid grid-cols-1 md:grid-cols-3 gap-2 border-b border-gray-200 pb-3 hover:bg-gray-50 p-2 rounded transition-colors duration-200"
							>
								<div class="font-semibold text-gray-700">رقم الهاتف:</div>
								<div class="md:col-span-2 text-gray-900">
									{{ selectedService.contactPhone }}
								</div>
							</div>
							<div
								class="grid grid-cols-1 md:grid-cols-3 gap-2 border-b border-gray-200 pb-3 hover:bg-gray-50 p-2 rounded transition-colors duration-200"
							>
								<div class="font-semibold text-gray-700">السعر التقريبي:</div>
								<div class="md:col-span-2 text-gray-900">
									{{ selectedService.estimatedPrice?.toLocaleString() }} د.ع
								</div>
							</div>
							<div
								class="grid grid-cols-1 md:grid-cols-3 gap-2 border-b border-gray-200 pb-3 hover:bg-gray-50 p-2 rounded transition-colors duration-200"
							>
								<div class="font-semibold text-gray-700">تاريخ الإنشاء:</div>
								<div class="md:col-span-2 text-gray-900">
									{{ formatDate(selectedService.createdDate) }}
								</div>
							</div>
							<div class="grid grid-cols-1 md:grid-cols-3 gap-2">
								<div class="font-semibold text-gray-700">
									التصنيفات الفرعية:
								</div>
								<div class="md:col-span-2">
									<div class="flex flex-wrap gap-2">
										<span
											v-for="subCategory in getSubCategories(selectedService)"
											:key="subCategory.id"
											class="px-3 py-1 bg-blue-100 text-blue-700 text-sm rounded-full"
										>
											{{ subCategory.name }}
										</span>
										<span
											v-if="getSubCategories(selectedService).length === 0"
											class="text-gray-500 text-sm"
										>
											لا توجد تصنيفات فرعية
										</span>
									</div>
								</div>
							</div>
						</div>
					</div>

					<div class="bg-gray-50 p-4 border-t border-gray-200 flex justify-end">
						<button
							@click="closeDetailsModal"
							class="px-5 py-2.5 bg-blue-600 text-white rounded-md hover:bg-blue-700 transition-all duration-200 flex items-center gap-2"
						>
							<svg
								xmlns="http://www.w3.org/2000/svg"
								class="h-5 w-5"
								viewBox="0 0 20 20"
								fill="currentColor"
							>
								<path
									fill-rule="evenodd"
									d="M4.293 4.293a1 1 0 011.414 0L10 8.586l4.293-4.293a1 1 0 111.414 1.414L11.414 10l4.293 4.293a1 1 0 01-1.414 1.414L10 11.414l-4.293 4.293a1 1 0 01-1.414-1.414L8.586 10 4.293 5.707a1 1 0 010-1.414z"
									clip-rule="evenodd"
								/>
							</svg>
							إغلاق
						</button>
					</div>
				</div>
			</div>

			<!-- Add/Edit Service Modal -->
			<div
				v-if="showServiceForm"
				class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50 px-4 backdrop-blur-sm transition-all duration-300"
				dir="rtl"
			>
				<div
					class="bg-white rounded-xl shadow-xl w-full max-w-4xl mx-auto overflow-hidden animate-modal-appear"
					style="margin-top: 3vh"
				>
					<div
						class="bg-gradient-to-r from-blue-600 to-blue-700 text-white p-4 flex justify-between items-center"
					>
						<h3 class="text-xl font-semibold flex items-center">
							<i class="fas fa-briefcase mr-2"></i>
							{{ isEditing ? "تعديل الخدمة" : "إضافة خدمة جديدة" }}
						</h3>
						<button
							@click="cancelForm"
							class="text-white hover:text-gray-200 focus:outline-none transition-colors duration-200 bg-white/20 rounded-full p-1.5 hover:bg-white/30"
						>
							<i class="fas fa-times"></i>
						</button>
					</div>

					<div class="p-6 max-h-[70vh] overflow-y-auto">
						<div
							class="bg-gradient-to-br from-white to-blue-50 rounded-xl shadow-md p-6 animate-fadeIn"
						>
							<div class="mb-6 flex justify-between items-center">
								<div class="flex items-center gap-3">
									<div class="bg-blue-100 p-3 rounded-full text-blue-600">
										<i class="fas fa-briefcase text-xl"></i>
									</div>
									<div>
										<h3 class="text-lg font-bold text-gray-800">
											{{
												isEditing
													? "تعديل معلومات الخدمة"
													: "معلومات الخدمة الجديدة"
											}}
										</h3>
										<p class="text-sm text-blue-600">
											{{
												isEditing
													? "قم بتعديل بيانات الخدمة"
													: "يرجى ملء جميع الحقول المطلوبة"
											}}
										</p>
									</div>
								</div>
								<div
									class="bg-blue-600 text-white px-4 py-1.5 rounded-lg text-sm font-medium shadow-sm"
								>
									{{ isEditing ? "تحديث البيانات" : "بيانات الخدمة الجديدة" }}
								</div>
							</div>
							<ServiceForm
								:is-editing="isEditing"
								:initial-data="serviceForm"
								:categories="categories"
								:hide-header="true"
								@submit="handleSubmit"
								@cancel="cancelForm"
								@reload-categories="reloadCategories"
							/>
						</div>
					</div>
				</div>
			</div>

			<!-- Delete Confirmation Modal -->
			<div
				v-if="serviceToDeleteId"
				class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50 px-4 backdrop-blur-sm transition-all duration-300"
				dir="rtl"
			>
				<div
					class="bg-white rounded-lg shadow-xl w-full max-w-md mx-auto overflow-hidden animate-modal-appear"
				>
					<div
						class="bg-red-600 text-white p-4 flex justify-between items-center"
					>
						<h3 class="text-xl font-semibold">تأكيد الحذف</h3>
						<button
							@click="serviceToDeleteId = null"
							class="text-white hover:text-gray-200 focus:outline-none transition-colors duration-200"
						>
							<svg
								xmlns="http://www.w3.org/2000/svg"
								class="h-6 w-6"
								fill="none"
								viewBox="0 0 24 24"
								stroke="currentColor"
							>
								<path
									stroke-linecap="round"
									stroke-linejoin="round"
									stroke-width="2"
									d="M6 18L18 6M6 6l12 12"
								/>
							</svg>
						</button>
					</div>

					<div class="p-6 max-h-[70vh] overflow-y-auto">
						<div class="space-y-4">
							<div class="flex items-center justify-center mb-4 text-red-600">
								<svg
									xmlns="http://www.w3.org/2000/svg"
									class="h-12 w-12"
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
							</div>
							<p class="text-center text-gray-700 mb-2">
								هل أنت متأكد من رغبتك في حذف هذه الخدمة؟
							</p>
							<p class="text-center text-gray-500 text-sm mb-4">
								لا يمكن التراجع عن هذا الإجراء.
							</p>
							<div class="font-bold text-center text-red-600 text-lg">
								{{ getServiceTitle(serviceToDeleteId) }}
							</div>
						</div>
					</div>

					<div
						class="bg-gray-50 p-4 border-t border-gray-200 flex justify-center space-x-reverse space-x-3"
					>
						<button
							@click="deleteService"
							class="px-5 py-2.5 bg-red-600 text-white rounded-md hover:bg-red-700 transition-all duration-200 flex items-center gap-2"
						>
							<svg
								xmlns="http://www.w3.org/2000/svg"
								class="h-5 w-5"
								viewBox="0 0 20 20"
								fill="currentColor"
							>
								<path
									fill-rule="evenodd"
									d="M9 2a1 1 0 00-.894.553L7.382 4H4a1 1 0 000 2v10a2 2 0 002 2h8a2 2 0 002-2V6a1 1 0 100-2h-3.382l-.724-1.447A1 1 0 0011 2H9zM7 8a1 1 0 012 0v6a1 1 0 11-2 0V8zm5-1a1 1 0 00-1 1v6a1 1 0 102 0V8a1 1 0 00-1-1z"
									clip-rule="evenodd"
								/>
							</svg>
							حذف
						</button>
						<button
							@click="serviceToDeleteId = null"
							class="px-5 py-2.5 bg-gray-200 text-gray-700 rounded-md hover:bg-gray-300 transition-all duration-200"
						>
							إلغاء
						</button>
					</div>
				</div>
			</div>
		</div>
	</div>
</template>

<script>
	import { useUserStore } from "@/store/userStore";
	import { ref, computed, onMounted, watch } from "vue";
	import { useRouter } from "vue-router";
	import { useToastStore } from "@/store/toastStore";
	import ServiceForm from "@/components/ServiceForm.vue";
	import axios from "axios";
	import { extractArrayFromResponse } from "@/utils/apiUtils";

	export default {
		name: "ProviderManagementView",
		components: {
			ServiceForm,
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
			const toastStore = useToastStore();
			const router = useRouter();
			const serverError = ref(false);
			const searchQuery = ref("");
			const selectedService = ref(null);

			const token = computed(() => userStore.token);
			const isProvider = computed(() => userStore.isProvider);

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
				await fetchProviderServices();
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

					// Variable to track if we loaded any subcategories
					let hasLoadedAnySubcategories = false;

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

								if (
									category.subCategories &&
									category.subCategories.length > 0
								) {
									hasLoadedAnySubcategories = true;
								}

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

					// If we haven't loaded any subcategories yet, try the global subcategory endpoint
					if (!hasLoadedAnySubcategories) {
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
												hasLoadedAnySubcategories = true;
											}
										}
									}
									if (hasLoadedAnySubcategories) {
										console.log("Assigned subcategories from global list");
									}
								}
							}
						} catch (error) {
							console.error("Error fetching all subcategories:", error);
						}
					}

					categories.value = processedCategories;
					console.log("Categories loaded:", categories.value.length);

					// Check if any categories have subcategories
					const totalSubcategories = categories.value.reduce(
						(total, cat) => total + (cat.subCategories?.length || 0),
						0
					);
					console.log(`Total subcategories loaded: ${totalSubcategories}`);

					// If no subcategories were loaded at all, create dummy ones for testing
					if (categories.value.length > 0 && totalSubcategories === 0) {
						console.log(
							"Creating fallback dummy subcategories for development"
						);
						categories.value.forEach((cat, index) => {
							cat.subCategories = [
								{
									id: `${cat.id}-sub1`,
									name: `${cat.name} - تصنيف فرعي 1`,
									categoryId: cat.id,
								},
								{
									id: `${cat.id}-sub2`,
									name: `${cat.name} - تصنيف فرعي 2`,
									categoryId: cat.id,
								},
							];
						});
						console.log("Added temporary test subcategories");
					}
				} catch (error) {
					console.error("Error loading categories:", error);
					toastStore.error("فشل تحميل التصنيفات. يرجى المحاولة مرة أخرى.");
					categories.value = [];
				}
				// Do not set loading to false here, that should be handled by fetchProviderServices
			};

			const fetchProviderServices = async () => {
				console.log("Fetching provider services...");
				loading.value = true;
				try {
					// Ensure we have the API_BASE_URL
					const API_BASE_URL = import.meta.env.VITE_API_URL || "";

					// Only proceed if we have a valid token
					if (!token.value) {
						console.error("No authentication token available");
						providerServices.value = [];
						return;
					}

					const response = await fetch(`${API_BASE_URL}/api/service/provider`, {
						headers: {
							Authorization: `Bearer ${token.value}`,
						},
					});

					console.log("Provider services response status:", response.status);

					if (!response.ok) {
						if (response.status === 404) {
							// Provider has no services yet, this is a normal case
							console.log("No services found for this provider");
							providerServices.value = [];
							return;
						}

						const errorText = await response.text();
						console.error("Error fetching provider services:", errorText);
						throw new Error(
							`Failed to fetch services: ${response.status} - ${errorText}`
						);
					}

					// Parse the response data
					const data = await response.json();
					console.log("Provider services data:", data);

					// Extract the array from the response
					providerServices.value = extractArrayFromResponse(data, "services");
					console.log("Services loaded:", providerServices.value.length);
				} catch (error) {
					console.error("Error fetching provider services:", error);
					providerServices.value = [];
					toastStore.error(
						"فشل تحميل الخدمات الخاصة بك. يرجى المحاولة مرة أخرى.",
						5000
					);
				} finally {
					loading.value = false;
				}
			};

			const editService = (service) => {
				isEditing.value = true;
				showServiceForm.value = true;
				currentServiceId.value = service.id;

				// Handle both formats - direct subCategories array or subCategories via ServiceSubCategory
				let subCategoryIds = [];
				if (service.subCategories && Array.isArray(service.subCategories)) {
					// Direct reference to subcategories (standard DTO format)
					subCategoryIds = service.subCategories
						.map((sc) => sc.id)
						.map((id) => Number(id));
				} else if (
					service.serviceSubCategory &&
					Array.isArray(service.serviceSubCategory)
				) {
					// Expanded reference via ServiceSubCategory (Entity format with reference handler)
					subCategoryIds = service.serviceSubCategory
						.filter((ssc) => ssc && ssc.subCategory)
						.map((ssc) => ssc.subCategory.id)
						.map((id) => Number(id));
				}

				console.log("Setting up form with subcategory IDs:", subCategoryIds);

				serviceForm.value = {
					title: service.title,
					description: service.description,
					estimatedPrice: Number(service.estimatedPrice),
					contactPhone: service.contactPhone,
					subCategoryIds: subCategoryIds,
				};
			};

			const cancelForm = () => {
				isEditing.value = false;
				showServiceForm.value = false;
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

			const handleSubmit = async (formData) => {
				try {
					console.log("Submitting service form:", formData);

					// Ensure we have the API_BASE_URL
					const API_BASE_URL = import.meta.env.VITE_API_URL || "";

					const serviceId = currentServiceId.value;
					const url = isEditing.value
						? `${API_BASE_URL}/api/service/${serviceId}`
						: `${API_BASE_URL}/api/service`;

					const isEditingOperation = isEditing.value;

					// Debug API endpoint
					console.log(
						`Making ${isEditingOperation ? "PUT" : "POST"} request to: ${url}`
					);

					// Create payload exactly matching backend CreateServiceDto structure
					const requestPayload = {
						title: formData.title,
						description: formData.description,
						estimatedPrice: Number(formData.estimatedPrice),
						contactPhone: formData.contactPhone,
						subCategoryIds: formData.subCategoryIds.map((id) => Number(id)),
					};

					console.log(
						"Sending payload matching backend DTO format:",
						requestPayload
					);

					let response;

					// Use fetch API for both operations
					response = await fetch(url, {
						method: isEditingOperation ? "PUT" : "POST",
						headers: {
							"Content-Type": "application/json",
							Authorization: `Bearer ${token.value}`,
						},
						body: JSON.stringify(requestPayload),
					});

					console.log("Fetch response status:", response.status);

					if (!response.ok) {
						let errorText = await response.text();
						console.error("Error response text:", errorText);
						throw new Error(`Server error: ${response.status} - ${errorText}`);
					}

					// Show success message
					toastStore.success(
						isEditingOperation
							? "تم تحديث الخدمة بنجاح"
							: "تمت إضافة الخدمة بنجاح",
						5000
					);

					// Reset form and hide it
					isEditing.value = false;
					showServiceForm.value = false;
					currentServiceId.value = null;
					resetForm();

					// Refresh services
					fetchProviderServices();
				} catch (error) {
					console.error("Error submitting service form:", error);
					toastStore.error(`حدث خطأ: ${error.message}`, 5000);
				}
			};

			const confirmDelete = (serviceId) => {
				serviceToDeleteId.value = serviceId;
			};

			const deleteService = async () => {
				try {
					// Ensure we have the API_BASE_URL
					const API_BASE_URL = import.meta.env.VITE_API_URL || "";

					const response = await fetch(
						`${API_BASE_URL}/api/service/${serviceToDeleteId.value}`,
						{
							method: "DELETE",
							headers: {
								Authorization: `Bearer ${token.value}`,
							},
						}
					);

					if (!response.ok) {
						const errorText = await response.text();
						throw new Error(`Failed to delete service: ${errorText}`);
					}

					// Close modal and refresh services
					serviceToDeleteId.value = null;
					await fetchProviderServices();

					// Success message
					toastStore.success("تم حذف الخدمة بنجاح!", 5000);
				} catch (error) {
					console.error("Error deleting service:", error);
					toastStore.error("فشل حذف الخدمة. يرجى المحاولة مرة أخرى.", 5000);
				}
			};

			const showAddServiceModal = () => {
				resetForm();
				isEditing.value = false;
				showServiceForm.value = true;
			};

			const viewServiceDetails = (service) => {
				selectedService.value = service;
			};

			const closeDetailsModal = () => {
				selectedService.value = null;
			};

			const getSubCategories = (service) => {
				if (service.subCategories) {
					return service.subCategories;
				} else if (service.serviceSubCategory) {
					return service.serviceSubCategory.map((ssc) => ssc.subCategory);
				}
				return [];
			};

			const getServiceTitle = (serviceId) => {
				const service = providerServices.value.find((s) => s.id === serviceId);
				return service ? service.title : "خدمة غير معروفة";
			};

			const formatDate = (dateStr) => {
				const date = new Date(dateStr);
				return date.toLocaleDateString("ar-IQ");
			};

			const filteredServices = computed(() => {
				if (!searchQuery.value) {
					return providerServices.value;
				}
				return providerServices.value.filter((service) =>
					service.title.toLowerCase().includes(searchQuery.value.toLowerCase())
				);
			});

			const reloadCategories = async () => {
				console.log("Manually reloading categories and subcategories...");
				// Clear existing categories first
				categories.value = [];
				// Then reload
				await loadCategories();
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
				showAddServiceModal,
				viewServiceDetails,
				closeDetailsModal,
				getSubCategories,
				getServiceTitle,
				formatDate,
				filteredServices,
				searchQuery,
				selectedService,
				reloadCategories,
				serverError,
				fetchProviderServices,
			};
		},
	};
</script>

<style scoped>
	.rtl {
		direction: rtl;
		text-align: right;
	}

	/* Animation for loading spinner */
	@keyframes spin {
		from {
			transform: rotate(0deg);
		}
		to {
			transform: rotate(360deg);
		}
	}

	.animate-spin {
		animation: spin 1s linear infinite;
	}

	/* Animation for fade in */
	@keyframes fadeIn {
		from {
			opacity: 0;
			transform: translateY(15px);
		}
		to {
			opacity: 1;
			transform: translateY(0);
		}
	}

	.animate-fadeIn {
		animation: fadeIn 0.4s cubic-bezier(0.34, 1.56, 0.64, 1);
	}

	/* Animation for modal appearance */
	@keyframes modal-appear {
		from {
			opacity: 0;
			transform: translateY(-20px);
		}
		to {
			opacity: 1;
			transform: translateY(0);
		}
	}

	.animate-modal-appear {
		animation: modal-appear 0.3s ease-out;
	}

	/* Input focus styles */
	input:focus,
	textarea:focus,
	select:focus {
		outline: none;
		border-color: #3b82f6;
		box-shadow: 0 0 0 2px rgba(59, 130, 246, 0.2);
	}

	/* Button hover transitions */
	button {
		transition: all 0.2s ease;
	}

	/* Table row hover effect */
	tr.hover\:bg-gray-50:hover {
		background-color: #f9fafb;
	}

	/* Status badge styling */
	.rounded-full {
		padding: 0.25rem 0.75rem;
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
