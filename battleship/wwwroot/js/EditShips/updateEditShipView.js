
function updateRoom() {

    $.post("/SetShips/UpdateInfoRoom", { player_id: player_id })
        .done(function (data) {
            //  console.log("ответ запроса");
            if (data.player2name == "")
                document.getElementById('opponent-name').innerText = "нет соперника";
            else
                document.getElementById('opponent-name').innerText = data.player2name;

         //   console.log("status - " + data.player2status + "  name - " + data.player2name);

            document.getElementById('player2status').innerHTML = data.player2status;
            if (data.player2status == "Ожидаем игрока" || data.player2status == "Поиск соперника") {           
                document.getElementById("player2-ready").src = "../img/not-ready.png";               
            }
            else
                if (data.player2status == "Готов") {
                    document.getElementById("player2-ready").src = "../img/ready.png";
                }

            if (data.gamestatus != "") {
                //  console.log(data.gamestatus);
                window.location.href = data.gamestatus;

            }
        });

}

function getShips() {
    let arr = ["singledesk1", "singledesk2", "singledesk3", "singledesk4", "doubledesk1", "doubledesk2", "doubledesk3", "tripledesk1", "tripledesk2", "fourdesk"];
    let fieldb = getElementBounds(field);
    let Xcoords = new Array(20);
    let Ycoords = new Array(20);
    let index = 0;
    for (let j = 0; j < arr.length; j++) {
        let ship = document.getElementById(arr[j]);
        let shipbounds = getElementBounds(ship); 
        if (!checkFieldBounds(shipbounds, fieldb)) {
            alert('Расставьте корабли!');
            return;
        }


        for (let k = j; k < arr.length - 1; k++) {
            if (!checkshipPosition(shipbounds, ship.id)) {
                alert('Расставьте корабли!');
                return;
            }
        }

        let deskcount  = ship.children.length;
        let flAlign = 0 //vert-0, hor-1
    
        if (ship.offsetWidth >= 2 * cell_size) {
             flAlign = 1;
        }
        else {
            flAlign = 0;
        }

        let x = Math.round((ship.offsetLeft - field.offsetLeft) / cell_size);
        let y = Math.round((ship.offsetTop - field.offsetTop) / cell_size);

        for (let i = 0; i < deskcount; i++) {

            Xcoords[index] = x + i * ((flAlign == 1) ? 1 : 0);
            Ycoords[index] = y + i * ((flAlign == 1) ? 0 : 1);
            index++;

        }




    }

    $.post("/SetShips/GetShipsCoords", { player_id: player_id, Xarr: Xcoords, Yarr: Ycoords })
        .done(function (data1) {
          //  document.getElementById('user1').innerText = login + "- Готов";
            disableButton(document.getElementById('btn-ready'));
            disableButton(document.getElementById('btn-random'));
            document.getElementById("player1-ready").src = "../img/ready.png";
            disableMoveShips();
            //  window.location.href = 'GameView';

        });

    function disableMoveShips() {
        //   let arr = ["singledesk1", "singledesk2", "singledesk3", "singledesk4", "doubledesk1", "doubledesk2", "doubledesk3", "tripledesk1", "tripledesk2", "fourdesk"];
        for (item in arrships) {
            let desk = document.getElementById(item);
            desk.onmousedown = null;
            desk.ondblclick = null;

        }
    }

}