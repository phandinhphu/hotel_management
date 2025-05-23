var options = {
	chart: {
		height: 285,
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
        name: 'Khách hàng mới',
        data: [50, 45, 60, 55, 60, 50, 70]
    }, {
        name: 'Khách hàng cũ',
        data: [23, 30, 45, 30, 40, 30, 40]
    }],
	grid: {
    borderColor: '#ffffff',
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
		right: 30,
		bottom: 0,
		left: 30
	}, 
  	},
	xaxis: {
		type: 'day',
        categories: ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"],
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
			format: 'dd/MM/yy HH:mm'
		},
		y: {
			formatter: function(val) {
				return val + ' khách hàng';
			}
		}
		
	}
}

var chart = new ApexCharts(
	document.querySelector("#deals"),
	options
);

chart.render();