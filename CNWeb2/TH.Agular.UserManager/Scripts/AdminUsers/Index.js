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
    $http.post(_domain + "AdminUsers/GetListUsers").then(function (response) {
        $scope.toDate2 = function (dataString) {
            if (dataString != null) {
                var number = Number(dataString.replace('/Date(', '').replace(')/', ''));

                return new Date(number).toLocaleString("it-IT");
            } else return "-";

        };
        $scope.listUsers = response.data;  

        $scope.btnDeleteClick = function (id) {
            if (!confirm("Bạn thật sự muốn xóa ?")) return;
            $http.post(_domain + "AdminUsers/DeleteUser/" + id).then(function (result) {
                if (result.data == true) {
                    $http.post(_domain + "AdminUsers/GetListUsers").then(function (response) {
                        $scope.listUsers = response.data;
                    });
                }
            });
        }
        
    });
});
app.controller("myDetailController", function ($scope, $routeParams, $http) {
    
    if ($routeParams.id == null) {
        //Insert
        $http.post(_domain + "AdminUsers/GetListShops").then(function (response) {
            $scope.IsActive = true;
            $scope.shops = response.data;
            $scope.btnSaveClick = function () {
                // validate
                if ($scope.FullName == null || $scope.FullName == "") {
                    alert("Nhập họ tên");
                    return;
                }
                if ($scope.UserName == null || $scope.UserName == "") {
                    alert("Nhập tên đăng nhập");
                    return;
                }
                if ($scope.Password == null || $scope.Password == "") {
                    alert("Nhập mật khẩu");
                    return;
                }
                if ($scope.ShopId == null || $scope.ShopId == "") {
                    alert("Chọn cửa hàng");
                    return;
                }
                // parameter
                var data = {
                    FullName: $scope.FullName,
                    UserName: $scope.UserName,
                    Password: $scope.Password,
                    Email: $scope.Email,
                    Phone: $scope.Phone,
                    IsActive: $scope.IsActive,
                    ShopId: $scope.ShopId
                }
                // post
                $http.post(_domain + "AdminUsers/InsertUser", JSON.stringify(data)).then(function (response) {
                    alert("Đã lưu dữ liệu thành công");
                    window.location = '#!/list';
                });
            }  
        });

    } else {
        // Update
        // load
        $scope.UserId = $routeParams.id;
        $http.post(_domain + "AdminUsers/GetListShops").then(function (response) {
            $scope.shops = response.data;
            $http.post(_domain + "AdminUsers/GetUser/" + $scope.UserId).then(function (response) {
                $scope.UserId = response.data.UserId;
                $scope.FullName = response.data.FullName;
                $scope.UserName = response.data.UserName;
                $scope.Email = response.data.Email;
                $scope.Phone = response.data.Phone;
                $scope.IsActive = response.data.IsActive;
                $scope.ShopId = response.data.ShopId;
                $scope.ShopName = response.data.ShopName;
                
            });
            $scope.Password = "";
            $("#txtPassword").attr("placeholder", " - Không đổi mật khẩu thì để trống -");
            $("#txtUserName").attr("disabled", "true");
            

            $scope.btnSaveClick = function () {
                // validate
                if ($scope.FullName == null || $scope.FullName == "") {
                    alert("Nhập họ tên");
                    return;
                }
                // parameter
                var data = {
                    UserId: $scope.UserId,
                    FullName: $scope.FullName,
                    UserName: $scope.UserName,
                    Password: $scope.Password,
                    Email: $scope.Email,
                    Phone: $scope.Phone,
                    IsActive: $scope.IsActive,
                    ShopId: $scope.ShopId
                }
                // post
                $http.post(_domain + "AdminUsers/UpdateUser", JSON.stringify(data)).then(function (response) {
                    alert("Đã lưu dữ liệu thành công");
                    window.location = '#!/list';
                });
            }   
        });
         
    }


});

