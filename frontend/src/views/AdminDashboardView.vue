<template>
	<div class="flex h-screen bg-gray-100 animate-fadeIn" dir="rtl">
		<!-- Admin Sidebar -->
		<div class="w-64 bg-blue-800 text-white shadow-lg">
			<div class="p-4 border-b border-blue-700">
				<h2 class="text-xl font-bold">لوحة التحكم</h2>
				<p class="text-sm text-blue-300">مرحباً بك في لوحة التحكم</p>
			</div>
			<nav class="p-2">
				<ul class="space-y-1">
					<li>
						<button
							@click="handleSectionChange('dashboard')"
							class="w-full flex items-center p-3 text-right rounded transition-colors"
							:class="
								activeSection === 'dashboard'
									? 'bg-blue-700 text-white'
									: 'text-blue-200 hover:bg-blue-700 hover:text-white'
							"
						>
							<svg
								xmlns="http://www.w3.org/2000/svg"
								class="h-5 w-5 ml-3"
								fill="none"
								viewBox="0 0 24 24"
								stroke="currentColor"
							>
								<path
									stroke-linecap="round"
									stroke-linejoin="round"
									stroke-width="2"
									d="M3 12l2-2m0 0l7-7 7 7M5 10v10a1 1 0 001 1h3m10-11l2 2m-2-2v10a1 1 0 01-1 1h-3m-6 0a1 1 0 001-1v-4a1 1 0 011-1h2a1 1 0 011 1v4a1 1 0 001 1m-6 0h6"
								/>
							</svg>
							الرئيسية
						</button>
					</li>
					<li>
						<button
							@click="handleSectionChange('services')"
							class="w-full flex items-center p-3 text-right rounded transition-colors"
							:class="
								activeSection === 'services'
									? 'bg-blue-700 text-white'
									: 'text-blue-200 hover:bg-blue-700 hover:text-white'
							"
						>
							<svg
								xmlns="http://www.w3.org/2000/svg"
								class="h-5 w-5 ml-3"
								fill="none"
								viewBox="0 0 24 24"
								stroke="currentColor"
							>
								<path
									stroke-linecap="round"
									stroke-linejoin="round"
									stroke-width="2"
									d="M21 13.255A23.931 23.931 0 0112 15c-3.183 0-6.22-.62-9-1.745M16 6V4a2 2 0 00-2-2h-4a2 2 0 00-2 2v2m4 6h.01M5 20h14a2 2 0 002-2V8a2 2 0 00-2-2H5a2 2 0 00-2 2v10a2 2 0 002 2z"
								/>
							</svg>
							إدارة الخدمات
						</button>
					</li>
					<li>
						<button
							@click="handleSectionChange('users')"
							class="w-full flex items-center p-3 text-right rounded transition-colors"
							:class="
								activeSection === 'users'
									? 'bg-blue-700 text-white'
									: 'text-blue-200 hover:bg-blue-700 hover:text-white'
							"
						>
							<svg
								xmlns="http://www.w3.org/2000/svg"
								class="h-5 w-5 ml-3"
								fill="none"
								viewBox="0 0 24 24"
								stroke="currentColor"
							>
								<path
									stroke-linecap="round"
									stroke-linejoin="round"
									stroke-width="2"
									d="M12 4.354a4 4 0 110 5.292M15 21H3v-1a6 6 0 0112 0v1zm0 0h6v-1a6 6 0 00-9-5.197M13 7a4 4 0 11-8 0 4 4 0 018 0z"
								/>
							</svg>
							إدارة المستخدمين
						</button>
					</li>
					<li>
						<button
							@click="handleSectionChange('categories')"
							class="w-full flex items-center p-3 text-right rounded transition-colors"
							:class="
								activeSection === 'categories'
									? 'bg-blue-700 text-white'
									: 'text-blue-200 hover:bg-blue-700 hover:text-white'
							"
						>
							<svg
								xmlns="http://www.w3.org/2000/svg"
								class="h-5 w-5 ml-3"
								fill="none"
								viewBox="0 0 24 24"
								stroke="currentColor"
							>
								<path
									stroke-linecap="round"
									stroke-linejoin="round"
									stroke-width="2"
									d="M7 7h.01M7 3h5c.512 0 1.024.195 1.414.586l7 7a2 2 0 010 2.828l-7 7a2 2 0 01-2.828 0l-7-7A1.994 1.994 0 013 12V7a4 4 0 014-4z"
								/>
							</svg>
							إدارة التصنيفات
						</button>
					</li>
					<li>
						<button
							@click="handleSectionChange('reports')"
							class="w-full flex items-center p-3 text-right rounded transition-colors"
							:class="
								activeSection === 'reports'
									? 'bg-blue-700 text-white'
									: 'text-blue-200 hover:bg-blue-700 hover:text-white'
							"
						>
							<svg
								xmlns="http://www.w3.org/2000/svg"
								class="h-5 w-5 ml-3"
								fill="none"
								viewBox="0 0 24 24"
								stroke="currentColor"
							>
								<path
									stroke-linecap="round"
									stroke-linejoin="round"
									stroke-width="2"
									d="M9 17v-2m3 2v-4m3 4v-6m2 10H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z"
								/>
							</svg>
							التقارير والإحصائيات
						</button>
					</li>
					<li>
						<button
							@click="handleSectionChange('settings')"
							class="w-full flex items-center p-3 text-right rounded transition-colors"
							:class="
								activeSection === 'settings'
									? 'bg-blue-700 text-white'
									: 'text-blue-200 hover:bg-blue-700 hover:text-white'
							"
						>
							<svg
								xmlns="http://www.w3.org/2000/svg"
								class="h-5 w-5 ml-3"
								fill="none"
								viewBox="0 0 24 24"
								stroke="currentColor"
							>
								<path
									stroke-linecap="round"
									stroke-linejoin="round"
									stroke-width="2"
									d="M10.325 4.317c.426-1.756 2.924-1.756 3.35 0a1.724 1.724 0 002.573 1.066c1.543-.94 3.31.826 2.37 2.37a1.724 1.724 0 001.065 2.572c1.756.426 1.756 2.924 0 3.35a1.724 1.724 0 00-1.066 2.573c.94 1.543-.826 3.31-2.37 2.37a1.724 1.724 0 00-2.572 1.065c-.426 1.756-2.924 1.756-3.35 0a1.724 1.724 0 00-2.573-1.066c-1.543.94-3.31-.826-2.37-2.37a1.724 1.724 0 00-1.065-2.572c-1.756-.426-1.756-2.924 0-3.35a1.724 1.724 0 001.066-2.573c-.94-1.543.826-3.31 2.37-2.37.996.608 2.296.07 2.572-1.065z"
								/>
								<path
									stroke-linecap="round"
									stroke-linejoin="round"
									stroke-width="2"
									d="M15 12a3 3 0 11-6 0 3 3 0 016 0z"
								/>
							</svg>
							الإعدادات
						</button>
					</li>
				</ul>
			</nav>
		</div>

		<!-- Main Content Area -->
		<div class="flex-1 overflow-auto">
			<div class="p-6">
				<!-- Dashboard Section -->
				<DashboardOverview
					v-if="activeSection === 'dashboard'"
					:total-services="totalServices"
					:approved-services="approvedServices"
					:pending-services="pendingServices"
					:total-users="totalUsers"
				/>

				<!-- Services Management Section -->
				<ServicesManagement
					v-if="activeSection === 'services'"
					:services="services"
					:loading="loading"
					@view-service-details="viewServiceDetails"
					@approve-service="approveService"
					@revoke-approval="revokeApproval"
				/>

				<!-- Users Management Section -->
				<UsersManagement
					v-if="activeSection === 'users'"
					:users="users"
					:loading="loadingUsers"
					@search="searchUsers"
					@filter-by-role="filterUsersByRole"
					@edit-user="editUser"
					@toggle-user-status="toggleUserStatus"
					@delete-user="confirmDeleteUser"
					@add-user="openAddUserModal"
					@reload-users="loadUsers"
				/>

				<!-- Categories Management Section -->
				<CategoriesManagement
					v-if="activeSection === 'categories'"
					:main-categories="mainCategories"
					:sub-categories="subCategories"
					:loading="loadingCategories"
					:category-name-map="categoryNameMap"
					@add-main-category="addMainCategory"
					@edit-main-category="editMainCategory"
					@delete-main-category="deleteMainCategory"
					@add-sub-category="addSubCategory"
					@edit-sub-category="editSubCategory"
					@delete-sub-category="deleteSubCategory"
				/>

				<!-- Reports Section -->
				<div v-if="activeSection === 'reports'" class="space-y-6">
					<h1 class="text-3xl font-bold text-gray-800">التقارير والإحصائيات</h1>
					<div class="grid grid-cols-1 lg:grid-cols-2 gap-6">
						<!-- Service Statistics -->
						<div class="bg-white rounded-lg shadow-md overflow-hidden">
							<div class="bg-blue-600 text-white p-4">
								<h4 class="font-bold text-xl">إحصائيات الخدمات</h4>
							</div>
							<div class="p-6">
								<div class="flex justify-between items-center mb-6">
									<h5 class="text-lg font-semibold text-gray-700">
										توزيع الخدمات حسب التصنيف
									</h5>
									<div class="text-sm text-gray-500">
										<select class="border rounded p-1">
											<option>آخر 30 يوم</option>
											<option>آخر 3 أشهر</option>
											<option>آخر سنة</option>
											<option>الكل</option>
										</select>
									</div>
								</div>
								<!-- Placeholder Chart -->
								<div
									class="h-64 flex items-center justify-center bg-gray-100 rounded"
								>
									<p class="text-gray-500">سيتم عرض رسم بياني هنا</p>
								</div>
							</div>
						</div>

						<!-- User Growth -->
						<div class="bg-white rounded-lg shadow-md overflow-hidden">
							<div class="bg-blue-600 text-white p-4">
								<h4 class="font-bold text-xl">نمو المستخدمين</h4>
							</div>
							<div class="p-6">
								<div class="flex justify-between items-center mb-6">
									<h5 class="text-lg font-semibold text-gray-700">
										عدد المستخدمين الجدد
									</h5>
									<div class="text-sm text-gray-500">
										<select class="border rounded p-1">
											<option>آخر 30 يوم</option>
											<option>آخر 3 أشهر</option>
											<option>آخر سنة</option>
										</select>
									</div>
								</div>
								<!-- Placeholder Chart -->
								<div
									class="h-64 flex items-center justify-center bg-gray-100 rounded"
								>
									<p class="text-gray-500">سيتم عرض رسم بياني هنا</p>
								</div>
							</div>
						</div>

						<!-- Service Engagement -->
						<div class="bg-white rounded-lg shadow-md overflow-hidden">
							<div class="bg-blue-600 text-white p-4">
								<h4 class="font-bold text-xl">تفاعل الخدمات</h4>
							</div>
							<div class="p-6">
								<div class="space-y-4">
									<div class="flex justify-between items-center">
										<h5 class="font-medium">متوسط ​​التقييمات</h5>
										<span class="flex items-center">
											<svg
												xmlns="http://www.w3.org/2000/svg"
												class="h-5 w-5 text-yellow-500"
												viewBox="0 0 20 20"
												fill="currentColor"
											>
												<path
													d="M9.049 2.927c.3-.921 1.603-.921 1.902 0l1.07 3.292a1 1 0 00.95.69h3.462c.969 0 1.371 1.24.588 1.81l-2.8 2.034a1 1 0 00-.364 1.118l1.07 3.292c.3.921-.755 1.688-1.54 1.118l-2.8-2.034a1 1 0 00-1.175 0l-2.8 2.034c-.784.57-1.838-.197-1.539-1.118l1.07-3.292a1 1 0 00-.364-1.118l-2.8-2.034c-.783-.57-.38-1.81.588-1.81h3.461a1 1 0 00.951-.69l1.07-3.292z"
												/>
											</svg>
											<span class="ml-1">4.2</span>
										</span>
									</div>
									<div class="w-full h-2 bg-gray-200 rounded">
										<div
											class="h-full bg-yellow-500 rounded"
											style="width: 84%"
										></div>
									</div>

									<div class="flex justify-between items-center">
										<h5 class="font-medium">معدل إكمال الخدمات</h5>
										<span>78%</span>
									</div>
									<div class="w-full h-2 bg-gray-200 rounded">
										<div
											class="h-full bg-green-500 rounded"
											style="width: 78%"
										></div>
									</div>

									<div class="flex justify-between items-center">
										<h5 class="font-medium">معدل النمو الشهري</h5>
										<span class="text-green-600">+12%</span>
									</div>
									<div class="w-full h-2 bg-gray-200 rounded">
										<div
											class="h-full bg-blue-500 rounded"
											style="width: 62%"
										></div>
									</div>
								</div>
							</div>
						</div>

						<!-- Top Performers -->
						<div class="bg-white rounded-lg shadow-md overflow-hidden">
							<div class="bg-blue-600 text-white p-4">
								<h4 class="font-bold text-xl">أفضل مقدمي الخدمات</h4>
							</div>
							<div class="p-6">
								<div class="space-y-4">
									<div class="text-sm text-gray-500 mb-2">
										بناءً على تقييمات المستخدمين وعدد الخدمات المقدمة
									</div>
									<div class="text-center py-8 text-gray-500">
										سيتم تطوير هذا القسم قريباً
									</div>
								</div>
							</div>
						</div>
					</div>
				</div>

				<!-- Settings Section -->
				<div v-if="activeSection === 'settings'" class="space-y-6">
					<h1 class="text-3xl font-bold text-gray-800">إعدادات الموقع</h1>
					<div class="bg-white rounded-lg shadow-md overflow-hidden">
						<div class="bg-blue-600 text-white p-4">
							<h4 class="font-bold text-xl">الإعدادات العامة</h4>
						</div>
						<div class="p-6">
							<form class="space-y-6">
								<div class="space-y-4">
									<h5 class="text-lg font-medium text-gray-700 border-b pb-2">
										معلومات الموقع
									</h5>

									<div class="grid grid-cols-1 md:grid-cols-2 gap-4">
										<div>
											<label
												class="block text-sm font-medium text-gray-700 mb-1"
												>عنوان الموقع</label
											>
											<input
												type="text"
												class="w-full border border-gray-300 rounded px-3 py-2 focus:outline-none focus:ring-1 focus:ring-blue-500"
												placeholder="أدخل عنوان الموقع"
											/>
										</div>
										<div>
											<label
												class="block text-sm font-medium text-gray-700 mb-1"
												>وصف الموقع</label
											>
											<input
												type="text"
												class="w-full border border-gray-300 rounded px-3 py-2 focus:outline-none focus:ring-1 focus:ring-blue-500"
												placeholder="أدخل وصف الموقع"
											/>
										</div>
									</div>

									<div>
										<label class="block text-sm font-medium text-gray-700 mb-1"
											>البريد الإلكتروني للتواصل</label
										>
										<input
											type="email"
											class="w-full border border-gray-300 rounded px-3 py-2 focus:outline-none focus:ring-1 focus:ring-blue-500"
											placeholder="admin@example.com"
										/>
									</div>
								</div>

								<div class="space-y-4">
									<h5 class="text-lg font-medium text-gray-700 border-b pb-2">
										إعدادات التسجيل
									</h5>

									<div class="flex items-center">
										<input
											type="checkbox"
											id="allowRegistration"
											class="w-5 h-5 ml-2 text-blue-600 focus:ring-blue-500"
										/>
										<label for="allowRegistration" class="text-gray-700"
											>السماح بتسجيل مستخدمين جدد</label
										>
									</div>

									<div class="flex items-center">
										<input
											type="checkbox"
											id="emailVerification"
											class="w-5 h-5 ml-2 text-blue-600 focus:ring-blue-500"
										/>
										<label for="emailVerification" class="text-gray-700"
											>تفعيل التحقق من البريد الإلكتروني</label
										>
									</div>
								</div>

								<div class="flex justify-end">
									<button
										type="submit"
										class="px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500 transition-colors"
									>
										حفظ الإعدادات
									</button>
								</div>
							</form>
						</div>
					</div>
				</div>
			</div>
		</div>

		<!-- Service Details Modal -->
		<Modal
			:is-open="!!selectedService"
			title="تفاصيل الخدمة"
			size="lg"
			@close="selectedService = null"
		>
			<div v-if="selectedService" class="space-y-4 text-right">
				<div
					class="grid grid-cols-1 md:grid-cols-3 gap-2 border-b border-gray-200 pb-3"
				>
					<div class="font-semibold">العنوان:</div>
					<div class="md:col-span-2">{{ selectedService.title }}</div>
				</div>
				<div
					class="grid grid-cols-1 md:grid-cols-3 gap-2 border-b border-gray-200 pb-3"
				>
					<div class="font-semibold">الوصف:</div>
					<div class="md:col-span-2">{{ selectedService.description }}</div>
				</div>
				<div
					class="grid grid-cols-1 md:grid-cols-3 gap-2 border-b border-gray-200 pb-3"
				>
					<div class="font-semibold">مقدم الخدمة:</div>
					<div class="md:col-span-2">
						{{ selectedService.providerUsername }}
					</div>
				</div>
				<div
					class="grid grid-cols-1 md:grid-cols-3 gap-2 border-b border-gray-200 pb-3"
				>
					<div class="font-semibold">رقم الهاتف:</div>
					<div class="md:col-span-2">{{ selectedService.contactPhone }}</div>
				</div>
				<div
					class="grid grid-cols-1 md:grid-cols-3 gap-2 border-b border-gray-200 pb-3"
				>
					<div class="font-semibold">السعر التقريبي:</div>
					<div class="md:col-span-2">
						{{ selectedService.estimatedPrice.toLocaleString() }} د.ع
					</div>
				</div>
				<div
					class="grid grid-cols-1 md:grid-cols-3 gap-2 border-b border-gray-200 pb-3"
				>
					<div class="font-semibold">تاريخ الإنشاء:</div>
					<div class="md:col-span-2">
						{{
							new Date(selectedService.createdDate).toLocaleDateString("ar-IQ")
						}}
					</div>
				</div>
				<div class="grid grid-cols-1 md:grid-cols-3 gap-2">
					<div class="font-semibold">التصنيفات الفرعية:</div>
					<div class="md:col-span-2">
						<div class="flex flex-wrap gap-2">
							<span
								v-for="subCategory in selectedService.subCategories"
								:key="subCategory.id"
								class="px-3 py-1 bg-blue-100 text-blue-700 text-sm rounded-full"
							>
								{{ subCategory.name }}
							</span>
							<span
								v-if="
									!selectedService.subCategories ||
									selectedService.subCategories.length === 0
								"
								class="text-gray-500 text-sm"
							>
								لا توجد تصنيفات فرعية
							</span>
						</div>
					</div>
				</div>
			</div>
			<template v-slot:actions>
				<div class="flex justify-between w-full">
					<button
						class="px-4 py-2 bg-gray-500 hover:bg-gray-600 text-white rounded transition-colors focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-gray-500"
						@click="selectedService = null"
					>
						إغلاق
					</button>
					<button
						v-if="selectedService && !selectedService.isApproved"
						class="flex items-center gap-1 px-4 py-2 bg-green-600 hover:bg-green-700 text-white rounded transition-colors focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-green-500"
						@click="approveServiceAndCloseModal"
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
								d="M5 13l4 4L19 7"
							/>
						</svg>
						الموافقة على الخدمة
					</button>
				</div>
			</template>
		</Modal>

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
						/>
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

					<div>
						<label class="block text-sm font-medium text-gray-700 mb-1"
							>الأدوار</label
						>
						<div class="space-y-2">
							<div class="flex items-center">
								<input
									type="checkbox"
									id="role-user"
									v-model="selectedUser.roles"
									value="User"
									class="w-5 h-5 ml-2 text-blue-600 focus:ring-blue-500"
								/>
								<label for="role-user" class="text-gray-700">مستخدم</label>
							</div>
							<div class="flex items-center">
								<input
									type="checkbox"
									id="role-provider"
									v-model="selectedUser.roles"
									value="Provider"
									class="w-5 h-5 ml-2 text-blue-600 focus:ring-blue-500"
								/>
								<label for="role-provider" class="text-gray-700"
									>مقدم خدمة</label
								>
							</div>
							<div class="flex items-center">
								<input
									type="checkbox"
									id="role-admin"
									v-model="selectedUser.roles"
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
				<div class="flex justify-between w-full">
					<button
						class="px-4 py-2 bg-gray-500 hover:bg-gray-600 text-white rounded transition-colors focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-gray-500"
						@click="showUserModal = false"
					>
						إلغاء
					</button>
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
						إلغاء
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

		<!-- Category Edit Modal -->
		<Modal
			:is-open="showCategoryModal"
			:title="
				selectedCategory && selectedCategory.id
					? 'تحرير تصنيف'
					: 'إضافة تصنيف جديد'
			"
			size="md"
			@close="showCategoryModal = false"
		>
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
			<template v-slot:actions>
				<div class="flex justify-between w-full">
					<button
						class="px-4 py-2 bg-gray-500 hover:bg-gray-600 text-white rounded transition-colors focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-gray-500"
						@click="showCategoryModal = false"
					>
						إلغاء
					</button>
					<button
						class="px-4 py-2 bg-blue-600 hover:bg-blue-700 text-white rounded transition-colors focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500"
						@click="saveCategory"
					>
						حفظ
					</button>
				</div>
			</template>
		</Modal>

		<!-- Sub Category Edit Modal -->
		<Modal
			:is-open="showSubCategoryModal"
			:title="
				selectedSubCategory && selectedSubCategory.id
					? 'تحرير تصنيف فرعي'
					: 'إضافة تصنيف فرعي'
			"
			size="md"
			@close="showSubCategoryModal = false"
		>
			<div class="space-y-4 text-right">
				<div>
					<label class="block text-sm font-medium text-gray-700 mb-1"
						>اسم التصنيف الفرعي</label
					>
					<input
						type="text"
						v-model="newCategoryName"
						class="w-full border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 px-4 py-2"
						placeholder="أدخل اسم التصنيف الفرعي"
					/>
				</div>
				<div>
					<label class="block text-sm font-medium text-gray-700 mb-1"
						>التصنيف الرئيسي</label
					>
					<select
						v-model="selectedMainCategoryId"
						class="w-full border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 px-4 py-2"
					>
						<option
							v-for="category in mainCategories"
							:key="category.id"
							:value="category.id"
						>
							{{ category.name }}
						</option>
					</select>
				</div>
			</div>
			<template v-slot:actions>
				<div class="flex justify-between w-full">
					<button
						class="px-4 py-2 bg-gray-500 hover:bg-gray-600 text-white rounded transition-colors focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-gray-500"
						@click="showSubCategoryModal = false"
					>
						إلغاء
					</button>
					<button
						class="px-4 py-2 bg-blue-600 hover:bg-blue-700 text-white rounded transition-colors focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500"
						@click="saveSubCategory"
					>
						حفظ
					</button>
				</div>
			</template>
		</Modal>

		<!-- Delete Category Confirmation -->
		<Modal
			:is-open="showDeleteCategoryConfirmation"
			title="تأكيد الحذف"
			size="sm"
			@close="showDeleteCategoryConfirmation = false"
		>
			<div class="text-right">
				<p class="text-gray-700">
					هل أنت متأكد من رغبتك في حذف التصنيف "{{ selectedCategory?.name }}"؟
				</p>
				<p class="text-red-600 text-sm mt-2">
					سيتم حذف جميع التصنيفات الفرعية المرتبطة بهذا التصنيف. لا يمكن التراجع
					عن هذا الإجراء.
				</p>
			</div>
			<template v-slot:actions>
				<div class="flex justify-between w-full">
					<button
						class="px-4 py-2 bg-gray-500 hover:bg-gray-600 text-white rounded transition-colors focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-gray-500"
						@click="showDeleteCategoryConfirmation = false"
					>
						إلغاء
					</button>
					<button
						class="px-4 py-2 bg-red-600 hover:bg-red-700 text-white rounded transition-colors focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-red-500"
						@click="confirmDeleteCategory"
					>
						حذف
					</button>
				</div>
			</template>
		</Modal>

		<!-- Delete Sub Category Confirmation -->
		<Modal
			:is-open="showDeleteSubCategoryConfirmation"
			title="تأكيد الحذف"
			size="sm"
			@close="showDeleteSubCategoryConfirmation = false"
		>
			<div class="text-right">
				<p class="text-gray-700">
					هل أنت متأكد من رغبتك في حذف التصنيف الفرعي "{{
						selectedSubCategory?.name
					}}"؟
				</p>
				<p class="text-red-600 text-sm mt-2">لا يمكن التراجع عن هذا الإجراء.</p>
			</div>
			<template v-slot:actions>
				<div class="flex justify-between w-full">
					<button
						class="px-4 py-2 bg-gray-500 hover:bg-gray-600 text-white rounded transition-colors focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-gray-500"
						@click="showDeleteSubCategoryConfirmation = false"
					>
						إلغاء
					</button>
					<button
						class="px-4 py-2 bg-red-600 hover:bg-red-700 text-white rounded transition-colors focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-red-500"
						@click="confirmDeleteSubCategory"
					>
						حذف
					</button>
				</div>
			</template>
		</Modal>
	</div>
