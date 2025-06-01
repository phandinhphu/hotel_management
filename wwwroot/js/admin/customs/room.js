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

// Function to handle price range validation
document.addEventListener('DOMContentLoaded', function() {
    const minPriceInput = document.getElementById('minPrice');
    const maxPriceInput = document.getElementById('maxPrice');
    const filterForm = document.getElementById('roomFilterForm');

    if (filterForm) {
        filterForm.addEventListener('submit', function(e) {
            const minPrice = Number(minPriceInput.value);
            const maxPrice = Number(maxPriceInput.value);
            
            // Validate if max price is greater than min price when both are provided
            if (minPrice && maxPrice && minPrice > maxPrice) {
                e.preventDefault();
                alert('Giá tối đa phải lớn hơn giá tối thiểu');
                return false;
            }
        });
    }

    // Set up automatic submission on select changes
    const autoSubmitElements = document.querySelectorAll('#roomType, #status');
    autoSubmitElements.forEach(element => {
        element.addEventListener('change', function() {
            filterForm.submit();
        });
    });
});
