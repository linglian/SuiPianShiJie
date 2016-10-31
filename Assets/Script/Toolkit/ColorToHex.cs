using UnityEngine;
using System.Collections;

public class ColorToHex {
    public static string colorToHex(Color color) {
        string str;
        int r = (int)(color.r * 255f);
        str = "#"+r.ToString("x2");
        int g = (int)(color.g * 255f);
        str +=  g.ToString("x2");
        int b = (int)(color.b * 255f);
        str +=  b.ToString("x2");
        return str;
    }
}
