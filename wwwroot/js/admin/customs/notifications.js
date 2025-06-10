/**
 * Custom notifications using Toastr
 * Author: Your Name
 * Version: 1.0.0
 */

// Cấu hình toastr toàn cục
toastr.options = {
    "closeButton": true,
    "debug": false,
    "newestOnTop": true,
    "progressBar": true,
    "positionClass": "toast-top-right",
    "preventDuplicates": false,
    "onclick": null,
    "showDuration": "500",
    "hideDuration": "1000",
    "timeOut": "5000",
    "extendedTimeOut": "1000",
    "showEasing": "swing",
    "hideEasing": "linear",
    "showMethod": "fadeIn",
    "hideMethod": "fadeOut"

}

// Namespace cho các chức năng thông báo
const Notifications = {
    /**
     * Hiển thị thông báo thành công
     * @param {any} message
     * @param {any} title
     */
    success: function (message, title = "Thành công") {
        toastr.success(message, title);
    },
    /**
     * Hiển thị thông báo lỗi
     * @param {any} message
     * @param {any} title
     */
    error: function (message, title = "Lỗi") {
        toastr.error(message, title);
    },
    /**
     * Hiển thị thông báo thông tin
     * @param {any} message
     * @param {any} title
     */
    info: function (message, title = "Thông báo") {
        toastr.info(message, title);
    },
    /**
     * Hiển thị thông báo cảnh báo
     * @param {any} message
     * @param {any} title
     */
    warning: function (message, title = "Cảnh báo") {
        toastr.warning(message, title);
    },
    /**
     * Hiển thị thông báo xác nhận
     * @param {any} message
     * @param {any} title
     * @param {any} callback
     */
    confirm: function (message, title = "Xác nhận", callback) {
        // Sử dụng SweetAlert2 để xác nhận
        Swal.fire({
            title: title,
            text: message,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Đồng ý',
            cancelButtonText: 'Hủy'
        }).then((result) => {
            if (result.isConfirmed) {
                callback(true);
            } else {
                callback(false);
            }
        });
    },
    /**
     * Xóa tất cả thông báo hiện tại
     */
    clear: function () {
        toastr.clear();
    }
};