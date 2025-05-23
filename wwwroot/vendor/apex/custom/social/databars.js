// Option 1
var options1 = {
  series: [
    {
      name: "Followers",
      data: [4000, 5000, 6000, 8000, 3000],
    },
  ],
  chart: {
    type: "bar",
    height: 210,
    toolbar: {
      show: false,
    },
  },
  plotOptions: {
    bar: {
      horizontal: true,
      dataLabels: {
        position: "top",
      },
    },
  },
  dataLabels: {
    enabled: true,
    offsetX: -6,
    style: {
      fontSize: "13px",
      colors: ["#ffffff"],
    },
  },
  colors: ["#1877f2"],
  tooltip: {
    y: {
      formatter: function (val) {
        return "$" + val + " Million";
      },
    },
  },
  xaxis: {
    categories: ["Day 1", "Day 2", "Day 3", "Day 4", "Day 5"],
  },
};

var chart = new ApexCharts(document.querySelector("#databar1"), options1);
chart.render();

















// Option 2
var options2 = {
  series: [
    {
      name: "Followers",
      data: [4000, 5000, 6000, 8000, 3000],
    },
  ],
  chart: {
    type: "bar",
    height: 210,
    toolbar: {
      show: false,
    },
  },
  plotOptions: {
    bar: {
      horizontal: true,
      dataLabels: {
        position: "top",
      },
    },
  },
  dataLabels: {
    enabled: true,
    offsetX: -6,
    style: {
      fontSize: "13px",
      colors: ["#ffffff"],
    },
  },
  colors: ["#1da1f2"],
  tooltip: {
    y: {
      formatter: function (val) {
        return "$" + val + " Million";
      },
    },
  },
  xaxis: {
    categories: ["Day 1", "Day 2", "Day 3", "Day 4", "Day 5"],
  },
};

var chart = new ApexCharts(document.querySelector("#databar2"), options2);
chart.render();












// Option 3
var options3 = {
  series: [
    {
      name: "Followers",
      data: [4000, 5000, 6000, 8000, 3000],
    },
  ],
  chart: {
    type: "bar",
    height: 210,
    toolbar: {
      show: false,
    },
  },
  plotOptions: {
    bar: {
      horizontal: true,
      dataLabels: {
        position: "top",
      },
    },
  },
  dataLabels: {
    enabled: true,
    offsetX: -6,
    style: {
      fontSize: "13px",
      colors: ["#ffffff"],
    },
  },
  colors: ["#bb01af"],
  tooltip: {
    y: {
      formatter: function (val) {
        return "$" + val + " Million";
      },
    },
  },
  xaxis: {
    categories: ["Day 1", "Day 2", "Day 3", "Day 4", "Day 5"],
  },
};

var chart = new ApexCharts(document.querySelector("#databar3"), options3);
chart.render();











// Option 4
var options4 = {
  series: [
    {
      name: "Subscribers",
      data: [4000, 5000, 6000, 8000, 3000],
    },
  ],
  chart: {
    type: "bar",
    height: 210,
    toolbar: {
      show: false,
    },
  },
  plotOptions: {
    bar: {
      horizontal: true,
      dataLabels: {
        position: "top",
      },
    },
  },
  dataLabels: {
    enabled: true,
    offsetX: -6,
    style: {
      fontSize: "13px",
      colors: ["#ffffff"],
    },
  },
  colors: ["#cc181e"],
  tooltip: {
    y: {
      formatter: function (val) {
        return "$" + val + " Million";
      },
    },
  },
  xaxis: {
    categories: ["Day 1", "Day 2", "Day 3", "Day 4", "Day 5"],
  },
};

var chart = new ApexCharts(document.querySelector("#databar4"), options4);
chart.render();
