// Camera mobile pinch/pan taken from Youtube user Waldo
// https://www.youtube.com/watch?v=0G4vcH9N0gc

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // TODO fill these values based on dungeon generation
    public float zoomMin;
    public float zoomMax;

    public float xMin = -10;
    public float xMax = 100;

    public float yMin = -10;
    public float yMax = 100;

    Vector3 touchStart;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if(Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPreviousPosition = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePreviousPosition = touchOne.position - touchOne.deltaPosition;

            float previousMagnitude = (touchZeroPreviousPosition - touchOnePreviousPosition).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - previousMagnitude;

            zoom(difference * 0.01f);
        }
        else if(Input.GetMouseButton(0))
        {
            Vector3 direction = touchStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 position = Camera.main.transform.position + direction;
            position.x = Mathf.Clamp(position.x, xMin, xMax);
            position.y = Mathf.Clamp(position.y, yMin, yMax);
            Camera.main.transform.position  = position;
        }
        zoom(Input.GetAxis("Mouse ScrollWheel") * 15);
    }

    private void zoom(float increment)
    {
        float size = Mathf.Clamp(Camera.main.orthographicSize - increment, zoomMin, zoomMax);
        Camera.main.orthographicSize = size;
    }

    public void zoomOutAndCenter(Vector2 center)
    {
        Camera.main.orthographicSize = zoomMax;
        Camera.main.transform.position = new Vector3 (center.x, center.y, -10);
    }

    public void panAndZoomTo(DungeonSector d1, DungeonSector d2)
    {
        Camera.main.orthographicSize = Mathf.Max(d1.size.x + d2.size.x, d1.size.y + d2.size.y);
        transform.position = new Vector3((d1.position.x + d2.position.x + d1.size.x + d2.size.x) / 2, (d1.position.y + d2.position.y + d1.size.y + d2.size.y) / 2, -10);
    }
    public void panAndZoomTo(DungeonSector d)
    {
        Camera.main.orthographicSize = Mathf.Max(d.size.x, d.size.y);
        transform.position = new Vector3((d.position.x + d.size.x) / 2, (d.position.y + d.size.y) / 2, -10);
    }

}
