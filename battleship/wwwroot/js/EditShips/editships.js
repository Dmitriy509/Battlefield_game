function dragShip(b) {
  

    var coords = getCoords(b);
    b.style.margin = 0 + 'px';
    var shiftX = event.pageX - coords.left;
    var shiftY = event.pageY - coords.top;

  //  alert(coords.left + " "+coords.top + " " + event.pageX + " " + event.pageY);

    var lastX = coords.left;
    var lastY = coords.top;

    var lastXChecking = coords.left;
    var lastYChecking = coords.top;
    b.style.position = 'absolute';
    // document.body.appendChild(b);       
    // moveAt(event);

    b.style.zIndex = 1000; // над другими элементами

    function moveAt(event) {
        let x = event.pageX - shiftX;
        let y = event.pageY - shiftY;

      //  document.getElementById('');

        if (x > fieldb.left - cell_size && x < fieldb.right && y > fieldb.top - cell_size && y < fieldb.bottom) {

            let curX = fieldb.left + Math.round((x - fieldb.left) / cell_size) * cell_size;
            let curY = fieldb.top + Math.round((y - fieldb.top) / cell_size) * cell_size;
            b.style.left = curX + 'px';
            b.style.top = curY + 'px';
            //let t1 = fieldb.right - b.offsetWidth;
            //let t2 = fieldb.bottom - b.offsetHeight;
            //document.getElementById('opponent-name').innerText =
            //    curX + ">=" + fieldb.left + " && " + curX + "<=" + t1 + " && " + curY + ">=" + fieldb.top + " && " + curY + "<=" + t2;
            if (curX >= fieldb.left && curX <= fieldb.right - b.offsetWidth && curY >= fieldb.top && curY <= fieldb.bottom - b.offsetHeight) {
                lastX = curX;
                lastY = curY;
            }
            //if (x >= fieldb.left && x <= fieldb.right - b.offsetWidth) {
            //    lastX = fieldb.left + Math.round((x - fieldb.left) / cell_size) * cell_size;
            //    b.style.left = lastX + 'px';
            //}
            //else {
            //    b.style.left = fieldb.left + Math.round((x - fieldb.left) / cell_size) * cell_size + 'px';
            //}

            //if (y >= fieldb.top && y <= fieldb.bottom - b.offsetHeight) {
            //    lastY = fieldb.top + Math.round((y - fieldb.top) / cell_size) * cell_size;
            //    b.style.top = lastY + 'px';
            //}
            //else {
            //    b.style.top = fieldb.top + Math.round((y - fieldb.top) / cell_size) * cell_size + 'px';
            //}


            //document.getElementById('player-name').innerText = lastX + " " + lastY;

           // document.getElementById('opponent-name').innerText = b.offsetWidth;
  
            //  vivod.innerText = Math.round((x - field.offsetLeft) / 30) * 30;
        }   
        else {
            b.style.left = x + 'px';
            b.style.top = y + 'px';
        }

        //vivod.innerText = Math.round(40/30);

    }

    document.onmousemove = function (event) {   
        moveAt(event);
    };

    function getshipcoords(ship)
    {
        if (ship.offsetLeft >= field.offsetLeft + field.offsetWidth) {
            arrships[ship.id].x = -1;
            arrships[ship.id].y = -1
        }
        else {
            arrships[ship.id].x = Math.round((ship.offsetLeft - field.offsetLeft) / cell_size);
            arrships[ship.id].y = Math.round((ship.offsetTop - field.offsetTop) / cell_size);
        }
    }


    document.onmouseup = function (event) {
        document.onmousemove = null;
        document.onmouseup = null;
        b.style.left = lastX + 'px';
        b.style.top = lastY + 'px';
        let bounds = getElementBounds(b);
      //  alert(el.left + " " + fieldb.left + " " + el.right + " " + fieldb.right + " cs " + cell_size);
        if (!checkshipPosition(bounds, b.id)) {
          //  alert("1!!!!");
            b.style.left = lastXChecking + 'px';
            b.style.top = lastYChecking + 'px';

        }
        else
        if (!checkFieldBounds(bounds, fieldb))
        {   
            
          //  let abc = getElementBounds(b);
            //     alert(abc.left + " " + fieldb.left + " " + abc.right + " " + fieldb.right + " cs " + cell_size);
          //  alert(b.style.left + " " + fieldb.left + " " + abc.right + " " + fieldb.right + " cs " + cell_size);

            var parent = b.parentElement;
          //  alert("2!! ");
            b.style.left = parent.style.left;
            b.style.top = parent.style.top;

            }

        getshipcoords(b);
    };
}

function getCoords(elem) {   // кроме IE8-
    var box = elem.getBoundingClientRect();
    return {
        top: box.top + pageYOffset,
        left: box.left + pageXOffset
    };
}

function dragstShip() {
    return false;
}

function getElementBounds(elem) {
    return {
        top: elem.offsetTop,
        bottom: elem.offsetTop + elem.offsetHeight,
        left: elem.offsetLeft,
        right: elem.offsetLeft + elem.offsetWidth
    };
}

