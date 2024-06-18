// site.js
function toggleMenu(menuId) {
    var menu = document.getElementById(menuId);
    var menus = document.getElementsByClassName('submenu');

    for (var i = 0; i < menus.length; i++) {
        if (menus[i].id !== menuId) {
            menus[i].style.display = 'none';
        }
    }

    if (menu.style.display === "block") {
        menu.style.display = "none";
    } else {
        menu.style.display = "block";
    }
}
