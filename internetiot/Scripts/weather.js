//weather.js

var weatherCallback = function () {



    fetch("https://api.weather.gov/gridpoints/TOP/31,80/forecast").then(function (response) {
        return response.json();
    })
        .then(function (data) {
            var temp = ((data.properties.periods[0].temperature - 32) * 5 / 9).toFixed(2)
            var text = "Temperature: " + temp + " °C";
            $("#temperatureDiv p").html(text);

            if (temp <= 20) {
                updateTemperatureDiv("It's pretty cold outside, we advice you to stay in the room rather then Rage it");
            }
            else if (temp > 16 && temp < 30) {
                updateTemperatureDiv("The weather is amazing! You can travel outside or enjoy raging in a room");
            } else {
                updateTemperatureDiv("It's very hot outside! Perfect weather for Rage room");
            }
        }
        ).catch(function () {
            console.log("Booo");
        });
}



function updateTemperatureDiv(text) {
    $("#temperatureDiv").append("<p>" + text + "</p>");
}