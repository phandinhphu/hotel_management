document.addEventListener('DOMContentLoaded', function() {
	var calendarEl = document.getElementById('customScheduleCal');

	var calendar = new FullCalendar.Calendar(calendarEl, {
		headerToolbar: {
			left: 'prev, next, today, dayGridMonth',
			center: '',
			right: 'title'
		},
		initialDate: '2021-12-12',
		navLinks: true, // can click day/week names to navigate views
		businessHours: true, // display business hours
		editable: true,
		selectable: true,
        contentHeight: "auto",
		events: [
			{
				title: '5',
				start: '2021-12-03',
				color: '#EB3333'
			},
			{
				title: '3',
				start: '2021-12-04',
				color: '#149865'
			},
			{
				title: '7',
				start: '2021-12-05',
			},
			{
				title: '3',
				start: '2021-12-08',
			},
			{
				title: '9',
				start: '2021-12-10',
				color: '#EB3333'
			},
			{
				title: '4',
				start: '2021-12-12',
				color: '#149865'
			},
			{
				title: '7',
				start: '2021-12-15',
				color: '#FFC424'
			},
			{
				title: '2',
				start: '2021-12-16',
			},			
			{
				title: '8',
				start: '2021-12-18',
				color: '#EB3333'
			},
			{
				title: '6',
				start: '2021-12-21',
				color: '#149865'
			},
			{
				title: '4',
				start: '2021-12-23',
				color: '#FFC424'
			},
			{
				title: '7',
				start: '2021-12-25',
			},
		]
	});

	calendar.render();
});