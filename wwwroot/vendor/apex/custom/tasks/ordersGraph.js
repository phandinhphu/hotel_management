var options = {
    chart: {
      height: 290,
      type: 'donut',
    },
    labels: ['Books', 'Gifts', 'Mobiles'],
    series: [60000, 45000, 15000],
    legend: {
      position: 'bottom',
    },
    dataLabels: {
      enabled: false
    },
    stroke: {
      width: 0,
    },
    colors: ['#1090d6', '#ffaf2b', '#fe7f58'],
    tooltip: {
      y: {
        formatter: function(val) {
          return  "$" + val
        }
      }
    },
  }
  var chart = new ApexCharts(
    document.querySelector("#recentOrders"),
    options
  );
  chart.render();