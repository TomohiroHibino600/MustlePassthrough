using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MustlePassthrough
{
    /// <summary>
    /// 筋トレの種目やその解説を示すViewの管理
    /// </summary>
    public class GuidanceView : MonoBehaviour
    {
        [SerializeField] GameObject _trainNameObj = null;
        [SerializeField] TextMeshProUGUI _trainName = null;

        public void ShowTrainName(string trainName) {
            _trainName.text = trainName;
        }
        
        public void DisactiveTrainName( ) {
            _trainNameObj.SetActive(false);
        }
    }
}
