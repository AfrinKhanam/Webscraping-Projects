jQuery(document).ready(function($){
	'use strict';
    $('body').find('.carousel-slider').each(function () {
        var _this = $(this);
        var autoWidth = _this.data('auto-width');
        var stagePadding = parseInt(_this.data('stage-padding'));
        stagePadding = stagePadding > 0 ? stagePadding : 0;
        if (jQuery().owlCarousel) {
            _this.owlCarousel({
                stagePadding: stagePadding,
                nav: _this.data('nav'),
                dots: _this.data('dots'),
                margin: _this.data('margin'),
                loop: _this.data('loop'),
                autoplay: _this.data('autoplay'),
                autoplayTimeout: _this.data('autoplay-timeout'),
                autoplaySpeed: _this.data('autoplay-speed'),
                autoplayHoverPause: _this.data('autoplay-hover-pause'),
                slideBy: _this.data('slide-by'),
                lazyLoad: _this.data('lazy-load'),
                autoWidth: autoWidth,
		items:4,
                navText: [
                    '<svg class="carousel-slider-nav-icon" viewBox="0 0 20 20"><path d="M14 5l-5 5 5 5-1 2-7-7 7-7z"></path></use></svg>',
                    '<svg class="carousel-slider-nav-icon" viewBox="0 0 20 20"><path d="M6 15l5-5-5-5 1-2 7 7-7 7z"></path></svg>'
                ],
                responsive: {
                    320: {items: _this.data('colums-mobile')},
                    600: {items: _this.data('colums-small-tablet')},
                    768: {items: _this.data('colums-tablet')},
                    993: {items: _this.data('colums-small-desktop')},
                    1200: {items: _this.data('colums-desktop')},
                    1921: {items: _this.data('colums')}
                }
            });
            if ('hero-banner-slider' === _this.data('slide-type')) {
                var animation = _this.data('animation');
                if (animation.length) {
                    _this.on('change.owl.carousel', function () {
                        var sliderContent = _this.find('.carousel-slider-hero__cell__content');
                        sliderContent.removeClass('animated' + ' ' + animation).hide();
                    });
                    _this.on('changed.owl.carousel', function (e) {
                        setTimeout(function () {
                            var current = $(e.target).find('.carousel-slider-hero__cell__content').eq(e.item.index);
                            current.show().addClass('animated' + ' ' + animation);
                        }, _this.data('autoplay-speed'));
                    });
                }
            }
        }
        if (jQuery().magnificPopup) {
            if (_this.data('slide-type') === 'product-carousel') {
                $(this).find('.magnific-popup').magnificPopup({
                    type: 'ajax'
                });
            } else if ('video-carousel' === _this.data('slide-type')) {
                $(this).find('.magnific-popup').magnificPopup({
                    type: 'iframe'
                });
            } else {
                $(this).find('.magnific-popup').magnificPopup({
                    type: 'image',
                    gallery: {
                        enabled: true
                    },
                    zoom: {
                        enabled: true,
                        duration: 300,
                        easing: 'ease-in-out'
                    }
                });
            }
        }
    });

    var ResponsiveMenu = {
        trigger: '#responsive-menu-button',
        animationSpeed: 500,
        breakpoint: 768,
        pushButton: 'off',
        animationType: 'slide',
        animationSide: 'left',
        pageWrapper: '',
        isOpen: false,
        triggerTypes: 'click',
        activeClass: 'is-active',
        container: '#responsive-menu-container',
        openClass: 'responsive-menu-open',
        accordion: 'off',
        activeArrow: '▲',
        inactiveArrow: '▼',
        wrapper: '#responsive-menu-wrapper',
        closeOnBodyClick: 'off',
        closeOnLinkClick: 'off',
        itemTriggerSubMenu: 'off',
        linkElement: '.responsive-menu-item-link',
        subMenuTransitionTime: 200,
        openMenu: function() {
            $(this.trigger).addClass(this.activeClass);
            $('html').addClass(this.openClass);
            $('.responsive-menu-button-icon-active').hide();
            $('.responsive-menu-button-icon-inactive').show();
            this.setButtonTextOpen();
            this.setWrapperTranslate();
            this.isOpen = true;
        },
        closeMenu: function() {
            $(this.trigger).removeClass(this.activeClass);
            $('html').removeClass(this.openClass);
            $('.responsive-menu-button-icon-inactive').hide();
            $('.responsive-menu-button-icon-active').show();
            this.setButtonText();
            this.clearWrapperTranslate();
            this.isOpen = false;
        },
        setButtonText: function() {
            if($('.responsive-menu-button-text-open').length > 0 && $('.responsive-menu-button-text').length > 0) {
                $('.responsive-menu-button-text-open').hide();
                $('.responsive-menu-button-text').show();
            }
        },
        setButtonTextOpen: function() {
            if($('.responsive-menu-button-text').length > 0 && $('.responsive-menu-button-text-open').length > 0) {
                $('.responsive-menu-button-text').hide();
                $('.responsive-menu-button-text-open').show();
            }
        },
        triggerMenu: function() {
            this.isOpen ? this.closeMenu() : this.openMenu();
        },
        triggerSubArrow: function(subarrow) {
            var sub_menu = $(subarrow).parent().siblings('.responsive-menu-submenu');
            var self = this;
            if(this.accordion == 'on') {
                /* Get Top Most Parent and the siblings */
                var top_siblings = sub_menu.parents('.responsive-menu-item-has-children').last().siblings('.responsive-menu-item-has-children');
                var first_siblings = sub_menu.parents('.responsive-menu-item-has-children').first().siblings('.responsive-menu-item-has-children');
                /* Close up just the top level parents to key the rest as it was */
                top_siblings.children('.responsive-menu-submenu').slideUp(self.subMenuTransitionTime, 'linear').removeClass('responsive-menu-submenu-open');
                /* Set each parent arrow to inactive */
                top_siblings.each(function() {
                    $(this).find('.responsive-menu-subarrow').first().html(self.inactiveArrow);
                    $(this).find('.responsive-menu-subarrow').first().removeClass('responsive-menu-subarrow-active');
                });
                /* Now Repeat for the current item siblings */
                first_siblings.children('.responsive-menu-submenu').slideUp(self.subMenuTransitionTime, 'linear').removeClass('responsive-menu-submenu-open');
                first_siblings.each(function() {
                    $(this).find('.responsive-menu-subarrow').first().html(self.inactiveArrow);
                    $(this).find('.responsive-menu-subarrow').first().removeClass('responsive-menu-subarrow-active');
                });
            }
            if(sub_menu.hasClass('responsive-menu-submenu-open')) {
                sub_menu.slideUp(self.subMenuTransitionTime, 'linear').removeClass('responsive-menu-submenu-open');
                $(subarrow).html(this.inactiveArrow);
                $(subarrow).removeClass('responsive-menu-subarrow-active');
            } else {
                sub_menu.slideDown(self.subMenuTransitionTime, 'linear').addClass('responsive-menu-submenu-open');
                $(subarrow).html(this.activeArrow);
                $(subarrow).addClass('responsive-menu-subarrow-active');
            }
        },
        menuHeight: function() {
            return $(this.container).height();
        },
        menuWidth: function() {
            return $(this.container).width();
        },
        wrapperHeight: function() {
            return $(this.wrapper).height();
        },
        setWrapperTranslate: function() {
            switch(this.animationSide) {
                case 'left':
                    var translate = 'translateX(' + this.menuWidth() + 'px)'; break;
                case 'right':
                    translate = 'translateX(-' + this.menuWidth() + 'px)'; break;
                case 'top':
                    translate = 'translateY(' + this.wrapperHeight() + 'px)'; break;
                case 'bottom':
                    translate = 'translateY(-' + this.menuHeight() + 'px)'; break;
            }
            if(this.animationType == 'push') {
                $(this.pageWrapper).css({'transform':translate});
                $('html, body').css('overflow-x', 'hidden');
            }
            if(this.pushButton == 'on') {
                $('#responsive-menu-button').css({'transform':translate});
            }
        },
        clearWrapperTranslate: function() {
            var self = this;
            if(this.animationType == 'push') {
                $(this.pageWrapper).css({'transform':''});
                setTimeout(function() {
                    $('html, body').css('overflow-x', '');
                }, self.animationSpeed);
            }
            if(this.pushButton == 'on') {
                $('#responsive-menu-button').css({'transform':''});
            }
        },
        init: function() {
            var self = this;
            $(this.trigger).on(this.triggerTypes, function(e){
                e.stopPropagation();
                self.triggerMenu();
            });
            $(this.trigger).mouseup(function(){
                $(self.trigger).blur();
            });
            $('.responsive-menu-subarrow').on('click', function(e) {
                e.preventDefault();
                e.stopPropagation();
                self.triggerSubArrow(this);
            });
            $(window).resize(function() {
                if($(window).width() > self.breakpoint) {
                    if(self.isOpen){
                        self.closeMenu();
                    }
                } else {
                    if($('.responsive-menu-open').length>0){
                        self.setWrapperTranslate();
                    }
                }
            });
            if(this.closeOnLinkClick == 'on') {
                $(this.linkElement).on('click', function(e) {
                    e.preventDefault();
                    /* Fix for when close menu on parent clicks is on */
                    if(self.itemTriggerSubMenu == 'on' && $(this).is('.responsive-menu-item-has-children > ' + self.linkElement)) {
                        return;
                    }
                    old_href = $(this).attr('href');
                    old_target = typeof $(this).attr('target') == 'undefined' ? '_self' : $(this).attr('target');
                    if(self.isOpen) {
                        if($(e.target).closest('.responsive-menu-subarrow').length) {
                            return;
                        }
                        self.closeMenu();
                        setTimeout(function() {
                            window.open(old_href, old_target);
                        }, self.animationSpeed);
                    }
                });
            }
            if(this.closeOnBodyClick == 'on') {
                $(document).on('click', 'body', function(e) {
                    if(self.isOpen) {
                        if($(e.target).closest('#responsive-menu-container').length || $(e.target).closest('#responsive-menu-button').length) {
                            return;
                        }
                    }
                    self.closeMenu();
                });
            }
            if(this.itemTriggerSubMenu == 'on') {
                $('.responsive-menu-item-has-children > ' + this.linkElement).on('click', function(e) {
                    e.preventDefault();
                    self.triggerSubArrow($(this).children('.responsive-menu-subarrow').first());
                });
            }
        }
    };
    ResponsiveMenu.init();

    /* Share Details - NSE - Old File
    var nseHref = "https://cors.io/?https://www.ndtv.com/business/marketdata/domestic-index-nse_nifty";
    $.ajax({
        url: nseHref,
        type: 'GET',
        dataType: 'html',
        success: function(html) {
            var nse = $(html).find(".company-status span.senx-up-down").html()
            var nsechange = $(html).find(".company-status table tr").eq(1).find("td").eq(0).text()
            var nsechangep = $(html).find(".company-status table tr").eq(1).find("td").eq(1).text()
            var chR = $(html).find('.status-change tr').eq(1).find("td span.red-btn").length;
            var chG = $(html).find('.status-change tr').eq(1).find("td span.green-btn").length;
            var s1 = "";
            var s2 = "";
            if(chR > 0) {
                // RED
                s1 = "<i class='fa fa-chevron-circle-down' style='color: #ca0000'> "
                s2 = "</i>"
            }
            if(chG > 0) {
                // GREEN
                s1 = "<i class='fa fa-chevron-circle-up shareGain' style='color: #078c07'> "
                s2 = "</i>"
            }
            $('#nse').html('NSE ' + nse + ' ' + s1 + nsechangep + s2);
        }
    }); */

    /* Share Details - BSE - Old File
    var bseHref = "https://cors.io/?https://www.ndtv.com/business/marketdata/domestic-index-bse_sensex";
    $.ajax({
        url: bseHref,
        type: 'GET',
        dataType: 'html',
        success: function(html) {
            var bse = $(html).find(".company-status span.senx-up-down").html()
            var bsechange = $(html).find(".company-status table tr").eq(1).find("td").eq(0).text()
            var bsechangep = $(html).find(".company-status table tr").eq(1).find("td").eq(1).text()                
            var chR = $(html).find('.status-change tr').eq(1).find("td span.red-btn").length;
            var chG = $(html).find('.status-change tr').eq(1).find("td span.green-btn").length;                
            var s1 = "";
            var s2 = "";
            if(chR > 0) {
                // Red
                s1 = "<i class='fa fa-chevron-circle-down' style='color: #ca0000'> "
                s2 = "</i>"
            }
            if(chG > 0) {
                // Green
                s1 = "<i class='fa fa-chevron-circle-up shareGain' style='color: #078c07'> "
                s2 = "</i>"
            }
            $('#bse').html('BSE ' + bse + ' ' + s1 + bsechangep + s2);
        }
    }); */

    /* Share Details - INDIAN BANK - Old File
    var ibHref = "https://cors.io/?https://www.ndtv.com/business/stock/indian-bank_indianb";
    $.ajax({
        url: ibHref,
        type: 'GET',
        dataType: 'html',
        success: function(html) {
            var nse = $(html).find("#nsesensex span.senx-up-down").html()
            var nsechange = $(html).find("#nsesensex table tr").eq(1).find("td").eq(0).text()
            var nsechangep = $(html).find("#nsesensex table tr").eq(1).find("td").eq(1).text()
            var bse = $(html).find("#bsesensex span.senx-up-down").html()
            var bsechange = $(html).find("#bsesensex table tr").eq(1).find("td").eq(0).text()
            var bsechangep = $(html).find("#bsesensex table tr").eq(1).find("td").eq(1).text()
            var chR = $(html).find('#nsesensex .status-change tr').eq(1).find("td span.red-btn").length;
            var chG = $(html).find('#nsesensex .status-change tr').eq(1).find("td span.green-btn").length;
            var s1 = "";
            var s2 = "";
            if(chR > 0) {
                // RED
                s1 = "<i class='fa fa-chevron-circle-down' style='color: #ca0000'> "
                s2 = "</i>"
            }
            if(chG > 0) {
                // GREEN
                s1 = "<i class='fa fa-chevron-circle-up' style='color: #078c07'> "
                s2 = "</i>"
            }
            var chR1 = $(html).find('#bsesensex .status-change tr').eq(1).find("td span.red-btn").length;
            var chG1 = $(html).find('#bsesensex .status-change tr').eq(1).find("td span.green-btn").length;
            var z1 = "";
            var z2 = "";
            if(chR1 > 0) {
                // RED
                z1 = "<i class='fa fa-chevron-circle-down' style='color: #ca0000'> "
                z2 = "</i>"
            }
            if(chG1 > 0) {
                // GREEN
                z1 = "<i class='fa fa-chevron-circle-up' style='color: #078c07'> "
                z2 = "</i>"
            }
            $('#ib').html('Indian Bank NSE ' + nse + ' ' + s1 + nsechangep + s2);
            $('#ibbse').html('Indian Bank BSE ' + bse + ' ' + z1 + bsechangep + z2);
        }
    });*/


	/* Share Details - BSE */
	var links = []
	var i	
	var nse_url = "https://www.ndtv.com/business/marketdata/domestic-index-bse_sensex"
	$.getJSON('https://api.allorigins.win/get?url=' + encodeURIComponent(nse_url), function (data) {
		//console.log(data.contents);
		var html = data.contents;
		//alert($(html).find(".footer_copy").html());
		var bse = $(html).find(".company-status span.senx-up-down").html()
		var bsechange = $(html).find(".company-status table tr").eq(1).find("td").eq(0).text()
		var bsechangep = $(html).find(".company-status table tr").eq(1).find("td").eq(1).text()
		var chR = $(html).find('.status-change tr').eq(1).find("td span.red-btn").length;
		var chG = $(html).find('.status-change tr').eq(1).find("td span.green-btn").length;
		var s1 = "";
		var s2 = "";
		if(chR > 0) {
			// Red
			s1 = "<i class='fa fa-chevron-circle-down' style='color: #ca0000'> "
	        s2 = "</i>"
		}
		if(chG > 0) {
			// Green
			s1 = "<i class='fa fa-chevron-circle-up' style='color: #078c07'> "
            s2 = "</i>"
		}
		$('#bse').html('BSE ' + bse + ' ' + s1 + bsechangep + s2);
	});
    /* Share Details - NSE */
	var links = []
	var i
	var nse_url = "https://www.ndtv.com/business/marketdata/domestic-index-nse_nifty"
	$.getJSON('https://api.allorigins.win/get?url=' + encodeURIComponent(nse_url), function (data) {
		// console.log(data.contents);
		var html = data.contents;
		//alert($(html).find(".footer_copy").html());
		var nse = $(html).find(".company-status span.senx-up-down").html()
		var nsechange = $(html).find(".company-status table tr").eq(1).find("td").eq(0).text()
		var nsechangep = $(html).find(".company-status table tr").eq(1).find("td").eq(1).text()
		var chR = $(html).find('.status-change tr').eq(1).find("td span.red-btn").length;
		var chG = $(html).find('.status-change tr').eq(1).find("td span.green-btn").length;
		var s1 = "";
		var s2 = "";
		if(chR > 0) {
			// RED
			s1 = "<i class='fa fa-chevron-circle-down' style='color: #ca0000'> "
	        s2 = "</i>"
		}
		if(chG > 0) {
			// GREEN
			s1 = "<i class='fa fa-chevron-circle-up' style='color: #078c07'> "
            s2 = "</i>"
		}
		$('#nse').html('NSE ' + nse + ' ' + s1 + nsechangep + s2);				
	});
	/* Share Details - INDIAN BANK */
	var links = []
	var i
	var nse_url = "https://www.ndtv.com/business/stock/indian-bank_indianb"
	$.getJSON('https://api.allorigins.win/get?url=' + encodeURIComponent(nse_url), function (data) {
		// console.log(data.contents);
		var html = data.contents;
		//alert($(html).find(".footer_copy").html());
		var nse = $(html).find("#nsesensex span.senx-up-down").html()
		var nsechange = $(html).find("#nsesensex table tr").eq(1).find("td").eq(0).text()
		var nsechangep = $(html).find("#nsesensex table tr").eq(1).find("td").eq(1).text()
		var bse = $(html).find("#bsesensex span.senx-up-down").html()
		var bsechange = $(html).find("#bsesensex table tr").eq(1).find("td").eq(0).text()
		var bsechangep = $(html).find("#bsesensex table tr").eq(1).find("td").eq(1).text()
		var chR = $(html).find('#nsesensex .status-change tr').eq(1).find("td span.red-btn").length;
		var chG = $(html).find('#nsesensex .status-change tr').eq(1).find("td span.green-btn").length;
		var s1 = "";
		var s2 = "";
		var space = "<br>";
		if(chR > 0) {
			// RED
		  	s1 = "<i class='fa fa-chevron-circle-down' style='color: #ca0000'> "
	        s2 = "</i>"
		}
		if(chG > 0) {
			// GREEN
		  	s1 = "<i class='fa fa-chevron-circle-up' style='color: #078c07'> "
            s2 = "</i>"
		}
		var chR1 = $(html).find('#bsesensex .status-change tr').eq(1).find("td span.red-btn").length;
		var chG1 = $(html).find('#bsesensex .status-change tr').eq(1).find("td span.green-btn").length;
		var z1 = "";
		var z2 = "";
		if(chR1 > 0) {
			// RED
			z1 = "<i class='fa fa-chevron-circle-down' style='color: #ca0000'> "
	        z2 = "</i>"
		}
		if(chG1 > 0) {
			// GREEN
			z1 = "<i class='fa fa-chevron-circle-up' style='color: #078c07'> "
            z2 = "</i>"
		}
		$('#ib').html('<span class="spEffBlinkDelay1">IB NSE '+ nse + '</span> ' + s1 + nsechangep + s2);
		$('#ibbse').html('<span class="spEffBlinkDelay2">IB BSE '+ bse + '</span> ' + z1 + bsechangep + z2);				
	});
    /*$('.flexslider').flexslider({
        animation: "slide",
        start: function(slider){
            $('body').removeClass('loading');
        }
    });*/

    $(".carousel-slider__post-title, .carousel-slider__post-image").click(function(){
        var status_id = $(this).attr('href').split('=');
        if(status_id == 'https://www.indianbank.in/psb-loans-in-59-minutes/'){
            $(this).attr("target", "blank");
            $(this).attr("href", "https://www.psbloansin59minutes.com/indianbank");
        }
    });

    /*$(".carousel-slider__post-header").click(function(){
        var status_id = $(this).attr('href').split('=');
        if(status_id == 'https://www.indianbank.in/psb-loans-in-59-minutes/'){
            $(this).attr("target", "blank");
            $(this).attr("href", "https://www.psbloansin59minutes.com/indianbank/");
        }
    });*/

    $(".firstLevelHeader #search, .secondLevelHeader #search").focus(function(){
        var placeholder = $(this).attr('placeholder');
        $("header form i").removeClass('fa-search');
    });

    $(".firstLevelHeader #search, .secondLevelHeader #search").blur(function(){
        if($(this).val() == ''){
            $("header form i").addClass('fa-search');            
        }
    });
    
    /* add on 24042018 */
    $("#product-crousel-warpper").mouseover(function(){
        $(this).parent().css('position', 'relative');
        $(this).parent().css('z-index', 101);
    });
    $(".scroll-point, .news-wraper").mouseover(function(){
        $("#product-crousel-warpper").parent().css('position', 'static');
        $("#product-crousel-warpper").parent().css('z-index', 99);
    });
    
    /* Today Changes on 19042018 */
    $('#menu-loan_category_menu li').addClass('wow fadeInDown');
    var listLen = $('#menu-loan_category_menu li').length;
    var delaySec = "0";   
    for(var i = 1; i<=listLen; i++){       
        if(i == 1){
            $('#menu-loan_category_menu li:nth-child('+i+')').attr('data-wow-delay', delaySec+'s');
        }else{
            var newDelaySec = parseFloat(delaySec);
            delaySec = parseFloat(newDelaySec + 0.2).toFixed(1);
            $('#menu-loan_category_menu li:nth-child('+i+')').attr('data-wow-delay', delaySec+'s');
        }
    }
    
    $('#menu-image_category_menu li').addClass('wow fadeInDown');
    var listLen = $('#menu-image_category_menu li').length;
    var delaySec = "0";   
    for(var i = 1; i<=listLen; i++){       
        if(i == 1){
            $('#menu-image_category_menu li:nth-child('+i+')').attr('data-wow-delay', delaySec+'s');
        }else{
            var newDelaySec = parseFloat(delaySec);
            delaySec = parseFloat(newDelaySec + 0.2).toFixed(1);
            $('#menu-image_category_menu li:nth-child('+i+')').attr('data-wow-delay', delaySec+'s');
        }
    }

    $('#menu-about_category_menu li').addClass('wow fadeInDown');
    var listLen = $('#menu-about_category_menu li').length;
    var delaySec = "0";   
    for(var i = 1; i<=listLen; i++){       
        if(i == 1){
            $('#menu-about_category_menu li:nth-child('+i+')').attr('data-wow-delay', delaySec+'s');
        }else{
            var newDelaySec = parseFloat(delaySec);
            delaySec = parseFloat(newDelaySec + 0.2).toFixed(1);
            $('#menu-about_category_menu li:nth-child('+i+')').attr('data-wow-delay', delaySec+'s');
        }
    }

    $('#menu-product_category_menu li').addClass('wow fadeInDown');
    var listLen = $('#menu-product_category_menu li').length;
    var delaySec = "0";   
    for(var i = 1; i<=listLen; i++){       
        if(i == 1){
            $('#menu-product_category_menu li:nth-child('+i+')').attr('data-wow-delay', delaySec+'s');
        }else{
            var newDelaySec = parseFloat(delaySec);
            delaySec = parseFloat(newDelaySec + 0.2).toFixed(1);
            $('#menu-product_category_menu li:nth-child('+i+')').attr('data-wow-delay', delaySec+'s');
        }
    }

    $('#menu-deposit_category_menu li').addClass('wow fadeInDown');
    var listLen = $('#menu-deposit_category_menu li').length;
    var delaySec = "0";   
    for(var i = 1; i<=listLen; i++){       
        if(i == 1){
            $('#menu-deposit_category_menu li:nth-child('+i+')').attr('data-wow-delay', delaySec+'s');
        }else{
            var newDelaySec = parseFloat(delaySec);
            delaySec = parseFloat(newDelaySec + 0.2).toFixed(1);
            $('#menu-deposit_category_menu li:nth-child('+i+')').attr('data-wow-delay', delaySec+'s');
        }
    }

    $('#menu-service_category_menu li').addClass('wow fadeInDown');
    var listLen = $('#menu-service_category_menu li').length;
    var delaySec = "0";   
    for(var i = 1; i<=listLen; i++){       
        if(i == 1){
            $('#menu-service_category_menu li:nth-child('+i+')').attr('data-wow-delay', delaySec+'s');
        }else{
            var newDelaySec = parseFloat(delaySec);
            delaySec = parseFloat(newDelaySec + 0.2).toFixed(1);
            $('#menu-service_category_menu li:nth-child('+i+')').attr('data-wow-delay', delaySec+'s');
        }
    }

    $('#menu-news_category_menu li').addClass('wow fadeInDown');
    var listLen = $('#menu-news_category_menu li').length;
    var delaySec = "0";   
    for(var i = 1; i<=listLen; i++){       
        if(i == 1){
            $('#menu-news_category_menu li:nth-child('+i+')').attr('data-wow-delay', delaySec+'s');
        }else{
            var newDelaySec = parseFloat(delaySec);
            delaySec = parseFloat(newDelaySec + 0.2).toFixed(1);
            $('#menu-news_category_menu li:nth-child('+i+')').attr('data-wow-delay', delaySec+'s');
        }
    }

    $('#menu-contact_category_menu li').addClass('wow fadeInDown');
    var listLen = $('#menu-contact_category_menu li').length;
    var delaySec = "0";   
    for(var i = 1; i<=listLen; i++){       
        if(i == 1){
            $('#menu-contact_category_menu li:nth-child('+i+')').attr('data-wow-delay', delaySec+'s');
        }else{
            var newDelaySec = parseFloat(delaySec);
            delaySec = parseFloat(newDelaySec + 0.2).toFixed(1);
            $('#menu-contact_category_menu li:nth-child('+i+')').attr('data-wow-delay', delaySec+'s');
        }
    }

    $('#menu-email_category_menu li').addClass('wow fadeInDown');
    var listLen = $('#menu-email_category_menu li').length;
    var delaySec = "0";   
    for(var i = 1; i<=listLen; i++){       
        if(i == 1){
            $('#menu-email_category_menu li:nth-child('+i+')').attr('data-wow-delay', delaySec+'s');
        }else{
            var newDelaySec = parseFloat(delaySec);
            delaySec = parseFloat(newDelaySec + 0.2).toFixed(1);
            $('#menu-email_category_menu li:nth-child('+i+')').attr('data-wow-delay', delaySec+'s');
        }
    }
        
    /*$('.contentDetail').addClass('wow fadeInRight');
    $('.contentDetail').attr('data-wow-delay', '0.3s');*/


    $(window).load(function(){
        $("#contentLoader").fadeOut();
	$(".specialForm16Effect").addClass("noRight");
    });
    
    /* add on 06042018 */
    $('#upiApp').click(function(){
        $('.mobile-app li').css('background', '#ea9b1b');
        $(this).css('background', '#4f7ebc');
        $('#upiAppStore').removeClass('noDisplay');
        $('#indAppStore').addClass('noDisplay');
        $('#smartAppStore').addClass('noDisplay');
        $('#customerAppStore').addClass('noDisplay');
    });
    $('#indPay').click(function(){
        $('.mobile-app li').css('background', '#ea9b1b');
        $(this).css('background', '#4f7ebc');
        $('#upiAppStore').addClass('noDisplay');
        $('#indAppStore').removeClass('noDisplay');
        $('#smartAppStore').addClass('noDisplay');
        $('#customerAppStore').addClass('noDisplay');
    });
    $('#smartRemote').click(function(){
        $('.mobile-app li').css('background', '#ea9b1b');
        $(this).css('background', '#4f7ebc');
        $('#upiAppStore').addClass('noDisplay');
        $('#indAppStore').addClass('noDisplay');
        $('#smartAppStore').removeClass('noDisplay');
        $('#customerAppStore').addClass('noDisplay');
    });
    $('#ibCustomer').click(function(){
        $('.mobile-app li').css('background', '#ea9b1b');
        $(this).css('background', '#4f7ebc');
        $('#upiAppStore').addClass('noDisplay');
        $('#indAppStore').addClass('noDisplay');
        $('#smartAppStore').addClass('noDisplay');
        $('#customerAppStore').removeClass('noDisplay');
    });
    
    /* add on 28042018 

    $(document).on('click', '#virtualClick', function(){
        $('body').css('overflow', 'hidden');
        $('.fullPlayWapper').css('display', 'block');
        $("#virtualVideoWarpper").html('<div class="closeVideo"><i class="fa fa-window-close"></i><span class="tooltiptext">Close</span></div><object data="https://www.p4panorama.com/panos/indianbank-360-virtual-reality-tour" />');
    });

    $(document).on('click', '.closeVideo', function(){
        $('body').css('overflow-y', 'scroll');
        $('#virtualVideoWarpper object').remove();
        $('.closeVideo').css('display', 'none');
        $('.fullPlayWapper').css('display', 'none');
    });*/

    /*$('.counter').counterUp({
        delay: 10,
        time: 500,
        offset: 70,
        beginAt: 100,
        formatter: function (n) {
            return n.replace(/,/g, '.');
        }
    });*/
    
    $('.mainContent li').addClass('wow flipInX');
    var innerListLen = $('.mainContent li').length;
    var delaySec = "0";   
    for(var i = 1; i<=innerListLen; i++){
        if(i == 1){
            $('.mainContent li:nth-child('+i+')').attr('data-wow-delay', delaySec+'s');
        }else{
            var newDelaySec = parseFloat(delaySec);
            delaySec = parseFloat(newDelaySec + 0.2).toFixed(1);
            $('.mainContent li:nth-child('+i+')').attr('data-wow-delay', delaySec+'s');
        }
    }
    
    $('.orange').click(function(){      
        $('body').toggleClass('orange-theme');
        $('body').removeClass('yellow-theme green-theme blue-theme');
        $('header.firstLevelHeader .header-bg').css('background', 'rgba(255,165,0,0.8)');
        $('header.secondLevelHeader .header-bg').css('background', 'rgba(255,165,0,1)');
        //$('footer .sub-footer').css({"background":"linear-gradient( rgba(255, 165, 0, 0.83), rgba(255, 165, 0, 0.83) ), url(wp-content/themes/cyfutureIndianBank/images/indian-bank-building-COM.jpg) no-repeat center center", "background-size":"cover"});
    });     
        
    $('.yellow').click(function(){      
        $('body').toggleClass('yellow-theme');
        $('body').removeClass('orange-theme green-theme blue-theme');
        $('header.firstLevelHeader .header-bg').css('background', 'rgba(254, 215, 76, 0.8)');
        $('header.secondLevelHeader .header-bg').css('background', 'rgba(254, 215, 76, 1)');
        //$('footer .sub-footer').css({"background":"linear-gradient( rgba(254, 215, 76, 0.83), rgba(254, 215, 76, 0.83) ), url(wp-content/themes/cyfutureIndianBank/images/indian-bank-building-COM.jpg) no-repeat center center", "background-size":"cover"});
    });

    $('.green').click(function(){       
        $('body').toggleClass('green-theme');
        $('body').removeClass('orange-theme yellow-theme blue-theme');
        $('header.firstLevelHeader .header-bg').css('background', 'rgba(78, 185, 167, 0.8)');
        $('header.secondLevelHeader .header-bg').css('background', 'rgba(78, 185, 167, 1)');
        //$('footer .sub-footer').css({"background":"linear-gradient( rgba(78, 185, 167, 0.83), rgba(78, 185, 167, 0.83) ), url(wp-content/themes/cyfutureIndianBank/images/indian-bank-building-COM.jpg) no-repeat center center", "background-size":"cover"});
    });

    $('.blue').click(function(){        
        $('body').toggleClass('blue-theme');
/*      if($('body').hasClass('orange-theme') || $('body').hasClass('orange-theme') || $('body').hasClass('orange-theme') ){
            
            }*/
            $('body').removeClass('orange-theme yellow-theme green-theme');
        $('header.firstLevelHeader .header-bg').css('background', 'rgba(12, 77, 162, 0.8)');
        $('header.secondLevelHeader .header-bg').css('background', 'rgba(12, 77, 162, 1)');
        //$('footer .sub-footer').css({"background":"linear-gradient( rgba(12, 77, 162, 0.83), rgba(12, 77, 162, 0.83) ), url(wp-content/themes/cyfutureIndianBank/images/indian-bank-building-COM.jpg) no-repeat center center", "background-size":"cover"});
    });
    
    if(!$('body').hasClass('home')){
        $('header.firstLevelHeader').css('display', 'none');
        $('header.secondLevelHeader').css('display', 'block');
    }
    
    /*setTimeout(function(){
        $(".footer-slide-bg").addClass( "bounce" );
        $(".sub-footer").addClass( "space" );
    }, 10000);*/

    //$(".footer-slide-bg").addClass( "bounce" );
    //$(".sub-footer").addClass( "space" );

    /*$('.counter').counterUp({time:2000, delay:10});*/

    /*$('.counter').countUp();*/

    $('.footerList li').addClass('col-md-3 wow flipInY');
    $('.footerList li').attr('data-wow-delay', '0.4s');

    $(window).scroll(function(){
        if($('body').hasClass('home')){
            var winHgt = $(this).scrollTop();
            if(winHgt > 400){
		$('header').removeClass('firstLevelHeader');
                $('header').addClass('secondLevelHeader');
                /*$('header.firstLevelHeader').css('display', 'none');
                $('header.secondLevelHeader').css('display', 'block');
                $('.arrowPointer').animate({width: 'show'}, "fast");*/
            }else if(winHgt < 400){
		$('header').removeClass('secondLevelHeader');
                $('header').addClass('firstLevelHeader');
                /*$('header.firstLevelHeader').css('display', 'block');
                $('header.secondLevelHeader').css('display', 'none');
                $('.arrowPointer').animate({width: 'hide'}, "fast");*/
            }
        }        
    });

    $(window).scroll(function(){
        var winHgt = $(this).scrollTop();
        if(winHgt > 400){
            $('.arrowIndicator').animate({width: 'show'}, "slow");
        }else if(winHgt < 400){
            $('.arrowIndicator').animate({width: 'hide'}, "slow");
        }
    })

    $(document).on('click', '.arrowIndicator', function (event) {
        event.preventDefault();
        $('html, body').animate({
            scrollTop: $('body').offset().top
        }, "3000");
        return false;   
    });

    /*$('.right-menu').mouseover(function() {
        $('.right-login').animate({width: 'toggle'}, "fast");
        $(".right-menu").html($(".right-menu").html() == 'Hide' ? 'Show' : 'Hide');
        $(this).toggleClass('arrow');
    });*/
    
    $(".tab_btn_in").click(function(){
        var get_tab_id_in =$(this).attr('data-tab');
        $('#'+get_tab_id_in).animate({height: 'toggle'}, "fast");
        $('.tab_btn_in').removeClass('tab_active_in');
        $(this).addClass('tab_active_in');
        $('.tab_cont_in').hide();
        $('#'+get_tab_id_in).show();
    });

    var count = 0;
    $("#ddmenu").removeClass("menuFontClass");
    $(".increase").click(function(){
        $(".decrease").addClass('notActive');
        count += 1;
        if ( count <= 3 ){
            var curFontSize = $('body').css('font-size');
            $('body').css('font-size', parseInt(curFontSize)+1);
            var tollNumberFontSize = $('header .toll-free strong').css('font-size');
            $('header .toll-free strong').css('font-size', parseInt(tollNumberFontSize)+1);
            var leftSlideMenuFontSize = $('.scroll-point-virtual li a').css('font-size');
            $('.scroll-point-virtual li a').css('font-size', leftSlideMenuFontSize);
            var rightSlideMenuFontSize1 = $('.scroll-point li a').css('font-size');
            $('.scroll-point li a').css('font-size', rightSlideMenuFontSize1);
            var rightSlideMenuFontSize2 = $('.scroll-point-rates li a').css('font-size');
            $('.scroll-point-rates li a').css('font-size', rightSlideMenuFontSize2);
            var contentSliderFontSize = $('.carousel-slider__post-title h1').css('font-size');
            $('.carousel-slider__post-title h1').css('font-size', parseInt(contentSliderFontSize)+1);
            var contentFontSize = $('.carousel-slider__post-excerpt').css('font-size');
            $('.carousel-slider__post-excerpt').css('font-size', parseInt(contentFontSize)+1);
            var bannerHeadingFontSize = $('.carousel-slider-hero__cell__heading').css('font-size');
            $('.carousel-slider-hero__cell__heading').css('font-size', parseInt(bannerHeadingFontSize)+1);
            var bannerContentFontSize = $('.carousel-slider-hero__cell__description').css('font-size');
            $('.carousel-slider-hero__cell__description').css('font-size', parseInt(bannerContentFontSize)+1);            
            var sectionHeadingFontSize = $('.section-heading').css('font-size');
            $('.section-heading').css('font-size', parseInt(sectionHeadingFontSize)+1);
            var mobileAppHeadingFontSize = $('.mobile-app h3').css('font-size');
            $('.mobile-app h3').css('font-size', parseInt(mobileAppHeadingFontSize)+1);
            var mobileAppHeadingStrongFontSize = $('.mobile-app h3 span').css('font-size');
            $('.mobile-app h3 span').css('font-size', parseInt(mobileAppHeadingStrongFontSize)+1);
            var mobileAppHeadingStrongFontSize = $('.mobile-app h3 span').css('font-size');
            $('.mobile-app h3 span').css('font-size', parseInt(mobileAppHeadingStrongFontSize)+1);
            var mobileAppListFontSize = $('.mobile-app li').css('font-size');
            $('.mobile-app li').css('font-size', parseInt(mobileAppListFontSize)+1);
            var counterNumberFontSize = $('.counter-text span').css('font-size');
            $('.counter-text span').css('font-size', parseInt(counterNumberFontSize)+1);
            var counterNameFontSize = $('.counter-text small').css('font-size');
            $('.counter-text small').css('font-size', parseInt(counterNameFontSize)+1);
            var branchModuleFontSize = $('footer .sub-footer h3').css('font-size');
            $('footer .sub-footer h3').css('font-size', parseInt(branchModuleFontSize)+1);
            var footerListFontSize = $('.footerList').css('font-size');
            $('.footerList').css('font-size', parseInt(footerListFontSize)+1);
	    var innerPageTemplateContent = $('.contentDetail').css('font-size');
            $('.contentDetail').css('font-size', parseInt(innerPageTemplateContent)+1);
            var innerPageTemplateTitle = $('h1.contentTitle span').css('font-size');
            $('h1.contentTitle span').css('font-size', parseInt(innerPageTemplateTitle)+1);
            var innerPageTemplateHeading1 = $('.contentDetail h1').css('font-size');
            $('.contentDetail h1').css('font-size', parseInt(innerPageTemplateHeading1)+1);
            var innerPageTemplateHeading2 = $('.contentDetail h2').css('font-size');
            $('.contentDetail h2').css('font-size', parseInt(innerPageTemplateHeading2)+1);
            var innerPageTemplateHeading3 = $('.contentDetail h3').css('font-size');
            $('.contentDetail h3').css('font-size', parseInt(innerPageTemplateHeading3)+1);
            var innerPageTemplateHeading4 = $('.contentDetail h4').css('font-size');
            $('.contentDetail h4').css('font-size', parseInt(innerPageTemplateHeading4)+1);
            var innerPageTemplateHeading5 = $('.contentDetail h5').css('font-size');
            $('.contentDetail h5').css('font-size', parseInt(innerPageTemplateHeading5)+1);
            var innerPageTemplateHeading6 = $('.contentDetail h6').css('font-size');
            $('.contentDetail h6').css('font-size', parseInt(innerPageTemplateHeading6)+1);
        }
    });

    $(".decrease").click(function(){
        $(".increase").addClass('notActive');
        count += 1;
        if ( count <= 3 ){
            var curFontSize = $('body').css('font-size');
            $('body').css('font-size', parseInt(curFontSize)-1);
            var tollNumberFontSize = $('header .toll-free strong').css('font-size');
            $('header .toll-free strong').css('font-size', parseInt(tollNumberFontSize)-1);
            var leftSlideMenuFontSize = $('.scroll-point-virtual li a').css('font-size');
            $('.scroll-point-virtual li a').css('font-size', leftSlideMenuFontSize);
            var rightSlideMenuFontSize1 = $('.scroll-point li a').css('font-size');
            $('.scroll-point li a').css('font-size', rightSlideMenuFontSize1);
            var rightSlideMenuFontSize2 = $('.scroll-point-rates li a').css('font-size');
            $('.scroll-point-rates li a').css('font-size', rightSlideMenuFontSize2);
            var contentSliderFontSize = $('.carousel-slider__post-title h1').css('font-size');
            $('.carousel-slider__post-title h1').css('font-size', parseInt(contentSliderFontSize)-1);
            var contentFontSize = $('.carousel-slider__post-excerpt').css('font-size');
            $('.carousel-slider__post-excerpt').css('font-size', parseInt(contentFontSize)-1);
            var bannerHeadingFontSize = $('.carousel-slider-hero__cell__heading').css('font-size');
            $('.carousel-slider-hero__cell__heading').css('font-size', parseInt(bannerHeadingFontSize)-1);
            var bannerContentFontSize = $('.carousel-slider-hero__cell__description').css('font-size');
            $('.carousel-slider-hero__cell__description').css('font-size', parseInt(bannerContentFontSize)-1);
            var sectionHeadingFontSize = $('.section-heading').css('font-size');
            $('.section-heading').css('font-size', parseInt(sectionHeadingFontSize)-1);
            var mobileAppHeadingFontSize = $('.mobile-app h3').css('font-size');
            $('.mobile-app h3').css('font-size', parseInt(mobileAppHeadingFontSize)-1);
            var mobileAppHeadingStrongFontSize = $('.mobile-app h3 span').css('font-size');
            $('.mobile-app h3 span').css('font-size', parseInt(mobileAppHeadingStrongFontSize)-1);
            var mobileAppHeadingStrongFontSize = $('.mobile-app h3 span').css('font-size');
            $('.mobile-app h3 span').css('font-size', parseInt(mobileAppHeadingStrongFontSize)-1);
            var mobileAppListFontSize = $('.mobile-app li').css('font-size');
            $('.mobile-app li').css('font-size', parseInt(mobileAppListFontSize)-1);
            var counterNumberFontSize = $('.counter-text span').css('font-size');
            $('.counter-text span').css('font-size', parseInt(counterNumberFontSize)-1);
            var counterNameFontSize = $('.counter-text small').css('font-size');
            $('.counter-text small').css('font-size', parseInt(counterNameFontSize)-1);
            var branchModuleFontSize = $('footer .sub-footer h3').css('font-size');
            $('footer .sub-footer h3').css('font-size', parseInt(branchModuleFontSize)-1);
            var footerListFontSize = $('.footerList').css('font-size');
            $('.footerList').css('font-size', parseInt(footerListFontSize)-1);
	    var innerPageTemplateContent = $('.contentDetail').css('font-size');
            $('.contentDetail').css('font-size', parseInt(innerPageTemplateContent)-1);
            var innerPageTemplateTitle = $('h1.contentTitle span').css('font-size');
            $('h1.contentTitle span').css('font-size', parseInt(innerPageTemplateTitle)-1);
            var innerPageTemplateHeading1 = $('.contentDetail h1').css('font-size');
            $('.contentDetail h1').css('font-size', parseInt(innerPageTemplateHeading1)-1);
            var innerPageTemplateHeading2 = $('.contentDetail h2').css('font-size');
            $('.contentDetail h2').css('font-size', parseInt(innerPageTemplateHeading2)-1);
            var innerPageTemplateHeading3 = $('.contentDetail h3').css('font-size');
            $('.contentDetail h3').css('font-size', parseInt(innerPageTemplateHeading3)-1);
            var innerPageTemplateHeading4 = $('.contentDetail h4').css('font-size');
            $('.contentDetail h4').css('font-size', parseInt(innerPageTemplateHeading4)-1);
            var innerPageTemplateHeading5 = $('.contentDetail h5').css('font-size');
            $('.contentDetail h5').css('font-size', parseInt(innerPageTemplateHeading5)-1);
            var innerPageTemplateHeading6 = $('.contentDetail h6').css('font-size');
            $('.contentDetail h6').css('font-size', parseInt(innerPageTemplateHeading6)-1);
        }
    });
    
    $('body').delegate('.reset', 'click', function (e) {
        $("#ddmenu").removeClass("menuFontClass");
        $('*').each(function () {
            $(this).css("font-size", "");
        });
        $(".increase").removeClass('notActive');
        $(".decrease").removeClass('notActive');
        count = 0;
        y = 2;
    });
    
    $("#ddmenu").click(function(){
        $("#ddmenu").addClass("menuClass");
    });

    /*var w = $(window).width();
    if (w >= 0 && w <= 992){
        $(".mega-menu-heading").click(function () {
            var get_tab_nav_id = $(this).attr('data-tab');
            $('#' + get_tab_nav_id).animate({ height: 'toggle' }, "slow"); ;
        });
    }*/

    $( ".branchAtmValue" ).attr('autocomplete', 'off');
    $(".branchAtmValue").keypress(function (e) {
        var regex = new RegExp("^[a-zA-Z0-9]+$");
        var str = String.fromCharCode(!e.charCode ? e.which : e.charCode);
        if (regex.test(str)) {
            return true;
        }
        e.preventDefault();
        return false;
    });

    $('.branchAtmValue').on("cut copy paste",function(e) {
        e.preventDefault();
    });

    $(".search-form input").addClass('searchInputText');
    $(".search-form input" ).attr('autocomplete', 'off');

    /*$(".searchInputText").keypress(function (e) {
        var regex = new RegExp("^[a-zA-Z0-9]+$");
        var str = String.fromCharCode(!e.charCode ? e.which : e.charCode);
        if (regex.test(str)) {
            return true;
        }
        e.preventDefault();
        return false;
    });*/

    $(".searchInputText").keypress(function(e){
        var keyCode = e.which;
        // Not allow special 
        if ( !( (keyCode >= 48 && keyCode <= 57) ||(keyCode >= 65 && keyCode <= 90) || (keyCode >= 97 && keyCode <= 122) ) && keyCode != 8 && keyCode != 32 && keyCode != 13 ) {
            e.preventDefault();
        }
    });

    /*$(".feedbackMobileNumber").keypress(function(e){
        var keyCode = e.which;
        // Not allow special 
        if ( !( (keyCode >= 48 && keyCode <= 57) ||(keyCode >= 65 && keyCode <= 90) || (keyCode >= 97 && keyCode <= 122) ) && keyCode != 8 && keyCode != 32 ) {
            e.preventDefault();
        }
    });*/

    $("#feedbackName").keypress(function(e){
        var keyCode = e.which;
        // Not allow special 
        if ( !( (keyCode >= 48 && keyCode <= 57) ||(keyCode >= 65 && keyCode <= 90) || (keyCode >= 97 && keyCode <= 122) ) && keyCode != 8 && keyCode != 32 && keyCode != 13 ) {
            e.preventDefault();
        }
    });

    $("#feedbackBranchName").keypress(function(e){
        var keyCode = e.which;
        // Not allow special 
        if ( !( (keyCode >= 48 && keyCode <= 57) ||(keyCode >= 65 && keyCode <= 90) || (keyCode >= 97 && keyCode <= 122) ) && keyCode != 8 && keyCode != 32 && keyCode != 13 ) {
            e.preventDefault();
        }
    });

    $("#feedbackMobileNumber").keydown(function (e) {
        // Allow: backspace, delete, tab, escape, enter, Ctrl+A, Command+A, home, end, left, right, down, up
        if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110]) !== -1 || (e.keyCode === 65 && (e.ctrlKey === true || e.metaKey === true)) || (e.keyCode >= 35 && e.keyCode <= 40)) {
            return;
        }
        // Ensure that it is a number and stop the keypress
        if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
            e.preventDefault();
        }
    });

    var validateEmail = function(elementValue) {
        var emailPattern = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;
        return emailPattern.test(elementValue);
    }

    $('#feedbackEmailID').keyup(function() {
        var value = $(this).val();
        var valid = validateEmail(value);
        if (!valid) {
            $(this).css('color', 'red');
        } else {
            $(this).css('color', '#000');
        }
    });



    /*$(document).on("keypress", ".searchInputText", function(e){
        var regex = new RegExp("^[a-zA-Z0-9]+$");
        var str = String.fromCharCode(!e.charCode ? e.which : e.charCode);
        if (regex.test(str)) {
            return true;
        }
        e.preventDefault();
        return false;
    });*/

    $('.searchInputText').on("cut copy paste", function(e) {
        e.preventDefault();
    });
    $('#feedbackName').on("cut copy paste", function(e) {
        e.preventDefault();
    });
    $('#feedbackBranchName').on("cut copy paste", function(e) {
        e.preventDefault();
    });
    $('#feedbackMobileNumber').on("cut copy paste", function(e) {
        e.preventDefault();
    });
    $('#feedbackEmailID').on("cut copy paste", function(e) {
        e.preventDefault();
    });
    $('#feedbackMessage').on("cut copy paste", function(e) {
        e.preventDefault();
    });
    $('#feedbackAccNumber').on("cut copy paste", function(e) {
        e.preventDefault();
    });
    $('.wpcf7-captchar').on("cut copy paste", function(e) {
        e.preventDefault();
    });


    $(".contentDetail li").prepend('<span class="listBullet"><i class="fa fa-chevron-circle-right faa-horizontal animated"></i></span>');
    $(".contentDetail table tbody tr td").removeAttr("bgColor");
    
    $(".contentDetail table tbody tr:odd").addClass("specificBackground");
    /*$(".contentDetail").load(function(){
        if($(".contentDetail").has(""))
    });*/

    $(".click-arrow").click(function() {                
        $(".footer-slide-bg").toggleClass( "bounce" );      
        $(".sub-footer").toggleClass( "space" );
    });
            
    $(".news-click").click(function() {
        $(".news-wraper").toggleClass("slide-left");
    });

    $(".service-click").click(function() {
        $(".service-wraper").toggleClass("slide-left");
    });
    
    /*wp_register_script( 'my-script', 'myscript_url' );
wp_enqueue_script( 'my-script' );
//$translation_array = array( 'templateUrl' => get_stylesheet_directory_uri() );
//after wp_enqueue_script
wp_localize_script( 'my-script', 'object_name', $translation_array );*/

    var src = window.location.origin+"/wp-content/themes/cyfutureIndianBank/images/";

    if($('body').hasClass('category-featured-products-services-schemes')){
        var imgUrl12 = src+"product-ca-spriteNew.png";
        $(".productbox .whtbg .innerIcon").css('background-image','url('+ imgUrl12 +')');
        $(".productbox .whtbg .innerIcon").css('background-repeat','no-repeat');
        $(".productbox .whtbg .innerIcon").css('background-position','center -664px');
    }else if($('body').hasClass('category-groups')){        
        var imgUrl1 = src+"group-loan-icon.png";
        $(".productbox .whtbg .innerIcon").css('background-image','url('+imgUrl1+')');
        $(".productbox .whtbg .innerIcon").css('background-repeat','no-repeat');
    }else if($('body').hasClass('category-personal-individual')){
        var imgUrl2 = src+"product-ca-spriteNew.png";
        $(".productbox .whtbg .innerIcon").css('background-image','url('+ imgUrl2 +')');
        $(".productbox .whtbg .innerIcon").css('background-repeat','no-repeat');
        $(".productbox .whtbg .innerIcon").css('background-position','center -132px');
    }else if($('body').hasClass('category-sme')){
        var imgUrl3 = src+"SME-loan-icon.png";
        $(".productbox .whtbg .innerIcon").css('background-image','url('+ imgUrl3 +')');
        $(".productbox .whtbg .innerIcon").css('background-repeat','no-repeat');
    }else if($('body').hasClass('category-education')){
        var imgUrl4 = src+"education-loan-icon.png";
        $(".productbox .whtbg .innerIcon").css('background-image','url('+ imgUrl4 +')');
        $(".productbox .whtbg .innerIcon").css('background-repeat','no-repeat');
    }else if($('body').hasClass('category-nri')){
        var imgUrl5 = src+"NRI-loan.png";
        $(".productbox .whtbg .innerIcon").css('background-image','url('+ imgUrl5 +')');
        $(".productbox .whtbg .innerIcon").css('background-repeat','no-repeat');
    }else if($('body').hasClass('category-savings-bank-a-c')){
        var imgUrl6 = src+"product-ca-spriteNew.png";
        $(".productbox .whtbg .innerIcon").css('background-image','url('+ imgUrl6 +')');
        $(".productbox .whtbg .innerIcon").css('background-repeat','no-repeat');
        $(".productbox .whtbg .innerIcon").css('background-position','center -533px');
    }else if($('body').hasClass('category-current-a-c')){
        var imgUrl7 = src+"product-ca-spriteNew.png";
        $(".productbox .whtbg .innerIcon").css('background-image','url('+ imgUrl7 +')');
        $(".productbox .whtbg .innerIcon").css('background-repeat','no-repeat');
        $(".productbox .whtbg .innerIcon").css('background-position','center -265px');
    }else if($('body').hasClass('category-term-deposits')){
        var imgUrl8 = src+"product-ca-spriteNew.png";
        $(".productbox .whtbg .innerIcon").css('background-image','url('+ imgUrl8 +')');
        $(".productbox .whtbg .innerIcon").css('background-repeat','no-repeat');
        $(".productbox .whtbg .innerIcon").css('background-position','center -399px');
    }else if($('body').hasClass('category-nri-a-cs')){
        var imgUrl9 = src+"product-ca-spriteNew.png";
        $(".productbox .whtbg .innerIcon").css('background-image','url('+ imgUrl9 +')');
        $(".productbox .whtbg .innerIcon").css('background-repeat','no-repeat');
        $(".productbox .whtbg .innerIcon").css('background-position','center -928px');
    }else if($('body').hasClass('category-premium-services')){
        var imgUrl10 = src+"product-ca-spriteNew.png";
        $(".productbox .whtbg .innerIcon").css('background-image','url('+ imgUrl10 +')');
        $(".productbox .whtbg .innerIcon").css('background-repeat','no-repeat');
        $(".productbox .whtbg .innerIcon").css('background-position','center -1061px');
    }else if($('body').hasClass('category-insurance-services')){
        var imgUrl11 = src+"product-ca-spriteNew.png";
        $(".productbox .whtbg .innerIcon").css('background-image','url('+ imgUrl11 +')');
        $(".productbox .whtbg .innerIcon").css('background-repeat','no-repeat');
        $(".productbox .whtbg .innerIcon").css('background-position','center -797px');
    }else if($('body').hasClass('category-agriculture')){
        var imgUrl12 = src+"product-ca-sprite.png";
        $(".productbox .whtbg .innerIcon").css('background-image','url('+ imgUrl12 +')');
        $(".productbox .whtbg .innerIcon").css('background-repeat','no-repeat');
    }


    $('#navTenderTab').click(function(){
        $('#navAuctionTab').removeClass('navTenderActive');
        $('#navOldTenderTab').removeClass('navTenderActive');
        $('#navTenderTab').addClass('navTenderActive');
    });
    $('#navAuctionTab').click(function(){
        $('#navOldTenderTab').removeClass('navTenderActive');
        $('#navTenderTab').removeClass('navTenderActive');
        $('#navAuctionTab').addClass('navTenderActive');
    });
    $('#navOldTenderTab').click(function(){
        $('#navAuctionTab').removeClass('navTenderActive');
        $('#navTenderTab').removeClass('navTenderActive');
        $('#navOldTenderTab').addClass('navTenderActive');
    });

    if($('body').hasClass('postid-3734')){
        $('.contentDetail img').addClass('gmImgSize');
    }
    if($('body').hasClass('postid-3730')){
        $('.contentDetail img').addClass('gmImgSize');
    }
    
    $('#landingPagePopUpStartBtn').trigger('click');

    $("#wp-megamenu-item-2518 a").on("click",function(e) {
        e.preventDefault();
        var link = $(this).attr('href');
        $('#myForm').attr('action', link);
        $("#myForm").submit();        
    });

    $("#responsive-menu-item-2518 a").on("click",function(e) {
        e.preventDefault();
        var link = $(this).attr('href');
        $('#myForm').attr('action', link);
        $("#myForm").submit();        
    });

    $("#wp-megamenu-item-2519 a").on("click",function(e) {
        e.preventDefault();
        var link = $(this).attr('href');
        $('#myForm').attr('action', link);
        $("#myForm").submit();        
    });

    $("#responsive-menu-item-2519 a").on("click",function(e) {
        e.preventDefault();
        var link = $(this).attr('href');
        $('#myForm').attr('action', link);
        $("#myForm").submit();        
    });

    $("#wp-megamenu-item-2520 a").on("click",function(e) {
        e.preventDefault();
        var link = $(this).attr('href');
        $('#myForm').attr('action', link);
        $("#myForm").submit();        
    });

    $("#responsive-menu-item-2520 a").on("click",function(e) {
        e.preventDefault();
        var link = $(this).attr('href');
        $('#myForm').attr('action', link);
        $("#myForm").submit();        
    });

    $("#wp-megamenu-item-2521 a").on("click",function(e) {
        e.preventDefault();
        var link = $(this).attr('href');
        $('#myForm').attr('action', link);
        $("#myForm").submit();        
    });

    $("#responsive-menu-item-2521 a").on("click",function(e) {
        e.preventDefault();
        var link = $(this).attr('href');
        $('#myForm').attr('action', link);
        $("#myForm").submit();        
    });

    $("#wp-megamenu-item-2522 a").on("click",function(e) {
        e.preventDefault();
        var link = $(this).attr('href');
        $('#myForm').attr('action', link);
        $("#myForm").submit();        
    });

    $("#responsive-menu-item-2522 a").on("click",function(e) {
        e.preventDefault();
        var link = $(this).attr('href');
        $('#myForm').attr('action', link);
        $("#myForm").submit();        
    });

    $("#wp-megamenu-item-2523 a").on("click",function(e) {
        e.preventDefault();
        var link = $(this).attr('href');
        $('#myForm').attr('action', link);
        $("#myForm").submit();        
    });

    $("#responsive-menu-item-2523 a").on("click",function(e) {
        e.preventDefault();
        var link = $(this).attr('href');
        $('#myForm').attr('action', link);
        $("#myForm").submit();        
    });

    $("#wp-megamenu-item-41712 a").on("click",function(e) {
        e.preventDefault();
        var link = $(this).attr('href');
        $('#myForm').attr('action', link);
        $("#myForm").submit();        
    });

    $("#responsive-menu-item-41712 a").on("click",function(e) {
        e.preventDefault();
        var link = $(this).attr('href');
        $('#myForm').attr('action', link);
        $("#myForm").submit();        
    });

    $("#wp-megamenu-item-41713 a").on("click",function(e) {
        e.preventDefault();
        var link = $(this).attr('href');
        $('#myForm').attr('action', link);
        $("#myForm").submit();        
    });

    $("#responsive-menu-item-41713 a").on("click",function(e) {
        e.preventDefault();
        var link = $(this).attr('href');
        $('#myForm').attr('action', link);
        $("#myForm").submit();        
    });

    $("#wp-megamenu-item-41714 a").on("click",function(e) {
        e.preventDefault();
        var link = $(this).attr('href');
        $('#myForm').attr('action', link);
        $("#myForm").submit();        
    });

    $("#responsive-menu-item-41714 a").on("click",function(e) {
        e.preventDefault();
        var link = $(this).attr('href');
        $('#myForm').attr('action', link);
        $("#myForm").submit();        
    });

    $("#wp-megamenu-item-41715 a").on("click",function(e) {
        e.preventDefault();
        var link = $(this).attr('href');
        $('#myForm').attr('action', link);
        $("#myForm").submit();        
    });

    $("#responsive-menu-item-41715 a").on("click",function(e) {
        e.preventDefault();
        var link = $(this).attr('href');
        $('#myForm').attr('action', link);
        $("#myForm").submit();        
    });

    $("#wp-megamenu-item-41716 a").on("click",function(e) {
        e.preventDefault();
        var link = $(this).attr('href');
        $('#myForm').attr('action', link);
        $("#myForm").submit();        
    });

    $("#responsive-menu-item-41716 a").on("click",function(e) {
        e.preventDefault();
        var link = $(this).attr('href');
        $('#myForm').attr('action', link);
        $("#myForm").submit();        
    });

    /*$(".homeAdvBanner .owl-item").click(function(e){
        var answer = confirm ("Please click on OK to continue.")
        if (answer){
        }else{
            e.preventDefault();
        }
    });*/

    if ($(window).width() <= 767) {
        $('.randomSideMenu').remove().insertAfter($('.randomContent'));
    } else {
        $('.randomSideMenu').remove().insertBefore($('.randomContent'));
    }

    $(window).resize(function() {
        if ($(window).width() <= 767) {
            $('.randomSideMenu').remove().insertAfter($('.randomContent'));
        } else {
            $('.randomSideMenu').remove().insertBefore($('.randomContent'));
        }
    });

    $('.scroll-point li').hover(function(){
        var zindexOpeningToggle = $('.openings-toggle').css('z-index');
        $('.scroll-point').css('z-index', parseInt(zindexOpeningToggle)+1);
    });
    $('.scroll-point li').mouseleave(function(){
        var zindexScrollPoint = $('.scroll-point').css('z-index'); 
        $('.openings-toggle').css('z-index', parseInt(zindexScrollPoint)+2);
    });

    if($('input[type="radio"]:checked')){
        $('#careerResultWarp').fadeIn();
    }

    $('.careerResultBy').click(function(){
        var careerResultBy = $('input[type="radio"]:checked').val();
        if(careerResultBy == 'registerNo'){
            $('.careerResultSelectLabel').html('Registration No');
            $('.careerResultSelectInput').attr('placeholder', 'Registration No');
            $('#careerResultWarp').fadeIn();
        }else{
            $('.careerResultSelectLabel').html('Roll No');
            $('.careerResultSelectInput').attr('placeholder', 'Roll No');
            $('#careerResultWarp').fadeIn();
        }
    });

    $("#resultRegNo").keyup(function(evt){
        evt = (evt) ? evt : window.event;
        var charCode = (evt.which) ? evt.which : evt.keyCode;
        if(charCode == 8 || charCode == 46){
            if($(this).val() == ""){
                $(".careerResultFormGroupReg").removeClass("has-success");
                $(".careerResultFormGroupReg").removeClass("has-error");
            }
        }
    });

    $("#resultRegNo").keypress(function(evt){
        evt = (evt) ? evt : window.event;
        var charCode = (evt.which) ? evt.which : evt.keyCode;
        if (charCode > 31 && (charCode < 48 || charCode > 57)) {
            $(".careerResultFormGroupReg").removeClass("has-success");
            $(".careerResultFormGroupReg").addClass("has-error");
            return false;
        }
        $(".careerResultFormGroupReg").removeClass("has-error");
        $(".careerResultFormGroupReg").addClass("has-success");
        return true;
    });

    $("#dobDay").keyup(function(evt){
        evt = (evt) ? evt : window.event;
        var charCode = (evt.which) ? evt.which : evt.keyCode;
        if(charCode == 8 || charCode == 46){
            if($(this).val() == ""){
                $(".careerResultFormGroupDOB").removeClass("has-success");
                $(".careerResultFormGroupDOB").removeClass("has-error");
            }
        }
    });

    $("#dobDay").keypress(function(evt){
        evt = (evt) ? evt : window.event;
        var charCode = (evt.which) ? evt.which : evt.keyCode;
        if (charCode > 31 && (charCode < 48 || charCode > 57)) {
            $(".careerResultFormGroupDOB").removeClass("has-success");
            $(".careerResultFormGroupDOB").addClass("has-error");
            return false;
        }
        $(".careerResultFormGroupDOB").removeClass("has-error");
        $(".careerResultFormGroupDOB").addClass("has-success");
        return true;
    });

    $("#dobMonth").keyup(function(evt){
        evt = (evt) ? evt : window.event;
        var charCode = (evt.which) ? evt.which : evt.keyCode;
        if(charCode == 8 || charCode == 46){
            if($(this).val() == ""){
                $(".careerResultFormGroupDOB").removeClass("has-success");
                $(".careerResultFormGroupDOB").removeClass("has-error");
            }
        }
    });

    $("#dobMonth").keypress(function(evt){
        evt = (evt) ? evt : window.event;
        var charCode = (evt.which) ? evt.which : evt.keyCode;
        if (charCode > 31 && (charCode < 48 || charCode > 57)) {
            $(".careerResultFormGroupDOB").removeClass("has-success");
            $(".careerResultFormGroupDOB").addClass("has-error");
            return false;
        }
        $(".careerResultFormGroupDOB").removeClass("has-error");
        $(".careerResultFormGroupDOB").addClass("has-success");
        return true;
    });

    $("#dobYear").keyup(function(evt){
        evt = (evt) ? evt : window.event;
        var charCode = (evt.which) ? evt.which : evt.keyCode;
        if(charCode == 8 || charCode == 46){
            if($(this).val() == ""){
                $(".careerResultFormGroupDOB").removeClass("has-success");
                $(".careerResultFormGroupDOB").removeClass("has-error");
            }
        }
    });

    $("#dobYear").keypress(function(evt){
        evt = (evt) ? evt : window.event;
        var charCode = (evt.which) ? evt.which : evt.keyCode;
        if (charCode > 31 && (charCode < 48 || charCode > 57)) {
            $(".careerResultFormGroupDOB").removeClass("has-success");
            $(".careerResultFormGroupDOB").addClass("has-error");
            return false;
        }
        $(".careerResultFormGroupDOB").removeClass("has-error");
        $(".careerResultFormGroupDOB").addClass("has-success");
        return true;
    });

    $("#submit_value").click(function(){
        var resultRegRoll = $("#resultRegNo").val();
        var resultDOBDay = $("#dobDay").val();
        var resultDOBMonth = $("#dobMonth").val();
        var resultDOBYear = $("#dobYear").val();
        var resultUrlSend = $("#resultUrlSend").val();

        if(resultRegRoll == '' || resultDOBDay == '' || resultDOBMonth == '' || resultDOBYear == ''){
            if(resultRegRoll == ''){
                $("#resultRegRollError").removeClass("noDisplay");
            }else{
                $("#resultRegRollError").addClass("noDisplay");            
            }

            if(resultDOBDay == '' || resultDOBMonth == '' || resultDOBYear == ''){
                $("#resultDOBError").removeClass("noDisplay");
            }else{
                $("#resultDOBError").addClass("noDisplay");
            }
        }else{
            $.ajax({
                url: resultUrlSend + '/careerResultExecu.php',
                type: 'POST',
                data: {
                    'resultRegRoll'   : resultRegRoll,
                    'resultDOBDay'    : resultDOBDay,
                    'resultDOBMonth'  : resultDOBMonth,
                    'resultDOBYear'   : resultDOBYear
                },
                dataType: "html",
                beforeSend: function(){
                    $("#results").html('');
                    $("#contentLoader").fadeIn();
                },
                success: function(response){
                    $("#results").html(response);
                },
                complete: function(){
                    $("#contentLoader").fadeOut();
                }
            });
        }
    });

    $("#submit_value_main").click(function(){
        var resultBy = $('input[type="radio"]:checked').val();
        var resultRegRoll = $("#resultRegNo").val();
        var resultDOBDay = $("#dobDay").val();
        var resultDOBMonth = $("#dobMonth").val();
        var resultDOBYear = $("#dobYear").val();
        var resultUrlSend = $("#resultUrlSend").val();

        if(resultRegRoll == '' || resultDOBDay == '' || resultDOBMonth == '' || resultDOBYear == ''){
            if(resultRegRoll == ''){
                $("#resultRegRollError").removeClass("noDisplay");
            }else{
                $("#resultRegRollError").addClass("noDisplay");            
            }

            if(resultDOBDay == '' || resultDOBMonth == '' || resultDOBYear == ''){
                $("#resultDOBError").removeClass("noDisplay");
            }else{
                $("#resultDOBError").addClass("noDisplay");
            }
        }else{
            $.ajax({
                url: resultUrlSend + '/careerResultPoMainExecu.php',
                type: 'POST',
                data: {
                    'resultBy'        : resultBy,
                    'resultRegRoll'   : resultRegRoll,
                    'resultDOBDay'    : resultDOBDay,
                    'resultDOBMonth'  : resultDOBMonth,
                    'resultDOBYear'   : resultDOBYear
                },
                dataType: "html",
                beforeSend: function(){
                    $("#results").html('');
                    $("#contentLoader").fadeIn();
                },
                success: function(response){
                    $("#results").html(response);
                },
                complete: function(){
                    $("#contentLoader").fadeOut();
                }
            });
        }
    });


	/*var isDragging = false;
	$(function () {
    	var chatIcon = $("#chat");
    	chatIcon.draggable({
        	start: function () {
            	isDragging = true;
        	},
        	stop: function () {
            	isDragging = false;
        	},
        	containment: "window"
    	});
    	// Then instead of using click use mouseup, and on mouseup only fire if the flag is set to false
    	chatIcon.bind('mouseup', function () {
        	if (!isDragging) {
            	loadChatbox();
        	}
    	});
	});*/


    $("#submit_value_so").click(function(){
        var resultBy = $('input[type="radio"]:checked').val();
        var resultRegRoll = $("#resultRegNo").val();
        var resultDOBDay = $("#dobDay").val();
        var resultDOBMonth = $("#dobMonth").val();
        var resultDOBYear = $("#dobYear").val();
        var resultUrlSend = $("#resultUrlSend").val();

        if(resultRegRoll == '' || resultDOBDay == '' || resultDOBMonth == '' || resultDOBYear == ''){
            if(resultRegRoll == ''){
                $("#resultRegRollError").removeClass("noDisplay");
            }else{
                $("#resultRegRollError").addClass("noDisplay");
            }

            if(resultDOBDay == '' || resultDOBMonth == '' || resultDOBYear == ''){
                $("#resultDOBError").removeClass("noDisplay");
            }else{
                $("#resultDOBError").addClass("noDisplay");
            }
        }else{
            $("#resultRegRollError").addClass("noDisplay");
            $("#resultDOBError").addClass("noDisplay");
            $.ajax({
                url: resultUrlSend + '/careerResultSoExecu.php',
                type: 'POST',
                data: {
                    'resultBy'        : resultBy,
                    'resultRegRoll'   : resultRegRoll,
                    'resultDOBDay'    : resultDOBDay,
                    'resultDOBMonth'  : resultDOBMonth,
                    'resultDOBYear'   : resultDOBYear
                },
                dataType: "html",
                beforeSend: function(){
                    $("#results").html('');
                    $("#contentLoader").fadeIn();
                },
                success: function(response){
                    $("#results").html(response);
                },
                complete: function(){
                    $("#contentLoader").fadeOut();
                }
            });
        }
    });

    $("#submit_value_so_call_letter").click(function(){
        var resultRegRoll = $("#resultRegNo").val();
        var resultDOBDay = $("#dobDay").val();
        var resultDOBMonth = $("#dobMonth").val();
        var resultDOBYear = $("#dobYear").val();
        var resultUrlSend = $("#resultUrlSend").val();

        if(resultRegRoll == '' || resultDOBDay == '' || resultDOBMonth == '' || resultDOBYear == ''){
            if(resultRegRoll == ''){
                $("#resultRegRollError").removeClass("noDisplay");
            }else{
                $("#resultRegRollError").addClass("noDisplay");
            }

            if(resultDOBDay == '' || resultDOBMonth == '' || resultDOBYear == ''){
                $("#resultDOBError").removeClass("noDisplay");
            }else{
                $("#resultDOBError").addClass("noDisplay");
            }
        }else{
            $("#resultRegRollError").addClass("noDisplay");
            $("#resultDOBError").addClass("noDisplay");
            $.ajax({
                url: resultUrlSend + '/careerResultSoCallLetterExecu.php',
                type: 'POST',
                data: {
                    'resultRegRoll'   : resultRegRoll,
                    'resultDOBDay'    : resultDOBDay,
                    'resultDOBMonth'  : resultDOBMonth,
                    'resultDOBYear'   : resultDOBYear
                },
                dataType: "html",
                beforeSend: function(){
                    $("#results").html('');
                    $("#contentLoader").fadeIn();
                },
                success: function(response){
                    $("#results").html(response);
                },
                complete: function(){
                    $("#contentLoader").fadeOut();
                }
            });
        }
    });

    $("#submit_value_po_call_letter").click(function(){
        var resultRegRoll = $("#resultRegNo").val();
        var resultDOBDay = $("#dobDay").val();
        var resultDOBMonth = $("#dobMonth").val();
        var resultDOBYear = $("#dobYear").val();
        var resultUrlSend = $("#resultUrlSend").val();

        if(resultRegRoll == '' || resultDOBDay == '' || resultDOBMonth == '' || resultDOBYear == ''){
            if(resultRegRoll == ''){
                $("#resultRegRollError").removeClass("noDisplay");
            }else{
                $("#resultRegRollError").addClass("noDisplay");
            }

            if(resultDOBDay == '' || resultDOBMonth == '' || resultDOBYear == ''){
                $("#resultDOBError").removeClass("noDisplay");
            }else{
                $("#resultDOBError").addClass("noDisplay");
            }
        }else{
            $("#resultRegRollError").addClass("noDisplay");
            $("#resultDOBError").addClass("noDisplay");
            $.ajax({
                url: resultUrlSend + '/careerResultPoCallLetterExecu.php',
                type: 'POST',
                data: {
                    'resultRegRoll'   : resultRegRoll,
                    'resultDOBDay'    : resultDOBDay,
                    'resultDOBMonth'  : resultDOBMonth,
                    'resultDOBYear'   : resultDOBYear
                },
                dataType: "html",
                beforeSend: function(){
                    $("#results").html('');
                    $("#contentLoader").fadeIn();
                },
                success: function(response){
                    $("#results").html(response);
                },
                complete: function(){
                    $("#contentLoader").fadeOut();
                }
            });
        }
    });

    $(".openings-toggle").click(function() {
        $(this).next(".toogle-content").slideToggle();
        $(this).toggleClass("active");
    });

    $("#siteMap h2.rounded").addClass('heading-line');
    //$("#siteMap h2").addClass('heading-line');

    $('.footerList li a').mouseover(function(){
        var dnsName = this.hostname;
        if(dnsName != 'www.indianbank.in'){
            $(this).attr('target','blank');
        }
    });

    $('#menu-item-48112 a').attr('target','blank');
    $('#menu-item-48113 a').attr('target','blank');
    $('#menu-item-48114 a').attr('target','blank');
    $('#menu-item-48115 a').attr('target','blank');    
    $('#menu-item-48116 a').attr('target','blank');
    $('#menu-item-48117 a').attr('target','blank');
    $('#menu-item-48118 a').attr('target','blank');
    $('#menu-item-48119 a').attr('target','blank');


	var lgxCounter = $('.lgx-counter');
        if (lgxCounter.length) {
            lgxCounter.counterUp({
                delay: 10,
                time: 5000
            });
        }


	/*  New Allahabad Bank IFSC Code check -- Starts    */
    $('.albResultBy').click(function(){
        var albResultBy = $('input[type="radio"]:checked').val();
        if(albResultBy == 'old_ifsc'){
            $('.careerResultSelectLabel').html('Old IFSC Code');
            $('#basic-addon1').removeClass('noDisplay');
            $('#basic-addon1').html('Old IFSC Code Format <strong>(ALLA0XXXXXX)</strong>');
            $('.careerResultSelectInput').attr('placeholder', 'Old IFSC Code');
            $('#careerResultWarp').fadeIn();
        }else if(albResultBy == 'old_brcode'){
            $('.careerResultSelectLabel').html('Old Branch Code');
            $('#basic-addon1').removeClass('noDisplay');
            $('#basic-addon1').html('Old Branch Code Format <strong>(21XXXX)</strong>');
            $('.careerResultSelectInput').attr('placeholder', 'Old Branch Code');
            $('#careerResultWarp').fadeIn();
        }else{
            $('.careerResultSelectLabel').html('Branch Name');
            $('#basic-addon1').removeClass('noDisplay');
            $('#basic-addon1').html('Search By <strong>Min. 4 letters (XXXX)</strong>');
            $('.careerResultSelectInput').attr('placeholder', 'Branch Name');
            $('#careerResultWarp').fadeIn();
        }
    });

    $("#resultnewIFSC").keyup(function(evt){
        evt = (evt) ? evt : window.event;
        var charCode = (evt.which) ? evt.which : evt.keyCode;
        if(charCode == 8 || charCode == 46){
            if($(this).val() == ""){
                $(".careerResultFormGroupReg").removeClass("has-success");
                $(".careerResultFormGroupReg").removeClass("has-error");
            }
        }
    });

    $("#resultnewIFSC").keypress(function(evt){
        evt = (evt) ? evt : window.event;
        var charCode = (evt.which) ? evt.which : evt.keyCode;
        var regex = /^[A-Za-z0-9]+$/;
        var isValid = regex.test(String.fromCharCode(charCode));
        if (!isValid) {
            $(".careerResultFormGroupReg").removeClass("has-success");
            $(".careerResultFormGroupReg").addClass("has-error");
            return false;
        }
        $(".careerResultFormGroupReg").removeClass("has-error");
        $(".careerResultFormGroupReg").addClass("has-success");
        return true;
    }); 

    $("#submitNewIFSC").click(function(){
        var resultBy = $('input[type="radio"]:checked').val();
        var resultnewIFSC = $("#resultnewIFSC").val();
        var resultUrlSend = $("#resultUrlSend").val();
        if(resultnewIFSC == ''){
            if(resultnewIFSC == ''){
                $("#resultRegRollError").removeClass("noDisplay");
            }else{
                $("#resultRegRollError").addClass("noDisplay");
            }
        }else{
            $.ajax({
                url: resultUrlSend + '/newIFSCExecu.php',
                type: 'POST',
                data: {
                    'resultBy'        : resultBy,
                    'resultnewIFSC'   : resultnewIFSC
                },
                dataType: "html",
                beforeSend: function(){
                    $("#results").html('');
                    $("#contentLoader").fadeIn();
                },
                success: function(response){
                    $("#results").html(response);
                },
                complete: function(){
                    $("#contentLoader").fadeOut();
                }
            });
        }
    });
    /*  New Allahabad Bank IFSC Code check -- Starts    */



});

