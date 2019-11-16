function cellSizeCount() {
    let place = document.getElementById("place-battlefield");
    let minDim = (place.offsetWidth < place.offsetHeight) ? place.offsetWidth : place.offsetHeight;
    return Math.floor((minDim - 9 * borderCell - 2 * borderTable) / 10) + 1;
}

function setShipsCellSize() {
    //   let arr = ["singledesk1", "singledesk2", "singledesk3", "singledesk4", "doubledesk1", "doubledesk2", "doubledesk3", "tripledesk1", "tripledesk2", "fourdesk"];
    for (item in arrships) {
        let desk = document.getElementById(item);
        setCellSize(desk);
       
        var parent = desk.parentElement;

        if (parent.id == "ship-container") {
          //  document.getElementById('player2status').innerText = desk.offsetWidth + " " + desk.offsetHeight;

            parent.style.width = (cell_size*5) + "px";
            parent.style.height = desk.offsetHeight + "px";

            parent.style.width = desk.offsetWidth + "px"; 
            parent.style.height = desk.offsetHeight  + "px";
            parent.style.marginRight = (cell_size - 3) + "px";
        }

    }
}


function setCellSize(element) {
    element.style.border = borderTable + "px solid #402205";
    //  let tdpadding = (cell_size - borderCell) / 2;
    for (var i = 0; i < element.rows.length; i++) {
        for (var j = 0; j < element.rows[0].cells.length; j++) {
            let td = element.rows[i].cells[j];
            td.style.border = borderCell + "px solid";
            td.style.width = (cell_size - borderCell) + "px";
            td.style.height = (cell_size - borderCell) + "px";
      
          //  td.style.padding = tdpadding+'px';
          
      
        }
        
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
            ship.style.left = fieldb.left + arrships[key].x * cell_size  + "px"
            ship.style.top = fieldb.top + arrships[key].y * cell_size+ "px"
         
        }

    }

};