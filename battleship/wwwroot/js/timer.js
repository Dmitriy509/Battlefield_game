function timerAnimationSetting(timerEl, duration) {
    //console.log(">>> " + duration);
    var spans = timerEl.querySelectorAll(".timer-animation span");
    spans.forEach(function (el) {
        const widthParent = el.parentElement.offsetWidth;
        el.style.borderWidth = widthParent + "px";
        el.style.animationDuration = duration + "s";
    });

}

function timerAnimationStop(timerEl) {
    //console.log(">>> " + duration);
    var spans = timerEl.querySelectorAll(".timer-animation span");
    spans.forEach(function (el) {
        // el.style.animationPlayState = "paused";
        el.style.animationDuration = "0s";
    });

}


function startTimer(id, stopFunction, mainWorkFunction) {

    let timer_label = document.querySelector("#" + id + " > label");
    let timerMode = document.querySelector("#" + id + " > input");
    let regexp = /(\d):(\d\d)/;
    let match = regexp.exec(timer_label.textContent);
    
    let current_timer = parseInt(match[1], 10) * 60 + parseInt(match[2], 10);

    let timerEl = document.getElementById(id);
    let parent = timerEl.parentElement;
    if (parent.classList.contains("timer-group")) timerAnimationSetting(parent, current_timer);
    //let timerMode = document.getElementById("timerMode");



    let startInterval = setInterval(function () {
        mainWorkFunction(false);
        let match = regexp.exec(timer_label.textContent);
        if (timerMode.value == 'stop') {

            if (parent.classList.contains("timer-group"))
                timerAnimationStop(parent)

            clearInterval(startInterval);
            return;
        }
        current_timer = current_timer == 0 ? stopTimer() : parseInt(match[1], 10) * 60 + parseInt(match[2], 10) - 1;
        timer_label.textContent = secondsStrFormat(current_timer);
        
    }, 1000);


    var stopTimer = function () {
        clearInterval(startInterval);
       // console.log("stopTimer");
        stopFunction();
        return 0;
    }
} 

function timerMove(timerId, modalId) {
    startTimer(timerId, endOfTime, update);
}

function stopTimer(idTimer)
{
    //var timer_label = document.querySelector("#" + id + " > label");
    let timerMode = document.querySelector("#" + idTimer + " > input");
    timerMode.value = 'stop';
}

//////////////////////////////////
function endOfTime()
{
    disableButton(document.getElementById('replaybtn')); 
    update(true);
}


function update(timeisup) {

    console.log("!!!!!!!!!! " + timeisup);

    $.post("/GameResults/UpdateGameResultView", { playername: login, fltimeisup: timeisup })
        .done(function (data) {
          //  player2statusR.innerText = data.player2status;
            idTimer = 'timer-decision';
            switch (data.player2status) {
                case "wait": break;
                case "ready":
                    setBtnStatus("opponent-name", "ready");
                    console.log("ready status notall");
                    if (ready) {
                        console.log("ready status all");
                        stopTimer(idTimer);
                        window.location.href = "/setships/FieldEditorView";
                    }
                    break;               
                case "exit":
                    setBtnStatus("opponent-name", "exit");
                    disableButton(document.getElementById('replaybtn'));
                    stopTimer(idTimer);
                    break;

            }

        });
}




//function aaa() {

//    stopTimer("timer-decision");
//}

//function timerAnimationSetting(timerEl, duration) {
//    var spans = timerEl.querySelectorAll(".timer-animation span");
//    spans.forEach(function (el) {
//        const widthParent = el.parentElement.offsetWidth;
//        el.style.borderWidth = widthParent + "px";
//        el.style.animationDuration = duration + "s";
//    });

//}


//function startTimer(id_label, durationSec) {
//    var timer_label = document.querySelector("#" + id_label + " > label");
//    timer_label.textContent = secondsStrFormat(durationSec);
//}







//function endOfTime()
//{
//    btn.disabled = true;
//    clearInterval(timerId)
//    flagFire = false;
//    document.getElementById('gamestatus').src = src = "../img/label_win.png";
//    openResultModal('result-modal');
//    $.post("/Game/GiveUp", { playername: login })
//        .done(function (data) {

//        });
//}


//function timerChallenge(timerId, durationSec, btnElem, statusElem) {
//    startTimer(timerId, durationSec, function () {
//        statusElem.setAttribute("src", "../img/reject.png");
//        disableButton(btnElem);
//    })
//}

function setTime(durationSec, timerId) {

   // id = "timer-move";
    var timer_label = document.querySelector("#" + timerId + " > label");
    timer_label.textContent = secondsStrFormat(durationSec);
//    console.log(timer_label.textContent);
}

function secondsStrFormat(secs) {
    let minutes = parseInt(secs / 60, 10);
    let seconds = parseInt(secs % 60, 10);
    return minutes + ":" + (seconds < 10 ? "0" + seconds : seconds);
}