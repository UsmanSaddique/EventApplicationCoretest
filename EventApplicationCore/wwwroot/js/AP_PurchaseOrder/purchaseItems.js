var ids = [];
$('.remove-item').click(function () {
    var element = this.id;
    var id = element.replace("remove-item", "");
    //ids to remove
    var itemId = $('#PurchaseItemId' + id).val();
    console.log(itemId);

    if (itemId != null) {
        ids.push(itemId);
    }
    //amounts of netValues removed
    var removedNetValue = $('#Total_' + id).val();
    if (removedNetValue != null) {
        calculateTotalNetValue(removedNetValue);
    }
    //amounts of Discounts removed
    var removedDiscount = $('#DiscountAmount_' + id).val();
    if (removedDiscount != null) {
        calculateTotalDiscountAmount(removedDiscount);
    }
    //amounts of sales taxes removed
    var removedSalesTax = $('#SalesTaxAmount_' + id).val();
    if (removedSalesTax != null) {
        calculateTotalSalesTax(removedSalesTax);
    }
    //amounts of items removed
    var removedAmount = $('#LineTotal_' + id).val();
    if (removedAmount != null) {
        calculateTotalAmount(removedAmount);
    }
    //amounts of Excise Tax removed from the page
    var removedExciseTax = $('#ExciseTaxAmount_' + id).val();
    if (removedExciseTax != null) {
        calculateTotalExciseTax(removedExciseTax);
    }

    $('#IdsDeleted').text(ids);
    $("#purchase-items" + id).remove();

});

function getItemDetails(val) {
    var element = val.replace("ItemId", "");
    var id = $('#' + val).val();//$('#ItemId0')
    $.ajax({
        url: '/AP/Purchase/GetItemDetails?id=' + id,
        type: 'GET'
    }).done(function (data) {
        // console.log(data[0].val);
        // console.log(data[0].uom);
        $('.UOM' + element).val(data[0].id);
        $('#UOM' + element).html(data[0].uom);
    });
    if ($('#ItemId' + element).val() == null) {
        $('.dependent').prop('readonly', 'readonly');
        $('.dependent').html(0);

    }
    else {
        $('.dependent').removeAttr('readonly', 'readonly');
        $('select[id=TaxSlab_' + element + ']').removeAttr("disabled", "disabled");

    }
}
function lockOutInputs(counter) {

    if ($('#ItemId' + counter).val() == null) {
        $('.dependent').prop('readonly', 'readonly');
        $('select[id=TaxSlab_' + counter + ']').prop("disabled", "disabled");
    }
    else {
        $('.dependent').removeAttr('readonly', 'readonly');
        $('select[id=TaxSlab_' + counter + ']').removeAttr("disabled", "disabled");
    }
}

