function OpenModal(url, title, autocomplete) {
    if (url !== undefined) {
        $.get(url, function (data) {
            $('.modal-body').empty().append(data);
            $('.modal-header-title').html(title);
            $('.modal').show();

            if (autocomplete) {
                MakeAutoComplete();
            }
        });
    }
    else {
        $('.modal').show();
    }
}

function MakeAutoComplete() {
    $("#author-autocomplete").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/autores/pesquisar",
                data: { query: request.term },
                success: function (data) {
                    var transformed = $.map(JSON.parse(data), function (el) {
                        return {
                            label: el.name,
                            value: el.name,
                            id: el.id
                        };
                    });
                    response(transformed);
                },
                error: function () {
                    response([]);
                }
            });
        },
        select: function (event, ui) {
            $("#AuthorId").val(ui.item.id);
            $("#author-autocomplete").val(ui.item.value);
        }
    });
    $("#author-autocomplete").autocomplete("option", "appendTo", "#ajaxSubmit");

    $("#category-autocomplete").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/categorias/pesquisar",
                data: { query: request.term },
                success: function (data) {
                    var transformed = $.map(JSON.parse(data), function (el) {
                        return {
                            label: el.name,
                            value: el.name,
                            id: el.id
                        };
                    });
                    response(transformed);
                },
                error: function () {
                    response([]);
                }
            });
        },
        select: function (event, ui) {
            $("#CategoryId").val(ui.item.id);
            $("#catecory-autocomplete").val(ui.item.value);
        }
    });
    $("#category-autocomplete").autocomplete("option", "appendTo", "#ajaxSubmit");

}


$(function () {

    $(".modal .close").click(function () {
        $('.modal').hide();
    });

    $('#btn-close').click(function () {
        $(".modal .close").trigger('click');
    });

    $('#btn-save-ajax').click(function (e) {
        $('#ajaxSubmit').unbind('submit').bind('submit', function (e) {
            e.preventDefault();
            var form = $(this);
            var url = form.attr('action');

            $.ajax({
                type: "POST",
                url: url,
                data: form.serialize(),
                success: function (data) {
                    if (data.success) {
                        window.location.reload();
                    }
                    else {
                        var alert = $('<div class="alert alert-danger" style="margin: 25px 0 15px 5%; width: 90%;"><strong>Falha ao processar requisi&ccedil;&atilde;o</strong></div>');
                        $.each(data.errors, function (i, el) {
                            alert.append("<p>" + el + "</p>");
                        });

                        form.find('.alert').remove();
                        form.prepend(alert);
                    }
                }
            });
        });
        $('#ajaxSubmit').trigger('submit');
    });

    $(".modal").click(function (ev, el) {
        if (ev.target.classList.contains("modal")) {
            $(".modal .close").trigger('click');
        }
    });
});