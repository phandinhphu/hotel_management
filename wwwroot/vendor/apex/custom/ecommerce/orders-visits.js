var options = {
	chart: {
		height: 300,
		type: 'line',
		stacked: false,
		toolbar: {
			show: false,
		},
	},
	dataLabels: {
		enabled: false
	},
	series: [{
		name: 'Orders',
		type: 'column',
		data: [40, 25, 29, 56, 62, 87, 85, 79, 49, 65, 75, 90]
	},{
		name: 'Visits',
		type: 'column',
		data: [25, 35, 11, 47, 51, 94, 56, 87, 52, 73, 64, 85]
	}],
	stroke: {
		width: [0, 0],
	},
	colors: ['#435EEF', '#59a2fb', '#262b31', '#63686f', '#868a90'],
	xaxis: {
		categories: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'June', 'July', 'Aug', 'Sept', 'Oct', 'Nov', 'Dec'],
	},
	legend: {
		horizontalAlign: 'center',
		offsetY: 10
	}
}

var chart = new ApexCharts(
	document.querySelector("#ordersVisits"),
	options
);
chart.render();