using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace Users.DbAccess.Tools
{
    /// <summary>
    /// A class to perform filter operations.
    /// </summary>
    public class FilterBuilder
    {
        /// <summary>
        /// Generates a <see cref="MongoDB.Driver.UpdateDefinition{TDocument}"/> of any given type.
        /// </summary>
        /// <example>
        /// If you want to update a parent Object/object :
        /// <code>
        /// var parentObject = new ParentObject() { Id = 123, First_Name = "Brian };
        /// var comparingObjectObject = objectReturnedFromDatabase
        /// var childObject = new ChildObject() { Id = 456, Field_1 = "Test" }
        /// FilterBuilder filterOptions = new FilterBuilder();
        /// var UpdateDefinition<ParentClass>FilterBuilder.ToUpdate<ParentClass>(parentObject, comparingObjectObject);
        /// </code>
        /// If you want to update a child object/Object inside a parent object/Object, call the function as follows :
        /// <code>
        /// var UpdateDefinition<ChildClass>FilterBuilder.ToUpdate<ChildClass>(parentObject, comparingObjectObject, childObject);
        /// </code>
        /// If you want to update an array. call the function as follows :
        /// <code>
        /// /// To update an array of a parent 
        /// var UpdateDefinition<ChildClass>FilterBuilder.ToUpdate<ChildClass>(parentObject, Child: childObject, ArrayProperty: parentObject.ArrayPropertyName);
        /// </code>
        /// </example>
        /// <typeparam name="T"></typeparam>
        /// <param name="Parent">The parent object</param>
        /// <param name="comparingObject">The comparing object(optional)</param>
        /// <param name="Child">The child object(optional)</param>
        /// <param name="ArrayProperty">The array property of the parent/child to update.</param>
        /// <returns>An <c>UpdateDefition</c> of any given type.</returns>
        public UpdateDefinition<T> ToUpdate<T>(object Parent, object comparingObject = null, object Child = null, object ArrayProperty = null)
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

        public UpdateDefinition<T> ToUpdate<T>(object Parent, object comparingChildObject, object Child)
        { return null; }

        public UpdateDefinition<T> ToUpdate<T>(object Child, object ArrayProperty)
        { return null;  }

        public FilterDefinition<T> ToFind<T>(string KeyPropertyName, object Value)
        {
            if (KeyPropertyName.ToLower().Contains("id"))
            {
                Value = new ObjectId(Value.ToString());
            }
            
            FilterDefinition<T> query_filter = Builders<T>.Filter.Eq(x => x.GetType().GetProperty(KeyPropertyName).GetValue(x), Value);

            return query_filter;
        }
    }
}
