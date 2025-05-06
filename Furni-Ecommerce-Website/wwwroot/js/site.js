// Global favorite counter management
//let favoriteCount = 0;

// Update favorite counter display
//function updateFavoriteDisplay(count) {
//    const $counter = $('#fav-count');
//    favoriteCount = count;

//    $counter.text(count);

    
//    if (count > 0) {
//        $counter.addClass('visible').removeClass('invisible').fadeIn(100);
//    } else {
//        $counter.addClass('invisible').removeClass('visible').fadeOut(100);
//    }
//}
function updateFavoriteDisplay(count) {
    const $counter = $('#fav-count');
    $counter.text(count);
    count > 0 ? $counter.removeClass('d-none') : $counter.addClass('d-none');
}


function toggleFavorite(productId, heartElement) {
    const $heart = $(heartElement);

    // Show loading state
    $heart.addClass('fa-spin');

    $.ajax({
        url: '/Home/Toggle',
        type: 'POST',
        data: { productId },
        success: function (res) {
            // Update heart icon
            if (res.isFavourite) {
                $heart.removeClass('fa-regular fa-spin')
                    .addClass('fa-solid')
                    .css('color', 'red');
                // Show error toast
                const toast = new bootstrap.Toast(document.getElementById('liveToast'));
                toast.show();
            } else {
                $heart.removeClass('fa-solid fa-spin')
                    .addClass('fa-regular')
                    .css('color', 'black');
            }

            // Update counter
            updateFavoriteDisplay(res.updatedFavCount);

            // Remove from favorites page if needed
            if (window.location.pathname.includes('Favorite') && !res.isFavourite) {
                $heart.closest('.col').fadeOut(300, function () {
                    $(this).remove();
                });
            }
        },
        error: function (xhr) {
            console.error("Favorite toggle failed:", xhr.responseText);
            $heart.removeClass('fa-spin');

            // Show error toast
            const toast = new bootstrap.Toast(document.getElementById('errorToast'));
            toast.show();
        }
    });
}

// Fetch current favorite count from server
//function fetchFavoriteCount() {
//    $.get('/Cart/GetFavCount', { _: new Date().getTime() })
//        .done(function (response) {
//            updateFavoriteDisplay(response.count);
//        })
//        .fail(function (xhr) {
//            console.error("Failed to fetch favorite count:", xhr.responseText);
//            // Fallback: Set counter to 0
//            updateFavoriteDisplay(0);
//        });
//}
function fetchFavoriteCount() {
    $.get('/Home/GetFavCount')
        .done(response => {
            console.log("Count:", response.count);
            updateFavoriteDisplay(response.count);
        })
        .fail(xhr => {
            console.error("Fetch failed:", xhr.statusText);
            updateFavoriteDisplay(0);
        });
}
// Initialize on page load
$(document).ready(function () {
    // Initial fetch
    fetchFavoriteCount();

    // Set up click handler
    $(document).on('click', '.heart', function () {
        const productId = $(this).data('id');
        toggleFavorite(productId, this);
    });

    // Optional: Refresh count periodically (every 30 seconds)
    setInterval(fetchFavoriteCount, 30000);
});