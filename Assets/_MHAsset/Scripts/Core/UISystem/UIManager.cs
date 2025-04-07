using MH.Singleton;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MH.UISystem
{
    public class UIManager : MonoSingleton<UIManager>
    {
        #region ------------- Fields ----------------

        private readonly Dictionary<Type, UILayer> _layerMap = new Dictionary<Type, UILayer>();
        #endregion

        #region ----------- Unity Methods -----------

        protected override void Awake()
        {
            base.Awake();
            CacheUILayers();
        }

        #endregion

        #region ----------------- Public Methods -----------------

        public TLayer GetLayer<TLayer>() where TLayer : UILayer
        {
            if (_layerMap.TryGetValue(typeof(TLayer), out var layer))
            {
                return layer as TLayer;
            }

            Debug.LogError($" [Bug UIManager] Not found layer with type == {typeof(TLayer)}");
            return null;
        }

        #endregion


        /// <summary>
        /// Cache the instances of UILayers in the UIManager.
        /// </summary>
        private void CacheUILayers()
        {
            var layers = GetComponentsInChildren<UILayer>();

            foreach (var layer in layers)
            {
                var layerType = layer.GetType();
                layer.Initialized();

                if (!_layerMap.TryAdd(layerType, layer))
                {
                    Debug.LogError($"Duplicate UI layer of type {layerType} found. Only the first instance will be used.");
                }
            }
        }
    }
}
