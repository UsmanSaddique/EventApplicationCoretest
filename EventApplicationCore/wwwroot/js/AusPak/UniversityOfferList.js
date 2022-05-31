
function getOffer() {
    var value = $('#offerId').val();
    if (value == "Offer Awaited") {
        var origin = window.origin;
       // window.location.href = "/AusPak/UniversityOffer/PendingOffer/";
        window.location.href = "/AusPak/UniversityOffer/PendingOffer/";
         

    }
    else if (value == "University Offer") {
       
        //window.location.href = "/AusPak/UniversityOffer/";
        window.location.href = "/AusPak/UniversityOffer/";
    }


}