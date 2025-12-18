using DG.Tweening;
using UnityEngine;

public class MoveTest : MonoBehaviour
{
    float timer = 0f;

    void Start()
    {

    }


    public void move(Vector2 dest)
    {
        this.transform.DOMove(dest, 0.3f);
    }
}
