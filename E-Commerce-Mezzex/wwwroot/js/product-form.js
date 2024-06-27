$(document).ready(function () {
    // Initialize Froala Editor
    new FroalaEditor('#content', {
        imageUploadURL: '/api/images',
        heightMin: 200
    });

    $('#tagsInput').select2({
        tags: true,
        tokenSeparators: [',']
    });

    $('#CategoryId').select2({
        placeholder: 'Select Categories',
        allowClear: true
    });

    $('#productForm').on('submit', function (e) {
        e.preventDefault();

        if ($(this).data('submitted') === true) {
            return;
        }

        $(this).data('submitted', true);

        // Check if required fields are filled
        if (!$('#Name').val()) {
            Swal.fire('Error', 'Product Name is required', 'error');
            $(this).data('submitted', false);
            return;
        }

        var selectedRelatedProducts = [];
        $('input[name="RelatedProductId"]:checked').each(function () {
            selectedRelatedProducts.push($(this).val());
        });
        var formData = $(this).serializeArray();
        formData.push({ name: "RelatedProductId", value: selectedRelatedProducts.join(',') });

        $.ajax({
            url: $(this).attr('action'),
            type: $(this).attr('method'),
            data: $.param(formData),
            success: function (response) {
                if (response.success) {
                    $('#productForm').append('<input type="hidden" id="productId" value="' + response.productId + '" />');
                    Swal.fire('Success', 'Product added successfully!', 'success').then(() => {
                        $('#custom-tabs-three-multimedia').html(`
                            <div class="form-group mb-3">
                                <label for="multipleImageUpload">Upload Images</label>
                                <div class="custom-file">
                                    <input type="file" class="custom-file-input" id="multipleImageUpload" multiple>
                                    <label class="custom-file-label" for="multipleImageUpload">Choose files</label>
                                </div>
                            </div>
                            <div class="form-group mb-3">
                                <table class="table">
                                    <thead>
                                        <tr>
                                            <th>Image</th>
                                            <th>SEO Filename</th>
                                            <th>Alt Attribute</th>
                                            <th>Title Attribute</th>
                                            <th>URL</th>
                                            <th>Actions</th>
                                        </tr>
                                    </thead>
                                    <tbody id="imageTableBody">
                                        <!-- Images will be appended here -->
                                    </tbody>
                                </table>
                                <button type="button" id="uploadAllImages" class="btn btn-primary">Save Images</button>
                            </div>
                        `);
                        $('#custom-tabs-three-multimedia-tab').tab('show');
                    });
                } else {
                    Swal.fire('Error', response.message, 'error');
                }
                $('#productForm').data('submitted', false); // Allow form to be submitted again
            },
            error: function (xhr, status, error) {
                console.error(xhr.responseText); // Log the error response for debugging
                Swal.fire('Error', 'An error occurred while adding the product.', 'error');
                $('#productForm').data('submitted', false); // Allow form to be submitted again
            }
        });
    });
   
    // Handle multimedia tab click
    $('#custom-tabs-three-multimedia-tab').on('click', function (e) {
        if (!$('#productId').val()) {
            e.preventDefault();
            Swal.fire('Warning', 'Please add the product first.', 'warning').then(() => {
                $('#custom-tabs-three-home-tab').tab('show');
            });
        }
    });
    $('#custom-tabs-three-variation-tab').on('click', function (e) {
        if (!$('#productId').val()) {
            e.preventDefault();
            Swal.fire('Warning', 'Please add the product first.', 'warning').then(() => {
                $('#custom-tabs-three-home-tab').tab('show');
            });
        }
    });
});