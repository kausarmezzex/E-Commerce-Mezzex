$(document).ready(function () {
    let variationValueId = null;
    let productId = $('#productId').val(); // Assuming this is set correctly after product creation

    function handleImageUpload(inputElement, imageTableBody) {
        let files = inputElement.files;

        for (let i = 0; i < files.length; i++) {
            let data = new FormData();
            data.append('file', files[i]);
            data.append('productId', productId);
            if (variationValueId) {
                data.append('variationValueId', variationValueId);
            }

            let mediaType = files[i].type.startsWith('image') ? 'Image' : (files[i].type.startsWith('video') ? 'Video' : 'Unknown');

            $.ajax({
                url: '/api/images',
                type: 'POST',
                processData: false,
                contentType: false,
                data: data,
                success: function (result) {
                    let row = `
                        <tr>
                            <td>
                                ${mediaType === 'Image' ? `<img src="${result.link}" style="width: 100px;" />` : `<video width="100" controls><source src="${result.link}" type="${files[i].type}"></video>`}
                            </td>
                            <td><input type="text" class="form-control seo-filename" /></td>
                            <td><input type="text" class="form-control alt-attribute" /></td>
                            <td><input type="text" class="form-control title-attribute" /></td>
                            <td><input type="text" class="form-control media-type" value="${mediaType}" readonly></td>
                            <td><input type="text" value="${result.link}" class="form-control image-url" readonly /></td>
                            <td>
                                <button type="button" class="btn btn-danger btn-sm delete-btn">Delete</button>
                            </td>
                        </tr>`;
                    imageTableBody.append(row);
                },
                error: function () {
                    Swal.fire('Error', 'Error uploading image.', 'error');
                }
            });
        }
    }

    function saveAllImages(imageTableBody) {
        let imageDetails = [];
        imageTableBody.find('tr').each(function () {
            let seoFilename = $(this).find('.seo-filename').val();
            let altAttribute = $(this).find('.alt-attribute').val();
            let titleAttribute = $(this).find('.title-attribute').val();
            let mediaType = $(this).find('.media-type').val();
            let imageUrl = $(this).find('.image-url').val();

            imageDetails.push({
                VirtualPath: imageUrl,
                SeoFilename: seoFilename,
                AltAttribute: altAttribute,
                TitleAttribute: titleAttribute,
                MediaType: mediaType,
                ProductId: parseInt(productId), // Ensure ProductId is an integer
                VariationValueId: variationValueId ? parseInt(variationValueId) : null // Ensure VariationValueId is an integer if present
            });
        });

        console.log(imageDetails); // Log the imageDetails for debugging

        $.ajax({
            url: '/api/ImagesDetails',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(imageDetails),
            success: function () {
                Swal.fire({
                    icon: 'success',
                    title: 'Success',
                    text: 'Images saved successfully.',
                    timer: 2000,
                    showConfirmButton: false
                }).then(() => {
                    /* window.location.href = '/Products/Index'; // Redirect to product index*/
                });
            },
            error: function (xhr, status, error) {
                console.error(xhr.responseText);
                console.log(imageDetails);// Log the error response for debugging
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'Error saving images.'
                });
            }
        });
    }

    $(document).on('change', '#multipleImageUpload', function () {
        handleImageUpload(this, $('#imageTableBody'));
    });

    $(document).on('click', '.delete-btn', function () {
        $(this).closest('tr').remove();
    });

    $(document).on('click', '#uploadAllImages', function () {
        saveAllImages($('#imageTableBody'));
    });

    // Variation value form submission
    $("#VariationValueForm").on("submit", function (event) {
        event.preventDefault();
        var formData = new FormData(this);

        $.ajax({
            url: $(this).attr("action"),
            type: $(this).attr("method"),
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                if (response.success) {
                    variationValueId = response.variationValueId;
                    Swal.fire({
                        icon: 'success',
                        title: 'Success',
                        text: response.message,
                        timer: 2000,
                        showConfirmButton: false
                    }).then(() => {
                        $("#VariationValueForm")[0].reset();
                        $('#variationList').append(`<li>${response.variationValueName} - ${response.variationType}</li>`); // Assuming 'name' is a property of VariationType
                        $("#uploadAllImages").trigger("click");
                        $('#imageTableBody').empty();
                    });
                } else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: response.message
                    });
                }
            },
            error: function (xhr, status, error) {
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'An error occurred while submitting the form. Please try again.'
                });
            }
        });
    });
});
