var uniIds = [];
$('.remove-item').click(function () {
    debugger;
    var element = this.id;
    var id = element.replace("remove-item", "");
    //ids to remove
    var itemId = $('#AcceptanceItemId' + id).val();
    if (itemId != null) {
        ids.push(itemId);
    }
    var digits = 2;
    var ausChk = $("#Aus_" + ItemId).prop("checked");
    var selfChk = $("#Self_" + ItemId).prop("checked");
    var removeFee = $('#FeeAmount_' + id).val();

    if (selfChk == true) {
        var removedSelfTotal = $('#Amount_' + id).val();
        var pageGrand = $('#SelfTotal').val();
        var totalFee = $('#SelfFeeTotal').val();
        if (pageGrand >= removedSelfTotal) {
            $('#SelfTotal').val(Number(pageGrand - removedSelfTotal).toFixed(digits));
            $('#SelfFeeTotal').val(Number(totalFee - removeFee).toFixed(digits));
        }
    }
    if (ausChk == true) {
        var removedAusPakTotal = $('#Amount_' + id).val();
        var pageGrand = $('#AusPakTotal').val();
        var totalFee = $('#AusFeeTotal').val();
        if (pageGrand >= removedAusPakTotal) {
            $('#AusPakTotal').val(Number(pageGrand - removedAusPakTotal).toFixed(digits));
            $('#AusFeeTotal').val(Number(totalFee - removeFee).toFixed(digits));
        }
    }
    $('#IdsDeleted').val(id);
    $("#university-Detail" + id).remove();
});

function getVisaStudent() {
    var value = $('#StudentId').val();
    debugger;
    $.ajax({
        type: 'GET',
        async: false,
        url: '/AusPak/UniversityOffer/GetStudent?id=' + value,
        contentType: 'JSON'
    }).done(function (data) {
        $('#tblStudents tbody tr').remove();
       
        var item = jQuery.parseJSON(JSON.stringify(data));
        debugger;
        var Gender = '';
        if (item.genderId === 44) {
            Gender = "Male";
        }
        else if (item.genderId === 43) {
            Gender = "Female";
        }
        else if (item.genderId === 29) {
            Gender = "TransGender";

        }
        var rows = '<tr>' +
            '<td>' + item.firstName + " " + item.lastName + '</td>' +
            '<td>' + moment(item.dob).format('DD-MMM-YYYY') + '</td>' +
            '<td>' + item.cnic + '</td>' +
            '<td>' + Gender + '</td>' +
            '<td>' + item.mobile + '</td>' +
            '<td>' + item.email + '</td>' +
            '</tr>';
        $('#tblStudents tbody').append(rows);
        getBranchById(data.branchId);
        getStudentQualification(value);
        getStudentDiploma(value);
        getStudentWork(value);
        getFinalUniversity(value);
        getUniversitySelectionOfferAcceptance(value);
        
    });
}
function getBranchById(branchid) {
    //var value = $('#StudentId').val();
    $.ajax({
        type: 'GET',
        async: false,
        url: '/AusPak/ApplicationForm/GetBranch?id=' + branchid,
        contentType: 'JSON'
    }).done(function (data) {
        var item = jQuery.parseJSON(JSON.stringify(data));
        debugger;
        $('#BranchName').val(item);
        // var item = jQuery.parseJSON(JSON.stringify(data));

    });
}
function getStudentQualification(value) {
    //var value = $('#StudentId').val();
    $.ajax({
        type: 'GET',
        async: false,
        url: '/AusPak/ApplicationForm/GetStudentQual?id=' + value,
        contentType: 'JSON'
    }).done(function (data) {


        $('#tblQualifications tbody tr').remove();
        var item = jQuery.parseJSON(JSON.stringify(data));


        // var item= JSON.parse(data);
        var Gender = '';

        var rows = "";
        var level = "";
        $.each(item, function (key, value) {

            if (item[key].levelId === 1) {
                level = "Matric";
            }
            else if (item[key].levelId === 2) {
                level = "Intermediate";
            }
            else if (item[key].levelId === 3) {
                level = "Bachelor";
            }
            else if (item[key].levelId === 4) {
                level = "O/A Level";
            }
            else if (item[key].levelId === 5) {
                level = "Master";
            }
            else {
                level = "N/A";
            }

            rows += '<tr>' +
                //'<td>' + item[key].levelId + '</td>' +
                '<td>' + level + '</td>' +
                '<td>' + item[key].program + '</td>' +
                '<td>' + item[key].institute + '</td>' +
                '<td>' + item[key].startDate + '</td>' +
                '<td>' + item[key].endDate + '</td>' +
                '<td>' + item[key].gpa + '</td>' +
                '<td>' + item[key].comments + '</td>' +
                '</tr>';

        });

        $('#tblQualifications tbody').append(rows);

    });
}

