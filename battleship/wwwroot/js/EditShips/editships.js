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
    fieldb = getElementBounds(field);
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

            if (curX >= fieldb.left && curX <= fieldb.right - (b.offsetWidth - (borderTable * 2 - borderCell)) && curY >= fieldb.top && curY <= fieldb.bottom - (b.offsetHeight - (borderTable * 2 - borderCell))) {
                lastX = curX;
                lastY = curY;
            }

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

        if (ship.offsetLeft >= fieldb.right) {
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
       //     alert("1!!!!");
            b.style.left = lastXChecking + 'px';
            b.style.top = lastYChecking + 'px';

        }
        else
        if (!checkFieldBounds(bounds, fieldb))
        {   
            
            let abc = getElementBounds(b);
        //    alert(abc.left + " " + fieldb.left + " " + abc.right + " " + fieldb.right + " cs " + cell_size);
          //  alert(b.style.left + " " + fieldb.left + " " + abc.right + " " + fieldb.right + " cs " + cell_size);

            var parent = b.parentElement;
           // alert("2!! ");
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
        bottom: elem.offsetTop + elem.offsetHeight - (borderTable * 2 - borderCell),
        left: elem.offsetLeft,
        right: elem.offsetLeft + elem.offsetWidth - (borderTable * 2 - borderCell)
    };
}

function checkFieldBounds(deskb, fieldb) {

    //alert(deskb.left + " " + fieldb.left + " " + deskb.right + " " + fieldb.right);

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
 //   document.getElementById('player-name').innerText ="curx, curXt"+ curX + " " + curXt + " curY, curYt " + curY + " " + curYt + " /// " + currentdesk.right + "  " + cell_size;
  
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
         //   alert("1! curX, curXt=" + curX + " " + curXt + "curY, curYt=" + curY + " " + curYt);
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
        //alert("222");
        shakingShip(ship);

        return;
    }

    rotate(ship);
    shipbounds = getElementBounds(ship);

    if (!checkshipPosition(shipbounds, ship.id)) {
      //  alert('asdf');
        rotate(ship);
        shakingShip(ship);
    }
}

function rotate(ship) {

    //borderwidth: top right bottom left
    let deskcount = ship.children.length;
    if (ship.offsetWidth >= 2 * cell_size) { //из гор в верт
        if (ship.offsetTop + deskcount * cell_size >= fieldb.bottom) {
            shakingShip(ship);
            return;
        }




        for (let i = 0; i < deskcount; i++) {
            if (i == 0) ship.children[i].style.borderWidth = borderTable + "px " + borderTable + "px " + borderCell + "px " + borderTable + "px";
            else
                if (i == deskcount - 1) ship.children[i].style.borderWidth = borderCell + "px " + borderTable + "px " + borderTable + "px " + borderTable + "px";
            else ship.children[i].style.borderWidth = borderCell + "px " + borderTable + "px " + borderCell + "px " + borderTable + "px";
        }
        ship.style.width = (cell_size + 2 * borderTable) + "px";
        ship.style.height = (cell_size * deskcount + 2 * borderTable) + "px";
    }
    else if (ship.offsetHeight >= 2 * cell_size) {


        if (ship.offsetLeft + deskcount * cell_size >= fieldb.right) {

            shakingShip(ship);
            return;
        }
     

       
        for (let i = 0; i < deskcount; i++) {
            if (i == 0) ship.children[i].style.borderWidth = borderTable + "px " + borderCell + "px " + borderTable + "px " + borderTable + "px";
            else
                if (i == deskcount - 1) ship.children[i].style.borderWidth = borderTable + "px " + borderTable + "px " + borderTable + "px " + borderCell + "px";
            else ship.children[i].style.borderWidth = borderTable + "px " + borderCell + "px " + borderTable + "px " + borderCell + "px";
        }
        ship.style.width = (cell_size * deskcount + 2 * borderTable) + "px";
        ship.style.height = (cell_size + 2 * borderTable) + "px";

    }
}


function shakingShip(ship) {
    let a = 1;
    let range = 2;
    let currepeatcount = 0;
    let repeatcount = 8;
    //let x = parseInt(ship.style.left);
    //let y = parseInt(ship.style.top);
    shake();
    function shake() {
        if (currepeatcount == repeatcount) {        
            //ship.style.left = x+'px';
            //ship.style.top = y+'px';     
            return;
        }
            else currepeatcount++;

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