using UnityEngine;

namespace MH.Interaction
{
    public interface IInteractor
    {
        void PerformInteraction();
        
        Transform Transform { get; }
        bool IsActive { get; set; }
    }
}