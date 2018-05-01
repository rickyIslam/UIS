/// <reference path="angular.min.js" />
/// <reference path="angular.js" />

var apiModule = angular.module("apiModule", []);


 //2nd controller 
apiModule.controller("enrolledList", function ($scope, $http) {

    $scope.btnText = "Run Query";
    $scope.saveDate = function () {
        $scope.btnText = "API is Working";
        $http.get("/api/enrolledApi?session=" + $scope.session + "&faculty=" + $scope.faculty + "&semester=" + $scope.semester).then(function (d) {
            $scope.toggle = !$scope.toggle;
            $scope.btnText = "Run Query";
            $scope.insert = d.data;
            $scope.msg = d.data;
            console.log(d.data);
            alert(d.data);
        }, function (error) {
            alert('Failed');
        });

    }
}), 

apiModule.controller("fiEnrolledList", function ($scope, $http) {

    $scope.btnText = "Run Query";
    $scope.saveDate = function () {
        $scope.btnText = "API is Working";
        $http.get("/api/fiEnrolledApi?session=" + $scope.session + "&faculty=" + $scope.faculty + "&semester=" + $scope.semester).then(function (d) {
            $scope.toggle = !$scope.toggle;
            $scope.btnText = "Run Query";
            $scope.insert = d.data;
            $scope.msg = d.data;
            console.log(d.data);
            alert(d.data);
        }, function (error) {
            alert('Failed');
        });

    }
});


