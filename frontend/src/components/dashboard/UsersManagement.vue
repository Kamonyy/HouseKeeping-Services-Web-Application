<template>
	<div class="space-y-6 animate-fadeIn">
		<h1 class="text-3xl font-bold text-gray-800">إدارة المستخدمين</h1>

		<!-- Search and Filters -->
		<div
			class="bg-gradient-to-br from-white to-blue-50 rounded-xl shadow-md overflow-hidden"
		>
			<div class="bg-gradient-to-r from-blue-600 to-blue-700 text-white p-4">
				<p class="font-bold text-xl flex items-center text-white">
					<i class="fas fa-search mr-2"></i>
					البحث
				</p>
			</div>
			<div class="p-4">
				<div class="flex flex-wrap gap-4">
					<div class="flex-1">
						<input
							type="text"
							v-model="searchQuery"
							placeholder="البحث عن مستخدم..."
							class="w-full px-4 py-3 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500 transition-all"
							@input="onSearch"
						/>
					</div>
					<div class="flex gap-4">
						<select
							v-model="roleFilter"
							class="px-4 py-3 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500 transition-all"
							@change="onFilterByRole"
						>
							<option value="all">الجميع</option>
							<option value="Admin">المدراء</option>
							<option value="Provider">مقدمي الخدمات</option>
							<option value="User">المستخدمين</option>
						</select>
						<button
							@click="loadUsers"
							class="px-4 py-3 bg-gradient-to-r from-blue-600 to-blue-700 text-white rounded-lg hover:from-blue-700 hover:to-blue-800 transition-all duration-200 flex items-center gap-2 shadow-md"
						>
							<i class="fas fa-sync-alt"></i>
							تحديث
						</button>
					</div>
				</div>
			</div>
		</div>

		<!-- Users List -->
		<div
			class="bg-gradient-to-br from-white to-blue-50 rounded-xl shadow-md overflow-hidden"
		>
			<div
				class="bg-gradient-to-r from-blue-600 to-blue-700 text-white p-4 flex justify-between items-center"
			>
				<p class="font-bold text-xl flex items-center text-white">
					<i class="fas fa-users mr-2"></i>
					قائمة المستخدمين
				</p>
				<button
					class="px-3 py-1.5 bg-blue-600 text-white rounded-lg flex items-center gap-1 hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500 text-sm transition-all transform hover:scale-105 duration-200 shadow-sm"
					@click="openAddUserModal"
				>
					<i class="fas fa-plus-circle mr-1"></i>
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

				<div
					v-else-if="filteredUsers.length === 0"
					class="text-center py-12 bg-blue-50/50 rounded-lg border border-blue-100"
				>
					<i class="fas fa-user-slash text-blue-300 text-6xl mb-4"></i>
					<p class="text-gray-500 text-lg font-medium">
						لم يتم العثور على مستخدمين
					</p>
					<p class="text-gray-400 mt-2">
						جرب تغيير معايير البحث أو إضافة مستخدمين جدد
					</p>
				</div>

				<div v-else class="overflow-x-auto">
					<table class="min-w-full divide-y divide-gray-200">
						<thead class="bg-blue-50">
							<tr>
								<th
									class="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider"
								>
									اسم المستخدم
								</th>
								<th
									class="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider"
								>
									الاسم الكامل
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
								class="hover:bg-blue-50/30 transition-colors"
							>
								<td class="px-6 py-4 whitespace-nowrap">
									<div class="flex items-center gap-3">
										<div
											class="h-10 w-10 rounded-full bg-gradient-to-br from-blue-100 to-blue-200 flex items-center justify-center text-blue-600 font-bold shadow-sm"
										>
											{{
												(user.username || user.userName || "?")
													.charAt(0)
													.toUpperCase()
											}}
										</div>
										<span class="font-medium text-gray-900">{{
											user.username || user.userName || "مستخدم غير معروف"
										}}</span>
									</div>
								</td>
								<td class="px-6 py-4 whitespace-nowrap">
									{{ getFullName(user) }}
								</td>
								<td class="px-6 py-4 whitespace-nowrap">
									{{ user.email || "لا يوجد بريد إلكتروني" }}
								</td>
								<td class="px-6 py-4 whitespace-nowrap">
									<span
										class="px-3 py-1 inline-flex text-xs leading-5 font-semibold rounded-full"
										:class="{
											'bg-purple-100 text-purple-800': isUserAdmin(user),
											'bg-blue-100 text-blue-800': isUserProvider(user),
											'bg-green-100 text-green-800': isUserCustomer(user),
											'bg-gray-100 text-gray-800': !hasAnyRole(user),
										}"
									>
										{{ getRoleDisplayText(user) }}
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
											@click="editUser(user)"
											class="p-1.5 bg-blue-100 text-blue-600 rounded-lg hover:bg-blue-200 transition-colors"
											title="تحرير"
										>
											<i class="fas fa-edit"></i>
										</button>
										<button
											@click="toggleUserStatus(user)"
											class="p-1.5 rounded-lg transition-colors"
											:class="
												user.isActive
													? 'bg-red-100 text-red-600 hover:bg-red-200'
													: 'bg-green-100 text-green-600 hover:bg-green-200'
											"
											:title="user.isActive ? 'تعطيل' : 'تفعيل'"
										>
											<i v-if="user.isActive" class="fas fa-ban"></i>
											<i v-else class="fas fa-check"></i>
										</button>
										<button
											@click="confirmDeleteUser(user)"
											class="p-1.5 bg-red-100 text-red-600 rounded-lg hover:bg-red-200 transition-colors"
											title="حذف"
										>
											<i class="fas fa-trash-alt"></i>
										</button>
									</div>
								</td>
							</tr>
						</tbody>
					</table>
				</div>
			</div>
		</div>

		<!-- User Edit Modal -->
		<Modal
			:is-open="showUserModal"
			:title="
				selectedUser && selectedUser.id ? 'تحرير مستخدم' : 'إضافة مستخدم جديد'
			"
			size="md"
			@close="showUserModal = false"
		>
			<div v-if="selectedUser" class="space-y-4 text-right">
				<div class="grid grid-cols-1 gap-4">
					<div>
						<label class="block text-sm font-medium text-gray-700 mb-1"
							>اسم المستخدم</label
						>
						<input
							type="text"
							v-model="selectedUser.username"
							class="w-full border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 px-4 py-2"
							placeholder="أدخل اسم المستخدم"
							:disabled="selectedUser.id !== 0"
						/>
						<p v-if="selectedUser.id !== 0" class="text-sm text-gray-500 mt-1">
							لا يمكن تغيير اسم المستخدم بعد الإنشاء
						</p>
					</div>

					<div class="grid grid-cols-1 md:grid-cols-2 gap-4">
						<div>
							<label class="block text-sm font-medium text-gray-700 mb-1"
								>الاسم الأول</label
							>
							<input
								type="text"
								v-model="selectedUser.FirstName"
								class="w-full border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 px-4 py-2"
								placeholder="أدخل الاسم الأول"
							/>
						</div>
						<div>
							<label class="block text-sm font-medium text-gray-700 mb-1"
								>اسم العائلة</label
							>
							<input
								type="text"
								v-model="selectedUser.LastName"
								class="w-full border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 px-4 py-2"
								placeholder="أدخل اسم العائلة"
							/>
						</div>
					</div>

					<div>
						<label class="block text-sm font-medium text-gray-700 mb-1"
							>البريد الإلكتروني</label
						>
						<input
							type="email"
							v-model="selectedUser.email"
							class="w-full border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 px-4 py-2"
							placeholder="أدخل البريد الإلكتروني"
						/>
					</div>

					<div v-if="selectedUser.id === 0">
						<label class="block text-sm font-medium text-gray-700 mb-1"
							>كلمة المرور</label
						>
						<input
							type="password"
							v-model="selectedUser.password"
							class="w-full border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 px-4 py-2"
							placeholder="أدخل كلمة المرور"
						/>
						<p class="text-sm text-gray-500 mt-1">
							اترك هذا الحقل فارغًا لاستخدام كلمة المرور الافتراضية
							(Password123!)
						</p>
					</div>

					<div>
						<label class="block text-sm font-medium text-gray-700 mb-1"
							>الأدوار</label
						>
						<div class="space-y-2">
							<div class="flex items-center">
								<input
									type="radio"
									id="role-user"
									v-model="selectedUser.role"
									value="User"
									class="w-5 h-5 ml-2 text-blue-600 focus:ring-blue-500"
								/>
								<label for="role-user" class="text-gray-700">مستخدم</label>
							</div>
							<div class="flex items-center">
								<input
									type="radio"
									id="role-provider"
									v-model="selectedUser.role"
									value="Provider"
									class="w-5 h-5 ml-2 text-blue-600 focus:ring-blue-500"
								/>
								<label for="role-provider" class="text-gray-700"
									>مقدم خدمة</label
								>
							</div>
							<div class="flex items-center">
								<input
									type="radio"
									id="role-admin"
									v-model="selectedUser.role"
									value="Admin"
									class="w-5 h-5 ml-2 text-blue-600 focus:ring-blue-500"
								/>
								<label for="role-admin" class="text-gray-700">مدير</label>
							</div>
						</div>
					</div>

					<div class="flex items-center">
						<input
							type="checkbox"
							id="is-active"
							v-model="selectedUser.isActive"
							class="w-5 h-5 ml-2 text-blue-600 focus:ring-blue-500"
						/>
						<label for="is-active" class="text-gray-700">حساب نشط</label>
					</div>
				</div>
			</div>
			<template v-slot:actions>
				<div class="flex justify-start w-full">
					<button
						class="px-4 py-2 bg-blue-600 hover:bg-blue-700 text-white rounded transition-colors focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500"
						@click="saveUser"
					>
						حفظ
					</button>
				</div>
			</template>
		</Modal>

		<!-- Delete Confirmation Modal -->
		<Modal
			:is-open="showDeleteConfirmation"
			title="تأكيد الحذف"
			size="sm"
			@close="showDeleteConfirmation = false"
		>
			<div class="text-right">
				<p class="text-gray-700">
					هل أنت متأكد من رغبتك في حذف المستخدم "{{ userToDelete?.username }}"؟
				</p>
				<p class="text-red-600 text-sm mt-2">لا يمكن التراجع عن هذا الإجراء.</p>
			</div>
			<template v-slot:actions>
				<div class="flex justify-between w-full">
					<button
						class="px-4 py-2 bg-gray-500 hover:bg-gray-600 text-white rounded transition-colors focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-gray-500"
						@click="showDeleteConfirmation = false"
					>
						اغلاق
					</button>
					<button
						class="px-4 py-2 bg-red-600 hover:bg-red-700 text-white rounded transition-colors focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-red-500"
						@click="deleteUser"
					>
						حذف
					</button>
				</div>
			</template>
		</Modal>
	</div>
