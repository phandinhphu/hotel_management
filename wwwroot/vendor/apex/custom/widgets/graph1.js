var options = {
	chart: {
		height: 270,
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
		name: 'Tổng thu',
		data: [11, 8, 9, 12, 10, 14, 7]
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
            return + val + " triệu VNĐ";
          }
        }
    },
}

var chart = new ApexCharts(
	document.querySelector("#graph1"),
	options
);

chart.render();