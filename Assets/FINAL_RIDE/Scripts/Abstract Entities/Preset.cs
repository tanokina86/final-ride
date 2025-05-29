using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditorInternal;

//[CreateAssetMenu(fileName = "Entity", menuName = "WeaponPreset / Entity")]
public sealed class WeaponPreset : ExcludeFromPresetAttribute
{
    private GameObject gameObject = null;

    [SerializeField]
    private SortingLayer renderLayer;

    public GameObject GameObject { get => gameObject; set => gameObject = value; }

    public List<Component> Components { get; private set; }

    public WeaponPreset()
    {
        //SetBaseComponents();
    }


    public WeaponPreset(GameObject gameObject)
    {
        if (gameObject != null)
        {
            this.gameObject = gameObject;
            
            SetBaseComponents();
        }
    }

    //partial class BaseComponents
    //{
    //    private readonly IEnumerable<Component> components = new List<Component>() { new Rigidbody2D(), new SpriteRenderer(), new Collider2D() };

    //    public IEnumerable<Component> Components => components;

    //    //public Dictionary<string, GameObject> Prefabs { get;}


    //    public BaseComponents()
    //    {
    //        //components = new List<Component>()
    //        //{
    //        //    new Rigidbody2D() { name = "Rigidbody2D", tag = string.Empty, isKinematic = true, bodyType = RigidbodyType2D.Kinematic, simulated = true,},
    //        //    new SpriteRenderer() { name = "SpriteRenderer", sprite = null, color = Color.grey},
    //        //    new BoxCollider2D() { name = "BoxCollider2D", autoTiling = true, isTrigger = true, enabled = true}
    //        //};
    //    }
    //}

    public void BaseInit(List<Component> components)
    { 
        int newComponets = 0;

        try
        {
            foreach(var component in components)
            {
                if (gameObject.GetComponents(component.GetType()).ToList().Contains(component)) continue;

                gameObject.AddComponent(component.GetType());

                newComponets++;
            }
        }

        finally
        {
            //Debug.Log($"{gameObject?.name} obtained {newComponets} new components");
        }
    }

    public void CreateTag(string someTag)
    {
        if (gameObject.tag.Equals(someTag)) return;

        if (!InternalEditorUtility.tags.Contains(someTag)) InternalEditorUtility.AddTag(someTag);

        gameObject.tag = someTag;
    }

    private void SetBaseComponents()
    {
        //BaseComponents baseComponents = new BaseComponents();

        //Components = new List<Component>() { new Rigidbody2D(), new SpriteRenderer(), new Collider2D() };



        BaseInit(new List<Component>() { new Rigidbody2D(), new SpriteRenderer() });
        //CreateInstance(typeof(BaseComponents)) as BaseComponents;
    }

    public List<Component> GetComponents()
    {
        return gameObject?.GetComponents<Component>().ToList();
    }

    public void SetRenderParameters(ICollection<object> parameters)
    {
        //TODO: Describe this logic
        //gameObject.GetComponent<SpriteRenderer>();
    }

    public void SetSprite(Sprite sprite, int renderOrder = 0)
    {
        if(gameObject is null) return;

        gameObject.GetComponent<SpriteRenderer>().sprite = sprite ?? null;
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = renderOrder;
    }
}
