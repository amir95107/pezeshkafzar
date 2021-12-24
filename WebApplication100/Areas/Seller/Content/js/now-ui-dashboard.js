var loader = '<div class="text-center"><i class="now-ui-icons loader_refresh spin"></i></div>';

(function () {
    isWindows = navigator.platform.indexOf('Win') > -1 ? true : false;

    if (isWindows) {
        // if we are on windows OS we activate the perfectScrollbar function
        var ps = new PerfectScrollbar('.sidebar-wrapper');
        var ps2 = new PerfectScrollbar('.main-panel');

        $('html').addClass('perfect-scrollbar-on');
    } else {
        $('html').addClass('perfect-scrollbar-off');
    }
})();

transparent = true;
transparentDemo = true;
fixedTop = false;

navbar_initialized = false;
backgroundOrange = false;
sidebar_mini_active = false;
toggle_initialized = false;

var is_iPad = navigator.userAgent.match(/iPad/i) != null;
var scrollElement = navigator.platform.indexOf('Win') > -1 ? $(".main-panel") : $(window);

seq = 0, delays = 80, durations = 500;
seq2 = 0, delays2 = 80, durations2 = 500;

$(document).ready(function () {

    if ($('.full-screen-map').length == 0 && $('.bd-docs').length == 0) {
        // On click navbar-collapse the menu will be white not transparent
        $('.collapse').on('show.bs.collapse', function () {
            $(this).closest('.navbar').removeClass('navbar-transparent').addClass('bg-white');
        }).on('hide.bs.collapse', function () {
            $(this).closest('.navbar').addClass('navbar-transparent').removeClass('bg-white');
        });
    }

    $navbar = $('.navbar[color-on-scroll]');
    scroll_distance = $navbar.attr('color-on-scroll') || 500;

    // Check if we have the class "navbar-color-on-scroll" then add the function to remove the class "navbar-transparent" so it will transform to a plain color.
    if ($('.navbar[color-on-scroll]').length != 0) {
        nowuiDashboard.checkScrollForTransparentNavbar();
        $(window).on('scroll', nowuiDashboard.checkScrollForTransparentNavbar)
    }

    $('.form-control').on("focus", function () {
        $(this).parent('.input-group').addClass("input-group-focus");
    }).on("blur", function () {
        $(this).parent(".input-group").removeClass("input-group-focus");
    });

    // Activate bootstrapSwitch
    $('.bootstrap-switch').each(function () {
        $this = $(this);
        data_on_label = $this.data('on-label') || '';
        data_off_label = $this.data('off-label') || '';

        $this.bootstrapSwitch({
            onText: data_on_label,
            offText: data_off_label
        });
    });
});

$(document).on('click', '.navbar-toggle', function () {
    $toggle = $(this);

    if (nowuiDashboard.misc.navbar_menu_visible == 1) {
        $('html').removeClass('nav-open');
        nowuiDashboard.misc.navbar_menu_visible = 0;
        setTimeout(function () {
            $toggle.removeClass('toggled');
            $('#bodyClick').remove();
        }, 550);

    } else {
        setTimeout(function () {
            $toggle.addClass('toggled');
        }, 580);

        div = '<div id="bodyClick"></div>';
        $(div).appendTo('body').click(function () {
            $('html').removeClass('nav-open');
            nowuiDashboard.misc.navbar_menu_visible = 0;
            setTimeout(function () {
                $toggle.removeClass('toggled');
                $('#bodyClick').remove();
            }, 550);
        });

        $('html').addClass('nav-open');
        nowuiDashboard.misc.navbar_menu_visible = 1;
    }
});

$(window).resize(function () {
    // reset the seq for charts drawing animations
    seq = seq2 = 0;

    if ($('.full-screen-map').length == 0 && $('.bd-docs').length == 0) {

        $navbar = $('.navbar');
        isExpanded = $('.navbar').find('[data-toggle="collapse"]').attr("aria-expanded");
        if ($navbar.hasClass('bg-white') && $(window).width() > 991) {
            if (scrollElement.scrollTop() == 0) {
                $navbar.removeClass('bg-white').addClass('navbar-transparent');
            }
        } else if ($navbar.hasClass('navbar-transparent') && $(window).width() < 991 && isExpanded != "false") {
            $navbar.addClass('bg-white').removeClass('navbar-transparent');
        }
    }
    if (is_iPad) {
        $('body').removeClass('sidebar-mini');
    }
});

