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
    ;
    var href = window.location.href;
    var page = href.split('/')[4];
    if (page == null || page == undefined) {
        page = 'admin';
    }
    if (page.toLowerCase() == 'orders' && href.split('/')[5] == 'reports') {
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
    $('#AllModal').modal('show').find('.modal-dialog').attr('data-modal', 'UserInformation');
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

sellectSellerProducts = (id) => {
    $('#productsList').html(loader);
    $.get('/admin/Products/ProductsList/' + id, (res) => {
        $('#productsList').html(res);
        $('.dataTable').DataTable()
    })

}

$('.biggerSize').click((e) => {
    var src = e.currentTarget.currentSrc;
    var newSrc = src.replace('/330', '');
    var img = '<img src="' + newSrc + '" />';
    $('#AllModal').modal('show').find('.modal-body').html(img);
})

function Create(parentId) {
    $.get("/Admin/Product_Groups/Create/" + parentId,
        function (result) {
            $("#myModal").modal();
            $("#myModalLabel").html("افزودن گروه جدید");
            $("#myModalBody").html(result);
        });
}

function Edit(id) {
    $.get("/Admin/Product_Groups/Edit/" + id,
        function (result) {
            $("#myModal").modal('show');
            $("#myModalLabel").html("ویرایش گروه");
            $("#myModalBody").html(result);
        });
}

function Delete(id) {
    if (confirm("آیا از حذف مطمئن هستید ؟")) {
        $.get("/Admin/Product_Groups/Delete/" + id,
            function (res) {
                $("#listGroup").html(res);
            }).fail(() => {
                $('#myModal').modal('show');
                $('#myModalLabel').html('اخطار');
                $('.modal-body').html('امکان حذف این فیلد وجود ندارد!');
            });
    }
}



function success() {
    $("#myModal").modal('hide');
}

deleteExpiredOrders = () => {
    $('#orderTable').html(loader);
    var hour = $('#selectHour').val();
    ;
    $.ajax({
        url: '/Admin/Orders/DeleteExpiredOrders',
        type: 'GET',
        data: { hour: hour },
        success: (res) => {
            $('#orderTable').html(res);
        }
    })
}

deactiveProduct = (id) => {
    $('#overlayer').addClass('show').html(loader).css({ 'font-size': '3rem', 'color': '#fff' })
    $.get('/admin/products/DeactiveProduct/' + id, (res) => {
        $('#productsList').html(res);
        $('#overlayer').removeClass('show').html('');
        $('.dataTable').dataTable();
    })

}

activateProduct = (id) => {
    $('#overlayer').addClass('show').html(loader).css({ 'font-size': '3rem', 'color': '#fff' });
    $.get('/admin/products/ActivateProduct/' + id, (res) => {
        $('#productsList').html(res);
        $('#overlayer').removeClass('show').html('');
        $('.dataTable').dataTable();
    })
}

setODCondition = (id, num, orderId) => {
    $('#overlayer').addClass('show').html(loader).css({ 'font-size': '3rem', 'color': '#fff' });
    $('#od_' + id).attr('data-conditoionSelected', num).find('.condition').html((num == 1) ? "ارسال شد" : "موجود ندارم")
    $.ajax({
        type: 'GET',
        url: '/Admin/Orders/SetOrderDetailsCondition',
        data: { id: id, num: num, orderId: orderId },
        success: (res) => {
            $('#orderTable').html(res);
            var tr = $('.modal-body tbody tr');
            var sum = 0;
            for (var i = 0; i < tr.length; i++) {
                sum += parseInt(tr.eq(i).attr('data-conditoionSelected'));

            }
            if (sum == tr.length) {
                $('#AllModal').modal('hide')
            }
            $('#overlayer').removeClass('show').html('');
            $('.dataTable').dataTable();
        }
    })
}

showInModal = (src) => {
    var bigImage = src.replace('/330', '');
    const img = `<img src="${bigImage}">`;
    $('#AllModal').modal('show').find('.modal-body').addClass('text-center').html(img)
}

$('.show-in-modal').click((e) => {
    showInModal(e.currentTarget.src);
})

selectFromMedia = () => {
    $('#AllModal').modal('show').find('.modal-dialog').addClass('modal-dialog-scrollable').find('.modal-body').html(loader);
    $.get('/Admin/Home/MediaPartial', (res) => {
        $('.modal-body').html(res);
    })
}

selectThisImage = (e) => {
    var src = e.currentTarget.src.replace('/330', '');
    var _src = src.split('/')[5];
    $('#imgPreviewProduct').attr("src", src);
    $('#imageNameInput').val(_src);
    $('#imageProduct').remove();
    $('#AllModal').modal('hide')
}

var input = $('input[type="checkbox"]')

input.click((ee) => {
    if ($(ee.currentTarget).attr('checked', true)) {
        $(ee.currentTarget).parents('.group-level-1-li').children('input').attr('checked', true)
        var level = $(ee.currentTarget).attr('data-level');
        if (level == 3) {
            $(ee.currentTarget).parent().parent().parent().children('input').attr('checked', true)
        }
    }
})

showOrderDetails = (id) => {
    $('#AllModal').modal('show').find('.modal-dialog').find('.modal-body').html(loader);
    $.get('/Admin/Oredrs/ShowOredrDetails', (res) => {
        $('.modal-body').html(res);
    })
}

$('#productGroupSearch').on('input', () => {
    var text = $("#productGroupSearch").val();
    $('.pg_group3').each((i, e) => {
        if ($(e).html().indexOf(text) != -1) {
            $(e).parent().show()
        } else {
            $(e).parent().hide()

        }
    })
})