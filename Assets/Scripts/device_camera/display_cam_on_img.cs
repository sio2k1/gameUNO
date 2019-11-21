using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;


// this script defines selfie camera i 
public class display_cam_on_img : MonoBehaviour
{

    public RawImage cam_out; // display camera content here
    WebCamTexture webCamTexture; 

    public byte[] lastcamerashoot=null; // bytes of last shoot


    public void startCamera() // start camera
    {

        if (WebCamTexture.devices.Length > 0) // if we have some cameras
        {
            cam_out.enabled = true;
            var d = WebCamTexture.devices.ToList().Find(x => x.isFrontFacing); // select selfie camera
            webCamTexture = new WebCamTexture(d.name); // intit webtexture on particular selfie camera
            webCamTexture.requestedWidth = 320;
            webCamTexture.requestedHeight = 240;
            cam_out.texture = webCamTexture; // output cam to image
            webCamTexture.Play(); // launch camera
            
        }
    }

    public void stopCamera()
    {
        if (webCamTexture!=null) //if cam was initialized
        {    
            webCamTexture.Stop(); // shut down
        }
    }

    public void takePhoto() // launch take photo in thread (we need to wait some time for frame draw so we use can use WaitForEndOfFrame(); inside)
    {
        StartCoroutine(TakePhoto());
    }

    IEnumerator TakePhoto()  // Start this Coroutine on some button click
    {

        // NOTE - you almost certainly have to do this here:
        yield return new WaitForSeconds(0.1f);
        yield return new WaitForEndOfFrame(); // to avoid black camera shoots

        Texture2D photo = new Texture2D(webCamTexture.width, webCamTexture.height);
        photo.SetPixels(webCamTexture.GetPixels());
        photo.Apply();
        webCamTexture.Pause();
        lastcamerashoot = photo.EncodeToJPG(85);

        //Encode to a PNG // degugging code
        //byte[] bytes = photo.EncodeToJPG(80);
        //File.WriteAllBytes(Application.streamingAssetsPath + "/pic.jpg", bytes);
    }

}
