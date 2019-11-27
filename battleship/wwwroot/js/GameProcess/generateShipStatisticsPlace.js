function genPlaceShipStat(idPlace) {
    const place = document.getElementById(idPlace);
    clearContent(place);
    var arr = [];
    const ships4 = [shipMini(4)];
    const ships3 = [shipMini(3), shipMini(3)];
    const ships2 = [shipMini(2), shipMini(2), shipMini(2)];
    const ships1 = [shipMini(1), shipMini(1), shipMini(1), shipMini(1)];

    arr = ships4.concat(ships3, ships2, ships1);
    for (let ship of arr) {
        place.appendChild(ship);
    }

}
function shipMini(size) {
    const ship = document.createElement('div');

    ship.setAttribute("class", "ship-miniature");
    for (var i = 0; i < size; i++) {
        const ship_block = document.createElement('div');
        ship_block.setAttribute("class", "ship-block-stat");
        ship.appendChild(ship_block);
    }
    return ship;
}

function clearContent(elem) {
    while (elem.firstChild) {
        elem.removeChild(elem.firstChild);
    }
}