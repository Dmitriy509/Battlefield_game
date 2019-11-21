var arrships = {
    "singledesk1": { x: -1, y: -1 },
    "singledesk2": { x: -1, y: -1 },
    "singledesk3": { x: -1, y: -1 },
    "singledesk4": { x: -1, y: -1 },
    "doubledesk1": { x: -1, y: -1 },
    "doubledesk2": { x: -1, y: -1 },
    "doubledesk3": { x: -1, y: -1 },
    "tripledesk1": { x: -1, y: -1 },
    "tripledesk2": { x: -1, y: -1 },
    "fourdesk": { x: -1, y: -1 }
};
//alert("aaa");
const field = document.getElementById('battlefield1');
var fieldb;
//alert(fieldb.left);
const borderCell = 1;
const borderTable = 3;
var cell_size = cellSizeCount();
const player_id = get_cookie("Player_Id");
//const login = get_cookie("Login");

if (player_id == null) {
    window.location.href = '/Login/Login';
}
else {
    document.getElementById('player-name').innerText = get_cookie("Login");;
    document.getElementById('playerIdSend').value = player_id;
}


//pageEditShipsResize.js
setCellSize(field);
fieldb = getElementBounds(field);
setShipsCellSize();


setPlayerReadyState();

//updateEditShipView.js
updateRoom();
let timerId = setInterval(function () { updateRoom(); }, 1000);





function setPlayerReadyState() {
    // console.log(PlayerShips[0].x + "  " + PlayerShips[0].y);

    if (flagPlayerReady) {
        for (var i = 0; i < PlayerShips.length; i++) {

            arrships[PlayerShips[i].shipname].x = PlayerShips[i].x;
            arrships[PlayerShips[i].shipname].y = PlayerShips[i].y;
            let ship = document.getElementById(PlayerShips[i].shipname);
            ship.style.position = 'absolute';
            ship.style.left = fieldb.left + arrships[PlayerShips[i].shipname].x * cell_size + "px"
            ship.style.top = fieldb.top + arrships[PlayerShips[i].shipname].y * cell_size + "px"
            ship.onmousedown = null;
            ship.ondblclick = null;
            if (PlayerShips[i].align == 'v') {
                rotate(ship);
            }

        }
        document.getElementById('btn-ready').disabled = true;
        document.getElementById("player1-ready").src = "../img/ready.png";
        PlayerShips = null;
    }




}


function test() {
    $.post("/Test/Ready", { player_id: player_id })
        .done(function (data1) {
        });
}

