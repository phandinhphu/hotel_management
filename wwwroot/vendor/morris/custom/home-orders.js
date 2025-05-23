// Morris Donut
Morris.Donut({
	element: 'donutOrders',
	data: [
		{value: 15, label: 'New'},
		{value: 10, label: 'Delivered'},
		{value: 7, label: 'Pending'},
	],
	backgroundColor: '#ffffff',
	labelColor: '#13202b',
	lineWidth: '10px',
	colors:['#435EEF', '#59a2fb', '#8ec0fd'],
	resize: true,
	hideHover: "auto",
	formatter: function (x) { return x}
});