var options = {
  	series: [{
      	name: "Male",
      	data: [5, 10, 15, 20, 25, 30, 35, 40, 60, 70, 80, 90, 25, 20, 15, 10, 5],
    },{
      	name: "Female",
      	data: [-5, -10, -15, -20, -25, -30, -35, -40, -60, -70, -80, -90, -25, -20, -15, -10, -5],
    },],
  	chart: {
		type: "bar",
		height: 300,
		stacked: true,
		toolbar: {
      		show: false,
    	},
 	},
	 colors: ["#435EEF", "#EB3333"],
  	dataLabels: {
    	enabled: false,
  	},
  	stroke: {
    	width: 0,
  	},
  	grid: {
    	xaxis: {
      		lines: {
        		show: false,
      		},
    	},
  	},
  	yaxis: {
    	show: false,
  	},
	plotOptions: {
		bar: {
		  	horizontal: false,
		  	borderRadius: 4,
		},
	},
  	tooltip: {
		shared: false,
		x: {
			formatter: function (val) {
				return val;
			},
		},
		y: {
			formatter: function (val) {
				return Math.abs(val) + "%";
			},
		},
  	},
  	xaxis: {
    	categories: [
			"5-9",
			"10-14",
			"15-19",
			"20-24",
			"25-29",
			"30-34",
			"35-39",
			"40-44",
			"45-49",
			"50-54",
			"55-59",
			"60-64",
			"65-69",
			"70-74",
			"75-79",
			"80-84",
			"85+",
    	],
    	labels: {
      		formatter: function (val) {
        		return Math.abs(Math.round(val)) + "%";
      		},
    	},
  	},
};

var chart = new ApexCharts(
  	document.querySelector("#instagramFollowers"),
  	options
);
chart.render();
