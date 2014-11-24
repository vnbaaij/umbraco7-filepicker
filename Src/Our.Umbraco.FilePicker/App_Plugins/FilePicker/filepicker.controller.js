function filePickerController($scope, dialogService) {


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

    $scope.remove = function () {
        $scope.model.value = "";
    };

    function populate(data) {
        $scope.model.value = $scope.model.config.folder + data;
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