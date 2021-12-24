//import { type } from "os";

// Main Js File
var loader = `<svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" style="margin: auto; background-color: transparent; display: block; shape-rendering: auto;" width="128px" height="128px" viewBox="0 0 100 100" preserveAspectRatio="xMidYMid">
<g transform="translate(80,50)">
<g transform="rotate(0)">
<circle cx="0" cy="0" r="6" fill="#aeaeae" fill-opacity="1">
  <animateTransform attributeName="transform" type="scale" begin="-0.875s" values="1.5 1.5;1 1" keyTimes="0;1" dur="1s" repeatCount="indefinite"/>
  <animate attributeName="fill-opacity" keyTimes="0;1" dur="1s" repeatCount="indefinite" values="1;0" begin="-0.875s"/>
</circle>
</g>
</g><g transform="translate(71.21320343559643,71.21320343559643)">
<g transform="rotate(45)">
<circle cx="0" cy="0" r="6" fill="#aeaeae" fill-opacity="0.875">
  <animateTransform attributeName="transform" type="scale" begin="-0.75s" values="1.5 1.5;1 1" keyTimes="0;1" dur="1s" repeatCount="indefinite"/>
  <animate attributeName="fill-opacity" keyTimes="0;1" dur="1s" repeatCount="indefinite" values="1;0" begin="-0.75s"/>
</circle>
</g>
</g><g transform="translate(50,80)">
<g transform="rotate(90)">
<circle cx="0" cy="0" r="6" fill="#aeaeae" fill-opacity="0.75">
  <animateTransform attributeName="transform" type="scale" begin="-0.625s" values="1.5 1.5;1 1" keyTimes="0;1" dur="1s" repeatCount="indefinite"/>
  <animate attributeName="fill-opacity" keyTimes="0;1" dur="1s" repeatCount="indefinite" values="1;0" begin="-0.625s"/>
</circle>
</g>
</g><g transform="translate(28.786796564403577,71.21320343559643)">
<g transform="rotate(135)">
<circle cx="0" cy="0" r="6" fill="#aeaeae" fill-opacity="0.625">
  <animateTransform attributeName="transform" type="scale" begin="-0.5s" values="1.5 1.5;1 1" keyTimes="0;1" dur="1s" repeatCount="indefinite"/>
  <animate attributeName="fill-opacity" keyTimes="0;1" dur="1s" repeatCount="indefinite" values="1;0" begin="-0.5s"/>
</circle>
</g>
</g><g transform="translate(20,50.00000000000001)">
<g transform="rotate(180)">
<circle cx="0" cy="0" r="6" fill="#aeaeae" fill-opacity="0.5">
  <animateTransform attributeName="transform" type="scale" begin="-0.375s" values="1.5 1.5;1 1" keyTimes="0;1" dur="1s" repeatCount="indefinite"/>
  <animate attributeName="fill-opacity" keyTimes="0;1" dur="1s" repeatCount="indefinite" values="1;0" begin="-0.375s"/>
</circle>
</g>
</g><g transform="translate(28.78679656440357,28.786796564403577)">
<g transform="rotate(225)">
<circle cx="0" cy="0" r="6" fill="#aeaeae" fill-opacity="0.375">
  <animateTransform attributeName="transform" type="scale" begin="-0.25s" values="1.5 1.5;1 1" keyTimes="0;1" dur="1s" repeatCount="indefinite"/>
  <animate attributeName="fill-opacity" keyTimes="0;1" dur="1s" repeatCount="indefinite" values="1;0" begin="-0.25s"/>
</circle>
</g>
</g><g transform="translate(49.99999999999999,20)">
<g transform="rotate(270)">
<circle cx="0" cy="0" r="6" fill="#aeaeae" fill-opacity="0.25">
  <animateTransform attributeName="transform" type="scale" begin="-0.125s" values="1.5 1.5;1 1" keyTimes="0;1" dur="1s" repeatCount="indefinite"/>
  <animate attributeName="fill-opacity" keyTimes="0;1" dur="1s" repeatCount="indefinite" values="1;0" begin="-0.125s"/>
</circle>
</g>
</g><g transform="translate(71.21320343559643,28.78679656440357)">
<g transform="rotate(315)">
<circle cx="0" cy="0" r="6" fill="#aeaeae" fill-opacity="0.125">
  <animateTransform attributeName="transform" type="scale" begin="0s" values="1.5 1.5;1 1" keyTimes="0;1" dur="1s" repeatCount="indefinite"/>
  <animate attributeName="fill-opacity" keyTimes="0;1" dur="1s" repeatCount="indefinite" values="1;0" begin="0s"/>
</circle>
</g>
</g></svg>`;
$(document).ready(function () {
    'use strict';

    owlCarousels();
    quantityInputs();

    // Header Search Toggle

    var $searchWrapper = $('.header-search-wrapper'),
        $body = $('body'),
        $searchToggle = $('.search-toggle');

    $searchToggle.on('click', function (e) {
        $searchWrapper.toggleClass('show');
        $(this).toggleClass('active');
        $searchWrapper.find('input').focus();
        e.preventDefault();
    });

    $body.on('click', function (e) {
        if ($searchWrapper.hasClass('show')) {
            $searchWrapper.removeClass('show');
            $searchToggle.removeClass('active');
            $body.removeClass('is-search-active');
        }
    });

    $('.header-search').on('click', function (e) {
        e.stopPropagation();
    });

    // Sticky header 
    var catDropdown = $('.category-dropdown'),
        catInitVal = catDropdown.data('visible');

    if ($('.sticky-header').length && $(window).width() >= 768) {
        var sticky = new Waypoint.Sticky({
            element: $('.sticky-header')[0],
            stuckClass: 'fixed',
            offset: 0,

            handler: function (direction) {
                // Show category dropdown
                if (catInitVal && direction == 'up') {
                    catDropdown.addClass('show').find('.dropdown-menu').addClass('show');
                    catDropdown.find('.dropdown-toggle').attr('aria-expanded', 'true');
                    return false;
                }

                // Hide category dropdown on fixed header
                if (catDropdown.hasClass('show')) {
                    catDropdown.removeClass('show').find('.dropdown-menu').removeClass('show');
                    catDropdown.find('.dropdown-toggle').attr('aria-expanded', 'false');
                }


            }
        });
    }

    $('.menu-level-1-li').on('mouseover', (e) => {
        $('.menu-level-1-li').removeClass('show')
        $(e.currentTarget).addClass('show');
        $('.sub-mega-menu').hide();
        $(e.currentTarget).find('.sub-mega-menu').show();
    })

    $('#category-dropdown > a').on('mouseover', () => {
        $('.menu-level-1-li').removeClass('show')
        $('.menu-level-1 .sub-mega-menu').hide();
        $('.menu-level-1 > li:first-child').addClass('show');
        $('.menu-level-1 > li:first-child .sub-mega-menu').show();
    }).on('mouseleave', () => {
        $('.menu-level-1 > li:first-child').removeClass('show');
        $('.menu-level-1 > li:first-child .sub-mega-menu').hide();
    })

    // Mobile Menu Toggle - Show & Hide
    $('.mobile-menu-toggler').on('click', function (e) {
        $body.toggleClass('mmenu-active');
        $(this).toggleClass('active');
        e.preventDefault();
    });

    $('.mobile-menu-overlay, .mobile-menu-close').on('click', function (e) {
        $body.removeClass('mmenu-active');
        $('.menu-toggler').removeClass('active');
        e.preventDefault();
    });

    // Add Mobile menu icon arrows to items with children
    $('.mobile-menu').find('li').each(function () {
        var $this = $(this);

        if ($this.find('ul').length) {
            $(`<span class="mmenu-btn"><svg xmlns="http://www.w3.org/2000/svg" width="12" height="12" fill="currentColor" class="bi bi-chevron-left" viewBox="0 0 16 16">
  <path fill-rule="evenodd" d="M11.354 1.646a.5.5 0 0 1 0 .708L5.707 8l5.647 5.646a.5.5 0 0 1-.708.708l-6-6a.5.5 0 0 1 0-.708l6-6a.5.5 0 0 1 .708 0z"/>
</svg></span>`).appendTo($this.children('a'));
        }
    });

    // Mobile Menu toggle children menu
    $('.mmenu-btn').on('click', function (e) {
        var $parent = $(this).closest('li'),
            $targetUl = $parent.find('ul').eq(0);

        if (!$parent.hasClass('open')) {
            $targetUl.slideDown(300, function () {
                $parent.addClass('open');
            });
        } else {
            $targetUl.slideUp(300, function () {
                $parent.removeClass('open');
            });
        }

        e.stopPropagation();
        e.preventDefault();
    });

    // Sidebar Filter - Show & Hide
    var $sidebarToggler = $('.sidebar-toggler');
    $sidebarToggler.on('click', function (e) {
        $body.toggleClass('sidebar-filter-active');
        $(this).toggleClass('active');
        e.preventDefault();
    });

    $('.sidebar-filter-overlay').on('click', function (e) {
        $body.removeClass('sidebar-filter-active');
        $sidebarToggler.removeClass('active');
        e.preventDefault();
    });

    // Clear All checkbox/remove filters in sidebar filter
    $('.sidebar-filter-clear').on('click', function (e) {
        $('.sidebar-shop').find('input').prop('checked', false);

        e.preventDefault();
    });

    // Popup - Iframe Video - Map etc.
    if ($.fn.magnificPopup) {
        $('.btn-iframe').magnificPopup({
            type: 'iframe',
            removalDelay: 600,
            preloader: false,
            fixedContentPos: false,
            closeBtnInside: false
        });
    }

    // Product hover
    if ($.fn.hoverIntent) {
        $('.product-3').hoverIntent(function () {
            var $this = $(this),
                animDiff = ($this.outerHeight() - ($this.find('.product-body').outerHeight() + $this.find('.product-media').outerHeight())),
                animDistance = ($this.find('.product-footer').outerHeight() - animDiff);

            $this.find('.product-footer').css({
                'visibility': 'visible',
                'transform': 'translateY(0)'
            });
            $this.find('.product-body').css('transform', 'translateY(' + -animDistance + 'px)');

        }, function () {
            var $this = $(this);

            $this.find('.product-footer').css({
                'visibility': 'hidden',
                'transform': 'translateY(100%)'
            });
            $this.find('.product-body').css('transform', 'translateY(0)');
        });
    }

    // Slider For category pages / filter price


    // Product countdown
    if ($.fn.countdown) {
        $('.product-countdown').each(function () {
            var $this = $(this),
                untilDate = $this.data('until'),
                compact = $this.data('compact'),
                dateFormat = (!$this.data('format')) ? 'DHMS' : $this.data('format'),
                newLabels = (!$this.data('labels-short')) ? ['سال', 'ماه', 'هفته', 'روز', 'ساعت', 'دقیقه', 'ثانیه'] : ['سال', 'ماه', 'هفته', 'روز', 'ساعت', 'دقیقه', 'ثانیه'],
                newLabels1 = (!$this.data('labels-short')) ? ['سال', 'ماه', 'هفته', 'روز', 'ساعت', 'دقیقه', 'ثانیه'] : ['سال', 'ماه', 'هفته', 'روز', 'ساعت', 'دقیقه', 'ثانیه'];

            var newDate;

            // Split and created again for ie and edge 
            if (!$this.data('relative')) {
                var untilDateArr = untilDate.split(", "), // data-until 2019, 10, 8 - yy,mm,dd
                    newDate = new Date(untilDateArr[0], untilDateArr[1] - 1, untilDateArr[2]);
            } else {
                newDate = untilDate;
            }

            $this.countdown({
                until: newDate,
                format: dateFormat,
                padZeroes: true,
                compact: compact,
                compactLabels: ['y', 'm', 'w', ' روز، '],
                timeSeparator: ' : ',
                labels: newLabels,
                labels1: newLabels1

            });
        });

        // Pause
        // $('.product-countdown').countdown('pause');
    }

    // Quantity Input - Cart page - Product Details pages
    function quantityInputs() {
        if ($.fn.inputSpinner) {
            $("input[type='number']").inputSpinner({
                decrementButton: '<i class="icon-minus">-</i>',
                incrementButton: '<i class="icon-plus">+</i>',
                groupClass: 'input-spinner',
                buttonsClass: 'btn-spinner',
                buttonsWidth: '26px'
            });
        }
    }

    // Sticky Content - Sidebar - Social Icons etc..
    // Wrap elements with <div class="sticky-content"></div> if you want to make it sticky
    if ($.fn.stick_in_parent && $(window).width() >= 992) {
        $('.sticky-content').stick_in_parent({
            offset_top: 80,
            inner_scrolling: false
        });
    }

    function owlCarousels($wrap, options) {
        if ($.fn.owlCarousel) {
            var owlSettings = {
                items: 1,
                loop: true,
                margin: 0,
                responsiveClass: true,
                nav: true,
                navText: [`<svg xmlns="http://www.w3.org/2000/svg" width="28" height="28" fill="currentColor" class="bi bi-chevron-left" viewBox="0 0 16 16">
  <path fill-rule="evenodd" d="M11.354 1.646a.5.5 0 0 1 0 .708L5.707 8l5.647 5.646a.5.5 0 0 1-.708.708l-6-6a.5.5 0 0 1 0-.708l6-6a.5.5 0 0 1 .708 0z"/>
</svg>`, `<svg xmlns="http://www.w3.org/2000/svg" width="28" height="28" fill="currentColor" class="bi bi-chevron-right" viewBox="0 0 16 16">
  <path fill-rule="evenodd" d="M4.646 1.646a.5.5 0 0 1 .708 0l6 6a.5.5 0 0 1 0 .708l-6 6a.5.5 0 0 1-.708-.708L10.293 8 4.646 2.354a.5.5 0 0 1 0-.708z"/>
</svg>`],
                dots: true,
                smartSpeed: 400,
                autoplay: false,
                autoplayTimeout: 15000,
                rtl: true
            };
            if (typeof $wrap == 'undefined') {
                $wrap = $('body');
            }
            if (options) {
                owlSettings = $.extend({}, owlSettings, options);
            }

            // Init all carousel
            $wrap.find('[data-toggle="owl"]').each(function () {
                var $this = $(this),
                    newOwlSettings = $.extend({}, owlSettings, $this.data('owl-options'));

                $this.owlCarousel(newOwlSettings);

            });
        }
    }
    $('.product-gallery-item').on('click', function () {
        $('#product-zoom-gallery').find('a').removeClass('active');
        var img = $(this).attr('data-image');
        $(this).addClass('active');
        $('#product-zoom').attr({ 'src': img, 'data-zoom-image': img })

    });
    // Product Image Zoom plugin - product pages
    if ($.fn.elevateZoom) {
        $('#product-zoom').elevateZoom({
            gallery: 'product-zoom-gallery',
            galleryActiveClass: 'active',
            zoomType: "inner",
            cursor: "crosshair",
            zoomWindowFadeIn: 400,
            zoomWindowFadeOut: 400,
            responsive: true
        });

        // On click change thumbs active item
        $('.product-gallery-item').on('click', function () {
            $('#product-zoom-gallery').find('a').removeClass('active');
            $(this).addClass('active');


        });

        var ez = $('#product-zoom').data('elevateZoom');

        // Open popup - product images
        $('#btn-product-gallery').on('click', function (e) {
            if ($.fn.magnificPopup) {
                $.magnificPopup.open({
                    items: ez.getGalleryList(),
                    type: 'image',
                    gallery: {
                        enabled: true
                    },
                    fixedContentPos: false,
                    removalDelay: 600,
                    closeBtnInside: false
                }, 0);

                e.preventDefault();
            }
        });
    }

    $('#btn-product-gallery').on('click', function () {

        var img = $('#product-zoom').attr('src');
        var imgTag = `<img src="${img}" />`
        $('#signin-modal').modal('show').find('.modal-body').html(imgTag)
    });

    // Product Gallery - product-gallery.html 
    if ($.fn.owlCarousel && $.fn.elevateZoom) {
        var owlProductGallery = $('.product-gallery-carousel');

        owlProductGallery.on('initialized.owl.carousel', function () {
            owlProductGallery.find('.active img').elevateZoom({
                zoomType: "inner",
                cursor: "crosshair",
                zoomWindowFadeIn: 400,
                zoomWindowFadeOut: 400,
                responsive: true
            });
        });

        owlProductGallery.owlCarousel({
            loop: false,
            margin: 0,
            responsiveClass: true,
            nav: true,
            navText: ['<i class="icon-angle-left">', '<i class="icon-angle-right">'],
            dots: false,
            smartSpeed: 400,
            autoplay: false,
            autoplayTimeout: 15000,
            responsive: {
                0: {
                    items: 1
                },
                560: {
                    items: 2
                },
                992: {
                    items: 3
                },
                1200: {
                    items: 3
                }
            }
        });

        owlProductGallery.on('change.owl.carousel', function () {
            $('.zoomContainer').remove();
        });

        owlProductGallery.on('translated.owl.carousel', function () {
            owlProductGallery.find('.active img').elevateZoom({
                zoomType: "inner",
                cursor: "crosshair",
                zoomWindowFadeIn: 400,
                zoomWindowFadeOut: 400,
                responsive: true
            });
        });
    }

    // Product Gallery Separeted- product-sticky.html 
    if ($.fn.elevateZoom) {
        $('.product-separated-item').find('img').elevateZoom({
            zoomType: "inner",
            cursor: "crosshair",
            zoomWindowFadeIn: 400,
            zoomWindowFadeOut: 400,
            responsive: true
        });

        // Create Array for gallery popup
        var galleryArr = [];
        $('.product-gallery-separated').find('img').each(function () {
            var $this = $(this),
                imgSrc = $this.attr('src'),
                imgTitle = $this.attr('alt'),
                obj = {
                    'src': imgSrc,
                    'title': imgTitle
                };

            galleryArr.push(obj);
        })

        $('#btn-separated-gallery').on('click', function (e) {
            if ($.fn.magnificPopup) {
                $.magnificPopup.open({
                    items: galleryArr,
                    type: 'image',
                    gallery: {
                        enabled: true
                    },
                    fixedContentPos: false,
                    removalDelay: 600,
                    closeBtnInside: false
                }, 0);

                e.preventDefault();
            }
        });
    }

    // Checkout discount input - toggle label if input is empty etc...
    $('#discountCuponCode').on('focus', function () {
        // Hide label on focus
        $(this).parent('form').find('label').css('opacity', 0);
        $(this).parent('form').find('button').show();
    }).on('blur', function () {
        // Check if input is empty / toggle label
        var $this = $(this);

        if ($this.val().length !== 0) {
            $this.parent('form').find('label').css('opacity', 0);
            $(this).parent('form').find('button').show();
        } else {
            $this.parent('form').find('label').css('opacity', 1);
            $(this).parent('form').find('button').hide();
        }
    });

    // Dashboard Page Tab Trigger
    $('.tab-trigger-link').on('click', function (e) {
        var targetHref = $(this).attr('href');

        $('.nav-dashboard').find('a[href="' + targetHref + '"]').trigger('click');

        e.preventDefault();
    });

    // Masonry / Grid layout fnction
    function layoutInit(container, selector) {
        $(container).each(function () {
            var $this = $(this);

            $this.isotope({
                itemSelector: selector,
                layoutMode: ($this.data('layout') ? $this.data('layout') : 'masonry')
            });
        });
    }

    function isotopeFilter(filterNav, container) {
        $(filterNav).find('a').on('click', function (e) {
            var $this = $(this),
                filter = $this.attr('data-filter');

            // Remove active class
            $(filterNav).find('.active').removeClass('active');

            // Init filter
            $(container).isotope({
                filter: filter,
                transitionDuration: '0.7s'
            });

            // Add active class
            $this.closest('li').addClass('active');
            e.preventDefault();
        });
    }

    /* Masonry / Grid Layout & Isotope Filter for blog/portfolio etc... */
    if (typeof imagesLoaded === 'function' && $.fn.isotope) {
        // Portfolio
        $('.portfolio-container').imagesLoaded(function () {
            // Portfolio Grid/Masonry
            layoutInit('.portfolio-container', '.portfolio-item'); // container - selector
            // Portfolio Filter
            isotopeFilter('.portfolio-filter', '.portfolio-container'); //filterNav - .container
        });

        // Blog
        $('.entry-container').imagesLoaded(function () {
            // Blog Grid/Masonry
            layoutInit('.entry-container', '.entry-item'); // container - selector
            // Blog Filter
            isotopeFilter('.entry-filter', '.entry-container'); //filterNav - .container
        });

        // Product masonry product-masonry.html
        $('.product-gallery-masonry').imagesLoaded(function () {
            // Products Grid/Masonry
            layoutInit('.product-gallery-masonry', '.product-gallery-item'); // container - selector
        });

        // Products - Demo 11
        $('.products-container').imagesLoaded(function () {
            // Products Grid/Masonry
            layoutInit('.products-container', '.product-item'); // container - selector
            // Product Filter
            isotopeFilter('.product-filter', '.products-container'); //filterNav - .container
        });
    }

    // Count
    var $countItem = $('.count');
    if ($.fn.countTo) {
        if ($.fn.waypoint) {
            $countItem.waypoint(function () {
                $(this.element).countTo();
            }, {
                offset: '90%',
                triggerOnce: true
            });
        } else {
            $countItem.countTo();
        }
    } else {
        // fallback
        // Get the data-to value and add it to element
        $countItem.each(function () {
            var $this = $(this),
                countValue = $this.data('to');
            $this.text(countValue);
        });
    }

    // Scroll To button
    var $scrollTo = $('.scroll-to');
    // If button scroll elements exists
    if ($scrollTo.length) {
        // Scroll to - Animate scroll
        $scrollTo.on('click', function (e) {
            var target = $(this).attr('href'),
                $target = $(target);
            if ($target.length) {
                // Add offset for sticky menu
                var scrolloffset = ($(window).width() >= 992) ? ($target.offset().top - 52) : $target.offset().top
                $('html, body').animate({
                    'scrollTop': scrolloffset
                }, 600);
                e.preventDefault();
            }
        });
    }

    // Review tab/collapse show + scroll to tab
    $('#review-link').on('click', function (e) {
        var target = $(this).attr('href'),
            $target = $(target);

        if ($('#product-accordion-review').length) {
            $('#product-accordion-review').collapse('show');
        }

        if ($target.length) {
            // Add offset for sticky menu
            var scrolloffset = ($(window).width() >= 992) ? ($target.offset().top - 72) : ($target.offset().top - 10)
            $('html, body').animate({
                'scrollTop': scrolloffset
            }, 600);

            $target.tab('show');
        }

        e.preventDefault();
    });

    // Scroll Top Button - Show
    var $scrollTop = $('#scroll-top , #call');
    var $justScroll = $('#scroll-top');

    $(window).on('load scroll', function () {
        if ($(window).scrollTop() >= 400) {
            $scrollTop.addClass('show');
        } else {
            $scrollTop.removeClass('show');
        }
    });

    // On click animate to top
    $justScroll.on('click', function (e) {
        $('html, body').animate({
            'scrollTop': 0
        }, 800);
        e.preventDefault();
    });

    // Google Map api v3 - Map for contact pages
    //if (document.getElementById("map") && typeof google === "object") {

    //    var content = '<address>' +
    //        '88 Pine St,<br>' +
    //        'New York, NY 10005, USA<br>' +
    //        '<a href="#" class="direction-link" target="_blank">Get Directions <i class="icon-angle-right"></i></a>' +
    //        '</address>';

    //    var latLong = new google.maps.LatLng(40.8127911, -73.9624553);

    //    var map = new google.maps.Map(document.getElementById('map'), {
    //        zoom: 14,
    //        center: latLong, // Map Center coordinates
    //        scrollwheel: false,
    //        mapTypeId: google.maps.MapTypeId.ROADMAP

    //    });

    //    var infowindow = new google.maps.InfoWindow({
    //        content: content,
    //        maxWidth: 360
    //    });

    //    var marker;
    //    marker = new google.maps.Marker({
    //        position: latLong,
    //        map: map,
    //        animation: google.maps.Animation.DROP
    //    });

    //    google.maps.event.addListener(marker, 'click', (function (marker) {
    //        return function () {
    //            infowindow.open(map, marker);
    //        }
    //    })(marker));
    //}

    var $viewAll = $('.view-all-demos');
    $viewAll.on('click', function (e) {
        e.preventDefault();
        $('.demo-item.hidden').addClass('show');
        $(this).addClass('disabled-hidden');
    })

    var $megamenu = $('.megamenu-container .sf-with-ul');
    $megamenu.hover(function () {
        $('.demo-item.show').addClass('hidden');
        $('.demo-item.show').removeClass('show');
        $viewAll.removeClass('disabled-hidden');
    });





    // Product quickView popup
    //$('.btn-quickview').on('click', function (e) {
    //    
    //    var ajaxUrl = $(this).attr('href').split('/')[2];
    //    if ($.fn.magnificPopup) {
    //        setTimeout(function () {
    //            $.magnificPopup.open({
    //                type: 'ajax',
    //                mainClass: "mfp-ajax-product",
    //                tLoading: '',
    //                preloader: false,
    //                removalDelay: 350,
    //                items: {
    //                    src: '/Product/QuickView/' + ajaxUrl
    //                },
    //                callbacks: {
    //                    ajaxContentAdded: function (res) {
    //                        $('.mfp-content').html(res)
    //                    },
    //                    open: function () {
    //                        $('body').css('overflow-x', 'hidden');
    //                        $('.sticky-header.fixed').css('padding-right', '1.7rem');
    //                    },
    //                    close: function () {
    //                        $('body').css('overflow-x', 'hidden');
    //                        $('.sticky-header.fixed').css('padding-right', '0');
    //                    }
    //                },

    //                ajax: {
    //                    tError: '',
    //                }
    //            }, 0);
    //        }, 500);

    //        e.preventDefault();
    //    }
    //});
    $('body').on('click', '.carousel-dot', function () {
        $(this).siblings().removeClass('active');
        $(this).addClass('active');
    });

    $('body').on('click', '.btn-fullscreen', function (e) {
        var galleryArr = [];
        $(this).parents('.owl-stage-outer').find('.owl-item:not(.cloned)').each(function () {
            var $this = $(this).find('img'),
                imgSrc = $this.attr('src'),
                imgTitle = $this.attr('alt'),
                obj = {
                    'src': imgSrc,
                    'title': imgTitle
                };
            galleryArr.push(obj);
        });

        var ajaxUrl = $(this).attr('href');

        var mpInstance = $.magnificPopup.instance;
        if (mpInstance.isOpen)
            mpInstance.close();

        setTimeout(function () {
            $.magnificPopup.open({
                type: 'ajax',
                mainClass: "mfp-ajax-product",
                tLoading: '',
                preloader: false,
                removalDelay: 350,
                items: {
                    src: ajaxUrl
                },
                callbacks: {
                    ajaxContentAdded: function () {
                        owlCarousels($('.quickView-content'), {
                            onTranslate: function (e) {
                                var $this = $(e.target),
                                    currentIndex = ($this.data('owl.carousel').current() + e.item.count - Math.ceil(e.item.count / 2)) % e.item.count;
                                $('.quickView-content .carousel-dot').eq(currentIndex).addClass('active').siblings().removeClass('active');
                                $('.curidx').html(currentIndex + 1);
                            }
                        });
                        quantityInputs();
                    },
                    open: function () {
                        $('body').css('overflow-x', 'hidden');
                        $('.sticky-header.fixed').css('padding-right', '1.7rem');
                    },
                    close: function () {
                        $('body').css('overflow-x', 'hidden');
                        $('.sticky-header.fixed').css('padding-right', '0');
                    }
                },

                ajax: {
                    tError: '',
                }
            }, 0);
        }, 500);

        e.preventDefault();
    });

    //if (document.getElementById('newsletter-popup-form')) {
    //    setTimeout(function () {
    //        var mpInstance = $.magnificPopup.instance;
    //        if (mpInstance.isOpen) {
    //            mpInstance.close();
    //        }

    //        setTimeout(function () {
    //            $.magnificPopup.open({
    //                items: {
    //                    src: '#newsletter-popup-form'
    //                },
    //                type: 'inline',
    //                removalDelay: 350,
    //                callbacks: {
    //                    open: function () {
    //                        $('body').css('overflow-x', 'hidden');
    //                        $('.sticky-header.fixed').css('padding-right', '1.7rem');
    //                    },
    //                    close: function () {
    //                        $('body').css('overflow-x', 'hidden');
    //                        $('.sticky-header.fixed').css('padding-right', '0');
    //                    }
    //                }
    //            });
    //        }, 500)
    //    }, 10000)
    //}
});

