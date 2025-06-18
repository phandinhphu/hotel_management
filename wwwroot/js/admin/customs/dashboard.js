
// Revenue Chart 
function initRevenueChart(data) {
    // Use provided data or fallback to default
    console.log("Data", data);

    const options = {
        chart: {
            height: 270,
            type: 'area',
            toolbar: { show: false }
        },
        dataLabels: { enabled: false },
        stroke: {
            curve: 'smooth',
            width: 3
        },
        series: [{
            name: 'Doanh thu',
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
        colors: ['#435EEF'],
        markers: {
            size: 0,
            opacity: 0.3,
            colors: ['#435EEF'],
            strokeColor: "#ffffff",
            strokeWidth: 2,
            hover: { size: 7 }
        },
        tooltip: {
            y: {
                formatter: function (val) {
                    return val + " triệu VNĐ";
                }
            }
        }
    };

    const chart = new ApexCharts(
        document.querySelector("#RevenueChart"),
        options
    );

    chart.render();
}

// Booking Chart
function initBookingChart(data) {
    // Use provided data or fallback to default
    const chartData = data;

    const options = {
        chart: {
            height: 280,
            type: 'bar',
            toolbar: { show: false }
        },
        plotOptions: {
            bar: {
                columnWidth: '50%',
                dataLabels: {
                    position: 'top'
                }
            }
        },
        series: [{
            name: 'Số lượt',
            data: chartData.values
        }],
        xaxis: {
            categories: chartData.categories,
            axisBorder: { show: false },
            tooltip: { enabled: true },
            labels: {
                show: true,
                rotate: -45,
                rotateAlways: true
            }
        },
        yaxis: {
            axisBorder: { show: false },
            axisTicks: { show: false }
        },
        grid: {
            borderColor: '#e0e6ed',
            strokeDashArray: 5,
            xaxis: { lines: { show: true } },
            yaxis: { lines: { show: false } },
            padding: {
                top: 0,
                right: 0,
                bottom: 0
            }
        },
        tooltip: {
            y: {
                formatter: function (val) {
                    return val;
                }
            }
        },
        colors: ['#435EEF', '#999999']
    };

    const chart = new ApexCharts(
        document.querySelector("#BookingChart"),
        options
    );

    chart.render();
}

// Room Type Chart
function initRoomTypeChart(data) {
    // Use provided data or fallback to default
    const chartData = data;

    const options = {
        chart: {
            height: 310,
            type: 'donut'
        },
        labels: chartData.labels,
        series: chartData.values,
        legend: {
            position: 'bottom'
        },
        dataLabels: {
            enabled: false
        },
        stroke: {
            width: 8,
            colors: ['#ffffff']
        },
        colors: ['#FF4560', '#775DD0', '#3F51B5', '#546E7A', '#D32F2F', '#FFA000', '#4CAF50'],
        tooltip: {
            y: {
                formatter: function (val) {
                    return val + "%"
                }
            }
        }
    };

    const chart = new ApexCharts(
        document.querySelector("#RoomTypeChart"),
        options
    );

    chart.render();
}

// Date filter functionality
function initDateFilter() {
    const applyFilterBtn = document.getElementById('applyFilter');
    const resetFilterBtn = document.getElementById('resetFilter');
    const fromDateInput = document.getElementById('fromDate');
    const toDateInput = document.getElementById('toDate');

    if (applyFilterBtn) {
        applyFilterBtn.addEventListener('click', function () {
            const fromDate = fromDateInput.value;
            const toDate = toDateInput.value;

            if (fromDate && toDate) {
                window.location.href = `/Admin/Dashboard/Index?startDate=${fromDate}&endDate=${toDate}`;
            }
        });
    }

    if (resetFilterBtn) {
        resetFilterBtn.addEventListener('click', function () {
            window.location.href = '/Admin/Dashboard/Index';
        });
    }
}

// Initialize date filter
document.addEventListener('DOMContentLoaded', function () {
    initDateFilter();
});