</template>

<script>
	import { ref, computed, onMounted } from "vue";
	import UserService from "@/services/UserService.js";
	import { useToastStore } from "@/store/toastStore";
	import Modal from "@/components/Modal.vue";

	export default {
		name: "UsersManagement",
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
			const users = ref([]);
			const loadingUsers = ref(false);
			const searchQuery = ref("");
			const roleFilter = ref("all");
			const selectedUser = ref(null);
			const showUserModal = ref(false);
			const userToDelete = ref(null);
			const showDeleteConfirmation = ref(false);
			const toastStore = useToastStore();

			// Filtered users based on search query and role filter
			const filteredUsers = computed(() => {
				let result = [...users.value];
				// Apply role filter
				if (roleFilter.value !== "all") {
					const targetRole = roleFilter.value;
					result = result.filter((user) => {
						if (targetRole === "Admin") return isUserAdmin(user);
						if (targetRole === "Provider") return isUserProvider(user);
						if (targetRole === "User") return isUserCustomer(user);
						return false;
					});
				}
				// Apply search query filter
				if (searchQuery.value.trim()) {
					const query = searchQuery.value.toLowerCase();
					result = result.filter((user) => {
						const username = (
							user.username ||
							user.userName ||
							""
						).toLowerCase();
						const email = (user.email || "").toLowerCase();
						return username.includes(query) || email.includes(query);
					});
				}
				return result;
			});

			onMounted(() => {
				loadUsers();
			});

			// User role helper methods
			const isUserAdmin = (user) => {
				if (user.UserType === 2 || user.userType === 2) return true;
				if (typeof user.UserType === "string") {
					if (user.UserType === "Admin" || user.UserType === "2") return true;
				}
				if (typeof user.userType === "string") {
					if (user.userType === "Admin" || user.userType === "2") return true;
				}
				return false;
			};
			const getRoleTypeFromName = (roleName) => {
				if (roleName === "Admin") return 2;
				if (roleName === "Provider") return 1;
				if (roleName === "User") return 0;
				return null;
			};
			const isUserProvider = (user) => {
				if (user.UserType === 1 || user.userType === 1) return true;
				if (typeof user.UserType === "string") {
					if (user.UserType === "Provider" || user.UserType === "1")
						return true;
				}
				if (typeof user.userType === "string") {
					if (user.userType === "Provider" || user.userType === "1")
						return true;
				}
				return false;
			};
			const isUserCustomer = (user) => {
				if (user.UserType === 0 || user.userType === 0) return true;
				if (typeof user.UserType === "string") {
					if (user.UserType === "Customer" || user.UserType === "0")
						return true;
				}
				if (typeof user.userType === "string") {
					if (user.userType === "Customer" || user.userType === "0")
						return true;
				}
				return false;
			};
			const hasAnyRole = (user) => {
				return (
					isUserAdmin(user) || isUserProvider(user) || isUserCustomer(user)
				);
			};
			const formatDate = (dateString) => {
				if (!dateString) return "غير محدد";
				try {
					return new Date(dateString).toLocaleDateString("ar-IQ");
				} catch (error) {
					return "غير محدد";
				}
			};
			const getRoleDisplayText = (user) => {
				if (!user) return "مستخدم";
				if (isUserAdmin(user)) return "مدير";
				if (isUserProvider(user)) return "مقدم خدمة";
				if (isUserCustomer(user)) return "مستخدم";
				return "مستخدم";
			};
			const getFullName = (user) => {
				const firstName =
					user.FirstName ||
					user.firstName ||
					user.first_name ||
					user.firstname ||
					"";
				const lastName =
					user.LastName ||
					user.lastName ||
					user.last_name ||
					user.lastname ||
					"";
				if (firstName || lastName) return `${firstName} ${lastName}`.trim();
				if (user.fullName || user.full_name || user.name)
					return user.fullName || user.full_name || user.name;
				return user.username || user.userName || "";
			};

			// User management methods
			const loadUsers = async () => {
				loadingUsers.value = true;
				try {
					const fetchedUsers = await UserService.getUsers(props.userToken);
					users.value = fetchedUsers.map((user) =>
						UserService.normalizeUserData(user)
					);
				} catch (error) {
					toastStore.error("فشل تحميل المستخدمين. يرجى المحاولة مرة أخرى.");
					users.value = [];
				} finally {
					loadingUsers.value = false;
				}
			};
			const editUser = (user) => {
				selectedUser.value = {
					...user,
					isActive: typeof user.isActive === "boolean" ? user.isActive : true,
					firstName: user.FirstName || user.firstName || "",
					lastName: user.LastName || user.lastName || "",
					FirstName: user.FirstName || user.firstName || "",
					LastName: user.LastName || user.lastName || "",
				};
				showUserModal.value = true;
			};
			const openAddUserModal = () => {
				selectedUser.value = {
					id: 0,
					username: "",
					email: "",
					role: "User",
					UserType: 0,
					isActive: true,
					password: "",
					firstName: "",
					lastName: "",
					FirstName: "",
					LastName: "",
				};
				showUserModal.value = true;
			};
			const saveUser = async () => {
				try {
					const singleRole = selectedUser.value.role;
					const roles = [singleRole];
					const userData = {
						email: selectedUser.value.email,
						Username: selectedUser.value.username,
						roles: roles,
						isActive: selectedUser.value.isActive,
						FirstName:
							selectedUser.value.FirstName ||
							selectedUser.value.firstName ||
							"",
						LastName:
							selectedUser.value.LastName || selectedUser.value.lastName || "",
					};
					if (selectedUser.value.id === 0) {
						userData.password = selectedUser.value.password || "Password123!";
					} else {
						const existingUser = users.value.find(
							(u) => u.id === selectedUser.value.id
						);
						if (existingUser) {
							const isUnchanged =
								existingUser.email === userData.email &&
								existingUser.username === userData.Username &&
								(existingUser.role === singleRole ||
									existingUser.UserType === getRoleTypeFromName(singleRole)) &&
								existingUser.isActive === userData.isActive &&
								(existingUser.FirstName || existingUser.firstName || "") ===
									userData.FirstName &&
								(existingUser.LastName || existingUser.lastName || "") ===
									userData.LastName;
							if (isUnchanged) {
								showUserModal.value = false;
								toastStore.info("لم يتم إجراء أي تغييرات على بيانات المستخدم");
								return;
							}
						}
					}
					let response;
					if (selectedUser.value.id === 0) {
						response = await UserService.createUser(userData, props.userToken);
					} else {
						response = await UserService.updateUser(
							selectedUser.value.id,
							userData,
							props.userToken
						);
					}
					showUserModal.value = false;
					await loadUsers();
					if (selectedUser.value.id === 0) {
						toastStore.success("تم إضافة المستخدم بنجاح!");
					} else {
						toastStore.success("تم تحديث بيانات المستخدم بنجاح!");
					}
				} catch (error) {
					toastStore.error(
						`فشل حفظ بيانات المستخدم. يرجى المحاولة مرة أخرى. ${error.message}`
					);
				}
			};
			const toggleUserStatus = async (user) => {
				try {
					const updateData = {
						email: user.email,
						Username: user.username,
						roles: [user.role],
						isActive: !user.isActive,
						FirstName: user.FirstName || "",
						LastName: user.LastName || "",
					};
					await UserService.updateUser(user.id, updateData, props.userToken);
					const index = users.value.findIndex((u) => u.id === user.id);
					if (index !== -1) {
						users.value[index].isActive = !user.isActive;
						const status = users.value[index].isActive ? "تفعيل" : "تعطيل";
						toastStore.success(`تم ${status} حساب المستخدم بنجاح!`);
					}
				} catch (error) {
					toastStore.error("فشل تغيير حالة المستخدم. يرجى المحاولة مرة أخرى.");
				}
			};
			const confirmDeleteUser = (user) => {
				userToDelete.value = user;
				showDeleteConfirmation.value = true;
			};
			const deleteUser = async () => {
				if (!userToDelete.value || !userToDelete.value.id) {
					toastStore.error("لم يتم تحديد مستخدم للحذف.");
					return;
				}
				try {
					await UserService.deleteUser(userToDelete.value.id, props.userToken);
					showDeleteConfirmation.value = false;
					await loadUsers();
					toastStore.success("تم حذف المستخدم بنجاح!");
				} catch (error) {
					toastStore.error("فشل حذف المستخدم. يرجى المحاولة مرة أخرى.");
				}
			};
			const onSearch = () => {
				// Implementation of onSearch method
			};
			const onFilterByRole = () => {
				// Implementation of onFilterByRole method
			};
			return {
				users,
				loadingUsers,
				searchQuery,
				roleFilter,
				selectedUser,
				showUserModal,
				userToDelete,
				showDeleteConfirmation,
				filteredUsers,
				loadUsers,
				isUserAdmin,
				isUserProvider,
				isUserCustomer,
				hasAnyRole,
				formatDate,
				getRoleDisplayText,
				getFullName,
				editUser,
				openAddUserModal,
				saveUser,
				toggleUserStatus,
				confirmDeleteUser,
				deleteUser,
				onSearch,
				onFilterByRole,
				getRoleTypeFromName,
			};
		},
	};
</script>
