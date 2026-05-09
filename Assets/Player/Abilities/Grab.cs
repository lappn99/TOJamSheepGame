using AbilityScheduler;
using UnityEngine;
using UnityEngine.InputSystem;

public class Grab : AbstractAbility
{

    private bool _hasSheep = false;
    [SerializeField] private LayerMask sheepMask;

    private Sheep _currentSheep;

    public override void UpdateAbility()
    {

        var mousePosition = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        var intersects = Physics2D.GetRayIntersection(ray, Mathf.Infinity, sheepMask.value);
        if (!_hasSheep)
        {
            
            if (intersects.collider)
            {
                if (intersects.collider.gameObject.TryGetComponent(out Sheep sheep))
                {
                    _currentSheep = sheep;
                    _hasSheep = true;
                }
            }
        }

        else
        {
            if (Mouse.current.leftButton.isPressed)
            {
                var position = Camera.main.ScreenToWorldPoint(mousePosition);
                _currentSheep.RigidBody.MovePosition(position);
            }

            else if (!intersects.collider)
            {
                _hasSheep = false;
            }
            
        }
    }
}
