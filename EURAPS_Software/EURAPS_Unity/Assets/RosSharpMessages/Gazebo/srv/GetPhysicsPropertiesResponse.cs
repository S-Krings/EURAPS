/* 
 * This message is auto generated by ROS#. Please DO NOT modify.
 * Note:
 * - Comments from the original code will be written in their own line 
 * - Variable sized arrays will be initialized to array of size 0 
 * Please report any issues at 
 * <https://github.com/siemens/ros-sharp> 
 */

using Newtonsoft.Json;

using RosSharp.RosBridgeClient.MessageTypes.Geometry;
using RosSharp.RosBridgeClient.MessageTypes.Gazebo;

namespace RosSharp.RosBridgeClient.MessageTypes.Gazebo
{
    public class GetPhysicsPropertiesResponse : Message
    {
        [JsonIgnore]
        public const string RosMessageName = "gazebo_msgs/GetPhysicsProperties";

        //  sets pose and twist of a link.  All children link poses/twists of the URDF tree will be updated accordingly
        public double time_step;
        //  dt in seconds
        public bool pause;
        //  true if physics engine is paused
        public double max_update_rate;
        //  throttle maximum physics update rate
        public Vector3 gravity;
        //  gravity vector (e.g. earth ~[0,0,-9.81])
        public ODEPhysics ode_config;
        //  contains physics configurations pertaining to ODE
        public bool success;
        //  return true if set wrench successful
        public string status_message;
        //  comments if available

        public GetPhysicsPropertiesResponse()
        {
            this.time_step = 0.0;
            this.pause = false;
            this.max_update_rate = 0.0;
            this.gravity = new Vector3();
            this.ode_config = new ODEPhysics();
            this.success = false;
            this.status_message = "";
        }

        public GetPhysicsPropertiesResponse(double time_step, bool pause, double max_update_rate, Vector3 gravity, ODEPhysics ode_config, bool success, string status_message)
        {
            this.time_step = time_step;
            this.pause = pause;
            this.max_update_rate = max_update_rate;
            this.gravity = gravity;
            this.ode_config = ode_config;
            this.success = success;
            this.status_message = status_message;
        }
    }
}