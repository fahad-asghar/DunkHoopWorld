using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AdjustDemo
{
    public class ToastTools : MonoBehaviour
    {
        public Text text;
        private Coroutine timer;

        // private void Awake()
        // {
        //     this.text = GetComponentInChildren<Text>();
        // }

        private IEnumerator SecondLayerCoroutine()
        {
            yield return new WaitForSeconds((float) 1.5);
            gameObject.SetActive(false);
            timer = null;
        }

        public void Toast(string str)
        {
            this.text.text = str;
            gameObject.SetActive(true);
            if (timer != null)
            {
                StopCoroutine(timer);
            }

            timer = StartCoroutine(SecondLayerCoroutine());
        }
    }
}