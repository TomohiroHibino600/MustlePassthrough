using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace MustlePassthrough
{

    public class Model : MonoBehaviour
    {
        public IntReactiveProperty TrainIndex = new IntReactiveProperty(0);
        public FloatReactiveProperty TrainScore = new FloatReactiveProperty(0f);
        public ReactiveCollection<float> TrainScores = new ReactiveCollection<float>();
        public FloatReactiveProperty TotalScore = new FloatReactiveProperty(0f);
        public string[] TrainName = {
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
