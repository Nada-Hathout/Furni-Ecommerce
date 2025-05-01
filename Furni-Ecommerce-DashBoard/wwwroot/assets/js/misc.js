var ChartColor = ["#5D62B4", "#54C3BE", "#EF726F", "#F9C446", "rgb(93.0, 98.0, 180.0)", "#21B7EC", "#04BCCC"];
var primaryColor = getComputedStyle(document.body).getPropertyValue('--primary');
var secondaryColor = getComputedStyle(document.body).getPropertyValue('--secondary');
var successColor = getComputedStyle(document.body).getPropertyValue('--success');
var warningColor = getComputedStyle(document.body).getPropertyValue('--warning');
var dangerColor = getComputedStyle(document.body).getPropertyValue('--danger');
var infoColor = getComputedStyle(document.body).getPropertyValue('--info');
var darkColor = getComputedStyle(document.body).getPropertyValue('--dark');
var lightColor = getComputedStyle(document.body).getPropertyValue('--light');

(function ($) {
    'use strict';
    $(function () {
        var body = $('body');
        var contentWrapper = $('.content-wrapper');
        var scroller = $('.container-scroller');
        var footer = $('.footer');
        var sidebar = $('.sidebar');

        // Enhanced function to add active class to nav-links
        function addActiveClass(element) {
            var currentPath = window.location.pathname.toLowerCase();
            var elementHref = element.attr('href');
            var elementController = element.attr('asp-controller');
            var elementAction = element.attr('asp-action');

            // Remove active class from all siblings first
            element.parents('.nav-item').siblings().removeClass('active');
            element.parents('.sub-menu').find('.nav-link').removeClass('active');

            // Check for ASP.NET routes first
            if (elementController) {
                var controllerPath = '/' + elementController.toLowerCase();
                var actionPath = elementAction ? '/' + elementAction.toLowerCase() : '';

                if (currentPath === controllerPath + actionPath ||
                    (elementAction === 'Index' && currentPath === controllerPath)) {
                    markAsActive(element);
                }
            }
            // Check for regular href links
            else if (elementHref) {
                var hrefPath = elementHref.toLowerCase();
                if (currentPath.includes(hrefPath.replace(/^\/|\/$/g, ''))) {
                    markAsActive(element);
                }
            }
        }

        function markAsActive(element) {
            element.addClass('active');
            element.parents('.nav-item').addClass('active');

            // If it's inside a collapse, open the parent collapse
            var parentCollapse = element.closest('.collapse');
            if (parentCollapse.length) {
                parentCollapse.addClass('show');
                parentCollapse.prev('.nav-link').addClass('active');
            }
        }

        // Initialize active states
        var current = location.pathname.split("/").slice(-1)[0].replace(/^\/|\/$/g, '');
        $('.nav li a', sidebar).each(function () {
            addActiveClass($(this));
        });

        $('.horizontal-menu .nav li a').each(function () {
            addActiveClass($(this));
        });

        // Close other submenus when opening one
        sidebar.on('show.bs.collapse', '.collapse', function () {
            sidebar.find('.collapse.show').not(this).collapse('hide');
        });

        // Change sidebar and content-wrapper height
        applyStyles();

        function applyStyles() {
            // Applying perfect scrollbar
            if (!body.hasClass("rtl")) {
                if ($('.settings-panel .tab-content .tab-pane.scroll-wrapper').length) {
                    const settingsPanelScroll = new PerfectScrollbar('.settings-panel .tab-content .tab-pane.scroll-wrapper');
                }
                if ($('.chats').length) {
                    const chatsScroll = new PerfectScrollbar('.chats');
                }
                if (body.hasClass("sidebar-fixed")) {
                    var fixedSidebarScroll = new PerfectScrollbar('#sidebar .nav');
                }
            }
        }

        // Toggle minimize
        $('[data-toggle="minimize"]').on("click", function () {
            if ((body.hasClass('sidebar-toggle-display')) || (body.hasClass('sidebar-absolute'))) {
                body.toggleClass('sidebar-hidden');
            } else {
                body.toggleClass('sidebar-icon-only');
            }
        });

        // Checkbox and radios
        $(".form-check label,.form-radio label").append('<i class="input-helper"></i>');

        // Fullscreen
        $("#fullscreen-button").on("click", function toggleFullScreen() {
            if ((document.fullScreenElement !== undefined && document.fullScreenElement === null) ||
                (document.msFullscreenElement !== undefined && document.msFullscreenElement === null) ||
                (document.mozFullScreen !== undefined && !document.mozFullScreen) ||
                (document.webkitIsFullScreen !== undefined && !document.webkitIsFullScreen)) {
                if (document.documentElement.requestFullScreen) {
                    document.documentElement.requestFullScreen();
                } else if (document.documentElement.mozRequestFullScreen) {
                    document.documentElement.mozRequestFullScreen();
                } else if (document.documentElement.webkitRequestFullScreen) {
                    document.documentElement.webkitRequestFullScreen(Element.ALLOW_KEYBOARD_INPUT);
                } else if (document.documentElement.msRequestFullscreen) {
                    document.documentElement.msRequestFullscreen();
                }
            } else {
                if (document.cancelFullScreen) {
                    document.cancelFullScreen();
                } else if (document.mozCancelFullScreen) {
                    document.mozCancelFullScreen();
                } else if (document.webkitCancelFullScreen) {
                    document.webkitCancelFullScreen();
                } else if (document.msExitFullscreen) {
                    document.msExitFullscreen();
                }
            }
        });

        // Handle click events on sidebar links to maintain active state
        $('.sidebar .nav-link').on('click', function () {
            var $this = $(this);

            // Don't interfere with collapse toggles
            if (!$this.attr('data-bs-toggle')) {
                // Remove active class from all items
                $('.sidebar .nav-link').removeClass('active');
                $('.sidebar .nav-item').removeClass('active');

                // Add active class to clicked item
                $this.addClass('active');
                $this.closest('.nav-item').addClass('active');

                // If it's inside a collapse, keep parent active
                var parentCollapse = $this.closest('.collapse');
                if (parentCollapse.length) {
                    parentCollapse.addClass('show');
                    parentCollapse.prev('.nav-link').addClass('active');
                }
            }
        });
    });
})(jQuery);