countShopCart = () => {
    $.get('/Api/Shop', (res) => {
        $("#countShopCart").text(res);
    })
}

showPreloader = () => {
    $('#overlayer').addClass('show').html(loader);
    $('body').addClass('freeze')
}

hidePreloader = () => {
    $('#overlayer,#overlayerInMobile').removeClass('show').html("");
    $('body,html').removeClass('freeze')
}

$('#showCart a').on('click', () => {
    showCartInMobile();
})

addToCart = (id) => {
    showPreloader();
    $.get("/Api/Shop/" + id, () => {
        hidePreloader();
        $("#showCart , #showCart2,#showCart3").load("/ShopCart/ShowCart", () => {
            showCartInMobile();
        }).fadeOut(200).fadeIn(200);
        $('#toast').toast('show').addClass('success').removeClass('liked-list').find('.toast-body').html(`<a class="text-white" href="/shopcart">محصول با موفقیت به سبد خرید اضافه شد</a>`);
                
        $.ajax({
            url: '/ShopCart/LogAddtoCart',
            type: 'POST',
            data: { productId: id, url: location.href },
            success: () => {
                console.log("cart logged")
            },
            error: err => console.log(err)
        });
    });
}

//function updateShopCart() {
//    $("#showCart , #showCart2").load("/ShopCart/ShowCart").fadeOut(500).fadeIn(500);
//}

