// Favorite Functions
function updateFavoriteDisplay(count) {
    const $counter = $('#fav-count');
    $counter.text(count);
    count > 0 ? $counter.removeClass('d-none') : $counter.addClass('d-none');
    console.log('Favorites counter updated to:', count);
}

function showToast(toastId) {
    const toastElement = document.getElementById(toastId);
    if (toastElement) {
        const toast = new bootstrap.Toast(toastElement, {
            delay: 2000,
            autohide: true
        });
        toast.show();
    } else {
        console.error('Toast element not found:', toastId);
    }
}

function toggleFavorite(productId, heartElement) {
    const $heart = $(heartElement);
    $heart.addClass('fa-spin');

    $.ajax({
        url: '/Home/Toggle',
        type: 'POST',
        data: { productId },
        success: function (res) {
            $heart.removeClass('fa-spin');

            // Update heart icon
            if (res.isFavourite) {
                $heart.removeClass('fa-regular').addClass('fa-solid').css('color', 'red');
                showToast('liveToast');
            } else {
                $heart.removeClass('fa-solid').addClass('fa-regular').css('color', 'black');
                showToast('errorToast');
            }

            // Update counter
            updateFavoriteDisplay(res.updatedFavCount);

            // Handle removal from favorites page
            if (window.location.pathname.includes('Favorite') && !res.isFavourite) {
                $heart.closest('.col').fadeOut(300, function () {
                    $(this).remove();
                    fetchFavoriteCount();
                });
            }
        },
        error: function (xhr) {
            console.error("Error:", xhr.responseText);
            $heart.removeClass('fa-spin');
            fetchFavoriteCount();
        }
    });
}

function fetchFavoriteCount() {
    $.get('/Home/GetFavCount')
        .done(function (response) {
            updateFavoriteDisplay(response.count);
        })
        .fail(function (xhr) {
            console.error("Failed to fetch favorites count:", xhr.statusText);
            updateFavoriteDisplay(0);
        });
}

function UpdateCartCount() {
    $.ajax({
        url: '/Cart/GetCartItemsCount',
        method: 'GET',
        success: function (response) {
            if (response) {

                $("#cart-count").text(response.count);
            }

          
        }
    });
}
$(document).ready(function () {
    UpdateCartCount();
});

//$(document).ready(function () {
//    UpdateCartCount();
//});

// Initialize everything
$(document).ready(function () {
    // Initialize counters
    fetchFavoriteCount();
    //UpdateCartCount();
    UpdateCartCount();

    // Set up event handlers
    $(document).on('click', '.heart', function () {
        const productId = $(this).data('id');
        toggleFavorite(productId, this);
    });

    // Set up periodic refreshes
    setInterval(fetchFavoriteCount, 30000);
    //setInterval(UpdateCartCount, 30000);
});