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
        /// Generates a <see cref="MongoDB.Driver.UpdateDefinition{TDocument}"/> of any given type.
        /// </summary>
        /// <example>
        /// If you want to update a parent model/object :
        /// <code>
        /// var parentModel = new ParentModel() { Id = 123, First_Name = "Brian };
        /// var comparingObjectModel = objectReturnedFromDatabase
        /// var childModel = new ChildModel() { Id = 456, Field_1 = "Test" }
        /// FilterOperations filterOptions = new FilterOperations();
        /// var UpdateDefinition<ParentClass>filterOperations.BuildUpdateFilter<ParentClass>(parentModel, comparingObjectModel);
        /// </code>
        /// If you want to update a child object/model inside a parent object/model, call the function as follows :
        /// <code>
        /// var UpdateDefinition<ChildClass>filterOperations.BuildUpdateFilter<ChildClass>(parentModel, comparingObjectModel, childModel);
        /// </code>
        /// </example>
        /// <typeparam name="T"></typeparam>
        /// <param name="Parent">The parent object</param>
        /// <param name="comparingObject">The comparing object(optional)</param>
        /// <param name="Child">The child object(optional)</param>
        /// <param name="IsUpdatingArray">Bool to determine if child should be pushed in an array.</param>
        /// <returns>An <c>UpdateDefition</c> of any given type.</returns>
        public static UpdateDefinition<T> BuildUpdateFilter<T>(object Parent, object comparingObject = null, object Child = null, object ArrayProperty= null)
        {
            UpdateDefinitionBuilder<T> update_filter = Builders<T>.Update;
            List<UpdateDefinition<T>> updates = new List<UpdateDefinition<T>>();

            #region Not updaing an array
            if (ArrayProperty == null)
            {
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
                            if (value != null && value.ToString() != oldValue.ToString() && value.ToString() != CheckDefault.ToString())
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
                        var CheckDefault = new object();

                        if (value == null)
                        {
                            CheckDefault = ValueChecker.ConvertObjectToString(value);
                        }
                        else { CheckDefault = ValueChecker.GetDefaultValue(value.GetType()); }

                        if (value != CheckDefault)
                        {
                            if (oldValue == null) oldValue = ValueChecker.ConvertObjectToString(oldValue);
                            if (value != null && value.ToString() != oldValue.ToString() && value.ToString() != CheckDefault.ToString())
                            {
                                string field_name = property.Name;
                                updates.Add(update_filter.Set(field_name, value));
                            }
                        }

                    }
                }
                #endregion
            }
            #endregion

            #region Updating an Array
            else
            {
                updates.Add(update_filter.Push(ArrayProperty.GetType().Name + ".", Child));
            }
            #endregion
            return update_filter.Combine(updates);
        }
    }
}