addToCompareList = (id) => {
    $.get("/Compare/AddToCompare/" + id, (res) => {
        $("#compare").html(res);
        $('#input-' + id).attr('onchange', 'deleteFromCompareList(' + id + ')');
    })
}

addToWishList = (id) => {
    if ($('body').hasClass('logged-in')) {
        $('#overlayer').addClass('show').html(loader);
        $.get("/WishList/AddToWish/" + id, (res) => {
            $("#wishList-header").html(res);
            $('#toast').toast('show').addClass('liked-list').removeClass('success').find('.toast-body').html('<a href="/WishList">محصول با موفقیت به لیست علاقه مندی افزوده شد.</a>');
            $('#icon-heart').css('color', 'red');
            $('#overlayer').removeClass('show').html("");
        })
    } else {
        $('#signin-modal').modal('show').find('.modal-body').html('لطفا ابتدا وارد حساب کاربری خود شوید').css({ 'padding': '20px', 'text-align': 'center' });
        setTimeout(function () { window.location.href = '/Login'; }, 2000)
    }
    //$('#wish-' + id).attr('onclick', 'deleteFromWishList(' + id + ')');
    //$('#wish-' + id).attr('isChecked', true).find('img').attr('src', '/content/img/red-heart.png');
    //var likeCount = parseInt($('#likeCount').html());
    //var like1 = likeCount + 1;
    //$('#likeCount').html(like1);
}

