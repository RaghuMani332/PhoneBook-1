using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace PhoneBook
{
    public class DAO
    {
        public static SqlConnection connection()
        {
            SqlConnection conn = null;
            String connection = "server=RAGHU\\SQLEXPRESS; Initial Catalog = Student; Integrated Security = SSPI";
            try
            {
                conn = new SqlConnection(connection);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return conn;
        }
       public static bool createTable()
        {
            SqlConnection conn = connection();
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            // cmd.CommandText= "CREATE TABLE AddressBook (firstname varchar(50) unique NOT NULL,lastname varchar(50) unique NOT NULL,contactnumber ,Email VARCHAR(100) NOT NULL UNIQUE,Address VARCHAR(100) NOT NULL,Pincode int )";

            cmd.CommandText = "CREATE TABLE AddressBook (firstname VARCHAR(50) UNIQUE NOT NULL, lastname VARCHAR(50) UNIQUE NOT NULL, contactnumber BIGINT, Email VARCHAR(100) NOT NULL UNIQUE, Address VARCHAR(100) NOT NULL, Pincode INT)";
            bool flag = cmd.ExecuteNonQuery() > 0 ? true : false;
          
            conn.Close();
            return flag;


        }
        public static int insert()
        {
            SqlConnection conn = connection();
            conn.Open();
            String[] s = "@firstname,@lastname,@contactnumber,@Email,@Address,@Pincode".Split(",");
            SqlCommand cmd = new SqlCommand("Insert into AddressBook values(@firstname,@lastname,@contactnumber,@Email,@Address,@Pincode)", conn);
            Object[] objs = getDataFromUser();
            int nora = 0;
            if (!checkRegex(objs))
            {
                for (int i = 0; i < s.Length; i++)
                {
                    cmd.Parameters.AddWithValue(s[i], objs[i]);
                }
                nora = cmd.ExecuteNonQuery();
            }
            else
            {
                Console.WriteLine("regex miss matching");
            }

            conn.Close();
            return nora;

        }
        static Object[] getDataFromUser()
        {
            Object[] obj = new object[6];
            Console.WriteLine("enter first name");
            obj[0] = Console.ReadLine();

            Console.WriteLine("enter last name");
            obj[1] = Console.ReadLine();

            Console.WriteLine("enter contact number");
            obj[2] = long.Parse(Console.ReadLine());

            Console.WriteLine("enter EMAIL");
            obj[3] = Console.ReadLine();

            Console.WriteLine("enter ADDRESS");
            obj[4] = Console.ReadLine();

            Console.WriteLine("enter PIN");
            obj[5] = int.Parse(Console.ReadLine());

            return obj;
        }
        static bool checkRegex(Object[] obj)
        {
            List<bool> list = new List<bool>();
            for (int i = 0; i < obj.Length; i++)
            {
                /*switch (i)
                {
                    case 0:
                        //list.Add(Regex.IsMatch(obj[i] + "", @"^[A-Z]{1}[a-z]$"));
                        list.Add(Regex.IsMatch(obj[i] + "", @"^[A-Z][a-z]$"));
                        break;
                    case 1:
                        //list.Add(Regex.IsMatch(obj[i] + "", @"^[A-Z]{1}[a-z]$"));
                        list.Add(Regex.IsMatch(obj[i] + "", @"^[A-Z][a-z]$"));
                        break;
                    case 2:
                        //list.Add(Regex.IsMatch(obj[i] + "", @"^[0-9]{10}$"));
                        list.Add(Regex.IsMatch(obj[i] + "", @"^[0-9]{10}$"));

                        break;
                    case 3:
                        //  list.Add(Regex.IsMatch(obj[i] + "", @"[a-z]+@+[a-z]{3,4}+.+[a-z]{2,}^"));
                        list.Add(Regex.IsMatch(obj[i] + "", @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"));
                        break;
                    case 4:
                        //list.Add(Regex.IsMatch(obj[i] + "", @"^[A-Z]{1,}[a-z]^$"));
                        list.Add(Regex.IsMatch(obj[i] + "", @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"));

                        break;
                    case 5:
                        //list.Add(Regex.IsMatch(obj[i] + "", @"^[0-9]{6}^$"));
                        list.Add(Regex.IsMatch(obj[i] + "", @"^[0-9]{6}$"));

                        break;
                }*/
                switch (i)
                {
                    case 0:
                        // Pattern: Starts with uppercase letter, followed by lowercase letters (at least one).
                        list.Add(Regex.IsMatch(obj[i] + "", @"^[A-Z][a-z]+$"));
                        break;
                    case 1:
                        // Pattern: Starts with uppercase letter, followed by lowercase letters (at least one).
                        list.Add(Regex.IsMatch(obj[i] + "", @"^[A-Z][a-z]+$"));
                        break;
                    case 2:
                        // Pattern: Exactly 10 digits.
                        list.Add(Regex.IsMatch(obj[i] + "", @"^\d{10}$"));
                        break;
                    case 3:
                        // Pattern: Email format.
                        list.Add(Regex.IsMatch(obj[i] + "", @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"));
                        break;
                    case 4:
                        // Pattern: Any characters allowed for address, no specific pattern.
                        list.Add(true); // No regex validation for address.
                        break;
                    case 5:
                        // Pattern: Exactly 6 digits.
                        list.Add(Regex.IsMatch(obj[i] + "", @"^\d{6}$"));
                        break;
                }

            }
            return list.Contains(false);
        }
        public static void selectAll()
        {
            SqlConnection con = connection();
            con.Open();
            SqlDataReader reader = readData(con);
            while (reader.Read())
            {
                Console.WriteLine("{0} {1} {2} {3} {4} {5}", reader[0], reader[1], reader[2], reader[3], reader[4], reader[5]);
            }
            con.Close();
        }
        static SqlDataReader readData(SqlConnection con)
        {
           
            String query = "Select * from AddressBook";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader reader = cmd.ExecuteReader();
           
            return reader;
        }
        public static bool delete()
        {
            SqlConnection con = connection();
            con.Open();
            selectAll();
            Console.WriteLine("Enter first name to delete:");
            String name = Console.ReadLine();
            String Query = "DELETE FROM AddressBook WHERE firstname = @FirstName";
            SqlCommand cmd = new SqlCommand(Query, con);
            cmd.Parameters.AddWithValue("@FirstName", name);
            int nora = cmd.ExecuteNonQuery();
            con.Close();
            return nora > 0 ? true : false;
        }
        public static void update()
        {
            SqlConnection con = connection();
            con.Open();
            selectAll();
           
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = "update AddressBook set contactnumber = @cno where firstname = @fname";
            Console.WriteLine("enter firstname ");
            cmd.Parameters.Add("@fname", SqlDbType.VarChar).Value = Console.ReadLine();
            Console.WriteLine("enter new number");
            String val = Console.ReadLine();
            if(Regex.IsMatch(val, @"^\d{10}$"))
            {
                cmd.Parameters.Add("@cno", SqlDbType.BigInt).Value = long.Parse(val);
            }   
            else
            {
                throw new Exception("invalid format");
            }
           
            int nora = cmd.ExecuteNonQuery();
            con.Close();
            Console.WriteLine("no.of.rows affected -> "+nora);
        }



    }
}
