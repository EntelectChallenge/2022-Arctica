#include <iostream>
#include "Position.h"
#include "RegenerationRate.h"

//
// Created by megan.humphreys on 2022/03/21.
//

#ifndef CPPBOT_NODE_H
#define CPPBOT_NODE_H

struct Node
{
    int type;
    int amount;
    int maxUnits;
    int currentUnits;
    int reward;
    int workTime;
    std::string id;
    int gameObjectType;
    Position position;
    RegenerationRate position;
};

#endif //CPPBOT_NODE_H
