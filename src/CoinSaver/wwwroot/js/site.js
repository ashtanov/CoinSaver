// Write your Javascript code.
window.onload = setPeriod;
var datestart;
var dateend;

$(document).ready(function () {
    $("#tabHeaders a").click(function (e) {
        e.preventDefault();
        $(this).tab('show');
    });
});

function setPeriod() {
    var pStart = document.getElementById("perStart");
    var pEnd = document.getElementById("perEnd");
    if (pStart !== null && pEnd !== null) {
        datestart = pStart.value;
        dateend = pEnd.value;
    }
}

function getHistoryFor(cat, catName) {
    $.post(
        'GetHistoryTable',
        {
            Category: cat,
            StartDate: datestart,
            EndDate: dateend
        }).success(
        function (data) {
            $('#purchaseHistory').html(data);
            document.getElementById("currentCat").textContent = catName;
            document.getElementById("historyHeader").classList = cat + '-clr';
            location.hash = "#history";
        });
}

function activaTab(tab) {
    $('.nav-tabs a[href="#' + tab + '"]').tab('show');
}
