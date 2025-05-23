var options = {
	chart: {
		height: 235,
		type: 'area',
		toolbar: {
			show: false,
		},
		dropShadow: {
			enabled: true,
			opacity: 0.1,
			blur: 5,
			left: -10,
			top: 10
		},
	},
	dataLabels: {
		enabled: false
	},
	stroke: {
		curve: 'smooth',
		width: 3
	},
	series: [{
        name: 'In Progess',
        data: [10, 41, 35, 51, 49, 21, 37]
    }, {
        name: 'Completed',
        data: [7, 23, 20, 37, 40, 12, 22]
    }],
	grid: {
		borderColor: '#ededf3',
		strokeDashArray: 5,
		xaxis: {
      		lines: {
        		show: true
      		}
    	},   
		yaxis: {
			lines: {
				show: false,
			}
		},
		padding: {
			top: 0,
			right: 20,
			bottom: 0,
			left: 20
		}, 
  	},
	xaxis: {
		type: 'day',
        categories: ["Sun", "Mon", "Tue", "Wedn", "Thu", "Fri", "Sat"],
	},
	colors: ['#435EEF', '#59a2fb'],
	yaxis: {
		show: false,
	},
	markers: {
		size: 0,
		opacity: 0.2,
		colors: ['#435EEF', '#59a2fb'],
		strokeColor: "#fff",
		strokeWidth: 2,
		hover: {
			size: 7,
		}
	},
	tooltip: {
		x: {
			format: 'dd/MM/yy'
		},
	}
}

var chart = new ApexCharts(
	document.querySelector("#taskAnalytics"),
	options
);

chart.render();