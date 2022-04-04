#include "../enums/ActionTypes.h"

//
// Created by megan.humphreys on 2022/03/21.
//

// Example format: [{"Type":1,"Units":1,"TargetNode":{"x":18,"y":18}}]}

#ifndef CPPBOT_ACTION_H
#define CPPBOT_ACTION_H

struct PlayerAction
{
    ActionTypes type;
    int units;
    std::string id;
};

#endif //CPPBOT_ACTION_H
