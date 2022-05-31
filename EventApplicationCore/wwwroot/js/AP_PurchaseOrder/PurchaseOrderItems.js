var ids = [];
$('.remove-item').click(function () {
    var element = this.id;
    var id = element.replace("remove-item", "");
    //ids to remove
    var itemId = $('#PRItemId' + id).val();
    if (itemId != null) {
        ids.push(itemId);
    }
    var removedTotal = $('#Total_' + id).val();
    if (removedTotal != null) {
        calculateTotalAmount(removedTotal);
    }
    var removedSalesTax = $('#TaxAmount_' + id).val();
    if (removedSalesTax != null) {
        calculateTotalSalesTax(removedSalesTax);
    }
    var removedLine = $('#LineTotal_' + id).val();
    if (removedLine != null) {
        calculateFreightAmount(removedLine);
    }
    var removedGrandTotal = $('#LineTotal_' + id).val();
    if (removedSalesTax != null) {
        calculateGrandTotal1(removedGrandTotal);
    }

    
    $('#IdsDeleted').text(ids);
    $("#purchase-Order" + id).remove();

});

$('.calculate-total').on('change', function () {
    var elementId = this.id;
    var id = elementId.split('_');
    id = id[1];
    //alert(id/1);
    //var elementValue = $(this).val();
    var qty = $('#Qty_' + id + '').val();
    var rate = $('#Rate_' + id + '').val();
    $('#Total_' + id).val((qty * rate).toFixed(2));
    //After selecting tax slab calculating other values
    $('#LineTotal_' + id).val($('#Total_' + id).val());
    calculateTax(elementId);
    //
    var totalBefore = $('#Total_' + id).val();
    $('#Total_' + id).val((qty * rate).toFixed(2));
    var totalAfter = $('#Total_' + id).val();
    calculateTotal(totalBefore, totalAfter);
    //
    var lineBefore = $('#LineTotal_' + id).val();
    $('#LineTotal_' + id).val((qty * rate).toFixed(2));
    var lineAfter = $('#LineTotal_' + id).val();
    calculateGrandTotal(lineBefore, lineAfter);
});

function calculateTax(elementId) {
    var id = elementId.split('_');
    id = id[1];
    //tax slab defined
    var taxPercentage = $('#TaxSlab_' + id).val();// %age of tax slab
    var slabVal=0;
    if (taxPercentage == 1) {
        slabVal = 15;
    }
    else if (taxPercentage == 2) {
        slabVal = 16;

    }
    else if (taxPercentage == 3) {
        slabVal = 17;

    }     
    var lineBefore = $('#LineTotal_' + id).val();
    var total = $('#Total_' + id).val();
    var taxAmount = ((slabVal / 100) * total).toFixed(2);
    var lineAfter = (Number(total) + Number(taxAmount)).toFixed(2)
    $('#LineTotal_' + id).val(lineAfter);
    //
    var tax = $('#TotalTaxAmount').val();
    var newTax = tax - $('#TaxAmount_' + id).val();
    $('#TaxAmount_' + id).val(taxAmount);
    $('#TotalTaxAmount').val((Number(taxAmount) + Number(newTax)).toFixed(2));
    //
    var totalAmt = $('#Total').val();
    var newTotal = $('#Total_' + id).val();
    calculateTotal(totalAmt, newTotal);
    calculateGrandTotal(lineBefore, lineAfter);

}

function calculateTotal(before, after) {
    //    New Net Value
    var total = $('#Total').val();
    var remainingTotal = total - before;
    var newTotal;
    if (total != 0.00) {
        newTotal = (Number(remainingTotal) + Number(after)).toFixed(2);
    }
    else {
        newTotal = after;
    }
    $('#Total').val(newTotal);
}

function calculateGrandTotal(before, after) {
    //    New Net Value
    var grandTotal = $('#GrandTotal').val();
    var remainingNetValue = grandTotal - before;
    var newGrandTotal;
    if (grandTotal != 0) {
        newGrandTotal = (Number(remainingNetValue) + Number(after)).toFixed(2);
    }
    else {
        newGrandTotal = after;
    }
    $('#GrandTotal').val(newGrandTotal);
    $('#Freight').val(newGrandTotal);
}

function getItemDetails(val) {
    var element = val.replace("ItemId", "");
    var id = $('#' + val).val();
    $.ajax({
        url: '/AP/PurchaseOrder/GetItemDetails?id=' + id,
        type: 'GET'
    }).done(function (data) {
        $('.UOM' + element).val(data[0].id);
        $('#UOM' + element).html(data[0].uom);
    });
    if ($('#ItemId' + element).val() == null) {
        $('.dependent').prop('readonly', 'readonly');
    }
    else {
        $('.dependent').removeAttr('readonly', 'readonly');
        $('select[id=TaxSlab_' + element + ']').removeAttr("disabled", "disabled");

    }
}

function lockOutInputs(counter) {

    if ($('#ItemId' + counter).val() == null) {
        $('.dependent').prop('readonly', 'readonly');
        //$('.dependent select').attr('disabled', 'disabled');
        $('select[id=TaxSlab_' + counter + ']').prop("disabled", "disabled");
    }
    else {
        $('.dependent').removeAttr('readonly', 'readonly');
        $('select[id=TaxSlab_' + counter + ']').removeAttr("disabled", "disabled");
    }
}

//Calculating total of all the amounts at present page
var total = 0;
var taxAmount = 0;
var freight = 0;
var grandTotal = 0;
$("input[name='Total_']").each(function () {
    total = (Number(total) + Number($(this).val())).toFixed(2);
    $('#Total').val(total);
});
$("input[name='TaxAmount']").each(function () {
    taxAmount = (Number(taxAmount) + Number($(this).val())).toFixed(2);
    $('#TotalTaxAmount').val(taxAmount);
});
$("input[name='LineTotal']").each(function () {
    grandTotal = (Number(grandTotal) + Number($(this).val())).toFixed(2);
    $('#GrandTotal').val(grandTotal);
    $('#Freight').val(grandTotal);
});



//functions to calculate page level total against different amounts
function calculateTotalAmount(lessAmount) {
    var totalAmount = $('#Total').val();
    var remainingAmount = totalAmount - lessAmount;
    $('#Total').val(remainingAmount.toFixed(2));
}
function calculateFreightAmount(lessAmount) {
    var freight = $('#Freight').val();
    var remainingAmount = freight - lessAmount;
    $('#Freight').val(remainingAmount.toFixed(2));
}
function calculateTotalSalesTax(lessAmount) {
    var salesTaxAmount = $('#TotalTaxAmount').val();
    var remainingAmount = salesTaxAmount - lessAmount;
    $('#TotalTaxAmount').val(remainingAmount.toFixed(2));
}
function calculateGrandTotal1(lessAmount) {
    var grandAmount = $('#GrandTotal').val();
    var remainingAmount = grandAmount - lessAmount;
    $('#GrandTotal').val(remainingAmount.toFixed(2));
}