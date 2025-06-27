// Khởi tạo biểu đồ hiệu suất làm việc
function initPerformanceChart(data) {
    const options = {
        chart: {
            height: 300,
            type: 'area',
            toolbar: { show: false }
        },
        dataLabels: { enabled: false },
        stroke: {
            curve: 'smooth',
            width: 3
        },
        series: [{
            name: 'Số lượt xử lý',
            data: data.values
        }],
        grid: {
            borderColor: '#e0e6ed',
            strokeDashArray: 5,
            xaxis: { lines: { show: true } },
            yaxis: { lines: { show: false } },
            padding: {
                top: 0,
                right: 20,
                bottom: 10,
                left: 20
            }
        },
        xaxis: {
            categories: data.categories
        },
        yaxis: {
            labels: { show: false }
        },
        colors: ['#28a745'],
        markers: {
            size: 0,
            opacity: 0.3,
            colors: ['#28a745'],
            strokeColor: "#ffffff",
            strokeWidth: 2,
            hover: { size: 7 }
        },
        tooltip: {
            y: {
                formatter: function (val) {
                    return val + " lượt";
                }
            }
        }
    };

    const chart = new ApexCharts(
        document.querySelector("#PerformanceChart"),
        options
    );

    chart.render();
}