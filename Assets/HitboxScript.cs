using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxScript : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(AutoDestruir());
    }

    private IEnumerator AutoDestruir()
    {
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D infoColisao)
    {
        if (infoColisao.gameObject.tag == "Trike" || infoColisao.gameObject.tag == "Rex")
        {
            Destroy(gameObject);
        }

    }
}
