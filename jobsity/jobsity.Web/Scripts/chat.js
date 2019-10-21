$(document).ready(function () {
    var lastMessageId = 0;
    var myUserId = $("#IdUser").val();
    var allowedCommands = [];

    $.get($("#botUrl").val() + "ChatBot/GetAllowedCommands", function (data) {
        allowedCommands = data;
    });

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
        var message = $("#Message1").val();
        if (message != "") {
            if (message.substring(0, 1) == "/") {
                if (!message.includes("=")) {
                    alert("Wrong Forman");
                    return false;
                }

                var command = message.split('=')[0].replace("/","");

                if (!allowedCommands.includes(command)) {
                    alert("Command not Allowed");
                    return false;
                }

                $.get($("#botUrl").val() + "ChatBot/getStock", { stock_code: message.split('=')[1] }, function (data) {
                    $("form textarea").val("");
                    if (data.error != null) {
                        alert(data.error);
                        return false;
                    }

                    console.log(data);
                    var message = data.obj.Symbol + " quote is $" + data.obj.Close + " per share";
                    console.log(message);
                    console.log($("#botUserId").val());

                    $.post("/Chat/CreateMessage", { Message1: message, IdUser: $("#botUserId").val(), __RequestVerificationToken:$("input[name=__RequestVerificationToken]").val() }, function () { $("form textarea").val(""); })
                });

            } else {
                var data = $("form").serialize();
                $.post("/Chat/CreateMessage", data, function () { $("form textarea").val(""); });
            }
        }
    });

});