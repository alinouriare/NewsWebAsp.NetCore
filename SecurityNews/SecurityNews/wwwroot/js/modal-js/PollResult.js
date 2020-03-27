(function ($) {
    function Poll() {
        var $this = this;

        function initilizeModel() {
            $("#modal-action-pollResult").on('loaded.bs.modal', function (e) {

            }).on('hidden.bs.modal', function (e) {
                $(this).removeData('bs.modal');
            });
        }
        $this.init = function () {
            initilizeModel();
        }
    }
    $(function () {
        var self = new Poll();
        self.init();
    })
}(jQuery))
