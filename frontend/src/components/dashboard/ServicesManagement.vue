<template>
	<div class="space-y-6">
		<h1 class="text-3xl font-bold text-gray-800">إدارة الخدمات</h1>

		<!-- Filter Controls -->
		<div class="bg-white rounded-lg shadow-md mb-6 overflow-hidden">
			<div class="bg-blue-600 text-white p-4">
				<h4 class="font-bold text-xl">فلترة الخدمات</h4>
			</div>
			<div class="p-4">
				<div class="flex flex-wrap gap-6">
					<div class="flex items-center">
						<input
							class="w-5 h-5 ml-2 text-blue-600 focus:ring-blue-500"
							type="radio"
							v-model="statusFilter"
							id="all"
							value="all"
						/>
						<label class="text-gray-700 cursor-pointer" for="all"
							>جميع الخدمات</label
						>
					</div>
					<div class="flex items-center">
						<input
							class="w-5 h-5 ml-2 text-blue-600 focus:ring-blue-500"
							type="radio"
							v-model="statusFilter"
							id="pending"
							value="pending"
						/>
						<label class="text-gray-700 cursor-pointer" for="pending"
							>بانتظار الموافقة</label
						>
					</div>
					<div class="flex items-center">
						<input
							class="w-5 h-5 ml-2 text-blue-600 focus:ring-blue-500"
							type="radio"
							v-model="statusFilter"
							id="approved"
							value="approved"
						/>
						<label class="text-gray-700 cursor-pointer" for="approved"
							>تمت الموافقة</label
						>
					</div>
				</div>
			</div>
		</div>

		<!-- Services List -->
		<div class="bg-white rounded-lg shadow-md overflow-hidden">
			<div class="bg-blue-600 text-white p-4">
				<h4 class="font-bold text-xl">الخدمات</h4>
			</div>
			<div class="p-4">
				<div v-if="loading" class="text-center py-8">
					<div
						class="inline-block w-12 h-12 border-4 border-blue-600 border-t-transparent rounded-full animate-spin"
					></div>
					<p class="mt-4 text-gray-600">جاري تحميل البيانات...</p>
				</div>

				<div
					v-else-if="filteredServices.length === 0"
					class="text-center py-12"
				>
					<svg
						xmlns="http://www.w3.org/2000/svg"
						class="h-16 w-16 text-gray-400 mx-auto mb-4"
						fill="none"
						viewBox="0 0 24 24"
						stroke="currentColor"
					>
						<path
							stroke-linecap="round"
							stroke-linejoin="round"
							stroke-width="2"
							d="M9.172 16.172a4 4 0 015.656 0M9 10h.01M15 10h.01M12 2a10 10 0 110 20 10 10 0 010-20z"
						/>
					</svg>
					<p class="text-gray-500 text-lg">
						لم يتم العثور على خدمات تطابق الفلتر المحدد
					</p>
				</div>

				<div v-else>
					<div class="overflow-x-auto">
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
										الإجراءات
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
										<a
											href="#"
											@click.prevent="viewServiceDetails(service)"
											class="text-blue-600 hover:text-blue-800 hover:underline font-medium"
										>
											{{ service.title }}
										</a>
									</td>
									<td class="px-6 py-4 whitespace-nowrap">
										{{ service.providerUsername }}
									</td>
									<td class="px-6 py-4 whitespace-nowrap">
										{{ service.estimatedPrice.toLocaleString() }} د.ع
									</td>
									<td class="px-6 py-4 whitespace-nowrap">
										<span
											class="px-3 py-1 inline-flex text-xs leading-5 font-semibold rounded-full"
											:class="{
												'bg-green-100 text-green-800': service.isApproved,
												'bg-yellow-100 text-yellow-800': !service.isApproved,
											}"
										>
											{{ service.isApproved ? "معتمدة" : "بانتظار الموافقة" }}
										</span>
									</td>
									<td class="px-6 py-4 whitespace-nowrap">
										{{ formatDate(service.createdDate) }}
									</td>
									<td class="px-6 py-4 whitespace-nowrap">
										<div class="flex gap-2">
											<button
												v-if="!service.isApproved"
												class="flex items-center gap-1 px-3 py-1.5 bg-green-600 text-white text-sm font-medium rounded hover:bg-green-700 transition-colors"
												@click="approveService(service.id)"
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
														d="M5 13l4 4L19 7"
													/>
												</svg>
												موافقة
											</button>
											<button
												v-else
												class="flex items-center gap-1 px-3 py-1.5 bg-yellow-500 text-white text-sm font-medium rounded hover:bg-yellow-600 transition-colors"
												@click="revokeApproval(service.id)"
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
														d="M6 18L18 6M6 6l12 12"
													/>
												</svg>
												إلغاء
											</button>
										</div>
									</td>
								</tr>
							</tbody>
						</table>
					</div>
				</div>
			</div>
		</div>
	</div>
</template>

<script>
	export default {
		name: "ServicesManagement",
		props: {
			services: {
				type: Array,
				required: true,
			},
			loading: {
				type: Boolean,
				default: false,
			},
		},
		emits: ["view-service-details", "approve-service", "revoke-approval"],
		data() {
			return {
				statusFilter: "all",
			};
		},
		computed: {
			filteredServices() {
				if (this.statusFilter === "all") {
					return this.services;
				} else if (this.statusFilter === "pending") {
					return this.services.filter((service) => !service.isApproved);
				} else {
					return this.services.filter((service) => service.isApproved);
				}
			},
		},
		methods: {
			formatDate(dateString) {
				return new Date(dateString).toLocaleDateString("ar-IQ");
			},
			viewServiceDetails(service) {
				this.$emit("view-service-details", service);
			},
			approveService(serviceId) {
				this.$emit("approve-service", serviceId);
			},
			revokeApproval(serviceId) {
				this.$emit("revoke-approval", serviceId);
			},
		},
	};
</script>