deleteFromCompareList = (id) => {
    $.get("/Compare/DeleteFromCompareList/" + id, (res) => {
        $("#compare").html(res);
        $('#input-' + id).attr('onchange', 'addToCompareList(' + id + ')');
        $('#input-' + id).attr('checked', false)
    })
}

deleteFromWishList = (id) => {
    var count = +($('#wishList-header').find('.wishlist-count').html());
    count -= 1;
    $.get("/WishList/DeleteFromWishList/" + id, (res) => {
        $("#wishList").html(res);
        $('#wishList-header').find('.wishlist-count').html(count);
    })
    //$('#wish-' + id).attr('onclick', 'addToWishList(' + id + ')');
    //$('#wish-' + id).attr('isChecked', false).find('img').attr('src', '/content/img/white-heart.png');
    //var likeCount = parseInt($('#likeCount').html());
    //var like1 = likeCount - 1;
    //$('#likeCount').html(like1);
}

function enablizeBtn(e, id) {
    if (e.target.checked) {
        $(`#${id}`).attr('disabled', false)
    } else {
        $(`#${id}`).attr('disabled', true)
    }
}

searchSuggestion = (e) => {

    var text = $('#searchInput').val()
    if (text != "" || text != null) {

        showOverlayer();
        
        if (text.length > 3) {
            $('#searchSuggestion').show().html(loader);
            $('.header-search-wrapper').addClass('suggestion-open')
            setTimeout(() => {
                $.ajax('/Search/SearchSuggestion', {
                    type: 'GET',
                    data: { q: text },
                    success: (res) => {
                        $('#searchSuggestion').show().html(res)
                    }
                });
            }, 0)
        }
    }
    if (text == "" || text == null || text == " ") {
        $('#searchSuggestion').html("").hide();
        $('.header-search-wrapper').removeClass('suggestion-open')
        hideOverlayer();
    }
}

