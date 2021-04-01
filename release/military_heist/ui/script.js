window.onload = main;

function main(){
    var silent = document.getElementById("silentHeist");
    var loud = document.getElementById("loudHeist");
    var confirm = document.getElementById("confirmSelect");
    var confirmText = document.getElementById("confirmText");
    var accept = document.getElementById("acceptHeist");
    var deny = document.getElementById("denyHeist");

    loud.addEventListener("click", () =>{
        confirm.removeAttribute("hidden");
        setTimeout(() => {
            confirm.style.opacity = "1";
            confirmText.innerText = "1. Uwaga wybierasz głośny napad który charakteryzuje się mniejszą szansą na spierdolenie go w trakcie oraz nieco mniejszym łupem.";
        }, 500);
    })
    silent.addEventListener("click", () =>{
        confirm.removeAttribute("hidden");
        setTimeout(() => {
            confirm.style.opacity = "1";
            confirmText.innerText = "2. Uwaga wybierasz cichy napad który charakteryzuje się większą szansą na spierdolenie go w trakcie oraz większym łupem łupem.";
        }, 500);
    })

    deny.addEventListener("click", () => {
        confirm.style.opacity = "0";
        setTimeout(() => {
            confirm.setAttribute("hidden", "true");
        }, 2000);
    })

    /**/

    var counterContinue = document.getElementById("counterContinue");
    //var rngs = document.getElementsByClassName("rng");
    var c4rng = document.getElementById("c4rng");
    var c4task = document.getElementById("c4task");
    var c4header = document.getElementById("c4header");

    DoRng();
    function DoRng(){
        var rng = Math.floor(Math.random() * (476 - 1)) + 1;
        c4rng.innerText = rng;
    }

    var firsttaskcouner = 6;
    counterContinue.addEventListener("click", () => {
        if(firsttaskcouner == 0){
            c4task.remove();
            c4rng.remove();
            c4header.innerText = "Brawo udało Ci się stworzyć bombe !"
            counterContinue.innerText = "Wyjście";
        }
        else {
            DoRng();
            firsttaskcouner--;
        }
    });

    /**/

    var alarmState = document.getElementById("alarmState");
    var alarmSecurityQuestion = document.getElementById("alarmSecurityquestion");
    var alarmTerminal = document.getElementById("alarmTerminal");
    var acceptQuestion = document.getElementById("acceptQuestion");
    var terminalSendedCommands = document.getElementById("terminalSendedCommands");
    var alarmTimer = document.getElementById("alarmTimer");
    var sq1 = document.getElementById("sq1");
    var sq2 = document.getElementById("sq2");
    var sq3 = document.getElementById("sq3");
    var sq4 = document.getElementById("sq4");
    var time = 20;
    alarmSecurityQuestion.addEventListener("mouseenter", () => {
        alarmIntervalEnabled = true;
    })
    var alarmIntervalEnabled = false;
    var alarmInterval = setInterval(() => {
        if (!alarmIntervalEnabled) return;
        alarmTimer.innerText = `Czas na odopwiedź : ${time}s`
        time--;
        if(time == -1) {clearInterval(alarmInterval); alarmState.innerText = "0"; alarmIntervalEnabled = false; }
    }, 950);
    acceptQuestion.addEventListener("click", () => {
        if(sq1.checked && !sq2.checked && sq3.checked && !sq4.checked && time > 0){
            alarmSecurityQuestion.style.display = "none";
            alarmTerminal.style.display = "block";
            clearInterval(alarmInterval)
        }
        else {
            alarmState.innerText = "0";
        }
    });
    var terminalArrow = document.getElementById("terminalArrow");
    setInterval(() => {
        terminalArrow.focus();
    }, 50)

    var currentDir = "/home";
    var dir1 = "/home"; var dir1_ = ["/home/maintenancework"]; dir1__ = ["readme.txt", "maintenancework.exe", "alarmhandler.dll"];
    var mworksactivated = false; var handlerdeleted = false;
    terminalArrow.addEventListener("keydown", (e) => {
        if(e.key == "Enter"){
            /*setTimeout(() => {
                
            }, 10);*/
            terminalSendedCommands.innerText += `\n ${terminalArrow.value}`;

            //var currentDir = "/home";
            
            //var dir2 = "/drivers"; var dir2_ = ["/drivers/alarmdriver"]; var dir2__ = ["alarm.dll", "alarmhandler.dll", "antihack.dll", "cameras.dll", "cloudsaving.dll"];
            var emptydirs = ["/home/topsecret", "/home/system69", "/home/windowsxpcode", "/home/cyberpunksourcecode", "/home/php", "/home/bestpranks"];

            var command = terminalArrow.value.split(" ")[0];
            var parameters = [terminalArrow.value.split(" ")[1], terminalArrow.value.split(" ")[2]];
            //console.log(command);


            if(command == "help"){
                terminalSendedCommands.innerText += `\n Commands: \n cd: changes current directroy \n cls: clears console \n dir: display files and folders in current directroy \n exit: quits terminal help: shows this message \n open: opens file \n del: deletes file`
            }
            else if(command == "cls"){
                terminalSendedCommands.innerText = "";
            }
            else if(command == "cd"){
                if(currentDir == "/home" || true){
                    for (let index = 0; index < emptydirs.length; index++) {
                        var element = emptydirs[index];
                        //console.log("p1: " + parameters[0] + "p2: " + parameters[1])
                        if(element.includes(parameters[0])) currentDir = element
                    }
                    if(parameters[0].includes("maintenancework")) currentDir = "/home/maintenancework";
                    terminalSendedCommands.innerText += `\n Current directory ${currentDir}`;
                    //console.log(currentDir);
                }
            }
            else if(command == "cd.."){
                if(currentDir.length < 6) return;
                //if(currentDir.includes("drivers")) currentDir = "/drivers";
                if(currentDir.includes("home")) currentDir = "/home";
                terminalSendedCommands.innerText += `\n Current directory ${currentDir}`;
            }
            else if(command == "dir"){
                for (let index = 0; index < emptydirs.length; index++) {
                    var element = emptydirs[index];
                    if(currentDir == element) {terminalSendedCommands.innerText += `\n Nothing in curret directory : ${currentDir}`; break;}
                }
                if(currentDir == "/home"){
                    var files = "";
                    for (let index = 0; index < emptydirs.length; index++) {
                        var element = emptydirs[index];
                        files += ` ${element} \n`;
                        if(index == 2) files += "/home/maintenancework \n"
                    }
                    terminalSendedCommands.innerText += `\n Files in current directory : \n ${files}`;
                }
                else if (currentDir == "/home/maintenancework"){
                    var files = "";
                    for (let index = 0; index < dir1__.length; index++) {
                        var element = dir1__[index];
                        files += ` ${element} \n`;
                    }
                    terminalSendedCommands.innerText += `\n Files in current directory : \n ${files}`;
                }
            }
            else if (command == "open"){
                if(currentDir == "/home/maintenancework" && parameters[0] == "readme.txt"){
                    //var rng = Math.floor(Math.random() * (4 - 2)) + 2;
                    terminalSendedCommands.innerText += `\n Opening readme.txt...`;
                    //setTimeout(() => {
                        terminalSendedCommands.innerText += `\n Content of readme.txt: \n Hey new worker ! Thats a simple guide how to disable alarm for maintenance work,`
                        terminalSendedCommands.innerText += ` first at all run the maintenancework.exe file than remove the alarmhandler.dll, thats all.`;
                    //}, 2000);
                }
                if(currentDir == "/home/maintenancework" && parameters[0] == "maintenancework.exe"){
                    terminalSendedCommands.innerText += `\n Activating maintenance works...`;
                    //var rng = Math.floor(Math.random() * (4 - 2)) + 2;
                    //setTimeout(() => {
                        mworksactivated = true;
                        terminalSendedCommands.innerText += `\n Maintenance works activated !`;
                    //}, rng * 1000);
                }
                else{terminalSendedCommands += "\n Error";}
            }
            else if (command == "del"){
                if(currentDir == "/home/maintenancework" && parameters[0] == "alarmhandler.dll"){
                    if(handlerdeleted) return;
                    terminalSendedCommands.innerText += `\n alarmhandler.dll deleted !`;
                    handlerdeleted = true;
                    dir1__.length = 2;
                    if(!mworksactivated){
                        terminalSendedCommands.innerText = "Intruders detected !!!";
                        terminalArrow.setAttribute("disabled" , "true");
                        alarmState.innerText = "0";
                    }
                    else {
                        alarmState.innerText = "1";
                    }
                }
            }
            else{
                terminalSendedCommands.innerText += "\n bad command or parameters !";
            }

            terminalArrow.value = "";
        }
    })
    /*var x = 0;
    setInterval(() => {
        if(x > 25) return;
        x++;
        RenderObstacles();
    }, 100)
    function RenderObstacles(){
        var obstacle = document.createElement("div");
        obstacle.classList.add("obstacle");
        var width = Math.floor(Math.random() * (100 - 5)) + 5;
        var height = Math.floor(Math.random() * (550 - 15)) + 15;
        obstacle.style.width = width;
        obstacle.style.height = height;
        obstacle.style.position = "absolute";
        var toprng = Math.floor(Math.random() * (100 - 1)) + 1;
        var leftrng = Math.floor(Math.random() * (100 - 1)) + 1;
        obstacle.style.top = `${toprng}%`
        obstacle.style.left = `${leftrng}%`
        if(leftrng > 85 && width > 100) return;
        if(leftrng > 95) return;
        if(leftrng < 10) return
        document.getElementById("alarmFirewall").appendChild(obstacle);
    }*/

    /**/

    var timer1 = document.getElementById("hackTimer1");
    var timer2 = document.getElementById("hackTimer2");
    var timer3 = document.getElementById("hackTimer3");

    var mathQuestionElement = document.getElementById("mathQ");
    var mathInput = document.getElementById("mathInput");
    var acceptMath = document.getElementById("acceptMath");
    var server1State = document.getElementById("server1State");

    var functionHandlerInterval = setInterval(() => {
        if(document.getElementById("serverHandler").innerText == "startServer1") {startServer1(); document.getElementById("serverHandler").innerText = "";}
        if(document.getElementById("serverHandler").innerText == "startServer2") {startServer2(); document.getElementById("serverHandler").innerText = "";}
        if(document.getElementById("serverHandler").innerText == "startServer3") {startServer3(); document.getElementById("serverHandler").innerText = "";}
    }, 2000);

    function startServer1(){
        NewMathQuestion();

        var result;
        function NewMathQuestion(){
            timer1Time = 30;

            var rng1 = Math.floor(Math.random() * (4 - 1)) + 1;
            var char = '';
            if(rng1 == 1) {char = '+';} else if(rng1 == 2) {char = '-';} else if(rng1 == 3) {char = '*';} else if(rng1 == 4) {char = '/';}
            var rng2 = Math.floor(Math.random() * (100 - 1)) + 1;
            var mathQuestion = `${rng2} ${char}`;

            var rng3 = Math.floor(Math.random() * (100 - 1)) + 1;
            mathQuestion += ` ${rng3}`;
            if(rng1 == 1) {result = rng2 + rng3;} if(rng1 == 2) {result = rng2 - rng3;} if(rng1 == 3) {result = rng2 * rng3;} if(rng1 == 4) {result = rng2 / rng3; result = Math.round(result);}
            //console.log(result);
            mathQuestionElement.innerText = mathQuestion;
        }

            var timer1Interval;
            var timer1Time = 30;
            timer1Interval = setInterval(() => {
                timer1.innerHTML = `Czas na odopwiedź : ${timer1Time}s`;
                timer1Time--;
                if(timer1Time == -1){
                    clearInterval(timer1Interval);
                    acceptMath.remove();
                }
            }, 950);

            var mathCounter = 0;
            acceptMath.addEventListener("click", () =>{
                if(mathInput.value == result){
                    mathCounter++;
                    if(mathCounter > 4){
                        clearInterval(timer1Interval);
                        acceptMath.remove();
                        server1State.innerHTML = "good";
                        return;
                    }
                    NewMathQuestion();
                }
                else {
                    server1State.innerHTML = "bad";
                    clearInterval(timer1Interval);
                    acceptMath.remove();
                }
            })
    }

    var server2State = document.getElementById("server2State");

    function startServer2(){
        RenderPacket();
        var packetDestroyCounter = 0;
        function RenderPacket(){
            var full = document.getElementById("hackServer2");
            var packet = document.createElement("div");
            packet.classList.add("packet");
            var rng = Math.floor(Math.random() * (90 - 1)) + 1;
            packet.style.top = "0%";
            packet.style.left = `${rng}%`;
            packet.addEventListener("click", () => {
                packet.remove();
                clearInterval(i1);
                pos = 0;
                packetDestroyCounter++;
                if(packetDestroyCounter == 40){
                    packet.removeEventListener("click", this);
                    server2State.innerText = "good";
                    return;
                }
                RenderPacket();
            });

            var rng2 = Math.floor(Math.random() * (70 - 20)) + 20;
            full.appendChild(packet);
            var pos = 0;
            var i1 = setInterval(() => {
                if(typeof(packet) == 'undefined' && packet == null) {clearInterval(i1); pos = 0; return;}
                console.log(pos);
                pos++;
                packet.style.top = `${pos}%`;
                if(pos == 92){
                    clearInterval(i1);
                    server2State.innerText = "bad"
                }
            }, rng2);
        }
    }
    
    function startServer3(){
        var counter = 0; var lifes = 2;
        NewPosition();
        function NewPosition(){
            var area = document.getElementById("area");
            var server3State = document.getElementById("server3State");

            var toprng = Math.floor(Math.random() * (80 - 1)) + 1;
            var leftrng = Math.floor(Math.random() * (85 - 1)) + 1;
            area.style.top = `${toprng}%`;
            area.style.left = `${leftrng}%`;

            var good = false;
            var lis1 = area.addEventListener("mouseenter", () =>{
                good = true;
            });
            var lis2 = area.addEventListener("mouseleave", () =>{
                good = false;
            });

            setTimeout(() => {
                area.style.borderColor = "yellow";
                setTimeout(() => {
                    area.style.borderColor = "red";
                    setTimeout(() => {
                        if(!good && lifes < 1) {server3State.innerText = "bad"; return;}
                        if(!good) lifes--;
                        counter++;
                        if(counter == 20){server3State.innerText = "good"; area.style.borderColor = "blue"; return;}
                        area.removeEventListener("mouseenter", lis1);
                        area.removeEventListener("mouseleave", lis2);
                        area.style.borderColor = "green";
                        NewPosition();
                    }, 100 +100 +100);
                }, 250 +50 +200);
            }, 250 +100 +200);
        }
    }
}