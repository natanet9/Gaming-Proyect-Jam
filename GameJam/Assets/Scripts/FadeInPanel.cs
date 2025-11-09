using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeInPanel : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private Image panelNegro;      
    [SerializeField] private float duracionFade = 2f; 
    [SerializeField] private bool desactivarAlFinal = true; 

    void Start()
    {

        if (panelNegro)
        {
            panelNegro.color = new Color(0, 0, 0, 1); // Negro opaco
            StartCoroutine(FadeIn());
        }
    }

    IEnumerator FadeIn()
    {
        float tiempo = 0f;
        Color colorInicial = panelNegro.color;

        while (tiempo < duracionFade)
        {
            tiempo += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, tiempo / duracionFade);
            panelNegro.color = new Color(0, 0, 0, alpha);
            yield return null;
        }


        panelNegro.color = new Color(0, 0, 0, 0);

    
        if (desactivarAlFinal)
        {
            panelNegro.gameObject.SetActive(false);
        }
    }

   
    public void IniciarFade()
    {
        if (panelNegro) panelNegro.gameObject.SetActive(true);
        panelNegro.color = new Color(0, 0, 0, 1);
        StartCoroutine(FadeIn());
    }
}