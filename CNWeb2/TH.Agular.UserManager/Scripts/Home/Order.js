$(document).ready(function () {
    $("#btnFinish").click(function () {
        

        var pFullName = $("#txtFullName").val();
        var pAddress = $("#txtAddress").val();
        var pPhone = $("#txtPhone").val();
        var pEmail = $("#txtEmail").val();
        var pDescription = $("#txtDescription").val();

        if (pFullName == "") {
            alert("Nhập họ tên");
            return;
        }
        if (pAddress == "") {
            alert("Nhập họ địa chỉ");
            return;
        }
        if (pPhone == "") {
            alert("Nhập điện thoại");
            return;
        }
        var url = _domain + "Home/FinishOrder";

        var param = {
            FullName:pFullName,
            Address:pAddress,
            Phone:pPhone,
            Email: pEmail,
            Description: pDescription
        }
        $.post(url, param).done(function (result) {
            if (result == "ok") {
                alert("Đã gửi đơn hàng thành công");
                window.location = _domain + "Home/Index";
            } else if (result == "false") {
                alert("Phiên làm việc hết hạn, vui lòng thêm lại giỏ hàng");
            } else {
                alert("Lỗi hệ thống:" + result);
            }
        });
    });
});