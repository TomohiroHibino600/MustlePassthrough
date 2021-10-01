using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MustlePassthrough
{
    /// <summary>
    /// ‹ØƒgƒŒ‚Ìí–Ú‚â‚»‚Ì‰ğà‚ğ¦‚·View‚ÌŠÇ—
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
