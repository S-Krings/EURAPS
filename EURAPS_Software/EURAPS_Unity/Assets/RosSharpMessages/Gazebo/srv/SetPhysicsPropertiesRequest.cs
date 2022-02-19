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
    public class SetPhysicsPropertiesRequest : Message
    {
        [JsonIgnore]
        public const string RosMessageName = "gazebo_msgs/SetPhysicsProperties";

        //  sets pose and twist of a link.  All children link poses/twists of the URDF tree will be updated accordingly
        public double time_step;
        //  dt in seconds
        public double max_update_rate;
        //  throttle maximum physics update rate
        public Vector3 gravity;
        //  gravity vector (e.g. earth ~[0,0,-9.81])
        public ODEPhysics ode_config;
        //  configurations for ODE

        public SetPhysicsPropertiesRequest()
        {
            this.time_step = 0.001;
            this.max_update_rate = 1000.0;
            this.gravity = new Vector3(0f,0f,-9.81f);
            this.ode_config = new ODEPhysics();
        }

        public SetPhysicsPropertiesRequest(double time_step, double max_update_rate, Vector3 gravity, ODEPhysics ode_config)
        {
            this.time_step = time_step;
            this.max_update_rate = max_update_rate;
            this.gravity = gravity;
            this.ode_config = ode_config;
        }
    }
}
