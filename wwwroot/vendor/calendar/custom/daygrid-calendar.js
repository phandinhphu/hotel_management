document.addEventListener('DOMContentLoaded', function() {
    var calendarEl = document.getElementById('dayGrid');

    var calendar = new FullCalendar.Calendar(calendarEl, {
      headerToolbar: {
        left: 'prevYear,prev,next,nextYear today',
        center: 'title',
        right: 'dayGridMonth,dayGridWeek,dayGridDay'
      },
      initialDate: '2021-07-12',
      navLinks: true, // can click day/week names to navigate views
      editable: true,
      dayMaxEvents: true, // allow "more" link when too many events
      events: [
        {
          title: 'All Day Event',
          start: '2021-07-01',
          color: '#ecbe3d'
        },
        {
          title: 'Long Event',
          start: '2021-07-07',
          end: '2021-07-10',
          color: '#f17c55'
        },
        {
          groupId: 999,
          title: 'Birthday',
          start: '2021-07-09T16:00:00',
          color: '#99a6f3'
        },
        {
          groupId: 999,
          title: 'Birthday',
          start: '2021-07-16T16:00:00',
          color: '#ec4f3d'
        },
        {
          title: 'Conference',
          start: '2021-07-11',
          end: '2021-07-13',
          color: '#8eca77'
        },
        {
          title: 'Meeting',
          start: '2021-07-14T10:30:00',
          end: '2021-07-14T12:30:00'
        },
        {
          title: 'Lunch',
          start: '2021-07-16T12:00:00',
          color: '#f1a436'
        },
        {
          title: 'Meeting',
          start: '2021-07-18T14:30:00',
          color: '#34c2d0'
        },
        {
          title: 'Interview',
          start: '2021-07-21T17:30:00',
          color: '#B2D553'
        },
        {
          title: 'Meeting',
          start: '2021-07-22T20:00:00',
          color: '#40cea6'
        },
        {
          title: 'Birthday',
          start: '2021-07-13T07:00:00',
          color: '#f5678b'
        },
        {
          title: 'Click for Google',
          url: 'http://google.com/',
          start: '2021-07-28',
          color: '#98c452'
        },
        {
          title: 'Interview',
          start: '2021-07-20',
          color: '#a770b5'
        },
        {
          title: 'Product Launch',
          start: '2021-07-29',
          color: '#f1a436'
        },
        {
          title: 'Leave',
          start: '2021-07-25',
          color: '#35b3c0'
        }
      ]
    });

    calendar.render();
  });