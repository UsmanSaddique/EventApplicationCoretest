var ids = [];
$('.remove-item').click(function () {
    var element = this.id;
    var id = element.replace("remove-item", "");
    //ids to remove
    var itemId = $('#ReceiptItemId' + id).val();
    if (itemId != null) {
        ids.push(itemId);
    }
    var digits = 2;
    var removedTotal = $('#ReceiptAmount_' + id).val();
    var removedIncomeTax = $('#TaxAmount_' + id).val();
    var removedGrandTotal = $('#LineTotal_' + id).val();

    var pageTotal = $('#TotalReceivedAmount').val();
    var pageTax = $('#TotalTaxAmount').val();
    var pageGrand = $('#GrandTotal').val();

    $('#TotalReceivedAmount').val(Number(pageTotal - removedTotal).toFixed(digits));
    $('#TotalTaxAmount').val(Number(pageTax - removedIncomeTax).toFixed(digits));
    $('#GrandTotal').val(Number(pageGrand-removedGrandTotal).toFixed(digits));

    $('#IdsDeleted').text(ids);
    $("#receipt-invoice" + id).remove();
});

var slabVal = 0.00;
function calculateTax(id) {
    //tax slab defined
    var exchange = 0.00;
    var taxId = $('#TaxSlab_' + id).val();//Value of select
    var payment =Number($('#ReceiptAmount_' + id).val());
    var balance =Number($('#Balance_' + id).val());
    var taxAmount = ((slabVal / 100) * payment).toFixed(2);
    var lineAfter = (Number(payment) - Number(taxAmount)).toFixed(2);
    exchange = $("#currencyrate").val();
    if (Number(payment) <= Number(balance))
    {
        debugger;
        var ExRate = $("#CurrencyExchangeRate_"+id).val();

        ExRate = ExRate * lineAfter;
        exchange = lineAfter * exchange;
        $('#ProfitLoss_' + id).val( exchange-ExRate);
        var RemBalance = ((Number(balance)) - (Number(payment) - Number(taxAmount))).toFixed(2);
        $('#LineTotal_' + id).val(exchange.toFixed(2) - (exchange - ExRate));
        $('#RemBalance_' + id).val(RemBalance);
        var tax = $('#TotalTaxAmount').val();
        var newTax = tax - $('#TaxAmount_' + id).val();
        $('#TaxAmount_' + id).val(taxAmount);
        $('#TotalTaxAmount').val((Number(taxAmount) + Number(newTax)).toFixed(2));
        calculateGrandTotal();
    } else {
        alert("Receipt Amount Should be Less than Balance Amount")
        $('#ReceiptAmount_' + id).val("0.00");
    }
  
    //total paid amt
    }

function calculateGrandTotal() {

    //Calculating total of all the amounts at present page
    var total = 0;
    var taxAmount = 0;
    var grandTotal = 0;
    $("input[name='ReceiptAmount']").each(function () {
        total = (Number(total) + Number($(this).val())).toFixed(2);
        $('#TotalReceivedAmount').val(total);
    });
    $("input[name='TaxAmount']").each(function () {
        taxAmount = (Number(taxAmount) + Number($(this).val())).toFixed(2);
        $('#TotalTaxAmount').val(taxAmount);
    });
    $("input[name='LineTotal']").each(function () {
        grandTotal = (Number(grandTotal) + Number($(this).val())).toFixed(2);
        $('#GrandTotal').val(grandTotal);
    });

}
function lockOutInputs(counter) {

    if ($('#InvoiceId' + counter).val() == null) {
        $('.dependent').prop('readonly', 'readonly');
        $('select[id=TaxSlab_' + counter + ']').prop("disabled", "disabled");
    }
    else {
        $('.dependent').removeAttr('readonly', 'readonly');
        $('select[id=TaxSlab_' + counter + ']').removeAttr("disabled", "disabled");
    }
}


// receiptAmount > 0 --validation
function ReceiptForm() {
    var i = true;
    $('input[name="ReceiptAmount"]').each(function () {
        var split = (this.id).split('_');
        var id = split[1];
        var receiptAmount = parseFloat($('#ReceiptAmount_' + id).val());
        if (receiptAmount < 1) {
            alert("'Receipt Amount' can't be Zero (0).");
            i = false;
            return false;
        }
    });
    if (i) $("#FormId").submit();
}