function checkFieldBounds(deskb, fieldb) {

  //  alert(deskb.left + " " + fieldb.left + " " + deskb.right + " " + fieldb.right);

    if (deskb.left >= fieldb.left && deskb.right <= fieldb.right)
        if (deskb.top >= fieldb.top && deskb.bottom <= fieldb.bottom)
            return true;
        else false;

    }

function checkshipPosition(currentdesk, id) {


    let arr = ["singledesk1", "singledesk2", "singledesk3", "singledesk4", "doubledesk1", "doubledesk2", "doubledesk3", "tripledesk1", "tripledesk2", "fourdesk"];
    let curX = Math.round(currentdesk.left / cell_size) - 1;
    let curY = Math.round(currentdesk.top  / cell_size) - 1;
    let curXt = Math.round((currentdesk.right - cell_size) / cell_size) + 1;
    let curYt = Math.round((currentdesk.bottom - cell_size) / cell_size) + 1;
    // alert(curX + " " + curY + " " + curXt + " " + curYt + " /// " + currentdesk.right );
   // document.getElementById('player-name').innerText ="curx, curXt"+ curX + " " + curXt + " curY, curYt " + curY + " " + curYt + " /// " + currentdesk.right + "  " + cell_size;
  
  //  alert(curX + " " + curXt)

    for (item of arr) {

        if (item == id) { continue; }
       


        let desk = getElementBounds(document.getElementById(item));

        if (!checkFieldBounds(desk, fieldb)) continue;
  

        let x = Math.round(desk.left / cell_size);
        let y = Math.round(desk.top / cell_size);

        let xt = Math.round((desk.right - cell_size) / cell_size);
        let yt = Math.round((desk.bottom - cell_size) / cell_size);
       // alert(x + " " + y + " " + yt + " " + xt );
        //if (item = 'fourdesk') {
        //    vivod.innerText = '<br/>x=' + x + ' xt=' + xt + ' y=' + y + ' yt' + yt + ' curX=' + curX + ' curXt=' + curXt + ' curY=' + curY + ' curYt=' + curYt + '/////';
        //}
        if ((curX <= x && x <= curXt) && (curY <= y && y <= curYt)) {
          //  alert("1! curX, curXt=" + curX + " " + curXt + "curY, curYt=" + curY + " " + curYt);
            return false;
        }
        else if ((curX <= xt && xt <= curXt) && (curY <= yt && yt <= curYt)) {
           // alert("2! curX, curXt=" + curX + " " + curXt + "curY, curYt=" + curY + " " + curYt);
            return false;
        }


    }
    return true;

}

function rotateShip(ship) {

    let shipbounds = getElementBounds(ship);
    if (!checkFieldBounds(shipbounds, fieldb)) {
        shakingShip(ship);
        return;
    }

    rotate(ship);
    shipbounds = getElementBounds(ship);

    if (!checkshipPosition(shipbounds, ship.id)) {
        //alert('asdf');
        rotate(ship);
        shakingShip(ship);
    }
}

function rotate(ship) {

    let tdpadding = (cell_size - 1) / 2;

    if (ship.rows.length > 1) { //из вертикального в гор
        let rowcount = ship.rows.length;
        if (ship.offsetLeft + rowcount * cell_size >= fieldb.right) {
            shakingShip(ship);
            return;
        }


        for (let i = 1; i < rowcount; i++) {
            ship.deleteRow(0);
        }

        for (let i = 1; i < rowcount; i++) {
            let allRows = ship.getElementsByTagName("tr");
            let cell = allRows[0].insertCell(0);
            cell.style.padding = tdpadding + "px";
            cell.style.border = borderCell + "px solid";
            //  cell.style.backgroundColor = 'rebeccapurple';
        }

    }
    else if (ship.rows[0].cells.length >= 2) {

        let colcount = ship.rows[0].cells.length;
        if (ship.offsetTop + colcount * cell_size >= fieldb.bottom) {
            shakingShip(ship);
            return;
        }

        for (let i = 1; i < colcount; i++) {
            let allRows = ship.getElementsByTagName("tr");
            allRows[0].deleteCell(0);

        }
        for (let i = 1; i < colcount; i++) {
            let add = ship.insertRow(0);
            let cell = add.insertCell(0);
            cell.style.padding = tdpadding + "px";
            cell.style.border = borderCell + "px solid";
            //cell.style.backgroundColor = 'rebeccapurple';
        }
    }
}


function shakingShip(ship) {
    let a = 1;
    let range = 2;
    let repeatcount = 0;
    //  let x = ship.style.left;
    //   let y = ship.style.top;
    shake();
    function shake() {
        if (repeatcount == 8) return; else repeatcount++;
        if (a == 1) ship.style.top = parseInt(ship.style.top) + range + "px";
        else if (a == 2) ship.style.left = parseInt(ship.style.left) + range + "px";
        else if (a == 3) ship.style.top = parseInt(ship.style.top) - range + "px";
        else ship.style.left = parseInt(ship.style.left) - range + "px";
        if (a < 4) a++
        else a = 1;
        setTimeout(function () { shake(); }, 33);
    }

    //ship.style.left = x + "px";
    //ship.style.top = y + "px";

}