$(document).ready(function () {
    // Existing code...

    $('#uploadAllImages').on('click', function () {
        var files = $('#multipleImageUpload')[0].files;

        if (files.length === 0) {
            Swal.fire('Warning', 'Please select images to upload.', 'warning');
            return;
        }

        var formData = new FormData();
        for (var i = 0; i < files.length; i++) {
            formData.append('images', files[i]);
        }

        $.ajax({
            url: '/api/images', // Replace with your actual upload URL
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                if (response.success) {
                    // Append uploaded images to the table
                    response.images.forEach(function (image) {
                        var newRow = `<tr>
                            <td><img src="${image.url}" alt="${image.alt}" width="50"></td>
                            <td>${image.seoFilename}</td>
                            <td>${image.alt}</td>
                            <td>${image.title}</td>
                            <td>${image.url}</td>
                            <td><button type="button" class="btn btn-danger remove-image" data-id="${image.id}">Remove</button></td>
                        </tr>`;
                        $('#imageTableBody').append(newRow);
                    });

                    Swal.fire('Success', 'Images uploaded successfully!', 'success');
                } else {
                    Swal.fire('Error', response.message, 'error');
                }
            },
            error: function (xhr, status, error) {
                console.error(xhr.responseText);
                Swal.fire('Error', 'An error occurred while uploading images.', 'error');
            }
        });
    });

    $('#imageTableBody').on('click', '.remove-image', function () {
        var button = $(this);
        var imageId = button.data('id');

        $.ajax({
            url: '/api/images/delete/' + imageId, // Replace with your actual delete URL
            type: 'DELETE',
            success: function (response) {
                if (response.success) {
                    button.closest('tr').remove();
                    Swal.fire('Success', 'Image removed successfully!', 'success');
                } else {
                    Swal.fire('Error', response.message, 'error');
                }
            },
            error: function (xhr, status, error) {
                console.error(xhr.responseText);
                Swal.fire('Error', 'An error occurred while removing the image.', 'error');
            }
        });
    });
});
