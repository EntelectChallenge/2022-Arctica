#include "ScoutTower.h"
#include "Node.h"
#include <vector>

//
// Created by megan.humphreys on 2022/03/21.
//

#ifndef CPPBOT_MAP_H
#define CPPBOT_MAP_H

struct Map
{
    std::vector<ScoutTower> scoutTowers;
    std::vector<Node> nodes;
};

#endif //CPPBOT_MAP_H
