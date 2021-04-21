// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function() {
    //const memorySpace = document.getElementById('memory-space');

    
    var connection = new signalR.HubConnectionBuilder()
        .withUrl("/msg")
        .configureLogging(signalR.LogLevel.Information)
        .build();

    /*connection.on('ReceiveMessage',
        (sender, messageText) => {
            //memorySpace.value = messageText;
            $("#memory-space").text(messageText);
            console.log(`${sender}:${messageText}`);
        });

    connection.on('receiveMessage',
        (sender, messageText) => {
            console.log(`${sender}:${messageText}`);
        });*/

    connection.on("ReceiveMetrics", function(metrics) {
        /*console.log(
            `FreeMemory=${metrics.FreeMemory},freeMemory=${metrics.freeMemory},freememory=${metrics.freememory} `);*/
        $("#memory-full").text(metrics.fullMemory);
        $("#memory-free").text(metrics.freeMemory);
        $("#processor").text(metrics.processorLoad);
        $("#hdd-full").text(metrics.fullSpaceHdd);
        $("#hdd-free").text(metrics.freeSpaceHdd);
        

    });

    connection.on("GetInterval",
        function() {
            var interval = parseInt($("#interval").val());
            console.log(`Getinterval=${interval}`);
            if (!isNaN(interval)) {
                console.log("GetInyerval -> sendInterval");
                connection.invoke("sendInterval", interval);
            } else {
                console.log("interval default value = 10000");
                connection.invoke("sendInterval", 10000);
            }
        });

    $("#btnSend").click(function (event) {
        console.log("interval is sent");
        event.preventDefault();
        var interval = parseInt($("#interval").val());
        console.log(`interval=${interval}`);
        if (!isNaN(interval)) {
            console.log("will be sent");
            connection.invoke("sendInterval", interval);
        }
        
    });



    connection.start()
        .then(() => console.log('connected!'))
        .catch(console.error);



});