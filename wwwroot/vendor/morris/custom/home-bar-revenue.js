// Morris Bar No Axis
Morris.Bar({
	element: 'barRevenue',
	data: [
		{x: 'Q1', y: 3000, z: 3500},
		{x: 'Q2', y: 4000, z: 4500},
		{x: 'Q3', y: 5000, z: 5500},
		{x: 'Q4', y: 6000, z: 6500},
	],
	xkey: 'x',
	ykeys: ['y', 'z'],
	labels: ['Revenue', 'Profit'],
	resize: true,
	hideHover: "auto",
	gridLineColor: "#dddddd",
	barColors:['#435EEF', '#59a2fb', '#8ec0fd'],
});