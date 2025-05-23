var options = {
	series: [
	{
	  name: 'Completed',
	  data: [
		{
		  x: 'Mobile App',
		  y: 12,
		  goals: [
			{
			  name: 'Expected',
			  value: 14,
			  strokeWidth: 2,
			  strokeDashArray: 2,
			  strokeColor: '#FFC424'
			}
		  ]
		},
		{
		  x: 'Landing Page',
		  y: 44,
		  goals: [
			{
			  name: 'Expected',
			  value: 54,
			  strokeWidth: 5,
			  strokeHeight: 10,
			  strokeColor: '#FFC424'
			}
		  ]
		},
		{
		  x: 'Dashboard Design',
		  y: 54,
		  goals: [
			{
			  name: 'Expected',
			  value: 52,
			  strokeWidth: 10,
			  strokeHeight: 0,
			  strokeLineCap: 'round',
			  strokeColor: '#FFC424'
			}
		  ]
		},
		{
		  x: 'Food Order App',
		  y: 81,
		  goals: [
			{
			  name: 'Expected',
			  value: 66,
			  strokeWidth: 10,
			  strokeHeight: 0,
			  strokeLineCap: 'round',
			  strokeColor: '#FFC424'
			}
		  ]
		},
		{
		  x: 'Can Rent App',
		  y: 67,
		  goals: [
			{
			  name: 'Expected',
			  value: 70,
			  strokeWidth: 5,
			  strokeHeight: 10,
			  strokeColor: '#FFC424'
			}
		  ]
		}
	  ]
	}
  ],
	chart: {
	height: 235,
	type: 'bar',
	toolbar: {
        show: false,
      },
  },
  plotOptions: {
	bar: {
	  horizontal: true,
	}
  },
  colors: ['#435EEF'],
  dataLabels: {
	formatter: function(val, opt) {
	  const goals =
		opt.w.config.series[opt.seriesIndex].data[opt.dataPointIndex]
		  .goals
  
	  if (goals && goals.length) {
		return `${val} / ${goals[0].value}`
	  }
	  return val
	}
  },
  legend: {
	show: true,
	showForSingleSeries: true,
	customLegendItems: ['Completed', 'Expected'],
	markers: {
	  fillColors: ['#435EEF', '#FFC424']
	}
  }
  };

  var chart = new ApexCharts(document.querySelector("#projectAnalytics"), options);
  chart.render();