 
function getStudent() {
    var value = $('#StudentId').val();
    $.ajax({
        type: 'GET',
        async: false,
        url: '/AusPak/ApplicationForm/GetStudent?id=' + value,
        contentType: 'JSON'
    }).done(function (data) {
         
        $('#tblStudents tbody tr').remove();
        var item = jQuery.parseJSON(JSON.stringify(data));
        //JSON.parse(data);
        var Gender = '';
        if (item.genderId === 44)
        {
            Gender = "Male";
        }
        else if (item.genderId === 43)
        {
            Gender = "Female";
        }
        else if (item.genderId === 29)
        {
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

 
         
       // $('#BranchName').val(data.createdBy);
        
        getBranchById(data.branchId);
 
        $('#CNIC').val(data.cnic);

        getStudentQualification(value );
        getStudentDiploma(value );
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
            else
            {
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


function getStudentById(Id) {
    //var value = $('#StudentId').val();
    $.ajax({
        type: 'GET',
        async: false,
        url: '/AusPak/ApplicationForm/GetStudent?id=' + Id,
        contentType: 'JSON'
    }).done(function (data) {
         
        $('#tblStudents tbody tr').remove();
        var item = jQuery.parseJSON(JSON.stringify(data));
        //JSON.parse(data);
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
        debugger;
        getBranchById(item.branchId);
        $('#CNIC').val(item.cnic);
        getStudentQualification(Id);
        getStudentDiploma(Id);
        getStudentWork(Id);

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

