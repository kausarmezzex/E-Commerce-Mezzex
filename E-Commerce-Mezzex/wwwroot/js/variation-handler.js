$(document).ready(function () {
    // Handle form submission for VariationType and VariationValue
    $('#variationTypeForm').on('submit', function (e) {
        e.preventDefault();

        $.ajax({
            url: $(this).attr('action'),
            type: $(this).attr('method'),
            data: $(this).serialize(),
            success: function (response) {
                if (response.success) {
                    Swal.fire('Success', 'Variation Type added successfully!', 'success');
                } else {
                    Swal.fire('Error', response.message, 'error');
                }
            },
            error: function (xhr) {
                console.error(xhr.responseText);
                Swal.fire('Error', 'An error occurred while adding the Variation Type.', 'error');
            }
        });
    });

    $('#variationValueForm').on('submit', function (e) {
        e.preventDefault();

        $.ajax({
            url: $(this).attr('action'),
            type: $(this).attr('method'),
            data: $(this).serialize(),
            success: function (response) {
                if (response.success) {
                    Swal.fire('Success', 'Variation Value added successfully!', 'success');
                } else {
                    Swal.fire('Error', response.message, 'error');
                }
            },
            error: function (xhr) {
                console.error(xhr.responseText);
                Swal.fire('Error', 'An error occurred while adding the Variation Value.', 'error');
            }
        });
    });
});