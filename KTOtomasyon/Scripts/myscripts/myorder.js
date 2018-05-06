//Genel Objelerimiz
var orderwithdetail = { OrderDetails: [] };
var OperationData;


//Dropdownlistlere onChange attr'si eklendi.
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
                if ($("#drpMIslemListe").val() == obj.Operation_Id) {
                    $("#txtMPrice").val(obj.Price);
                }
            });

            $('#drpMIslemListe').trigger("chosen:updated");


        });
}

function drpIslemGetir() {
    $.each(OperationData, function (idx, obj) {
        if ($("#drpMIslemListe").val() == obj.Operation_Id) {
            $("#txtMPrice").val(obj.Price);
        }
    });
    $('#txtMPrice').trigger("chosen:updated");

}

function txtTelefonGetir() {
    var pid = $("#txtMTelefon").val();

    $.post("Home/GetPhoneData", {
        PNumber: pid
    },
    function (data, status) {
        if (data.success == true) {
            Bildirim(data.message);
            $("#txtMAdi").val(data.retObject.CustomerName);
            $("#txtMTelefon").val(data.retObject.PhoneNumber);
            $("#txtMDebt").val(data.retObject.Debt);
        }
        else {
            if (data.requiredLogin) {
                Bildirim(data.message);
                setTimeout(function () { location.href = "/Login/Index"; }, 2000);
            }
            else {
                Bildirim(data.message);
            }
        }

    });
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
    var r = confirm("İşlemi onaylıyor musunuz?");
    if (r == true) {
        var array = orderwithdetail.OrderDetails;
        if (index > -1) {
            array.splice(index, 1);

            createTable();
        }
    }          
}


$("#btnSiparisKaydet").click(function () {

    var MAdi = $("#txtMAdi").val();
    var MTelefon = $("#txtMTelefon").val();
    var MSTarihi = $("#txtMSTarihi").val();
    var MDebt = $("#txtMDebt").val();
    var MDiscount = $("#txtMDiscount").val();
    var MAddition = $("#txtMAddition").val();
    var MOrderId = $("#txtMOrderId").val();
    var MTDurum;
    var MOdendi;
    if ($("#txtMDelivery")[0].checked == true) {
        MTDurum = true;
    }
    else {
        MTDurum = false;
    }
    if ($("#txtMPaid")[0].checked == true) {
        MOdendi = true;
    }
    else {
        MOdendi = false;
    }
    

    orderwithdetail.Order_Id = MOrderId;
    orderwithdetail.CustomerName = MAdi;
    orderwithdetail.PhoneNumber = MTelefon;
    orderwithdetail.Debt = MDebt;
    orderwithdetail.OrderDate = MSTarihi;
    orderwithdetail.CreatedUser = 1;
    orderwithdetail.CreatedDate = Date.now;
    orderwithdetail.IsPaid = MOdendi;
    orderwithdetail.IsDelivered = MTDurum;
    orderwithdetail.IsDeleted = false;
    orderwithdetail.Discount = MDiscount;
    orderwithdetail.Addition = MAddition;
    orderwithdetail.Debt = MDebt;


    if (MAdi !== "" && MTelefon !== "") {
        $("#btnSiparisKaydet").attr("disabled", "disabled");
        $.post("Home/OrderSave", { orderWithDetail: orderwithdetail },
            function (data, status) {
                if (data.success == true) {
                    Bildirim(data.message);
                    //setTimeout(function () {
                    //    location.href = "/Home/Index/?filter=" + $("#txtMTelefon").val();
                    //}, 2000);
                }
                else {
                    $("#btnSiparisKaydet").removeAttr("disabled");
                    if (data.requiredLogin) {
                        Bildirim(data.message);
                        setTimeout(function () { location.href = "/Login/Index"; }, 2000);
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
            $("#txtMDebt").val(orderwithdetail.Debt);
            $("#txtMSTarihi").val(inputFormatDate(jsDate(orderwithdetail.OrderDate)));
            $("#txtMCTarihi").val(inputFormatDate(jsDate(orderwithdetail.CreatedDate)));
            $("#txtMDelivery").attr('checked', orderwithdetail.IsDelivered);
            $("#txtMPaid").attr('checked', orderwithdetail.IsPaid);
            $("#txtMDiscount").val(orderwithdetail.Discount);
            $("#txtMAddition").val(orderwithdetail.Addition);
            $("#txtMDebt").val(orderwithdetail.Debt);

        });

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


    if (MUrun != 0 && MIslem != 0 && MIslemText != 0) {
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

