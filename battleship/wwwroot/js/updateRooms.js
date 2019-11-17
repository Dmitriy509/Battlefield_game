getInfoRooms();
let timerId = setInterval(function () { getInfoRooms(); }, 3000);


function getInfoRooms() {
    $.post("/Rooms/GetInfoRooms", { playername: login})
        .done(function (data) {
            document.getElementById('online-players').innerHTML = data.player_count+" игрока онлайн";
            document.getElementById('current-battles').innerHTML = data.game_count + " битв идёт";
            

       
            let table = document.getElementById('roomslist');
            let rowCount = table.rows.length;
  
            for (let i = 0; i < rowCount; i++) {
                table.deleteRow(0);
            }
           
            rowCount = data.roomnames.length;
           //  alert(data.player_count[0]);
      
            for (let i = 0; i < rowCount; i++) {
                let row = table.insertRow(0);
                let cell = row.insertCell(0);
                cell.classList.add("room-selection-name");
                cell = row.insertCell(1);
                cell.classList.add("room-selection-button");
                row.cells[0].innerHTML = data.roomnames[i];
                let h = "<form method='POST' action='/Rooms/EnterTheRoom'><input type = 'hidden' name = 'roomname' value = '" + data.roomnames[i] + "' /><input type = 'hidden' name = 'playername' value = '" + login + "' /><input type='image' name='take_challenge' src='../img/take_challenge.png'></form >"
                row.cells[1].innerHTML = h;

            }

            //$("#res").html(result_str);
            // console.log(data);
        });

}