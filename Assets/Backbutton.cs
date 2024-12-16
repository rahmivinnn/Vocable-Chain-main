using UnityEngine;
using UnityEngine.SceneManagement;  // Untuk memanipulasi scene jika ingin kembali ke menu

public class Backbutton : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // Periksa apakah tombol 'back' pada Xbox 360 controller ditekan
        if (Input.GetButtonDown("Xbox_ButtonBack"))
        {
            // Misalnya kembali ke scene utama (menu)
            GoToMainMenu();
        }
    }

    void GoToMainMenu()
    {
        // Ganti 'MainMenu' dengan nama scene yang sesuai
        SceneManager.LoadScene("MainMenu");
    }
}
