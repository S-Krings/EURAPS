#!/bin/bash 
cd ~/catkin_ws 

pkill Xvfb
tmux new-session -d 'source devel/setup.bash && roslaunch dobot_description dobot_control.launch' 

tmux split-window -h 'source devel/setup.bash && sleep 5 && xvfb-run --auto-servernum --server-num=1 roslaunch gazebo_simulation_scene dobot_simulation_scene.launch' 
tmux attach -t 0 
