/* Set the width of the side navigation to 250px and the left margin of the page content to 250px */
function openNav() {
    document.getElementById("mySidenav").style.width = "250px";
    document.getElementById("mySidenav").style.height = "100%";
    document.getElementById("mySidenav").style.display = "block";
    document.getElementById("main").style.marginLeft = "250px";
}

/* Set the width of the side navigation to 0 and the left margin of the page content to 0 */
function closeNav() {
    closeSubUser();
    document.getElementById("mySidenav").style.width = "0px";
    document.getElementById("mySidenav").style.height = "0px";
    document.getElementById("mySidenav").style.display = "none";
    document.getElementById("main").style.marginLeft = "0px";
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

function mostrarAcciones(id) {
    if ((document.getElementById("SubActividades" + id).style.display == "") ||
        (document.getElementById("SubActividades" + id).style.display == "none")) {
        document.getElementById("SubActividades" + id).style.display = "block"
    } else {
        document.getElementById("SubActividades" + id).style.display = "none"
    }
}

function mostrarModificarSujeto(id) {
    if ((document.getElementById("formModificarSujeto" + id).style.display == "") ||
        (document.getElementById("formModificarSujeto" + id).style.display == "none")) {
        document.getElementById("formModificarSujeto" + id).style.display = "block"
    } else {
        document.getElementById("formModificarSujeto" + id).style.display = "none"
    }
}

function mostrarCrearSujeto() {
    if ((document.getElementById("formCrearSujeto").style.display == "") ||
        (document.getElementById("formCrearSujeto").style.display == "none")) {
        document.getElementById("formCrearSujeto").style.display = "block"
    } else {
        document.getElementById("formCrearSujeto").style.display = "none"
    }
}

function mostrarAccionesObs(id) {
    if ((document.getElementById("Lista" + id).style.display == "") ||
        (document.getElementById("Lista" + id).style.display == "none")) {
        document.getElementById("Lista" + id).style.display = "block"
    } else {
        document.getElementById("Lista" + id).style.display = "none"
    }
}