</template>

<script>
	import { useUserStore } from "@/store/userStore";
	import { ref, computed, onMounted } from "vue";
	import { useRouter } from "vue-router";
	import Modal from "@/components/Modal.vue";
	import {
		extractArrayFromResponse,
		debugCategoryRelationships,
	} from "@/utils/apiUtils";
	// Import the dashboard components
	import DashboardOverview from "@/components/dashboard/DashboardOverview.vue";
	import ServicesManagement from "@/components/dashboard/ServicesManagement.vue";
	import UsersManagement from "@/components/dashboard/UsersManagement.vue";
	import CategoriesManagement from "@/components/dashboard/CategoriesManagement.vue";

	export default {
		name: "AdminDashboardView",
		components: {
			Modal,
			DashboardOverview,
			ServicesManagement,
			UsersManagement,
			CategoriesManagement,
		},
		setup() {
			const loading = ref(true);
			const services = ref([]);
			const statusFilter = ref("all");
			const selectedService = ref(null);
			const userStore = useUserStore();
			const router = useRouter();
			const activeSection = ref("dashboard");

			// Statistics
			const totalServices = ref(0);
			const totalUsers = ref(0);

			// Users management
			const users = ref([]);
			const loadingUsers = ref(false);
			const userSearchQuery = ref("");
			const userRoleFilter = ref("all");
			const selectedUser = ref(null);
			const showUserModal = ref(false);
			const userToDelete = ref(null);
			const showDeleteConfirmation = ref(false);

			// Categories management
			const mainCategories = ref([]);
			const subCategories = ref([]);
			const loadingCategories = ref(false);
			const categoryNameMap = ref({});

			// Add new state variables for category modals
			const showCategoryModal = ref(false);
			const showSubCategoryModal = ref(false);
			const showDeleteCategoryConfirmation = ref(false);
			const showDeleteSubCategoryConfirmation = ref(false);
			const selectedCategory = ref(null);
			const selectedSubCategory = ref(null);
			const newCategoryName = ref("");
			const selectedMainCategoryId = ref(null);

			const filteredServices = computed(() => {
				if (statusFilter.value === "all") {
					return services.value;
				} else if (statusFilter.value === "pending") {
					return services.value.filter((service) => !service.isApproved);
				} else {
					return services.value.filter((service) => service.isApproved);
				}
			});

			const approvedServices = computed(() => {
				return services.value.filter((service) => service.isApproved).length;
			});

			const pendingServices = computed(() => {
				return services.value.filter((service) => !service.isApproved).length;
			});

			const filteredUsers = computed(() => {
				let result = [...users.value];

				// Apply role filter
				if (userRoleFilter.value !== "all") {
					result = result.filter((user) =>
						user.roles.includes(userRoleFilter.value)
					);
				}

				// Apply search query filter
				if (userSearchQuery.value.trim()) {
					const query = userSearchQuery.value.toLowerCase();
					result = result.filter(
						(user) =>
							user.username.toLowerCase().includes(query) ||
							user.email.toLowerCase().includes(query)
					);
				}

				return result;
			});

			const isAdmin = computed(() => userStore.isAdmin);

			onMounted(() => {
				// Check if user is an admin, otherwise redirect
				if (!isAdmin.value) {
					router.push("/");
					return;
				}

				loadDashboardData();
			});

			const loadDashboardData = async () => {
				loading.value = true;
				try {
					await Promise.all([loadServices(), loadUserCount()]);
				} catch (error) {
					console.error("خطأ في تحميل بيانات لوحة التحكم:", error);
					alert("فشل تحميل البيانات. يرجى المحاولة مرة أخرى.");
				} finally {
					loading.value = false;
				}
			};

			const loadServices = async () => {
				try {
					const response = await fetch("/api/service/admin", {
						headers: {
							Authorization: `Bearer ${userStore.token}`,
						},
					});
					if (!response.ok) throw new Error("فشل تحميل الخدمات");
					const data = await response.json();
					services.value = extractArrayFromResponse(data, "services");
					totalServices.value = services.value.length;
					return services.value;
				} catch (error) {
					console.error("خطأ في تحميل الخدمات:", error);
					services.value = [];
					return [];
				}
			};

			const loadUserCount = async () => {
				try {
					// This is a placeholder - you'll need to implement the actual API
					// For now we'll just set a dummy value
					totalUsers.value = 28; // Replace with actual API call
					return totalUsers.value;
				} catch (error) {
					console.error("خطأ في تحميل عدد المستخدمين:", error);
					totalUsers.value = 0;
					return 0;
				}
			};

			const viewServiceDetails = (service) => {
				selectedService.value = service;
			};

			const approveService = async (serviceId) => {
				try {
					const response = await fetch(`/api/service/approve/${serviceId}`, {
						method: "PUT",
						headers: {
							Authorization: `Bearer ${userStore.token}`,
						},
					});

					if (!response.ok) throw new Error("فشل الموافقة على الخدمة");

					// Update the service in the list
					const serviceIndex = services.value.findIndex(
						(s) => s.id === serviceId
					);
					if (serviceIndex !== -1) {
						services.value[serviceIndex].isApproved = true;
					}

					alert("تمت الموافقة على الخدمة بنجاح!");
				} catch (error) {
					console.error("خطأ في الموافقة على الخدمة:", error);
					alert("فشل الموافقة على الخدمة. يرجى المحاولة مرة أخرى.");
				}
			};

			const revokeApproval = async (serviceId) => {
				try {
					const response = await fetch(`/api/service/revoke/${serviceId}`, {
						method: "PUT",
						headers: {
							Authorization: `Bearer ${userStore.token}`,
						},
					});

					if (!response.ok) throw new Error("فشل إلغاء الموافقة");

					// Update the service in the list
					const serviceIndex = services.value.findIndex(
						(s) => s.id === serviceId
					);
					if (serviceIndex !== -1) {
						services.value[serviceIndex].isApproved = false;
					}

					alert("تم إلغاء الموافقة على الخدمة بنجاح!");
				} catch (error) {
					console.error("خطأ في إلغاء الموافقة:", error);
					alert("فشل إلغاء الموافقة. يرجى المحاولة مرة أخرى.");
				}
			};

			const approveServiceAndCloseModal = () => {
				if (selectedService.value) {
					approveService(selectedService.value.id);
					selectedService.value = null;
				}
			};

			// User Management Methods
			const loadUsers = async () => {
				loadingUsers.value = true;
				try {
					// For testing purposes, we'll create dummy data
					// In a real application, you would fetch from the API
					// const response = await fetch('/api/users', {
					//   headers: {
					//     Authorization: `Bearer ${userStore.token}`
					//   }
					// });
					// if (!response.ok) throw new Error('فشل تحميل المستخدمين');
					// const data = await response.json();
					// users.value = data.users;

					// Dummy data for testing
					setTimeout(() => {
						users.value = [
							{
								id: 1,
								username: "admin",
								email: "admin@example.com",
								roles: ["Admin", "User"],
								isActive: true,
								createdDate: "2023-01-01T00:00:00Z",
							},
							{
								id: 2,
								username: "provider1",
								email: "provider1@example.com",
								roles: ["Provider", "User"],
								isActive: true,
								createdDate: "2023-02-15T00:00:00Z",
							},
							{
								id: 3,
								username: "user1",
								email: "user1@example.com",
								roles: ["User"],
								isActive: true,
								createdDate: "2023-03-20T00:00:00Z",
							},
							{
								id: 4,
								username: "provider2",
								email: "provider2@example.com",
								roles: ["Provider", "User"],
								isActive: false,
								createdDate: "2023-04-10T00:00:00Z",
							},
							{
								id: 5,
								username: "user2",
								email: "user2@example.com",
								roles: ["User"],
								isActive: true,
								createdDate: "2023-05-05T00:00:00Z",
							},
						];
						loadingUsers.value = false;
						totalUsers.value = users.value.length;
					}, 1000);
				} catch (error) {
					console.error("خطأ في تحميل المستخدمين:", error);
					alert("فشل تحميل المستخدمين. يرجى المحاولة مرة أخرى.");
					loadingUsers.value = false;
				}
			};

			const searchUsers = () => {
				// The filtering is handled by the computed property
				// This method can be used for more complex search logic if needed
			};

			const filterUsersByRole = () => {
				// The filtering is handled by the computed property
				// This method can be used for more complex filtering logic if needed
			};

			const getUserRoleDisplay = (roles) => {
				if (roles.includes("Admin")) return "مدير";
				if (roles.includes("Provider")) return "مقدم خدمة";
				return "مستخدم";
			};

			const formatDate = (dateString) => {
				return new Date(dateString).toLocaleDateString("ar-IQ");
			};

			const editUser = (user) => {
				selectedUser.value = { ...user };
				showUserModal.value = true;
			};

			const openAddUserModal = () => {
				selectedUser.value = {
					id: 0,
					username: "",
					email: "",
					roles: ["User"],
					isActive: true,
					createdDate: new Date().toISOString(),
				};
				showUserModal.value = true;
			};

			const saveUser = async () => {
				// In a real application, you would send to the API
				// For now, we'll just update the local array

				try {
					if (selectedUser.value.id === 0) {
						// New user
						selectedUser.value.id = users.value.length + 1;
						users.value.push({ ...selectedUser.value });
						alert("تم إضافة المستخدم بنجاح!");
					} else {
						// Existing user
						const index = users.value.findIndex(
							(u) => u.id === selectedUser.value.id
						);
						if (index !== -1) {
							users.value[index] = { ...selectedUser.value };
						}
						alert("تم تحديث بيانات المستخدم بنجاح!");
					}
					showUserModal.value = false;
				} catch (error) {
					console.error("خطأ في حفظ المستخدم:", error);
					alert("فشل حفظ بيانات المستخدم. يرجى المحاولة مرة أخرى.");
				}
			};

			const toggleUserStatus = async (user) => {
				try {
					// In a real application, you would send to the API
					// For now, we'll just update the local array
					const index = users.value.findIndex((u) => u.id === user.id);
					if (index !== -1) {
						users.value[index].isActive = !users.value[index].isActive;
						alert(
							`تم ${
								users.value[index].isActive ? "تفعيل" : "تعطيل"
							} حساب المستخدم بنجاح!`
						);
					}
				} catch (error) {
					console.error("خطأ في تغيير حالة المستخدم:", error);
					alert("فشل تغيير حالة المستخدم. يرجى المحاولة مرة أخرى.");
				}
			};

			const confirmDeleteUser = (user) => {
				userToDelete.value = user;
				showDeleteConfirmation.value = true;
			};

			const deleteUser = async () => {
				try {
					// In a real application, you would send to the API
					// For now, we'll just update the local array
					users.value = users.value.filter(
						(u) => u.id !== userToDelete.value.id
					);
					showDeleteConfirmation.value = false;
					alert("تم حذف المستخدم بنجاح!");
				} catch (error) {
					console.error("خطأ في حذف المستخدم:", error);
					alert("فشل حذف المستخدم. يرجى المحاولة مرة أخرى.");
				}
			};

			// Call loadUsers when switching to the users section
			const handleSectionChange = (section) => {
				activeSection.value = section;
				if (section === "users" && users.value.length === 0) {
					loadUsers();
				}
				if (section === "categories" && mainCategories.value.length === 0) {
					loadCategories();
				}
			};

			// Categories methods
			const loadCategories = async () => {
				loadingCategories.value = true;
				try {
					// Fetch main categories
					const mainCategoriesResponse = await fetch("/api/category", {
						headers: {
							Authorization: `Bearer ${userStore.token}`,
						},
					});

					if (!mainCategoriesResponse.ok)
						throw new Error("فشل تحميل التصنيفات الرئيسية");
					const mainCategoriesData = await mainCategoriesResponse.json();
					console.log("categories API raw response:", mainCategoriesData);
					console.log(
						"categories raw JSON:",
						JSON.stringify(mainCategoriesData)
					);

					// Handle direct array or object with array property
					if (Array.isArray(mainCategoriesData)) {
						mainCategories.value = mainCategoriesData;
					} else {
						mainCategories.value =
							extractArrayFromResponse(mainCategoriesData, "categories") || [];
					}

					// If no categories found, try different property paths
					if (
						mainCategories.value.length === 0 &&
						typeof mainCategoriesData === "object"
					) {
						console.log(
							"Trying to extract categories from response properties"
						);
						// Try all possible property paths
						for (const key in mainCategoriesData) {
							if (Array.isArray(mainCategoriesData[key])) {
								console.log(`Found array in property: ${key}`);
								mainCategories.value = mainCategoriesData[key];
								break;
							}
						}
					}

					console.log("Processed main categories:", mainCategories.value);

					// Fetch sub categories
					const subCategoriesResponse = await fetch("/api/subCategory", {
						headers: {
							Authorization: `Bearer ${userStore.token}`,
						},
					});

					if (!subCategoriesResponse.ok)
						throw new Error("فشل تحميل التصنيفات الفرعية");
					const subCategoriesData = await subCategoriesResponse.json();
					console.log("subCategories API raw response:", subCategoriesData);
					console.log(
						"subCategories raw JSON:",
						JSON.stringify(subCategoriesData)
					);

					// Handle direct array or object with array property
					if (Array.isArray(subCategoriesData)) {
						subCategories.value = subCategoriesData;
					} else {
						subCategories.value =
							extractArrayFromResponse(subCategoriesData, "subCategories") ||
							[];
					}

					// If no subcategories found, try different property paths
					if (
						subCategories.value.length === 0 &&
						typeof subCategoriesData === "object"
					) {
						console.log(
							"Trying to extract subcategories from response properties"
						);
						// Try all possible property paths
						for (const key in subCategoriesData) {
							if (Array.isArray(subCategoriesData[key])) {
								console.log(`Found array in property: ${key}`);
								subCategories.value = subCategoriesData[key];
								break;
							}
						}
					}

					console.log("Processed sub categories:", subCategories.value);

					// CRITICAL: Ensure all objects have the same property names regardless of casing
					// Clone categories to avoid reactive issues
					const normalizedMainCategories = mainCategories.value.map(
						(category) => {
							// Create a new object for each category
							const normalized = { ...category };

							// Ensure id property exists (lowercase)
							if (normalized.Id !== undefined && normalized.id === undefined) {
								normalized.id = normalized.Id;
							}

							// Ensure Name property exists (uppercase)
							if (
								normalized.name !== undefined &&
								normalized.Name === undefined
							) {
								normalized.Name = normalized.name;
							}

							return normalized;
						}
					);

					// Update the reactive mainCategories with normalized values
					mainCategories.value = normalizedMainCategories;

					// Clone subcategories to avoid reactive issues
					const normalizedSubCategories = subCategories.value.map(
						(subCategory) => {
							// Create a new object for each subcategory
							const normalized = { ...subCategory };

							// Ensure id property exists (lowercase)
							if (normalized.Id !== undefined && normalized.id === undefined) {
								normalized.id = normalized.Id;
							}

							// Ensure Name property exists (uppercase)
							if (
								normalized.name !== undefined &&
								normalized.Name === undefined
							) {
								normalized.Name = normalized.name;
							}

							// Critical: Handle CategoryId variations (categoryId, mainCategoryId, CategoryId)
							// Order matters: check most specific to least specific
							if (
								normalized.categoryId !== undefined &&
								normalized.CategoryId === undefined
							) {
								normalized.CategoryId = normalized.categoryId;
							} else if (
								normalized.mainCategoryId !== undefined &&
								normalized.CategoryId === undefined
							) {
								normalized.CategoryId = normalized.mainCategoryId;
							} else if (
								normalized.CategoryID !== undefined &&
								normalized.CategoryId === undefined
							) {
								normalized.CategoryId = normalized.CategoryID;
							}

							return normalized;
						}
					);

					// Update the reactive subCategories with normalized values
					subCategories.value = normalizedSubCategories;

					// DEBUG: Create a direct mapping of category IDs to names
					categoryNameMap.value = {};
					mainCategories.value.forEach((cat) => {
						const id = cat.id || cat.Id;
						const name = cat.Name || cat.name;
						if (id !== undefined && name) {
							// Store mapping for string ID
							categoryNameMap.value[String(id)] = name;
							// Also store for numeric ID just to be safe
							categoryNameMap.value[Number(id)] = name;
						}
					});

					console.log("Category name map:", categoryNameMap.value);

					// Debug the relationships between categories and subcategories
					debugCategoryRelationships(mainCategories.value, subCategories.value);

					console.log("Final mainCategories:", mainCategories.value);
					console.log("Final subCategories:", subCategories.value);
				} catch (error) {
					console.error("خطأ في تحميل التصنيفات:", error);
					alert("فشل تحميل التصنيفات. يرجى المحاولة مرة أخرى.");
				} finally {
					loadingCategories.value = false;
				}
			};

			// Update the addMainCategory method to use a modal
			const addMainCategory = () => {
				selectedCategory.value = { id: 0, name: "" };
				newCategoryName.value = "";
				showCategoryModal.value = true;
			};

			// Update the editMainCategory method to use a modal
			const editMainCategory = (category) => {
				selectedCategory.value = { ...category };
				newCategoryName.value = category.name;
				showCategoryModal.value = true;
			};

			// Update the deleteMainCategory method to use a confirmation modal
			const deleteMainCategory = (category) => {
				selectedCategory.value = { ...category };
				showDeleteCategoryConfirmation.value = true;
			};

			// Update the addSubCategory method to use a modal
			const addSubCategory = () => {
				if (mainCategories.value.length === 0) {
					alert("يجب إضافة تصنيف رئيسي أولاً");
					return;
				}

				selectedSubCategory.value = {
					id: 0,
					name: "",
					mainCategoryId: mainCategories.value[0].id,
				};
				newCategoryName.value = "";
				selectedMainCategoryId.value = mainCategories.value[0].id;
				showSubCategoryModal.value = true;
			};

			// Update the editSubCategory method to use a modal
			const editSubCategory = (subCategory) => {
				selectedSubCategory.value = { ...subCategory };
				newCategoryName.value = subCategory.Name;
				selectedMainCategoryId.value = subCategory.CategoryId;
				showSubCategoryModal.value = true;
			};

			// Update the deleteSubCategory method to use a confirmation modal
			const deleteSubCategory = (subCategory) => {
				selectedSubCategory.value = { ...subCategory };
				showDeleteSubCategoryConfirmation.value = true;
			};

			// Add new methods for saving changes from modals
			const saveCategory = async () => {
				if (!newCategoryName.value.trim()) {
					alert("يرجى إدخال اسم التصنيف");
					return;
				}

				try {
					let response;

					if (selectedCategory.value.id === 0) {
						// Add new category
						response = await fetch("/api/category", {
							method: "POST",
							headers: {
								"Content-Type": "application/json",
								Authorization: `Bearer ${userStore.token}`,
							},
							body: JSON.stringify({ Name: newCategoryName.value }),
						});
					} else {
						// Update existing category
						response = await fetch(
							`/api/category/${selectedCategory.value.id}`,
							{
								method: "PUT",
								headers: {
									"Content-Type": "application/json",
									Authorization: `Bearer ${userStore.token}`,
								},
								body: JSON.stringify({ Name: newCategoryName.value }),
							}
						);
					}

					if (!response.ok) throw new Error("فشل حفظ التصنيف");

					showCategoryModal.value = false;
					loadCategories(); // Reload the categories
				} catch (error) {
					console.error("خطأ في حفظ التصنيف:", error);
					alert("فشل حفظ التصنيف. يرجى المحاولة مرة أخرى.");
				}
			};

			const saveSubCategory = async () => {
				if (!newCategoryName.value.trim()) {
					alert("يرجى إدخال اسم التصنيف الفرعي");
					return;
				}

				try {
					let response;

					if (selectedSubCategory.value.id === 0) {
						// Add new subcategory
						response = await fetch("/api/subCategory", {
							method: "POST",
							headers: {
								"Content-Type": "application/json",
								Authorization: `Bearer ${userStore.token}`,
							},
							body: JSON.stringify({
								Name: newCategoryName.value,
								CategoryId: selectedMainCategoryId.value,
							}),
						});
					} else {
						// Update existing subcategory
						response = await fetch(
							`/api/subCategory/${selectedSubCategory.value.id}`,
							{
								method: "PUT",
								headers: {
									"Content-Type": "application/json",
									Authorization: `Bearer ${userStore.token}`,
								},
								body: JSON.stringify({
									Name: newCategoryName.value,
									CategoryId: selectedMainCategoryId.value,
								}),
							}
						);
					}

					if (!response.ok) throw new Error("فشل حفظ التصنيف الفرعي");

					showSubCategoryModal.value = false;
					loadCategories(); // Reload the categories
				} catch (error) {
					console.error("خطأ في حفظ التصنيف الفرعي:", error);
					alert("فشل حفظ التصنيف الفرعي. يرجى المحاولة مرة أخرى.");
				}
			};

			const confirmDeleteCategory = async () => {
				try {
					const response = await fetch(
						`/api/category/${selectedCategory.value.id}`,
						{
							method: "DELETE",
							headers: {
								Authorization: `Bearer ${userStore.token}`,
							},
						}
					);

					if (!response.ok) throw new Error("فشل حذف التصنيف");

					showDeleteCategoryConfirmation.value = false;
					loadCategories(); // Reload the categories
				} catch (error) {
					console.error("خطأ في حذف التصنيف:", error);
					alert("فشل حذف التصنيف. يرجى المحاولة مرة أخرى.");
				}
			};

			const confirmDeleteSubCategory = async () => {
				try {
					const response = await fetch(
						`/api/subCategory/${selectedSubCategory.value.id}`,
						{
							method: "DELETE",
							headers: {
								Authorization: `Bearer ${userStore.token}`,
							},
						}
					);

					if (!response.ok) throw new Error("فشل حذف التصنيف الفرعي");

					showDeleteSubCategoryConfirmation.value = false;
					loadCategories(); // Reload the categories
				} catch (error) {
					console.error("خطأ في حذف التصنيف الفرعي:", error);
					alert("فشل حذف التصنيف الفرعي. يرجى المحاولة مرة أخرى.");
				}
			};

			return {
				loading,
				loadingUsers,
				loadingCategories,
				services,
				users,
				mainCategories,
				subCategories,
				statusFilter,
				userSearchQuery,
				userRoleFilter,
				selectedService,
				selectedUser,
				showUserModal,
				userToDelete,
				showDeleteConfirmation,
				filteredServices,
				filteredUsers,
				isAdmin,
				activeSection,
				totalServices,
				approvedServices,
				pendingServices,
				totalUsers,
				viewServiceDetails,
				approveService,
				revokeApproval,
				approveServiceAndCloseModal,
				loadUsers,
				searchUsers,
				filterUsersByRole,
				getUserRoleDisplay,
				formatDate,
				editUser,
				openAddUserModal,
				saveUser,
				toggleUserStatus,
				confirmDeleteUser,
				deleteUser,
				handleSectionChange,
				// Categories methods
				loadCategories,
				addMainCategory,
				editMainCategory,
				deleteMainCategory,
				addSubCategory,
				editSubCategory,
				deleteSubCategory,
				showCategoryModal,
				showSubCategoryModal,
				showDeleteCategoryConfirmation,
				showDeleteSubCategoryConfirmation,
				selectedCategory,
				selectedSubCategory,
				newCategoryName,
				selectedMainCategoryId,
				saveCategory,
				saveSubCategory,
				confirmDeleteCategory,
				confirmDeleteSubCategory,
				categoryNameMap,
			};
		},
	};
</script>

<style scoped>
	@keyframes fadeIn {
		from {
			opacity: 0;
			transform: translateY(10px);
		}
		to {
			opacity: 1;
			transform: translateY(0);
		}
	}

	.animate-fadeIn {
		animation: fadeIn 0.8s ease-out;
	}
</style>
