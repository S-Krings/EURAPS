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
    public class GetModelPropertiesResponse : Message
    {
        [JsonIgnore]
        public const string RosMessageName = "gazebo_msgs/GetModelProperties";

        public string parent_model_name;
        //  parent model
        public string canonical_body_name;
        //  name of canonical body, body names are prefixed by model name, e.g. pr2::base_link
        public string[] body_names;
        //  list of bodies, body names are prefixed by model name, e.g. pr2::base_link
        public string[] geom_names;
        //  list of geoms
        public string[] joint_names;
        //  list of joints attached to the model
        public string[] child_model_names;
        //  list of child models
        public bool is_static;
        //  returns true if model is static
        public bool success;
        //  return true if get successful
        public string status_message;
        //  comments if available

        public GetModelPropertiesResponse()
        {
            this.parent_model_name = "";
            this.canonical_body_name = "";
            this.body_names = new string[0];
            this.geom_names = new string[0];
            this.joint_names = new string[0];
            this.child_model_names = new string[0];
            this.is_static = false;
            this.success = false;
            this.status_message = "";
        }

        public GetModelPropertiesResponse(string parent_model_name, string canonical_body_name, string[] body_names, string[] geom_names, string[] joint_names, string[] child_model_names, bool is_static, bool success, string status_message)
        {
            this.parent_model_name = parent_model_name;
            this.canonical_body_name = canonical_body_name;
            this.body_names = body_names;
            this.geom_names = geom_names;
            this.joint_names = joint_names;
            this.child_model_names = child_model_names;
            this.is_static = is_static;
            this.success = success;
            this.status_message = status_message;
        }
    }
}