$('.overlayer.show').click(() => {
    hideOverlayer();
    $('#searchSuggestion').hide().html("");
    $('.header-search-wrapper').removeClass('suggestion-open')
})



function successComment() {

    $("#Name").val("");
    $("#Email").val("");

    $("#Comment").val("");
    $('#ParentID').val("");
}

function answerComment(id) {
    $('#ParentID').val(id);
    $('html,body').delay(500).animate({ scrollTop: $("#commentProduct").offset().top }, 500);
}

$('body,html').scroll(() => {
    if ($('#sticky-bar-product').scrollTop() == 500) {
        $('#sticky-bar-product').show();
    } else {
        $('#sticky-bar-product').hide();
    }
})

$(window).on('load scroll', function () {
    if ($(window).scrollTop() >= 400) {
        $('#sticky-bar-product').addClass('show');
    } else {
        $('#sticky-bar-product').removeClass('show');
    }
})

function changePage(pageId) {
    $("#pageId").val(pageId);
    $("#filterForm").submit();
}

function showOverlayer() {
    $('#overlayer').addClass('show');
    $('body,html').addClass('freeze');
    $('.overlayer.show').click(() => {
        hideOverlayer();
        $('#searchSuggestion').hide().html("");
        hideCartInMobile()
    })
}

function showOverlayerInMobile() {
    $('#overlayerInMobile').addClass('show');
    if (window.innerWidth < 768) $('body,html').addClass('freeze');
    $('.overlayer.show').click(() => {
        hideOverlayer();
        $('#searchSuggestion').hide().html("");
        hideCartInMobile()
    })
}

