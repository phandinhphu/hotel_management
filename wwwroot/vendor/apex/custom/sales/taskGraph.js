var options = {
	chart: {
		height: 310,
		type: 'radialBar',
		toolbar: {
			show: false,
		},
		dropShadow: {
			enabled: true,
			opacity: 0.2,
			blur: 10,
			left: 10,
			top: 10
		},
	},
	plotOptions: {
		radialBar: {
			dataLabels: {
				name: {
					fontSize: '12px',
					fontColor: 'black',
          fontFamily: 'Merriweather',
				},
				value: {
					fontSize: '21px',
					fontFamily: 'Merriweather',
				},
				total: {
					show: true,
					label: 'Tasks',
					formatter: function (w) {
						// By default this function returns the average of all series. The below is just an example to show the use of custom formatter function
						return '19'
					}
				}
			}
		}
	},
	series: [80, 70, 10],
	labels: ['New', 'Completed', 'Pending'],
	colors: ['#435EEF', '#149865', '#383737'],
}

var chart = new ApexCharts(
	document.querySelector("#taskGraph"),
	options
);
chart.render();