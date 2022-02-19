# EURAPS
EURAPS is an End-User Robot programming environment with AR-assisted Path Planning Strategies. It enables users without robot- or even general programming experience to program robots by themselves. This is archieved by offering interaction approaches that spare users from mentally converting between different coordinate spaces, as weell as offering an in-place simulation and direct building to the robot. EURAPS is available on the Microsoft HoloLens and on Android smartphones.
## Contents
* [Features and Preview](#features-and-preview)
  * [Preview Video](preview-video)
* [Installation and Development](installation-and-development)
  * [Unity Project and Vuforia](unity-project-and-vuforia)
  * [Lego NXT Software](lego-nXT-software)
  * [Run](run)
## Features and Preview
In EURAPS, we use Augmented Reality (AR) to let users do their programming directly in the robot's coordinate space. To make the interaction as intuitive as possible, we offer different approaches to plan/program robot paths: the Drawing method, where the user can draw the path in one go, and the Waypoints method, where they can create commands for the individual steps on the robot path. Both approaches work by moving a sphere in AR to select targets or draw a path. Created path programs are represented in a block-based manner for better overview and editing.

In addition to this, it is possible at the click of a button to simulate created programs on a virtual robot in the real robot's world space. This way errors or problems can be noticed without the risk of damage. Programs can also be built directly to the real robot via bluetooth.

A detailed walkthrough of EURAPS can be found in the evaluation section in the usage instructions (Drawing, Waypoints). Of course, both methids can be freely combined.

### Preview Video
This video gives a short overview of EURAPS' features in use.


## Installation and Development
This section will describe the steps to install and run EURAPS.

### Unity Project and Vuforia
Since we used the third-party software Vuforia, it is not legally possible to publish EURAPS as-is. We had to remove the Vuforia Code from the published projects. However, this is easily reversible by using an own Vuforia license and download.
The following steps enable readers to build and run their own EURAPS version:
1. Download the EURAPS_Software folder.
2. Import EURAPS_Unity as a Unity project.
3. Aquire a Vuforia license and license key (see https://developer.vuforia.com/license-manager)
4. Download Vuforia into the Unity Project via the Package Manager.
5. Include your new license key into the Vuforia Configuration file (Windows>Vuforia Configuration)
6. Build and Run on your preferred device.

### Lego NXT Software
In our demo, we program a Lego NXT Mindstorms robot. This requires a special software for recieving and interpreting the Bluetooth messages with the program code. Once a Lego NXT is prepared, it can be used as long as necessary.

1. Aquire the Lejos Library (https://lejos.sourceforge) and a 32-bit version of the Eclipse IDE.
2. Import the "BTNavigator.java"(EURAPS_Software/LeJOS_Code_for_NXT_Robot/BTNavigator.java) into Eclipse.
3. Connect the Lego NXT to the computer.
4. Use the Lejos tool to flash Lejos onto the NXT.
5. Via Eclipse, run the BTNavigator on the NXT (BTNavigator.java>Run as>LeJOS NXT Program)

### Run
When all devices are equipped with the correct software, remember to pair the Lego NXT robot with the AR programming device. Then, once the app is started, EURAPS will take care of the rest and the robot programming can begin.
