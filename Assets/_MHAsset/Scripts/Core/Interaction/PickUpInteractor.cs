using UnityEngine;
using MH.Interaction;

public class PickUpInteractor : MonoBehaviour, IInteractor
{
    [SerializeField] private float pickupDistance = 2f;    // Distance in front where the object is held
    [SerializeField] private float maxPickupRange = 5f;    // Maximum distance for raycast
    [SerializeField] private float holdForce = 1000f;      // Force to keep the object in place
    [SerializeField] private LayerMask pickupLayer;        // Layers that can be picked up

    private IInteractable currentInteractable;             // Currently held object
    private IInteractable detectedInteractable;            // Interactable detected by raycast
    private Rigidbody heldRigidbody;                       // Rigidbody of the held object
    private bool isHolding = false;                        // Flag for holding state
    private Camera mainCamera;                             // Reference to the main camera

    // Property from IInteractor interface
    public IInteractable InteractableTarget => detectedInteractable;

    private void Awake()
    {
        mainCamera = Camera.main; // Cache the main camera
    }

    private void Update()
    {
        // Raycast to detect interactables
        DetectInteractable();

        // Maintain holding position if holding an object
        if (isHolding && heldRigidbody != null)
        {
            MaintainHoldPosition();
        }

        // Perform interaction when E is pressed
        if (Input.GetKeyDown(KeyCode.E))
        {
            PerformInteraction(detectedInteractable);
        }
    }

    private void DetectInteractable()
    {
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, maxPickupRange, pickupLayer))
        {
            // Check if the hit object has an IInteractable component
            if (hit.collider.TryGetComponent(out IInteractable interactable))
            {
                detectedInteractable = interactable;
                detectedInteractable.OnTrackingEnter();
                return;
            }
        }
        // If no interactable is hit, set to null
        if(detectedInteractable != null) detectedInteractable.OnTrackingExit();
        detectedInteractable = null;
        
    }

    // Implementation of PerformInteraction from IInteractor
    public void PerformInteraction(IInteractable interactable)
    {
        if (interactable == null || !interactable.CanInteract(this)) return;

        // If holding something, drop it
        if (isHolding)
        {
            DropItem();
        }
        // If not holding, pick up the detected item
        else
        {
            GameObject targetObject = (interactable as MonoBehaviour)?.gameObject;
            if (targetObject != null)
            {
                PickUpItem(interactable, targetObject);
            }
        }
    }

    private void PickUpItem(IInteractable interactable, GameObject target)
    {
        currentInteractable = interactable;
        
        // Perform the interaction
        currentInteractable.Interact(this);

        // Get and configure the Rigidbody
        if (target.TryGetComponent(out heldRigidbody))
        {
            heldRigidbody.useGravity = false; // Disable gravity while held
            heldRigidbody.drag = 10f;         // Add drag for smoother movement
            heldRigidbody.angularDrag = 10f;
            isHolding = true;
        }
    }

    private void MaintainHoldPosition()
    {
        // Calculate the target position in front of the interactor
        Vector3 holdPosition = mainCamera.transform.position + mainCamera.transform.forward * pickupDistance;
        
        // Calculate the force needed to move the object to the hold position
        Vector3 directionToHold = holdPosition - heldRigidbody.position;
        Vector3 force = directionToHold * holdForce * Time.deltaTime;

        // Apply force to move the object
        heldRigidbody.AddForce(force, ForceMode.Acceleration);

        // Dampen rotation to keep it stable
        heldRigidbody.angularVelocity *= 0.9f;
    }

    private void DropItem()
    {
        if (currentInteractable == null || heldRigidbody == null) return;

        // Notify the interactable that interaction has ended
        currentInteractable.Interact(this);

        // Restore physics properties
        heldRigidbody.useGravity = true;
        heldRigidbody.drag = 0f;
        heldRigidbody.angularDrag = 0.05f;

        // Clear references
        currentInteractable = null;
        heldRigidbody = null;
        isHolding = false;
    }

    // Clean up if the interactor is destroyed
    private void OnDestroy()
    {
        if (isHolding)
        {
            DropItem();
        }
    }
}