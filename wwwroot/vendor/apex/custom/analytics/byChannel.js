var options = {
	chart: {
	  height: 250,
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
	dataLabels: {
	  enabled: false
	},
	grid: {
	  borderColor: '#ffffff',
	  strokeDashArray: 5,
	  xaxis: {
		lines: {
		  show: false,
		}
	  },   
	  yaxis: {
		lines: {
		  show: true,
		} 
	  },
	  padding: {
		top: 0,
		right: 0,
		bottom: 0,
		left: 0
	  },
	},
	series: [{
	  name: 'Tổng thu',
	  data: [10, 14, 12, 16, 9]
	}],
	colors: ['#435EEF', '#59a2fb'],
	xaxis: {
	  categories: ["Giặt đồ", "Đồ ăn", "Dịch vụ", "Phòng", "Khác"],
	},
	tooltip: {
	  y: {
		formatter: function(val) {
		  return val + ' triệu VNĐ'
		}
	  }
	},
  }
  
  var chart = new ApexCharts(
	document.querySelector("#byChannel"),
	options
  );
  
  chart.render();