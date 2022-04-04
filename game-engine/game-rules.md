# The Game


### The Game for Entelect Challenge 2022 is **Arctica**.
---
**NB:
The values provided within this readme are subject to change during balance phases.
Entelect will endeavour to maintain this readme file. However the most accurate values can be found in appsettings.json within the game-engine folder.**

---



Hello traveller 😊 

You look weary. 

I know it’s cold, why don’t you set up camp?

You’re not the only one around these parts - I’ve seen a few others looking for shelter recently...

They need **food**, **warmth** and are happy to provide labour in return. Will you be able to create a prosperous community? Or lose them to the surrounding camps? 


In this game, you will be the leader of **one** of **four** neighbouring villages. Competing for resources and fighting to survive in this cold tundra 🥶 

You are not alone; you have your comrades to aid you in this venture. They will do as you say. From **scouting** for information to **gathering resources**. They are your key to thriving in this unforgiving clime. If you show yourself to be a good leader and can provide a surplus of food and warmth 🔥 word will spread, and you will attract more **travellers to your base**. 

However, the opposite is also true ☝️ those who do not have their needs being met will **leave** to find refuge elsewhere. 

Your goal is not only to survive but thrive in this winter landscape. To expand your village and resources, you need to **outcompete** your competitors by having the biggest village at the end of the game. This is primarily based on population size, but individual resources also hold value in the final score.



