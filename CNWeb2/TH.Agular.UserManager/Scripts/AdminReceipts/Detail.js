$(document).ready(function () {
    $("#selectServices").select2();
    $("#btnSelectCustomer").click(function () {
        $("#modalSelectCustomer").modal("show");
    });
    btnAddItem_Click();
    txtSearchService_KeyUp();
    //$('.quantity').mask('000,000,000,000,000.00', { reverse: true });
    $('.price').mask('000,000,000,000,000', { reverse: true });
    $('.money').mask('000,000,000,000,000', { reverse: true });
    
    //btnRemoveItem_Click();


    //txtQuantity_Change();
});
var totalMoney = 0;



//function btnRemoveItem_Click() {
//    $(".btnRemoveItem").click(function () {

//        var receiptServiceId = $(this).data("receipt-service-id");
//        var _param = { 
//            ReceiptServiceId: receiptServiceId
//        }
//        $http.post(_domain + "AdminReceipts/RemoveItem/", _param).then(function (result) {
//            if (result.data == "true") {
//                istReceiptServices = result.data;
//                $scope.listReceiptServices = listReceiptServices;
//            }                
//        });
//    });
//}
function txtSearchService_KeyUp() {
    $("#txtSearchService").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#tableSelectService tr").filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
        });
    });
}

