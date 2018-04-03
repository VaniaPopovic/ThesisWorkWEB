using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Serialization;

namespace NQL_Thesis
{
    public class Manager
    {
        public void Manage()
        {
            var serializer = new XmlSerializer(typeof(XMLObject.language));
            using (var loadStream = new FileStream(HttpRuntime.AppDomainAppPath + "\\file.xml", FileMode.Open,
                FileAccess.Read))
            {
                    var loadedObject = (XMLObject.language)serializer.Deserialize(loadStream);
            // Console.WriteLine(loadedObject);

            // Console.WriteLine(loadedObject.dimensions.product[0].data_binding[0]);

            using (var conn = new SqlConnection())
            {

                conn.ConnectionString = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;
                conn.Open();
                //Calculating Facts
                SqlCommand command;
                //    foreach (var fact in loadedObject.calculated_facts)
                //       {
                //Console.WriteLine(loadedObject.calculated_facts[0].calculation.ToString());
                //    string a=   loadedObject.calculated_facts[0].calculation.ToString();
                string com = "DECLARE @TD_DATE datetime; SET @TD_DATE = CONVERT(date, getdate()); SELECT ";
                StringBuilder a = new StringBuilder(com);
                foreach (var fact in loadedObject.calculated_facts)
                {
                    string app = fact.calculation;
                    string name = fact.name;
                    com += app + " AS " + name + ", ";
                }

                com = com.Substring(0, com.Length - 2);
                Console.WriteLine(com);

                command = new SqlCommand(com, conn);
                var dataTable = new DataTable();
                var da = new SqlDataAdapter(command);
                // this will query your database and return the result to your datatable

                //// this will query your database and return the result to your datatable
                da.Fill(dataTable);
                //// loadedObject.calculated_facts[0].data_binding = command.ExecuteScalar().ToString();

                ////     }
                foreach (DataRow dtRow in dataTable.Rows)
                {
                    foreach (DataColumn column in dataTable.Columns)
                    {
                        //  Console.WriteLine(column.ColumnName);
                        int index = 0;
                        foreach (var fact in loadedObject.calculated_facts)
                        {
                            if (column.ColumnName.Equals(fact.name.Trim()))
                            {
                                Console.WriteLine(column.ColumnName);
                                fact.data_binding = dtRow[index].ToString();


                            }

                            index++;
                        }
                        //   Console.WriteLine(loadedObject.dimensions.product.);

                    }
                }

                //PRODUCTS
                dataTable = new DataTable();
                command = new SqlCommand("SELECT PRODUCT_NAME, CATEGORY_NAME, BRAND_NAME, SIZE, SIZE_TYPE FROM dbo.PRODUCTS", conn);

                da = new SqlDataAdapter(command);
                // this will query your database and return the result to your datatable
                da.Fill(dataTable);


                foreach (DataRow dtRow in dataTable.Rows)
                {
                    foreach (DataColumn column in dataTable.Columns)
                    {
                        //  Console.WriteLine(column.ColumnName);
                        foreach (var vari in loadedObject.dimensions.product)
                        {
                            //   Console.WriteLine(loadedObject.dimensions.product.);
                            if (column.ColumnName == vari.name)
                            {
                                //   Console.WriteLine(vari.name);
                                if (!vari.data_binding.Contains(dtRow[column].ToString().Trim()))
                                    vari.data_binding.Add(dtRow[column].ToString().Trim());
                            }
                        }
                    }
                }
                //OUTLETS
                dataTable.Reset();
                command = new SqlCommand("SELECT * FROM dbo.OUTLETS", conn);

                da = new SqlDataAdapter(command);
                // this will query your database and return the result to your datatable
                da.Fill(dataTable);


                foreach (DataRow dtRow in dataTable.Rows)
                    foreach (DataColumn column in dataTable.Columns)
                        foreach (var vari in loadedObject.dimensions.outlet)
                            if (column.ColumnName == vari.name)
                            {
                                //   Console.WriteLine(vari.name);
                                if (!vari.data_binding.Contains(dtRow[column].ToString().Trim()))
                                    vari.data_binding.Add(dtRow[column].ToString().Trim());
                            }
                //CUSTOMERS
                dataTable.Reset();
//                command = new SqlCommand("SELECT * FROM dbo.CUSTOMER", conn);
//
//                da = new SqlDataAdapter(command);
//                // this will query your database and return the result to your datatable
//                da.Fill(dataTable);
//
//
//                foreach (DataRow dtRow in dataTable.Rows)
//                    foreach (DataColumn column in dataTable.Columns)
//                        foreach (var vari in loadedObject.dimensions.customer)
//                            if (column.ColumnName == vari.name)
//                            {
//                                //   Console.WriteLine(vari.name);
//                                if (!vari.data_binding.Contains(dtRow[column].ToString().Trim()))
//                                    vari.data_binding.Add(dtRow[column].ToString().Trim());
//                            }
//
//
//
//                dataTable.Reset();
                command = new SqlCommand("SELECT * FROM dbo.CITIES", conn);

                da = new SqlDataAdapter(command);
                // this will query your database and return the result to your datatable
                da.Fill(dataTable);


                foreach (DataRow dtRow in dataTable.Rows)
                    foreach (DataColumn column in dataTable.Columns)
                        foreach (var vari in loadedObject.dimensions.city)
                            if (column.ColumnName == vari.name)
                            {
                                //   Console.WriteLine(vari.name);
                                if (!vari.data_binding.Contains(dtRow[column].ToString().Trim()))
                                    vari.data_binding.Add(dtRow[column].ToString().Trim());
                            }

                conn.Close();
                da.Dispose();
            }


            using (var writer = new StreamWriter(HttpRuntime.AppDomainAppPath + "\\file2.xml"))
            {
                serializer.Serialize(writer, loadedObject);
                writer.Flush();
                writer.Close();
            }
            Console.WriteLine("CREATING XML FILE FROM DATABASE -DONE!");

            loadStream.Close();
            }
            
        }
    }
}