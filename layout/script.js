    var cell_size = 0;

    function clearContent(elem) { 
      while (elem.firstChild) {
        elem.removeChild(elem.firstChild); 
      }
    }

    function tableGen(x, y, borderCell, borderTable) {
      var tbl = document.createElement('table');
      tbl.style.border = borderTable + "px solid #402205";

      var tbdy = document.createElement('tbody');
      for (var i = 0; i < x; i++) {
        var tr = document.createElement('tr');
        for (var j = 0; j < y; j++) {
          var td = document.createElement('td');
          td.onclick = fight;
          td.style.padding = cell_size + "px";
          td.style.border = borderCell + "px solid";
          td.setAttribute("class", "blackouted");
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
      clearContent(place);
      const minDim = (place.offsetWidth < place.offsetHeight) ? place.offsetWidth : place.offsetHeight;

      cell_size = Math.floor((minDim - 9*borderCell - 2 * borderTable)/20);
      var tbl = tableGen(10,10,borderCell, borderTable);
      tbl.setAttribute("class", "table-battlefield");
      place.appendChild(tbl);
    }

    function shipsCreate(idPlaceShips) {
      const borderCell = 1; 
      const borderShip= 2;

      function cloneShip(shipN, n) {
        var arr = []
        for (i=0; i < n; i++) {
          var ship = tableGen(1,shipN,borderCell, borderShip);
          ship.style.backgroundColor = "#A9672C";
          arr.push(ship);
        } 
        return arr;
      }

      var place = document.getElementById(idPlaceShips);
      clearContent(place);

      var ships4 = cloneShip(4,1);
      var ships3 = cloneShip(3,2);
      var ships2 = cloneShip(2,3);
      var ships1 = cloneShip(1,4);
      var all_ships = ships4.concat(ships3, ships2,ships1);

      all_ships.forEach(function(el) {
        el.style.marginRight = (2*cell_size - 2)+"px";
        var div_wrapper = document.createElement('div');
        div_wrapper.appendChild(el);
        place.appendChild(div_wrapper);
        div_wrapper.style.width = div_wrapper.offsetWidth + "px";
      });

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

    function genPlaceShipStat(idPlace) {
      const place = document.getElementById(idPlace);
      clearContent(place);
      var arr = [];
      const ships4 = [shipMini(4)];
      const ships3 = [shipMini(3),shipMini(3)];
      const ships2 = [shipMini(2),shipMini(2), shipMini(2)];
      const ships1 = [shipMini(1),shipMini(1), shipMini(1), shipMini(1)];

      arr = ships4.concat(ships3, ships2, ships1);
      for (let ship of arr) {
        place.appendChild(ship);
      }

    }

    function secondsStrFormat(secs) {
        let minutes = parseInt(secs / 60, 10);
        let seconds = parseInt(secs % 60, 10);
        return minutes + ":" + (seconds < 10 ? "0" + seconds : seconds);
    }

    function startTimer(id, durationSec, stopFunction) {

      var parent = document.getElementById(id).parentElement;
      if (parent.classList.contains("timer-group")) timerAnimationSetting(parent, durationSec);

      var timer_label = document.querySelector("#" + id + " > label"); 

      let current_timer = durationSec;
      let regexp = /(\d):(\d\d)/;
      timer_label.textContent = secondsStrFormat(durationSec); 

      let startInterval = setInterval(function() {
        let match = regexp.exec(timer_label.textContent);

        current_timer = current_timer == 0 ? stopTimer() : parseInt(match[1],10)* 60 + parseInt(match[2],10) - 1;
        timer_label.textContent = secondsStrFormat(current_timer); 
      }, 1000);


      var stopTimer = function () {
        clearInterval(startInterval);
        stopFunction();
        return 0;
      }
    } 

    function timerMove(timerId, durationSec, modalId) {
      startTimer(timerId, durationSec, function() { openModalDefeat(modalId); });
    }

    function timerChallenge(timerId, durationSec, btnElem, statusElem) {
      startTimer(timerId, durationSec, function() {
        statusElem.setAttribute("src", "./img/reject.png");
        disableButton(btnElem);
      })
    }

    function timer(id, durationSec) {
      var parent = document.getElementById(id).parentElement;
      if (parent.classList.contains("timer-group")) timerAnimationSetting(parent, durationSec);

      var timer_label = document.querySelector("#" + id + " > label"); 

      let current_timer = durationSec;
      let regexp = /(\d):(\d\d)/;
      timer_label.textContent = secondsStrFormat(durationSec); 

      let startTimer = setInterval(function() {
        let match = regexp.exec(timer_label.textContent);

        current_timer = current_timer == 0 ? stopTimer() : parseInt(match[1],10)* 60 + parseInt(match[2],10) - 1;
        timer_label.textContent = secondsStrFormat(current_timer); 
      }, 1000);

      var stopTimer = function () {
        clearInterval(startTimer);
        return 0;
      }
    }

    function openModalDefeat(modalId) {
      openModal(modalId);
      timerChallenge("timer-decision", 15, document.getElementById("btn-again"), document.querySelector("#player-name > img") );
    }

    function openModal(modalId) {
      var modal = document.getElementById(modalId);
      var inputs = document.querySelectorAll("#" + modalId + "input[type='text']");
      inputs.forEach(function(el) {
        el.value = "";
      });
      modal.style.display = "flex";
    }

    function closeModal(modalId) {
      document.getElementById(modalId).style.display = "none";
    } 

    function timerAnimationSetting(timerEl, duration) {
      var spans = timerEl.querySelectorAll(".timer-animation span");
      spans.forEach(function(el) {
        const widthParent = el.parentElement.offsetWidth;
        el.style.borderWidth =  widthParent + "px";
        el.style.animationDuration = duration + "s";
      });

    }

    function readySet(id) {
      var imgReady = document.querySelector("#"+id+" > img");
      imgReady.setAttribute("src", "./img/ready.png");
    }

    function disableButton(el) {
      el.disabled = true;
      el.style.opacity = '0.5';
    }

    function fight(isMiss) {
      if (!this.firstChild) {
        if(isMiss) {
      var resultWrapper = document.createElement('div');
      resultWrapper.className="fight-result";
      var result = document.createElement('div');
      result.className= "miss";
      resultWrapper.appendChild(result);
      this.appendChild(resultWrapper); } else {
        
      }
    }
    }