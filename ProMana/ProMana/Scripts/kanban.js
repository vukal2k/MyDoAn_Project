var appendTo;
var currentElement;
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

            //post data
            $.ajax({
                url: "../../Solution/Create/" + taskId,
                cache: false,
                processData: false,
                contentType: false,
                type: 'GET',
                success: function (result) {
                    $("#solutionModalDungChan").html(result);
                    settingValidateSolution();
                    submitSolution();
                }
            });
            
            currentElement = document.getElementById(elementId);
            appendTo = children;
        }

        event.preventDefault();
    });
}


function settingValidateSolution() {
    $("#solutionModal").modal("show");


    $('#formSolution').validate({
        errorClass: 'errors',
        rules: {
            ResolveType: {
                required: true
            },
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
            ResolveType: {
                required: "Resolve Type is required"
            },
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
    $('#submitSolution').bind('click', function (event) {
        var dataParam = {
            "TaskId": document.getElementById("solution.TaskId").value,
            "ResolveType": document.getElementById("solution.ResolveType").value,
            "Reason": document.getElementById("solution.Reason").value,
            "Solution": document.getElementById("solution.Solution").value,
            "Description": document.getElementById("solution.Description").value
        }
        var returnValue = false;
        var valid = $("#formSolution").valid();
        if (valid === true) {
            var fd = $("#formSolution").serialize();
            alert(JSON.stringify(dataParam));
            $.ajax({
                url: $("#formSolution").attr('action'),
                data: {
                    "solutionJson": JSON.stringify(dataParam)
                },
                cache: false,
                type: 'post',
                dataType: "html",
                success: function (result) {
                    if (result == "1") {
                        returnValue = true;
                    }
                }
            });
            
        }
        if (returnValue === true) {
            appendTo.append(currentElement);
        }
        event.preventDefault();
    });
}