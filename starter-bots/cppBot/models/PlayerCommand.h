#include <vector>
#include <iostream>
#include <optional>
#include "PlayerAction.h"

#ifndef CPPBOT_PLAYERACTION_H
#define CPPBOT_PLAYERACTION_H

// {"PlayerId":"c0f75c6a-fffe-47de-876c-a6f5dbbffda3","Actions":[{"Type":1,"Units":1,"TargetNode":{"x":18,"y":18}}]}

struct PlayerCommand
{
    std::string playerId;
    std::vector<PlayerAction> actions;
};

#endif //CPPBOT_PLAYERACTION_H
