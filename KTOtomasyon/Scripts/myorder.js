var orderwithdetail = { OrderDetails: [] };
var OperationData;
var OrderData;

//Image Chosen Dropdown
$(".my-select").chosen({ width: "32px;", height: "32px;" });
$(".my-select2").chosen();

//Modal Ekranı
$('#myModal').on('show.bs.modal', function (e) {
    console.debug('modal shown!');
    $('.my-select', this).chosen({ width: "700px" });
    $('.my-select2', this).chosen({ width: "700px" });
});

$("#drpMUrunListe").chosen().change(drpUrunIslem);
$("#drpMIslemListe").chosen().change(drpIslemGetir);

function drpUrunIslem() {
    $.post("Home/GetOperations", {
        Product_Id: $("#drpMUrunListe").val()
    },
        function (data, status) {
            OperationData = data;

            if (data.length > 0) {
                $("#txtMPrice").val(data[0].Price);
            }
            $('#drpMIslemListe').empty(); //remove all child nodes
            $.each(data, function (idx, obj) {
                $("#drpMIslemListe").append('<option value="' + obj.Operation_Id + '">' + obj.Name + '</option>');
                if ($("#drpMIslemListe").val() === obj.Operation_Id) {
                    $("#txtMPrice").val(obj.Price);
                }
            });

            $('#drpMIslemListe').trigger("chosen:updated");


        });
}

function drpIslemGetir() {
    $.each(OperationData, function (idx, obj) {
        if ($("#drpMIslemListe").val() === obj.Operation_Id) {
            $("#txtMPrice").val(obj.Price);
        }
    });
    $('#txtMPrice').trigger("chosen:updated");

}

function createTable() {
    var rows = "";
    for (var i = 0; i < orderwithdetail.OrderDetails.length; i++) {
        rows += '<tr><td>' + '<button type="button" id="btnIslemSil" class="btn btn-danger btn-xs" onclick="IslemSil(' + i.toString() + ')" value="' + orderwithdetail.OrderDetails[i].Operation_Id + '">';
        rows += '<span class="fa fa-close"></span></button> ' + '</td > ';
        rows += '<td>' + orderwithdetail.OrderDetails[i].OperationText + '</td>';
        rows += '<td>' + orderwithdetail.OrderDetails[i].Quantity + '</td>';
        rows += '<td>' + orderwithdetail.OrderDetails[i].Price + 'TL' + '</td>';
    }
    var divTable = document.getElementById("tabloveri");
    divTable.innerHTML = rows;
}

function IslemSil(index) {
    var array = orderwithdetail.OrderDetails;
   
    if (index > -1) {
        array.splice(index, 1);

        createTable();
    }
}

$("#btnUrunEkle").click(function () {
    var orderdetail = {};


    var MOrderDetailId = $("#txtMOrderDetailId").val();
    var MUrun = $("#drpMUrunListe").val();
    var MIslem = $("#drpMIslemListe").val();
    var MIslemText = $("#drpMIslemListe option:selected").text();
    var MAdet = $("#txtMAdet").val();
    var MPrice = $("#txtMPrice").val();
    var MTPrice = MAdet * MPrice;

    orderdetail.OrderDetail_Id = MOrderDetailId;
    orderdetail.Operation_Id = MIslem;
    orderdetail.OperationText = MIslemText;
    orderdetail.Quantity = MAdet;
    orderdetail.Price = MPrice;
    orderdetail.TotalPrice = MTPrice;


    if (MUrun != 0) {
        orderwithdetail.OrderDetails.push(orderdetail);
        createTable();
        $('#drpMUrunListe').val(0);
        $('#drpMIslemListe').val(0);
        $('#drpMUrunListe').trigger("chosen:updated");
        $('#drpMIslemListe').trigger("chosen:updated");
        $("#txtMPrice").val("");
    }
    else {
        HataBildirim();
    }

});

$("#btnSiparisKaydet").click(function () {

    var MAdi = $("#txtMAdi").val();
    var MTelefon = $("#txtMTelefon").val();
    var MSTarihi = $("#txtMSTarihi").val();
    var MAciklama = $("#txtMAciklama").val();
    var MOrderId = $("#txtMOrderId").val();
    var MTDurum;
    if ($("#txtMDelivery")[0].checked==true) {
        MTDurum = true;
    }
    else {
        MTDurum = false;
    }
 

    orderwithdetail.Order_Id = MOrderId;
    orderwithdetail.CustomerName = MAdi;
    orderwithdetail.PhoneNumber = MTelefon;
    orderwithdetail.Description = MAciklama;
    orderwithdetail.OrderDate = MSTarihi;
    orderwithdetail.CreatedUser = 1;
    orderwithdetail.CreatedDate = Date.now;
    orderwithdetail.IsDelivered = MTDurum;
    orderwithdetail.IsDeleted = false;
    

   
    if (MAdi !== "" && MTelefon !== "" && MSTarihi !== "") {
        $("#btnSiparisKaydet").attr("disabled", "disabled");
        $.post("Home/OrderSave", { orderwithdetail: orderwithdetail },
            function (data, status) {
                if (data.success == true) {
                    Bildirim(data.message);
                    setTimeout(function () { location.reload(); }, 2000);
                }
                else {
                    $("#btnSiparisKaydet").removeAttr("disabled");
                    if (data.requiredLogin) {
                        Bildirim(data.message);
                        setTimeout(function () { location.href = "/Login/Index"; },2000);                        
                    }
                    else {
                        Bildirim(data.message);
                    }
                }

            });
    }
    else {
        HataBildirim();
    }
});

function GetOrder(id) {
    $.post("Home/GetOrderData", {
        Order_Id: id
    },

        function (data, status) {
            
            orderwithdetail = data;
            createTable();

            $("#txtMOrderId").val(orderwithdetail.Order_Id);
            $("#txtMAdi").val(orderwithdetail.CustomerName);
            $("#txtMTelefon").val(orderwithdetail.PhoneNumber);
            $("#txtMAciklama").val(orderwithdetail.Description);
            $("#txtMSTarihi").val(inputFormatDate(jsDate(orderwithdetail.OrderDate)));
            $("#txtMDelivery").attr('checked', orderwithdetail.IsDelivered);
                   
        });

}

function Bildirim(text) {
    $.notify({
        icon: 'fa fa-info',
        message: text
    },
        {
            type: 'info',
            timer: 5000
        });
}

function HataBildirim() {
    $.notify({
        icon: 'fa fa-warning',
        message: 'Bilgilerinizi kontrol ediniz!'
    },
        {
            type: 'danger',
            timer: 5000
        });
}

function Yenile() { location.reload(); }

$("#modalkapat").click(function () {
    Yenile();
});

