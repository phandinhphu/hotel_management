var options = {
	chart: {
	  height: 110,
	  width: '85%',
	  type: 'area',
	  zoom: {
		enabled: false
	  },
	  toolbar: {
		show: false
	  },
	},
	dataLabels: {
	  enabled: false
	},
	stroke: {
	  curve: 'smooth',
	  width: 7,
	},
	series: [{
	  name: "Notifications",
	  data: [80, 300, 300, 50, 150, 170, 550, 500]
	}],
	grid: {
	  borderColor: '#bede68',
	  strokeDashArray: 0,
	  show: false,
	  xaxis: {
		lines: {
		  show: false,
		}
	  },   
	  yaxis: {
		lines: {
		  show: false,
		} 
	  },
	  padding: {
		top: 0,
		right: 10,
		bottom: 0,
		left: 10
	  }, 
	},
	xaxis: {
	  labels: {
		show: false,
	  },
	},
	yaxis: {
	  show: false,
	},
	fill: {
	  type:"gradient",
	  gradient: {
		type: "vertical",
		shadeIntensity: 1,
		inverseColors: !1,
		opacityFrom: .4,
		opacityTo: 0,
		stops: [15, 100]
	  }
	},
	colors: ['#ffffff'],
	markers: {
	  size: 0,
	  opacity: 0.2,
	  colors: ["#ffffff"],
	  strokeColor: "#49914E",
	  strokeWidth: 2,
	  hover: {
		size: 10,
	  }
	},
	tooltip: {
	  y: {
		formatter: function(val) {
		  return val
		}
	  }
	},
  }
  
  var chart = new ApexCharts(
	document.querySelector("#notificationsGraph"),
	options
  );
  
  chart.render();

  













// var options1 = {
// 	series: [{
// 		data: [10, 20, 30, 40, 30, 50, 70]
// }],
// 	chart: {
// 		type: 'area',
// 		width: 150,
// 		height: 80,
// 		sparkline: {
// 			enabled: true
// 		}
// 	},
// 	colors: ['#ffffff'],
// 	stroke: {
// 		curve: 'smooth',
// 		width: 3,
// 	},
// 	tooltip: {
// 		fixed: {
// 			enabled: false
// 		},
// 		x: {
// 			show: false
// 		},	
// 		marker: {
// 			show: false
// 		}
// 	},
// 	tooltip: {
// 		y: {
// 			formatter: function(val) {
// 				return val
// 			}
// 		}
// 	},
// };

// var chart1 = new ApexCharts(document.querySelector("#notificationsGraph"), options1);
// chart1.render();
