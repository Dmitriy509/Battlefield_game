//updateRoom();
let timerId = setInterval(function () { updateRoom(); }, 1000);
function updateRoom() {

    // alert('asdf');
    // console.log(login);

    $.post("/SetShips/UpdateInfoRoom", { playername: login })
        .done(function (data) {
            //  console.log("ответ запроса");
            document.getElementById('user2').innerText = data.player2status;
            if (data.gamestatus != "") {
                //  console.log(data.gamestatus);
                window.location.href = data.gamestatus;

            }
        });
    console.log(login);
}