nowuiDashboard = {
    misc: {
        navbar_menu_visible: 0
    },

    showNotification: function (from, align) {
        color = 'primary';

        $.notify({
            icon: "now-ui-icons ui-1_bell-53",
            message: "Welcome to <b>Now Ui Dashboard</b> - a beautiful freebie for every web developer."

        }, {
            type: color,
            timer: 8000,
            placement: {
                from: from,
                align: align
            }
        });
    }


};

function hexToRGB(hex, alpha) {
    var r = parseInt(hex.slice(1, 3), 16),
        g = parseInt(hex.slice(3, 5), 16),
        b = parseInt(hex.slice(5, 7), 16);

    if (alpha) {
        return "rgba(" + r + ", " + g + ", " + b + ", " + alpha + ")";
    } else {
        return "rgb(" + r + ", " + g + ", " + b + ")";
    }
}

toggleDiv = (id) => {
    $(id).slideToggle('slow')
}

searchBrand = (brand) => {
    var items = $('.brand-item');
    items.fadeOut();
    items.each((i, e) => {
        if ($(e).attr('data-brand-name').indexOf(brand) != -1) {
            $(e).fadeIn()
        }
    })
}

showEditBrandModal = (id) => {
    $('#AllModal').modal('show');
    $.get('/Admin/Products/EditProductBrand/' + id, (res) => {
        $('.modal-body').html(res)
    })
}

$(function () {
    $('[data-toggle="tooltip"]').tooltip();
    
        var href = window.location.href;
    var page = href.split('/')[4].toLowerCase();
    if (page == null || page == undefined) {
        page = 'admin';
    }
    if (page == 'orders' && href.split('/')[5] == 'reports') {
        page = 'reports';
    }
        var navs = $('.sidebar-wrapper .nav .nav-item');
        navs.removeClass('active');
        
        $('.nav-item[data-page="' + page + '"]').addClass('active');
    

})

confirmDeleteBlog = (id) => {
    var con = confirm('آیا از حذف این بلاگ مطمئنید؟');
    if (con == true) {
        $.get('/Admin/Blogs/Delete/' + id, () => {

        });
    }
}

showDetails = (id) => {
    $('#AllModal').modal('show');
    $.get('/admin/orders/ShowDetails/' + id, (res) => {
        $('.modal-body').html(res)
    })
}

DeleteOrder = (id) => {
    $('#AllModal').modal('show');
    $.get('/admin/orders/Delete/' + id, (res) => {
        $('.modal-body').html(res)
    })
}

finalizeOrder = (id) => {
    $.get('/admin/orders/FinalizeOrder/' + id, (res) => {
        $('#orderTable').html(res)
    })
}

reportSelling = (id) => {
    $('#reportTable').html(loader);
    $.get('/admin/orders/ReportOrders/' + id, (res) => {
        $('#reportTable').html(res);
        $('.dataTable').DataTable()
    })
}

showUserInformation = (id) => {
    $('#AllModal').modal('show').find('.modal-dialog').attr('data-modal','UserInformation');
    $.get('/admin/Users/GetUserInformation/' + id, (res) => {
        $('.modal-body').html(res);
    })
}

editUserInformation = (id) => {
    $('.modal-body').html(loader);
    $.get('/admin/Users/EditUserInformation/' + id, (res) => {
        $('.modal-body').html(res);
    })
}

saveChangesOfUserInformation = (id, e) => {
    e.preventDefault();
    $.ajax({
        url: "/admin/Users/EditUserInformation/" + id,
        type: "POST",
        success: editUserInformation(id)
    })
}

getUsers = (id) => {
    $('#usersTable').html(loader);
    $.get('/admin/Users/UsersTable/' + id, (res) => {
        $('#usersTable').html(res);
    })
    $('.dataTable').DataTable();
    $('#AllModal').modal('hide');
}

editUsers = (id) => {
    $('#AllModal').modal('show');
    $('.modal-body').html(loader);
    $.get('/admin/Users/EditUsers/' + id, (res) => {
        $('.modal-body').html(res);
    })
}

