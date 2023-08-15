# Unity-Roguelike
![GameView](https://github.com/omar-owis/Unity-Roguelike/blob/main/Readme%20Images/image0.png)

## Vision
Dungeon Man is an innovative and exhilarating fusion of the fast-paced combat mechanics from the popular battle royale game Spellbreak and the engaging roguelike elements. Set in a mystical realm of ever-changing landscapes and ancient dungeons, players will embark on an epic journey to harness their magical prowess, gather powerful artifacts, and ultimately face off against a formidable final boss.

## Development Details
This repository contains an ongoing Unity roguelike game project, showcasing several key game features. These features encompass a flexible Inventory system with item creation tool, procedural terrain generation tool, a versatile player controller, and a custom ability creator. Each of these elements is designed to contribute to the overall gameplay experience, enhancing the game's depth and engagement.

The game's development is centered around Unity-3D, a popular game engine known for its capabilities in creating immersive and interactive experiences. Leveraging Unity-3D provides a solid foundation for building and iterating upon the various components that comprise the game.

Notably, the repository distinguishes itself by employing internal assets for its pivotal game features. External assets are limited in usage, primarily reserved for temporary player character model, player animations, prototype grids, and freelook camera control facilitated by Cinemachine. This approach ensures a level of originality and control over the game's core mechanics.

The existing progress on the project spans approximately six months of development effort. Detailed documentation of daily advancements can be accessed through the [progress logs folder](https://github.com/omar-owis/Unity-Roguelike/tree/main/Progress%20Logs), offering insight into the iterative process that has driven the game's evolution.

The entirety of the project's advancement has developed and designed by a solo developer.

## Inventory System and Item Creation Tool

![Inventory](https://github.com/omar-owis/Unity-Roguelike/blob/main/Readme%20Images/image1.png)

The flexible inventory system constitutes a vital gameplay component, offering a range of item management features that enrich the player experience:
- Items can be picked up from the game world by the parent entity.
- Inventories supports item dropping.
- Opening and closing the inventory is integrated into the gameplay mechanics.
- Dynamic and static inventories can be easily added within the Unity editor.
- New items can be conveniently created and integrated using the Unity editor.
- Item creation tool has diverse item types with distinct functionalities contribute to gameplay variety.
- Items can be swapped within inventories.
- Customizable inventory user interface (UI) through unity editor

## Procedural Terrain Generation Tool
The game incorporates an innovative infinite procedural terrain generation tool, which leverages industry-standard techniques such as Perlin noise to create diverse and dynamic landscapes. This tool encompasses various features:
- Terrain generation settings, including noise scale, octaves, persistence, lacunarity, seed, offset, terrain height multiplier, and height curve.

![Terrain Settings](https://github.com/omar-owis/Unity-Roguelike/blob/main/Readme%20Images/image2.png)

- Rendering of terrain includes a level of detail mechanic for optimization.

![LOD settings](https://github.com/omar-owis/Unity-Roguelike/blob/main/Readme%20Images/image3.png)

- Unloading and loading of terrain collision relative to player for optimization.
- Use of treading in terrain generation for optimization.
- Terrain shader graph that utilizes tri-planar texture mapping

## Player Controller

![Player Controller Settings](https://github.com/omar-owis/Unity-Roguelike/blob/main/Readme%20Images/image4.png)

The game's versatile player controller introduces a physics-based 3D movement system, complemented by a hierarchical state machine software design pattern. 
- Use of factory software design pattern to create the current player root state and sub state.
- Current implemented player root-states: Jump state, Fall state, Grounded state, and Levitation State.
- Current Implemented player sub-states: Idle state, Walk state, Run state, Dash state, External-movement state.
- Implementation allows for seamless scalability for both root-states and sub-states
- Implementation extends for other entities to use same controller logic with identical or distinct states
## Ability Creator Tool

![Ability Creator Tool](https://github.com/omar-owis/Unity-Roguelike/blob/main/Readme%20Images/image5.png)

A unique custom ability creator tool serves as a standout feature, utilizing C# async to execute abilities during run-time. Key attributes of this tool include:

- Abilities can be easily designed using pre-defined ability behaviors.
- The framework supports expandability of ability behaviors, accommodating future enhancements.
- Custom abilities can be created and integrated using the custom editor within unity.
- A dedicated custom editor streamlines the ability creation process, promoting developer-friendly design.
