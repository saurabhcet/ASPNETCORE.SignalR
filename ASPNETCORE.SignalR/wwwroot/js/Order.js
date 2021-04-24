"use strict";

//WebSocket = undefined;
//EventSource = undefined;
//signalR.HttpTransportType.LongPolling

//Disable send button until connection is established
document.getElementById("submit").disabled = true;

var connection = new signalR.HubConnectionBuilder().withUrl("/orderhub").build();
    connection.on("NewOrder", function (order) {
        var product = order.product;
        var size = order.size;
        var encodedMsg = "Your order has been placed, " + product + ", " + size;
        var li = document.createElement("li");
        li.textContent = encodedMsg;
        document.getElementById("status").appendChild(li);
    });

    connection.on("OrderUpdateResponse", function (message) {
        var encodedMsg = message;
        var li = document.createElement("li");
        li.textContent = encodedMsg;
        document.getElementById("status").appendChild(li);
    });

    connection.on("Finished", function () {
        var li = document.createElement("li");
        li.textContent = "Request completed";
        document.getElementById("status").appendChild(li);
    });

    connection.start().then(function () {
        document.getElementById("submit").disabled = false;
    }).catch(function (err) {
        return console.error(err.toString());
    });

document.getElementById("submit").addEventListener("click", function (event) {
    event.preventDefault();
    const product = document.getElementById("product").value;
    const size = document.getElementById("size").value;

    fetch("/Order",
    {
        method: "POST",
        body: JSON.stringify({ product, size }),
        headers: {
            'content-type': 'application/json'
        }
    })
    .then(response => response.text())
        .then(id => connection.invoke("OrderUpdateRequest", parseInt(id)));
});