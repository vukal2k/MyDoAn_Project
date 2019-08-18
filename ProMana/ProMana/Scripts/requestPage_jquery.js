function AssigneTo(taskId) {
    var assigneTo = $("AssigneTo").val();

    //post data
    $.ajax({
        url: "../../Task/AssignTo?taskId=" + taskId + "&userName=" + assigneTo,
        cache: false,
        processData: false,
        contentType: false,
        type: 'GET',
        data: {
            'taskId': taskId
        },
        success: function (result) {
            if (result !== "0" && result !== "1") {
                $("#noiDetailDungChan").html(result);
                validateTask("edit");
            }
        }
    });
}