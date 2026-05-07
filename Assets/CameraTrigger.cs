using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    [Header("Target")]
    public Transform target;  // Player
    
    [Header("Configuração")]
    public float smoothSpeed = 0.125f;
    public float minY = -15f;    // Limite inferior (mais baixo)
    public float maxY = 5f;      // Limite superior (mais alto)
    public float fixedX = 0f;    // X fixo da câmera
    
    void Start()
    {
        // Procura o player automaticamente se não foi atribuído
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) target = player.transform;
        }
        
        // Define o X fixo como a posição atual da câmera
        fixedX = transform.position.x;
    }
    
    void LateUpdate()
    {
        if (target == null) return;
        
        // Só segue no eixo Y
        float desiredY = target.position.y;
        float clampedY = Mathf.Clamp(desiredY, minY, maxY);
        
        Vector3 desiredPosition = new Vector3(fixedX, clampedY, -10f);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        
        transform.position = smoothedPosition;
    }
    
    // Visualiza os limites no editor
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector3 limiteCima = new Vector3(transform.position.x, maxY, 0);
        Vector3 limiteBaixo = new Vector3(transform.position.x, minY, 0);
        Gizmos.DrawLine(limiteCima - Vector3.right * 5, limiteCima + Vector3.right * 5);
        Gizmos.DrawLine(limiteBaixo - Vector3.right * 5, limiteBaixo + Vector3.right * 5);
    }
}