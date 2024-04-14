using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class StandardCards
{
    //// A Test behaves as an ordinary method
    //[Test]
    //public void StandardCardsSimplePasses()
    //{
    //    // Use the Assert class to test conditions
    //}

    //// A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    //// `yield return null;` to skip a frame.
    //[UnityTest]
    //public IEnumerator StandardCardsWithEnumeratorPasses()
    //{
    //    // Use the Assert class to test conditions.
    //    // Use yield to skip a frame.
    //    yield return null;
    //}


    [UnityTest]
    public IEnumerable CardsOnDragTest()
    {
        SceneManager.LoadScene("Scenes/Classic");
        yield return null;
    }

}
