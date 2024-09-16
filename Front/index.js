//AO CARREGAR O HTML, CARREGA A DATA ATUAL
window.onload = function () {
    let dataAtual = new Date();
    let data = dataAtual.getDay() + "/" + dataAtual.getMonth() + "/" + dataAtual.getFullYear();
    document.getElementById("horario-atual").innerHTML = data

    carregarItens();
}

function carregarItens() {
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
document.addEventListener('DOMContentLoaded', function () {
    let placas = document.getElementsByClassName('input-info');
    for (const placaInd of placas) {
        placaInd.addEventListener('input', function (event) {
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
function abrirModal() {
    let input = document.getElementById("placa");
    input.value = "";
    cancelar();
    let form = document.getElementById("form");
    form.style.display = "block";
}

function abrirModalPesquisa() {
    let input = document.getElementById("placa");
    input.value = "";
    cancelar();
    let divPesquisa = document.getElementById("caixaPesquisa");
    divPesquisa.style.display = "block";
}

function pesquisarPlaca() {
    let placa = document.getElementById("placa-pesquisa");
    placa = placa.value;
    $.ajax({
        type: "GET",
        url: `https://localhost:7070/ControleCarro/getUnico/${placa}`,
        success: carregaItem,
        header: {},
        contentType: "application/json",
        datatype: "json",
    });
}

function carregaItem(itens) {
    let tabela = document.getElementById("listagem");
    tabela.innerHTML = "";
    itens.forEach(linha => {
        if (linha.duracao != "00:00:00") {
            btn = "<Button class='btn btn-warning' disable><i>Finalizado</i></Button>";
        } else {
            btn = `<Button class='btn btn-danger' onclick='finalizar("${linha.placa}")'>Finalizar</Button>`;
        }
        const carro = `
            <tr>
                <td>${linha.id}</td>
                <td>${linha.placa}</td>
                <td>${formatacaoDatas(linha.chegada)}</td> 
                <td>${formatacaoDatas(linha.saida === "0001-01-01T00:00:00" ? "-" : linha.saida)}</td>
                <td>${linha.duracao === "00:00:00" ? "-" : linha.duracao}</td>
                <td>${linha.tempoCobradoHora}</td> 
                <td>${linha.preco}</td>
                <td>R$ ${parseFloat(linha.valorPagar).toFixed(2)}</td>
                <td>${btn}</td>
            </tr>
       `;
        $(`#listagem`).append($(carro));
    });
}

function abrirModalEspec() {
    let input = document.getElementById("placa");
    input.value = "";
    let chegada = document.getElementById("chegada");
    chegada.value = 0;
    let saida = document.getElementById("saida");
    saida.value = 0;
    cancelar();
    let divEspec = document.getElementById("caixaPesquisaEspec");
    divEspec.style.display = "block";
}

function registrarEspecifico() {
    let input = document.getElementById("placa-espec").value;
    let c = document.getElementById("chegada").value;
    let s = document.getElementById("saida").value;
    if (s < c) {
        return;
    }
    if (input === "") {
        return;
    }
    carro = {
        placa: input,
        chegada: c,
        saida: s
    }
    $.ajax({
        type: "POST",
        url: `https://localhost:7070/ControleCarro/addEspec`,
        data: JSON.stringify(carro),
        success: carregarItens,
        header: {},
        contentType: "application/json",
        datatype: "json",
    });
}

function cancelar() {
    let input = document.getElementsByClassName("input-info");
    for (const inputSeparado of input) {
        inputSeparado.value = "";
    }
    let divs = document.getElementsByClassName("modal-meu")
    for (const divsSeparadas of divs) {
        divsSeparadas.style.display = "none";
    }
    carregarItens();
}

//ENVIO DA PLACA PARA O BANCO
function registrarPlaca() {

    let placa = document.getElementById("placa").value;
    if (placa === "") {
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
function carregaTabela(itens) {
    let tabela = document.getElementById("listagem");
    tabela.innerHTML = "";
    let btn;
    itens.forEach(linha => {
        if (linha.duracao != "00:00:00") {
            btn = "<Button class='btn btn-warning' disable><i>Finalizado</i></Button>";
        } else {
            btn = `<Button class='btn btn-danger' onclick='finalizar("${linha.placa}")'>Finalizar</Button>`;
        }
        const carro = `
            <tr>
                <td>${linha.id}</td>
                <td>${linha.placa}</td>
                <td>${formatacaoDatas(linha.chegada)}</td> 
                <td>${formatacaoDatas(linha.saida === "0001-01-01T00:00:00" ? "-" : linha.saida)}</td>
                <td>${linha.duracao === "00:00:00" ? "-" : linha.duracao}</td>
                <td>${linha.tempoCobradoHora}</td> 
                <td>${linha.preco}</td>
                <td>R$ ${parseFloat(linha.valorPagar).toFixed(2)}</td>
                <td>${btn}</td>
            </tr>
       `;
        $(`#listagem`).append($(carro));
    });
}

function formatacaoDatas(dataSql) {
    if(dataSql === "-"){
        return "-";
    }
    let data = new Date(dataSql);

    let dia = String(data.getDate()).padStart(2, '0');
    let mes = String(data.getMonth() + 1).padStart(2, '0');
    let ano = data.getFullYear();
    let horas = String(data.getHours()).padStart(2, '0');
    let minutos = String(data.getMinutes()).padStart(2, '0');
    let segundos = String(data.getSeconds()).padStart(2, '0');

    let dataFormatada = `${dia}-${mes}-${ano} ${horas}:${minutos}:${segundos}`;
    return dataFormatada;
}

function finalizar(placa) {
    $.ajax({
        type: "PUT",
        url: `https://localhost:7070/ControleCarro/${placa}`,
        header: {},
        success: carregarItens,
        contentType: "application/json",
        datatype: "json",
    });
    cancelar();
}