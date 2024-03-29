# The Game


### The Game for Entelect Challenge 2022 is **Arctica**.
---
**NB:
The values provided within this readme are subject to change during balance phases.
Entelect will endeavour to maintain this readme file. However the most accurate values can be found in `appsettings.json` within the game-engine folder.**

---



Hello traveller 😊

You look weary.

I know it’s cold, why don’t you set up camp?🔥

You’re not the only one around these parts - I’ve seen a few others looking for shelter recently...

They need **food**, **warmth** and are happy to provide labour in return. Will you be able to create a prosperous community? Or lose them to the surrounding camps?


In this game, you will be the leader of **one** of **four** neighboring villages. Competing for resources and fighting to survive in this cold tundra 🥶

You are not alone; you have your comrades to aid you in this venture. They will do as you say. From **scouting** for information to **gathering resources**. They are your key to thriving in this unforgiving clime. If you show yourself to be a good leader and can provide a surplus of food and warmth 🔥 word will spread, and you will attract more **travellers to your base**.

However, the opposite is also true ☝️ those who do not have their needs being met will **leave** to find refuge elsewhere.

Your goal is not only to survive but thrive in this winter landscape. To expand your village and resources, you need to **outcompete** your competitors by having the biggest village at the end of the game. This is primarily based on population size, but individual resources also hold value in the final score.

** a few months later **

Ah, traveller! I see you have managed to establish a fine village in this tough landscape. Your population has grown rather impressively and your camp has begun to spread over quite the area. 
It seems that some of your villagers are rather knowledgeable as well, some of them craftsmen and builders. I wonder, will you begin to develop your village into a small town? 
It could very well help with growing your settlement and maximizing your resource gathering.

Traveller, have you seen? Over the hills there are a few more villages that are thriving, just as yours is. In fact, I've seen some of them starting to scout for food and resources rather close to the outskirts of your village. 
It seems like there might be some competition for the land and resources in these parts. It may be worthwhile to start expanding your territory to ensure that you are able to maintain control over those precious resources!
It will likely benefit you in the long term!

** one eternity later **

Traveller! Times have changed - there is very little left to explore, it is now imperative that you use your available buildings to more effectively control your territory.
Shhhhh 🤫 they are approaching fast - they come for our land...

** a short while later ** 

... They come! Traveller, they come! Hurry — protect your land from those that try take it! Send people to protect your borders against the invaders that come from other villages. 
You are no push over, traveller! Pressure them in return! Show them that you, too, are willing to fight for the land in these snowy plains. Retribution!

---

