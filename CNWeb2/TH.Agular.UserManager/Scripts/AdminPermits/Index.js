$(document).ready(function () {
    btnAddPermit_Click();
    btnRemovePermit_Click();
    $("#selectUser").select2();
});
function btnAddPermit_Click() {
    $(".btnAddPermit").click(function () {
        $("#hiddenRoleId").val($(this).data("role-id"));
        $("#txtRoleName").val($(this).data("role-name"));
        $("#modalAddPermit").modal("show");

        $("#btnSavePermit").unbind("click").click(function () {
            var roleId = $("#hiddenRoleId").val();
            var userId = $("#selectUser").val();
            if (userId == "") {
                alert("Chưa chọn người dùng");
                return;
            }

            var param = {
                RoleId: roleId,
                UserId: userId
            }
            $.post(_domain + "AdminPermits/CheckPermitAvailable", param)
            .done(function (response) {
                if (response == "available") {
                    alert("Người dùng đã phân quyền vào nhóm này rồi, vui lòng kiểm tra lại");
                    return;
                } else {
                    $.post(_domain + "AdminPermits/InsertPermit", param)
                        .done(function (_response) {
                            if (_response == "ok") {
                                alert("Thêm phân quyền thành công");
                                location.reload();
                            }
                        });
                }
            });              
        });
    });
}
function btnRemovePermit_Click() {
    $(".btnRemovePermit").click(function () {
        if (confirm("Bạn thật sự muốn xóa ?")) {
            var permitId = $(this).data("permit-id");
            var param = {
                Id: permitId
            }
            $.post(_domain + "AdminPermits/RemovePermit", param)
                .done(function (_response) {
                    if (_response == "ok") {
                        //alert("Đã xóa phân quyền thành công");
                        location.reload();
                    }
                });
        }
        
    });
}