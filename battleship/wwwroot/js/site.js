function getShips() {
    let arr = ["singledesk1", "singledesk2", "singledesk3", "singledesk4", "doubledesk1", "doubledesk2", "doubledesk3", "tripledesk1", "tripledesk2", "fourdesk"];


    let Xcoords = new Array(20);
    let Ycoords = new Array(20);
    let index = 0;
    for (let j = 0; j < arr.length; j++) {
        let ship = document.getElementById(arr[j]);

        for (let k = j; k < arr.length - 1; k++) {
            if (!checkshipPosition(ship)) {
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




        let x = Math.round((ship.offsetLeft - field.offsetLeft) / cellsize);
        let y = Math.round((ship.offsetTop - field.offsetTop) / cellsize);

        for (let i = 0; i < deskcount; i++) {

            Xcoords[index] = x + i * ((flAlign == 1) ? 1 : 0);
            Ycoords[index] = y + i * ((flAlign == 1) ? 0 : 1);
            index++;

        }




    }

    $.post("/SetShips/GetShipsCoords", { playername: login, Xarr: Xcoords, Yarr: Ycoords })
        .done(function (data1) {
            document.getElementById('user1').innerText = login + "- Готов";
            document.getElementById('readybtn').disabled = true;
            // alert(data1.ff)
            // alert(data1.ss)
            //  window.location.href = 'GameView';

        });

    //  alert('posle');

}

//function successFunc(data, status) {
//    alert(data);
//}

//function errorFunc(errorData) {
//    alert('Ошибка' + errorData.responseText);
//}



function getInfo(row, col) {

    $.post("/Default/GetInfo", { row: row, col: col })
        .done(function (data) {

            var i = data[0];
            var j = data[1];
            document.getElementById('battlefield1').rows[i].cells[j].innerHTML = "Text";
            //$("#res").html(result_str);
            // console.log(data);
        });
}