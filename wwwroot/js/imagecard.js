$('div.carouselPrev').on('click', function () {
	var currentLeft = Math.abs(parseInt($(carousel).css("left")));
	var newLeft = currentLeft - carouselSlideWidth;
	if (newLeft < 0 || isAnimating === true) { return; }
	$('#carousel ul').css({
		'left': "-" + newLeft + "px",
		"transition": "300ms ease-out"
	});
	isAnimating = true;
	setTimeout(function () { isAnimating = false; }, 300);
});
