# LOADING_MOTOR_DT

This README will briefly guide you to install and run the *loadng_motor_tb digital twin* for the project **PSG-453**
## General Overview
****

loading_motor_tb repo is a digital twin for the loading motor -- name -- . 

It consists of 2 parts:
- ROS package tb_digital_twin
- Unity project containing vizualization of -- name --

These 2 parts are interconnected and are communicating over the ROS bridge, however, they are also capable of running on their own, if you provide the required data (e.g. csv files / rostopic pub).

In current version, we simulate the data reception with use of .csv files that are processed in ROS. This data is then used for computation and simulation purposes.

**NB!** In this repo you **WILL NOT** find the .csv files, as they are too big. Refer to the Requirements section of this README to learn how to retrieve them. 

## Requirements
****

1. You need to have a Virtual Machine (VM) with Linux Ubuntu (or equivalent) of at least ver. 18.04

2. You need to have Unity installed

3. You need to have an establishes two-way connection between the VM and host PC

4. You need to have the .csv files required to run the project( can be found [here](https://livettu.sharepoint.com/:f:/s/PSG453PUTprojectgroup/EiC93gX70itHoPBO5sS3aMMBApxqi6LMp3AXtNC7x-fKPA?e=0xOOCw) )

### ROS requirements

1. You need to have ROS installed of at least ver. Melodic (we recommend Noetic)

2. You need to have [ROS Bridge Suite](http://wiki.ros.org/rosbridge_suite) installed 

## How To Setup
****

### ROS package setup

1. Copy tb_digital_twin in the /src folder of your ROS workspace

2. In the CMakeLists.txt file **COMMENT OUT** all the *add_executable* and *target_link_libraries* . Then compile the package with *catkin_make*. This step is required to generate messages first before running the package

3. Uncomment the previously commented parts of CMakeLists.txt file and compile again

4. Place the downloaded files(extract first) from Requirements section in (step 4) into /src/Utilities folder of your ROS workspace

5. In files: *input_current.launch* and *efficiency_map.launch* change *sejego* in the line below to **your host machine name**:

```xml
<param name="csv_file" type="str" value="/home/sejego/catkin_ws/src/Utilities/$(arg filename)" />
```

### Unity Project

1. Download Unity Hub and install Unity 2019.3

2. Add Unity Visulaization as a project in Unity Hub and launch it

3. Find scene called *Motor* and load it

4. In the scene find object *ROS Bridge and Controller* under it there is a *Bridge* module where you specify IP address of the ROS machine that runs the part above

5. Use *Advanced Motor Visualizer Component* attached to the *Load Motor* object to controll the simulation

## How To Run
****
### ROS

1. Launch the ROS Bridge according to instruction [here](http://wiki.ros.org/rosbridge_suite/Tutorials/RunningRosbridge). Make sure to use UDP launch.

2. Launch the full_*launch.launch* file of ROS.
### Unity

1. Press the play button

2. Use controls attached to the *Load Motor* object to manipulate the simulation

## 
