var toasterOptions = {
    "closeButton": true,
    "debug": false,
    "newestOnTop": false,
    "progresponsesBar": false,
    "positionClass": "toast-top-right",
    "preventDuplicates": true,
    "showDuration": "300",
    "hideDuration": "1000",
    "timeOut": "5000",
    "extendedTimeOut": "1000",
    "showEasing": "swing",
    "hideEasing": "linear",
    "showMethod": "fadeIn",
    "hideMethod": "fadeOut"
};

function handelMessage(response) {
    return response.message != null && (response.message.indexOf('\n') >= 0 || response.message.indexOf('\r') >= 0) ? response.message.replace('\n', '<br />') : response.message
}

function showTosater(response) {
    var message = handelMessage(response);
    switch (response.type) {
        case 0:
            {
                toastr.success(message, response.title, toasterOptions);
                break;
            }
        case 1:
            {
                toastr.error(message, response.title, toasterOptions);
                break;
            }
        case 2:
            {
                toastr.warning(message, response.title, toasterOptions);
                break;
            }
        case 3:
            {
                toastr.info(message, response.title, toasterOptions);
                break;
            }
    }
}

const AjaxSuccess = function (e) {
    // get the message error
    const res = arguments && arguments["1"] && arguments["1"].responseJSON && arguments && arguments["1"] && arguments["1"].responseJSON
    $('[data-ajax-success-handler=true]').each(function () {
        const { ajaxSuccess } = this.dataset;
        // invoke failure function  by its stored name in the dataset
        eval(ajaxSuccess)(res);
    })
}

$(document).ajaxSuccess(AjaxSuccess);