## Contents
- [The Game](#the-game)
    - [The Map](#the-map)
        - [Visibility](#visibility)
        - [Boundary](#boundary)
    - [Game Objects](#game-objects)
        - [Scout Towers](#scout-towers-🏯)
    - [Resource Distribution](#resource-distribution)
        - [Ship to ship collisions](#ship-to-ship-collisions)
        - [Food collisions](#food-collisions)
    - [Game Tick Payload](#game-tick-payload)
        - [GameObjects](#gameObjects)
        - [Unit State](#unit-state)
    - [The Commands](#the-commands)
        - [All Commands](#all-commands)
        - [Command Structure](#command-structure)
        - [Command: SCOUT](#command:-FORWARD)
        - [Command: MINE](#command:-STOP)
        - [Command: FARM](#command:-START_AFTERBURNER)
        - [Command: BURN WOOD?](#command:-STOP_AFTERBURNER)
    - [Endgame](#endgame)
    - [Scoring](#scoring)
    - [The Math](#the-math)
        - [Resource Distribution](#resource-distribution)
        - [Distance Calculation](#distance-calculation)
        - [Direction Calculation](#direction-calculation)
        - [Rounding](#rounding)



        - race condition - unites from different bases entering the same node
        - warmth stuff
        - Day night cycle - people joining and leaving 
        - Unit Tiers 
        - Travel time
        - ✅unit state

## The Map

The Map is a **square** grid of variable size. Each cell in the grid is referred to as a **node**. Your **base** is situated randomly within the world.

The map contains the following objects:

* Player bases 🛖  yourself and the other opponents located in the map.
* Scout towers 🏯 points on the map that reveal other nodes around them
* Resource nodes 🟩 nodes that has harvestable resources 
  * wood 🪵
  * crops 🌾🌾  
  * stone 🪨

### Visibility

You will have limited visibility depending of the amount of scout towers you have visited. 

At the start of the game you will only have access your base and all the scout towers in the map, as shown in the image below.

Once you visit a scout tower, you will have access to the resource nodes surrounding the scout tower.

![starting state](images\fig1.svg)
##### starting state

<!-- The Map is a cartesian plane stretching out in both positive and negative directions. The map will only deal with whole numbers. Your ship can only be in an integer x,y position on the map. The map center will be 0,0 and the edge will be provided as a radius, the maximum rounds of a game will be equal to the size of the radius. -->

<!-- 
### Boundary

The size of the map will shrink each game tick. Objects that fall outside of the map at the end of a game tick will be removed from the map. This excludes players, being outside of the map will reduce the players size by 1 each game tick.

* The map size reduction is the final action of the game tick. -->

## Game Objects

The map is split into a number of regions. Resource nodes are distributed randomly throughout the regions. The random value is within a given range.

The maximum amount of units that can enter a node, the **reward/harvest** amount  and **work time** values are also determined by a random value selected within a given range. eg. 👇

```
      "QuantityRangePerRegion": [
        3,
        10
      ],
```
👆 The quantity of each node in the region is randomly selected between 3 and 10

The random values selected by a seed
<!-- 
All objects are on a 

All objects are represented by a circular shape and has a centre point with X and Y coordinates and a radius that defines it size and shape. -->
---
### Player Bases

Bases

---
### Scout Towers 🏯

TODO: map size is dependent on the number of regions


Increase visibility of the area, within their respective region.

There is a scout tower for each region. As seen on the image below if the map had 4 region:

![scout tower regions](images\scout-tower-regions.svg)

 Each scout tower is generated randomly within their respective region.

The scout towers will disclose the locations of resource nodes and rival bases within their respective region.

At the beginning of the game you will only have access to the scout towers as seen in the **[starting-state](#starting-state)**

Use the **[scout](#scout)** command to visit a scout tower to retrieve the information of the quadrants.

_Note: Bases who do not scout at that scout location will not have access to the area's nodes_

---

### Resource Nodes 🟩

Areas where the players units can harvest resources. 

Resource nodes have a **maximum unit capacity** i.e they do not allow more units past a certain point. 

Units from different bases can harvest resources on the same node. The distribution of resources is dependent on the  **number of units** for each base.

//TODO: can bots see how many units are one a resource at a given tick

_See **[Resource Distribution](#resource-distribution)** to see how resources are distributed._

There are a **finite amount** of resources that can be collected for certain resource nodes, namely: **stone** and **wood**. These resources will not replenish. 

**Farming nodes** Will replenish after a certain amount ot ticks _see
**[Regeneration Rate](#regeneration-rate)**_

There are currently **three** kinds of resources:
- Food
- Wood
- Stone

_for how these resources are used see **[Resources](#resources)**_

After using the **[scout](#scout)** command to increase visibility there will be three resource node types available to extract resources from

 - Farm 
 - Mine 
 - Forrest 

#### Farm

Infinite resource supply. Use the **[farm](#farm)** command on your units to **collect food** at this node.


#### Mine

Limited resource supply. Use the **[mine](#mine)** command on your units to **mine stone** at this node.

#### Forrest

Limited resource supply. Use the **[log](#log)** command on your units to **chop wood** at this node





<!-- ### Food

The map will be scattered with food objects of size 3 which can be consumed by players.

* Food will not move.
* Food will be removed if it falls outside of the map.
* If a player collides with food it will consume the food in it's entirety and the player will increase by the same size as the food. -->

<!-- ### Wormholes

Wormholes exist in pairs, allowing a player's ship to enter one side and exit the other. Wormholes will grow each game tick to a set maximum size, when traversed, the wormhole will shrink by half the size of the ship traversing through it.

* Traversal will only be possible if the wormhole is larger than the ship.
* Traversing the wormhole is instantaneous, no penalties are applied to the player for using a wormhole.
* Momentum and direction is maintained through the wormhole.
* If one end of the wormhole pair falls outside of the map both will be destroyed.
* Wormhole pairings are not given but rather will need to be mapped by players through trial and error. -->

<!-- ### Gas Clouds

Gas clouds will be scattered around the map in groupings of smaller clouds. Ships can traverse through gas clouds, however once a ship is colliding with a gas cloud it will be reduced in size by 1 each game tick. Once a ship is no longer colliding with the gas cloud the effect will be removed. -->

<!-- ### Asteroid Fields

Asteroid fields will be scattered around the map in groupings of smaller clouds. Ships can traverse through asteroid fields, however once a ship is colliding with a asteroid cloud its speed will be reduced by a factor of 2. Once a ship is no longer colliding with the asteroid field the effect will be removed. -->

<!-- ## The Ship

Your bot is playing as a circular spaceship, that feeds on planetary objects and other ships to grow in size. Your ship begins with the following values:

* **Speed** - your ship will move x positions forward each game tick, where x is your speed. Your speed will start at 20 and decrease as the ship grows.
* **Size** - your ship will start with a radius of 10.
* **Heading** - your ship will move in this direction, between 0 and 359 degrees.

Your ship will not begin moving until you have given it a command to do so. Once given the first forward command it will continue moving at the ship's current speed and heading until the stop command is given. 

There is a minimum size of 5 for the ship if at any point the size is smaller than this the ship will be removed from the map and considered eliminated. -->
---
### Day night cycle 

At the end of **12 ticks** you


<!-- ### Speed

Speed determines how far forward your ship will move each game tick. Your ship will not move when stopped or an initial FORWARD command has been issued, however your speed value will remain the same. Speed is inversely linked to size, a larger ship will move slower. Speed is determined by the following formula:
```
speedRatio = 200
speedRatio/bot.size = speed
```
With the result being rounded to ceiling and with a minimum of 1.


**Note** *the value 200 comes from the Speeds.Ratio in the appsettings.json and may change during balancing.* -->

<!-- 
### Afterburner

The afterburner will increase your ship's speed by a factor of 2. However this will also start reducing the size of your ship by 1 per tick.

* An active afterburner can cause self destruction if the ship's size becomes less than 5. -->

<!-- ## Collisions

A collision will occur when two objects overlap by at least one unit of world space. This means that objects can touch each other, but the moment they overlap by a single world space unit they will be considered colliding and collision mechanics will apply. -->

<!-- ### Ship to ship collisions

When player ships collide, the ship with the larger size will consume the smaller ship at the rate of 50% of the larger ship's size to a maximum of the smaller ship's size.

After a ship collision, both ships heading will be reversed by 180 degrees, they will be separated by 1 unit of world space and thereafter will continue to move in this new direction thereafter. This will simulate a bounce. -->
<!-- 
### Food collisions

When a ship collides with a food particle, the ship will consume the food in its entirety and will increase in size by the size of the food it just collided with. -->

## Game Tick Payload

All players will receive the state of the world, all game objects and all player objects at the start of each tick. The payload of each game tick will contain the following information:
<!-- 
 ```
{
  "World": {
      "CenterPoint": {
        "X": 0,
        "Y": 0
      },
      "Radius": 1000,
      "CurrentTick": 0
    },
    "GameObjects": {
      "8b77d46b-2844-48c4-a3f3-179de15776a3": [ 3, 0, 0, 2, 42, 225 ],
      "2b75d46b-2866-48f1-a4g5-179de15779o0": [ 3, 0, 0, 2, 234, -900 ],
      "9b34d46b-4844-48g2-a3b2-179de15771m8": [ 3, 0, 0, 2, -100, 189 ],
      ...
    },
    "PlayerObjects": {
      "ad672ef2-f6a7-404c-950a-a867c54c7de0": [ 10, 20, 0, 1, 42, 225, 1 ],
      "5f535caf-c3fc-4935-9d95-6e48b3680fd7": [ 20, 10, 90, 1, 234, -900, 2 ],
      "e671e725-4bbb-4d4d-ad18-f013a567dfda": [ 40, 5, 180, 1, -100, 189, 4 ],
      ...
    }
}  
``` -->






### GameObjects

The GameObjects list contains all objects on the map. The list contains of a guid for each object and the objects data.

<!-- The order of the data will not change, and is as follows:
* Size - The radius of the object
* Speed - The speed it is able to move at
* Heading - The direction it is looking towards
* GameObjectType - The type of object
    * 2: Food
    * 3: Wormhole
    * 4: Gas Cloud
    * 5: Asteroid Field
* X Position - The x position on the cartesian plane
* Y Position - The y position on the cartesian plane -->

### PlayerObjects

The PlayerObjects list contains all player objects on the map. The list contains of a guid for each object and the objects data.

<!-- The order of the data will not change, and is as follows:
* Size - The radius of the object
* Speed - The speed it is able to move at
* Heading - The direction it is looking towards
* GameObjectType - The type of object
    * 1: Player
* X Position - The x position on the cartesian plane
* Y Position - The y position on the cartesian plane
* Active Effects - Bitwise effects currently on affect of the bot
    * This is a cumulative bit flag, represented by:
        * 0 = No effect
        * 1 = Afterburner active
        * 2 = Asteriod Field
        * 4 = Gas cloud 
    * For example, if a ship has all three effects the active effect will be 7. -->

### Unit state



## The Commands

Bases have a set amount of units that they can dictate actions to.

In each game tick, a base can submit a list of commands for their units. This command list **cannot send more** units than the base has available.

The commands are a list of actions that a unit can perform once they have reached the target node. 

_Note: ids are used to determine the target node not the x, y value_

Each unit can perform the five commands.


### All Commands

* SCOUT
* FARM
* MINE
* LOG
* BURN WOOD

### Command Structure

Commands can be sent to the engine as often as you like, but the engine does not wait for commands from the bots and processes the game state at a set rate.

The Runner will only allow a maximum of one 'command' but many actions in a list per tick.

Your commands will be lodged against your bot for processing during the next tick. These actions are processed under FIFO (First in, First out), meaning your earliest sent action is processed first. However, this list of actions should only ever be one command long.

This means that your bot does not need to send a command each tick and can take as long as it wants to send each command. Feel free to run all the clever artificial intelligence you like! Just note that other bots might still be sending commands as there is no wait time.

Example Payload in JSON with types:

```jsonc
{
  "playerId" : "00000000-0000-0000-0000-000000000000",  // id of the bot - string/GUID/UUID
  "actions" : [ {                                       // list of actions
    "type" : 1,                                         // action type -  int
    "units" : 4,                                        // number of units - int
    "id" : "00000000-0000-0000-0000-000000000000"       // id of the destination node -string/GUID/UUID
  },
  {
    "type" : 3,                                         // action type -  int
    "units" : 3,                                        // number of units - int
    "id" : "00000000-0000-0000-0000-000000000000"       // id of the destination node -string/GUID/UUID
  }
  ...                                                   // and so on..
  ]
} 
``` 
---

### Command: SCOUT

```
SCOUT: 1
```

This command will send the amount of units specified to a scout tower.

Example Payload in JSON with types:
```jsonc
{
    "type" : 1,                                   // SCOUT action type - int
    "units" : 2,                                  // number of units to travel to node - int
    "id" : "00000000-0000-0000-0000-000000000000" // destination node - string/UUID/GUID
}
```

### Command: MINE

```
MINE: 2
```

This command will send the amount of specified units to mine

Example Payload in JSON with types:
```jsonc
{
    "type" : 2,                                   // MINE action type - int
    "units" : 2,                                  // number of units to travel to node- int
    "id" : "00000000-0000-0000-0000-000000000000" // destination node - string/UUID/GUID
}
```

### Command: FARM

```
FARM: 3
```

This command will send the amount of specified units to farm, farms are an infinite resource and will not deplete.

Example Payload in JSON with types:
```jsonc
{
    "type" : 3,                                   // FARM action type - int
    "units" : 2,                                  // number of units to travel to node- int
    "id" : "00000000-0000-0000-0000-000000000000" // destination node - string/UUID/GUID
}
```

### Command: LUMBER

```
LUMBER: 4
```

This command will send the amount of specified units to log, forests are a renewable resource and have a regeneration rate

Example Payload in JSON with types:
```jsonc
{
    "type" : 4,                                   // LUMBER action type - int
    "units" : 2,                                  // number of units to travel to node- int
    "id" : "00000000-0000-0000-0000-000000000000" // destination node - string/UUID/GUID
}
```

## Endgame

The base/bot to reach the highest score will be determined the winner. 
Score is dependent on population size and stone accumulated.
score per person  : 
score per stone   :

## Scoring

A score will be kept for each player which will be visible once the match is over. This will only be used for tie breaking.

Score is dependent on population size and stone accumulated.
score per person  : 
score per stone   :

***Note** these values are representative only and are subject to change during balancing. Final values can be found in the appsettings.json of the game-engine folder.*

## The Math
This section is to explain the general math used for the movement and placement calculations by the engine, as well as basic functions to calculate this. These formulas have been included as functions in each starter bot.

The world uses the standard mathematical cartesian plane with the 0 degree at the rightmost axis and it positively increments counter clockwise as can be seen below:

![cartesian plane](https://github.com/Jana-Wessels/images/blob/master/cartesian_plane.png?raw=true)

### Distance Calculation
Distance is calculated using the standard pythagoras theorem, a diagram showing the values can be seen below:

![cartesian plane](https://github.com/Jana-Wessels/images/blob/master/distance_calculation.png?raw=true)



Direction does not matter in the distance calculation as we are only concerned with the distance between two objects regardless of where they are.
Where delta y and delta x:
```
ΔX = X1 - X2
ΔY = Y1 - Y2
```
It does not matter if the delta's are negative as that will be cancelled out in the formula through squaring them. The formula for the distance calculation is as follows:

```
distance = Math.Sqrt((deltaX ^ 2) + (deltaY ^ 2)) 
// or
distance = Math.Sqrt((y2 - y1)^2 + (x2 - x1)^2)
```
This formula can be used to calculate the distance between nodes in the world. 

---

### Population Growth/Decline Rate




---


### Node Distribution 

Each resource has a set `reward` field, this field effectively determines how many resources a single unit can carry back to base. When resource distribution is calculated the `NumberOfUnits` is multiplied by the `reward`. 
i.e  
```
3 units, log lumber at node id: "123"

resource node 123's reward amount is 4

AmountExtracted = 3 * 4 = 12
```

The `reward` amount is generated randomly from a range at that start of the game.

TODO: if the node does not currently have the resource amount available then..

---


### Rounding
<!-- Due to the fact that the engine uses integers to represent the world rounding had to be applied to certain calculations that gave decimal results. 

The game engine uses C# which uses round-to-even or banker's rounding, which means any value that is at exactly at the midpoint ( x.5 ), will be rounded to the nearest even number.
Take the following examples:

- 23.5 will become 24
- 24.5 will become 24
- 24.6 will become 25
- 25.5 will become 26

A summary of those rounding decisions can be seen below:

####Distance between objects:
Uses standard Math.Round rounding.

####Position calculation:
Uses standard Math.Round rounding

####Speed calculations:
Uses Math.Ceiling rounding as there is a minimum that it shouldn't go below.

####Size calculations:
Uses Math.Ceiling rounding as there is a minimum that it shouldn't go below.

Note when doing rounding calculations in your own bot check the rules of the specific language as some use different rounding strategies to C#. 

No language will have a advantage as only integer values are used in commands and the engine calculates all bot commands in the same way.
This section is for reference to check that your bot calculations and decision points align with how the engine works. -->


