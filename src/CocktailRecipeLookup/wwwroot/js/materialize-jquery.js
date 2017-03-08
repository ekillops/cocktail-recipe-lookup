﻿$(document).ready(function () {

    $('select').material_select();
    $(".dropdown-button").dropdown();
    $('.parallax').parallax();
    $('.modal').modal();

    $('.button-collapse').sideNav({
        menuWidth: 200, // Default is 300
        edge: 'right', // Choose the horizontal origin
        closeOnClick: true, // Closes side-nav on <a> clicks, useful for Angular/Meteor
        draggable: true // Choose whether you can drag to open on touch screens
    });

    $('a#toggle-search').click(function () {
        var search = $('div#search');

        search.is(":visible") ? search.slideUp() : search.slideDown(function () {
            search.find('input').focus();
        });

        return false;
    });
});