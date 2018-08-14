var app = angular.module("app", ['ui.bootstrap']);
app.controller("userController", ["$scope", "$http", function ($scope, $http) {

    $scope.page = function (pageinex) {
        $scope.pagesize = 1;
        $scope.pageindex = pageinex || 1;
        $http.post("/user/page", { pageIndex: $scope.pageindex, pageSize: $scope.pagesize }).then(function (result) {
            $scope.data = result.data.rows;
            $scope.pagecount = result.data.count;
        });
    }
    $scope.page();
}])

app.controller("authorizeController", ["$scope", "$http", function ($scope, $http) {
    $scope.message = "请加载站点授权信息！";
    $scope.apiUrl = "http://localhost:8000/api/schema";
    $scope.load = function () {
        if (!$scope.apiUrl) return;
        $http.get($scope.apiUrl).then(function (result) {
            $scope.data = result.data;
        }, function () {
            $scope.data = null;
            $scope.message = "加载站点授权信息失败！";
        })
    }
    $scope.save = function (action) {
        $http.post("/user/updateclaim", { userId: userId, checked: action.checked, action: action }).then(function () {
            alert("Success!");
        });
    }
}]);