var isDragging = false;

$(function () {
    var chatIcon = $("#chat");

    chatIcon.draggable({
        start: function () {
            isDragging = true;
        },
        stop: function () {
            isDragging = false;
        },
        containment: "window"
    });

    // Then instead of using click use mouseup, and on mouseup only fire if the flag is set to false
    chatIcon.bind('mouseup', function () {
        if (!isDragging) {
            loadChatbox();
        }
    });
});

function loadChatbox() {
    var e = document.getElementById("chatbox");
    e.style.margin = "0";
    e.style.display = "block"
    var f = document.getElementById("chat");
    var t = f.style.right;
    f.style.display = "none";
}

function closeChatbox() {
    var e = document.getElementById("chatbox");
    e.style.display = "none";
    var f = document.getElementById("chat");
    f.style.display = "block"
}

function reloadChatbox() {
    var e = document.getElementById("ibotWindow");
    e.src = e.src;
}
