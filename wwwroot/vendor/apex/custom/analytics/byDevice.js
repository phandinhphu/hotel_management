var options = {
  chart: {
    height: 310,
    type: 'donut',
  },
  labels: ['Phòng Deluxe', 'Phòng King', 'Phòng Queen', 'Phòng Đơn', 'Phòng Penthouse', 'Phòng Kểt Nối', 'Phòng Valentine'],
  series: [44, 55, 41, 17, 15, 10, 20],
  legend: {
    position: 'bottom',
  },
  dataLabels: {
    enabled: false
  },
  stroke: {
    width: 8,
    colors: ['#ffffff'],
  },
  colors: ['#FF4560', '#775DD0', '#3F51B5', '#546E7A', '#D32F2F', '#FFA000', '#4CAF50'],
  tooltip: {
    y: {
      formatter: function(val) {
        return val + "%"
      }
    }
  },
}
var chart = new ApexCharts(
  document.querySelector("#byDevice"),
  options
);
chart.render();