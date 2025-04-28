// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function updateFavoriteCounter(count) {
    const counter = $('.fav-counter');
    counter.text(count);

    if (count > 0) {
        counter.addClass('visible').removeClass('hidden');
    } else {
        counter.addClass('hidden').removeClass('visible');
    }
}

function toggleFavorite(productId, heartElement) {
    $.ajax({
        url: '/Home/Toggle',
        type: 'POST',
        data: { productId },
        success: function (res) {
            //const toastTrigger = document.getElementById('liveToast');
            //const toast = new bootstrap.Toast(toastTrigger);
            if (res.isFavourite) {
                $(heartElement).removeClass('fa-regular').addClass('fa-solid').css('color', 'red');
               
                //toast.show();
            } else {
                $(heartElement).removeClass('fa-solid').addClass('fa-regular').css('color', 'black');
            }

            
            $('.fav-counter').text(res.updatedFavCount).toggle(res.updatedFavCount > 0);

            if (window.location.pathname.includes('Favorite') && !res.isFavourite) {
                $(heartElement).closest('.col').fadeOut(300, function () {
                    $(this).remove();
                });
            }
        },
        error: function (xhr, status, error) {
            console.log("AJAX error:", error);
        }
    });
}

$(document).ready(function () {
   
    $(document).on('click', '.heart', function () {
        const productId = $(this).data('id');
        toggleFavorite(productId, this);
    });
});