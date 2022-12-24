function check(index)
{
    $(index).on('keyup keydown', function (e) {
        var newVal = $(this).val().replace(".", ",");
        $(this).val(newVal);
    });
}

check('#person_height');
check('#person_weight');