function getStudentDiploma(value) {
    //var value = $('#StudentId').val();
    $.ajax({
        type: 'GET',
        async: false,
        url: '/AusPak/ApplicationForm/GetStudentDip?id=' + value,
        contentType: 'JSON'
    }).done(function (data) {

        $('#tblDiplomas tbody tr').remove();
        var item = jQuery.parseJSON(JSON.stringify(data));
        //JSON.parse(data);
        var Gender = '';

        var rows = "";
        $.each(item, function (key, value) {

            rows += '<tr>' +
                '<td>' + item[key].diplomaName + '</td>' +
                '<td>' + moment(item[key].diplomaDate).format('DD-MMM-YYYY') + '</td>' +
                '<td>' + item[key].bandScore + '</td>' +
                '<td>' + item[key].individualScore + '</td>' +
                '<td>' + item[key].comments + '</td>' +
                '</tr>';
        });
        $('#tblDiplomas tbody').append(rows);

    });
}

function getStudentWork(value) {
    //   var value = $('#StudentId').val();
    $.ajax({
        type: 'GET',
        async: false,
        url: '/AusPak/ApplicationForm/GetStudentWork?id=' + value,
        contentType: 'JSON'
    }).done(function (data) {
        $('#tblWorks tbody tr').remove();
        var item = jQuery.parseJSON(JSON.stringify(data));
        //JSON.parse(data);
        var Gender = '';
        var rows = "";
        $.each(item, function (key, value) {

            rows += '<tr>' +
                '<td>' + item[key].organization + '</td>' +
                '<td>' + item[key].designation + '</td>' +
                '<td>' + moment(item[key].fromDate).format('DD-MMM-YYYY') + '</td>' +
                '<td>' + moment(item[key].toDate).format('DD-MMM-YYYY') + '</td>' +
                '<td>' + item[key].comments + '</td>' +
                '</tr>';
        });
        $('#tblWorks tbody').append(rows);

    });
}
 
function getFinalUniversity(value) {
    //var value = $('#StudentId').val();
    $.ajax({
        type: 'GET',
        async: false,
        url: '/AusPak/Visa/GetUniversity?id=' + value,
        contentType: 'JSON'
    }).done(function (data) {
         
        var final = data;
        //$('#UniversityChange').val(final);
        $('#ttt').val(final);
        //var item = jQuery.parseJSON(JSON.stringify(data));

    });
}

function getUniversitySelectionOfferAcceptance(value) {
    debugger;
    $.ajax({
        type: 'GET',
        async: false,
        url: '/AusPak/OfferAcceptance/GetUniversitySelection?id=' + value,
        contentType: 'JSON'
    }).done(function (data) {
        $('#tblUniversitySelection tbody tr').remove();
        var item = jQuery.parseJSON(JSON.stringify(data));

        var rows = "";
        $.each(item, function (key, value) {
            var checked = '';
            if (value.isFinal == true) {
                checked = 'checked';
            }
            rows += '<tr>' +
                '<td>' + value.countryName + '</td>' +
                '<td>' + value.universityName + '</td>' +
                '<td>' + value.program + '</td>' +
                '<td>' + value.session + '</td>' +
                '<td>' + value.applicationFee + '</td>' +
                '<td>' + value.ausPakFee + '</td>' +
                '<td style="display:none"><input type=hidden id="UniversityId' + key + '" name="UniversityId" value="' + value.universityId + '" /></td>' +
                //'<td><input type="checkbox" id="checkbox" name="checkbox" ' + checked + ' onclick="abc(this)"/ > Final</td>' +
                '</tr>';
        });
        $('#tblUniversitySelection tbody').append(rows);

    });
}