function btnAddItem_Click() {
    $("#btnAddItem").click(function () {
        $("#modalSelectServices").modal("show");        
    });
}
showLoading();
app.controller('myReceiptDetailController', function ($scope, $http) {
    // load list receipt services

    
    
    var _param = {
        ReceiptId: receiptId
    }
    $scope.loadReceiptServices = function () {
        $http.post(_domain + "AdminCustomers/GetListCustomers").then(function (result) {
            $scope.listCustomers = result.data;
            $scope.btnSelectCustomerClick = function (receiptId, customerId) {

                //alert(receiptId + ";" + customerId);

                var _param = {
                    ReceiptId: receiptId,
                    CustomerId: customerId
                }

                $http.post(_domain + "AdminReceipts/UpdateCustomerId/", _param).then(function (result) {
                    if (result.data == "true") {
                        $http.post(_domain + "AdminReceipts/GetReceiptCustomer/", _param).then(function (result) {

                            $scope.customer = result.data;
                            $("#modalSelectCustomer").modal("hide");
                        });
                    }
                });

            }
        });
        $http.post(_domain + "AdminReceipts/GetReceipt", _param).then(function (result) {
            $scope.receipt = result.data;
            $http.post(_domain + "AdminReceipts/GetReceiptCustomer/", _param).then(function (result) {

                $scope.customer = result.data;
                hideLoading();
            });
            //alert($scope.receipt.UserId);
            if ($scope.receipt.UserId == null) {
                $scope.UserFullName = currentUserFullName;
                $scope.UserId = currentUserId;
            } else {
                $scope.UserFullName = $scope.receipt.UserFullName;
                $scope.UserId = $scope.receipt.UserId;
            }

        });

        $http.post(_domain + "AdminReceipts/LoadListReceiptServices/", _param).then(function (result) {

            $scope.listReceiptServices = result.data;
            $scope.btnRemoveItemClick = function (id, itemName) {
                if (!confirm("Bạn thật sự muốn xóa '" + itemName + "' ?")) return;

                var param = {
                    ReceiptServiceId: id
                }
                $http.post(_domain + "AdminReceipts/RemoveItem/", param).then(function (result) {
                    if (result.data == "true") {
                        $http.post(_domain + "AdminReceipts/LoadListReceiptServices/", _param).then(function (result) {


                            $scope.listReceiptServices = result.data;
                            $scope.sumMoney();
                        });
                    }

                });
            }
            $scope.btnSelectServiceClick = function (receiptId, serviceId) {

                var _param = {
                    ReceiptId: receiptId,
                    ServiceId: serviceId
                }

                $http.post(_domain + "AdminReceipts/CheckAvailableReceiptServices/", _param).then(function (result) {
                    if (result.data == "true") {
                        alert("Lỗi: Dịch vụ này đã thêm rồi, vui lòng kiểm tra lại");
                    } else {
                        $http.post(_domain + "AdminReceipts/ReceiptAddItem/", _param).then(function (result) {
                            if (result.data == "true") {
                                $("#modalSelectServices").modal("hide");
                                $http.post(_domain + "AdminReceipts/LoadListReceiptServices/", _param).then(function (result) {
                                    $scope.listReceiptServices = result.data;
                                    $scope.sumMoney();
                                });
                            }
                        });
                    }
                });

            }
            $scope.sumMoney = function () {
                $('.money').unmask();
                $("#txtTotalMoney").unmask();
                var totalMoney = 0;
                angular.forEach($scope.listReceiptServices, function (value, key) {
                    value.Money = value.Quantity * value.Price;
                    totalMoney += value.Money;
                });
                $('.money').mask('000,000,000,000,000', { reverse: true });
                $("#txtTotalMoney").val(totalMoney);
                $("#txtTotalMoney_Text").val(DocTienBangChu(totalMoney));
                $('#txtTotalMoney').mask('000,000,000,000,000', { reverse: true });
            }
            $scope.sumMoney();
        });

        $scope.btnSaveClick = function () {
            //alert("hihi");
            if ($scope.customer.CustomerId == "") {
                // thêm
                var param = {
                    FullName: $scope.customer.FullName,
                    Address: $scope.customer.Address,
                    Phone: $scope.customer.Phone,
                    Email: $scope.customer.Email,
                    UserId: $scope.UserId
                }
                $http.post(_domain + "AdminReceipts/InsertCustomer", param).then(function (result) {
                    var newCustomerId = result.data;
                    
                    var _param = {
                        ReceiptId: $scope.receipt.ReceiptId,
                        CustomerId: newCustomerId
                    }
                    $http.post(_domain + "AdminReceipts/UpdateCustomerId/", _param).then(function (result) {
                        if (result.data == "true") {

                            var _param = {
                                ReceiptId: $scope.receipt.ReceiptId,
                                Description: $scope.receipt.Description,
                                UserId: $scope.receipt.UserId
                            }

                            $http.post(_domain + "AdminReceipts/UpdateReceipt/", _param).then(function (result) {
                                if (result.data == "true") {
                                    //alert("Đã lưu hóa đơn");
                                    $scope.customer.CustomerId = newCustomerId;
                                    $.notify({ message: 'Đã lưu dữ liệu thành công'},{
                                        // options                                        
                                        delay: 1000, 
                                        type: 'success',
                                        placement: {
                                            from: 'bottom',
                                            align: 'center'
                                        }
                                    });
                                }                                
                            });

                        }
                    });
                });
            } else {
                 //chỉnh
                var _param = {
                    ReceiptId: $scope.receipt.ReceiptId,
                    Description: $scope.receipt.Description,
                    UserId: $scope.UserId
                }
                $http.post(_domain + "AdminReceipts/UpdateReceipt/", _param).then(function (result) {
                    if (result.data == "true") {
                        //alert("Đã lưu dữ liệu thành công");
                        $.notify({ message: 'Đã lưu dữ liệu thành công' }, {
                                                                
                            delay: 1000,
                            type: 'success',
                            placement: {
                                from: 'bottom',
                                align: 'center'
                            }
                        });
                    }
                });
            }
        };
    };
    $scope.txtQuantityChange = function (itemId, itemQuantity) {
        
        var param = {
            ReceiptServiceId: itemId,
            Quantity: itemQuantity
        }
        $http.post(_domain + "AdminReceipts/UpdateQuantity/", param).then(function (result) {
            if (result.data == "true") {
                $scope.sumMoney();
            } else {
                alert("Lỗi hệ thống, chưa thể cập nhật dữ liệu!");
            }
        });
        //$scope.sumMoney();
    }
    $scope.loadReceiptServices();
    
    
    
    
});



var ChuSo = new Array(" không ", " một ", " hai ", " ba ", " bốn ", " năm ", " sáu ", " bảy ", " tám ", " chín ");
var Tien = new Array("", " nghìn", " triệu", " tỷ", " nghìn tỷ", " triệu tỷ");

