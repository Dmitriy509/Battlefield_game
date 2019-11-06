setCellSize(field);
fieldb = getElementBounds(field);
setShipsCellSize();
fieldb = getElementBounds(field);
setPlayerReadyState();

function setShipsCellSize() {
    //   let arr = ["singledesk1", "singledesk2", "singledesk3", "singledesk4", "doubledesk1", "doubledesk2", "doubledesk3", "tripledesk1", "tripledesk2", "fourdesk"];
    for (item in arrships) {
        let desk = document.getElementById(item);
        setCellSize(desk);
        var parent = desk.parentElement;
        if (parent.id == "ship-container") {
            parent.style.width = desk.offsetWidth + "px";
            parent.style.height = desk.offsetHeight + "px";
            parent.style.marginRight = (cell_size - 3) + "px";
        }

    }
}


function setCellSize(element) {
    element.style.border = borderTable + "px solid #402205";
    let tdpadding = (cell_size - 1) / 2;
    for (var i = 0; i < element.rows.length; i++) {
        for (var j = 0; j < element.rows[0].cells.length; j++) {
            let td = element.rows[i].cells[j];
            td.style.padding = tdpadding + "px";
            td.style.border = borderCell + "px solid";

        }
    }
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
                rotateShip(ship);
            }

        }
        document.getElementById('btn-ready').disabled = true;
        document.getElementById("player1-ready").src = "../img/ready.png";
        PlayerShips = null;
    }




}


window.onresize = function (event) {

    cell_size = cellSizeCount();
    //   tableCreate('place-battlefield');
    //  shipsCreate('place-ships');
    //  var t = document.getElementById('battlefield1');
    setCellSize(field);
    setShipsCellSize();
    fieldb = getElementBounds(field);
    for (key in arrships) {
        if (arrships[key].x != -1) {
            let ship = document.getElementById(key);
            ship.style.left = fieldb.left + arrships[key].x * cell_size + "px"
            ship.style.top = fieldb.top + arrships[key].y * cell_size + "px"
          //  document.getElementById('opponent-status').innerHTML = field.offsetLeft;
        }

    }

};