## UPDATE: Phase 3 part 2 is out!!
This new release contains a new feature that will allow you to [claim territory](#claiming-territory) from other villages, protect the resources in your territory, as well as some balancing, and a community-driven bug-fix to [improve fairness for slower bots](#slow-bot-bug-fix).

## Contents
- [The Game](#the-game)
    - [The Map](#the-map)
        - [Visibility](#visibility)
    - [Game Objects](#game-objects)
        - [Scout Towers](#scout-towers)
        - [Resource Nodes](#resource-nodes)
        - [Buildings](#buildings)
        - [Territory](#territory)
          - [Claiming Territory](#claiming-territory)
          - [Territory Recalculation](#territory-recalculation)
          - [Pressure Calculation](#pressure-calculation)
          - [Territory Representation](#territory-representation)
        - [Status Effects](#status-effects-buffsdebuffs)
    - [Game Tick Payload](#game-tick-payload)
        - [World](#world)
        - [Nodes](#nodes)
        - [Bots](#bots)
    - [The Commands](#the-commands)
        - [Command Structure](#command-structure)
        - [All Commands](#all-commands)
        - [Command stages](#command-stages)
        - [Command: SCOUT](#command-scout)
        - [Command: MINE](#command-mine)
        - [Command: FARM](#command-farm)
        - [Command: LUMBER](#command-lumber)
        - [Command: START_CAMPFIRE](#command-start_campfire)
        - [Command: QUARRY](#command-quarry)
        - [Command: OUTPOST](#command-outpost)
        - [Command: ROAD](#command-road)
    - [Inter tick calculations](#inter-tick-calculations)
      - [Travel time](#travel-time)
      - [population tiers](#population-tiers)
      - [Population Growth/Decline Rate](#population-growthdecline-rate) 
    - [Endgame](#endgame)
    - [Scoring](#scoring)
    - [The Math](#the-math)
        - [Resource Distribution](#resource-distribution)
        - [Distance Calculation](#distance-calculation)
        - [Rounding](#rounding)
    - [Slow Bot Bug Fix](#slow-bot-bug-fix)     
    - [Logging](#logging)     

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

Once you visit a scout tower, you will have access to the resource nodes and player territory surrounding the scout tower.

![starting state](images/fig1.svg)
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
Scout towers are the main driver of resource discovery. Each region on the map has a scout tower that holds all the resource node and territory information for that specific region.
They provide visibility of their region by disclosing the locations of resource nodes and territories of rival bases for the bots to make use of.

By scouting a specific tower, the bot retrieves the information from the scout tower and can now begin to harvest resources from the discovered nodes. A tower only needs to be scouted once to retrieve the region information.

The image below illustrates the regions and their scout towers, for a map with 4 regions:

![scout tower regions](images/scout-tower-regions.svg)

Each scout tower is generated and placed randomly within their respective region.

At the beginning of the game you will only have access to the scout towers as seen in the **[starting-state](#starting-state)**

Use the **[scout](#command-scout)** command to visit a scout tower to retrieve the information of the regions.

_Note: Bases who do not scout at that scout location will not have access to the area's nodes_

---

### Resource Nodes

These are locations where the player's units can harvest resources.

Resource nodes have a **maximum unit capacity** i.e they do not allow more units past a certain point.

Units from different bases can harvest resources on the same node. The distribution of resources is dependent on the **number of units** sent to harvest said resource from each base.

Each node has a `reward` field (what each unit can carry). This value is set at random, within a given range. Referred to as the  `RewardRange`. This is dependent on the resource type. i.e :

```
"Stone": { 
...
    "RewardRange": [
        1,
        7
      ],
```   

_**Note** please refer to the `appsettings.json` file under `ResourceGenerationConfig` in the `gameengine` project for all the resource values_

Bots have access to how many units are currently available on a node, as well as the remaining resources on that node.

_See **[Resource Distribution](#resource-distribution)** to see how resources are distributed._

There is a **finite amount** of resources that can be collected for certain resource nodes, namely: **stone** and **wood**. These resources will not replenish.

**Farming nodes** Will replenish after a certain amount ot ticks _see
**[Regeneration Rate](#regeneration-rate)**_

There are currently **three** kinds of resources:
- Food
- Wood
- Stone

_For how these resources are used see **[Resources](#resources)**_

After using the **[scout](#command-scout)** command to increase visibility there will be three resource node types available to extract resources from

- Farm
- Mine
- Forest

#### Farm

Infinite resource supply. Use the **[farm](#command-farm)** command on your units to **collect food** at this node.


#### Mine

Limited resource supply. Use the **[mine](#command-mine)** command on your units to **mine stone** at this node.

#### Forest

Limited resource supply. Use the **[lumber](#command-lumber)** command on your units to **chop wood** at this node.

#### Day night cycle

At the end of **10 ticks** all bots enter the night cycle of the game. This is where your people choose to leave or stay based on how much food and warmth they have.

[Territory recalculation](#territory-recalculation) also occurs here.

_For more information see **[Population](#population-growthdecline-rate)**_

### Buildings
This is a new game object that has been added in PHASE 2. Buildings come with the concept of territory: every building has a small territory around it. Each territory is a square shape with the building at the center.
Buildings can be placed almost anywhere on the map, under two conditions:
1. The building must be placed on an empty node: if there is a resource there you cannot place a building.
2. Buildings can only be placed inside your territory.
To start off with, each bot has a base/village that has a territory of its own. From here you can expand your territory outwards

#### Building Types

These are the building types that supply the following status buffs:

- Quarry - improves the collection of stone and gold
- Farmer's guild - improves the collection of food
- Lumber mill - improves the collection of wood

## Territory

When placing buildings, territory is gained on a **first come, first served** basis. 
![Territory expanding](images/expanding-territory.png)

In the image above, there are two bots that are placing buildings (represented as small dark squares), which allow them to expand their territory. Note, the blue territory was placed first, so the yellow territory cannot replace it.

**PHASE 3 UPDATE**  
As part of phase 3, we have added the ability to claim territories from other bots. It is important to do this as we have also added in a debuff when harvesting resources from other bot territories.

### Claiming Territory
Two new actions have been added, OccupyLand and LeaveLand, that allow bots to occupy land. Each piece of land corresponds to a node on the map, but only those that are already part of another territory.

#### TL;DR: very basic summary
Territory can be claimed from another bot by sending more and more units to occupy enemy territory that borders your own.
Depending on the amount of units occupying a node, at the end of each day, territory will be transferred to the bot with the most pressure. 
Pressure is directly increased by the number of units, and is described in more detail below.

#### Basic rules for claiming territory

There are a few basic rules to this functionality:
1. Units can be placed at a Land object using the OccupyLand action. 
2. Each group of Occupants on a Land object belongs to a different bot. A quantity called [Pressure](#pressure-calculation) is calculated Based on:
   1. the position of the Land (RadialWeight), 
   2. the ownership (HomeGroundWeight), 
   3. and the number of units occupying it (Count).
3. At the end of each day (10 ticks), territory recalculation happens.

For the OccupyLand action, Land can only be occupied if any of the following are true:
1. The land is in your territory
2. The is on the border of your territory
3. Or the land already has a positive amount of occupants from your bot.

LeaveLand actions can only be sent to Land that already has a positive amount of occupants from the bot.

Consider a scenario between two bots, one blue and the other purple, to demonstrate the above rules.

The below image shows, in the blue box, which territory the blue bot can claim from the purple bot.
![Blue bot claiming purple bot territory](images/BlueTakingPurple.png)

The below image shows, in the red box, which territory the purple bot can claim from the blue bot.
![Purple bot claiming blue bot territory](images/PurpleTakingBlue.png)

**The OccupyLand action is designed to both claim and protect territory.**

There is no upper bound to the amount of units that can occupy a specific piece of land, however, the only way to get them to return is to bring them all back at once with the LeaveLand action.  
Be careful about how many units you send to a specific target node!

Note: buildings cannot be claimed by another bot, only resource and available nodes. This can result in your building being inside of another bot's territory, but this won't change anything.

### Territory Recalculation
Territory ownership shifts to the dominant bot on a piece of Land at the end of every day (10 ticks).
The dominant bot is determined by the occupant group with the highest pressure.
If there are multiple occupant groups with the same amount of pressure, then territory ownership won't change until one of them breaks the stalemate.


### Pressure Calculation
Pressure is calculated using the following formula:  
```Pressure = (Count + 1) * RadialWeight * (ownsLand ? HomeGroundWeight : 1)```

The HomeGroundWeight is a simple config variable, while the RadialWeight has a formula to it.  
The RadialWeight is designed as a hyperbolic weighting centered around each bot's base. This means that Occupants on land closer to the bot's base will inherently produce a higher pressure.

The RadialWeight formula is shown below in text and the image:  
```RadialWeight = Floor(1 + 10/(radialDistanceFromBase + 0.01))```

![RadialWeight Formula](images/RadialWeightCalculation.png)

The `x` variable represents the `radialDistanceFromBase`. The original hyperbolic graph is overlayed with the floored version to get a better sense of how the weights change.


### Territory Buffs/Debuffs
The initial aim of territory was to boost harvesting actions with increased rewards based on the number of buildings that one owns. 
In this new phase, resource nodes in your territory are now protected as well. If a node is in your territory, then all other bots will yield a reduced amount of resources from it.
But remember, this goes in both directions: if you try to harvest resources inside another bot's territory, the same reduction applies.


### Territory Representation
The new classes, Land and Occupants, also serve as DTOs to show the distribution of territory in the game.
Previously Position objects were used to represent territory, but Land builds on that with the following fields:

Land: 
```json
{
  "land": {
    "X": "int",
    "Y": "int",
    "Owner": "GUID",
    "NodeOnLand": "GUID",
    "Occupants": ["List of occupant objects"]
  }
}
```

Occupants:
```json
{
    "BotId": "GUID",
    "Count": "int",
    "Pressure": "int"
}
```

---

## Status Effects (Buffs/Debuffs)
In PHASE 2 we have also added the ability to boost or decrease certain aspects of the game in order to provide a competitive advantage, improve efficiency, or boost the harvesting of resources that are vital to your strategy.

This concept is largely tied to buildings and territory at the moment and we have plans to build on this as we go along.

Currently, these are the specific buffs and debuffs that your bot can make use of:
- Reward increase when harvesting certain resources
- Travel time decrease when travelling to nodes in your territory
- *todo: insert buffs here*


---
## Game Tick Payload

### Engine config

In `tick 0` your bot will receive the full `engineConfig` file. The enginge config is collection of all the important values that the gameengine uses. Below is an example of the game engine

_note: for the complete and up to date values, please refer to the `appsettings.json` file in the `gameengine`_

```jsonc

  "RunnerUrl": "http://localhost",
  "RunnerPort": "5000",
  "BotCount": 4,
  "TickRate": 250,     // amount of time between ticks 
  "StartingUnits": 2,  /
  "StartingFood": 10,
  "RegionSize": 10,
  "BaseZoneSize": 10,
  "ProcessTick": 10,
  "NumberOfRegionsInMapLength": 4,
  "ScoutWorkTime": 2,
  "MaxTicks": 2500,  // amount of ticks per round  
  "WorldSeed": 52323, 
  "MinimumPopulation": 4,
  "PopulationTiers": [
    {
      "level": 0,
      "name": "Tier 0",
      "maxPopulation": 50,
      "populationChangeFactorRange": [
        -0.05,
        0.05
      ],
      "tierResourceConstraints": {
        "food": 0,
        "wood": 0,
        "stone": 0
      },
      "tierMaxResources": {
        "food": 333,
        "wood": 500,
        "stone": 35
      }
    },
    {
      "level": 1,
      "name": "Tier 1",
      "maxPopulation": 273,
      "tierResourceConstraints": {
        "food": 78,
        "wood": 50,
        "stone": 2
      },
      "tierMaxResources": {
        "food": 1675,
        "wood": 2513,
        "stone": 168
      },
      "populationChangeFactorRange": [
        -0.05,
        0.05
      ]
    }, ...
}
```

---
### State

All players will receive the state of the world, all game objects and all player objects at the start of each tick. The payload of each game tick will contain the following information:

```jsonc
{
    "world": {                          // world state
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
                ],
                "availableNodes" : [ 
                  "cafe19f8-029b-4673-beb5-1cc236527d97",
                  "49d19868-0b20-4bbd-8e00-be818fda0fad"
                ],
            },
            "population": 2,
            "baseLocation": {
                "x": 30,
                "y": 30
            },
            "pendingActions" : [ {
              "targetNodeId" : "1e253509-e3a7-4550-850e-dcfaebea96eb",
              "numberOfUnits" : 1,
              "tickActionCompleted" : 2460,
              "tickActionStart" : 2458,
              "tickReceived" : 2442,
              "actionType" : 1
            } ],
            "actions" : [ {
              "targetNodeId" : "99afa6c1-59af-4eae-a88a-2ea570d3ac51",
              "numberOfUnits" : 1,
              "tickActionCompleted" : 2453,
              "tickActionStart" : 2445,
              "tickReceived" : 2442,
              "actionType" : 3
            } ],
            "buildings" : [ {
              "id" : "73f3af54-3509-4efd-a891-41bbc3de9d73",
              "gameObjectType" : 1,
              "position" : {
                "x" : 10,
                "y" : 10
              },
              "territorySquare" : 1,
              "type" : 1,
              "soreMultiplier" : 0
            } ],
            "territory" : [ {
                "x" : 9,
                "y" : 9,
                "owner": "dfdcda53-d615-432c-b3f7-12ea74a1a72c",
                "nodeOnLand": "99afa6c1-59af-4eae-a88a-2ea570d3ac51",
                "occupants": [
                    {
                        "BotId": "dfdcda53-d615-432c-b3f7-12ea74a1a72c",
                        "Count": 10,
                        "Pressure": 15
                    },
                    ...
                ]
              }, {
                "x" : 9,
                "y" : 10
              },
              //... 
              "statusMultiplier": {
                "woodReward": 10,
                "foodReward": 3,
                "stoneReward": 5,
                "goldReward": 2,
                "heatReward": 5
                },
              ],
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
    * `availableNodes` - ids of the nodes available to your bot for *building* see - [buildings](#buildings)
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
  * `GOLD - 4`
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
* `pendingActions` - contains the list of actions in pending
  * `targetNodeId` - node where the action is taking place
  * `numberOfUnits` - number of units performing the action
  * `tickActionCompleted` - tick when the action is completed 
  * `tickActionStart` - tick where the action will be processed 
  * `tickReceived` - the tick that the action was received  
  * `actionType` - the type of action to be conducted
* `actions` - actions that are currently in progress
  * `targetNodeId` - same as pendingActions
  * `numberOfUnits` - same as pendingActions
  * `tickActionCompleted` - same as pendingActions 
  * `tickActionStart` - same as pendingActions 
  * `tickReceived` - same as pendingActions  
  * `actionType` - same as pendingActions
* `buildings` - list of placed buildings
  * `scoreMultiplier` - the amount that a building will increase the amount of resources collected at a time (reward)
* `territory` - the positions within your territory -  see [Territory](#territory)
* `statusMultiplier` - stores the increase of resource rewards - only applies to resources in your territory  (place [buildings](#buildings) to increase!)
  * `woodReward` - increase the amount of **wood** collected 
  * `foodReward`  - increase the amount of **food** collected 
  * `stoneReward` - increase the amount of **stone** collected 
  * `goldReward` - increase the amount of **gold** collected 
  * `heatReward` - increase the amount of **heat** collected 
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
### Command Structure

Commands can be sent to the engine as often as you like, but the engine does not wait for commands from the bots and processes the game state at a set rate.

The Runner will only allow a maximum of one 'command' **which contains a list of many `actions`**  per tick.

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
### All Commands

* SCOUT
* FARM
* MINE
* LOG
* START_CAMPFIRE
* QUARRY
* FARMERS_GUILD
* LUMBER_MILL
* ROAD
* OUTPOST
* OCCUPY_LAND
* LEAVE_LAND

---
### Command stages

There are 4 stages in which a command you issue goes through, 

- The command is issued
- The unit(s) is/are in a traveling state to the node
- The unit(s) is/are collecting resource(s)
- the unit(s) is/are returned to base with the resource(s) - immediate 

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

This command will send the amount of specified units to mine. The resource reward will depend on the resource node that you are mining. 
**Gold has just been added as a new resource type in PHASE 2 so start digging, prospector!**

Resources available to mine:
- stone
- gold

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
---
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
### Command: QUARRY

```
QUARRY: 6
```

This command will send the amount of specified units to build a quarry, 

Example Payload in JSON with types:
```jsonc
{
    "type" : 6,                                   // BUILDING action type - int
    "units" : 2,                                  // number of units to travel to node- int
    "id" : "60b71170-bbe3-4cfa-8069-87202340fc9f    " // destination node - string/UUID/GUID
}
```

---
### Command: FARMERS_GUILD

```
FARMERS_GUILD: 7
```

This command will send the amount of specified units to build a farmer's guild, 

Example Payload in JSON with types:
```jsonc
{
    "type" : 7,                                   // BUILDING action type - int
    "units" : 2,                                  // number of units to travel to node- int
    "id" : "60b71170-bbe3-4cfa-8069-87202340fc9f    " // destination node - string/UUID/GUID
}
```

---
### Command: LUMBER_MILL

```
LUMBER_MILL: 8
```

This command will send the amount of specified units to build a lumber mill, 

Example Payload in JSON with types:
```jsonc
{
    "type" : 8,                                   // BUILDING action type - int
    "units" : 2,                                  // number of units to travel to node- int
    "id" : "60b71170-bbe3-4cfa-8069-87202340fc9f    " // destination node - string/UUID/GUID
}
```
---
### Command: OUTPOST

```
OUTPOST: 9 
```

This command will send the amount of specified units to build an outpost,

Example Payload in JSON with types:
```jsonc
{
    "type" : 8,                                   // BUILDING action type - int
    "units" : 2,                                  // number of units to travel to node- int
    "id" : "60b71170-bbe3-4cfa-8069-87202340fc9f    " // destination node - string/UUID/GUID
}
```
---
### Command: ROAD

```
ROAD: 10
```

This command will send the amount of specified units to build a road,

Example Payload in JSON with types:
```jsonc
{
    "type" : 8,                                   // BUILDING action type - int
    "units" : 2,                                  // number of units to travel to node- int
    "id" : "60b71170-bbe3-4cfa-8069-87202340fc9f    " // destination node - string/UUID/GUID
}
```

---
---
### Command: OCCUPY_LAND

```
OCCUPY_LAND: 11
```

This command will send the amount of specified units to occupy the target node,

Example Payload in JSON with types:
```jsonc
{
    "type" : 11,                                   // action type - int
    "units" : 20,                                  // number of units to travel to node- int
    "id" : "60b71170-bbe3-4cfa-8069-87202340fc9f" // destination node - string/UUID/GUID
}
```

---
---
### Command: LEAVE_LAND

```
LEAVE_LAND: 12
```

This command will send one unit to inform all the occupants on a target node to vacate and return home.

Example Payload in JSON with types:
```jsonc
{
    "type" : 12,                                   // action type - int
    "units" : 1,                                  // number of units to travel to node- int
    "id" : "60b71170-bbe3-4cfa-8069-87202340fc9f" // destination node - string/UUID/GUID
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

Each base has a tier level this influences the population growth rate range of the base. This is calculated for every tick 

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

_**Note** the tier levels range from 0-8. Navigate to the appsettings.json file, under the `PopulationTiers` heading, in the gameengine project for more details_

---
## Population Growth/Decline Rate

The population will change depending on the amount of food harvested and how warm they are. 

This process happens every 10 ticks.

_**Note** stone does play a factor in the final scoring but does not effect your population growth_

In order to sustain more people, you need a greater amount of food and warmth than what your current population requires to sustain itself. This will attract more people to your base. 

_**Hint** population growth is determined by the lowest value between your excess warmth and food_

Once the population has a surplus of **food and warmth**, the population will increase based on the current [tier level](#population-tiers).


If the population has **less food or warmth** than the amount of people then the population cannot be sustained and will decrease based on the tier size


_**Note** Visit GetPopulationChange() in the calculationService.cs in the gameEngine project for your math needs. We encourage you to dig into the code to get a better understanding on the logic._ 👆

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

_Visit the calculationService.cs in the gameEngine project for your math needs. We encourage you to dig into the code to get a better understanding on the logic._ 👆

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

Each resource has a set `reward`(what each unit can carry back to base). Which is to say that: distribution is calculated the `NumberOfUnits` is multiplied by the `reward`. 

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
- 24.4 will become 24
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

### Slow Bot Bug Fix
Thanks to the community for identifying a bug in the fairness of the game.
Thanks to Kobus Van Schoor for putting together a PR that solved the issue!

A brief description of the issue and the fix:  
The game engine operates on tick cycles. Every tick it receives actions from the bot and immediately acts on them.

Resource nodes have a maximum capacity for units on them at any given moment. If this capacity is reached, no more units can be placed on the node.
When a harvesting action is received, it would instantly take up space on the resource node. If too much demand for a specific resource node happens on a given tick, then there is no space remaining for the later actions that are received in that tick.

The fix collects harvesting actions and distributes the available space at the target node between the actions.
The distribution is proportional to the number of units on each action, as a percentage of the total units from all actions that are trying to be placed. 

---

Logging

The logger is used to keep an auit trail of the game state. From each tick there is a verbose log and a condenced logs this can be toggled in the `appsettings.json` file in the `gameLogger` 

  `"CondencedLoggingToggle":  "true"`

it is toggled on by default.  

---