function hideOverlayer() {
    $('#overlayer,#overlayerInMobile').removeClass('show');
    $('body,html').removeClass('freeze');
    $('#searchSuggestion').hide().html("");
}

quickView = (id) => {
    $('#signin-modal').modal('show');
    $('.modal-body').html(loader);
    $.get('/Product/QuickView/' + id, (res) => {
        $('.modal-body').html(res);
    })
}

updateShopCart = () => {
    $.get('/ShopCart/Order', (res) => {
        $('#showOrder').html(res);
        quantityInputs();
        showFreeShipping();
    })
}

submitDiscountCode = (e) => {
    e.preventDefault();
    if ($('body').hasClass('logged-in')) {
        var code = $('#discountCuponCode').val();
        $.ajax('/ShopCart/DiscountCodeChecker', {

            type: 'POST',
            data: { dCode: code },
            success: (res) => {
                var isSuccess = false;
                if (res != "" && res != null && res != " ") {
                    $('#showOrder').html(res);
                    isSuccess = true
                }
                const mes = isSuccess ? "کد تخفیف با موفقیت اعمال شد." : "چنین کد تخفیفی در حال حاضر وجود ندارد.";
                $('#toast').toast('show').addClass('bg-dark text-white').find('.toast-body').html(mes);
            }
        });
    } else {
        $('#signin-modal').modal('show').find('.modal-body').html('لطفا ابتدا وارد حساب کاربری خود شوید').css({ 'padding': '20px', 'text-align': 'center' });
        setTimeout(function () { window.location.href = '/Login?ReturnUrl=/ShopCart'; }, 2000)
    }
}


function commandOrder(id, count) {
    showPreloader();
    $('.cart-product-quantity input[type="number"]').attr('disabled', 'disabled');
    $.ajax({
        url: "/ShopCart/CommandOrder/" + id,
        type: "Get",
        data: { count: count }
    }).done((res) => {
        $("#showOrder").html(res);
        $("#showCart , #showCart2,#showCart3").load("/ShopCart/ShowCart").fadeOut(500).fadeIn(500);
        quantityInputs();
        setTimeout(() => { hidePreloader() }, 1)
    });

}

function quantityInputs() {
    if ($.fn.inputSpinner) {
        $("input[type='number']").inputSpinner({
            decrementButton: '<i class="icon-minus">-</i>',
            incrementButton: '<i class="icon-plus">+</i>',
            groupClass: 'input-spinner',
            buttonsClass: 'btn-spinner',
            buttonsWidth: '26px'
        });
    }
}

