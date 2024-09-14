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

//FORMATA A PLACA DIGITADA CORRETAMENTE
document.addEventListener('DOMContentLoaded', function() {
    let placa = document.getElementById('placa'); 
    placa.addEventListener('input', function(event) {
        let value = placa.value;
        value = value.replace(/[^A-Z0-9]/gi, '');
        if (value.length <= 7) {
            value = value.toUpperCase(); 
            value = value.replace(/^([A-Z]{3})([A-Z0-9]{0,4})/, '$1-$2');
        }    
        placa.value = value;
    });
});

//ABRE E FECHA O MODAL DE ENVIO DA PLACA DO CARRO
function abrirModal(){
    let input = document.getElementById("placa");
    input.value = "";
    let form = document.getElementById("form");
    form.style.display = "block";
    
}

function registrarPlaca(){
    let placa = document.getElementById("placa").value;
    if(placa === ""){
        return;
    }
    $.ajax({
        type: "POST",
        url: `https://localhost:7070/ControleCarro/${placa}`,
        header: {},
        contentType: "application/json",
        datatype: "json",
    });
}