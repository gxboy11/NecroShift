using UnityEngine;

[System.Serializable] //Contenedor que maneja nuestros sonidos
public class Sound
{
    [SerializeField]
    public string name;

    [SerializeField]
    public AudioClip sound;
}
