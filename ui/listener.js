$(function() {
    /* open the UI for user */
    var id = 0;
    window.addEventListener('message', function(event) {
        if(event.data.type == "select"){
            $('#selectType').css('display', 'flex');
            id = event.data.serverid;
        } 
        else if (event.data.type == "c4"){
            $('#makeC4').css('display', 'block');
        }
        else if (event.data.type == "alarmHack"){
            $('#hackAlarm').css('display', 'block');
        }
        else if (event.data.type == "hackServer1"){
            $('#hackInside').css('display', 'flex');
            $('#hackServer1').css('display', 'block');
            setTimeout(() => {
                document.getElementById("serverHandler").innerText = "startServer1";
            }, 1000);
        }
        else if (event.data.type == "hackServer2"){
            $('#hackInside').css('display', 'flex');
            $('#hackServer2').css('display', 'block');
            setTimeout(() => {
                document.getElementById("serverHandler").innerText = "startServer2";
            }, 1000);
        }
        else if (event.data.type == "hackServer3"){
            $('#hackInside').css('display', 'flex');
            $('#hackServer3').css('display', 'block');
            setTimeout(() => {
                document.getElementById("serverHandler").innerText = "startServer3";
            }, 3500);
        }
        else if (event.data.type == "close"){
            $('#selectType').css('display', 'none');
        }
    });
    /*window.addEventListener("click", () => {
        SendData("quit");
    })*/
    /*document.getElementById("close").addEventListener("click", () => {
        SendData("quit");
    });*/
    var closeElements = document.getElementsByClassName("close");
    for (let index = 0; index < closeElements.length; index++) {
        var element = closeElements[index];
        element.addEventListener("click", () => {
            SendData("quit");
        });
    }
    document.getElementById("acceptHeist").addEventListener("click", () => {
        var type = document.getElementById("confirmText").innerText[0];
        SendData(`Accept-${type}`);
    });

    //C4
    var c4Number = document.getElementById("c4rng");
    var c4EnteredNumber = document.getElementById("c4task");
    var c4Btn = document.getElementById("counterContinue");
    c4Btn.addEventListener("click", () =>{
        //console.log("1-- " + c4Number.innerText + " 2-- " + c4EnteredNumber.value)
        if(c4Number.innerText != c4EnteredNumber.value){
            SendData("Boom");
        }
        else if (c4Btn.innerText == "Wyjście"){
            SendData("MakeC4");
        }
    })

    //Alarm Hacking
    var alarmState = document.getElementById("alarmState");
    var alarmInterval = setInterval(() => {
        if (alarmState.innerText == "0") { SendData("alarm-bad"); clearInterval(alarmInterval);}
        if (alarmState.innerText == "1") { SendData("alarm-good"); clearInterval(alarmInterval);}
    }, 2500);

    //Servers
    var server1State = document.getElementById("server1State");
    var server2State = document.getElementById("server2State");
    var server3State = document.getElementById("server3State");

    var serversInterval = setInterval(() => {
        if(server1State.innerText == "good") {SendData("server1-good"); server1State.innerHTML = "";}
        if(server1State.innerText == "bad") {SendData("server1-bad"); clearInterval(serversInterval);}
        if(server2State.innerText == "good") {SendData("server2-good"); server2State.innerHTML = "";}
        if(server2State.innerText == "bad") {SendData("server2-bad"); clearInterval(serversInterval);}
        if(server3State.innerText == "good") {SendData("server3-good"); server3State.innerHTML = "";}
        if(server3State.innerText == "bad") {SendData("server3-bad"); clearInterval(serversInterval);}
    }, 2500);


    function SendData(data, closeui = true){
        if(closeui == true) { 
            $('#selectType').css('display', 'none'); $('#makeC4').css('display', 'none'); $('#hackAlarm').css('display', 'none'); $('#hackInside').css('display', 'none');
            $('#hackServer1').css('display', 'none'); $('#hackServer2').css('display', 'none'); $('#hackServer3').css('display', 'none');}
        fetch(`https://${GetParentResourceName()}/maincallback`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json; charset=UTF-8',
            },
            body: JSON.stringify({
                callback: data,
                serverid: id
            })
        });
    }
});	