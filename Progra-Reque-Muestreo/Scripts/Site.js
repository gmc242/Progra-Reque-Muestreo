/* Set the width of the side navigation to 250px and the left margin of the page content to 250px */
function openNav() {
    document.getElementById("mySidenav").style.width = "250px";
    document.getElementById("mySidenav").style.height = "100%";
    document.getElementById("mySidenav").style.display = "block";
    document.getElementById("main").style.marginLeft = "250px";
}

/* Set the width of the side navigation to 0 and the left margin of the page content to 0 */
function closeNav() {
    document.getElementById("mySidenav").style.width = "0px";
    document.getElementById("mySidenav").style.height = "0px";
    document.getElementById("mySidenav").style.display = "none";
    closeSubMod()
    document.getElementById("main").style.marginLeft = "0px";
} 

function sub() {
    if ((document.getElementById("subModificar").style.height == "") ||
        (document.getElementById("subModificar").style.height == "0px")){
        openSubMod();
    } else {
        closeSubMod();
    }
}

function openSubMod() {
    document.getElementById("subModificar").style.width = "250px";
    document.getElementById("subModificar").style.height = "auto";
    document.getElementById("subModificar").style.display = "block";
}

function closeSubMod() {
    document.getElementById("subModificar").style.width = "0px";
    document.getElementById("subModificar").style.height = "0px";
    document.getElementById("subModificar").style.display = "none";
}

function subUser() {
    if ((document.getElementById("subUsuario").style.height == "") ||
        (document.getElementById("subUsuario").style.height == "0px")) {
        openSubUser();
    } else {
        closeSubUser();
    }
}

function openSubUser() {
    document.getElementById("subUsuario").style.width = "250px";
    document.getElementById("subUsuario").style.height = "auto";
    document.getElementById("subUsuario").style.display = "block";
}

function closeSubUser() {
    document.getElementById("subUsuario").style.width = "0px";
    document.getElementById("subUsuario").style.height = "0px";
    document.getElementById("subUsuario").style.display = "none";
}