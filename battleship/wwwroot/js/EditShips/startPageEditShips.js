/// <reference path="pageeditshipsresize.js" />
/// <reference path="pageeditshipsresize.js" />
/// <reference path="pageeditshipsresize.js" />
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
const borderTable = 2;
var cell_size = cellSizeCount();
const login = get_cookie("Login");

if (login == null) {
    window.location.href = 'Login';
}
else {
    document.getElementById('player-name').innerText = login;
    document.getElementById('playerNameSend').value = login;
}


//pageEditShipsResize.js
setCellSize(field);
fieldb = getElementBounds(field);
setShipsCellSize();


setPlayerReadyState();

//updateEditShipView.js
updateRoom();
let timerId = setInterval(function () { updateRoom(); }, 1000);



function cellSizeCount() {
    let place = document.getElementById("place-battlefield");
    let minDim = (place.offsetWidth < place.offsetHeight) ? place.offsetWidth : place.offsetHeight;
    return Math.floor((minDim - 9 * borderCell - 2 * borderTable) / 10) + 1;
}

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
    $.post("/Test/Ready", { playername: login })
        .done(function (data1) {
        });
}

