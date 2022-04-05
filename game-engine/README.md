# Game Engine

This project contains the Game Engine for the Entelect Challenge 2022

## Game Rules

The game for 2022 is Arctica. Detailed game rules can be found [here](game-rules.md)

## Configuration Options

The engine will respect the following environment variables to change how the game is run:

- `BOT_COUNT`
    - This sets the expected amount of bots to connect before a game will be run

When these are not specified, the values present in `appsettings.json` will be used.

The game map is additionally dynamically generated based on the number of bots given.

To compute the values of these fields at runtime, each of these fields has a corresponding `fieldnameRatio` named config item in `appsettings.json`
This is ratio field is then multiplied by the bot count to generate the final world the game will start with.

## Bespin Release Format

For cloud squad the gitlab config needs to be configured for each new release/bugfix:

`year.phase.version`