function Func(Shahrestanha) {
    const state = $('#Ostan').find('option[value="' + Shahrestanha + '"]').attr('data-value');
    var _Shahrestan = document.getElementById('Shahrestan');
    _Shahrestan.options.length = 0;
    if (Shahrestanha != "") {
        var arr = state.split(",");
        for (i = 0; i < arr.length; i++) {
            if (arr[i] != "") {
                _Shahrestan.options[_Shahrestan.options.length] = new Option(arr[i], arr[i]);
            }
        }
    }
}

payWay = (int, id) => {
    var btnPay = $('#btnPay');
    if (int == 1) {
        btnPay.attr('href', '/Pay/Pay?orderId=' + id).html('انتقال به درگاه بانکی')
    } else if (int == 2) {
        btnPay.attr('href', '/ShopCart/PayWithCardNo/' + id).html('نمایش شماره کارت')
    }
}

//showFreeShipping = () => {
//    let tCost = $('#tCost').val();
//    if (tCost >= 1000000) {
//        $('#free-shipping-tr').show();
//    }
//}

//$(window).ready(() => {
//    showFreeShipping();
//})

shipping = (id) => {
    //var prim = +($('#free-shipping-tr').next().find('.shipping-price').html().replace(',', ''));
    showPreloader();
    $.ajax({
        url: "/ShopCart/Shipping/",
        type: "Get",
        data: { id: id }
    }).done((res) => {
        $("#showOrder").html(res);
        $('input.custom-control-input[value="' + id + '"]').attr('checked', true)
        quantityInputs();
        hidePreloader()
        //showFreeShipping()
    });
}

getEmailNumber = (e) => {
    $.ajax({
        url: "/Home/Lead",
        type: "Get",
        data: { num: $('#lead-mobile').val() }
    }).done(() => {
        $('#lead-mobile').val('');
        alert('موبایل شما با موفقیت دریافت شد!');
    });
    e.preventDefault();
}

showThisImage = (e) => {
    $('.product-gallery-item').removeClass('active');
    $(e).parent().addClass('active');
    var img = $(e).find('img').attr('src');
    var bigImg = img.replace('/330', '');
    $('#qv-big-image').attr('src', bigImg)
}

showHumanBody = () => {
    $('#signin-modal').modal('show').find('.modal-dialog').addClass('humanBody').find('.modal-body').html(loader);
    $.get('/Home/HumanBody', (res) => {
        $('.modal-body').html(res)
    })
}

function getSorted(selector, attrName) {
    return $($(selector).toArray().sort(function (a, b) {
        var aVal = parseInt(a.children[0].getAttribute(attrName)),
            bVal = parseInt(b.children[0].getAttribute(attrName));
        return aVal - bVal;
    }));
}


function getDeSorted(selector, attrName) {
    return $($(selector).toArray().sort(function (a, b) {
        var aVal = parseInt(a.children[0].getAttribute(attrName)),
            bVal = parseInt(b.children[0].getAttribute(attrName));
        return bVal - aVal;
    }));
}

sortProducts = (type) => {
    var dataType = 'data-date';
    if (type == 'theMostExpensive') {
        dataType = 'data-price';
        $('#prodcutContainer').html(getDeSorted('.product-item', dataType));
    } else if (type == 'theCheapest') {
        dataType = 'data-price';
        $('#prodcutContainer').html(getSorted('.product-item', dataType));
    } else {
        dataType = 'data-date';
        $('#prodcutContainer').html(getSorted('.product-item', dataType));
    }
}


showOrderDetails = (id) => {
    $('.modal-body').html(loader)
    $('#signin-modal').modal('show');
    $.get('/Account/ShowOrderDetails/' + id, (res) => {
        $('.modal-body').html(res)
    })
}

submitArchiveForm = (id) => {
    debugger
    var min = $(id).find('#minPriceVal').val();
    var max = $(id).find('#maxPriceVal').val();
    if (+max >= +min) {
        $('#filterForm').submit();
    } else {
        $('#signin-modal').modal('show').find('.modal-body').html('فیلتر قابل اعمال نیست!').css({ 'text-align': 'center', 'padding': '25px', 'color': 'red' })
    }
}

//$('.call-for-price').on('click', () => {
//    if (window.outerWidth > 768) {
//        $('#signin-modal').modal('show').find('.modal-body').html('شماره تماس: 09120869780').css({ 'text-align': 'center', 'padding': '25px', 'color': 'blue' })
//    }
//})

copyUrl = () => {
    var dummy = document.createElement('input'),
        text = window.location.href;

    document.body.appendChild(dummy);
    dummy.value = text;
    dummy.select();
    document.execCommand('copy');
    document.body.removeChild(dummy);
    $('#toast').toast('show').addClass('bg-dark text-white').removeClass('success').find('.toast-body').html('لینک صفحه کپی شد')
}

$('#category-dropdown').on('mouseover', () => {
    $('#overlayer').addClass('show')
}).on('mouseleave', () => {
    $('#overlayer').removeClass('show')
})

$('img').contextmenu(function (e) {
    window.navigator.vibrate(300);
    $('#signin-modal').modal('show').find('.modal-body').html('با احترام ، شما اجازه دانلود و یا ذخیره این عکس را ندارید!' + "<br/>" + 'استفاده از محتوای این وبسایت تنها در صورت ذکر منبع مجاز است.')
    e.preventDefault();
});

$('.spl').on('click', () => {
    showPreloader();
})

getNumber = (id) => {
    $('#signin-modal').modal('show').find('.modal-body').html(loader);
    debugger;
    $.get('/Telegram/GetNumber/' + id, (res) => {
        $('#signin-modal').find('.modal-body').html(res);
    })
}

numberValidator = (x) => {
    let isValid;
    if (x.length == 11 && x.substring(0, 2) == '09') {
        isValid = true;
    }
    return isValid;
}

sendNumber = (e, id) => {
    var num = $('#phoneNumber').val();
    $('.modal-body').html(loader);
    e.preventDefault();
    if (numberValidator(num)) {
        $.ajax({
            url: '/Telegram/Start/' + id,
            type: 'POST',
            data: { num: num },
            success: (res) => {
                if (res) {
                    $('.modal-body').html("شماره شما با موفقیت دریافت شد<br>منتظر تماس همکاران ما باشید!").addClass('text-success');
                    setInterval(() => {
                        $('#signin-modal').modal('hide')
                    }, 5000)
                } else {
                    $('.modal-body').html("عملیات با شکست مواجه شد. لطفا بعدا تلاش کنید.").addClass('text-danger');
                    setInterval(() => {
                        $('#signin-modal').modal('hide')
                    }, 5000)
                }
            },
            error: err => $('.modal-body').html(err.message).addClass('text-danger')
        })
    } else {
        $('.modal-body').html("شماره ای که وارد کردید اشتباه است!").addClass('text-danger');
    }

}

