
![title](https://i.imgur.com/zYgA0iL.png)


#### Contributors: 
Jacob Coffland [cofflaj@wwu.edu]<br/>
Kevin Doan [doanm3@wwu.edu]<br/>
Trevor Yokoyama [Yokoyat2@wwu.edu]<br/>
Albert Furlong [Furlona@wwu.edu]
<br/>
#### CS412 Winter 2020, Professor Admed

#### Video: https://drive.google.com/file/d/18pXlGqwmBglvjwkeivUYE4XRfFFRDcS-/view?usp=sharing

Shiblitz is a rogue like dungeon crawler that uses your real world position to generate new dungeons and enemy locations. The game is intended to feel like endgame chess, with a focus on fast make or break decisions.

<img src="https://i.imgur.com/7tLI5tR.png" alt="alt text" width="100"> <img src="https://i.imgur.com/fDy9ZY8.png" alt="alt text" width="100"> <img src="https://i.imgur.com/akerTeE.png" alt="alt text" width="100"> <img src="https://i.imgur.com/hqnpd1o.png" alt="alt text" width="100">


In Shiblitz, you play as a Shiba Inu on a quest to kill monsters. Current features include GPS location, OSM integration, Sensor based map generation, Graphics/Sounds/Animations, Database, and moves selected from a shuffled deck of cards.

During the research stage of our project, we analyzed the following eight apps:<br/>
[Turf Wars](https://turfwarsapp.com/)<br/>
[Pokemon Go](https://www.pokemongo.com/en-us/)<br/>
[Tourality](htttp://www.tourality.com/)<br/>
[Ingress Prime](https://play.google.com/store/apps/details?id=com.nianticproject.ingress&hl=en​)<br/>
[Resources](​https://play.google.com/store/apps/details?id=ch.pala.resources​)<br/>
[Spec Trek](https://play.google.com/store/apps/details?id=com.spectrekking.full&hl=en)<br/>
[The Great Land Grab](http://thegreatlandgrab.com/)<br/>
[DominAnt](https://play.google.com/store/apps/details?id=eu.melkersson.antdomination&hl=en​)<br/>

Our main design goals for the final project involved simplifying and streamlining the design so that the game is both approachable and easy to understand. The scope was narrowed in order to focus in on the gameplay aspect of our app. The main goal for the app is to promote healthy physical habits through incentivizing with entertainment. Therefore, through the reward of appealing and fun gameplay, the user will want to continue hiking. In essence, the final project is meant to highlight our team efforts to keep user focus on the gameplay's mechanics through minimizing the uniportant aspects.

### App Evolution
V1:
Started on the GUI of many screens and started on the back-end server. Started a unity tile app that could be run from the app.

<img src="https://imgur.com/BGfWKXX.png">

V2: Our GUI screens are now connected and animations will be played between screens. Login feature, and seed generation have been mostly implemented.
Started on AI pathfinding and dungeon generation.

V3: GUI fully implemented and styled consistently. Message passing between android studio and database and unity mostly implemented. AI pathfinding and randomly generated dungeon implemented.

<img src="https://imgur.com/HOvUc3m.png">

V4:  Animations between screens fully finished. Database server containing character information are mostly implemented. Messaging between Android and database completed. Equipment Screen and preliminary seed claimation finished.

<img src="https://imgur.com/XOPp5jW.png"> 

V5: Finish data passing. Finish gameplay mechanics in-scope.

## Final Product:
Criteria Fufilled:
- Sensors<br/>
- Google Maps<br/>
- Animations<br/>
- Sound/Music<br/>
- GPS<br/>

Summary: <br/>
The Shitblitz app is composed of 3 different parts. The MongoDB database, the Android Studio GUI and the Unity Game. Each of these 3 components work together to provide a entertaining and reliable system that is built to motivate the user to explore their local hikes and enviorment. The database allows for unique player profiles which will track their progress through the Unity game as well as their hikes that they have finished through the use of the Seed function of the Shiblitz App.

## MVC Architecture:

Model Files:<br/>
- Peak.java<br/>
- Seed.java<br/>
- User.java <br/>
- Parameter.java<br/>
- Player.java <br/>
    
Summary:<br/>
    The model provides the integration of the GUI, Database and the Unity Project. These files serve to provide the user with the ability to store their progress both in hiking their local enviorments as well as their progress through the Unity Dungeons.

View Files:<br/>
- activity\_blitz.xml        
- activity\_main.xml<br/>
- activity\_mapactivity.xml  
- activity\_menu.xml    
- activity\_unity.xml   
- login\_layout.xml   
- register\_layout.xml<br/>

Summary:<br/>
    The Views serve to graphically show the user their progress through the app as well as in their hikes. The views control the GUI and button placement that allows the user to navigate the application. Furthermore, the View helps to display the user's in-game stats. Mainly the View serves as the main access point to each of the other components of the app. (The Database and Unity Game)

Controller Files:<br/>
- MainActivity.java<br/>
- BlitzActivity.java<br/>
- LoginActivity.java<br/>
- MenuActivity.java<br/>
- UnityActivity.java   
- MapActivity.java<br/>   

Summary:<br/>
    The controller class allows the user navigate their way through the app. Through the available buttons and input fields, the user is able to navigate through the GUI. Furthermore, the user is able to quickly and clearly select the activity that they wish to utilize. These activities being either the Google Maps activity or the Unity Game activity. 

#### Cards:
<img src="https://i.imgur.com/wAcW51A.png" alt="alt text" width="100"> <img src="https://i.imgur.com/G17rC8z.png" alt="alt text" width="100"> <img src="https://i.imgur.com/rji1JOZ.png" alt="alt text" width="100">
##### Cantrips:
Cantrips are able to be cast faster than other spells, and do not end your turn. Use them to gain life, move, or draw cards.

<img src="https://i.imgur.com/SWuutms.png" alt="alt text" width="100"> <img src="https://i.imgur.com/xh9Eu0N.png" alt="alt text" width="100"> <img src="https://i.imgur.com/s3tsEzh.png" alt="alt text" width="100">
##### Magic Spells
Magic spells are the most powerful weapons ion your arsenal. They end your turn, but have great effects. Watch out for fireball!

<img src="https://i.imgur.com/wONMNFc.png" alt="alt text" width="100"> <img src="https://i.imgur.com/nlcikFs.png" alt="alt text" width="100"> <img src="https://i.imgur.com/gWrDs66.png" alt="alt text" width="100"> <img src="https://i.imgur.com/vuHbk0y.png" alt="alt text" width="100">
#### Melee Weapons
Reusable melee strikes and movement spells. The bread and butter of any mercenary shiba.

#### Tutorial
Shiblitz takes place in short turns. Every turn, a player may cast as many cantrips as they want before ending their turn with a spell or attack. Cantrips, like their namesake, are quick spells that have slight effects and work to cycle the cards that you have available.

Should you choose or accidentally skip a turn, you will instead draw a card. 

Enemies display their next target for any move they might take. This indcludes ranged attacks as well as generic movement.

Damage is calculated on attack, while status effects like fire are calculated at the end of a turn. This means that if a skeleton fires a fireball and the fireball hits you but you move off as your move, you take no damage.

This is a game with high lethality. You can not sprint from room to room and mow down enemies. You must think, predict, and bait out attacks from opponents.

## Future work
**
We want to implement and polish game mechanics along with more interactive gameplay. The leveling system could be reforumlated to be more in-depth. There could also be more work done with regard to having the map's location and sensor's impact on dungeon generation. Through these improvements, we can get more people interested in the game.

