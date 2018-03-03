var orderwithdetail = { orderDetails: [] };

var OperationData;


//Image Dropdown
$(".my-select").chosen({ width: "32px;", height: "32px;" });
$(".my-select2").chosen();

//Modal Ekranı
$('#myModal').on('show.bs.modal', function (e) {
    console.debug('modal shown!');
    $('.my-select', this).chosen({ width: "550px" });
    $('.my-select2', this).chosen({ width: "550px" });
});

$("#drpMUrunListe").chosen().change(drpUrunIslem);
$("#drpMIslemListe").chosen().change(drpIslemGetir);


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
    orderdetail.UnitPrice = MPrice;
    orderdetail.TotalPrice = MTPrice;

    orderwithdetail.orderDetails.push(orderdetail);

    createTable();

});


function drpUrunIslem() {
    $.post("Home/GetOperations", {
        Product_Id: $("#drpMUrunListe").val()
    },
        function (data, status) {
            OperationData = data;

            if (data.length > 0) {
                $("#txtMPrice").val(data[0].UnitPrice + " ₺");
            }
            $('#drpMIslemListe').empty(); //remove all child nodes
            $.each(data, function (idx, obj) {
                $("#drpMIslemListe").append('<option value="' + obj.Operation_Id + '">' + obj.Name + '</option>');
                if ($("#drpMIslemListe").val() == obj.Operation_Id) {
                    $("#txtMPrice").val(obj.UnitPrice + " ₺");
                }
            });         
            
            $('#drpMIslemListe').trigger("chosen:updated");

            
        });
}



function drpIslemGetir() {
    $.each(OperationData, function (idx, obj) {
        if ($("#drpMIslemListe").val() == obj.Operation_Id) {
            $("#txtMPrice").val(obj.UnitPrice + " ₺");
        }
    });
    $('#txtMPrice').trigger("chosen:updated");
    
}

function createTable() {
    var rows = "";
    for (var i = 0; i < orderwithdetail.orderDetails.length; i++) {
        rows += '<tr><td>' + '<a href="#"><span class="fa fa-close"></span></a>' + '</td>';
        rows += '<td>' + orderwithdetail.orderDetails[i].OperationText + '</td>';
        rows += '<td>' + orderwithdetail.orderDetails[i].Quantity + '</td>';
        rows += '<td>' + orderwithdetail.orderDetails[i].UnitPrice + '</td>';
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