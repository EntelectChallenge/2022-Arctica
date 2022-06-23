# Game Engine

This project contains the Game Engine, Runner, and Logger for the Entelect Challenge 2022

## Game Rules

The game for 2022 is Arctica. Detailed game rules can be found [here](game-rules.md)

## Configuration Options

The engine will respect the following environment variables to change how the game is run:

- `BOT_COUNT`
    - This sets the expected amount of bots to connect before a game will be run
 
- `WORLD_SEED`
    - This sets the seed of the world, which is used to generate the map and resource distribution 

When these are not specified, the values present in `appsettings.json` will be used.

## Bespin Release Format

For cloud squad the gitlab config needs to be configured for each new release/bugfix:

`year.phase.version`
