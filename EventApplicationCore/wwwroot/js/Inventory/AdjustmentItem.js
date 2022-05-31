//global array declaration
var ids = [];

$('.remove-item').click(function () {
    var element = this.id;
    var id = element.replace("remove-item", "");


    var itemId = $('#AdjustmentItemId'+id).val();
    if (itemId != null) {
        ids.push(itemId);
        console.log(ids);
    }

    //for remove Id of Adjustmentitems
    $('#IdsDeleted').text(ids);
    $("#current-item" + id).remove();
});
function getItemValues(itemId, val) {
    var id = itemId.replace("ItemId", "");
    $.ajax({
        url: '/AR/Invoice/GetUOM?id=' + val,
        type: 'GET'
    }).done(function (data) {
        $('#UOMId_'+id).val(data[0].id);
        $('#UOM_Value'+id).val(data[0].uom);
        $('#Rate_'+id).val(data[0].rate);
        $('#Stock_' + id).val(data[0].stock);
        console.log($('#UOMId_'+id).val());
    });
}
$('.calculate-total').on('keyup', function () {
    var split = (this.id).split('_');
    var id = split[1];
    var stock = parseFloat($('#Stock_' + id).val());
    var physicalStock = $(this).val();
    $('#Balance_' + id).val((stock - physicalStock).toFixed(2));
});

