﻿function fireclick(td) {

    if (flagFire) {
        //alert(td.cellIndex + "  " + td.parentNode.rowIndex)
        console.log("fire!");
        flagFire = false;
        // let field = document.getElementById('battlefield2');
        // startTimer("timer-move", moveTime);
        $.post("/Game/Fire", { playername: login, x: td.cellIndex, y: td.parentNode.rowIndex })
            .done(function (data) {
                console.log("getfiredata!");
                let f = document.getElementById("battlefield2")
                for (let i = 0; i < data.cells.length; i++) {
                    console.log("res " + data.fireresult[i]);
                    switch (data.fireresult[i]) {
                        case 3:  //injured
                            injured(f.rows[data.rows[i]].cells[data.cells[i]]);
                            flagFire = true;
                            break;
                        case 4: //miss
                            miss(f.rows[data.rows[i]].cells[data.cells[i]]);
                            break;
                        default:

                            break;
                    }

                    updateShipPanel("opponent-ships-stat", data.shipcount);

                }
              
                setTime(data.movetime, "timer-move");
             //   document.getElementById('shipscount2').innerText = data.shipcount[0] + " - 1п, " + data.shipcount[1] + " - 2п, " + data.shipcount[2] + " - 3п, " + data.shipcount[3] + " - 4п, ";
                //if (data.gamestatus != "") {
                //    window.location.href = data.gamestatus;
                //}
            });

    }

}

function injured(td) {
    td.style.backgroundColor = "rebeccapurple";
    let canvas = document.createElement("canvas");
   // let csize = (td.offsetwidth-3)+'px';
  
   // let csize = 27 * 96 / 72;
       let csize = cell_size*0.9;
    //console.log("cell_size1 " + cell_size + "  " + td.style.width);
    canvas.width = csize;
    canvas.height = csize;

    context = canvas.getContext("2d");
    context.beginPath();
    context.moveTo(0, 0);
    context.lineTo(csize, csize);
    context.moveTo(0, csize);
    context.lineTo(csize, 0);
    context.strokeStyle = "red";
    context.stroke();
    context.closePath();
    td.appendChild(canvas);
    td.onclick = null;
}

function miss(td) {
    let canvas = document.createElement("canvas");
  //  let csize = 27 * 96 / 72;
    let csize = cell_size * 0.9;
    canvas.width = csize;
    canvas.height = csize;
    context = canvas.getContext("2d");
    context.beginPath();
    context.arc(csize / 2, csize / 2, 5, 0, 2 * Math.PI, false);
    context.fillStyle = 'black';
    context.fill();
    context.lineWidth = 1;
    context.strokeStyle = 'black';
    context.stroke();
    context.closePath();
    td.appendChild(canvas);
    td.onclick = null;
}

function updateBattleField(fieldname, field, flwithships)
{
  //  console.log("начало");
  //  console.log(field[0][0]);
   // console.log("this is length");
    let viewfield = document.getElementById(fieldname);
    console.log(viewfield.id);
    for (let i = 0; i < field.length; i++)
        for (let j = 0; j < field.length; j++) {
            // console.log("щас получим ячейку");
            let td = viewfield.rows[i].cells[j];
            if (td.childNodes.length > 0) continue;
            console.log("внутри фор");
            switch (field[i][j]) {
                case 2:  //ships
                    if (flwithships)
                    td.style.backgroundColor = "rebeccapurple";
                    break;
                case 3:  //injured
                    injured(td);
                    break;
                case 4: //miss
                    miss(td);
                    break;
                default:

                    break;
            }

        }
    console.log("конец");


}

function updateRoom() {



    // console.log("updateroom Begin1  " + (flagFire ? 1 : 2)+"   "+login);
    $.post("/Game/UpdateGameProcess", { playername: login, curmovestate: (flagFire ? 1 : 2) })
        .done(function (data) {
            console.log("time " + data.movetime);
           
            if (data.gamestatus == "") {
                setTime(data.movetime, "timer-move");
                if (data.movestate == 1) {
                    flagFire = true;
                    document.getElementById("player-move").style.opacity="1"; //move
                    document.getElementById("opponent-move").style.opacity = "0.3"; //not move
                }
                else if (data.movestate == 2) {
                    document.getElementById("opponent-move").style.opacity = "1";
                    document.getElementById("player-move").style.opacity = "0.3";
                    flagFire = false;
                }
                else {
                    document.getElementById("player-move").style.opacity = "0.3";
                    document.getElementById("opponent-move").style.opacity = "0.3";
                    flagFire = false;
                }
                //   console.log("щас будет дата");
                if (data.field != null) {
                  //  console.log("поле не нул");
                    console.log("получен массив  " + data.field.length);

                  //  document.getElementById('shipscount1').innerText = data.shipcount[0] + " - 1п, " + data.shipcount[1] + " - 2п, " + data.shipcount[2] + " - 3п, " + data.shipcount[3] + " - 4п, ";

                    let viewfield = document.getElementById("battlefield1");

                    console.log(viewfield.id);
                    for (let i = 0; i < data.field.length; i++)
                        for (let j = 0; j < data.field.length; j++) {
                            // console.log("щас получим ячейку");
                            let td = viewfield.rows[i].cells[j];
                            if (td.childNodes.length > 0) continue;
                            console.log("внутри фор");
                            switch (data.field[i][j]) {
                                case 3:  //injured
                                    injured(td);
                                    break;
                                case 4: //miss
                                    miss(td);
                                    break;
                                default:

                                    break;
                            }

                        }

                    updateShipPanel("player-ships-stat", data.shipcount);

                }
            //    document.getElementById('user2status').innerText = data.player2status;
            }
            if (data.gamestatus == "results") {
                disableButton(document.getElementById("giveupbtn")); 
               // document.getElementById("giveupbtn").disabled = true;
                clearInterval(timerId)
                flagFire = false;
                if (data.gameresult == "win")
                    document.getElementById('gamestatus').src = "../img/label_win.png";
                else
                    document.getElementById('gamestatus').src = "../img/label_defeat.png";

                openResultModal("result-modal");
            }
               
        });
   // document.getElementById('vivod').innerHTML = flagFire;

}

function updateShipPanel(idShipsStat, shipsStatArr) {
    //  var ships = timerEl.querySelectorAll(".timer-animation span");
    let ships = document.querySelectorAll("#" + idShipsStat + " > div");
    ships.forEach(function (el) {
        sunkenShips(el.childNodes.length, el);
    });

    function sunkenShips(deskcount, el) {
        deskcount--;
        let shipscount = 4 - deskcount;
        if (shipsStatArr[deskcount] < shipscount) {
            el.style.backgroundColor = 'red';
            shipsStatArr[deskcount]++;
        }
    }
}

function fgiveupbtn(btn)
{
    disableButton(btn);  
    clearInterval(timerId);
    flagFire = false;
    document.getElementById('gamestatus').src =  "../img/label_defeat.png";  
    openResultModal('result-modal');

    $.post("/Game/GiveUp", { playername: login})
        .done(function (data) {

        });
    
    


}

function setTime(durationSec, timerId) {

    // id = "timer-move";
    var timer_label = document.querySelector("#" + timerId + " > label");
    timer_label.textContent = secondsStrFormat(durationSec);
    //    console.log(timer_label.textContent);
}