using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class display_cam_on_img : MonoBehaviour
{
    // Start is called before the first frame update

    public RawImage cam_out;
    WebCamDevice cam;
    WebCamTexture webCamTexture;

    public byte[] lastcamerashoot=null;


    public void startCamera()
    {
        if (WebCamTexture.devices.Length > 0)
        {
            cam_out.enabled = true;
            var d = WebCamTexture.devices.ToList().Find(x => x.isFrontFacing);
            webCamTexture = new WebCamTexture(d.name);
            cam_out.texture = webCamTexture;
            webCamTexture.Play();
            StartCoroutine(TakePhoto());
        }
    }

    public void stopCamera()
    {
        if (webCamTexture!=null)
        {
            if (webCamTexture.isPlaying)
            {
                
                webCamTexture.Stop();
            }
        }
        if (cam_out.enabled)
        {
            cam_out.enabled = false;
        }
    }

    public void takePhoto()
    {
        StartCoroutine(TakePhoto());
    }

    IEnumerator TakePhoto()  // Start this Coroutine on some button click
    {

        // NOTE - you almost certainly have to do this here:
        yield return new WaitForSeconds(0.1f);
        yield return new WaitForEndOfFrame();

        // it's a rare case where the Unity doco is pretty clear,
        // http://docs.unity3d.com/ScriptReference/WaitForEndOfFrame.html
        // be sure to scroll down to the SECOND long example on that doco page 

        Texture2D photo = new Texture2D(webCamTexture.width, webCamTexture.height);
        photo.SetPixels(webCamTexture.GetPixels());
        photo.Apply();
        webCamTexture.Pause();
        lastcamerashoot = photo.EncodeToPNG();

        //Encode to a PNG
        //byte[] bytes = photo.EncodeToPNG();
        //File.WriteAllBytes(Application.streamingAssetsPath + "/pic.png", bytes);
    }

    IEnumerator cam_start()
    {
       yield return new WaitForSeconds(1); /*
        if (WebCamTexture.devices.Length > 0)
        {
            List<WebCamDevice> d = WebCamTexture.devices.ToList();
            cam = d[0];

            tex = new WebCamTexture("DroidCam Source 3");
            Debug.Log(cam.name);
            cam_out.texture = tex;
            cam_out.material.mainTexture = tex;
            //tex.requestedFPS = 10;
            yield return new WaitForSeconds(10);
            //tex.Play();
            

           // Texture2D t = (Texture2D)cam_out.texture;
           // byte[] b = t.EncodeToPNG();

           // yield return new WaitForSeconds(4);
            //tex.Stop();
            //gameObject.GetComponentInChildren<MeshRenderer>().material.mainTexture = tex;
            //test_img.texture = tex;


    
            

            Texture2D newtex = new Texture2D(200,200);
           // newtex.LoadImage(b);


            yield return new WaitForSeconds(1);
            //tex.Play();
            yield return new WaitForSeconds(1);

        }
        else
        {
            Debug.Log("No camera");
        }*/
    }
    void Start()
    {
        /*
        if (WebCamTexture.devices.Length > 0)
        {
            var d = WebCamTexture.devices.ToList().Find(x => x.isFrontFacing);
            webCamTexture = new WebCamTexture(d.name);
            cam_out.texture = webCamTexture;
            webCamTexture.Play();
            StartCoroutine(TakePhoto());
        }*/

    }

    // Update is called once per frame
    void Update()
    {

    }
}
