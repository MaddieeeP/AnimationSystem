using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class StaticMethods
{
    public static List<Transform> GetAllChildren(this Transform aParent)
    {
        List<Transform> children = new List<Transform>();

        Queue<Transform> queue = new Queue<Transform>();
        queue.Enqueue(aParent);
        while (queue.Count > 0)
        {
            var c = queue.Dequeue();

            children.Add(c);

            foreach (Transform t in c)
                queue.Enqueue(t);
        }

        return children;
    }

    public static GameObject AddImageChild(this Transform transform, string name, Color color, string spritePath, Vector2 imageSize) //FIX
    {
        //if (AssetDatabase.GenerateUniqueAssetPath(spritePath) == spritePath)
            //return null;

        GameObject imgObject = new GameObject(name);
        imgObject.transform.SetParent(transform);

        RectTransform trans = imgObject.AddComponent<RectTransform>();
        trans.localScale = Vector3.one;
        trans.anchoredPosition = new Vector2(0f, 0f);
        trans.sizeDelta = imageSize;

        Image image = imgObject.AddComponent<Image>();
        //image.sprite = (Sprite)AssetDatabase.LoadAssetAtPath<Sprite>(spritePath); //Resources.Load()
        image.sprite = (Sprite)Resources.Load<Sprite>(spritePath);

        Debug.Log(spritePath);

        image.color = color;

        return imgObject;
    }
}





public class SpriteAnimationController : MonoBehaviour
{
    public string charName;
    public string state = "idle";

    public Transform model;

    public Vector2 imageSize = new Vector2(960, 1080);
    
    public List<string> colors; //0-primary 1-hair 2-clothes 3-accessory1 4-accessory2 5-accessory3

    public Dictionary<string, Dictionary<string, Anim>> animDict;
    public static Dictionary<string, Color> colorDict;

    public void Start()
    {
        //FIX 
        charName = "minotaur";

        SetUp();
    }

    public void SetUp()
    {
        foreach (Transform part in model.GetAllChildren())
        {
            string path = "";

            if (part.name != "spriteComponent")
                continue;
            
            Destroy(part.GetComponent<Image>());
            part.GetComponent<RectTransform>().sizeDelta = imageSize;
            part.GetComponent<RectTransform>().pivot = new Vector2(0.5f,0f);
            part.localPosition = model.position - part.parent.parent.position;

            for (int num = 0; num < colors.Count; num++) //FIX - ALL SPRITES
            {
                path = "enemySprites/"+charName+"/"+part.transform.parent.parent.name+"/"+num.ToString()+".png";
                GameObject imgObject = part.transform.AddImageChild(num.ToString(), colorDict[colors[num]], path, imageSize);
            }

            path = "enemySprites/"+charName+"/"+part.transform.parent.parent.name+"/"+"lineart.png";
            GameObject lineartObject = part.transform.AddImageChild("lineart", colorDict["lineart"], path, imageSize);
        }
    }

    public void PlayAnim(string animation)
    {
        StopAllCoroutines();

        foreach (Transform part in model.GetAllChildren())
        {
            if (part.name != "spriteAnimator")
                continue;

            part.GetComponent<SpriteAnimator>().Animate(animDict[animation][part.parent.name]);
        }
    }

    public IEnumerator PlayAnimWithDelay(string animation, float delay)
    {
        yield return new WaitForSeconds(delay);
        PlayAnim(animation);
    }
}   

