var options = {
	chart: {
		height: 300,
		type: 'area',
		toolbar: {
      show: false,
    },
	},
	dataLabels: {
		enabled: false
	},
	stroke: {
		width: 5,
		curve: 'smooth',
	},
	series: [{
		name: 'Comments',
		data: [30, 20, 80, 40, 70, 50, 110]
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
	colors: ['#EB3333'],
	markers: {
		size: 0,
		opacity: 0.3,
		colors: ['#EB3333'],
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
	document.querySelector("#commentsHistory"),
	options
);

chart.render();