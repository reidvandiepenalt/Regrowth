using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float speed;
    Camera camera;


    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<Camera>();
    }

    void FixedUpdate()
    {
        Vector2 moveDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        transform.Translate(moveDir * speed * Time.fixedDeltaTime);
        Vector3 screenBL = camera.ScreenToWorldPoint(new Vector3(0, 0, 30));
        Vector2 centerOffset = camera.transform.position - screenBL;
        if (screenBL.x < 0)
        {
            camera.transform.position = new Vector3(TilemapManager.worldWidth + screenBL.x + centerOffset.x,
                camera.transform.position.y, -30);
        }
        if (screenBL.y < 0)
        {
            camera.transform.position = new Vector3(camera.transform.position.x,
                TilemapManager.worldHeight + screenBL.y + centerOffset.y, -30);
        }
        if (screenBL.x > TilemapManager.worldWidth) 
        {
            camera.transform.position = new Vector3(screenBL.x - TilemapManager.worldWidth + centerOffset.x,
                camera.transform.position.y, -30);
        } 
        if (screenBL.y > TilemapManager.worldHeight)
        {
            camera.transform.position = new Vector3(camera.transform.position.x,
                screenBL.y - TilemapManager.worldHeight + centerOffset.y, -30);
        }
    }
}
