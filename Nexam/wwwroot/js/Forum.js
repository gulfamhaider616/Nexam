

$(document).ready(function () {
    $('#questionSubmit').click(function () {
        var uname = $('#qname').val();
        var qemail = $('#qemail').val();
        var qcountry = $('#qcountry').val();
        var qsubject = $('#qsubject').val();
        var qexplanation = $('#qexplanation').val();
        $.ajax({
            url: '/Forum/SaveQuestion',
            type: 'GET',
            dataType: 'json',
            data: {
                'uname': uname,
                'qemail': qemail,
                'qcountry': qcountry,
                'qsubject': qsubject,
                'qexplanation': qexplanation
            },
            success: function (result) {
                if(result==true)
                {
                    showPopupModel("Thank you!", "We'll review your question and publish it very soon.", "/Nexam-Forum")
                }
                else
                {
                    showPopupModel("Sorry!", "We can't publish your question, Please try again.", "/Nexam-Forum-Question")
                }
            },
            error: function () {
                showPopupModel("Sorry!", "We can't publish your question due to internal error.", "/Nexam-Forum-Question")
            }
        });
    });
});

function showPopupModel(headingText, paragraphText, baseUrl) {
    var modal = document.getElementById('myModal');
    document.getElementById('modelheading').innerHTML = headingText;
    document.getElementById('modelparagraph').innerHTML = paragraphText;
    // When the user clicks the button, open the modal
    modal.style.display = "block";
    // Get the <span> element that closes the modal
    var span = document.getElementsByClassName("close")[0];
    // When the user clicks on <span> (x), close the modal
    span.onclick = function () {
        modal.style.display = "none";
        window.location.href = baseUrl;
    }
    // When the user clicks anywhere outside of the modal, close it
    window.onclick = function (event) {
        if (event.target == modal) {
            modal.style.display = "none";
            window.location.href = baseUrl;
        }
    }
}

function PublishQuestion(QuestionID) {
    $.ajax({
        url: '/Forum/PublishQuestionByAdmin',
        type: 'GET',
        datatype: 'Json',
        data: { 'QuestionID': QuestionID },
        success: function (result) {
            if (result == true) {
                showPopupModel("Thank you!", "Your Question has been Published.", "/Forum/UnPublish")
            }
            else {
                showPopupModel("Sorry!", "We can't publish your question, Please try again.", "/Forum/UnPublish")
            }
        },
        error: function () {
            showPopupModel("Sorry!", "We can't publish your question due to internal error.", "/Forum/UnPublish")
        }
    });
}

function DeleteQuestions(QuestionID) {
    $.ajax({
        url: '/Forum/DeleteQuestions',
        type: 'GET',
        datatype: 'Json',
        data: { 'QuestionID': QuestionID },
        success: function (result) {
            if (result == true) {
                showPopupModel("Thank you!", "Your Question has been Deleted.", "/Forum/UnPublish")
            }
            else {
                showPopupModel("Sorry!", "We can't Deleted your question, Please try again.", "/Forum/UnPublish")
            }
        },
        error: function () {
            showPopupModel("Sorry!", "We can't Deleted your question due to internal error.", "/Forum/UnPublish")
        }
    });
}