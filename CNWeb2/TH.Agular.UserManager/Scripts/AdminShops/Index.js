app.config(function ($routeProvider) {   
    $routeProvider
        .when("/", {
            templateUrl: urlList
        })
        .when("/list", {
            templateUrl: urlList
        })
        .when("/new", {
            templateUrl: urlDetail
        }).when("/edit/:id", {
            templateUrl: urlDetail

        });
});
app.controller('myListController', function ($scope, $http) {
    $http.post(_domain + "AdminShops/GetListShops").then(function (response) {

        $scope.listShops = response.data;  
        $scope.btnDeleteClick = function (id) {
            if (!confirm("Bạn thật sự muốn xóa ?")) return;
            $http.post(_domain + "AdminShops/DeleteShop/" + id).then(function (result) {
                if (result.data == true) {
                    $http.post(_domain + "AdminShops/GetListShops").then(function (response) {
                        $scope.listShops = response.data;
                    });
                }
            });
        }
    });
});
app.controller("myDetailController", function ($scope, $routeParams, $http) {
    if ($routeParams.id == null) {
        //Insert
        onDetailLoad();
        $scope.IsActive = true;
        $scope.btnSaveClick = function () {
            // validate
            //if ($scope.Name == null || $scope.Name == "") {
            //    alert("Nhập tên cửa hàng");
            //    return;
            //}
            //if ($scope.OrderIndex == null || $scope.OrderIndex == "") {
            //    alert("Nhập số thứ tự");
            //    return;
            //}
            // parameter
            var _img = $("#hiddenImageSrc").val();
            var data = {
                Name: $scope.Name,
                Description: $scope.Description,
                Alias: $scope.Alias,
                Image: _img,
                Phone: $scope.Phone,
                Address: $scope.Address,
                OwnerName: $scope.OwnerName,
                OwnerPhone: $scope.OwnerPhone,                                
                OrderIndex: $scope.OrderIndex,
                IsActive: $scope.IsActive
            }
            // post
            $http.post(_domain + "AdminShops/InsertShop", JSON.stringify(data)).then(function (response) {
                alert("Đã lưu dữ liệu thành công");
                window.location = '#!/list';
            });
        }  
        
    } else {
        // Update
        // load
        onDetailLoad();
        $scope.ShopId = $routeParams.id;
        $http.post(_domain + "AdminShops/GetShop/" + $scope.ShopId).then(function (response) {
            $scope.ShopId = response.data.ShopId; 
            $scope.Name = response.data.Name;
            $scope.Description = response.data.Description;
            $scope.Alias = response.data.Alias;
            //$scope.Image = response.data.Image,
            var responseImage = response.data.Image;
            $("#hiddenImageSrc").val(responseImage);
            if (responseImage == null || responseImage == "") {
                $('#imgShowImage').attr('src', _urlNoImage);                                  
            } else {
                $('#imgShowImage').attr('src', responseImage);                
            }
            
            $scope.Phone = response.data.Phone;
            $scope.Address = response.data.Address;
            $scope.OwnerName = response.data.OwnerName;
            $scope.OwnerPhone = response.data.OwnerPhone;
            $scope.OrderIndex = response.data.OrderIndex;
            $scope.IsActive = response.data.IsActive;

            
        });
        $scope.btnSaveClick = function () {
            // validate
            //if ($scope.Name == null || $scope.Name == "") {
            //    alert("Nhập tên loại dịch vụ");
            //    return;
            //}
            //if ($scope.OrderIndex == null || $scope.OrderIndex == "") {
            //    alert("Nhập số thứ tự");
            //    return;
            //}
            // parameter
            var _img = $("#hiddenImageSrc").val();
            var data = {
                ShopId: $scope.ShopId,
                Name: $scope.Name,
                Description: $scope.Description,
                Alias: $scope.Alias,
                Phone: $scope.Phone,
                Address: $scope.Address,
                OwnerName: $scope.OwnerName,
                OwnerPhone: $scope.OwnerPhone,
                OrderIndex: $scope.OrderIndex,
                IsActive: $scope.IsActive,
                Image: _img
            }
            // post
            $http.post(_domain + "AdminShops/UpdateShop", JSON.stringify(data)).then(function (response) {
                alert("Đã lưu dữ liệu thành công");
                window.location = '#!/list';       
            });
        }    
        
    } 
});

function onDetailLoad() {
    $("#btnSelectImage, #divSelectImage").click(function () {
        //alert("chọn ảnh");
        $("#fileUploadImage").trigger("click");

    });

    $("#fileUploadImage").change(function () {
        readURL(this);
    });
    $("#btnRemoveImage").click(function () {
        if (confirm("Bạn muốn gỡ bỏ ảnh ?")) {
            $('#imgShowImage').attr('src', "../../Content/admin/images/no-image.png");
            $("#hiddenImageSrc").val("../../Content/admin/images/no-image.png");
        }
    });
    
}
function readURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#imgShowImage').attr('src', e.target.result);
            $("#hiddenImageSrc").val(e.target.result);

        }
        reader.readAsDataURL(input.files[0]); // convert to base64 string

    }
}




