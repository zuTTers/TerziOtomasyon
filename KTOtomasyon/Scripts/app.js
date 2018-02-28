
(function (window) {

    /* A full compatability script from MDN: */
    var supportPageOffset = window.pageXOffset !== undefined;
    var isCSS1Compat = ((document.compatMode || "") === "CSS1Compat");

    /* Set up some variables  */
    var demoItem2 = document.getElementById("demoItem2");
    var demoItem3 = document.getElementById("demoItem3");
    /* Add an event to the window.onscroll event */
    window.addEventListener("scroll", function (e) {

        /* A full compatability script from MDN for gathering the x and y values of scroll: */
        var x = supportPageOffset ? window.pageXOffset : isCSS1Compat ? document.documentElement.scrollLeft : document.body.scrollLeft;
        var y = supportPageOffset ? window.pageYOffset : isCSS1Compat ? document.documentElement.scrollTop : document.body.scrollTop;

        demoItem2.style.left = -x + 50 + "px";
        demoItem3.style.top = -y + 50 + "px";
    });

})(window);

$(function () {
    var activeIndex = parseInt($('#AccordionIndex').val());

    $("#accordion").accordion({
        collapsible: true,
        active: activeIndex,
        event: "mousedown",
        change: function (event, ui) {
            var index = $(this).children('h3').index(ui.newHeader);
            $('#AccordionIndex').val(index);
        }
    });
});


$("#accordion").accordion({ header: "h3" });


//this will open 1st accordian.
$('#openfirst').click(function () {
    $(".accordion").accordion({ active: 0 });
});

//this will open 2nd accordian.
$('#BtnCustomerEkle').click(function () {
    var index = 0;    
    $(".accordion").accordion({ active: index + 1 });
    $("#AccordionIndex").val(index + 2);   
    
});

//this will open 3rd accordian.
$('#openthird').click(function () {
    $(".accordion").accordion({ active: 2 });
});








$("#BtnUrunSec").on("click", function () {
    $("#txtOId2").val($("#txtOId").val());
    $("#txtPPrice2").val($("#txtPPrice").val());
    $("#txtPQuan2").val($("#txtPQuan").val());
});



//$("#BtnUrunSec").click(function () {
//    $("#txtOId2").val($("#txtOId").val());
//    $("#txtPPrice2").val($("#txtPPrice").val());
//    $("#txtPQuan2").val($("#txtPQuan").val());
//});





