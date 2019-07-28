using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Users.UnitTest
{
    public class DbCRUD
    {
        SqlConnection sql;
        SqlCommand cmd;
        public DbCRUD(string connectionString)
        {
           sql = new SqlConnection(connectionString);

            Command(new string[] { "@FullName", "@Age" }, new string[] { "Aousaf", "20" }, "AddEmployee", AutoExec:false);
            Employee[] ReturnedEmployees = Execute();
            
        }

        public void Command(string[] parameterNames, string[] values, string procedureName, bool AutoExec = true)
        {
            if (parameterNames.Length != values.Length)
            {
                throw new ValueIsNotEqualException("One or more values were not passed for one or more parameters.");
            }
            else
            {
                cmd = new SqlCommand("DeleteEmployee, sql");

                cmd.CommandType = CommandType.StoredProcedure;
                sql.Open();

                for (int i = 0; i <= parameterNames.Length; i++)
                {
                    cmd.Parameters.AddWithValue(parameterNames[i], values[i]);
                }
                
                if (AutoExec)
                {
                    
                }
            }
        }

        public (bool IsSuccessfull, Employee[] EmployeeList) Execute(bool IsReturning = true)
        {
            List<Employee> employeeCollection = new List<Employee>();
            if (IsReturning)
            {
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    employeeCollection.Add(Employee.Create(reader));
                }
            }
            else
            {
                cmd.ExecuteNonQuery();
            }
            return (true, null);
        }
    }

    public class Employee
    {
        public string Employee_Name { get; set; }
        public int Age { get; set; }

        public static Employee Create(IDataReader reader)
        {
            return new Employee()
            {
                Employee_Name = (string)reader[0],
                Age = (int)reader[1]
            };
        }

        public void Get(string FullName)
        {
            DbCRUD db = new DbCRUD("saklsdjlksajdlkjsalkd");
            db.Command(parameterNames: new string[] { "@FullName" }, new string[] { FullName }, "GetEmployees", AutoExec: false);
            Employee[] employees = db.Execute().EmployeeList;
        }

        public void Delete(string FullName)
        {
            DbCRUD db = new DbCRUD("saklsdjlksajdlkjsalkd");
            db.Command(parameterNames: new string[] { "@FullName" }, new string[] { FullName }, "DeleteEmployees");

        }
    }

    public class ValueIsNotEqualException : Exception
    {
        public ValueIsNotEqualException()
        {

        }

        public ValueIsNotEqualException(string name): base(name)
        {

        }
    }
}
