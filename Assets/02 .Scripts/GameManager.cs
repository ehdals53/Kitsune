using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Texture2D[] cursers;


    private void Awake()
    {
        instance = this;
        Cursor.SetCursor(cursers[0], Vector2.zero, CursorMode.Auto);

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
