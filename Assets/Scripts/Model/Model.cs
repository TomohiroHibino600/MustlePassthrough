using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace MustlePassthrough
{
    public class Model : MonoBehaviour
    {
        public readonly IntReactiveProperty TrainIndex = new IntReactiveProperty(0);
        public readonly FloatReactiveProperty TrainScore = new FloatReactiveProperty(0f);
        public readonly ReactiveCollection<float> TrainScores = new ReactiveCollection<float>();
        public readonly string[] TrainName = {
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
