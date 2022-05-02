using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class UserTest
{

    private string testUsername = "sheffield";
    private string expectedEmail = "jluong1@sheffield.ac.uk";
    private int expectedImagesLoaded = 478;

    User user;

    [UnitySetUp]
    public IEnumerator Setup(){
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
        while (SceneManager.GetActiveScene().name != "SampleScene")
        {
            // wait for scene to load..
            yield return new WaitForSeconds(.1f);
        }

        // load the user
        GameObject player = GameObject.Find("XR Origin");
        user = player.GetComponent<User>();
        user.Login(testUsername);
        while (!user.loggedIn)
        {
            // wait for user to login..
            yield return new WaitForSeconds(.1f);
        }

        // initialise the main display with thumbnails
        Gallery galleryComponent = GameObject.Find("Gallery Scroll View").GetComponent<Gallery>();
        galleryComponent.initPhotos();

        while(!user.libraryPhotos.loaded){
            // keep waiting until all photos have loaded...
            yield return new WaitForSeconds(.1f);
        }
    }

    [UnityTest]
    public IEnumerator PhotosLoaded()
    {   
        int imagesLoaded = user.libraryPhotos.allPhotos.Count;
        if(imagesLoaded == 0){
            Assert.Fail("Could not load images for the test, save did not exist?");
        }else{
            Assert.AreEqual(expectedEmail, user.email);
            Assert.AreEqual(expectedImagesLoaded, imagesLoaded);
        }
        yield return null;
    }

    [UnityTest]
    public IEnumerator ThumbnailsLoaded()
    {   
        Gallery galleryComponent = GameObject.Find("Gallery Scroll View").GetComponent<Gallery>();

        // count number of thumbnails
        // the date headers have no children, so its fine to add 0 to thumbnailCount
        int thumbnailCount = 0;
        for (int i = 0; i < galleryComponent.content.transform.childCount; i++)
        {
            Transform child = galleryComponent.content.transform.GetChild(i);
            thumbnailCount += child.childCount;
        }

        // number of thumbnails should be equal to images loaded
        Assert.AreEqual(expectedImagesLoaded, thumbnailCount);
        yield return null;
    }
}
