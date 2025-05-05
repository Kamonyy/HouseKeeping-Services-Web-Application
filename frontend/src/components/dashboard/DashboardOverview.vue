<template>
	<div class="space-y-6">
		<h1 class="text-3xl font-bold text-gray-800">لوحة التحكم الرئيسية</h1>
		<div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
			<div class="bg-white p-6 rounded-lg shadow-md border-r-4 border-blue-500">
				<h3 class="text-lg font-semibold text-gray-700">إجمالي الخدمات</h3>
				<p class="text-3xl font-bold text-blue-600">{{ totalServices }}</p>
			</div>
			<div
				class="bg-white p-6 rounded-lg shadow-md border-r-4 border-green-500"
			>
				<h3 class="text-lg font-semibold text-gray-700">الخدمات المعتمدة</h3>
				<p class="text-3xl font-bold text-green-600">{{ approvedServices }}</p>
			</div>
			<div
				class="bg-white p-6 rounded-lg shadow-md border-r-4 border-yellow-500"
			>
				<h3 class="text-lg font-semibold text-gray-700">بانتظار الموافقة</h3>
				<p class="text-3xl font-bold text-yellow-600">{{ pendingServices }}</p>
			</div>
			<div
				class="bg-white p-6 rounded-lg shadow-md border-r-4 border-purple-500"
			>
				<h3 class="text-lg font-semibold text-gray-700">إجمالي المستخدمين</h3>
				<p class="text-3xl font-bold text-purple-600">{{ totalUsers }}</p>
			</div>
		</div>
	</div>
</template>

<script>
	export default {
		name: "DashboardOverview",
		props: {
			totalServices: {
				type: Number,
				required: true,
			},
			approvedServices: {
				type: Number,
				required: true,
			},
			pendingServices: {
				type: Number,
				required: true,
			},
			totalUsers: {
				type: Number,
				required: true,
			},
		},
	};
</script>
