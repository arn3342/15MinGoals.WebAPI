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
        /// To update a parent object, call the function as follows:
        /// <code>
        /// var parentObject = new ParentObject() { Id = 123, First_Name = "Brian };
        /// var comparingObjectObject = objectReturnedFromDatabase
        /// 
        /// FilterBuilder fl = new FilterBuilder();
        /// var Upadte = fl.ToUpdate<ParentClass>(parentObject, comparingObjectObject);
        /// </code>
        /// <typeparam name="T"></typeparam>
        /// <param name="Parent">The object to update.</param>
        /// <param name="comparingObject">The comparing object. This is basically the document returned from the collection.</param>
        /// <returns>An <see cref="UpdateDefinition{TDocument}"> of any given type.</returns>
        public UpdateDefinition<T> ToUpdate<T>(object Parent, object comparingObject)
        {
            UpdateDefinitionBuilder<T> update_filter = Builders<T>.Update;
            List<UpdateDefinition<T>> updates = new List<UpdateDefinition<T>>();

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
            return update_filter.Combine(updates);
        }

        /// <summary>
        /// An overload of <see cref="ToUpdate{T}(object, object)"/> to generate a <see cref="MongoDB.Driver.UpdateDefinition{TDocument}"/> of a child object.
        /// </summary>
        /// <example>
        /// To update a child object, call the function as follows:
        /// <code>
        /// var childObject = new ChildObject() { Id = 456, Child_First_Name = "Aousaf };
        /// var comparingObjectObject = childObjectReturnedFromDatabase
        /// 
        /// FilterBuilder fl = new FilterBuilder();
        /// var Upadte = fl.ToUpdate<ParentClass>(childObject, childObjectReturnedFromDatabase, true);
        /// </code>
        /// <typeparam name="T"></typeparam>
        /// <param name="Parent">The object to update.</param>
        /// <param name="comparingObject">The comparing object. This is basically the document returned from the collection.</param>
        /// <returns>An <see cref="UpdateDefinition{TDocument}"> of any given type.</returns>
        public UpdateDefinition<T> ToUpdate<T>(object Parent, object comparingObject, bool IsChild)
        {
            UpdateDefinitionBuilder<T> update_filter = Builders<T>.Update;
            List<UpdateDefinition<T>> updates = new List<UpdateDefinition<T>>();
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
                        updates.Add(update_filter.Set(Parent.GetType().Name + "." + field_name, value));
                    }
                }
            }
            return update_filter.Combine(updates);
        }

        /// <summary>
        /// An overload of <see cref="ToUpdate{T}(object, object)"/> to generate a <see cref="MongoDB.Driver.UpdateDefinition{TDocument}"/> to update an array of child objects.
        /// </summary>
        /// <example>
        /// To update an array of child objects, call the function as follows:
        /// <code>
        /// var childObject = new ChildObject() { Id = 456, Child_First_Name = "Aousaf };
        /// var array = ParentObject.ArrayProperty;
        /// 
        /// FilterBuilder fl = new FilterBuilder();
        /// var Upadte = fl.ToUpdate<ParentClass>(childObject, array);
        /// </code>
        /// <typeparam name="T"></typeparam>
        /// <param name="Parent">The object to update.</param>
        /// <param name="ArrayProperty">The array property of the parent.</param>
        /// <returns>An <see cref="UpdateDefinition{TDocument}"> of any given type.</returns>
        public UpdateDefinition<T> ToUpdate<T>(object Parent, Array ArrayProperty)
        {
            UpdateDefinitionBuilder<T> update_filter = Builders<T>.Update;
            List<UpdateDefinition<T>> updates = new List<UpdateDefinition<T>>();
            updates.Add(update_filter.Push(ArrayProperty.GetType().Name, Parent));
            return update_filter.Combine(updates);
        }

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
