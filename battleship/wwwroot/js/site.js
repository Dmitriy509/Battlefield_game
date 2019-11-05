var cell_size = 0;

function tableGen(x, y, borderCell, borderTable) {
    var tbl = document.createElement('table');
    tbl.style.border = borderTable + "px solid #402205";

    var tbdy = document.createElement('tbody');
    for (var i = 0; i < x; i++) {
        var tr = document.createElement('tr');
        for (var j = 0; j < y; j++) {
            var td = document.createElement('td');
            td.style.padding = cell_size + "px";
            td.style.border = borderCell + "px solid";
            tr.appendChild(td)
        }
        tbdy.appendChild(tr);
    }
    tbl.appendChild(tbdy);
    return tbl;
}

function tableCreate(idTableWrapper) {
    const borderCell = 1;
    const borderTable = 2;
    const place = document.getElementById(idTableWrapper);

    while (place.firstChild) {
        place.removeChild(place.firstChild);
    }
    const minDim = (place.offsetWidth < place.offsetHeight) ? place.offsetWidth : place.offsetHeight;

    cell_size = Math.floor((minDim - 9 * borderCell - 2 * borderTable) / 20);
    var tbl = tableGen(10, 10, borderCell, borderTable);
    tbl.setAttribute("class", "table-battlefield");
    place.appendChild(tbl);
}

function shipsCreate(idPlaceShips) {
    const borderCell = 1;
    const borderShip = 2;

    function cloneShip(shipN, n) {
        var arr = []
        for (i = 0; i < n; i++) {
            arr.push(tableGen(1, shipN, borderCell, borderShip));
        }
        return arr;
    }

    var place = document.getElementById(idPlaceShips);
    while (place.firstChild) {
        place.removeChild(place.firstChild);
    }

    var ships4 = cloneShip(4, 1);
    var ships3 = cloneShip(3, 2);
    var ships2 = cloneShip(2, 3);
    var ships1 = cloneShip(1, 4);
    var all_ships = ships4.concat(ships3, ships2, ships1);

    all_ships.forEach(function (el) {
        el.style.marginRight = (2 * cell_size - 2) + "px";
        place.appendChild(el);
    });

}