/*function loadChatbox() {
    var e = document.getElementById("chatbox");
    e.style.margin = "0";
    e.style.display = "block"
    var f = document.getElementById("chat");
    var t = f.style.right;
    f.style.display = "none";
}

function closeChatbox() {
    var e = document.getElementById("chatbox");
    e.style.display = "none";
    var f = document.getElementById("chat");
    f.style.display = "block"
}

function reloadChatbox() {
    var e = document.getElementById("ibotWindow");
    e.src = e.src;
}*/

/* Investor Page Counter Message */
var counter = 1;
var interval = setInterval(function() {
  counter++;
  // Display 'counter' wherever you want to display it.
  if (counter > 10) {
    counter = 0;
  } else {
	var contentMsg = jQuery('.investorMessages #'+counter).text();
    jQuery('.flashMsg').html(contentMsg);
  }
}, 5000);
jQuery(document).on('click', '#wp-megamenu-item-51032 a', function(e){ 
    e.preventDefault(); 
    var url = jQuery(this).attr('href'); 
    window.open(url, '_blank');
});
jQuery(document).on('click', '#responsive-menu-item-51032 a', function(e){ 
    e.preventDefault(); 
    var url = jQuery(this).attr('href'); 
    window.open(url, '_blank');
});


jQuery(document).ready(function($){

$("#accountNumber").keypress(function(e){
	var x = $(this).val();
	var y = e.which || e.keycode;
	if (y >= 48 && y <= 57){
		if(x.length < 9){
			$("#accNoWarp").removeClass('has-error');
			$("#accNoWarp").removeClass('has-success');
			$("#accNoWarp").addClass('has-warning');
			$("#accountNumberSign").removeClass('glyphicon-ok');
			$("#accountNumberSign").removeClass('glyphicon-remove');
			$("#accountNumberSign").removeClass('noDisplay');
			$("#accountNumberStatus").removeClass('noDisplay');
			$("#accountNumberSign").addClass('glyphicon-warning-sign');
			$("#accountAlertText").addClass('noDisplay');
			$("#accountWarningAlertText").removeClass('noDisplay');
			$("#accountErrorAlertText").addClass('noDisplay');
			$("#accountSuccessAlertText").addClass('noDisplay');
			$(".eligblityCriteria").fadeOut(500, function() {
			    $(this).addClass("noDisplay");
			});
		}else{
			$("#accNoWarp").removeClass('has-warning');
			$("#accNoWarp").removeClass('has-error');
			$("#accNoWarp").addClass('has-success');
			$("#accountNumberSign").removeClass('glyphicon-remove');
			$("#accountNumberSign").removeClass('glyphicon-warning-sign');
			$("#accountNumberSign").addClass('glyphicon-ok');
			$("#accountAlertText").addClass('noDisplay');
			$("#accountWarningAlertText").addClass('noDisplay');
			$("#accountErrorAlertText").addClass('noDisplay');
			$("#accountSuccessAlertText").removeClass('noDisplay');
			$(".eligblityCriteria").fadeIn(700, function() {
			    $(this).removeClass("noDisplay");
			});
		}
		return true;
	}else{
		$("#accNoWarp").removeClass('has-warning');
		$("#accNoWarp").removeClass('has-success');
		$("#accNoWarp").addClass('has-error');
		$("#accountNumberSign").removeClass('glyphicon-warning-sign');
		$("#accountNumberSign").removeClass('glyphicon-ok');
		$("#accountNumberSign").removeClass('noDisplay');
		$("#accountNumberStatus").removeClass('noDisplay');
		$("#accountNumberSign").addClass('glyphicon-remove');
		$("#accountAlertText").addClass('noDisplay');
		$("#accountWarningAlertText").addClass('noDisplay');
		$("#accountErrorAlertText").removeClass('noDisplay');
		$("#accountSuccessAlertText").addClass('noDisplay');
		$(".eligblityCriteria").fadeOut(500, function() {
		    $(this).addClass("noDisplay");
		});
		return false;
	}
		return true;
	});

	$("input[type='button']").click(function(){
		$("input[type='radio']").attr('disabled', true);	
		$("input[type='button']").attr('disabled', true);
		$("#accountNumber").attr('disabled', true);
        var radioValue1 = $("input[name='quest1']:checked").val();
        var radioValue2 = $("input[name='quest2']:checked").val();
        var radioValue3 = $("input[name='quest3']:checked").val();
        var radioValue4 = $("input[name='quest4']:checked").val();
        var radioValue5 = $("input[name='quest5']:checked").val();
        var radioValue6 = $("input[name='quest6']:checked").val();
        var radioValue7 = $("input[name='quest7']:checked").val();
        var radioValue8 = $("input[name='quest8']:checked").val();
       	var countRadioValue = parseInt(radioValue1) + parseInt(radioValue2) + parseInt(radioValue3) + parseInt(radioValue4) + parseInt(radioValue5) + parseInt(radioValue6) + parseInt(radioValue7) + parseInt(radioValue8);
       	if(countRadioValue === 8){
       		$(".eligibityCustomer").removeClass('noDisplay');
       	}else{
       		if(radioValue1 != 1){
       			$(".quest1").addClass('radio1');
       		}
       		if(radioValue2 != 1){
       			$(".quest2").addClass('radio2');
       		}
       		if(radioValue3 != 1){
       			$(".quest3").addClass('radio3');
       		}
       		if(radioValue4 != 1){
       			$(".quest4").addClass('radio4');

       		}
       		if(radioValue5 != 1){
       			$(".quest5").addClass('radio5');
       		}
       		if(radioValue6 != 1){
       			$(".quest6").addClass('radio6');
       		}
       		if(radioValue7 != 1){
       			$(".quest7").addClass('radio7');
       		}
       		if(radioValue8 != 1){
       			$(".quest8").addClass('radio8');
       		}
       		$(".nonEligibityCustomer").removeClass('noDisplay');
       	}
       });


});



