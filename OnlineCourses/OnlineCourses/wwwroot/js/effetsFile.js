	$(window).scroll(function() {
			if ($(document).scrollTop() > 10) {
			$('.navbar').addClass('shrink');
              
			}
			else {
			$('.navbar').removeClass('shrink');
           
			});

