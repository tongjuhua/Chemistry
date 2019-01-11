using UnityEngine;

namespace Conversion {

    public class ConversionUtils {
        // Vector3 to string
        public static string Vector2String(Vector3 vec) {
            string str = vec.x + " " + vec.y + " " + vec.z;
            return str;
        }

        // string to Vector3
        public static Vector3 String2Vector(string str) {
            string[] strs = str.Split(' ');
            return new Vector3(float.Parse(strs[0]), float.Parse(strs[1]), float.Parse(strs[2]));
        }

        // Quaternion to string
        public static string Quaternion2String(Quaternion qt) {
            string str = qt.x + " " + qt.y + " " + qt.z + " " + qt.w;
            return str;
        }

        // string to Quaternion
        public static Quaternion String2Quaternion(string str) {
            string[] strs = str.Split(' ');
            return new Quaternion(float.Parse(strs[0]), float.Parse(strs[1]), float.Parse(strs[2]), float.Parse(strs[3]));
        }

    }

}
