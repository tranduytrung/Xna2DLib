using System;

namespace tranduytrung.Xna.Core
{
    public delegate bool ValidationCallback(object obj);
    public sealed class AttachableProperty
    {
        private Guid _id;
        private Type _propertyType;
        private ValidationCallback _validator;
        private object _defaultValue;

        public Guid Id
        {
            get { return _id; }
        }

        public Type PropertyType
        {
            get { return _propertyType; }
        }

        public ValidationCallback Validator
        {
            get { return _validator; }
        }

        public object DefaultValue
        {
            get { return _defaultValue; }
        }

        public static AttachableProperty RegisterProperty(Type propertyType, object defaultValue = null, ValidationCallback callback = null)
        {
            var property = new AttachableProperty()
            {
                _id = Guid.NewGuid(),
                _propertyType = propertyType,
                _validator = callback,
                _defaultValue = defaultValue ?? (propertyType.IsValueType? Activator.CreateInstance(propertyType) : null)
            };

            return property;
        }

        private AttachableProperty()
        {
            
        }
    }
}
