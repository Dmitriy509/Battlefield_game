//updateRoom();
let timerId = setInterval(function () { updateRoom(); }, 1000);
function updateRoom() {

    // alert('asdf');
    // console.log(login);

    $.post("/SetShips/UpdateInfoRoom", { playername: login })
        .done(function (data) {
            //  console.log("ответ запроса");
            document.getElementById('player2status').innerText = data.player2status;
            if (data.gamestatus != "") {
                //  console.log(data.gamestatus);
                window.location.href = data.gamestatus;

            }
        });
    console.log(login);
}

function getShips() {
    let arr = ["singledesk1", "singledesk2", "singledesk3", "singledesk4", "doubledesk1", "doubledesk2", "doubledesk3", "tripledesk1", "tripledesk2", "fourdesk"];


    let Xcoords = new Array(20);
    let Ycoords = new Array(20);
    let index = 0;
    for (let j = 0; j < arr.length; j++) {
        let ship = document.getElementById(arr[j]);

        for (let k = j; k < arr.length - 1; k++) {
            if (!checkshipPosition(ship, true)) {
                alert('Кораблики неправильна!');
                return;
            }
        }

        let deskcount = 1;
        let flAlign = 0 //vert-0, gor-1
        if (ship.rows.length > ship.rows[0].cells.length) {
            deskcount = ship.rows.length; flAlign = 0;
        }
        else {
            deskcount = ship.rows[0].cells.length;
            flAlign = 1;
        }
        // alert(deskcount + "  " + flAlign);
        //let rowcount = ship.rows.length;
        //let colcount = ship.rows[0].cells.length;




        let x = Math.round((ship.offsetLeft - field.offsetLeft) / cell_size);
        let y = Math.round((ship.offsetTop - field.offsetTop) / cell_size);

        for (let i = 0; i < deskcount; i++) {

            Xcoords[index] = x + i * ((flAlign == 1) ? 1 : 0);
            Ycoords[index] = y + i * ((flAlign == 1) ? 0 : 1);
            index++;

        }




    }

    $.post("/SetShips/GetShipsCoords", { playername: login, Xarr: Xcoords, Yarr: Ycoords })
        .done(function (data1) {
          //  document.getElementById('user1').innerText = login + "- Готов";
            document.getElementById('btn-ready').disabled = true;
            document.getElementById("player1-ready").src = "../img/ready.png";
            // alert(data1.ff)
            // alert(data1.ss)
            //  window.location.href = 'GameView';

        });

    //  alert('posle');

}