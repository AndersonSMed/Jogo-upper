using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {
    [SerializeField]
    // Stores the life slider
    private Slider lifeSlider;

    private void Update() {
        //Everyframe get the lifeslider value
        lifeSlider.value = GameManager.Instance.GetLife();
    }

}
