function OpenModal(url) {
    if (url !== undefined) {
        $.get(url, function (data) {
            $('.modal-body').empty().append(data);
            $('.modal').show();
        });
    }
    else {
        $('.modal').show();
    }
}

$(function () {

    $(".modal .close").click(function () {
        $('.modal').hide();
    });

    $(".modal").click(function (ev, el) {
        if (ev.target.classList.contains("modal")) {
            $(".modal .close").trigger('click');
        }
    });

    $('#btn-save-ajax').click(function (e) {
        $('#ajaxSubmit').submit(function (e) {
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
                        form.prepend("<p>" + data.errors + "</p>");
                    }
                }
            });
        });

        $('#ajaxSubmit').submit();
    });
})