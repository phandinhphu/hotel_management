// Remove Product
$(".remove-from-cart").on('click', function () {
	$(this).closest('.col-12').remove();
});

// Add to Cart and Remove From Cart
$('.addToCart').on('click', function () {
	var $this = $(this);
	$this.toggleClass('addToCart');
	if($this.hasClass('addToCart')){
		$this.text('Add To Cart');
		$this.removeClass('btn-warning');
		$this.addClass('btn-success');
	} else {
		$this.text('Remove From Cart');
		$this.addClass('btn-warning');
		$this.removeClass('btn-success');
	}
});


// Delete Row
$(".deleteRow").on('click', function () {
	$(this).closest('tr').remove();
});