// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    $('.dropdown-submenu a.dropdown-toggle').on("click", function (e) {
        e.preventDefault();
        e.stopPropagation();
        var $subMenu = $(this).next(".dropdown-menu");
        $('.dropdown-submenu .dropdown-menu').not($subMenu).removeClass('show');
        $subMenu.toggleClass('show');
    });
});
