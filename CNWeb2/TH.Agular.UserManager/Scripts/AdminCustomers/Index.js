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
    $http.post(_domain + "AdminCustomers/GetListCustomers").then(function (response) {

        $scope.listCustomers = response.data;  
        $scope.btnDeleteClick = function (id) {
            if (!confirm("Bạn thật sự muốn xóa ?")) return;
            $http.post(_domain + "AdminCustomers/DeleteCustomer/" + id).then(function (result) {
                if (result.data == true) {
                    $http.post(_domain + "AdminCustomers/GetListCustomers").then(function (response) {
                        $scope.listCustomers = response.data;
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
        $scope.btnSaveClick = function () {
            // validate
            if ($scope.FullName == null || $scope.FullName == "") {
                alert("Nhập họ tên");
                return;
            }
            // parameter
            var data = {
                FullName: $scope.FullName,
                Address: $scope.Address,
                Phone: $scope.Phone,
                Email: $scope.Email
            }
            // post
            $http.post(_domain + "AdminCustomers/InsertCustomer", JSON.stringify(data)).then(function (response) {
                alert("Đã lưu dữ liệu thành công");
                window.location = '#!/list';
            });
        }  
    } else {
        // Update
        // load
        $scope.CustomerId = $routeParams.id;
        $http.post(_domain + "AdminCustomers/GetCustomer/" + $scope.CustomerId).then(function (response) {
            $scope.CustomerId = response.data.CustomerId;
            $scope.FullName = response.data.FullName;
            $scope.Address = response.data.Address;
            $scope.Phone = response.data.Phone;
            $scope.Email = response.data.Email;
        });
        $scope.btnSaveClick = function () {
            
            // validate
            if ($scope.FullName == null || $scope.FullName == "") {
                alert("Nhập họ tên");
                return;
            }            
            // parameter
            var data = {
                CustomerId: $scope.CustomerId,
                FullName: $scope.FullName,
                Address: $scope.Address,
                Phone: $scope.Phone,
                Email: $scope.Email

            }
            // post
            $http.post(_domain + "AdminCustomers/UpdateCustomer", JSON.stringify(data)).then(function (response) {
                alert("Đã lưu dữ liệu thành công");
                window.location = '#!/list';       
            });
        }    
    }              
});

