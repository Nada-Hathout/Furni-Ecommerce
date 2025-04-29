(function ($) {
    'use strict';
    $(function () {
        // Offcanvas toggle
        $('[data-toggle="offcanvas"]').on("click", function () {
            $('.sidebar-offcanvas').toggleClass('active')
        });

        // Active menu item handling
        function setActiveMenuItem() {
            var currentPath = window.location.pathname;

            // Remove all active classes first
            $('.sidebar .nav-link').removeClass('active');
            $('.sidebar .nav-item').removeClass('active');
            $('.sidebar .collapse').removeClass('show');

            // Find matching link and set active state
            $('.sidebar .nav-link').each(function () {
                var $this = $(this);
                var linkPath = $this.attr('href');

                if (linkPath && currentPath.includes(linkPath.replace(/^\/|\/$/g, ''))) {
                    $this.addClass('active');
                    $this.closest('.nav-item').addClass('active');

                    // If it's inside a collapse, open the parent collapse
                    var parentCollapse = $this.closest('.collapse');
                    if (parentCollapse.length) {
                        parentCollapse.addClass('show');
                        parentCollapse.prev('.nav-link').addClass('active');
                    }
                }
            });

            // Special case for home/dashboard
            if (currentPath.endsWith('/') || currentPath.endsWith('/Home') || currentPath.endsWith('/Home/Index')) {
                $('.sidebar .nav-link[asp-controller="Home"]').addClass('active');
            }
        }

        // Initialize active state on page load
        setActiveMenuItem();

        // Handle click events on sidebar links
        $('.sidebar .nav-link').on('click', function (e) {
            // Don't prevent default for dropdown toggles
            if ($(this).attr('data-bs-toggle') !== 'collapse') {
                // Remove active class from all items
                $('.sidebar .nav-link').removeClass('active');
                $('.sidebar .nav-item').removeClass('active');

                // Add active class to clicked item
                $(this).addClass('active');
                $(this).closest('.nav-item').addClass('active');

                // If it's inside a collapse, keep parent active
                var parentCollapse = $(this).closest('.collapse');
                if (parentCollapse.length) {
                    parentCollapse.prev('.nav-link').addClass('active');
                }
            }
        });

        // Handle collapse events to maintain proper active states
        $('.sidebar [data-bs-toggle="collapse"]').on('click', function () {
            var target = $(this).attr('href');

            // Close other open collapses
            $('.sidebar .collapse').not(target).collapse('hide');

            // Remove active from other collapse triggers
            $('.sidebar [data-bs-toggle="collapse"]').not(this).removeClass('active');
        });
    });
})(jQuery);