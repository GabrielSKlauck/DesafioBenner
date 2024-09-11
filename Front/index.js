window.onload=function(){
    let dataAtual = new Date();
    let data = dataAtual.getDay() + "/" + dataAtual.getMonth() + "/" + dataAtual.getFullYear();
    document.getElementById("horario-atual").innerHTML = data
}

function abrirModal(){
    let form = document.getElementById("form");
    form.style.display = "block";
}