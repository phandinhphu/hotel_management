Dropzone.autoDiscover = false;

document.addEventListener('DOMContentLoaded', function () {
    // Dropzone cho nhiều ảnh
    if (document.querySelector("#dropzone-area")) {
        const dropzone = new Dropzone("#dropzone-area", {
            url: "#",
            autoProcessQueue: false,
            uploadMultiple: true,
            addRemoveLinks: true,
            dictRemoveFile: "Xóa ảnh",
            dictDefaultMessage: "Kéo thả file vào đây hoặc nhấn để chọn file",
            clickable: true,
            thumbnailWidth: null,
            thumbnailHeight: null,
            init: function () {
                const inputFile = document.querySelector("#productImages");
                const myDropzone = this;

                if (!inputFile) {
                    console.error("Input #productImages không tồn tại");
                    return;
                }

                // Hàm đồng bộ file từ Dropzone sang input
                const updateInputFiles = function () {
                    const dataTransfer = new DataTransfer();
                    myDropzone.files.forEach(file => {
                        dataTransfer.items.add(file);
                    });
                    inputFile.files = dataTransfer.files;
                };

                // Khi file được thêm vào Dropzone
                this.on("addedfile", function (file) {
                    updateInputFiles();
                    const progress = file.previewElement.querySelector(".dz-progress");
                    if (progress) progress.style.display = "none";
                });

                // Khi file bị xóa khỏi Dropzone
                this.on("removedfile", function (file) {
                    updateInputFiles();
                });

                // Khi thumbnail được tạo
                this.on("thumbnail", function (file, dataUrl) {
                    // Đảm bảo ảnh hiển thị đầy đủ
                    const thumbnail = file.previewElement.querySelector(".dz-image img");
                    if (thumbnail) {
                        thumbnail.style.width = "100%"; // Hiển thị full chiều rộng
                        thumbnail.style.height = "auto"; // Tự động điều chỉnh chiều cao
                        thumbnail.style.objectFit = "contain"; // Giữ tỷ lệ ảnh
                    }
                });
            }
        });
    }

    // Dropzone cho một ảnh duy nhất
    if (document.querySelector("#single-dropzone-area")) {
        const singleImageDropzone = new Dropzone("#single-dropzone-area", {
            url: "#",
            autoProcessQueue: false,
            uploadMultiple: false, // Chỉ upload 1 file
            maxFiles: 1, // Giới hạn số lượng file là 1
            addRemoveLinks: true,
            dictRemoveFile: "Xóa ảnh",
            dictDefaultMessage: "Kéo thả ảnh vào đây hoặc nhấn để chọn file",
            dictMaxFilesExceeded: "Chỉ được tải lên 1 ảnh",
            clickable: true,
            // Đảm bảo không thay đổi kích thước ảnh
            thumbnailWidth: null,
            thumbnailHeight: null,
            resizeWidth: null,
            resizeHeight: null,
            resizeMethod: 'contain',
            acceptedFiles: "image/jpeg,image/jpg,image/png,image/gif",
            init: function () {
                const inputFile = document.querySelector("#productImage"); // Input file cho ảnh đơn
                const myDropzone = this;

                if (!inputFile) {
                    console.error("Input #productImage không tồn tại");
                    return;
                }

                // Hàm đồng bộ file từ Dropzone sang input (cho 1 file)
                const updateSingleInputFile = function () {
                    if (myDropzone.files.length > 0) {
                        const dataTransfer = new DataTransfer();
                        dataTransfer.items.add(myDropzone.files[0]); // Chỉ lấy file đầu tiên
                        inputFile.files = dataTransfer.files;
                    } else {
                        // Nếu không có file, reset input
                        inputFile.value = '';
                    }
                };

                // Khi file được thêm vào Dropzone
                this.on("addedfile", function (file) {
                    // Nếu đã có file trước đó, xóa file cũ
                    if (myDropzone.files.length > 1) {
                        myDropzone.removeFile(myDropzone.files[0]);
                    }
                    updateSingleInputFile();
                    const progress = file.previewElement.querySelector(".dz-progress");
                    if (progress) progress.style.display = "none";
                });

                // Khi file bị xóa khỏi Dropzone
                this.on("removedfile", function (file) {
                    updateSingleInputFile();
                });

                // Thêm xử lý thumbnail để đảm bảo hiển thị đúng
                this.on("thumbnail", function (file, dataUrl) {
                    // Đảm bảo ảnh hiển thị đầy đủ
                    const thumbnail = file.previewElement.querySelector(".dz-image img");
                    if (thumbnail) {
                        // Sử dụng style để đảm bảo ảnh hiển thị đúng kích thước
                        thumbnail.style.width = "auto"; // Không cưỡng chế chiều rộng
                        thumbnail.style.height = "auto"; // Không cưỡng chế chiều cao
                        thumbnail.style.maxWidth = "100%"; // Giới hạn chiều rộng tối đa
                        thumbnail.style.maxHeight = "100%"; // Giới hạn chiều cao tối đa
                        thumbnail.style.objectFit = "contain"; // Đảm bảo hiển thị đầy đủ ảnh

                        // Điều chỉnh container nếu cần
                        const imageContainer = thumbnail.closest('.dz-image');
                        if (imageContainer) {
                            imageContainer.style.width = "auto";
                            imageContainer.style.height = "auto";
                            imageContainer.style.display = "flex";
                            imageContainer.style.alignItems = "center";
                            imageContainer.style.justifyContent = "center";
                        }
                    }
                });

                // Xử lý lỗi
                this.on("error", function (file, message) {
                    console.error("Lỗi Dropzone:", message);
                    alert(message);
                    this.removeFile(file);
                });
            }
        });
    }
});
