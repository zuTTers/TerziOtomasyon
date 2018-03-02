var orderwithdetail = { orderDetails: [] };



$("#btnUrunEkle").click(function () {
    var orderdetail = {};


    var MUrun = $("#drpMUrunListe").val();
    var MIslem = $("#drpMIslemListe").val();
    var MIslemText = $("#drpMIslemListe option:selected").text();
    var MAdet = $("#txtMAdet").val();
    var MPrice = $("#txtMPrice").val();
    var MTPrice = $("#txtMTotalPrice").val();

    orderdetail.OrderDetail_Id = 0;
    orderdetail.Operation_Id = MIslem;
    orderdetail.OperationText = MIslemText;
    orderdetail.Quantity = MAdet;
    orderdetail.Price = MPrice;
    orderdetail.TotalPrice = MTPrice;

    orderwithdetail.orderDetails.push(orderdetail);

    createTable();

});

var OperationData;

function drpUrunIslem() {

    $.post("Home/GetOperations", {
        Product_Id: $("#drpMUrunListe").val()
    },
        function (data, status) {
            OperationData = data;
            $("#drpMIslemListe").html('');
            if (data.length > 0) {
                $("#txtMPrice").val(data[0].UnitPrice + " TL");
            }
            $.each(data, function (idx, obj) {
                $("#drpMIslemListe").append('<option value="' + obj.OperationText + '">' + obj.Name + '</option>');

            });

        });
}


function drpIslem() {

    $.each(OperationData, function (idx, obj) {
        if ($("#drpMIslemListe").val() == obj.Operation_Id) {
            $("#txtMPrice").val(obj.UnitPrice + " TL");
        }

    });

}


function createTable() {

    var rows = "";

    for (var i = 0; i < orderwithdetail.orderDetails.length; i++) {
        rows += '<tr><td>' + orderwithdetail.orderDetails[i].OrderDetail_Id + '</td>';
        rows += '<td>' + orderwithdetail.orderDetails[i].OperationText + '</td>';
        rows += '<td>' + orderwithdetail.orderDetails[i].Quantity + '</td>';
        rows += '<td>' + orderwithdetail.orderDetails[i].Price + '</td>';
    }
    var divTable = document.getElementById("tabloveri");
    divTable.innerHTML = rows;

}













$("#btnSiparisKaydet").click(function () {
    var MAdi = $("#txtMAdi").val();
    var MTelefon = $("#txtMTelefon").val();
    var MSTarihi = $("#txtMSTarihi").val();
    var MAciklama = $("#txtMAciklama").val();

    orderwithdetail.CustomerName = MAdi;
    orderwithdetail.PhoneNumber = MTelefon;
    orderwithdetail.Description = MAciklama;
    orderwithdetail.OrderDate = MSTarihi;
    orderwithdetail.CreatedUser = 1;
    orderwithdetail.CreatedUserText = "";
    orderwithdetail.IsDelivered = false;
    orderwithdetail.IsDeleted = false;


});