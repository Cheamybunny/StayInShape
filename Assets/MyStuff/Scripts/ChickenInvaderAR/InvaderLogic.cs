using System.Collections;
using UnityEngine;

public class InvaderLogic : MonoBehaviour
{
    [SerializeField] AudioSource chickenSound;
    public float moveSpeed;
    private bool isChasedAway;
    public static float interval = 0.5f;
    private static float SMALL_CONSTANT = 0.025f;
    private ChickenInvaderManager manager;
    private Transform goal;

    public void Move(Vector3 direction, float scale) // Should be called per frame
    {
        // Ensure that chicken cannot "go" into the ground
        direction = new Vector3(direction.x, 1, direction.z).normalized;
        direction *= moveSpeed * SMALL_CONSTANT * scale;
        transform.position += direction;
    }

    public IEnumerator AttackTarget(Transform target, ChickenInvaderManager man)
    {
        goal = target;
        isChasedAway = false;
        Debug.Log("Invader attacking!");
        manager = man;
        while (!isChasedAway && !manager.isGameEnded)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * SMALL_CONSTANT); // My "Move" method doesn't work smoothly with the LookAt method :(
            transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z)); // Constrain Y-Axis
            chickenSound.Play();
            yield return new WaitForSeconds(interval);
        }
    }

    public void MoveAway()
    {
        //Vector3 heightlessPosition = new Vector3(target.position.x, 0, target.position.z);
        Vector3 direction = transform.position - goal.position;
        Move(direction, 2 * moveSpeed);
    }
}
