using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class display_cam_on_img : MonoBehaviour
{
    // Start is called before the first frame update
    public WebCamDevice cam;
    public bool hasCam = false;
    WebCamTexture tex;
    IEnumerator cam_start()
    {
        //yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        //yield return new WaitForSeconds(1);

        //Permission.HasUserAuthorizedPermission(Permission.Camera)

        if (WebCamTexture.devices.Length > 0)
        {
            cam = WebCamTexture.devices.ToList().Find(x => x.isFrontFacing);
            yield return new WaitForSeconds(0.5f);
            tex = new WebCamTexture(cam.name);
            Debug.Log(cam.name);

            GameObject.FindObjectOfType<RawImage>().texture = tex;
            //yield return new WaitForSeconds(2f);
            //tex.Stop();
            if (!tex.isPlaying)
            {
                tex.Play();
            }
            //yield return new WaitForSeconds(0.8f);
            //yield return new WaitForSeconds(10);
            //tex.Pause();
            //hasCam = true;
            //Debug.Log(cam.name);
        }
        else
        {
            Debug.Log("No camera");
        }
    }
        void Start()
    {
        //Application.RequestUserAuthorization(UserAuthorization.WebCam);
        StartCoroutine(cam_start());

    }

    // Update is called once per frame
    void Update()
    {
        if (hasCam)
        {


        }
    }
}
