/* JS Document */

/******************************

[Table of Contents]

1. Vars and Inits
2. Set Header
3. Init Menu
4. Init Isotope


******************************/

$(document).ready(function()
{
	"use strict";

	/* 

	1. Vars and Inits

	*/

	var header = $('.header');
	var hamburgerBar = $('.hamburger_bar');
	var hamburger = $('.hamburger');

	setHeader();

	$(window).on('resize', function()
	{
		setHeader();

		setTimeout(function()
		{
			$(window).trigger('resize.px.parallax');
		}, 375);
	});

	$(document).on('scroll', function()
	{
		setHeader();
	});

	initMenu();
	initIsotope();

	/* 

	2. Set Header

	*/

	function setHeader()
	{
		if($(window).scrollTop() > 91)
		{
			header.addClass('scrolled');
			hamburgerBar.addClass('scrolled');
		}
		else
		{
			header.removeClass('scrolled');
			hamburgerBar.removeClass('scrolled');
		}
	}

	/* 

	3. Init Menu

	*/

	function initMenu()
	{
		if($('.menu').length)
		{
			var menu = $('.menu');
			hamburger.on('click', function()
			{
				hamburger.toggleClass('active');
				menu.toggleClass('active');
			});
		}
	}

	/* 

	4. Init Isotope

	*/

    function initIsotope()
    {
    	var sortingButtons = $('.item_sorting_btn');

    	if($('.grid').length)
    	{
    		var grid = $('.grid').isotope({
	  			itemSelector: '.grid-item',
	  			percentPosition: true,
	  			masonry:
	  			{
				    horizontalOrder: true
			  	},
			  	getSortData:
	            {
	            	price: function(itemElement)
	            	{
	            		var priceEle = $(itemElement).find('.product_price').text().replace( '$', '' );
	            		return parseFloat(priceEle);
	            	},
	            	name: '.tt_class_title'
	            }
	        });

	        // Filtering
	        $('.item_filter_btn').on('click', function()
	        {
	        	var buttons = $('.item_filter_btn');
	        	buttons.removeClass('active');
	        	$(this).addClass('active');
		        var filterValue = $(this).attr('data-filter');
				grid.isotope({ filter: filterValue });
	        });
    	}
    }

});