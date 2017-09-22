///////////////// add COURSE text|vydeo //////////////////
$("#confirmAddLessonText").click(function () {
    var text = $("#addLessonText").val();
    if (text === '') {
        alert("Введіть текст");
    } else {
        $("#newCourse").append('<div class="col-md-10 col-md-offset-1" style="font-size: 1.3em;"><button type="button" class="btn btn-danger pull-right" id="right-panel-link" href="#right-panel"><i class="fa fa-times" aria-hidden="true"></i></button> <button type="button" class="btn btn-warning pull-right" id="right-panel-link" href="#right-panel"><i class="fa fa-pencil" aria-hidden="true"></i></button>' + text + '</div>');
    }
});

$("#confirmAddLessonVideo").click(function () {
    var videoSrc = $("#addFile").val();
    if (videoSrc === '') {
        alert("Виберіть файл");
    } else {
        var video = '<video controls style="max-width: 100%;height: auto;"><source src="' + videoSrc + '" type="video/mp4"><source src="' + videoSrc + '" type="video/ogg"> Your browser does not support HTML5 video.</video>';
        $("#newCourse").append('<div class="col-md-10 col-md-offset-1 text-center"> <button type="button" class="btn btn-danger pull-right" id="right-panel-link" href="#right-panel"><i class="fa fa-times" aria-hidden="true"></i></button><button type="button" class="btn btn-warning pull-right" id="right-panel-link" href="#right-panel"><i class="fa fa-pencil" aria-hidden="true"></i></button>' + video + '</div>');
        //		alert(videoSrc);
    }
});
/////////////////end add COURSE text|vydeo //////////////////

$("#clearAddLesson").click(function () {
    $("#nameLesson").val('');
    $("#descLesson").val('');
});

$(document).ready(function () {
    $('#addLessonForm').hide();
    $('#emptyInput').hide();
});

$('.SeeMore').click(function () {
    var $this = $(this);
    $this.toggleClass('SeeMore');
    if ($this.hasClass('SeeMore')) {
        $this.text('Додати урок');
        $('#addLessonForm').hide();
        $('#newCourse').hide();
    } else {
        $this.text('Приховати');
        $('#addLessonForm').fadeIn('slow');
        $('#newCourse').fadeIn('slow');
    }
});

$("#confirmAddLesson").click(function () {
    var name = $("#nameLesson").val();
    var desc = $("#descLesson").val();

    if (name === '' || desc === '') {
        $("#emptyInput").hide();
        $('#emptyInput').fadeIn('slow');
        return false;
    } else {
        $("#emptyInput").hide();
        var lesson = '<tr class="openLesson" data-href="#"><td class="numLessonTh">Урок№</td><td class="nameLessonTh">' + name + '</td><td class="shortDescTh">' + desc + '</td><td class="editButt text-center"><i class="fa fa-pencil" aria-hidden="true"></i></td><td class="delButt text-center"><i class="fa fa-times" aria-hidden="true"></i></td></tr>';
        $("#tableLesson").append(lesson);
    }
});

$(document).ready(function () {
    $('#addTextLessonForm').hide();
    $('#addVideoLessonForm').hide();
});

$("#addTextButt").click(function () {
    $('#addTextLessonForm').fadeIn('slow');
    $('#createLessonForm').hide();
    $('#addVideoLessonForm').hide();
});

$("#cancleAddText").click(function () {
    $('#createLessonForm').fadeIn('slow');
    $('#addTextLessonForm').hide();
    $('#addVideoLessonForm').hide();
});

$("#addVideoButt").click(function () {
    $('#addVideoLessonForm').fadeIn('slow');
    $('#createLessonForm').hide();
    $('#addTextLessonForm').hide();
});

$("#cancleAddVideo").click(function () {
    $('#createLessonForm').fadeIn('slow');
    $('#addTextLessonForm').hide();
    $('#addVideoLessonForm').hide();
});
//lesson display
$(function () {
    $(".lesson-card").on({
        mouseenter: function (e) {
            $(this).children(".lesson-card-left-wing,.lesson-card-right-wing").css('box-shadow', "8px 6px 12px 0 rgba(0, 0, 0, 0.3)");
        },
        mouseleave: function (e) {
            $(this).children(".lesson-card-left-wing,.lesson-card-right-wing").css('box-shadow', "6px 3px 8px 0 rgba(0, 0, 0, 0.15)");
        }
    });
    $(".lesson-card").not("[type='locked'],[type='editable']").on({
        click: function (e) {
            document.location = "/Lesson/" + $(this).data('id');
        }
    });
});

$(function () {
    $(".lesson-card[type='locked']").children(".lesson-card-content").after('<div class="lesson-card-content-locked"> <i class="fa fa-lock fa-4x"></i> </div>');
    $(".lesson-card[type='locked'] .lesson-card-content").css("opacity", "0.5");
    $(".lesson-card[type='locked']").hover(
        function (e) {
            $(this).find(".lesson-card-content-locked").css("opacity", "1");
        },
        function (e) {
            $(this).find(".lesson-card-content-locked").css("opacity", "0.6");
        });
    $(".lesson-card[type='locked']").click(function (e) {
        e.preventDefault();
    });
});

$(function () {
    //on hover adding a pencil\trash icons
    $(".lesson-card[type='editable']").hover(function (e) {
            $(".lesson-card[type='editable'] .lesson-card-content").css("opacity", "0.5");
            $(this).children(".lesson-card-left-wing").append('<i class="fa fa-pencil fa-4x lesson-card-edit"></i>');
            $(this).children(".lesson-card-right-wing").append('<i class="fa fa-trash fa-4x lesson-card-delete"></i>');
        },
        function (e) {
            $(this).children(".lesson-card-left-wing").empty();
            $(this).children(".lesson-card-right-wing").empty();
            $(".lesson-card[type='editable'] .lesson-card-content").css("opacity", "1");
        });
    //removing pointer events, so left and right wings become hoverable
    $(".lesson-card[type='editable'] .lesson-card-content").css("pointer-events", "none");
    //highlighting selected wing
    $(".lesson-card[type='editable'] .lesson-card-left-wing,.lesson-card[type='editable'] .lesson-card-right-wing")
        .hover(
            function (e) {
                $(this).children().css("opacity", "1");
            },
            function (e) {
                $(this).children().css("opacity", "0.6");
            });
    $(".lesson-card[type='editable'] .lesson-card-left-wing").click(function (e) {
        document.location = "/LessonEditor/" + $(this).data('id');
    });
    $(".lesson-card[type='editable'] .lesson-card-right-wing").click(function (e) {
        $.post(
            '/Lecturer/DeleteCourse?courseID=' + $(this).parent().data('id'));
    });
});