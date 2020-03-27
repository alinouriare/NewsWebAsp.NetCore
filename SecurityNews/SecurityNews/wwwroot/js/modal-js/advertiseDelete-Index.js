(function ($) {
    function Advertising() {
        var $this = this;

        function initilizeModel() {
            $("#modal-action-advertiseDelete").on('loaded.bs.modal', function (e) {

            }).on('hidden.bs.modal', function (e) {
                $(this).removeData('bs.modal');
            });
        }
        $this.init = function () {
            initilizeModel();
        }
    }
    $(function () {
        var self = new Advertising();
        self.init();
    })
}(jQuery))
