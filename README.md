# A-Star-pathfinding-system
This is a algorithmn demonstration of the A* grid system implemented in Unity 2021 and there is a comparison with the Built in Unity Navmesh System
## Instructions of the Unity Demo
There are two versions of the A* system in my project however v2 is only one that works properly in scene

There is the now standard pathfinding system Navmesh that I compared it to where it's the same scene except with the navmesh system instead

### To run A* pathfinding scene
The grid script is in the A* gameobject on the scene where it will display the grid in both the game and scene display you can just manipulate it freely.

Clicking on the game screen on the scene will move the player object to anywhere on the plane which in turn shows the new path on screen on update

WARNING THE APP WITH A LARGE NODE COUNT WILL TAKE A TOLL ON THE SYSTEM atm

probably be better with ecs grid implementation once I learn that

### To run Navmesh scene
on NavMeshDemo scene simply press the spacebar in the keyboard to initiate travel to the target destination in the scene it's cube (5)

## Resources 
A* introduction - http://theory.stanford.edu/~amitp/GameProgramming/AStarComparison.html
