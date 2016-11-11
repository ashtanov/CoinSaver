﻿// Write your Javascript code.
window.onload = setPeriod;
var datestart;
var dateend;

function setPeriod() {
    var pStart = document.getElementById("perStart");
    var pEnd = document.getElementById("perEnd");
    if (pStart !== null && pEnd !== null) {
        datestart = pStart.value;
        dateend = pEnd.value;
    }    
}

function getHistoryFor(cat, catName) {

    document.getElementById("currentCat").textContent = catName;
    document.getElementById("historyHeader").classList = cat + '-clr';
    $.post(
        'GetHistoryTable',
        {
            Category: cat,
            StartDate: datestart,
            EndDate: dateend
        }).success(
        function (data) {
            $('#purchaseHistory').html(data);
        });
}