//1. Hàm đọc số có ba chữ số;
function DocSo3ChuSo(baso) {
    var tram;
    var chuc;
    var donvi;
    var KetQua = "";
    tram = parseInt(baso / 100);
    chuc = parseInt((baso % 100) / 10);
    donvi = baso % 10;
    if (tram == 0 && chuc == 0 && donvi == 0) return "";
    if (tram != 0) {
        KetQua += ChuSo[tram] + " trăm ";
        if ((chuc == 0) && (donvi != 0)) KetQua += " linh ";
    }
    if ((chuc != 0) && (chuc != 1)) {
        KetQua += ChuSo[chuc] + " mươi";
        if ((chuc == 0) && (donvi != 0)) KetQua = KetQua + " linh ";
    }
    if (chuc == 1) KetQua += " mười ";
    switch (donvi) {
        case 1:
            if ((chuc != 0) && (chuc != 1)) {
                KetQua += " mốt ";
            }
            else {
                KetQua += ChuSo[donvi];
            }
            break;
        case 5:
            if (chuc == 0) {
                KetQua += ChuSo[donvi];
            }
            else {
                KetQua += " lăm ";
            }
            break;
        default:
            if (donvi != 0) {
                KetQua += ChuSo[donvi];
            }
            break;
    }
    return KetQua;
}
function DocTienBangChu(SoTien) {
    var lan = 0;
    var i = 0;
    var so = 0;
    var KetQua = "";
    var tmp = "";
    var ViTri = new Array();
    if (SoTien < 0) return "Số tiền âm !";
    if (SoTien == 0) return "Không đồng !";
    if (SoTien > 0) {
        so = SoTien;
    }
    else {
        so = -SoTien;
    }
    if (SoTien > 8999999999999999) {
        //SoTien = 0;
        return "Số quá lớn!";
    }
    ViTri[5] = Math.floor(so / 1000000000000000);
    if (isNaN(ViTri[5]))
        ViTri[5] = "0";
    so = so - parseFloat(ViTri[5].toString()) * 1000000000000000;
    ViTri[4] = Math.floor(so / 1000000000000);
    if (isNaN(ViTri[4]))
        ViTri[4] = "0";
    so = so - parseFloat(ViTri[4].toString()) * 1000000000000;
    ViTri[3] = Math.floor(so / 1000000000);
    if (isNaN(ViTri[3]))
        ViTri[3] = "0";
    so = so - parseFloat(ViTri[3].toString()) * 1000000000;
    ViTri[2] = parseInt(so / 1000000);
    if (isNaN(ViTri[2]))
        ViTri[2] = "0";
    ViTri[1] = parseInt((so % 1000000) / 1000);
    if (isNaN(ViTri[1]))
        ViTri[1] = "0";
    ViTri[0] = parseInt(so % 1000);
    if (isNaN(ViTri[0]))
        ViTri[0] = "0";
    if (ViTri[5] > 0) {
        lan = 5;
    }
    else if (ViTri[4] > 0) {
        lan = 4;
    }
    else if (ViTri[3] > 0) {
        lan = 3;
    }
    else if (ViTri[2] > 0) {
        lan = 2;
    }
    else if (ViTri[1] > 0) {
        lan = 1;
    }
    else {
        lan = 0;
    }
    for (i = lan; i >= 0; i--) {
        tmp = DocSo3ChuSo(ViTri[i]);
        KetQua += tmp;
        if (ViTri[i] > 0) KetQua += Tien[i];
        if ((i > 0) && (tmp.length > 0)) KetQua += ',';//&& (!string.IsNullOrEmpty(tmp))
    }
    if (KetQua.substring(KetQua.length - 1) == ',') {
        KetQua = KetQua.substring(0, KetQua.length - 1);
    }
    KetQua = KetQua.substring(1, 2).toUpperCase() + KetQua.substring(2);
    return KetQua;//.substring(0, 1);//.toUpperCase();// + KetQua.substring(1);
}