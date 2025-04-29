(function ($) {
    'use strict';
    $(function () {
        // Existing functionality (keep all your current code)
        $(".nav-settings").click(function () {
            $("#right-sidebar").toggleClass("open");
        });
        $(".settings-close").click(function () {
            $("#right-sidebar,#theme-settings,#layout-settings").removeClass("open");
        });

        $("#settings-trigger").on("click", function () {
            $("#theme-settings").toggleClass("open");
        });

        // Check all boxes in order status 
        $("#check-all").click(function () {
            $(".checkbox").prop('checked', $(this).prop('checked'));
        });

        // Background constants
        var navbar_classes = "navbar-danger navbar-success navbar-warning navbar-dark navbar-light navbar-primary navbar-info navbar-pink";
        var sidebar_classes = "sidebar-light sidebar-dark";
        var $body = $("body");

        // Sidebar backgrounds
        $("#sidebar-default-theme").on("click", function () {
            $body.removeClass(sidebar_classes);
            $(".sidebar-bg-options").removeClass("selected");
            $(this).addClass("selected");
        });
        $("#sidebar-dark-theme").on("click", function () {
            $body.removeClass(sidebar_classes);
            $body.addClass("sidebar-dark");
            $(".sidebar-bg-options").removeClass("selected");
            $(this).addClass("selected");
        });
        $("#sidebar-light-theme").on("click", function () {
            $body.removeClass(sidebar_classes);
            $body.addClass("sidebar-light");
            $(".sidebar-bg-options").removeClass("selected");
            $(this).addClass("selected");
        });

        // Navbar Backgrounds
        $(".tiles.primary").on("click", function () {
            $(".navbar").removeClass(navbar_classes);
            $(".navbar").addClass("navbar-primary");
            $(".tiles").removeClass("selected");
            $(this).addClass("selected");
        });
        $(".tiles.success").on("click", function () {
            $(".navbar").removeClass(navbar_classes);
            $(".navbar").addClass("navbar-success");
            $(".tiles").removeClass("selected");
            $(this).addClass("selected");
        });
        $(".tiles.warning").on("click", function () {
            $(".navbar").removeClass(navbar_classes);
            $(".navbar").addClass("navbar-warning");
            $(".tiles").removeClass("selected");
            $(this).addClass("selected");
        });
        $(".tiles.danger").on("click", function () {
            $(".navbar").removeClass(navbar_classes);
            $(".navbar").addClass("navbar-danger");
            $(".tiles").removeClass("selected");
            $(this).addClass("selected");
        });
        $(".tiles.info").on("click", function () {
            $(".navbar").removeClass(navbar_classes);
            $(".navbar").addClass("navbar-info");
            $(".tiles").removeClass("selected");
            $(this).addClass("selected");
        });
        $(".tiles.dark").on("click", function () {
            $(".navbar").removeClass(navbar_classes);
            $(".navbar").addClass("navbar-dark");
            $(".tiles").removeClass("selected");
            $(this).addClass("selected");
        });
        $(".tiles.default").on("click", function () {
            $(".navbar").removeClass(navbar_classes);
            $(".tiles").removeClass("selected");
            $(this).addClass("selected");
        });

        // Horizontal menu in mobile
        $('[data-toggle="horizontal-menu-toggle"]').on("click", function () {
            $(".horizontal-menu .bottom-navbar").toggleClass("header-toggled");
        });

        // Horizontal menu navigation in mobile menu on click
        var navItemClicked = $('.horizontal-menu .page-navigation >.nav-item');
        navItemClicked.on("click", function (event) {
            if (window.matchMedia('(max-width: 991px)').matches) {
                if (!($(this).hasClass('show-submenu'))) {
                    navItemClicked.removeClass('show-submenu');
                }
                $(this).toggleClass('show-submenu');
            }
        });

        $(window).scroll(function () {
            if (window.matchMedia('(min-width: 992px)').matches) {
                var header = $('.horizontal-menu');
                if ($(window).scrollTop() >= 71) {
                    $(header).addClass('fixed-on-scroll');
                } else {
                    $(header).removeClass('fixed-on-scroll');
                }
            }
        });

        $("#layout-toggler").on("click", function () {
            $("#layout-settings").addClass("open");
        });
        $("#chat-toggler").on("click", function () {
            $("#right-sidebar").addClass("open");
        });

        // Enhanced Sidebar Navigation Active State Handling
        function activateNavItem($element) {
            $element.addClass('active');
            $element.closest('.nav-item').addClass('active');

            // If it's inside a collapse, open the parent collapse
            var parentCollapse = $element.closest('.collapse');
            if (parentCollapse.length) {
                parentCollapse.addClass('show');
                parentCollapse.prev('.nav-link').addClass('active');
            }
        }

        function setActiveNavItem() {
            var currentPath = window.location.pathname.toLowerCase();

            // Remove all active classes first
            $('.sidebar .nav-link').removeClass('active');
            $('.sidebar .nav-item').removeClass('active');
            $('.sidebar .collapse').removeClass('show');

            // Find and activate matching link
            $('.sidebar .nav-link').each(function () {
                var $this = $(this);
                var linkPath = $this.attr('href');

                if (linkPath) {
                    // Handle both ASP.NET routes and regular hrefs
                    var isAspRoute = $this.is('[asp-action]') || $this.is('[asp-controller]');
                    var normalizedLink = linkPath.toLowerCase();

                    if (isAspRoute) {
                        var controller = ($this.attr('asp-controller') || '').toLowerCase();
                        var action = ($this.attr('asp-action') || '').toLowerCase();

                        // Special case for Home/Index
                        if (controller === 'home' && (action === 'index' || action === '')) {
                            if (currentPath.endsWith('/') || currentPath.includes('/home') || currentPath.includes('/home/index')) {
                                activateNavItem($this);
                            }
                        }
                        // Match controller/action
                        else if (currentPath.includes('/' + controller + '/' + action)) {
                            activateNavItem($this);
                        }
                        // Match just controller
                        else if (action === '' && currentPath.includes('/' + controller)) {
                            activateNavItem($this);
                        }
                    }
                    // Match regular hrefs
                    else if (currentPath.includes(normalizedLink.replace(/^\/|\/$/g, ''))) {
                        activateNavItem($this);
                    }
                }
            });
        }

        // Initialize active state on page load
        setActiveNavItem();

        // Handle click events on sidebar links
        $('.sidebar .nav-link').on('click', function (e) {
            var $this = $(this);

            // Don't prevent default for collapse toggles
            if ($this.attr('data-bs-toggle') !== 'collapse') {
                // Remove active class from all items
                $('.sidebar .nav-link').removeClass('active');
                $('.sidebar .nav-item').removeClass('active');

                // Add active class to clicked item
                activateNavItem($this);
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

        // Perfect scrollbar initialization (if needed)
        if (typeof PerfectScrollbar !== 'undefined') {
            if ($('.sidebar .nav').length) {
                new PerfectScrollbar('.sidebar .nav');
            }
        }
    });
})(jQuery);

// Chart colors (keep your existing variables)
var ChartColor = ["#5D62B4", "#54C3BE", "#EF726F", "#F9C446", "rgb(93.0, 98.0, 180.0)", "#21B7EC", "#04BCCC"];
var primaryColor = getComputedStyle(document.body).getPropertyValue('--primary');
var secondaryColor = getComputedStyle(document.body).getPropertyValue('--secondary');
var successColor = getComputedStyle(document.body).getPropertyValue('--success');
var warningColor = getComputedStyle(document.body).getPropertyValue('--warning');
var dangerColor = getComputedStyle(document.body).getPropertyValue('--danger');
var infoColor = getComputedStyle(document.body).getPropertyValue('--info');
var darkColor = getComputedStyle(document.body).getPropertyValue('--dark');
var lightColor = getComputedStyle(document.body).getPropertyValue('--light');