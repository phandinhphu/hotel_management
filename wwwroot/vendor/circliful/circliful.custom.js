$( document ).ready(function() {

	$("#weeklyEarnings").circliful({
		animation: 1,
		animationStep: 5,
		foregroundBorderWidth: 25,
		backgroundBorderWidth: 25,
		percent: 100,
		textStyle: 'font-size: 12px;',
		fontColor: '#1F314E',
		foregroundColor: '#F5BE2B',
		backgroundColor: 'rgba(0, 0, 0, 0.1)',
		multiPercentage: 1,
		percentages: [10, 20, 30],
	});

	$("#monthlyEarnings").circliful({
		animation: 1,
		animationStep: 5,
		foregroundBorderWidth: 25,
		backgroundBorderWidth: 25,
		percent: 100,
		fontColor: '#1F314E',
		foregroundColor: '#2CA757',
		backgroundColor: 'rgba(0, 0, 0, 0.1)',
		multiPercentage: 1,
		percentages: [10, 20, 30]
	});	

	$("#overallSales").circliful({
		animation: 1,
		animationStep: 5,
		foregroundBorderWidth: 25,
		backgroundBorderWidth: 25,
		percent: 100,
		fontColor: '#1F314E',
		foregroundColor: '#435EEF',
		backgroundColor: 'rgba(0, 0, 0, 0.1)',
		multiPercentage: 1,
		percentages: [10, 20, 30]
	});

	$("#overallExpenses").circliful({
		animation: 1,
		animationStep: 5,
		foregroundBorderWidth: 25,
		backgroundBorderWidth: 25,
		percent: 80,
		fontColor: '#1F314E',
		foregroundColor: '#435EEF',
		backgroundColor: 'rgba(0, 0, 0, 0.1)',
		multiPercentage: 1,
		percentages: [10, 20, 30]
	});

	$("#overallIncome").circliful({
		animation: 1,
		animationStep: 5,
		foregroundBorderWidth: 25,
		backgroundBorderWidth: 25,
		percent: 70,
		fontColor: '#1F314E',
		foregroundColor: '#149865',
		backgroundColor: 'rgba(0, 0, 0, 0.1)',
		multiPercentage: 1,
		percentages: [10, 20, 30]
	});


});