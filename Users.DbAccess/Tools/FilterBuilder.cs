using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Users.Models;

namespace Users.DbAccess.Tools
{
    /// <summary>
    /// A class to perform filter operations.
    /// </summary>
    public class FilterBuilder
    {
        /// <summary>
        /// Generates a <see cref="MongoDB.Driver.UpdateDefinition{TDocument}"/> of any given type.
        /// <para>
        /// Used to build a query to update a specific document within a collection.
        /// </para>
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

        /// <summary>
        /// Generates a <see cref="FilterDefinition{TDocument}"/> of any given type. 
        /// <para>
        /// Used to build a query to find a specific document within a collection.
        /// </para>
        /// </summary>
        /// <example>
        /// To create a filter to fetch a document that mathces has a given value in any of it's fields, call the function as follows:
        /// <code>
        /// FilterBuilder fl = new FilterBuilder();
        /// var FindingFilter = fl.ToFInd<TypeOfObject>("User_Id", 1234);
        /// </code>
        /// </example>
        /// <typeparam name="T"></typeparam>
        /// <param name="Value">The value to search for. By default, the function considers the name of the variable passed as <paramref name="Value"/> to be the same as the field's name in the document.</param>
        /// <returns>A <see cref="FilterDefinition{TDocument}"/> of any given type.</returns>
        public FilterDefinition<T> ToFind<T>(string Field_Name, object Value, bool IsObjectId = false)
        {
            if (IsObjectId && Field_Name.ToLower().Contains("id"))
            {
                Value = new ObjectId(Value.ToString());
            }

            if (char.IsLower(Field_Name[0]))
            {
                var upper = char.ToUpper(Field_Name[0]);
                Field_Name = upper + Field_Name.Remove(0, 1);
            }
            FilterDefinitionBuilder<T> query_filter = Builders<T>.Filter;

            FilterDefinition<T> fl = query_filter.Eq(Field_Name, Value);
            return fl;
        }

        /// <summary>
        /// Generates a <see cref="ProjectionDefinition{TSource}"/> of any given type, limits the results as required.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression">The lambda expression to defile the array/collection.</param>
        /// <param name="limit">The number of documents to fetch.</param>
        /// <param name="skip">The number of documents to skip.</param>
        /// <returns></returns>
        public ProjectionDefinition<T> ToFindSome<T>(System.Linq.Expressions.Expression<Func<T, object>> expression, int limit, int skip = 0)
        {
            ProjectionDefinitionBuilder<T> query_filter = Builders<T>.Projection;
            var qr = query_filter.Slice(expression, skip, limit);
            return qr;
        }

        /// <summary>
        /// An overload of <see cref="ToFindSome{T}(System.Linq.Expressions.Expression{Func{T, object}}, int, int)"/>, accepts string as field name to simplify the search.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="FieldName">The name of the field of the array/collection, or the name of the collection.</param>
        /// <param name="limit"></param>
        /// <param name="skip"></param>
        /// <returns></returns>
        public ProjectionDefinition<T> ToFindSome<T>(string FieldName, int limit, int skip = 0)
        {
            ProjectionDefinitionBuilder<T> query_filter = Builders<T>.Projection;
            var qr = query_filter.Slice(FieldName, skip, limit);
            return qr;
        }
    }
}
