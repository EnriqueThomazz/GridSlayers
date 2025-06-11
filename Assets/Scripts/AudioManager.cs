using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioSource sfxSource;

    public AudioClip background;
    public AudioClip menuBckg;
    public AudioClip enemyDmg;
    public AudioClip playerDmg;
    public AudioClip gameOver;
    public AudioClip faseComplete;

    public string currentScene;

    private void Start()
    {
        playMusic();
    }

    public void playMusic()
    {
        Scene cenaAtual = SceneManager.GetActiveScene();

        if (cenaAtual.name != currentScene) // Detectando mudanças de cena
        {
            Debug.Log("Mundado para cena " + cenaAtual.name);
            currentScene = cenaAtual.name;

            if (cenaAtual.name == "Menu")
            {
                musicSource.clip = menuBckg;
            }
            else
            {
                musicSource.clip = background;
            }

            musicSource.Play();
        }
    }

    public void playSFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    private void Update()
    {
        playMusic(); // Sempre verifica se houve mudança de cena
    }
}
