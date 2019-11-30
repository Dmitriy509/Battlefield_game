function getRandomInRange(min, max) {
    return Math.floor(Math.random() * (max - min + 1)) + min;
}
function coinToss() {
    return Math.floor(Math.random() * 2);
}

function buildmas(ships) {
    let i = 0;

    for (var key in arrships) {
        //ships = new Array(arrships.length);
        var deskcount = 1;
        if (key == "doubledesk1" || key == "doubledesk2" || key == "doubledesk3") deskcount = 2;
        else if (key == "tripledesk1" || key == "tripledesk2") deskcount = 3;
        else if (key == "fourdesk") deskcount = 4;


        ships[i] = setdata(deskcount);
        //	alert(ships[i].deskcount+" "+i+" "+key);
        i++;
    }
    function setdata() {
        return {
            name: key,
            deskcount: deskcount,
            align: -1,
            x: -1,
            y: -1
        };
    }

    return ships;
}

function fillmas(field) {
    field = new Array(10);
    for (var i = 0; i < 10; i++) {
        field[i] = new Array(10);
        for (var j = 0; j < 10; j++) {
            field[i][j] = 0;
        }
    }
    return field;
}


function randomPosition() {
    let field;
    field = fillmas(field);

    let ships = new Array(arrships.length);
    ships = buildmas(ships);
    ships = shuffle(ships);

    for (let i = 0; i < ships.length; i++) {
        let align = coinToss(); //0 - gor, 1 - vert
        ships[i].align = align;
        let x = getRandomInRange(0, align == 0 ? 9 - ships[i].deskcount + 1 : 9);
        let y = getRandomInRange(0, align == 1 ? 9 - ships[i].deskcount + 1 : 9);
        //console.log("shipmain "+i+" "+ships[i].name+" x"+x+" y:"+y);  

        let minc1;
        let maxc1;
        let c2;
        let flCheckCells = false;
        //console.log("shipmain "+i);
        let count = 0;
        while (!flCheckCells) {

            count++;
            //console.log("ship "+i+"   x:"+x+" y:"+y);
            if (count > 101) break;
            flCheckCells = true;
            if (align == 0) //gor
            {
                minc1 = x - 1;
                maxc1 = x + ships[i].deskcount;
                c2 = y;
                if (maxc1 - 1 <= 9) {
                    for (let j = x; j < maxc1; j++) {

                        if (field[c2][j] != 0) { flCheckCells = false; break; }
                    }
                } else flCheckCells = false;

            }
            else //vert
            {
                minc1 = y - 1;
                maxc1 = y + ships[i].deskcount;
                c2 = x;
                if (maxc1 - 1 <= 9) {
                    for (let j = y; j < maxc1; j++) {
                        if (field[j][c2] != 0) { flCheckCells = false; break; }
                    }
                } else flCheckCells = false;
            }

            if (!flCheckCells) {
                x++;
                if (x == 10) {
                    x = 0; y++;

                    if (y == 10) {
                        y = 0; x = 0;
                    }
                }

            }
            else {
                for (let j = minc1; j <= maxc1; j++) {
                    if (j < 0 || j > 9) continue;

                    if (c2 >= 0 && c2 <= 9) {
                        if (align == 0)
                            field[c2][j] = 2; else field[j][c2] = 2;
                    }
                    if (c2 - 1 >= 0 && c2 - 1 <= 9) {
                        if (align == 0)
                            field[c2 - 1][j] = 2; else field[j][c2 - 1] = 2;
                    }
                    if (c2 + 1 >= 0 && c2 + 1 <= 9) {
                        if (align == 0)
                            field[c2 + 1][j] = 2; else field[j][c2 + 1] = 2;
                    }

                }

                ships[i].x = x;
                ships[i].y = y;

            }

        }



    }



    setSipsPositions(ships);
}



function setSipsPositions(ships)
{
   // console.log("----------------");
    for (let i = 0; i < ships.length; i++) {
        let ship = document.getElementById(ships[i].name)
        ship.style.position = 'absolute';
    //    console.log(ships[i].x + " " + ships[i].y + " " + ships[i].name)
        ship.style.left = field.offsetLeft + ships[i].x * cell_size + 'px';
        ship.style.top = field.offsetTop + ships[i].y * cell_size + 'px';
        if (ships[i].align == 0) {
            if (ship.offsetHeight >= 2 * cell_size) {
                rotate(ship);  


            }
        }
        else {
            if (ship.offsetWidth >= 2 * cell_size) {
                rotate(ship);
            }
        }


        arrships[ship.id].x = ships[i].x;
        arrships[ship.id].y = ships[i].y;

    }

}

function shuffle(arr) {
    var j, temp;
    for (var i = arr.length - 1; i > 0; i--) {
        j = Math.floor(Math.random() * (i + 1));
        temp = arr[j];
        arr[j] = arr[i];
        arr[i] = temp;
    }
    return arr;
}