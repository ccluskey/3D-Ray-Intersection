using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContiguousInteractable : MonoBehaviour
{
    public Transform rightHandLocation;
    public Transform rightPalmLocation;
    public Transform leftHandLocation;
    public Transform leftPalmLocation;

    public Collider rightHandCollider;
    public Collider leftHandCollider;

    // How fast the object will try to move to the calculated position
    public float transitionSpeed;

    // Maximum distance the object can be from your hands and still be controlled
    public float maxDistance;

    private bool idle;
    private float homeYPosition;

    // Start is called before the first frame update
    void Start()
    {
        // Start with the idle animation until the object is picked up
        idle = true;
        homeYPosition = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (idle)
            IdleAnimation();

        Debug.Log($"Idle: {idle}");

        // If the hands are too far from me, don't allow movement
        if (Vector3.Distance(transform.position, rightHandLocation.position) > maxDistance ||
            Vector3.Distance(transform.position, leftHandLocation.position) > maxDistance)
            return;

        Vector3 rightHandDirection = rightPalmLocation.position - rightHandLocation.position;
        Vector3 leftHandDirection = leftPalmLocation.position - leftHandLocation.position;
        
        // If the hand vectors are pointing at each other, just use the midpoint of the controllers
        RaycastHit hit;
        if (leftHandCollider.Raycast(new Ray(rightHandLocation.position, rightHandDirection), out hit, 10)
            && rightHandCollider.Raycast(new Ray(leftHandLocation.position, leftHandDirection), out hit, 10))
        {
            idle = false;
            transform.position = Vector3.Lerp(transform.position, Vector3.Lerp(rightHandLocation.position, leftHandLocation.position, 0.5f), Time.deltaTime * transitionSpeed);
            // This is considered a perfect match so set opacity to 1
            //SetAlpha(1);
        }
        else // else use ray intersection to find where the object should be positioned
        {
            RayIntersectionResult result = RayIntersection.FindRayIntersection(
                rightHandLocation.position,
                rightHandDirection,
                leftHandLocation.position,
                leftHandDirection
            );

            if ((result.getIntersects() || result.getPositiveSkewPoint()) && result.getIntersectionPoint().HasValue) 
            {
                idle = false;
                // Set transform
                Transform transform = GetComponent<Transform>();
                transform.position = Vector3.Lerp(transform.position,result.getIntersectionPoint().Value, Time.deltaTime * transitionSpeed);

                // Set opacity based on how close the skew lines are
                //SetAlpha(Mathf.Max(1 - result.getDistance()*10, 0));
            }
        }
        // Calculate the scale
        // I think keeping the object the same scale makes sense for now, but keeping this code just in case
        /*
        float distanceToRightHand = Vector3.Distance(transform.position, rightHandLocation.position);
        float distanceToLefttHand = Vector3.Distance(transform.position, leftHandLocation.position);
        float distanceBetweenHands = Vector3.Distance(transform.position, leftHandLocation.position);
        float scale = Mathf.Min(distanceToLefttHand, distanceToRightHand);

        transform.localScale = new Vector3(scale, scale, scale);
        */
    }

    private void IdleAnimation()
    {
        transform.position = new Vector3(
            transform.position.x,
            homeYPosition + Mathf.Sin(Time.time*2.5f) * .06f,
            transform.position.z
        );
    }
    private void SetAlpha(float value)
    {
        Material material = GetComponent<MeshRenderer>().material;
        Color color = GetComponent<MeshRenderer>().material.color;
        color.a = value;
        material.color = color;
    }
}
