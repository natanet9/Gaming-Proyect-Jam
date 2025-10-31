using UnityEngine;

public static class ProgresoLimbo
{
    private const string KEY_VISITAS = "Limbo_Visitas";
    private const string KEY_NIVEL_COMPLETADO = "Limbo_Nivel_{0}_Completado";

    public static int Visitas
    {
        get => PlayerPrefs.GetInt(KEY_VISITAS, 0);
        private set => PlayerPrefs.SetInt(KEY_VISITAS, value);
    }

    public static void MarcarNivelCompletado(int nivel)
    {
        string key = string.Format(KEY_NIVEL_COMPLETADO, nivel);
        PlayerPrefs.SetInt(key, 1);
        PlayerPrefs.Save();
    }

    public static bool EstaNivelCompletado(int nivel)
    {
        string key = string.Format(KEY_NIVEL_COMPLETADO, nivel);
        return PlayerPrefs.GetInt(key, 0) == 1;
    }

    public static void IncrementarVisita()
    {
        Visitas++;
        PlayerPrefs.Save();
    }

    public static void Reset()
    {
        PlayerPrefs.DeleteKey(KEY_VISITAS);
        for (int i = 1; i <= 4; i++)
        {
            string key = string.Format(KEY_NIVEL_COMPLETADO, i);
            PlayerPrefs.DeleteKey(key);
        }
        PlayerPrefs.Save();
    }
}