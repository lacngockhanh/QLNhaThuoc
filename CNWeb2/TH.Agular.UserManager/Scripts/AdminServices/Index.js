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
    $http.post(_domain + "AdminServices/GetListServices").then(function (response) {

        $scope.listServices = response.data;  
        $scope.btnDeleteClick = function (id, name) {
            if (!confirm("Bạn thật sự muốn xóa '" + name + "'?")) return;
            var param = {
                id: id
            }
            $http.post(_domain + "AdminServices/DeleteService", param).then(function (result) {
                if (result.data == true) {
                    window.location.reload();
                }
            });
        }
    });
    $(document).ready(function () {
        $("#txtSearch").on("keyup", function () {
            var value = $(this).val().toLowerCase();
            $("#myTable tr").filter(function () {
                $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
            });
        });
    });
});
app.controller("myDetailController", function ($scope, $routeParams, $http) {
    if ($routeParams.id == null) {
        //Insert

        $http.post(_domain + "AdminServices/GetService/" + $scope.ServiceId).then(function (response) {
            $http.post(_domain + "AdminServiceTypes/GetListServiceTypes").then(function (response) {
                $scope.servicetypes = response.data;
            });            
        });
        $scope.IsActive = true;

        $scope.btnSaveClick = function () {
            // validate
            if ($scope.Name == null || $scope.Name == "") {
                alert("Nhập tên dịch vụ");
                return;
            }
            if ($scope.OrderIndex == null || $scope.OrderIndex == "") {
                alert("Nhập số thứ tự");
                return;
            }
            // parameter
            var data = {
                ServiceId: $scope.ServiceId,
                ServiceTypeId: $scope.ServiceTypeId,
                OrderIndex: $scope.OrderIndex,
                Name: $scope.Name,
                Unit: $scope.Unit,
                Alias: $scope.Alias,
                DefaultQuantity: $scope.DefaultQuantity,
                ImportPrice: $scope.ImportPrice,
                Price: $scope.Price,
                Description: $scope.Description,
                OrderIndex: $scope.OrderIndex,
                IsActive: $scope.IsActive
            }
            // post
            $http.post(_domain + "AdminServices/InsertService", JSON.stringify(data)).then(function (response) {
                alert("Đã lưu dữ liệu thành công");
                window.location = _domain + urlIndex; 
            });
        }  
    } else {
        // Update
        // load
        $scope.ServiceId = $routeParams.id;
        $scope.ServiceTypeId = "";
        $http.post(_domain + "AdminServices/GetService/" + $scope.ServiceId).then(function (response) {
            $http.post(_domain + "AdminServiceTypes/GetListServiceTypes").then(function (response) {
                $scope.servicetypes = response.data;
            });             
            $scope.ServiceId = response.data.ServiceId;
            $scope.ServiceTypeId = response.data.ServiceTypeId;
            $scope.OrderIndex = response.data.OrderIndex;
            $scope.Name = response.data.Name;
            $scope.Alias = response.data.Alias;
            $scope.Unit = response.data.Unit;
            $scope.DefaultQuantity = response.data.DefaultQuantity;
            $scope.ImportPrice = response.data.ImportPrice;
            $scope.Price = response.data.Price;
            $scope.Description = response.data.Description;
            $scope.OrderIndex = response.data.OrderIndex;
            $scope.IsActive = response.data.IsActive;  
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

            
            //alert($scope.ServiceTypeId);
            //return;
            // parameter
            var data = {                                  
                ServiceId: $scope.ServiceId,
                ServiceTypeId: $scope.ServiceTypeId,
                OrderIndex: $scope.OrderIndex,
                Name: $scope.Name,
                Unit: $scope.Unit,
                Alias: $scope.Alias,
                DefaultQuantity: $scope.DefaultQuantity,
                ImportPrice: $scope.ImportPrice,
                Price: $scope.Price,
                Description: $scope.Description,
                OrderIndex: $scope.OrderIndex,
                IsActive: $scope.IsActive
            }
            // post
            $http.post(_domain + "AdminServices/UpdateService/" + $scope.ServiceId, JSON.stringify(data)).then(function (response) {
                alert("Đã lưu dữ liệu thành công");
                window.location = _domain + urlIndex;       
            });
        }    
    }

    
});