goBack = (level) => {
    $(`.menu-level-${level}-li`).removeClass('open')
}

addEditAddress = (type, id) => {
    $('#signin-modal').modal('show').find('.modal-body').html(loader)
    if (type == 'add') {
        $.get('/account/AddAddress', (res) => {
            $('.modal-body').html(res)
        })
    } else if (type == 'edit') {
        $.get('/account/EditAddress/' + id, (res) => {
            $('.modal-body').html(res)

        })
    }
}

deleteAddress = (id) => {
    confirm("آیا از حذف این آدرس اطمینان دارید؟", () => {
        $.post('/account/DeleteAddress/' + id, (res) => {
            $('#AddressesList').html(res);
        })
    })
}

successAddesAdress = () => {
    $('#signin-modal').modal('hide')
}

setDefaultAddress = (id) => {
    showPreloader();
    $.post('/address/SetDefaultAddressWithReturn/' + id, (res) => {
        $('#AddressList').html(res);
        hidePreloader()
    })
}

setDefaultAddress1 = (id) => {
    showPreloader();
    $.post('/address/SetDefaultAddress/' + id, (res) => {
        location.reload();
    })
}

confirmDeleteAddress = (id) => {
    const mb = `<div>آیا از حذف این آدرس اطمینان دارید؟<br/><br/><a class="btn btn-danger m-1" onclick="deleteAddress(${id})">بله</a><a onclick="$('#signin-modal').modal('hide')" class="btn btn-outline-info text-info m-1">خیر</a></div>`;
    $('.modal-body').html(mb)
    $('#signin-modal').modal('show');
}

deleteAddress = (id) => {
    $.post('/address/delete/' + id, (res) => {
        $('#AddressList').html(res)
        $('#signin-modal').modal('hide');

    }).done(() => {
        checkDefaultAddress()
    })
}

checkDefaultAddress = () => {
    if ($('.address-row').length > 0 && $('.address-row.default-address').length < 1) {
        $('#signin-modal').modal('show').find('.modal-body').html('لطفا یک آدرس را به عنوان پیشفرض انتخاب کنید')
    }
}




function showRemaining(date) {
    var _second = 1000;
    var _minute = _second * 60;
    var _hour = _minute * 60;
    var _day = _hour * 24;
    var now = new Date();
    var distance = end - now;
    if (distance < 0) {

        clearInterval(timer);
        document.getElementById('countdown').innerHTML = 'EXPIRED!';

        return;
    }
    var days = Math.floor(distance / _day);
    var hours = Math.floor((distance % _day) / _hour);
    var minutes = Math.floor((distance % _hour) / _minute);
    var seconds = Math.floor((distance % _minute) / _second);

    document.getElementById('countdown').innerHTML = days + 'days ';
    document.getElementById('countdown').innerHTML += hours + 'hrs ';
    document.getElementById('countdown').innerHTML += minutes + 'mins ';
    document.getElementById('countdown').innerHTML += seconds + 'secs';
}

goToPage = (url) => {
    if (window.location.pathname != url) {
        window.location.href = url;
    }
}

const input = document.getElementById("search-input");
const searchBtn = document.getElementById("search-btn");

const expand = () => {
    searchBtn.classList.toggle("close");
    input.classList.toggle("square");
};

//searchBtn.addEventListener("click", expand);

mobileSearchSuggestion = (e) => {
    var mobileSearchSuggestion = $('#mobileSearchSuggestion');
    e.preventDefault();
    let val = $('#search').val();
    if (val.length > 3) {
        $('.last-searched-products').fadeOut();
        $('#searchAlert').hide().html('')
        if (mobileSearchSuggestion.children().length < 1) {
            mobileSearchSuggestion.html(loader)
        }
        else {
            $('#search-overlayer').show()
            mobileSearchSuggestion.addClass('blured')
        }
        $.ajax({
            url: '/search/MobileSearchSuggestion',
            type: 'POST',
            data: { q: val },
            success: (res) => {
                $('#search-overlayer').hide()
                mobileSearchSuggestion.removeClass('blured')
                mobileSearchSuggestion.html(res)
                $('#searched-word').html(val)
                if ($('#emptyResult').length > 0) {
                    $('.last-searched-products').fadeIn()
                }
            }
        })
    } else if (val.length <= 3 && val.length > 0) {
        $('#searchAlert').show().html('تعداد کاراکترهای وارد شده باید بیشتر از 3تا باشد.')
    } else {
        $('#searchAlert').hide().html('')
        mobileSearchSuggestion.html('')
        $('.last-searched-products').fadeIn()
    }
}

setLastSearchedCookies = (e, id) => {
    e.preventDefault();
    var href = $(e.currentTarget).attr('href');
    $.get('/search/SetLastSearchedCookie/' + id, () => {
        window.location.href = href;
    })
}

showDeliveryDescription = (id) => {
    if (id == 0) {
        $('#signin-modal').modal('show').find('.modal-body').html('برای سفارش های بالای یک میلیون تومان')
    } else {
        $('#signin-modal').modal('show').find('.modal-body').html(loader)
        $.get('/ShopCart/ShowDeliveryDescription/' + id, (res) => {
            $('.modal-body').html(res)
        })
    }
}

showCartInMobile = () => {
    $('#showCart3 .dropdown-menu').addClass('show');
    $('#goftino_w').fadeOut()
    $('.fixed-add-to-cart-in-product-page').removeClass('in-mobile').fadeOut();
    showOverlayerInMobile()
}

hideCartInMobile = () => {
    $('#showCart3 .dropdown-menu').removeClass('show');
    $('#goftino_w').fadeIn();
    $('.fixed-add-to-cart-in-product-page').addClass('in-mobile').fadeIn();
    hideOverlayer()
}

$('#showCart3 .dropdown-toggle').on('click', () => {
    showCartInMobile()
})

$('.btn-close-wrapper').on('click', () => {
    hideCartInMobile()
})


