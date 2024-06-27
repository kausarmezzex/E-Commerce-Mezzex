$(document).ready(function () {
    // Handle adding variation value to the table
    $('#addVariationValue').on('click', function (e) {
        e.preventDefault();

        // Get values from the form
        var variationValue = $('#VariationValue_Value').val();
        var variationType = $('#VariationValue_VariationTypeId option:selected').text();
        var variationTypeId = $('#VariationValue_VariationTypeId').val();

        // Check if values are not empty
        if (!variationValue || !variationTypeId) {
            Swal.fire('Error', 'Please provide a variation value and type.', 'error');
            return;
        }

        // Append new row to the table
        var newRow = `
            <tr>
                <td>${variationValue}</td>
                <td>${variationType}</td>
                <td>
                    <button type="button" class="btn btn-success save-variation-value" data-value="${variationValue}" data-typeid="${variationTypeId}">Save to DB</button>
                    <button type="button" class="btn btn-primary add-image" data-value="${variationValue}" data-typeid="${variationTypeId}">Add Image</button>
                </td>
            </tr>`;

        $('#variationValueTableBody').append(newRow);
    });

    // Handle Save to DB button click
    $('#variationValueTableBody').on('click', '.save-variation-value', function () {
        var button = $(this);
        var value = button.data('value');
        var typeId = button.data('typeid');

        $.ajax({
            url: '/VariationValues/Create',
            type: 'POST',
            data: {
                VariationValue: {
                    Value: value,
                    VariationTypeId: typeId,
                    ProductId: $('#ProductId').val()
                }
            },
            success: function (response) {
                if (response.success) {
                    Swal.fire('Success', 'Variation Value saved successfully!', 'success');
                } else {
                    Swal.fire('Error', response.message, 'error');
                }
            },
            error: function (xhr, status, error) {
                console.error(xhr.responseText);
                Swal.fire('Error', 'An error occurred while saving the Variation Value.', 'error');
            }
        });
    });

    // Handle Add Image button click
    $('#variationValueTableBody').on('click', '.add-image', function () {
        $('#imageUploadSection').show();
    });

    // Handle image upload
    $(document).on('change', '#multipleImageUpload', function () {
        handleImageUpload(this, $('#imageTableBody'));
    });

    function handleImageUpload(inputElement, imageTableBody) {
        let files = inputElement.files;
        let productId = $('#ProductId').val();
        for (let i = 0; i < files.length; i++) {
            let data = new FormData();
            data.append('file', files[i]);
            data.append('productId', productId);

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

    // Handle Save Images button click
    $('#uploadAllImages').on('click', function () {
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
                ProductId: $('#ProductId').val()
            });
        });

        $.ajax({
            url: '/api/ImagesDetails',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(imageDetails),
            success: function () {
                Swal.fire('Success', 'Images saved successfully.', 'success').then(() => {
                    window.location.href = '/Products/Index';
                });
            },
            error: function (xhr, status, error) {
                console.error(xhr.responseText); // Log the error response for debugging
                Swal.fire('Error', 'Error saving images.', 'error');
            }
        });
    });
});
