var connection = new signalR.HubConnectionBuilder()
    .withUrl("/serverHub").build();

connection.start().then(function () {
    let hubId = document.getElementById('HubID');
    if (hubId !== undefined && hubId !== null) {
        connection.invoke("GetConnectionUserId").then(function (id) {
            hubId.value = id;
        });
    }
    console.log("Hub Conectado");
}).catch(function (err) {
    console.log(err);
});

connection.on("Notification", function (color) {
    notification(color);
});

connection.on("Process", function (text) {
    addProcessWithProgressBar(text);
});

connection.on("ProgressBar", function (value) {
    let ele = document.getElementById("progressBar");
    ele.ariaValueNow = value;
    ele.ariaValueMin = value;
    ele.ariaValueMax = 100;
    ele.textContent = value + '%';
    ele.style.width = ele.textContent;
});

connection.on("RemoveProcess", function () {
    removeProcess();
    notification();
    audio();
});