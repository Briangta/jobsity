$(document).ready(function () {
    var lastMessageId = 0;
    var myUserId = $("#IdUser").val();

    function getMessages() {
        $.post("/Chat/GetMessages", { lastMessageId: lastMessageId }, function (data) {
            data = JSON.parse(data);
            $.each(data, function (da, va) {
                showMessages(va);
                lastMessageId = va.IdMessage;
            });

            $("#messagesContainer").scrollTop($("#messagesContainer")[0].scrollHeight);
        });
    }

    getMessages();

    setInterval(function () {
        getMessages();
    }, 2000);

    function showMessages(message) {
        var obj = $("#messageTemplate").clone();
        
        obj.find(".details").text(message.User.FirstName + " " + message.User.LastName);
        obj.find(".date").text(message.Date.replace("T", " "));
        obj.find(".message").text(message.Message1);
        obj.css("display", "block");

        if (message.IdUser == myUserId)
            obj.removeClass("alert-primary").addClass("alert-success");

        $("#messagesContainer").append(obj);
    }

    $("form").submit(function (e) {
        e.preventDefault();
        var data = $("form").serialize();
        $.post("/Chat/CreateMessage", data, function () { $("form textarea").val(""); });
    });

});