function GetFeeByStudentIds() {
     
    var studentId = $('#StudentId').val();
    debugger;
    $.ajax({
        type: 'GET',
        async: false,
        url: '/AusPak/Visa/GetVisaHistory?id=' + studentId,
        contentType: 'JSON'
    }).done(function (data) {
        $('#itemTable tbody tr').remove();
        var item = jQuery.parseJSON(JSON.stringify(data));
        debugger;
         
        var rows = "";
        $.each(item, function (key, value) {
            rows += '<tr>' +
                '<td>' + value.status + '</td>' +
                '<td>' + moment(value.visaDate).format('DD-MMM-YYYY') + '</td>' +
                '<td>' + value.caseOfficer + '</td>' +
                '<td>' + value.actionTaken + '</td>' +
                '<td>' + value.remarks + '</td>' +
                '</tr>';
        });
         
        $('#itemTable tbody').append(rows);

        });
    GetOfferHistory();
    GetAcceptanceHistory();
}

function GetOfferHistory() {
     
    var studentId = $('#StudentId').val();
    debugger;
    $.ajax({
        type: 'GET',
        async: false,
       // url: '/AusPak/UniversityOffer/GetFeeByStudentId2?id=' + studentId,
        url: '/AusPak/Visa/GetOfferHistory?id=' + studentId,
        contentType: 'JSON'
    }).done(function (data) {
        $('#offerTable tbody tr').remove();
        var item = jQuery.parseJSON(JSON.stringify(data));
         
        var rows = "";
        $.each(item, function (key, value) {
            rows += '<tr>' +
                '<td>' + value.updatedBy + '</td>' +
                '<td>' + value.status + '</td>' +
                '<td>' + moment(value.visaDate).format('DD-MMM-YYYY') + '</td>' +
                //'<td>' + value.remarks + '</td>' +
                '</tr>';
        });
         
        $('#offerTable tbody').append(rows);

    });
}

function GetAcceptanceHistory() {
     
    var studentId = $('#StudentId').val();
     
    $.ajax({
        type: 'GET',
        async: false,
        //url: '/AusPak/OfferAcceptance/GetFeeByStudentId2?id=' + studentId,
        url: '/AusPak/Visa/GetAcceptanceHistory?id=' + studentId,
        contentType: 'JSON'
    }).done(function (data) {
        $('#acceptanceTable tbody tr').remove();
        var item = jQuery.parseJSON(JSON.stringify(data));
        debugger;
        var rows = "";
        $.each(item, function (key, value) {
            rows += '<tr>' +
                '<td>' + value.updatedBy + '</td>' +
                '<td>' + value.status + '</td>' +
                '<td>' + moment(value.visaDate).format('DD-MMM-YYYY') + '</td>' +
                '<td>' + value.remarks + '</td>' +
                '</tr>';
        });
         
        $('#acceptanceTable tbody').append(rows);

    });
}

function getVisa() {
    var value = $('#visaId').val();
    if (value == "Calling Visa") {
        var origin = window.origin;
        // window.location.href = "/AusPak/UniversityOffer/PendingOffer/";
        window.location.href = "/AusPak/Visa/PendingVisa/";


    }
    else if (value == "Visa Form") {

        //window.location.href = "/AusPak/UniversityOffer/";
        window.location.href = "/AusPak/Visa/";
    }


}