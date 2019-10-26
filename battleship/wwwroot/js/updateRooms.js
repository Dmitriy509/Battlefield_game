getInfoRooms();
let timerId = setInterval(function () { getInfoRooms(); }, 700);


function getInfoRooms() {

    let login = get_cookie("Login");
    if (login == null) {
        window.location.href = 'Login';
        return;
    }

    $.post("/Default/GetInfoRooms", {})
        .done(function (data) {
            // alert(data.roomnames.length);
            let table = document.getElementById('roomslist');
            let rowCount = table.rows.length;

            for (let i = 0; i < rowCount; i++) {
                table.deleteRow(i);
            }
            rowCount = data.roomnames.length;
            // alert(data.player_count[0]);

            for (let i = 0; i < rowCount; i++) {
                let row = table.insertRow(0);
                let cell = row.insertCell(0);
                cell = row.insertCell(1);
                cell = row.insertCell(2);
                row.cells[0].innerHTML = data.RoomNames[i];
                row.cells[1].innerHTML = data.Player_Count[i];
                //   alert(data.roomnames[i]);
                let h = "<form method='POST' action='/Default/EnterTheRoom'><input type = 'hidden' name = 'roomname' value = '" + data.roomnames[i] + "' /><input type = 'hidden' name = 'playername' value = '" + login + "' /><input type='submit' " + (data.player_count[i] == 1 ? "" : "disabled") + " value='+' /></form >"
                row.cells[2].innerHTML = h;

            }

            //$("#res").html(result_str);
            // console.log(data);
        });

}