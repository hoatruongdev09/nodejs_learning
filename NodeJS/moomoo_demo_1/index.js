
var app = require('express')();
var server = require('http').Server(app);
var io = require('socket.io')(server);
//message code
const rq_connection = '1'
const rq_connection_successed = '2'
const rq_connection_failed = '3'
const rq_info = '4'
const rq_info_map = '5.1'
const rq_info_player = '5.2'
const rq_join = '6'
const rq_join_successed = '7'
const rq_other_join = '8'
const disconnected = 'dc'
const sync_position = "9"
const sync_position_player = "10"
const sync_rotation = "11"
const sync_rotation_player = "12"
const sync_resource = "13"
//
server.listen(3000);
app.get("/", function (req, res) {
    res.send("lolol");
});

console.log("server is running");
var maxClient = 50;
const woodCount = 10;
const foodCount = 10;
const rockCount = 10;
const goldCount = 10;
var wolrd = [];
var mapSize = { x: 300, y: 300 };
generateWorld()

var clients = [];

io.on("connect", function (socket) {
    var currentPlayer = {};
    socket.on(rq_connection, function () {
        if (clients.length < 50) {
            var _id = generateId();
            currentPlayer = {
                id: _id,
                position: "unknown",
                rotation: "unknow",
                score: 0,
                isJoined: false,
            }
            console.log("current player id: " + currentPlayer.id);
            clients.push(currentPlayer);
            socket.emit(rq_connection_successed, { id: currentPlayer.id });
            for (var i = 0; i < wolrd.length; i++) {
                socket.emit(sync_resource, wolrd[i]);
            }
        } else {
            socket.emit(rq_connection_failed);
        }
    })
    socket.on(rq_info, function () {
        console.log("player " + currentPlayer.id + " request info");

        for (var i = 0; i < clients.length; i++) {
            if (clients[i].isJoined) {
                var other = {
                    id: clients[i].id,
                    position: clients[i].position,
                    rotation: clients[i].rotation,
                    score: clients[i].score
                }
                socket.emit(rq_info_player, other);
            }
        }
        var info = {
            mapSize: mapSize,
        }
        socket.emit(rq_info_map, info);
    })
    socket.on(rq_join, function () {
        console.log("player " + currentPlayer.id + " request join");
        currentPlayer.isJoined = true;
        var spawnPos = getRandomPosition();
        currentPlayer.position = spawnPos;
        currentPlayer.rotation = 0;
        socket.emit(rq_join_successed, spawnPos)
        socket.broadcast.emit(rq_other_join, { id: currentPlayer.id, spawnPosition: spawnPos });
    })
    socket.on(sync_position, function (data) {
        console.log("player " + currentPlayer.id + " position " + JSON.stringify(data));
        currentPlayer.position = data;
        socket.broadcast.emit(sync_position_player, { id: currentPlayer.id, position: currentPlayer.position });
    });
    socket.on(sync_rotation, function (data) {
        console.log("player " + currentPlayer.id + " rotation " + JSON.stringify(data));
        currentPlayer.rotation = data;
        socket.broadcast.emit(sync_rotation_player, { id: currentPlayer.id, rotation: currentPlayer.rotation })
    })

    socket.on('disconnect', function () {
        console.log('player ' + currentPlayer.id + ' disconnected');
        var id = currentPlayer.id;
        removePlayerById(id);
        socket.broadcast.emit(disconnected, { id: id });
    })

})
function generateId() {
    var id = 0;
    while (findPlayById(id) !== null) {
        id++;
    }
    return id;
}
function removePlayerById(id) {
    for (var i = 0; i < clients.length; i++) {
        if (clients[i].id === id) {
            clients.splice(i, 1);
            break;
        }
    }
}
function findPlayById(id) {
    for (var i = 0; i < clients.length; i++) {
        if (clients[i].id === id) {
            return clients[i];
        }
    }
    return null;
}
function generateWorld() {
    for (var i = 0; i < foodCount; i++) {
        wolrd.push({ type: 'food', position: getRandomPosition() });
    }
    for (var i = 0; i < woodCount; i++) {
        wolrd.push({ type: 'wood', position: getRandomPosition() });
    }
    for (var i = 0; i < rockCount; i++) {
        wolrd.push({ type: 'rock', position: getRandomPosition() });
    }
    for (var i = 0; i < goldCount; i++) {
        wolrd.push({ type: 'gold', position: getRandomPosition() })
    }
}
function getRandomPosition() {
    var sizeX = mapSize.x / 2;
    var sizeY = mapSize.y / 2;
    var randomX = getRandomArbitrary(-sizeX, sizeX)
    var randomY = getRandomArbitrary(-sizeY, sizeY);
    return { x: randomX, y: randomY };
}
function getRandomArbitrary(min, max) {
    return Math.random() * (max - min) + min;
}
