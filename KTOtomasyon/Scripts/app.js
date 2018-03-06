
function jsDate(x) {
    var jsonDateRE = /^\/Date\((-?\d )(\ |-)?(\d )?\)\/$/;
    var arr = jsonDateRE.exec(x);
    if (arr) {
        // 0 - complete results; 1 - ticks; 2 - sign; 3 - minutes
        return new Date(parseInt(arr[1]));
    }
    return x;
}


function formatDate(formattedDate) {
    try {
        formattedDate = new Date(formattedDate);
        var d = formattedDate.getDate();
        var m = formattedDate.getMonth();
        m = 1;  // JavaScript months are 0-11
        m = ("00" + m).substr(("00" + m).length - 2);
        d = ("00" + d).substr(("00" + d).length - 2);
        var y = formattedDate.getFullYear();
        return (d + "-" + m + "-" + y);
    } catch (e) {
        return formattedDate;
    }

}
function formatDateTime(formattedDate) {
    try {
        var d = formattedDate.getDate();
        var m = formattedDate.getMonth();
        var h = formattedDate.getHours();
        var mm = formattedDate.getMinutes();
        m = 1;  // JavaScript months are 0-11
        m = ("00" + m).substr(("00" + m).length - 2);
        d = ("00" + d).substr(("00" + d).length - 2);
        h = ("00" + h).substr(("00" + h).length - 2);
        mm = ("00" + mm).substr(("00" + mm).length - 2);
        var y = formattedDate.getFullYear();
        return (d + "-" + m + "-" + y + " " + h + ":" + mm);
    } catch (e) {
        return formattedDate;
    }

}
function formatTime(formattedDate) {
    try {
        var h = formattedDate.getHours();
        var mm = formattedDate.getMinutes();
        h = ("00" + h).substr(("00" + h).length - 2);
        mm = ("00" + mm).substr(("00" + mm).length - 2);
        return (h + ":" + mm);
    } catch (e) {
        return formattedDate;
    }

}
















/*BURADAN AŞAĞISI BOŞ*/

//(function (window) {

//    /* A full compatability script from MDN: */
//    var supportPageOffset = window.pageXOffset !== undefined;
//    var isCSS1Compat = ((document.compatMode || "") === "CSS1Compat");

//    /* Set up some variables  */
//    var demoItem2 = document.getElementById("demoItem2");
//    var demoItem3 = document.getElementById("demoItem3");
//    /* Add an event to the window.onscroll event */
//    window.addEventListener("scroll", function (e) {

//        /* A full compatability script from MDN for gathering the x and y values of scroll: */
//        var x = supportPageOffset ? window.pageXOffset : isCSS1Compat ? document.documentElement.scrollLeft : document.body.scrollLeft;
//        var y = supportPageOffset ? window.pageYOffset : isCSS1Compat ? document.documentElement.scrollTop : document.body.scrollTop;

//        demoItem2.style.left = -x + 50 + "px";
//        demoItem3.style.top = -y + 50 + "px";
//    });

//})(window);

//$(function () {
//    var activeIndex = parseInt($('#AccordionIndex').val());

//    $("#accordion").accordion({
//        collapsible: true,
//        active: activeIndex,
//        event: "mousedown",
//        change: function (event, ui) {
//            var index = $(this).children('h3').index(ui.newHeader);
//            $('#AccordionIndex').val(index);
//        }
//    });
//});


//$("#accordion").accordion({ header: "h3" });

////this will open 1st accordian.
//$('#openfirst').click(function () {
//    $(".accordion").accordion({ active: 0 });
//});

////this will open 2nd accordian.
//$('#BtnCustomerEkle').click(function () {
//    var index = 0;    
//    $(".accordion").accordion({ active: index + 1 });
//    $("#AccordionIndex").val(index + 2);   

//});

////this will open 3rd accordian.
//$('#openthird').click(function () {
//    $(".accordion").accordion({ active: 2 });
//});

//$("#BtnUrunSec").on("click", function () {
//    $("#txtOId2").val($("#txtOId").val());
//    $("#txtPPrice2").val($("#txtPPrice").val());
//    $("#txtPQuan2").val($("#txtPQuan").val());
//});


//$("#BtnUrunSec").click(function () {
//    $("#txtOId2").val($("#txtOId").val());
//    $("#txtPPrice2").val($("#txtPPrice").val());
//    $("#txtPQuan2").val($("#txtPQuan").val());
//});





