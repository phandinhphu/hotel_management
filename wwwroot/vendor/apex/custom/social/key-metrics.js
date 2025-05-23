var options = {
  series: [
    {
      name: "Actual",
      data: [
        {
          x: "Followers",
          y: 5000,
          goals: [
            {
              name: "Expected",
              value: 3000,
              strokeHeight: 10,
              strokeWidth: 5,
              strokeColor: "#BBBBBB",
            },
          ],
        },
        {
          x: "Tweets",
          y: 7000,
          goals: [
            {
              name: "Expected",
              value: 5000,
              strokeHeight: 10,
              strokeWidth: 5,
              strokeColor: "#BBBBBB",
            },
          ],
        },
        {
          x: "ReTweets",
          y: 9000,
          goals: [
            {
              name: "Expected",
              value: 7000,
              strokeHeight: 10,
              strokeWidth: 5,
              strokeColor: "#BBBBBB",
            },
          ],
        },
        {
          x: "Clicks",
          y: 3000,
          goals: [
            {
              name: "Expected",
              value: 3000,
              strokeHeight: 10,
              strokeWidth: 5,
              strokeColor: "#BBBBBB",
            },
          ],
        },
        {
          x: "Likes",
          y: 5000,
          goals: [
            {
              name: "Expected",
              value: 5500,
              strokeHeight: 10,
              strokeWidth: 5,
              strokeColor: "#BBBBBB",
            },
          ],
        },
        {
          x: "Shares",
          y: 6500,
          goals: [
            {
              name: "Expected",
              value: 7000,
              strokeHeight: 10,
              strokeWidth: 5,
              strokeColor: "#BBBBBB",
            },
          ],
        },
        {
          x: "Replies",
          y: 3500,
          goals: [
            {
              name: "Expected",
              value: 4000,
              strokeHeight: 10,
              strokeWidth: 5,
              strokeColor: "#BBBBBB",
            },
          ],
        },
      ],
    },
  ],
  chart: {
    height: 270,
    type: "bar",
    toolbar: {
      show: false,
    },
  },
  plotOptions: {
    bar: {
      horizontal: false,
      columnWidth: "70%",
    },
  },
  dataLabels: {
    enabled: false,
  },
  stroke: {
    show: false,
  },
  colors: ["#435EEF"],
  yaxis: {
    show: false,
  },
  legend: {
    show: true,
    showForSingleSeries: true,
    customLegendItems: ["Actual", "Expected"],
    markers: {
      fillColors: ["#435EEF", "#BBBBBB"],
    },
  },
};

var chart = new ApexCharts(document.querySelector("#keyMetrics"), options);
chart.render();
