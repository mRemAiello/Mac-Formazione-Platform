using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    // Riferimento alla camera per ottenere il movimento
    private Transform cameraTransform;
    private Vector3 previousCameraPosition;
    
    // Fattore di parallax per controllare la velocità di ogni layer
    public Vector2 parallaxMultiplier;

    private float _textureUnitSizeX;
    private float _textureUnitSizeY;

    // Start viene chiamato una volta all'avvio del gioco
    void Start()
    {
        // Ottieni la posizione della camera e salvala
        cameraTransform = Camera.main.transform;
        previousCameraPosition = cameraTransform.position;

        //
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        _textureUnitSizeX = texture.width / sprite.pixelsPerUnit;
        _textureUnitSizeY = texture.height / sprite.pixelsPerUnit;
    }

    // Update viene chiamato ad ogni frame
    void LateUpdate()
    {
        // Calcola il movimento della camera rispetto al frame precedente
        Vector3 deltaMovement = cameraTransform.position - previousCameraPosition;

        // Sposta il layer corrente in base al fattore di parallax
        transform.position += new Vector3(deltaMovement.x * parallaxMultiplier.x, deltaMovement.y * parallaxMultiplier.y, 0);

        // Aggiorna la posizione della camera per il prossimo frame
        previousCameraPosition = cameraTransform.position;

        // Scrolling infinito su X
        if (Mathf.Abs(cameraTransform.position.x - transform.position.x) >= _textureUnitSizeX)
        {
            float offsetPositionX = (cameraTransform.position.x - transform.position.x) % _textureUnitSizeX;
            transform.position = new Vector3(cameraTransform.position.x + offsetPositionX, transform.position.y);
        }

        // Scrolling infinito su Y
        if (Mathf.Abs(cameraTransform.position.y - transform.position.y) >= _textureUnitSizeX)
        {
            float offsetPositionY = (cameraTransform.position.y - transform.position.y) % _textureUnitSizeX;
            transform.position = new Vector3(transform.position.x, cameraTransform.position.y + offsetPositionY);
        }
    }
}