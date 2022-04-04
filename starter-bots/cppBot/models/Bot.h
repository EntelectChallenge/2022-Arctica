#include <vector>
#include <iostream>
#include "BotMap.h"
#include "Position.h"

//
// Created by megan.humphreys on 2022/03/21.
//

#ifndef CPPBOT_BOT_H
#define CPPBOT_BOT_H

struct Bot
{
    std::string id;
    int currentTierLevel;
    int tick;
    BotMap map;
    int population;
    int travellingUnits;
    int farmingUnits;
    int scoutingUnits;
    int lumberingUnits;
    int miningUnits;
    int availableUnits;
    int seed;
    int wood;
    int stone;
    
    Position baseLocation;

#endif //CPPBOT_MAP_H
