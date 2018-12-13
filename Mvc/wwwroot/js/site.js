// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.



// Campo de pesquisa
$('#filtro_tabela').on('keyup', function () {
    var nomeFiltro = $(this).val().toLowerCase();
    $('#tabela_dados').find('tr').each(function () {
        var conteudoCelula = $(this).find('td').text();
        var corresponde = conteudoCelula.toLowerCase().indexOf(nomeFiltro) >= 0;
        $(this).css('display', corresponde ? '' : 'none');
    });
});

// Modal Editar Estoque
$(document).ready(function () {
    $(".btnEditar").click(function () {
        var id = $(this).data("value");
        $("#conteudoModal").load("/Estoque/Editar/" + id,
            function () {
                $('#myModal').modal("show");
            }
        );
    });
});

// Modal Salvar Estoque
$(document).ready(function () {
    $(".btnNovo").click(function () {
        var id = $(this).data("value");
        $("#conteudoModal").load("/Estoque/Salvar/" + id,
            function () {
                $('#myModal').modal("show");
            }
        );
    });
});

// Modal Deletar Estoque
$(document).ready(function () {
    $(".btnDeletar").click(function () {
        var id = $(this).data("value");
        $("#conteudoModal").load("/Estoque/Deletar/" + id,
            function () {
                $('#myModal').modal("show");
            }
        );
    });
});

// Modal Salvar Produto