$('.calculate-total').on('keyup', function () {
    var elementId = this.id;
    var id = elementId.split('_');
    id = id[1];

    var qty = $('#Qty_' + id + '').val();
    var rate = $('#Rate_' + id + '').val();
    var netValueBefore = $('#Total_' + id).val();
    $('#Total_' + id).val((qty * rate).toFixed(2));
    var netValueAfter = $('#Total_' + id).val();
    TotalNetValue(netValueBefore, netValueAfter);
    //After selecting tax slab calculating other values
    var discountPercentage = $('#DiscountPercentage_' + id).val();
    var beforeDiscount = $('#DiscountAmount_' + id).val();
    var discountAmount = (discountPercentage / 100) * $('#Total_' + id).val();
    var afterDiscount = discountAmount;
    calculateDiscount(beforeDiscount, afterDiscount);
    if (discountPercentage == 0) { $('#DiscountAmount_' + id).val(0); }
    $('#DiscountAmount_' + id).val(discountAmount.toFixed(2));
    calculateTax(elementId);
});
//calculate totalNetValue
function TotalNetValue(before, after) {
    //    New Net Value
    var totalNetValue = $('#Total').val();
    var remainingNetValue = totalNetValue - before;

    var newNetValue = (Number(remainingNetValue) + Number(after)).toFixed(2);
    $('#Total').val(newNetValue);
}
//Calculate Discount
function calculateDiscount(before, after) {
    var totalDiscount = $('#discountAmount').val();
    var remainingDiscount = totalDiscount - before;

    var newDiscount = (Number(remainingDiscount) + Number(after)).toFixed(2);
    $('#discountAmount').val(newDiscount);
}
//Calculate total Sales Tax
function totalSalesTaxAmount(before, after) {
    var totalSalesTaxAmount = $('#salesTaxAmount').val();
    var remainingSalesTaxAmount = totalSalesTaxAmount - before;

    var newSalesTaxAmount = (Number(remainingSalesTaxAmount) + Number(after)).toFixed(2);
    $('#salesTaxAmount').val(newSalesTaxAmount);
}
//Calculate Excise Amount
function totalExciseAmount(before, after) {
    var totalExciseAmount = $('#exciseTaxAmount').val();
    var remainingExciseAmount = totalExciseAmount - before;

    var newExciseAmount = (Number(remainingExciseAmount) + Number(after)).toFixed(2);
    $('#exciseTaxAmount').val(newExciseAmount);
}
function calculateTax(elementId) {
    var id = elementId.split('_');
    id = id[1];
    //tax slab defined
    var slabVal = $('#TaxSlab_' + id).val();
    if (slabVal == 1) {
        $('#SalesTaxPercentage_' + id).val(15);
        $('#ExciseTaxPercentage_' + id).val(1);
    }
    else if (slabVal == 2) {
        $('#SalesTaxPercentage_' + id).val(16);
        $('#ExciseTaxPercentage_' + id).val(2);
    }
    else if (slabVal == 3) {
        $('#SalesTaxPercentage_' + id).val(17);
        $('#ExciseTaxPercentage_' + id).val(3);
    }
    //calculate sales tax
    var salesTaxPercentage = $('#SalesTaxPercentage_' + id).val();
    var salesTaxBefore = $('#SalesTaxAmount_' + id).val();
    var salesTaxAmount = (salesTaxPercentage / 100) * $('#Total_' + id).val();
    $('#SalesTaxAmount_' + id).val(salesTaxAmount.toFixed(2));
    var salesTaxAfter = $('#SalesTaxAmount_' + id).val();
    totalSalesTaxAmount(salesTaxBefore, salesTaxAfter)
    //calculate excise tax
    var exciseTaxPercentage = $('#ExciseTaxPercentage_' + id).val();
    var beforeExciseAmount = $('#ExciseTaxAmount_' + id).val();
    var exciseTaxAmount = (exciseTaxPercentage / 100) * $('#Total_' + id).val();
    $('#ExciseTaxAmount_' + id).val(exciseTaxAmount.toFixed(2));
    var afterExciseAmount = $('#ExciseTaxAmount_' + id).val();
    totalExciseAmount(beforeExciseAmount, afterExciseAmount);
    //Calculate total remaining amount after taxes and iscount
    var disc = parseFloat($('#DiscountAmount_' + id).val() == null ? 0 : $('#DiscountAmount_' + id).val());
    var saleTax = parseFloat($('#SalesTaxAmount_' + id).val());
    var exciseTax = parseFloat($('#ExciseTaxAmount_' + id).val());
    var lessAmount = parseFloat(disc + saleTax + exciseTax);
    var netValue = parseFloat($('#Total_' + id).val());
    //Cumulative LineTotal
    if (elementId.search('LineTotal')) {
        var totalAmount = $('#GrandTotal').val();
        var currentAmount = $('#LineTotal_' + id).val();
        // console.log($('#LineTotal' + id).val());
        var remaining = totalAmount - currentAmount;
        $('#LineTotal_' + id).val(Number(netValue + lessAmount).toFixed(2));
        var newAmount = Number(remaining) + Number($('#LineTotal_' + id).val());
        $('#GrandTotal').val(newAmount);
    }
}

//Calculating total of all the amounts at present page
var totalAmount = 0;
var netValue = 0;
var discountAmount = 0;
var salesTaxAmount = 0;
var exciseTaxAmount = 0;
$("input[name='LineTotal']").each(function () {
    totalAmount = (Number(totalAmount) + Number($(this).val())).toFixed(2);
    $('#GrandTotal').val(totalAmount);
});
$("input[name='Total_']").each(function () {
    netValue = (Number(netValue) + Number($(this).val())).toFixed(2);
    $('#Total').val(netValue);
});
$("input[name='DiscountAmount']").each(function () {
    discountAmount = (Number(discountAmount) + Number($(this).val())).toFixed(2);
    $('#discountAmount').val(discountAmount);
});
$("input[name='SalesTaxAmount']").each(function () {
    salesTaxAmount = (Number(salesTaxAmount) + Number($(this).val())).toFixed(2);
    $('#salesTaxAmount').val(salesTaxAmount);
});
$("input[name='ExciseTaxAmount']").each(function () {
    exciseTaxAmount = (Number(exciseTaxAmount) + Number($(this).val())).toFixed(2);
    $('#exciseTaxAmount').val(exciseTaxAmount);
});


//functions to calculate page level total against different amounts
function calculateTotalAmount(lessAmount) {
    var totalAmount = $('#GrandTotal').val();
    var remainingAmount = totalAmount - lessAmount;
    $('#GrandTotal').val(remainingAmount);
}
function calculateTotalNetValue(lessAmount) {
    var netValue = $('#Total').val();
    var remainingAmount = netValue - lessAmount;
    $('#Total').val(remainingAmount);
}
function calculateTotalDiscountAmount(lessAmount) {
    var discountAmount = $('#discountAmount').val();
    var remainingAmount = discountAmount - lessAmount;
    $('#discountAmount').val(remainingAmount);
}
function calculateTotalSalesTax(lessAmount) {
    var salesTaxAmount = $('#salesTaxAmount').val();
    var remainingAmount = salesTaxAmount - lessAmount;
    $('#salesTaxAmount').val(remainingAmount);
}
function calculateTotalExciseTax(lessAmount) {
    var salesTaxAmount = $('#exciseTaxAmount').val();
    var remainingAmount = salesTaxAmount - lessAmount;
    $('#exciseTaxAmount').val(remainingAmount);
}
