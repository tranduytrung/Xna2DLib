﻿using System;
using tranduytrung.Xna.Core;

namespace tranduytrung.Xna.Map
{
    public static class IsometricMapExtension
    {
        private static readonly AttachableProperty BindingObjectProperty =
            AttachableProperty.RegisterProperty(typeof (DrawableObject));

        private static void MouseMoved(object obj, IsometricMouseEventArgs e)
        {
            var map = (IsometricMap) obj;
            var drawObj = (DrawableObject) map.GetValue(BindingObjectProperty);
            var deploy = (IIsometricDeployable)drawObj.GetValue(IsometricMap.DeploymentProperty);
            deploy.Deploy(e.Coordinate, e.CellX, e.CellY);
        }

        public static void BindToMouse(this IsometricMap map, DrawableObject obj)
        {
            if (map.GetValue(BindingObjectProperty) == null)
            {
                map.IsometricMouseMoved += MouseMoved;
            }

            if (obj.GetValue(IsometricMap.DeploymentProperty) == null)
            {
                throw new ArgumentException("obj do not has tranduytrung.Xna.Map.IsometricMap ");
            }

            map.EnableInteractiveChildren = false;
            map.SetValue(BindingObjectProperty, obj);
        }

        public static void UnbindToMouse(this IsometricMap map)
        {
            if (map.GetValue(BindingObjectProperty) != null)
            {
                map.IsometricMouseMoved -= MouseMoved;
                map.EnableInteractiveChildren = true;
                map.SetValue(BindingObjectProperty, null);
            }
        }
    }
}
