# Kotlin Readme

## Running

You can build the bot with the command : 
        
        mvn clean package
        
This will produce a jar file in the target directory with the name specified in the pom.xml file.

Once built you can run the bot with command :

        java -jar KotlinBot.jar
    
from the target directory.

## Testing

There are some example tests under the src/test/kotlin directory to get you going.

You can create more test files in the same directory, as long as their name ends with `Test`.