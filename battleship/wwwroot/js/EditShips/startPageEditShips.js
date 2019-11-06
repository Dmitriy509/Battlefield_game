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




function cellSizeCount() {
    let place = document.getElementById("place-battlefield");
    let minDim = (place.offsetWidth < place.offsetHeight) ? place.offsetWidth : place.offsetHeight;
    return Math.floor((minDim - 9 * borderCell - 2 * borderTable) / 10) + 1;
}

function getElementBounds(elem) {
    return {
        top: elem.offsetTop,
        bottom: elem.offsetTop + elem.offsetHeight,
        left: elem.offsetLeft,
        right: elem.offsetLeft + elem.offsetWidth
    };
}

function test() {
    $.post("/Test/Ready", { playername: login })
        .done(function (data1) {
        });
}

