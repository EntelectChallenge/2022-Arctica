#include <vector>
#include "World.h"
#include "Bot.h"

#ifndef CPPBOT_GAMESTATE_H
#define CPPBOT_GAMESTATE_H

struct GameState
{
    World world;
    std::vector<Bot> bots;
};

#endif //CPPBOT_GAMESTATE_H
