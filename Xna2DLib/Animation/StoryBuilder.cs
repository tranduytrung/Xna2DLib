using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using tranduytrung.Xna.Core;
using tranduytrung.Xna.Helper;

namespace tranduytrung.Xna.Animation
{
    /// <summary>
    /// Provide a fluent api class to build the storyboard
    /// </summary>
    public sealed class StoryBuilder
    {
        private List<object> _objectList;
        private readonly List<IAnimation> _animationList;
        private TimeSpan _totalTime;        

        private StoryBuilder()
        {
            _animationList = new List<IAnimation>();
        }

        /// <summary>
        /// Create a builder start with some objects
        /// </summary>
        /// <param name="objects">the object will be animated</param>
        /// <returns>the builder instance</returns>
        public static StoryBuilder Select(params object[] objects)
        {
            if (objects == null)
                throw new ArgumentNullException("objects", "objects parameter cannot be null");

            var instance = new StoryBuilder {_objectList = new List<object>(objects)};
            return instance;
        }

        /// <summary>
        /// Wait for a period time
        /// </summary>
        /// <param name="duration">amount of time to wait for</param>
        /// <returns>the builder instance</returns>
        public StoryBuilder Wait(TimeSpan duration)
        {
            _totalTime += duration;
            return this;
        }

        /// <summary>
        /// Animate the selected objects with specified properties, from values, and to values, 
        /// respectively, in a duration
        /// </summary>
        /// <param name="properties">properties's path name seperated by comma</param>
        /// <param name="fromValues">coresponding from values</param>
        /// <param name="toValues">coresponding to values</param>
        /// <param name="duration">duration of animation</param>
        /// <returns>the builder instance</returns>
        public StoryBuilder Animate(string properties, object[] fromValues, object[] toValues, TimeSpan duration)
        {
            var propertiesName = properties.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
            if (propertiesName.Length != fromValues.Length || propertiesName.Length != toValues.Length)
            {
                throw new ArgumentException("number of properties and number of values do not match.");
            }

            foreach (var obj in _objectList)
            {
                for (var i = 0; i < propertiesName.Length; ++i)
                {
                    var type = obj.GetPropertyType(propertiesName[i]);

                    if (type == typeof (int))
                    {
                        var animation = new IntegerlAnimation(obj, propertiesName[i])
                        {
                            BeginTime = _totalTime,
                            To = Convert.ToInt32(toValues[i]),
                            Duration = duration
                        };

                        if (fromValues[i] == null)
                            animation.From = null;
                        else
                            animation.From = Convert.ToInt32(fromValues[i]);

                        _animationList.Add(animation);
                        continue;
                    }

                    if (type == typeof (float))
                    {
                        var animation = new FloatAnimation(obj, propertiesName[i])
                        {
                            BeginTime = _totalTime,
                            To = Convert.ToSingle(toValues[i]),
                            Duration = duration
                        };

                        if (fromValues[i] == null)
                            animation.From = null;
                        else
                            animation.From = Convert.ToSingle(fromValues[i]);

                        _animationList.Add(animation);
                        continue;
                    }

                    if (type == typeof(double))
                    {
                        var animation = new DoubleAnimation(obj, propertiesName[i])
                        {
                            BeginTime = _totalTime,
                            To = Convert.ToDouble(toValues[i]),
                            Duration = duration
                        };

                        if (fromValues[i] == null)
                            animation.From = null;
                        else
                            animation.From = Convert.ToDouble(fromValues[i]);

                        _animationList.Add(animation);
                        continue;
                    }

                    if (type == typeof(Color))
                    {
                        var animation = new ColorAnimation(obj, propertiesName[i])
                        {
                            BeginTime = _totalTime,
                            To = (Color)toValues[i],
                            Duration = duration
                        };

                        if (fromValues[i] == null)
                            animation.From = null;
                        else
                            animation.From = (Color)fromValues[i];

                        _animationList.Add(animation);
                        continue;
                    }

                    throw new NotSupportedException(string.Format("Do not support {0} type", type.FullName));
                }
            }

            _totalTime += duration;
            return this;
        }

        /// <summary>
        /// Animate the selected objects with specified propertyPath, from value, and to value, 
        /// respectively, in a duration
        /// </summary>
        /// <param name="propertyPath">properties's path name seperated by comma</param>
        /// <param name="fromValue">coresponding from values</param>
        /// <param name="toValue">coresponding to values</param>
        /// <param name="duration">duration of animation</param>
        /// <returns>the builder instance</returns>
        public StoryBuilder Animate(string propertyPath, object fromValue, object toValue, TimeSpan duration)
        {
            return Animate(propertyPath, new [] {fromValue}, new[] { toValue }, duration);
        }

        /// <summary>
        /// Animate the selected objects with specified properties, and to values, 
        /// respectively, in a duration.
        /// </summary>
        /// <param name="properties">properties's path name seperated by comma</param>
        /// <param name="toValues">coresponding to values</param>
        /// <param name="duration">duration of animation</param>
        /// <returns>the builder instance</returns>
        public StoryBuilder Animate(string properties, object[] toValues, TimeSpan duration)
        {
            return Animate(properties, new object[toValues.Length], toValues, duration);
        }


        /// <summary>
        /// Animate the selected object with single property path
        /// </summary>
        /// <param name="propertyPath">the property path</param>
        /// <param name="toValue">to value</param>
        /// <param name="duration">duration of animation</param>
        /// <returns>the builder instance</returns>
        public StoryBuilder Animate(string propertyPath, object toValue, TimeSpan duration)
        {
            return Animate(propertyPath, new object[1], new[] {toValue}, duration);
        }

        /// <summary>
        /// Join to builder together including selected objects and animations created before
        /// </summary>
        /// <param name="otherBuilder">the builder which will be joined</param>
        /// <returns>the builder instance</returns>
        public StoryBuilder Join(StoryBuilder otherBuilder)
        {
            _totalTime = _totalTime < otherBuilder._totalTime ? otherBuilder._totalTime : _totalTime;
            _objectList.AddRange(otherBuilder._objectList);
            _animationList.AddRange(otherBuilder._animationList);

            return this;
        }

        /// <summary>
        /// Select more objects into this builder
        /// </summary>
        /// <param name="objects">the object which will join to next animation</param>
        /// <returns>the builder instance</returns>
        public StoryBuilder Add(params object[] objects)
        {
            _objectList.AddRange(objects);
            return this;
        }

        /// <summary>
        /// Remove specified objects from this builder
        /// </summary>
        /// <param name="objects">the objects which will be detach out</param>
        /// <returns>the builder instance</returns>
        public StoryBuilder Remove(params object[] objects)
        {
            foreach (var obj in objects)
            {
                _objectList.Remove(obj);
            }

            return this;
        }

        /// <summary>
        /// Returns the storyboard which is ready to animate
        /// </summary>
        /// <returns>the storyboard</returns>
        public Storyboard ToStoryboard()
        {
            var story = new Storyboard();
            foreach (var animation in _animationList)
            {
                story.Animations.Add(animation);
            }

            return story;
        }
    }
}
