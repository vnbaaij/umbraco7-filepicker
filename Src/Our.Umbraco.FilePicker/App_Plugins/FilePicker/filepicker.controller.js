function filePickerController($scope, dialogService) {
    $scope.model.value = $scope.model.value || [];
    
    $scope.openPicker = function () {
        dialogService.open({
            template: "/App_Plugins/FilePicker/filepickerdialog.html",
            callback: populate,
            dialogData: {
                filter: $scope.model.config.filter,
                folder: $scope.model.config.folder
            }
        });
    };

    $scope.remove = function (item) {
        $scope.model.value.filter(function (obj) {
            return obj !== item;
        });
    };

    function populate(data) {
        var file = $scope.model.config.folder + data;
        if ($scope.model.value.indexOf(file) === -1) {
            $scope.model.value.push(file);
        }
    };
};
angular.module("umbraco").controller("Our.Umbraco.FilePickerController", filePickerController);

function folderPickerController($scope, dialogService) {
    $scope.openPicker = function () {
        dialogService.open({
            template: "/App_Plugins/FilePicker/folderpickerdialog.html",
            callback: populate
        });
    };
    function populate(data) {
        $scope.model.value = "/" + data;
    };

};
angular.module("umbraco").controller("Our.Umbraco.FolderPickerController", folderPickerController);

function filePickerDialogController($scope, dialogService) {
    $scope.dialogEventHandler = $({});
    $scope.dialogEventHandler.bind("treeNodeSelect", nodeSelectHandler);

    function nodeSelectHandler(ev, args) {
        args.event.preventDefault();
        args.event.stopPropagation();
        if (args.node.icon !== "icon-folder")
            $scope.submit(args.node.id);
    };
};
angular.module("umbraco").controller("Our.Umbraco.FilePickerDialogController", filePickerDialogController);

function folderPickerDialogController($scope, dialogService) {
    $scope.dialogEventHandler = $({});
    $scope.dialogEventHandler.bind("treeNodeSelect", nodeSelectHandler);

    function nodeSelectHandler(ev, args) {
        args.event.preventDefault();
        args.event.stopPropagation();
        $scope.submit(args.node.id);
    };
};
angular.module("umbraco").controller("Our.Umbraco.FolderPickerDialogController", folderPickerDialogController);
