cmake_minimum_required(VERSION 2.8.3)
project(iseauto)

find_package(catkin REQUIRED COMPONENTS
  roscpp
  std_msgs
)

catkin_package(

)

###########
## Build ##
###########

## Specify additional locations of header files
## Your package locations should be listed before other locations
include_directories(
# include
  ${catkin_INCLUDE_DIRS}
)



## Declare a C++ executable
## With catkin_make all packages are built within a single CMake context
## The recommended prefix ensures that target names across packages don't collide
add_executable(Efficiency src/EfficiencyMap.cpp)
add_executable(Control src/Controller.cpp)
## Specify libraries to link a library or executable target against
#target_link_libraries(${PROJECT_NAME}_CCN
target_link_libraries(Efficiency
  ${catkin_LIBRARIES}
)
target_link_libraries(Control
        ${catkin_LIBRARIES}
        )

