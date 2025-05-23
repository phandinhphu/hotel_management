var options = {
	series: [
		
	{
		name: 'In Millions',
	  data: [
		{
		  	x: 'United States',
		  	y: 218
		},		
		{
		  	x: 'India',
		  	y: 197
		},
		{
			x: 'Brazil',
			y: 149
	  	},
		{
		  	x: 'Turkey',
		  	y: 55
		},
		{
		  	x: 'Indonesia',
		  	y: 84
		},
		{
		  	x: 'United Kingdom',
		  	y: 31
		},
		{
		  	x: 'Saudi Arabia',
		  	y: 70
		},
		{
		  	x: 'Germany',
		  	y: 30
		},
		{
		  	x: 'Mexico',
		  	y: 44
		},
		{
		  	x: 'France',
		  	y: 68
		},
		{
		  	x: 'Vietnam',
		  	y: 28
		},
		{
		  	x: 'Hungary',
		  	y: 19
		},
		{
		  	x: 'Philippines',
		  	y: 29
		}
	  ]
	}
  	],
	legend: {
		show: false
  	},
  	chart: {
		height: 270,
		type: 'treemap',
		toolbar: {
			show: false,
	  },
  	},
	  colors: ["#435EEF", "#EB3333"],
	  tooltip: {
        y: {
            formatter: function(val) {
                return '$' + val + " Million"
            }
        }
    },
  };

  var chart = new ApexCharts(document.querySelector("#adsRevenue"), options);
  chart.render();