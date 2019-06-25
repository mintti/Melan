using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Central : MonoBehaviour
{
    public Transform invisibleKnight;
    
    List<Arranger> arrangers;

    Arranger workingArranger;
    int originalIndex;

    void Start()
    {
        arrangers = new List<Arranger>();

        var arrs = transform.GetComponentsInChildren<Arranger>();

        for(int i = 0; i < arrs.Length; i++)
        {
            arrangers.Add(arrs[i]);
        }
    }

    public static void SwapKngihts(Transform sour, Transform dest)
    {
        Transform sourParent = sour.parent;
        Transform destParent = dest.parent;

        int sourIndex = sour.GetSiblingIndex();
        int destIndex = dest.GetSiblingIndex();

        sour.SetParent(destParent);
        sour.SetSiblingIndex(destIndex);

        dest.SetParent(sourParent);
        dest.SetSiblingIndex(sourIndex);
    }
    
    void SwapKngihtHierarchy(Transform sour, Transform dest)
    {
        SwapKngihts(sour, dest);

        arrangers.ForEach(t => t.UpdateChildren());
    }


    bool ContainPos(RectTransform rt, Vector2 pos)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(rt,pos);
    }

    void BeginDrag(Transform knight)
    {
        workingArranger = arrangers.Find(t=>ContainPos(t.transform as RectTransform, knight.position));
        originalIndex = knight.GetSiblingIndex();

        SwapKngihtHierarchy(invisibleKnight, knight);
    }

    void Drag(Transform knight)
    {
        var whichArrangerKnight = arrangers.Find(t=> ContainPos(t.transform as RectTransform, knight.position));
        
        if(whichArrangerKnight == null)
        {
            bool updateChildren = transform != invisibleKnight.parent;

            invisibleKnight.SetParent(transform);

            if(updateChildren)
            {
                arrangers.ForEach(t => t.UpdateChildren());
            }
        }
        
        else
        {
            bool insert = invisibleKnight.parent == transform;

            if(insert)
            {
                int index = whichArrangerKnight.GetIndexByPosition(knight);

                invisibleKnight.SetParent(whichArrangerKnight.transform);
                whichArrangerKnight.InsertKnight(invisibleKnight, index );
            }
            else
            {
                int invisibleKnightIndex = invisibleKnight.GetSiblingIndex();
                int targetIndex = whichArrangerKnight.GetIndexByPosition(knight, invisibleKnightIndex);
                
                if(invisibleKnightIndex != targetIndex)
                {
                    whichArrangerKnight.SwapKnight(invisibleKnightIndex, targetIndex);
                }
            }
        }
    }
    void EndDrag(Transform knight)
    {
        if(invisibleKnight.parent == transform)
        {
            workingArranger.InsertKnight(knight, originalIndex);
            workingArranger = null;
            originalIndex = -1;
        }
        else
        {
            SwapKngihtHierarchy(invisibleKnight, knight);
        }
       
    }
}
