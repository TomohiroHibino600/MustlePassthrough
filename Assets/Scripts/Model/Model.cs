using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace MustlePassthrough
{
    public class Model : MonoBehaviour
    {
        public readonly IntReactiveProperty TrainIndex = new IntReactiveProperty(0);
        public readonly IntReactiveProperty TrainNumber = new IntReactiveProperty(0);
        public readonly int GoalNumber = 10;
        public readonly string[] TrainNames = {
            "Squat",
            "PushUp",
            "Superman",
            "Plank",
            "Cycling",
            "Crunch",
            "NeatChest",
            "SideVent",
            "SidePlank"
        };
    }
}
