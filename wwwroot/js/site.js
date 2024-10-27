// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

document.addEventListener('DOMContentLoaded', function() {
    var table = document.querySelector('.table-scroll table');
    var scrollContainer = document.querySelector('.table-scroll');
    
    function adjustScrollBoxWidth() {
        var tableWidth = table.offsetWidth;
        scrollContainer.style.width = tableWidth + 'px';
    }

    // Run on page load
    adjustScrollBoxWidth();

    // Run on window resize
    window.addEventListener('resize', adjustScrollBoxWidth);
});
