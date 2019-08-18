//<nav aria-label="Page navigation example" id="navForPaging">
//                    <ul class="pagination pagination-template d-flex justify-content-center">
//                        <li class="page-item">
//                            <button class="btn btn-danger" id="DownerPage" onclick="UpDownPage(false)"><i class="fa fa-angle-left"></i></button>
//                        </li>
//                        <li class="page-item text-center">

//                            <input type="text" class="form-text text-center" style="width:30%;display:inline" id="currentPage" min="1" onkeyup="ChangeCurrentPage()" /> <b> / </b> <b id="maxPage"></b>

//                        </li>
//                        <li>
//                            <button class="btn btn-danger" id="UpperPage" onclick="UpDownPage(true)"><i class="fa fa-angle-right"></i></button>
//                        </li>
//                    </ul>
//</nav>

function CheckNavForPaging() {
    if (getMaxPage <= 0) {
        $("#navForPaging").hide()
    }
    else {
        $("#navForPaging").show();
    }
}

var getMaxPage;
var pageSize = 5;
var offsetBetweenRealPostsAndFirstPosts = 0;
var posts
var currentPageText = $("#currentPage");

ResetPage();

function MaxPage() {

    if (posts.length % pageSize == 0) {
        getMaxPage = parseInt((posts.length / pageSize));
    }
    else if (posts.length > 2) {
        getMaxPage = parseInt((posts.length / pageSize)) + 1;
    }
    else {
        getMaxPage = 1;
    }
}


function ResetPage(currentPageFromAnother) {
    posts = $("[name='commentPanel']")
    MaxPage();
    //posts.remove();
    $("#maxPage").html(getMaxPage);
    document.getElementById("currentPage").max = getMaxPage;
    for (var i = 0; i < posts.length; i++) {
        if (i >= pageSize) {
            $(posts[i]).hide();
        }
        else {
            //$("#noiCommentDungChan").append();
            $(posts[i]).show();
        }
    }
    currentPageText.val("1");
    currentPage = 1;
    CheckButtonUpDownPage();
    CheckNavForPaging();
    ChangeCurrentPage(currentPageFromAnother);
}

var currentPage;
//$(".post").remove();
function ChangeCurrentPage(currentPageFromAnother) {
    currentPage = document.getElementById("currentPage").value;
    if (isNaN(parseInt(currentPageFromAnother)) == false) {
        currentPageText.val(currentPageFromAnother);
        currentPage = parseInt(currentPageFromAnother);
    }
    if (isNaN(parseInt(currentPageText.val())) && currentPageText.val() != "") {
        currentPageText.val("1");
        currentPage = 1;
    }
    else if (parseInt(currentPage) < 1) {
        currentPageText.val("1");
        currentPage = 1;
    }
    else if (parseInt(currentPage) > getMaxPage) {
        currentPageText.val(getMaxPage);
        currentPage = getMaxPage;
    }
    if (currentPage != "") {
        var skipPost = (currentPage - 1) * pageSize;
        for (var i = 0; i < posts.length; i++) {
            if (i < skipPost || i >= skipPost + pageSize) {
                $(posts[i]).hide();
            }
            else {
                //$("#noiCommentDungChan").append(posts[i]);
                $(posts[i]).show();
            }
        }
    }
    CheckButtonUpDownPage()
}

function CheckButtonUpDownPage() {
    var downButton = $("#DownerPage");
    var upButton = $("#UpperPage");
    if (currentPage <= 1) {
        downButton.hide();
        upButton.show();
    }
    else if (currentPage >= getMaxPage) {
        downButton.show();
        upButton.hide();
    }
    else {
        downButton.show();
        upButton.show();
    }
}

function UpDownPage(isUp) {
    if (isUp == true) {
        if (currentPage >= getMaxPage) {
            currentPage = 1;
        }
        else {
            currentPage++;
        }
    } else {
        if (currentPage <= 1) {
            currentPage = getMaxPage;
        }
        else {
            currentPage--;
        }
    }
    currentPageText.val(currentPage);
    ChangeCurrentPage();
}