var options = {
	chart: {
		height: 320,
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
        name: 'Claimed',
        data: [3000, 1000, 3000, 1000]
    }, {
        name: 'Expired',
        data: [1500, 3500, 1500, 3500]
    }],
	grid: {
    borderColor: '#e1e1e1',
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
        categories: ["Q1", "Q2", "Q3", "Q4"],
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
	document.querySelector("#graph4"),
	options
);

chart.render();