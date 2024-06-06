using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class UnixTime 
{
    static public double GetTime()
    {
        var epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
        var timestamp = (System.DateTime.UtcNow - epochStart).TotalSeconds;
        return timestamp;
    }
}
