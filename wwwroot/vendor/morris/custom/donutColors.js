// Morris Donut
Morris.Donut({
	element: 'donutColors',
	data: [
		{value: 30, label: 'foo'},
		{value: 15, label: 'bar'},
		{value: 10, label: 'baz'},
		{value: 5, label: 'A really really long label'}
	],
	backgroundColor: '#ffffff',
	labelColor: '#ffffff',
	colors:['#435EEF', '#59a2fb', '#8ec0fd', '#c7e0ff'],
	resize: true,
	hideHover: "auto",
	gridLineColor: "#dddddd",
	formatter: function (x) { return x + "%"}
});