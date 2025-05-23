// Markers on the world map
$(function(){
	$('#world-map-markers').vectorMap({
		map: 'world_mill_en',
		normalizeFunction: 'polynomial',
		hoverOpacity: 0.7,
		hoverColor: false,
		zoomOnScroll: false,
		markerStyle: {
			initial: {
				fill: '#FFC424',
				stroke: '#FFC424',
				r: 10
			}
		},
		zoomMin: 1,
		hoverColor: true,
		series: {
			regions: [{
				values: gdpData,
				scale: ['#435EEF', '#59a2fb'],
				normalizeFunction: 'polynomial'
			}]
		},
		backgroundColor: 'transparent',
		markers: [
			{latLng: [37.30, -119.30], name: 'California, CA'},
			{latLng: [28.61, 77.20], name: 'New Delhi'},
			{latLng: [33.55, 18.22], name: 'Cape Town'},
			{latLng: [61.52, 105.31], name: 'Moscow'},
			{latLng: [51.50, 0.12], name: 'London'},
			{latLng: [14.23, 51.92], name: 'Brazil'},
		]
	});
});