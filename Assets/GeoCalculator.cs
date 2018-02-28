using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeoCalculator {

    static Vector3 moereMountainPos = new Vector3(141.425995f, 0f, 43.122080f);

    GeoCalculator() { }
    ~GeoCalculator() { }

    public static Vector3 ToXYZ(float latitude, float longitude, float height)
    {
        float z = (latitude - moereMountainPos.z) * 90f;
        float x = (longitude - moereMountainPos.x) * 65f;
        float y = height / 1700.0f;

        return new Vector3(x, y, z);
    }

}
