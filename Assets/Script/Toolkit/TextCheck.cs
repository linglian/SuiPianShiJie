using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public class TextCheck {
    public static bool isUserName(string str) {
        Regex reg = new Regex(@"\W+");
        return !reg.IsMatch(str);
    }
}
