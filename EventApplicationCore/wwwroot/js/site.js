
$(function () {

    //sidebar collapse button
    scrollCollapsebutton();
    $('#collapse-button').css({ 'display': 'block' });

    //implement date picker
    $('.custom-date-picker').datepicker({
        todayBtn: "linked",
        keyboardNavigation: false,
        forceParse: false,
        calendarWeeks: true,
        autoclose: true,
        format: 'dd-M-yyyy',
        todayHighlight: true
    });
    $(".monthpicker").datepicker({
        startView: "months",
        minViewMode: "months",
        autoclose: true,
        forceParse: false,
        format:"M-yy"
    });
    //implement data table
    $('.dataTables-example').DataTable({
        pageLength: 100,
        responsive: true,
        dom: '<"html5buttons"B>lTfgitp',
        order: [0, "desc"],
        buttons: [
            { extend: 'copy' },
            { extend: 'csv' },
            { extend: 'excel', title: 'ExampleFile' },
            { extend: 'pdf', title: 'ExampleFile' },

            {
                extend: 'print',
                customize: function (win) {
                    $(win.document.body).addClass('white-bg');
                    $(win.document.body).css('font-size', '10px');

                    $(win.document.body).find('table')
                        .addClass('compact')
                        .css('font-size', 'inherit');
                }
            }
        ]

    });
});

$(window).scroll(function () {
    scrollCollapsebutton();
});
function scrollCollapsebutton() {
    var winScrollTop = $(window).scrollTop();
    var winHeight = $(window).height();
    var floaterHeight = $('#collapse-button').outerHeight(true);
    var top = Math.max(0, ((winHeight - floaterHeight) / 2) + winScrollTop) + "px"
    $('#collapse-button').css({ 'top': top });
}
function toasterMessage(isError = false, title, message) {
    toastr.options = {
        "closeButton": true,
        "debug": false,
        "progressBar": true,
        "preventDuplicates": false,
        "positionClass": "toast-top-right",
        "onclick": null,
        "showDuration": "400",
        "hideDuration": "1000",
        "timeOut": "7000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut",
        onclick: function () {
            $('#right-sidebar').toggleClass('sidebar-open');
        }
    };
    if (isError)
        toastr.error(message, title);
    else
        toastr.success(message, title);
}
function formatApprovalStatus(status) {
    var returnVal = status; //default
    if (status == "Created" || status=="Booking")
        returnVal = '<span class="label">' + status + '</span>';
    else if (status == "Approved")
        returnVal = '<span class="label label-success">' + status + '</span>';

    return returnVal;
}
function dateFormat(date) {
    const months = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
    var current_datetime = new Date(date);
    var formatted_date = current_datetime.getDate() + "-" + months[current_datetime.getMonth()] + "-" + current_datetime.getFullYear();
    return formatted_date;
}
function generateReport() {
    $.post('/GL/Api/Create',
        $("#report").serialize()).done(function (data) {
            //var reportURL = "@ViewBag.ReportPath"
            //var reportURL = "http://localhost:53525/ReportViewer";
            //reportURL += "?Id=";
           var reportURL = data;
            window.open(reportURL, "_blank");
        });;
}
//function generateVoucher(voucherId) {
//    $.post('/GL/Api/Create',
//        $("#report").serialize()).done(function (data) {
//            //var reportURL = "@ViewBag.ReportPath"
//            //var reportURL = "http://localhost:53525/ReportViewer";
//            //reportURL += "?Id=";
//            reportURL = data;
//            window.open(reportURL, "_blank");
//        });;
//}




function bindSelect2(select2Id, listUrl, urlById, id = 0) {

    $(select2Id).select2({
        placeholder: "Please select",
        ajax: {
            url: listUrl,
            dataType: 'json',
            delay: 250,
            // placeholder: 'Search for an item',
            processResults: function (data, params) {
                return {
                    results: data
                };
            },
            cache: true
        }
    });
    $.ajax({
        type: 'GET',
        placeholder: "Please select",
        url: urlById + id
    }).then(function (data) {
        var option = new Option(data.text, data.id, true, true);
        select2Id.append(option).trigger('change');
        select2Id.trigger({
            type: 'select2:select',
            params: {
                data: data
            }
        });
    });

}