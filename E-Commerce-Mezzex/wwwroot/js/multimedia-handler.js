$(document).ready(function () {
    var variationValueId = null;

    // Handle form submission for VariationValue
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
                        // Reset the form
                        $("#VariationValueForm")[0].reset();

                        // Append new variation to the list
                        $('#variationList').append(`<li>${response.variationValueName} - ${response.variationType}</li>`); // Assuming 'name' is a property of VariationType

                        // Automatically trigger the Save Images button click
                        $("#uploadAllImages").trigger("click");
                        $('#imageTableBody').empty(); // Clear image table
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

    // Handle image upload
    $(document).on('change', '#multipleImageUpload', function () {
        handleImageUpload(this, $('#imageTableBody'));
    });
   
    function handleImageUpload(inputElement, imageTableBody) {
        let files = inputElement.files;
        let productId = $('#productId').val();

        for (let i = 0; i < files.length; i++) {
            let data = new FormData();
            data.append('file', files[i]);
            data.append('productId', productId);
            data.append('variationValueId', variationValueId);

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

    // Handle deleting image row
    $(document).on('click', '.delete-btn', function () {
        $(this).closest('tr').remove();
    });

    // Handle saving all images
    $(document).on('click', '#uploadAllImages', function () {
        let imageDetails = [];
        $('#imageTableBody tr').each(function () {
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
                ProductId: $('#productId').val(),
                VariationValueId: variationValueId
            });
        });

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
                console.error(xhr.responseText); // Log the error response for debugging
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'Error saving images.'
                });
            }
        });
    });
});