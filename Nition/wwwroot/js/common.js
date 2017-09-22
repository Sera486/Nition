$(window).scroll(function () {
    if ($(document).scrollTop() > 150) {
        $('.navbar').addClass('shrink');
        var color = '#303030';
        var rgbaCol = 'rgba(' + parseInt(color.slice(-6, -4), 16)
            + ',' + parseInt(color.slice(-4, -2), 16)
            + ',' + parseInt(color.slice(-2), 16)
            + ',0.7)';
        $('.navbar').css('background-color', rgbaCol);
        $('.navbar').css("transition", "1s");
        $('.userImgHead').css("padding-left", "10px");
    }
    else {
        $('.navbar').removeClass('shrink');
        $('.navbar').css("transition", "1s");
        $('.navbar').css("background-color", 'transparent');
    }
});
////////////////////////////////////////////////////////////////////

$(function () {
    $('.btn-circle').on('click', function () {
        $('.btn-circle.btn-info').removeClass('btn-info').addClass('btn-default');
        $(this).addClass('btn-info').removeClass('btn-default').blur();
    });

    $('.next-step, .prev-step').on('click', function (e) {
        var $activeTab = $('.tab-pane.active');

        $('.btn-circle.btn-info').removeClass('btn-info').addClass('btn-default');

        if ($(e.target).hasClass('next-step')) {
            var nextTab = $activeTab.next('.tab-pane').attr('id');
            $('[href="#' + nextTab + '"]').addClass('btn-info').removeClass('btn-default');
            $('[href="#' + nextTab + '"]').tab('show');
        }
        else {
            var prevTab = $activeTab.prev('.tab-pane').attr('id');
            $('[href="#' + prevTab + '"]').addClass('btn-info').removeClass('btn-default');
            $('[href="#' + prevTab + '"]').tab('show');
        }
    });
});

$(document).ready(function () {
    $('.carousel').carousel({
        interval: 1611000
    })
});

$(function () {
    $('.button-checkbox').each(function () {
        var $widget = $(this),
            $button = $widget.find('button'),
            $checkbox = $widget.find('input:checkbox'),
            color = $button.data('color'),
            settings = {
                on: {
                    icon: 'glyphicon glyphicon-check'
                },
                off: {
                    icon: 'glyphicon glyphicon-unchecked'
                }
            };

        $button.on('click', function () {
            $checkbox.prop('checked', !$checkbox.is(':checked'));
            $checkbox.triggerHandler('change');
            updateDisplay();
        });

        $checkbox.on('change', function () {
            updateDisplay();
        });

        function updateDisplay() {
            var isChecked = $checkbox.is(':checked');
            // Set the button's state
            $button.data('state', (isChecked) ? "on" : "off");

            // Set the button's icon
            $button.find('.state-icon')
                .removeClass()
                .addClass('state-icon ' + settings[$button.data('state')].icon);

            // Update the button's color
            if (isChecked) {
                $button
                    .removeClass('btn-default')
                    .addClass('btn-' + color + ' active');
            }
            else {
                $button
                    .removeClass('btn-' + color + ' active')
                    .addClass('btn-default');
            }
        }
        function init() {
            updateDisplay();
            // Inject the icon if applicable
            if ($button.find('.state-icon').length == 0) {
                $button.prepend('<i class="state-icon ' + settings[$button.data('state')].icon + '"></i> ');
            }
        }
        init();
    });
});

$(window).scroll(function () {
    $(".slideanim").each(function () {
        var pos = $(this).offset().top;

        var winTop = $(window).scrollTop();
        if (pos < winTop + 600) {
            $(this).addClass("slide");
        }
    });
});

function myMap() {
    var myCenter = new google.maps.LatLng(41.878114, -87.629798);
    var mapProp = { center: myCenter, zoom: 12, scrollwheel: false, draggable: false, mapTypeId: google.maps.MapTypeId.ROADMAP };
    var map = new google.maps.Map(document.getElementById("googleMap"), mapProp);
    var marker = new google.maps.Marker({ position: myCenter });
    marker.setMap(map);
}

///////////////// add COURSE text|vydeo //////////////////
function editBlock() {
    $("#editBlock").append('<div class="col-md-10 col-md-offset-1"><form><div class="form-group"><label for="comment">Змінити текст:</label><textarea class="form-control" rows="3" id="commentTextArea"></textarea><button style="margin-top:1vmin;margin-bottm:1vmin;" type="submit" class="btn btn-info pull-right">Підтвердити</button></div></form>');
};

window.onscroll = function () { scrollFunction() };

function scrollFunction() {
    if (document.body.scrollTop > 20 || document.documentElement.scrollTop > 20) {
        $("#BtnUp").css("display", "block");
    } else {
        $("#BtnUp").css("display", "none");
    }
}

// When the user clicks on the button, scroll to the top of the document
function topFunction() {
    document.body.scrollTop = 0;
    document.documentElement.scrollTop = 0;
}

//crops image, so it will always fit in container
jQuery(function($) {
    $('img').on('load',
        function() {
            var $img = $(this);
            var tempImage1 = new Image();
            tempImage1.src = $img.attr('src');
            tempImage1.onload = function() {
                var ratio = tempImage1.width / tempImage1.height;
                if (!isNaN(ratio) && ratio < 1) $img.addClass('vertical');
            }
        }).each(function() {
        if (this.complete) $(this).load();
    });
});