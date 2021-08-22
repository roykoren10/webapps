//weather.js

var weatherCallback = function (data) {
    var wind = data.query.results.channel.wind;
    var item = data.query.results.channel.item;
    var text = "Temperature: " + item.condition.temp + " °C";
    $("#temperatureDiv p").html(text);

    if (item.condition.temp <= 20) {
        updateTemperatureDiv("It's pretty cold outside, we advice you to stay in the room rather then Rage it");
    }
    else if (item.condition.temp > 16 && item.condition.temp < 30) {
        updateTemperatureDiv("The weather is amazing! You can travel outside or enjoy escaping a room");
    } else {
        updateTemperatureDiv("It's very hot outside! Perfect weather for Rage room");
    }
};

function updateTemperatureDiv(text) {
    $("#temperatureDiv").append("<p>" + text + "</p>");
}