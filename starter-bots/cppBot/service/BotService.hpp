#include "../models/PlayerCommand.h"
#include "../models/GameState.h"
#include <iostream>
#include <chrono>
#include <thread>
#include <future>

#ifndef CPPBOT_BOTSERVICE_HPP
#define CPPBOT_BOTSERVICE_HPP

class BotService {
private:
    Bot bot;
    PlayerCommand playerCommand;
    GameState gameState;
public:
    void setPlayerId(std::string &id){
        playerCommand.playerId = id;
    }

    PlayerCommand getPlayerAction() const
    {
        return playerCommand;
    }

    GameState getGameState()
    {
        return gameState;
    }

    Bot getBot() const
    {
        return bot;
    }

    void setBot(Bot& bot)
    {
        this->bot = bot;
    }

    void updateSelfState()
    {
        
    }

    void setGameState(GameState& gameState)
    {
        this->gameState = gameState;
        updateSelfState();
    }


    void computeNextPlayerAction(std::promise<void> &startTask);
};

#endif //CPPBOT_BOTSERVICE_HPP
