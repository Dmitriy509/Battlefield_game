var field = document.getElementById('battlefield1');
var vivod = document.getElementById('vivod');
//var k = 2;
var cellsize = 30 * 96 / 72;  //transform to pt


function dragShip(b) {
    //vivod.innerText = 'klick';
    // alert(event.target.id);
    var coords = getCoords(b);
    b.style.margin = 0 + 'px';
    var shiftX = event.pageX - coords.left;
    var shiftY = event.pageY - coords.top;

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

        if (x > field.offsetLeft - cellsize && x < field.offsetLeft + field.offsetWidth && y > field.offsetTop - cellsize && y < field.offsetTop + field.offsetHeight) {

            if (x > field.offsetLeft && x < field.offsetLeft + field.offsetWidth - cellsize * b.rows[0].cells.length) {
                lastX = field.offsetLeft + Math.round((x - field.offsetLeft) / cellsize) * cellsize;
                b.style.left = lastX + 'px';
            }
            else {
                b.style.left = field.offsetLeft + Math.round((x - field.offsetLeft) / cellsize) * cellsize + 'px';
            }

            if (y > field.offsetTop && y < field.offsetTop + field.offsetHeight - cellsize * b.rows.length) {
                lastY = field.offsetTop + Math.round((y - field.offsetTop) / cellsize) * cellsize;
                b.style.top = lastY + 'px';
            }
            else {
                b.style.top = field.offsetTop + Math.round((y - field.offsetTop) / cellsize) * cellsize + 'px';
            }
            //  vivod.innerText = Math.round((x - field.offsetLeft) / 30) * 30;
        }
        else {
            b.style.left = x + 'px';
            b.style.top = y + 'px';
        }

        //vivod.innerText = Math.round(40/30);

    }

    document.onmousemove = function (event) {
        // if (k == 2) {
        moveAt(event);
        //    k = 0;
        //} else {
        //    k++;
        //}
    };

    document.onmouseup = function (event) {
        document.onmousemove = null;
        document.onmouseup = null;
        b.style.left = lastX + 'px';
        b.style.top = lastY + 'px';
        if (!checkshipPosition(b)) {
            b.style.left = lastXChecking + 'px';
            b.style.top = lastYChecking + 'px';

            //  vivod.innerText = 'not ok';
        }
    };
    /*  b.onmouseup = function (event) {
         
          document.onmousemove = null;
          b.onmouseup = null;
        
          if (checkshipPosition(b)) {
              b.style.left = lastX + 'px';
              b.style.top = lastY + 'px';
  
              vivod.innerText = 'ok';
          } else
          {
              b.style.left = lastXChecking + 'px';
              b.style.top = lastYChecking + 'px';
  
              vivod.innerText = 'not ok';
          }
         
      };*/
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

function checkshipPosition(currentdesk) {

    let arr = ["singledesk1", "singledesk2", "singledesk3", "singledesk4", "doubledesk1", "doubledesk2", "doubledesk3", "tripledesk1", "tripledesk2", "fourdesk"];
    let curX = Math.round(currentdesk.offsetLeft / cellsize) - 1;
    let curY = Math.round(currentdesk.offsetTop / cellsize) - 1;
    let curXt = Math.round((currentdesk.offsetLeft + currentdesk.offsetWidth - cellsize) / cellsize) + 1;
    let curYt = Math.round((currentdesk.offsetTop + currentdesk.offsetHeight - cellsize) / cellsize) + 1;

    for (item of arr) {

        if (item == currentdesk.id) { continue; }



        let desk = document.getElementById(item);

        let x = Math.round(desk.offsetLeft / cellsize);
        let y = Math.round(desk.offsetTop / cellsize);

        let xt = Math.round((desk.offsetLeft + desk.offsetWidth - cellsize) / cellsize);
        let yt = Math.round((desk.offsetTop + desk.offsetHeight - cellsize) / cellsize);
        //if (item = 'fourdesk') {
        //    vivod.innerText = '<br/>x=' + x + ' xt=' + xt + ' y=' + y + ' yt' + yt + ' curX=' + curX + ' curXt=' + curXt + ' curY=' + curY + ' curYt=' + curYt + '/////';
        //}
        if ((curX <= x && x <= curXt) && (curY <= y && y <= curYt)) {
            //  alert('1');
            return false;
        }
        else if ((curX <= xt && xt <= curXt) && (curY <= yt && yt <= curYt)) {
            //  alert('2');
            return false;
        }


    }

    return true;

}

function rotateShip(ship) {
    // alert(ball.rows[0].cells.length);
    rotate();
    if (!checkshipPosition(ship)) {
        //alert('asdf');
        rotate();
        shakingShip(ship);
    }
    function rotate() {

        if (ship.rows.length > 1) { //из вертикального в гор
            let rowcount = ship.rows.length;
            if (ship.offsetLeft + rowcount * cellsize >= field.offsetLeft + field.offsetWidth) {
                shakingShip(ship);
                return;
            }


            for (let i = 1; i < rowcount; i++) {
                ship.deleteRow(0);
            }

            for (let i = 1; i < rowcount; i++) {
                let allRows = ship.getElementsByTagName("tr");
                let cell = allRows[0].insertCell(0);
                //  cell.style.backgroundColor = 'rebeccapurple';
            }

        }
        else if (ship.rows[0].cells.length >= 2) {

            let colcount = ship.rows[0].cells.length;
            if (ship.offsetTop + colcount * cellsize >= field.offsetTop + field.offsetHeight) {
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
                //cell.style.backgroundColor = 'rebeccapurple';
            }
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