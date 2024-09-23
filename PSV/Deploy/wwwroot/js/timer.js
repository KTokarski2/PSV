var timerOn = JSON.parse(document.getElementById("timerOn").innerText)
console.log(timerOn);
console.log(timerOn);

document.addEventListener("DOMContentLoaded", function () {
    
    let timerElement = document.getElementById('timer');
    let startTime;
    let timerInterval;
    
    function startTimer() {
        startTime = Date.now();
        timerInterval = setInterval(function () {
            let elapsedTime = Date.now() - startTime;
            let hours = Math.floor(elapsedTime / 3600000);
            let minutes = Math.floor((elapsedTime % 3600000) / 60000);
            let seconds = Math.floor((elapsedTime % 60000) / 1000);

            timerElement.textContent =
                ('0' + hours).slice(-2) + ':' +
                ('0' + minutes).slice(-2) + ':' +
                ('0' + seconds).slice(-2);
        }, 1000);
    }
    
    if (timerOn === true) {
        startTimer();
    }

    document.querySelector('.control-button-end').addEventListener('click', function () {
        clearInterval(timerInterval);
    }); 
});