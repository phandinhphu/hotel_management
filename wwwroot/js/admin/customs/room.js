// Contry Select Initialization
$('#bookingRoom').on('shown.bs.modal', function () {
    var $country = $('#country');
    if (typeof $.fn.countrySelect === "function" && $country.length) {
        // Nếu đã khởi tạo trước đó, có thể destroy trước khi khởi tạo lại (nếu cần)
        if ($country.data('countrySelect')) {
            $country.countrySelect('destroy');
        }
        $country.countrySelect({
            defaultCountry: "vn",
            preferredCountries: ['vn', 'us', 'gb']
        });
    }
});
