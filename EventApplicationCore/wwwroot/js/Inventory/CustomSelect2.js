function _applySelect(item, self, value) {
    setTimeout(function () {
        item.select2({
            width: "100%", //self.width,
            placeholder: 'Search for an Item',
            //allowClear: true,
            templateResult: formatOption,
            ajax: ({
                url: '/Inventory/Api/GetItems',
                dataType: 'json',
                delay: 250,
                processResults: function (data, params) {
                    return {
                        results: data
                    };
                },
                cache: true
            })
        });
    });
}

function _applySelectAccount(item, self, value) {
    setTimeout(function () {
        item.select2({
            width: "100%", //self.width,
            placeholder: 'Search for an Account',
            //allowClear: true,
            templateResult: formatOption,
            ajax: ({
                url: '/AusPak/Api/GetAccounts',
                dataType: 'json',
                delay: 250,
                processResults: function (data, params) {
                    return {
                        results: data
                    };
                },
                cache: true
            })
        });
    });
}
function formatOption(data) {
    var option = $('<div><strong>Code : </strong>' + data.code + '</div><div><strong>Name : </strong>' + data.text + '</div>');
    return option;
}



//function formatOption(data) {
//    var option = $('<div><strong>Id : ' + data.id + '</strong></div><div>Name : ' + data.text + '</div><div>Code : ' + data.code + '</div><div><p>Item type is vehicle for all current available items</p></div>');
//    return option;
//}