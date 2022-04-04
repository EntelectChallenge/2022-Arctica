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


In this game, you will be the leader of **one** of **four** neighboring villages. Competing for resources and fighting to survive in this cold tundra 🥶

You are not alone; you have your comrades to aid you in this venture. They will do as you say. From **scouting** for information to **gathering resources**. They are your key to thriving in this unforgiving clime. If you show yourself to be a good leader and can provide a surplus of food and warmth 🔥 word will spread, and you will attract more **travellers to your base**.

However, the opposite is also true ☝️ those who do not have their needs being met will **leave** to find refuge elsewhere.

Your goal is not only to survive but thrive in this winter landscape. To expand your village and resources, you need to **outcompete** your competitors by having the biggest village at the end of the game. This is primarily based on population size, but individual resources also hold value in the final score.

---

## Contents
- [The Game](#the-game)
    - [The Map](#the-map)
        - [Visibility](#visibility)
    - [Game Objects](#game-objects)
        - [Scout Towers](#scout-towers)
        - [Resource Nodes](#resource-nodes)
    - [Game Tick Payload](#game-tick-payload)
        - [World](#world)
        - [Nodes](#nodes)
        - [Bots](#bots)
    - [The Commands](#the-commands)
        - [All Commands](#all-commands)
        - [Command Structure](#command-structure)
        - [Command: SCOUT](#command-scout)
        - [Command: MINE](#command-mine)
        - [Command: FARM](#command-farm)
        - [Command: LUMBER](#command-lumber)
        - [Command: START_CAMPFIRE](#command-startcampfire)
    - [Inter tick calculations](#inter-tick-calculations)
      - [Travel time](#travel-time)
      - [population tiers](#population-tiers)
      -[Population Growth/Decline Rate](#population-growthdecline-rate) 
    - [Endgame](#endgame)
    - [Scoring](#scoring)
    - [The Math](#the-math)
        - [Resource Distribution](#resource-distribution)
        - [Distance Calculation](#distance-calculation)
        - [Rounding](#rounding)

---
## The Map

The Map is a **square** grid of variable size. Each cell in the grid is referred to as a **node**. The map is split into a number of regions.
the map size is dependent on the number of regions.

The map contains the following objects:

* Player bases 🛖  yourself and the other opponents located in the map.
* Scout towers 🏯 points on the map that reveal other nodes around them
* Resource nodes 🟩 nodes that have harvestable resources
    * wood 🪵
    * crops 🌾🌾
    * stone 🪨

---
### Visibility

You will have limited visibility depending of the amount of scout towers you have visited.

At the start of the game you will only have access to your base information and all the scout towers in the map, as shown in the image below.

Once you visit a scout tower, you will have access to the resource nodes surrounding the scout tower.

![starting state](images\fig1.svg)
##### starting state

---

## Game Objects

Resource nodes are distributed randomly throughout the regions. The random value is within a given range.

The maximum amount of units that can enter a node, the **reward/harvest** amount and **work time** values are also determined by a random value selected within a given range. eg. 👇

```
      "QuantityRangePerRegion": [
        3,
        10
      ],
```
👆 The quantity of each node in the region is randomly selected between 3 and 10

The random values are controlled by a seed

---
### Player Bases

A player base is the village that the bot will be managing. It will be placed at a designated location that a player will have for the whole match.
There are four specific positions that a base could be at. These are assigned randomly to the bots in a match.
The player base is where the resources are gathered and where the units reside.

---
### Scout Towers
Scout towers are the main driver of resource discovery. Each region on the map has a scout tower that holds all the resource node information for that specific region.
They provide visibility of their region by disclosing the locations of resource nodes and, in the future, rival bases for the bots to make use of.

By scouting a specific tower, the bot retrieves the information from the scout tower and can now begin to harvest resources from the discovered nodes. A tower only needs to be scouted once to retrieve the region information.

The image below illustrates the regions and their scout towers, for a map with 4 regions:

![scout tower regions](images\scout-tower-regions.svg)

Each scout tower is generated and placed randomly within their respective region.

At the beginning of the game you will only have access to the scout towers as seen in the **[starting-state](#starting-state)**

Use the **[scout](#command-scout)** command to visit a scout tower to retrieve the information of the regions.

_Note: Bases who do not scout at that scout location will not have access to the area's nodes_

---

### Resource Nodes

These are locations where the player's units can harvest resources.

Resource nodes have a **maximum unit capacity** i.e they do not allow more units past a certain point.

Units from different bases can harvest resources on the same node. The distribution of resources is dependent on the  **number of units** for each base.

Bots have access to how many units are currently available on a node, as well as the remaining resources on that node.

_See **[Resource Distribution](#resource-distribution)** to see how resources are distributed._

There is a **finite amount** of resources that can be collected for certain resource nodes, namely: **stone** and **wood**. These resources will not replenish.

**Farming nodes** Will replenish after a certain amount ot ticks _see
**[Regeneration Rate](#regeneration-rate)**_

There are currently **three** kinds of resources:
- Food
- Wood
- Stone

_for how these resources are used see **[Resources](#resources)**_

After using the **[scout](#command-scout)** command to increase visibility there will be three resource node types available to extract resources from

- Farm
- Mine
- Forest

#### Farm

Infinite resource supply. Use the **[farm](#command-farm)** command on your units to **collect food** at this node.


#### Mine

Limited resource supply. Use the **[mine](#command-mine)** command on your units to **mine stone** at this node.

#### Forest

Limited resource supply. Use the **[lumber](#command-lumber)** command on your units to **chop wood** at this node

### Day night cycle

At the end of **10 ticks** all bots enter the night cycle of the game. This is where

---
## Game Tick Payload

All players will receive the state of the world, all game objects and all player objects at the start of each tick. The payload of each game tick will contain the following information:

```jsonc
{
    "world": {                            // world state
        "size": 40,
        "currentTick": 2442,
        "populationTiers": [
            {
                "level": 0,
                "name": "Tier 0",
                "maxPopulation": 10,
                "populationChangeRange": [-0.03, 0.03]
            },
            {
                "level": 1,
                "name": "Tier 1",
                "maxPopulation": 100,
                "tierResourceConstraints": {
                    "food": 20,
                    "wood": 100,
                    "stone": 35
                },
                "populationChangeRange": [-0.03, 0.03]
            }
        ],
        "map": {
            "scoutTowers": [
                {
                    "id": "a1af363b-3de5-47fb-8bd6-260f917a12b9",
                    "gameObjectType": 2,  // Game object type: scout tower
                    "position": {
                        "x": 22,
                        "y": 21
                    },
                    "nodes": [] //TODO: should this still be here???
                }
                //...
            ],
            "nodes": [
                {
                    "id": "d04dbdfd-09c6-4f36-83ac-9a6f56ece5a4",
                    "gameObjectType": 3,  // resources node
                    "position": {
                        "x": 26,
                        "y": 39
                    },
                    "type": 2,            // resource type: food 
                    "amount": 5000,
                    "maxUnits": 1000,
                    "currentUnits": 0,
                    "reward": 6,
                    "workTime": 1,
                    "regenerationRate": {
                        "ticks": 3,
                        "amount": 6
                    }
                },
                {
                    "id": "000320f6-36e8-4954-9210-43adc4672d99",
                    "gameObjectType": 3,
                    "position": {
                        "x": 23,
                        "y": 35
                    },
                    "type": 1,      // resource type: wood 
                    "amount": 4973,
                    "maxUnits": 1000,
                    "currentUnits": 0,
                    "reward": 9,
                    "workTime": 3,
                    "regenerationRate": null
                }
                //...
            ] 
        }
    },
    "bots": [     // player state
        {
            "id": "dfdcda53-d615-432c-b3f7-12ea74a1a72c",
            "currentTierLevel": 0,
            "tick": 2442,
            "map": {
                "scoutTowers": [
                    "1e253509-e3a7-4550-850e-dcfaebea96eb"
                ],
                "nodes": [
                    "d04dbdfd-09c6-4f36-83ac-9a6f56ece5a4",
                    "000320f6-36e8-4954-9210-43adc4672d99"
                ]
            },
            "population": 2,
            "baseLocation": {
                "x": 30,
                "y": 30
            },
            "travellingUnits": 1,
            "lumberingUnits": 0,
            "miningUnits": 0,
            "farmingUnits": 0,
            "scoutingUnits": 0,
            "availableUnits": 1,
            "seed": 13123,
            "wood": 815,
            "food": 0,
            "stone": 0
        }
        //..
    ], 
    "botId": "dfdcda53-d615-432c-b3f7-12ea74a1a72c"
}
```
---
### World

The `world` list contains the basic world information and a list of all **scout towers** on the map. The list contains of a guid for scout tower and the scout tower data.

The order of the data will not change, and is as follows:
* `size` - The size of the map, square grid
* `currentTick` - tick where the payload has been processed and returned
* `populationTiers` - List of tiers and their resource contraints and how the population changes will be calculated
* `map` - all the information on the map
  * `scoutTowers` - list of scout towers that exist on the map
    * `id` - id of the scout tower
    * `gameObjectType` - the type of game object. Stored as an enum, in the case of the scout tower it will always be `2 - SCOUT_TOWER`
    * `position` - ( x , y ) position on the map
* `botId` - id of the bot
---
### Nodes

The `nodes` list contains the list of nodes that the base has access to. This list will be filled **after** a scouting action has been completed on a scout tower on the map. Then the nodes in that region will be visible. The list contains of a guid for nodes and the node data.

The order of the data will not change, and is as follows:
* `id` - id of the node
* `gameObjectType` - the type of game object the resource is. Stored as an enum. In this case it will always be `3 - RESOURCE_NODE`
* `position` - ( x , y ) position on the map
* `type` - resource type
  * `WOOD   -   1`
  * `FOOD   -   2`
  * `STONE   -   3`
* `amount` - the number of resources available for harvesting on that node
* `maxUnits` the number of units allowed on that node at any givin point int time
* `currentUnits` - the number of units on the node at the tick time that the payload was returned
* `reward` - the amount of harvested resource that a single unit can carry back to base
* `workTime` - the amount of tick time taken to perform the harvesting action
* `regenerationRate` - determines the amount of tick time, and by how much, is taken for the resource to replenish it's stock
//TODO does this only happen when the amount is zero or less than the original amount?
_Note: this only applies to farming nodes. Forrest and mine nodes have the regenerationRate set to null_

---
### Bot

The `bots` list contains all player bases on the map _will be reviled after scouting action_. The list contains of a guid for each object and the objects data.

* `id` - id of the bot
* `currentTierLevel` - population tier level
* `tick` - current tick that the payload has been published on
* `map` - contains information of the bots personal map
  * `scoutTowers` - scout towers already scouted 
  * `nodes` - nodes ids that the player has revealed by the scouting action
* `population`- current total population
* `baseLocation`- ( x, y ) location of the base
* `travellingUnits` - amount of units currently ***travelling*** to their destination node
* `lumberingUnits` - amount of units currently **logging wood**
* `miningUnits` - amount of units currently **mining**
* `farmingUnits` - amount of units currently **farming**
* `scoutingUnits` - amount of units currently **scouting**
* `availableUnits` - amount of units idle
* `seed` - seed of the map
* `wood` - amount of wood reserved
* `food` - amount of food reserved
* `stone` - amount of stone reserved


---
## The Commands

Bases have a set amount of units that they can dictate actions to.

In each game tick, a base can submit a list of commands for their units. This command list **cannot send more** units than the base has available.

The commands are a list of actions that a unit can perform once they have reached the target node.

_Note: ids are used to determine the target node not the x, y value_

Each unit can perform the five commands.

---
### All Commands

* SCOUT
* FARM
* MINE
* LOG
* START_CAMPFIRE
---
### Command Structure

Commands can be sent to the engine as often as you like, but the engine does not wait for commands from the bots and processes the game state at a set rate.

The Runner will only allow a maximum of one 'command' but many actions in a list per tick.

Your commands will be lodged against your bot for processing during the next tick. These actions are processed under FIFO (First in, First out), meaning your earliest sent action is processed first. However, this list of actions should only ever be one command long.

This means that your bot does not need to send a command each tick and can take as long as it wants to send each command. Feel free to run all the clever artificial intelligence you like! Just note that other bots might still be sending commands as there is no wait time.

Example Payload in JSON with types:

```jsonc
{
  "playerId" : "801fee9f-b05d-485e-afdf-84d3fc19bbfc",  // id of the bot - string/GUID/UUID
  "actions" : [                                         // list of actions
    {                                       
      "type" : 1,                                       // action type -  int
      "units" : 4,                                      // number of units - int
      "id" : "06239e36-bd91-4932-ba23-b5eb753f3a22"     // id of the destination node -string/GUID/UUID
    },
    {
      "type" : 3,                                       // action type -  int
      "units" : 3,                                      // number of units - int
      "id" : "f051f9a0-a5a3-418a-aaed-1d765279794b"     // id of the destination node -string/GUID/UUID
    },
    ...                                                 // and so on..
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
    "id" : "410d392c-ecf5-43b9-a228-299c0a8d224a" // destination node - string/UUID/GUID
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
    "id" : "8b3f69b7-bcda-490e-8b6a-5a1231d5119f" // destination node - string/UUID/GUID
}
```
---
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
    "id" : "13a19dfd-b05e-4d6d-883a-77d45cf31b75" // destination node - string/UUID/GUID
}
```
---
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
    "id" : "60b71170-bbe3-4cfa-8069-87202340fc9f    " // destination node - string/UUID/GUID
}
```

### Command: START_CAMPFIRE

```
START_CAMPFIRE: 5
```

This command will send the amount of specified units to start a campfire at the village. Each unit in the action contributes a set amount of wood, so the wood usage scales with unit amount.
The campfire will be at the village and so no node is associated. An empty guid is sent through to the

Example Payload in JSON with types:
```jsonc
{
    "type" : 5,                                   // LUMBER action type - int
    "units" : 2,                                  // number of units to start a campfire at the base
    "id" : "00000000-0000-0000-0000-000000000000" // destination node - string/UUID/GUID
}
```
---
## Inter tick calculations

These calculations happen during the tick cycle 

### Travel time

For a unit To reach a node. Distance is taken into account.

if the unit if not idle or performing an action on the node. It will be travelling to the node.

Travel time is dependent on the distance from the base node to the target node. Once the distance is calculated it will be added to the units travel time. eg. if the travel time is calculated as 10 it will take 10 ticks for the unit to reach the node

For more information view the [distance calculation](#distance-calculation) section.

### Population tiers

Each base has a tier level this. Determines the population growth rate of the base. This is calculated for every tick 

eg
```
    {
      "level": 0,
      "name": "Tier 0",
      "maxPopulation": 10,
      "populationChangeRange": [-0.03, 0.03]
    },
```

When population reaches the `maxPopulation` for their tier then the tier is increased

---
### Population Growth/Decline Rate

The population will change depending on the amount of food harvested.
This process happens every 10 ticks

if the population has a surplus of food the the population will increase based on the current tier level

eg:
```
total population = 5
tier level = 0
increase factor = 1
new total population = 6
```

if the population has less food then the amount of people
then the population will decrease. by a 10%

eg:
```
total population = 100
population decrease = 10
remaining population = 90
```


---
## Endgame

The bot to reach the highest score will be determined the winner.
Score is dependent on population size and amount of resources accumulated.

---
## Scoring

A score will be kept for each player which will be visible once the match is over.

*    Population: 25,
*    Food: 1,
*    Wood: 1,
*    Stone: 2


***Note** these values are representative only and are subject to change during balancing. Final values can be found in the appsettings.json of the game-engine folder.*

---

## The Math
This section is to explain the general math used for the movement and placement calculations by the engine, as well as basic functions to calculate this. These formulas have been included as functions in each starter bot.

The world uses the standard mathematical cartesian plane with the top left corner of the map being representative of the point (X=0, Y=0)

---
### Distance Calculation
Distance is calculated using the standard Pythagorean theorem, as seen below:

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

### Resource Distribution

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
Due to the fact that the engine uses integers to represent the world rounding had to be applied to certain calculations that gave decimal results.

The game engine uses C# which uses round-to-even or banker's rounding, which means any value that is at exactly at the midpoint ( x.5 ), will be rounded to the nearest even number.
Take the following examples:

- 23.5 will become 24
- 24.5 will become 24
- 24.6 will become 25
- 25.5 will become 26

A summary of those rounding decisions can be seen below:

####Distance between objects:
Uses standard Math.Round rounding.

####resource distribution calculation:
Uses standard Math.Round rounding

Note when doing rounding calculations in your own bot check the rules of the specific language as some use different rounding strategies to C#.

No language will have a advantage as only integer values are used in commands and the engine calculates all bot commands in the same way.
This section is for reference to check that your bot calculations and decision points align with how the engine works.


---