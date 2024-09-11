//AO CARREGAR O HTML, CARREGA A DATA ATUAL
window.onload=function(){
    let dataAtual = new Date();
    let data = dataAtual.getDay() + "/" + dataAtual.getMonth() + "/" + dataAtual.getFullYear();
    document.getElementById("horario-atual").innerHTML = data
}

//CANCELA E NÃO RECARREGA A PAGINA
document.addEventListener('DOMContentLoaded', function() {
    var button = document.getElementById('cancelar');
    button.addEventListener('click', function(event) {
        event.preventDefault();
        let form = document.getElementById("form");
        form.style.display = "none";
    });
});

function abrirModal(){
    let form = document.getElementById("form");
    form.style.display = "block";
}