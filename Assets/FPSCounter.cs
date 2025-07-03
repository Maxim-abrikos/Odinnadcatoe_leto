using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace StarterAssets
{
    public class FPSCounter : MonoBehaviour
    {
        public TextMeshProUGUI fpsText;
        private float deltaTime;

        void Update()
        {
            deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
            float fps = 1.0f / deltaTime;
            fpsText.text = $"FPS: {Mathf.Ceil(fps)}";
        }
    }
}