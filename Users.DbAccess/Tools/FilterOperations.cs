using MongoDB.Driver;
using System.Collections.Generic;

namespace Users.DbAccess.Tools
{
    /// <summary>
    /// A class to perform filter operations.
    /// </summary>
    public class FilterOperations
    {
        /// <summary>
        /// Generates an UpdateDefinition of any given type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Parent">The parent object</param>
        /// <param name="comparingObject">The comparing object</param>
        /// <param name="Child">The child object(optional)</param>
        /// <returns>An <c>UpdateDefition</c> of any given type.</returns>
        public UpdateDefinition<T> BuildUpdateFilter<T>(object Parent, object comparingObject, object Child = null)
        {
            UpdateDefinitionBuilder<T> update_filter = Builders<T>.Update;
            List<UpdateDefinition<T>> updates = new List<UpdateDefinition<T>>();

            #region Modifying a child document
            if (Child != null)
            {
                foreach (var property in Child.GetType().GetProperties())
                {
                    var value = property.GetValue(Child);
                    var oldValue = property.GetValue(comparingObject);
                    var CheckDefault = new object();
                    if (value == null)
                    {
                        CheckDefault = ValueChecker.ConvertObjectToString(value);
                    }
                    else { CheckDefault = ValueChecker.GetDefaultValue(value.GetType()); }

                    if (value != CheckDefault)
                    {
                        if (oldValue == null) oldValue = ValueChecker.ConvertObjectToString(oldValue);
                        if (value != null && value.ToString() != oldValue.ToString())
                        {
                            string field_name = property.Name;
                            updates.Add(update_filter.Set(Child.GetType().Name + "." + field_name, value));
                        }
                    }
                }
            }
            #endregion

            #region Modifying the parent document.
            else
            {
                foreach (var property in Parent.GetType().GetProperties())
                {
                    var value = property.GetValue(Parent);
                    var oldValue = property.GetValue(comparingObject);

                    if (value != null)
                    {
                        var CheckDefault = ValueChecker.GetDefaultValue(value.GetType());
                        if (!value.Equals(CheckDefault))
                        {
                            if (value != oldValue)
                            {
                                string field_name = property.Name;
                                updates.Add(update_filter.Set(field_name, value));
                            }
                        }
                    }
                }
            }
            #endregion
            return update_filter.Combine(updates);
        }
    }
}
