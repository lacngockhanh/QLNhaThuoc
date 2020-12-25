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
    $http.post(_domain + "AdminServiceTypes/GetListServiceTypes").then(function (response) {

        $scope.listServiceTypes = response.data;  
        $scope.btnDeleteClick = function (id) {
            if (!confirm("Bạn thật sự muốn xóa ?")) return;
            $http.post(_domain + "AdminServiceTypes/DeleteServiceType/" + id).then(function (result) {
                if (result.data == true) {
                    $http.post(_domain + "AdminServiceTypes/GetListServiceTypes").then(function (response) {
                        $scope.listServiceTypes = response.data;
                    });
                }
            });
        }
    });
});
app.controller("myDetailController", function ($scope, $routeParams, $http) {
    if ($routeParams.id == null) {
        //Insert
        $scope.IsActive = true;
        // load select Shops
        $http.post(_domain + "AdminServiceTypes/GetListShops").then(function (response) {
            $scope.shops = response.data;            
        });

        $scope.btnSaveClick = function () {
            // validate
            if ($scope.Name == null || $scope.Name == "") {
                alert("Nhập tên loại dịch vụ");
                return;
            }
            if ($scope.OrderIndex == null || $scope.OrderIndex == "") {
                alert("Nhập số thứ tự");
                return;
            }
            if ($scope.ShopId == null || $scope.ShopId == "") {
                alert("Chọn nhà thuốc");
                return;
            }
            // parameter
            var data = {
                Name: $scope.Name,
                OrderIndex: $scope.OrderIndex,
                IsActive: $scope.IsActive,
                ShopId: $scope.ShopId
            }
            // post
            $http.post(_domain + "AdminServiceTypes/InsertServiceType", JSON.stringify(data)).then(function (response) {
                alert("Đã lưu dữ liệu thành công");
                window.location = '#!/list';
            });
        }  
    } else {
        // Update
        // load

        
        $scope.ServiceTypeId = $routeParams.id;
        // load select Shops
        $http.post(_domain + "AdminServiceTypes/GetListShops").then(function (response) {
            $scope.shops = response.data;
            $http.post(_domain + "AdminServiceTypes/GetServiceType/" + $scope.ServiceTypeId).then(function (response) {
                $scope.ServiceTypeId = response.data.ServiceTypeId;
                $scope.Name = response.data.Name;
                $scope.OrderIndex = response.data.OrderIndex;
                $scope.IsActive = response.data.IsActive;
                $scope.ShopId = response.data.ShopId;

            });
        });
        
        $scope.btnSaveClick = function () {
            // validate
            if ($scope.Name == null || $scope.Name == "") {
                alert("Nhập tên loại dịch vụ");
                return;
            }
            if ($scope.OrderIndex == null || $scope.OrderIndex == "") {
                alert("Nhập số thứ tự");
                return;
            }
            // parameter
            var data = {
                ServiceTypeId: $scope.ServiceTypeId,
                Name: $scope.Name,
                OrderIndex: $scope.OrderIndex,
                ShopId: $scope.ShopId,
                IsActive: $scope.IsActive
            }
            // post
            $http.post(_domain + "AdminServiceTypes/UpdateServiceType", JSON.stringify(data)).then(function (response) {
                alert("Đã lưu dữ liệu thành công");
                window.location = '#!/list';       
            });
        }    
    }


});

