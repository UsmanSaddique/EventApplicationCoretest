$('.remove-item').click(function () {
    var element = this.id;
    var id = element.replace("remove-item", "");
    $("#university-Detail" + id).remove();

});


function _applySelectUniversity(item, self, value) {
 
    setTimeout(function () {
        item.select2({
            width: "100%", //self.width,
            placeholder: 'Search for University',
            //allowClear: true,
            templateResult: formatUniversityOption,
            ajax: ({
                url: '/AusPak/Api/GetUniversity',
                dataType: 'json',
                delay: 250,
                processResults: function (data) {
                    return {
                        results: data
                    };
                },
                cache: true
            })
        });
    });
}
function formatUniversityOption(data) {
 
    var option = $('<div><strong>Id : ' + data.id + '</strong></div><div>Name : ' + data.text + '</div>');
    return option;
}
function GetCountry(id) {
    var option = '<option selected disabled>Select...</option>';
    $.ajax({
        type: 'GET',
        url: '/AusPak/Api/GetCountry',
        contentType: 'json',
        success: function (result) {
         
            //   var result = JSON.parse(result);
            $.each(result, function (key, value) {
                option += '<option value=' + value.id + '>' + value.text + '</option>';
            });
            $('#CountryId' + id).html(option);

        }
    });
}
function GetCommisionType(id) {
    var option = '<option selected disabled>Select...</option>';
    $.ajax({
        type: 'GET',
        url: '/AusPak/Api/GetCommisionType',
        contentType: 'json',
        success: function (result) {
         
            //   var result = JSON.parse(result);
            $.each(result, function (key, value) {
                option += '<option value=' + value.id + '>' + value.text + '</option>';
            });
            $('#CommisionType' + id).html(option);

        }
    });
}
function getItemDetails(val) {
 
    var element = val.replace("UniversityId", "");
    var id = $('#' + val).val();
    $.ajax({
        url: '/AusPak/UniversityAgreement/GetUniversityDetails?id=' + id,
        type: 'GET'
    }).done(function (data) {
             
       // $('#Type_' + element).prop('readonly', false);
        $('#Type_' + element).val(data);
       // $('#Type_' + element).prop('readonly', true);
    });
    //if ($('#ItemId' + element).val() == null) {
    //    $('.dependent').prop('readonly', 'readonly');
    //}
    //else {
    //    $('.dependent').removeAttr('readonly', 'readonly');
    //    $('select[id=TaxSlab_' + element + ']').removeAttr("disabled", "disabled");

    //}
}


