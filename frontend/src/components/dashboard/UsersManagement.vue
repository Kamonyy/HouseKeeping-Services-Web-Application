<template>
	<div class="space-y-6">
		<h1 class="text-3xl font-bold text-gray-800">إدارة المستخدمين</h1>

		<!-- Search and Filters -->
		<div class="bg-white rounded-lg shadow-md overflow-hidden">
			<div class="bg-blue-600 text-white p-4">
				<h4 class="font-bold text-xl">البحث والفلترة</h4>
			</div>
			<div class="p-4">
				<div class="flex flex-wrap gap-4">
					<div class="flex-1">
						<input
							type="text"
							v-model="searchQuery"
							placeholder="البحث عن مستخدم..."
							class="w-full px-4 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
							@input="onSearch"
						/>
					</div>
					<div class="flex gap-4">
						<select
							v-model="roleFilter"
							class="px-4 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
							@change="onFilterByRole"
						>
							<option value="all">جميع الأدوار</option>
							<option value="Admin">المدراء</option>
							<option value="User">المستخدمين</option>
							<option value="Provider">مقدمي الخدمات</option>
						</select>
						<button
							@click="$emit('reload-users')"
							class="px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500"
						>
							تحديث
						</button>
					</div>
				</div>
			</div>
		</div>

		<!-- Users List -->
		<div class="bg-white rounded-lg shadow-md overflow-hidden">
			<div class="bg-blue-600 text-white p-4 flex justify-between items-center">
				<h4 class="font-bold text-xl">قائمة المستخدمين</h4>
				<button
					class="px-3 py-1 bg-white text-blue-600 rounded flex items-center gap-1 hover:bg-gray-100 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500 text-sm"
					@click="$emit('add-user')"
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
					إضافة مستخدم
				</button>
			</div>
			<div class="p-4">
				<div v-if="loading" class="text-center py-8">
					<div
						class="inline-block w-12 h-12 border-4 border-blue-600 border-t-transparent rounded-full animate-spin"
					></div>
					<p class="mt-4 text-gray-600">جاري تحميل بيانات المستخدمين...</p>
				</div>

				<div v-else-if="filteredUsers.length === 0" class="text-center py-12">
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
					<p class="text-gray-500 text-lg">لم يتم العثور على مستخدمين</p>
				</div>

				<div v-else class="overflow-x-auto">
					<table class="min-w-full divide-y divide-gray-200">
						<thead class="bg-gray-50">
							<tr>
								<th
									class="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider"
								>
									اسم المستخدم
								</th>
								<th
									class="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider"
								>
									البريد الإلكتروني
								</th>
								<th
									class="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider"
								>
									الدور
								</th>
								<th
									class="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider"
								>
									الحالة
								</th>
								<th
									class="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider"
								>
									تاريخ التسجيل
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
								v-for="user in filteredUsers"
								:key="user.id"
								class="hover:bg-gray-50 transition-colors"
							>
								<td class="px-6 py-4 whitespace-nowrap">
									<div class="flex items-center gap-3">
										<div
											class="h-10 w-10 rounded-full bg-blue-100 flex items-center justify-center text-blue-600 font-bold"
										>
											{{ user.username.charAt(0).toUpperCase() }}
										</div>
										<span class="font-medium text-gray-900">{{
											user.username
										}}</span>
									</div>
								</td>
								<td class="px-6 py-4 whitespace-nowrap">
									{{ user.email }}
								</td>
								<td class="px-6 py-4 whitespace-nowrap">
									<span
										class="px-3 py-1 inline-flex text-xs leading-5 font-semibold rounded-full"
										:class="{
											'bg-purple-100 text-purple-800':
												user.roles.includes('Admin'),
											'bg-green-100 text-green-800':
												user.roles.includes('Provider'),
											'bg-blue-100 text-blue-800':
												user.roles.includes('User') &&
												!user.roles.includes('Admin') &&
												!user.roles.includes('Provider'),
										}"
									>
										{{ getUserRoleDisplay(user.roles) }}
									</span>
								</td>
								<td class="px-6 py-4 whitespace-nowrap">
									<span
										class="px-3 py-1 inline-flex text-xs leading-5 font-semibold rounded-full"
										:class="{
											'bg-green-100 text-green-800': user.isActive,
											'bg-red-100 text-red-800': !user.isActive,
										}"
									>
										{{ user.isActive ? "نشط" : "غير نشط" }}
									</span>
								</td>
								<td class="px-6 py-4 whitespace-nowrap">
									{{ formatDate(user.createdDate) }}
								</td>
								<td class="px-6 py-4 whitespace-nowrap">
									<div class="flex gap-2">
										<button
											@click="$emit('edit-user', user)"
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
											@click="$emit('toggle-user-status', user)"
											class="p-1 transition-colors"
											:class="
												user.isActive
													? 'text-red-600 hover:text-red-800'
													: 'text-green-600 hover:text-green-800'
											"
											:title="user.isActive ? 'تعطيل' : 'تفعيل'"
										>
											<svg
												v-if="user.isActive"
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
													d="M18.364 18.364A9 9 0 005.636 5.636m12.728 12.728A9 9 0 015.636 5.636m12.728 12.728L5.636 5.636"
												/>
											</svg>
											<svg
												v-else
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
													d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z"
												/>
											</svg>
										</button>
										<button
											@click="$emit('delete-user', user)"
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
								</td>
							</tr>
						</tbody>
					</table>
				</div>
			</div>
		</div>
	</div>
</template>

<script>
	export default {
		name: "UsersManagement",
		props: {
			users: {
				type: Array,
				required: true,
			},
			loading: {
				type: Boolean,
				default: false,
			},
		},
		emits: [
			"search",
			"filter-by-role",
			"edit-user",
			"toggle-user-status",
			"delete-user",
			"add-user",
			"reload-users",
		],
		data() {
			return {
				searchQuery: "",
				roleFilter: "all",
			};
		},
		computed: {
			filteredUsers() {
				let result = [...this.users];

				// Apply role filter
				if (this.roleFilter !== "all") {
					result = result.filter((user) =>
						user.roles.includes(this.roleFilter)
					);
				}

				// Apply search query filter
				if (this.searchQuery.trim()) {
					const query = this.searchQuery.toLowerCase();
					result = result.filter(
						(user) =>
							user.username.toLowerCase().includes(query) ||
							user.email.toLowerCase().includes(query)
					);
				}

				return result;
			},
		},
		methods: {
			formatDate(dateString) {
				return new Date(dateString).toLocaleDateString("ar-IQ");
			},
			getUserRoleDisplay(roles) {
				if (roles.includes("Admin")) return "مدير";
				if (roles.includes("Provider")) return "مقدم خدمة";
				return "مستخدم";
			},
			onSearch() {
				this.$emit("search", this.searchQuery);
			},
			onFilterByRole() {
				this.$emit("filter-by-role", this.roleFilter);
			},
		},
	};
</script>
