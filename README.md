# Block Party VR
Collaborative Block building VR experience
Organized by GDGCentralFlorida.org

Last updated - 2/6/2018

## Purpose

Block Party VR provides an open source project based learning experience for developers getting started in VR or gaming. In this proof of concept game, we will build a multiplayer “Minecraft” like experience designed for VR. The current implementation leverages Google Firebase for client collaboration.

To maximize the impact of the project, we will focus on building a Google Cardboard app in the beginning.   Google Cardboard VR has the largest VR market share.  The platform can support iOS and Android devices.   

https://vr.google.com/cardboard/

Keep in mind, we’re trying to create a collaborative, fun, learning project to support the growth of developers.  Look forward to seeing you join our Block Party! :)

We are open to seeing Block Party VR ported to other VR and AR platforms.   While the current implementation leverages Google Firebase, it would be cool to learn other multi-play platforms too.

## Where is the source code?

All code for block party is released under MIT public license.
https://github.com/michaelprosario/BlockPartyVR

## Want to make a contribution of something you’ve learned?  

Create a feature branch and share your code!

## Who is the audience target the Block Party VR demo?
This is a project based learning experience for developers
The project should attempt interest people who typically like Minecraft

## Meetups collaborating on Block Party VR

We want to thank all the developer groups contributing to this learning experience.
- GDGCentralFlorida.org
- Unity 3D Developers of Orlando - we especially want to thank the Unity 3D developers of Orlando for collaborating on this concept.

## Features completed
- As a player, I should be able to change the types of blocks I construct in the world to add diversity to the scene.
- Make sure we can support Google Cardboard with bluetooth keyboard and game pad

## Product Backlog for Block Party VR
- Make sure delete block works from remote client
- (high priority)As a player, I should be able to add different types of shapes to add diversity to the scene; Can we integrate Google Poly? https://poly.google.com/
- Support Google DayDream elements - Teleportation
https://developers.google.com/vr/elements/teleportation
- Support Google Daydream Chase Cam
https://developers.google.com/vr/elements/chase-cam
- Support Google DayDream menu style
https://developers.google.com/vr/elements/swipe-menu
- Given I am exploring the VR scene when I place a block in the scene the system should apply small visual effects to increase interest.
- For the demo session, can we add a time limit for the player to make sure many people can see the VR demo.
- As a player, I should be able to explore a tutorial to introduce features of the app to maximize usability.  This probably should load by default the first time I load the app.
- As a player, I should see a title screen when I start the app so that I can join a block party or explore the tutorial for the experience.
- As a player, I should be able walk around a scene in a FPS style.  This makes the experience more like “survival mode” in Minecraft.
- As a player, I should be able to step on blocks.
- As a player, I should be able to collect jewels to earn points.
- Port BlockParty to Microsoft Mixed Reality to support HoloLens and other Microsoft headsets.
- Create AFrame.IO implementation of BlockPartyVR.

## Tools to Get Started

- Get started with Google VR in Unity on Android
https://developers.google.com/vr/develop/unity/get-started
- Not finding android sdk (Unity)
https://stackoverflow.com/questions/42538433/not-finding-android-sdk-unity#
- Moving forward in cardboard using first person controller script
https://stackoverflow.com/questions/37458861/moving-forward-in-cardboard-using-first-person-controller-script

## Meetup content suggestions
- Consider exploring other features of Google Firebase 
- Consider exploring other plugins for creating multiplayer VR experiences
- Integrate street maps data or Google Earth with Unity
- Explore AR technologies like Vuforia, Google ARCore, Apple ARKit, Wikitude



