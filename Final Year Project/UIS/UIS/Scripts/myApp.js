/// <reference path="angular.min.js" />
/// <reference path="angular.js" />

var apiModule = angular.module("apiModule", []);


// 2nd controller 
apiModule.controller("insert", function ($scope, $http) {


        $http.get("http://localhost:8024/api/Test").then(function (d) {
                   $scope.insert = d.data[0];
                   alert($scope.insert);
                }, function (error) {
                    alert('Failed');
                });

    //$scope.btnText = "Save dataaa";
    //$scope.saveDate = function () {


    //    $http.get("http://localhost:8024/api/Test").then(function (d) {
    //              //  $scope.insert = d.data[0];
    //                alert(d.data);
    //            }, function (error) {
    //                alert('Failed');
    //            });

    //    //$scope.btnText = "Please Wait...";
    //    //$http.post('http://localhost:8024/api/Test?', JSON.stringify($scope.insert)).then(function (d) {

    //    //   // $http.post('http://localhost:8024/api/Test').then(function (d) {

    //    //    $scope.btnText = "Doneeeeeeee";
    //    //    alert(d.data);

    //    //},
    //    //function (error) {
    //    //    alert("Problemmm");
    //    //}

    //    //);

    //};






});




//var myApp = angular
//.module("myModule", [])
//.controller("myController", function ($scope, $http) {

//    // insert data
//    $scope.btnText = "Save These";
//    $scope.saveDate = function () {
//        $scope.btnText = "Please wait...";//"Please wait...";
//        var data = $scope.insert;
//        $http.post('/dynamic/insertRecord', JSON.stringify($scope.insert)).then(function (response) {

//            if (response.data)

//                alert(response.data);
//            $scope.btnText = "Save";
//            $scope.insert = null;
//            console.log($scope.insert);


//        }, function (response) {

//            alert(response.data);

//        });

//    };


//    // update data (Fatching)
//    $scope.loadRecord = function (id) {

//        $http.get("/dynamic/getPersonalDetail?id=" + id).then(function (d) {
//            $scope.insert = d.data[0];
//            alert($scope.insert);
//        }, function (error) {
//            alert('Failed');
//        });

//    };


//    // update data (Storing....)
//    $scope.updateData = function () {
//        $scope.btnText = "Please wait...";//"Please wait...";
//        var data = $scope.insert;
//        $http.post('/dynamic/updateRecord', JSON.stringify($scope.insert)).then(function (response) {

//            if (response.data)

//                alert(response.data);
//            $scope.btnText = "Save";
//            $scope.insert = null;
//            //console.log($scope.insert);


//        }, function (response) {

//            alert(response.data);

//        });

//    };

//    // retrive data 
//    $http.get("/dynamic/getMessage").then(

//        function (d) {
//            $scope.msg = d.data;
//        },

//        function (error) {
//            alert('Failed');
//        }
//    );


//    // delete data
//    $scope.deleteRecord = function (id) {
//        $http.get("/dynamic/deleteRecord?id=" + id).then(function (d) {
//            alert(d.data);
//            $http.get("/dynamic/getMessage").then(function (d) {
//                $scope.msg = d.data;
//            }, function (error) {
//                alert('Failed');
//            });
//        }, function (error) {
//            alert('Failed');
//        });
//    };


//});


