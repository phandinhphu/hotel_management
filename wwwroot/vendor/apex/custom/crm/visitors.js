var options = {
	chart: {
		height: 235,
		type: 'area',
		toolbar: {
      		show: false,
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
		name: 'Visitors',
		data: [10, 40, 15, 40, 35, 96, 69]
	}],
	grid: {
    	borderColor: '#e0e6ed',
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
			bottom: 10,
			left: 20
		},
  	},
	xaxis: {
		categories: ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'],
	},
	yaxis: {
		labels: {
			show: false,
		}
	},
	colors: ['#435EEF'],
	markers: {
		size: 0,
		opacity: 0.3,
		colors: ['#435EEF'],
		strokeColor: "#ffffff",
		strokeWidth: 2,
		hover: {
			size: 7,
		}
	},
    tooltip: {
        y: {
          	formatter: function(val) {
            	return + val + "k"
          	}
        }
    },
}

var chart = new ApexCharts(
	document.querySelector("#visitorsGraph"),
	options
);

chart.render();