var uniIds = [];
var status = false;
 
function getStudent() {
    debugger;
    var value = $('#StudentId').val();
    $.ajax({
        type: 'GET',
        async: false,
        url: '/AusPak/UniversityOffer/GetStudent?id=' + value,
        contentType: 'JSON'
    }).done(function (data) {
        $('#tblStudents tbody tr').remove();
        var item = jQuery.parseJSON(JSON.stringify(data));
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
            '<td>' + Gender + '</td>' +
            '<td>' + item.mobile + '</td>' +
            '<td>' + item.email + '</td>' +
            '</tr>';
        $('#tblStudents tbody').append(rows);
        getUniversitySelection(value);
           // GetOfferStatus();
        getStudentQualification(value);
        getStudentDiploma(value);
        getStudentWork(value);
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

        debugger;
        $('#tblQualifications tbody tr').remove();
        var item = jQuery.parseJSON(JSON.stringify(data));

        debugger;
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
        debugger;
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

function checkStudent() {
    debugger;
    var value = $('#StudentId').val();
    //if (status == "false") {
        $.ajax({
            type: 'GET',
            async: false,
            url: '/AusPak/UniversityOffer/CheckStudent?id=' + value,
            contentType: 'JSON'
        }).done(function (data) {
            var item = jQuery.parseJSON(JSON.stringify(data));
            debugger;
            if (item != 0) {
                var offerId = item;

                // window.location.href = "http://110.37.207.234/AusPak/UniversityOffer/Create/" + offerId + "";
               window.location.href = "/AusPak/UniversityOffer/Create/" + offerId + "";
                
            }
            else {
                debugger;
                getStudent();
            }
        });
}

function getStudent2() {
//    var value = $('#StudentId').val();
  

     
  
    var stdId = $('#StudentId').val();
    var UniId = $('#UniversityId').val();
    $.ajax({
        type: 'GET',
        async: false,
        url: '/AusPak/UniversityOffer/GetStudent2?id=' + UniId + ' & stdId=' + stdId,
        

        contentType: 'JSON'
    }).done(function (data) {


         
        $('#tblStudents tbody tr').remove();
        var item = jQuery.parseJSON(JSON.stringify(data));
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
            '<td>' + item.firstName + '</td>' +
            '<td>' + moment(item.dob).format('DD-MMM-YYYY') + '</td>' +
            '<td>' + Gender + '</td>' +
            '<td>' + item.mobile + '</td>' +
            '<td>' + item.email + '</td>' +
            '</tr>';
        $('#tblStudents tbody').append(rows);
        getUniversitySelection(value);

        
       
    });
}





function abc(row) {
     
    $('#tblUniversitySelection tbody tr').each(function (key, value) {

         

       
      //  var isTrue =  $(this).find('tr').find('input[type="checkbox"]').prop("checked");
        var isTrue = $(value).find('input[type="checkbox"]').prop("checked");
        if (isTrue == true) {
            $(value).find('input[type="checkbox"]').prop("checked", true)

            var unidiii = $(value).find('input[name="UniversityIds"]').val();


            $('#UniversityId').val(unidiii);
           
            //$('input[type="checkbox"]').not(this).prop('checked', true);

        }
        uniIds.push(unidiii);
        tests.push(unidiii);

       
        //if (isTrue == true)
        //{
        //    //$('input[type="checkbox"]').not(this).prop('checked', true);
        //    $(value).find('input[type="checkbox"]').prop("checked", false)
        //}
        //test3();
        
    });
   
}



function getUniversitySelection(value) {
    debugger;
    $.ajax({
        type: 'GET',
        async: false,
        url: '/AusPak/UniversityOffer/GetUniversitySelection?id=' + value,
        contentType: 'JSON'
    }).done(function (data) {
        $('#tblUniversitySelection tbody tr').remove();
        var item = jQuery.parseJSON(JSON.stringify(data));

        var selectedOptions = item;

        debugger;
        var rows = "";
        $.each(item, function (key, value) {
            debugger;
            var checked ='';
            if (value.isFinal == true)
            {
                checked = 'checked';
            }
            var id = window.location.href.split('/')[6];
            if (parseInt(id) > 0) {
                rows += '<tr>' +
                    '<td>' + value.countryName + '</td>' +
                    '<td>' + value.universityName + '</td>' +
                    '<td>' + value.program + '</td>' +
                    '<td>' + value.session + '</td>' +
                    '<td>' + value.applicationFee + '</td>' +
                    '<td>' + value.ausPakFee + '</td>' +
                    '<td><select asp-for="newStatus" id="newStatus'+(key + 1)+'" name="newStatus"  value="' + value.status + '" class ="form-control" asp-items=" "></select></td>' +


//                    '<td><select asp-for="newStatus" name="newStatus"  value="' + value.status + '" class ="form-control" asp-items=" "></select></td>' +
                    //  '<td><select name="StatusId"  asp-for="StatusId" data-validation="required" data-validation-error-msg="Status is required" data-validation-error-msg-container="#statusId" asp-items="@ViewBag.Status">< option selected = "selected"  > Select...</option ></select ><p id="statusId"></p></td>' +

                    //'<td><input type="Text" id="newStatus" name="newStatus" value="' + value.status + '" /></td>' +
                    '<td style="display:none"><input type=hidden id="UniversityId' + key + '" name="UniversityIds" value="' + value.universityId + '" /></td>' +
                    //'<td><input type="checkbox" id="checkbox" name="chk' + key + '" onclick="abc()" >Final</td>' +
                    '<td><input type="checkbox" id="checkbox" name="checkbox" ' + checked + ' onclick="abc(this)"/ ></td>' +
                    //'<td><input type="checkbox" id="checkbox" name="checkbox" onclick="abc(this)" >Final</td>' +


                    '</tr>';
               
            }
            else if (id != "")
            {
                rows += '<tr>' +
                    '<td>' + value.countryName + '</td>' +
                    '<td>' + value.universityName + '</td>' +
                    '<td>' + value.program + '</td>' +
                    '<td>' + value.session + '</td>' +
                    '<td>' + value.applicationFee + '</td>' +
                    '<td>' + value.ausPakFee + '</td>' +
                    '<td><select asp-for="newStatus" id="newStatus' + (key + 1) + '" name="newStatus"  value="' + value.status + '" class ="form-control" asp-items=" "></select></td>' +
                    //                    '<td><select asp-for="newStatus" name="newStatus"  value="' + value.status + '" class ="form-control" asp-items=" "></select></td>' +
                    //  '<td><select name="StatusId"  asp-for="StatusId" data-validation="required" data-validation-error-msg="Status is required" data-validation-error-msg-container="#statusId" asp-items="@ViewBag.Status">< option selected = "selected"  > Select...</option ></select ><p id="statusId"></p></td>' +

                    //'<td><input type="Text" id="newStatus" name="newStatus" value="' + value.status + '" /></td>' +
                    '<td style="display:none"><input type=hidden id="UniversityId' + key + '" name="UniversityIds" value="' + value.universityId + '" /></td>' +
                    //'<td><input type="checkbox" id="checkbox" name="chk' + key + '" onclick="abc()" >Final</td>' +
                    '<td><input type="checkbox" id="checkbox" name="checkbox" ' + checked + ' onclick="abc(this)"/ ></td>' +
                    //'<td><input type="checkbox" id="checkbox" name="checkbox" onclick="abc(this)" >Final</td>' +
                    '</tr>';
            }
            else {
                rows += '<tr>' +
                    '<td>' + value.countryName + '</td>' +
                    '<td>' + value.universityName + '</td>' +
                    '<td>' + value.program + '</td>' +
                    '<td>' + value.session + '</td>' +
                    '<td>' + value.applicationFee + '</td>' +
                    '<td>' + value.ausPakFee + '</td>' +
                    '<td><select asp-for="newStatus" name="newStatus"  id ="newStatus' + key + '" class ="form-control" asp-items=" "></select></td>' +
                    //  '<td><select name="StatusId"  asp-for="StatusId" data-validation="required" data-validation-error-msg="Status is required" data-validation-error-msg-container="#statusId" asp-items="@ViewBag.Status">< option selected = "selected"  > Select...</option ></select ><p id="statusId"></p></td>' +

                    //'<td><input type="Text" id="newStatus" name="newStatus" value="' + value.status + '" /></td>' +
                    '<td style="display:none"><input type=hidden id="UniversityId' + key + '" name="UniversityIds" value="' + value.universityId + '" /></td>' +
                    //'<td><input type="checkbox" id="checkbox" name="chk' + key + '" onclick="abc()" >Final</td>' +
                    '<td><input type="checkbox" id="checkbox" name="checkbox" ' + checked + ' onclick="abc(this)"/ ></td>' +
                    //'<td><input type="checkbox" id="checkbox" name="checkbox" onclick="abc(this)" >Final</td>' +
                    '</tr>';
            }
        });
        $('#tblUniversitySelection tbody').append(rows);      
        GetOfferStatus();
        debugger;
        setTimeout(function () {
            $.each(selectedOptions, function (key, value) {
                debugger;
//                $('#newStatus' + (key + 1)).html(value.status);

  //              $("#ddlDepartment" + (key + 1)).val(value.DepartmentId);

                $("#newStatus" + (key + 1)).val(value.status).trigger('change');

            });
        }, 2000)
 
    });
}


function GetOfferStatus(value) {
    debugger;
    $.ajax({
        type: 'GET',
        async: false,
        url: '/AusPak/UniversityOffer/GetStatus',
        contentType: 'JSON'
    }).done(function (data) {
        debugger;
        var option = '<option value="0">Please Select</option>';
        var item = jQuery.parseJSON(JSON.stringify(data));

        $(item).each(function (key, value) {
            debugger;
            option += '<option value="' + value.value + '">' + value.text+'</option>'
          //  $('#finalStatus' + key + '').val(value.id);

        });
        $('select[name="newStatus"]').html(option);
        });

}

function GetFeeByStudentId2() {
     
    var studentId = $('#StudentId').val();
     
    $.ajax({
        type: 'GET',
        async: false,
        url: '/AusPak/UniversityOffer/GetFeeByStudentId2?id=' + studentId,
        contentType: 'JSON'
    }).done(function (data) {
        $('#itemTable tbody tr').remove();
        var item = jQuery.parseJSON(JSON.stringify(data));      
        var rows = "";
        $.each(item, function (key, value) {
            debugger;
            rows += '<tr>' +
                '<td>' + value.updatedBy + '</td>' +
                '<td>' + moment(value.offerDate).format('DD-MMM-YYYY') + '</td>' +
                '<td>' + value.status + '</td>' +
              
                '</tr>';
            $('#studentName').val(value.createdBy);
        });       
        $('#itemTable tbody').append(rows);
        

    });
}
var tests = [];
function test3() {
   
    var formData = {
        id: tests,
        stdId: 12
    }
    $.ajax({
        type: 'POST',
        async: false,
        url: '/AusPak/UniversityOffer/GetAcceptanceSelection',
        dataType: 'JSON',
        traditional: true,
        data: formData
    }).done(function (data) {
        if (data == true) {
            alert("University final done successfully");
            location.reload();
        }
        else {
            alert("University not found");

        }
    });
}




     







