/* 
 * This message is auto generated by ROS#. Please DO NOT modify.
 * Note:
 * - Comments from the original code will be written in their own line 
 * - Variable sized arrays will be initialized to array of size 0 
 * Please report any issues at 
 * <https://github.com/siemens/ros-sharp> 
 */

using Newtonsoft.Json;

namespace RosSharp.RosBridgeClient.MessageTypes.Gazebo
{
    public class SetPhysicsPropertiesResponse : Message
    {
        [JsonIgnore]
        public const string RosMessageName = "gazebo_msgs/SetPhysicsProperties";

        public bool success;
        //  return true if set wrench successful
        public string status_message;
        //  comments if available

        public SetPhysicsPropertiesResponse()
        {
            this.success = false;
            this.status_message = "";
        }

        public SetPhysicsPropertiesResponse(bool success, string status_message)
        {
            this.success = success;
            this.status_message = status_message;
        }
    }
}
