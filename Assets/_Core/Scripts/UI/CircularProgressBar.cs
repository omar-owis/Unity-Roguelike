using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DungeonMan
{
    public class CircularProgressBar : MonoBehaviour
    {
        [SerializeField] private bool _hideOnFull;
        private Image _image;
        void Awake()
        {
            _image = GetComponent<Image>();
            if(_hideOnFull) gameObject.SetActive(false);
        }

        public void UpdateProgressBar(float newVal)
        {
            if(newVal < 1)
            {
                gameObject.SetActive(true);
            }

            _image.fillAmount = newVal;

            if(_image.fillAmount == 1 && _hideOnFull)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
