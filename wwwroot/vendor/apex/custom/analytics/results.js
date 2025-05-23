var options = {
    series: [{
    name: 'Direct',
    type: 'area',
    data: [640, 505, 770, 415]
    }, {
        name: 'Online',
        type: 'line',
        data: [540, 405, 670, 315]
    }],
    chart: {
        height: 280,
        type: 'line',
        zoom: {
            enabled: false
        },
        toolbar: {
            show: false
        },
    },
    colors: ['#435EEF', '#FFC424'],
    stroke: {
        width: [0, 4],
        curve: 'smooth',
    },
    tooltip: {
        y: {
            formatter: function(val) {
                return + val + "k"
            }
        }
    },
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
            bottom: 0,
            left: 20
        },
    },
    dataLabels: {
        enabled: true,
        enabledOnSeries: [1]
    },
    labels: ['Q1', 'Q2', 'Q3', 'Q4'],
        xaxis: {
            type: 'day'
        },
    };

var chart = new ApexCharts(document.querySelector("#results"), options);
chart.render();