/* 
 * This message is auto generated by ROS#. Please DO NOT modify.
 * Note:
 * - Comments from the original code will be written in their own line 
 * - Variable sized arrays will be initialized to array of size 0 
 * Please report any issues at 
 * <https://github.com/siemens/ros-sharp> 
 */

using Newtonsoft.Json;

using RosSharp.RosBridgeClient.MessageTypes.Std;

namespace RosSharp.RosBridgeClient.MessageTypes.Gazebo
{
    public class GetLightPropertiesResponse : Message
    {
        [JsonIgnore]
        public const string RosMessageName = "gazebo_msgs/GetLightProperties";

        public ColorRGBA diffuse;
        //  diffuse color as red, green, blue, alpha
        public double attenuation_constant;
        public double attenuation_linear;
        public double attenuation_quadratic;
        public bool success;
        //  return true if get successful
        public string status_message;
        //  comments if available

        public GetLightPropertiesResponse()
        {
            this.diffuse = new ColorRGBA();
            this.attenuation_constant = 0.0;
            this.attenuation_linear = 0.0;
            this.attenuation_quadratic = 0.0;
            this.success = false;
            this.status_message = "";
        }

        public GetLightPropertiesResponse(ColorRGBA diffuse, double attenuation_constant, double attenuation_linear, double attenuation_quadratic, bool success, string status_message)
        {
            this.diffuse = diffuse;
            this.attenuation_constant = attenuation_constant;
            this.attenuation_linear = attenuation_linear;
            this.attenuation_quadratic = attenuation_quadratic;
            this.success = success;
            this.status_message = status_message;
        }
    }
}
