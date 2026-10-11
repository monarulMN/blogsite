document.addEventListener("DOMContentLoaded", function () {

    const chatToggle = document.getElementById("chatToggle");
    const chatWidget = document.getElementById("chatWidget");
    const chatClose = document.getElementById("chatClose");
    const chatBody = document.getElementById("chatBody");
    const chatForm = document.getElementById("message-box"); 

    if (!chatToggle || !chatWidget || !chatClose || !chatForm || !chatBody) {
        console.error("Chat elements not found in DOM.");
        return;
    }


    chatToggle.addEventListener("click", () => {

        var userId = $("#userId").val();

        $.ajax({
            url: '/Reader/Support/GetUserMessages',
            type: 'POST',
            data: { userId: userId },

            success: function (response) {

                chatBody.innerHTML = "";

                $.each(response, function (index, message) {

                    let isMine = message.userType === 1 || message.userType === 2;

                    if (message.userType === 3) {
                        $("#RecipientId").val(message.senderId);
                    }

                    let messageHtml = `
                        <div class="message ${isMine ? "outgoing" : "incoming"}">
                            <div class="message-bubble">
                                ${message.text}
                                <div class="message-time">
                                    ${formatTime(message.createdAt)}
                                </div>
                            </div>
                        </div>
                    `;

                    chatBody.insertAdjacentHTML("beforeend", messageHtml);
                });

                chatBody.scrollTop = chatBody.scrollHeight;
            },
            error: function (err) {
                console.log(err);
            }
        });

        chatWidget.style.display = "flex";
    });

    chatClose.addEventListener("click", () => {
        chatWidget.style.display = "none";
    });

 
    chatForm.addEventListener("submit", function (e) {
        e.preventDefault();
        const input = document.getElementById("MessageText");
        const text = input.value.trim();
        if (!text) return;

        var formData = $(this).serialize();

        $.ajax({
            url: $(this).attr("action"),
            type: "POST",
            data: formData,

            success: function (response) {

                input.value = "";

                if (response && response.text) {

                    let messageHtml = `
                        <div class="message outgoing">
                            <div class="message-bubble">
                                ${response.text}
                                <div class="message-time">
                                    Just now
                                </div>
                            </div>
                        </div>
                    `;

                    chatBody.insertAdjacentHTML("beforeend", messageHtml);
                    chatBody.scrollTop = chatBody.scrollHeight;
                }
            },
            error: function (err) {
                console.log(err);
                alert("Message sending failed!");
            }
        });
    });


    function formatTime(dateString) {
        let date = new Date(dateString);
        return date.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
    }

});