using System;
using System.Collections.Generic;

namespace tranduytrung.Xna.Core
{
    public abstract class GameObject : IAttachableObject
    {
        private readonly Dictionary<Guid, object> _propertyDictionary = new Dictionary<Guid, object>();
        public abstract void Update();
        public object GetValue(AttachableProperty property)
        {
            object value;
            return _propertyDictionary.TryGetValue(property.Id, out value) ? value : property.DefaultValue;
        }

        public void SetValue(AttachableProperty property, object value)
        {
            bool valid = true;
            if (property.Validator != null)
            {
                valid = property.Validator(value);
            }

            if (valid)
            {
                _propertyDictionary[property.Id] = value;
            }
        }
    }
}
