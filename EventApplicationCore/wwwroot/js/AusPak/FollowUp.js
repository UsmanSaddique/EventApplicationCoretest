function getFollowStudent()
{
    debugger;
    var value = $('#StudentId').val();    
    $.ajax({
        type: 'GET',
        async: false,
        url: '/AusPak/Follow/GetStudent?id=' + value,
        contentType: 'JSON'
    }).done(function (data) {      
        $('#tblStudents tbody tr').remove();
        var item = jQuery.parseJSON(JSON.stringify(data));
            var rows = '<tr>' +
                '<td>' + item.studentNo + '</td>' +
                '<td>' + item.firstName + '</td>' +
                '<td>' + item.program + '</td>' +
                '<td>' + item.work + '</td>' +
                '<td>' + item.diploma + '</td>' +
                '</tr>';
             
            $('#tblStudents tbody').append(rows);

        $.ajax({
            type: 'GET',
            async: false,
            url: '/AusPak/Follow/GetFollowUpHistory?id=' + value,
            contentType: 'JSON'
        }).done(function (data) {
             
              //$('#history').remove();
            //rows.remove();
            document.getElementById("history").innerHTML = "";
            var item = jQuery.parseJSON(JSON.stringify(data));
            //JSON.parse(data);

            for (var i = 0; i < item.length; i++)
            {
                 
                var Followup = '';
                //var newType = item[i].followUpType;

                //if (newType === 36)
                //{
                //     
                //    Followup = 'First FollowUp'; 
                //}
                //else if (newType === 35) {
                //    Followup = 'Second FollowUp'; 
                //}
                //else if (newType === 34) {
                //    Followup = 'Third FollowUp';
                //}
                //else {
                //    Followup = 'Not Found';

                //}
                
                var rows = '<div class="to_sho_w_da_te_and_tim_e">' +
                    '<h4>' +
                    '<i class="fa fa-clock-o" aria-hidden="true"></i>' +
                    item[i].nextFDate +
                    '</h4>' +
                    '<h5>' +
                    item[i].followUpTime +
                    '</h5>' +
                    '<h3>' +
                    '<i class="fa fa-envelope-open to_sty_le" aria - hidden="true" ></i>' +
                    '</h3>' +
                    '<div class="to_assi_gn_wid_th" >' +
                    '<h6>' +
                    '<i class="fa fa-user" aria-hidden="true" ></i>' +
                    item[i].followType +
                    '</h6> ' +
                    '</div >' +
                    '<div class="to_assi_gn_wid_th">' +
                    '<h6>' +
                    '<i class="fa fa-clock-o" aria-hidden="true"></i>' +
                    item[i].description +
                    '</h6> ' +
                    '</div>' +

                    '</div>';
                $('#history').append(rows);

            }

        });
    });

}

function checkStudent() {
     
    var value = $('#StudentId').val();
    $.ajax({
        type: 'GET',
        async: false,
        url: '/AusPak/Follow/CheckStudent?id=' + value,
        contentType: 'JSON'
    }).done(function (data) {
        var item = jQuery.parseJSON(JSON.stringify(data));
         
        if (item != 0) {
            var followId = item;

            window.location.href = "/AusPak/Follow/Create/" + followId + "";
        }
        else {
             
            getFollowStudent();
        }
    });
 
}