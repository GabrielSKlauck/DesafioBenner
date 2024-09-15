//AO CARREGAR O HTML, CARREGA A DATA ATUAL
window.onload=function(){
    let dataAtual = new Date();
    let data = dataAtual.getDay() + "/" + dataAtual.getMonth() + "/" + dataAtual.getFullYear();
    document.getElementById("horario-atual").innerHTML = data

    $.ajax({
        type: "GET",
        url: `https://localhost:7070/ControleCarro`,
        success: carregaTabela,
        header: {},
        contentType: "application/json",
        datatype: "json",
    });
}

//FORMATA A PLACA DIGITADA CORRETAMENTE
document.addEventListener('DOMContentLoaded', function() {
    let placas = document.getElementsByClassName('input-info'); 
    for(const placaInd of placas){
        placaInd.addEventListener('input', function(event) {
            let value = placaInd.value;
            value = value.replace(/[^A-Z0-9]/gi, '');
            if (value.length <= 7) {
                value = value.toUpperCase(); 
                value = value.replace(/^([A-Z]{3})([A-Z0-9]{0,4})/, '$1-$2');
            }    
            placaInd.value = value;
        });
    }
});

//ABRE O MODAL DE ENVIO DA PLACA DO CARRO
function abrirModal(){
    let input = document.getElementById("placa");
    input.value = "";
    cancelar();
    let form = document.getElementById("form");
    form.style.display = "block"; 
}

function abrirModalPesquisa(){
    let input = document.getElementById("placa");
    input.value = "";
    cancelar();
    let divPesquisa = document.getElementById("caixaPesquisa");
    divPesquisa.style.display = "block";
}

function pesquisarPlaca(){

}

function abrirModalPesquisaDia(){
    let input = document.getElementById("dia");
    input.value = "";
    cancelar();
    let divPesquisa = document.getElementById("caixaPesquisaDia");
    divPesquisa.style.display = "block";
}

function pesquisarDia(){
    cancelar();
}

function cancelar(){
    event.preventDefault();
    let input = document.getElementsByClassName("input-info");
    for(const inputSeparado of input){
        inputSeparado.value = "";
    }
    let divs = document.getElementsByClassName("modal-meu")
    for(const divsSeparadas of divs){
        divsSeparadas.style.display = "none";
    }  
}

//ENVIO DA PLACA PARA O BANCO
function registrarPlaca(){
    //event.preventDefault();
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

//CARREGA CARROS DO BANCO E AGREGA AO TBODY 
function carregaTabela(itens){
    itens.forEach(linha => { 
        const carro = `
            <tr>
                <td>${linha.id}</td>
                <td>${linha.placa}</td>
                <td>${linha.chegada}</td> 
                <td>${linha.saida === "0001-01-01T00:00:00" ? "-" : linha.saida}</td>
                <td>${linha.duracao === "00:00:00" ? "-" : linha.duracao}</td>
                <td>${linha.tempoCobradoHora}</td> 
                <td>${linha.preco}</td>
                <td>${linha.valorPagar}</td>
                <td><Button class="btn btn-danger" onclick="finalizar('${linha.placa}')">Finalizar</Button></td>
            </tr>
       `;
        $(`#listagem`).append($(carro));
    });  
}

function finalizar(placa){
    console.log("acho")
    $.ajax({
        type: "PUT",
        url: `https://localhost:7070/ControleCarro/${placa}`,
        header: {},
        success: window.location.reload(),
        contentType: "application/json",
        datatype: "json",
    });
}