using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cake : MonoBehaviour, IPointerClickHandler
{
    public Agust_System system { get; set; }
    void Start()
    {
        system = FindObjectOfType<Agust_System>();
    }

    void Update()
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (system.is_game_start && !system.game_end)
            system.GetCake(this);
    }
}
