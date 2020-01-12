Here you can find a collection of commands you might want to send.

# You can send:
## Create Lobby
```
{
    "type":"createLobby",
    "receiverId":"_serverId_",
    "data":{[["_localId_"],"_gameName_"]}
}
```
Creates a new Lobby with the name
* `_serverId_` The id of the server on which to create the game. (usually `[1,0]`)
* `_gameName_` The name of the game.
* `_localId_` Local Id in the form of `[0,x]` that is used as placeholder until an online id (`[1+,x]`) arrives.

Results in a [response](#response-to-a-create-command)

## Get Lobbies
```
{
    "type":"getLobbies",
    "receiverId":"_serverId_",
    "messageId":"_messageId_"
    "data":{}
}
```
Creates a new Lobby with the name
* `_serverId_` The id of the server on which to create the game. (usually `[1,0]`)
* `_messageId_` Some `integer` value that identifies this message
## Join Lobby
```
{
    "type":"joinLobby",
    "receiverId":"_serverId_"
    "data":{"lobbyName":"_LobbyName_"}
}
```
Joins a lobby
* `_serverId_` The id of the server.
* `_LobbyName_` The name of the lobby.

## Get Maps not implemented
```
{
    "type":"getMaps",
    "receiverId":"_serverId_",
    "senderId":"_playerId_",
    "data":{}
}
```
* `_playerId_` The id of the player taking the action.
* `_serverId_` The id of the server from wich to get the map.

## Save Map
```
{
    "type":"saveMap",
    "receiverId":"_serverId_",
    "senderId":"_playerId_",
    "data":[[true,false,true]]
}
```
* `_playerId_` The id of the player taking the action. 
* `_serverId_` The id of the server on wich to save the map.

## Set Map
```
{
    "type":"setMap",
    "receiverId":"_gameId_",
    "senderId":"_playerId_",
    "data":{"mapId":"_mapId_"}
}
```
* `_playerId_` The id of the player taking the action. Will set the field if it is the one who created it.
* `_gameId_` The id of the game for which to set the map.
* `_mapId_` The id of the map that was selected.

## Start Game
```
{
    "type":"startGame",
    "receiverId":"_gameId_",
    "senderId":"_playerId_",
    "data":{}
}
```
* `_playerId_` The id of the player taking the action. Will only start the game if it is the one who created it.
* `_gameId_` The id of the game that should be started.

## Move
```
{
    "type":"move",
    "receiverId":"_gameId_",
    "senderId":"_playerId_",
    "data":{
        "direction":"_direction_ (up,left,rigth,down)"
    }
}
```
Sent if a player wants to move
* `_playerId_` The id of the player moved. Usefull if two players play over one keyboard.
* `_gameId_` The id of the game that is currently played.
* `_direction_` The direction in which to move

## Shoot
```
{
    "type":"shoot",
    "receiverId":"_gameId_",
    "senderId":"_playerId_",
    "data":{}
}
```
* `_playerId_` The id of the player that wants to shot. Usefull if two players play over one keyboard.
* `_gameId_` The id of the game that is currently played.


# The frontend has to implement

## Receive Lobbies
```
{
    "type":"lobbiesResponse",
    "senderId":"_server_",
    "data":[{"name":"Lobby1",playerCount:2,"lobbyId":[1,15421]},{"name":"_someLobby_",playerCount:1,"lobbyId":[1,15421]}]
}
```
* `_someLobby_` Name of a Lobby.

## Response to A create Command
```
{
    "type":"response",
    "senderId":"_requestedResourceId_",
    "data":{"_oldMessageId__ObjectData_"}
}
```
* `_requestedResourceId_` The id of the requested Resource.
* `_oldMessageId_` The id of the message that represented the get request.
* `_ObjectData_` The serialized represenation of the object.

The `_oldMessageId_` is exactly the first 8 bytes of the `data` and should be removed bevore `_objectData_` is parsed.


## GameSync
```
{
    "type":"gameSync",
    "data":{
        "field":[[true,false,....]],
        "players":[{"name":"Jon","points":8000,"id":[2551,487456]}]
    }
}
```
Tells the frontend to render a new game field.

## Spawn
```
{
    "type":"spawned",
    "data":{
        "coords":[5,9],
        "type":2,
        "id":[2551,487457]
    }
}
```
Valid Types:
* `1` Player
* `2` Shot
* `32` Monster Lvl 1
* `33` Monster Lvl 2
* `34` Monster Lvl 3
* `35` Monster Lvl 4

`0` is `error`

## Move To
```
{
    "type":"movedTo",
    "data":{
        "id":"_entityId_",
        "position":[2,4]
    }
}
```
* `_entityId_` The id of the entity received from `spawn`.

## Kill
```
{
    "type":"kill",
    "data":{
        "id":"_entityId_"
    }
}
```
* `_entityId_` The id of the entity received from `spawn`.
Removes an object from the game.


## SetPoints
```
{
    "type":"kill",
    "data":{
        "id":"_playerId_",
        "points":5400
    }
}
```
* `_playerId_` The id of the player to set the points.
Sets the points of a player.


