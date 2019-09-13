var appendTo;
var currentElement;
var statusId = 0;
$(function () {
    var kanbanCol = $('.panel-body');
    kanbanCol.css('max-height', (window.innerHeight - 150) + 'px');

    var kanbanColCount = parseInt(kanbanCol.length);
    $('.container-fluid').css('min-width', (kanbanColCount * 350) + 'px');

    draggableInit();

    $('.panel-heading').click(function () {
        var $panelBody = $(this).parent().children('.panel-body');
        $panelBody.slideToggle();
    });
});

function draggableInit() {
    var sourceId;

    $('[draggable=true]').bind('dragstart', function (event) {
        sourceId = $(this).parent().attr('id');
        event.originalEvent.dataTransfer.setData("text/plain", event.target.getAttribute('id'));
    });

    $('.panel-body').bind('dragover', function (event) {
        event.preventDefault();
    });

    $('.panel-body').bind('drop', function (event) {
        var children = $(this).children();
        var targetId = children.attr('id');

        if (sourceId != targetId) {
            var elementId = event.originalEvent.dataTransfer.getData("text/plain");
            var taskId = elementId.slice(4);
            
            currentElement = document.getElementById(elementId);
            appendTo = children;

            var toStatus = $(this).children().attr('id')
            switch (toStatus) {
                case "InProgress":
                    statusId = 2;
                    break;
                case "Resolved":
                    statusId = 3
                    break;
                case "Closed":
                    statusId = 10;
                    break;
                default:
                    break;
            }

            //post data
            $.ajax({
                url: "../../Solution/Create?taskId=" + taskId + "&statusId=" + statusId,
                cache: false,
                processData: false,
                contentType: false,
                type: 'GET',
                data: {
                    'taskId': taskId,
                    'statusId': status
                },
                success: function (result) {
                    if (result !== "0" && result !== "1") {
                        $("#solutionModalDungChan").html(result);
                        settingValidateSolution();
                        submitSolution();
                        initTextArea();
                    } else if (result === "1") {
                        appendTo.append(currentElement);
                    }
                }
            });
        }

        event.preventDefault();
    });
}

function startSolutionByButton(taskId, toStatus) {
    switch (toStatus) {
        case "InProgress":
            statusId = 2;
            break;
        case "Resolved":
            statusId = 3
            break;
        case "ReOpened":
            statusId = 4;
            break;
        case "Closed":
            statusId = 10;
            break;
        default:
            break;
    }

    //post data
    $.ajax({
        url: "../../Solution/Create?taskId=" + taskId + "&statusId=" + statusId,
        type: 'GET',
        data: {
        },
        success: function (result) {
            if (result !== "0" && result !== "1") {
                $("#solutionModalDungChan").html(result);
                settingValidateSolution();
                submitSolution();
                initTextArea();
            }
            else if (result == "1") {
                var btn = document.getElementById("card" + taskId);
                chooseTask(btn, parseInt(taskId));
                $.notify("Success!", "success");
            }
            else {
                $.notify("Failed!", "error");
            }
                
        }
    });
}

function settingValidateSolution() {
    $("#solutionModal").modal("show");


    $('#formSolution').validate({
        errorClass: 'errors',
        rules: {
            Reason: {
                required: true,
                maxlength: 300
            },
            Solution: {
                required: true,
                maxlength: 300
            }
        },
        messages: {
            Reason: {
                required: "Reason is required",
                maxlength: "Reason max length is 300"
            },
            Solution: {
                required: "Solution is required",
                maxlength: "Solution max length is 300"
            }
        },
        highlight: function (element) {
            $(element).parent().addClass('has-error');
        },
        unhighlight: function (element) {
            $(element).parent().removeClass('has-error');
        },
    });
}

function submitSolution() {
    $('#submitSolution').click(function () {
        var taskId = document.getElementById("solution.TaskId").value;
        var dataParam = {
            "TaskId": taskId,
            "ResolveType": document.getElementById("solution.ResolveType").value,
            "Reason": tinymce.get('solution.Reason').getContent(),
            "SolutionDescription": tinymce.get('solution.Solution').getContent(),
            "Description": tinymce.get('solution.Description').getContent()
        }
        var returnValue = false;
        var resolveType = document.getElementById("solution.ResolveType");
        if (resolveType.value === "0") {
            $(resolveType).parent().addClass('has-error');
            returnValue = false;
        } else {
            $(resolveType).parent().removeClass('has-error');
            returnValue = true;
        }
        var valid = $("#formSolution").valid() && returnValue;
        if (valid === true) {
            $.ajax({
                url: $("#formSolution").attr('action'),
                data: {
                    "solutionJson": JSON.stringify(dataParam),
                    "StatusId": statusId
                },
                cache: false,
                type: 'post',
                dataType: "html",
                success: function (result) {
                    if (result == "1") {
                        returnValue = true;
                    }

                    if (returnValue === true && currentElement != null) {
                        $("#solutionModal").modal("hide");
                        appendTo.append(currentElement);
                    } else if (returnValue === true && currentElement == null) {
                        var btn = document.getElementById("card" + taskId);
                        chooseTask(btn, parseInt(taskId));
                        $("#solutionModal").modal("hide");
                    }
                }
            });
            
        }
        return false;
    });
}