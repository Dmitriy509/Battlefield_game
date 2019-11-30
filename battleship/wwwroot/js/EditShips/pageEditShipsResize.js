function cellSizeCount() {
    let place = document.getElementById("place-battlefield");
    let minDim = (place.offsetWidth < place.offsetHeight) ? place.offsetWidth : place.offsetHeight;
    //console.log("cellsize "+Math.floor((minDim - 9 * borderCell - 2 * borderTable) / 10) + 1);

   // if (window.devicePixelRatio>=1)
        return Math.floor((minDim - 9 * borderCell - 2 * borderTable) / 10) ;
   // else return Math.floor(((minDim - 9 * borderCell - 2 * borderTable) / 10) * window.devicePixelRatio) ;

   
}

function setShipsCellSize() {

    for (item in arrships) {
        let desk = document.getElementById(item);
        setCellSize(desk);
       
        var parent = desk.parentElement;

        if (parent.id == "ship-container") {
            //parent.style.width = (cell_size*5) + "px";
            //parent.style.height = desk.offsetHeight + "px";

            parent.style.width = desk.offsetWidth + "px"; 
            parent.style.height = desk.offsetHeight  + "px";
            parent.style.marginRight = (cell_size - 3) + "px";
        }

    }
}


function setCellSize(element) {
  //  element.style.border = borderTable + "px solid #402205";
    let count = element.children.length;
    

    if (count > 5) {
        element.style.width = (cell_size * 10 + 2 * borderTable) + "px";
        element.style.height = (cell_size * 10 + 2 * borderTable) + "px";
    }
    else {
        if (element.offsetWidth < element.offsetHeight) {
            element.style.width = (cell_size + 2 * (borderTable-borderCell)) + "px";
            element.style.height = (cell_size * count + 2 * (borderTable - borderCell)) + "px";
        }
        else {

            element.style.width = (cell_size * count + 2 * (borderTable - borderCell)) + "px";
            element.style.height = (cell_size + 2 * (borderTable - borderCell)) + "px";
        }
    }
    let deskcount = Math.floor(element.offsetWidth / cell_size);
    let firstcellheight = element.children[0].offsetHeight;
    for (var i = 0; i < count; i++) {
        let cell = element.children[i];
        if (firstcellheight ==0) {
            cell.style.border = borderCell + "px solid";
            let ii = Math.floor(i / 10);
            let jj = i % 10;



            if (ii == 0) cell.style.borderTop = borderTable + "px solid";
            if (jj == 0) cell.style.borderLeft = borderTable + "px solid";
            if (ii == deskcount - 1 || count < 10) cell.style.borderBottom = borderTable + "px solid";
            if (jj == deskcount - 1) cell.style.borderRight = borderTable + "px solid";

        }
            cell.style.width = (cell_size - 2*borderCell) + "px";
            cell.style.height = (cell_size - 2*borderCell) + "px";      
        }
        
    }
  



window.onresize = function (event) {

    cell_size = cellSizeCount();
    //   tableCreate('place-battlefield');
    //  shipsCreate('place-ships');
    //  var t = document.getElementById('battlefield1');
    setCellSize(field);
    setShipsCellSize();
    let fieldb = getElementBounds(field);
    for (key in arrships) {
        if (arrships[key].x != -1) {
            let ship = document.getElementById(key);
            ship.style.left = fieldb.left + arrships[key].x * cell_size  + "px"
            ship.style.top = fieldb.top + arrships[key].y * cell_size+ "px"
         
        }

    }

};