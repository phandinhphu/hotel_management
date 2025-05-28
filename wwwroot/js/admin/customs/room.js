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
// Handle room image preview
document.addEventListener('DOMContentLoaded', function() {
    // Image preview on file selection
    const roomImageInput = document.getElementById('roomImage');
    const imagePreview = document.getElementById('imagePreview');
    
    if (roomImageInput) {
        roomImageInput.addEventListener('change', function() {
            if (this.files && this.files[0]) {
                const reader = new FileReader();
                reader.onload = function(e) {
                    imagePreview.src = e.target.result;
                    imagePreview.style.display = 'block';
                }
                reader.readAsDataURL(this.files[0]);
            }
        });
    }
    
    // Handle create room form submission
    const createRoomForm = document.getElementById('createRoomForm');
    if (createRoomForm) {
        createRoomForm.addEventListener('submit', function(e) {
            e.preventDefault();
            
            const formData = new FormData(createRoomForm);
            
            // Show loading state
            const saveBtn = document.getElementById('saveRoomBtn');
            const originalBtnText = saveBtn.innerHTML;
            saveBtn.disabled = true;
            saveBtn.innerHTML = '<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> Đang xử lý...';
            
            // Submit form via AJAX
            fetch('/Admin/Room/Create', {
                method: 'POST',
                body: formData
            })
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    // Show success message
                    Swal.fire({
                        icon: 'success',
                        title: 'Thành công!',
                        text: data.message,
                        confirmButtonText: 'OK'
                    }).then(() => {
                        // Close modal and refresh the page
                        $('#createRoom').modal('hide');
                        createRoomForm.reset();
                        imagePreview.style.display = 'none';
                        location.reload();
                    });
                } else {
                    // Show error message
                    Swal.fire({
                        icon: 'error',
                        title: 'Lỗi!',
                        text: data.message,
                        confirmButtonText: 'OK'
                    });
                }
            })
            .catch(error => {
                console.error('Error:', error);
                Swal.fire({
                    icon: 'error',
                    title: 'Lỗi!',
                    text: 'Đã xảy ra lỗi khi xử lý yêu cầu.',
                    confirmButtonText: 'OK'
                });
            })
            .finally(() => {
                // Restore button state
                saveBtn.disabled = false;
                saveBtn.innerHTML = originalBtnText;
            });
        });
    }
});
