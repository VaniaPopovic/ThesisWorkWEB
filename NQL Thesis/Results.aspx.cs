using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Serialization;

namespace NQL_Thesis
{
    public partial class Results : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            var list = //new List<Tuple<string, string>>();
//            list.Add(new Tuple<string, string>("START_PERIOD", "January 2017"));
//            list.Add(new Tuple<string, string>("END_PERIOD", "January 2017"));
//            list.Add(new Tuple<string, string>("BRAND", "Coca Cola"));
//            list.Add(new Tuple<string, string>("PRESENTATION_TYPE", "table"));
//            list.Add(new Tuple<string, string>("CITY_NAME", "NICOSIA"));
//            list.Add(new Tuple<string, string>("SELECT CLAUSE", "show"));
//            list.Add(new Tuple<string, string>("Function", "sales"));
            (List<Tuple<string,string>>) HttpContext.Current.Session["Pairs"];

            if(list == null) Response.Redirect("Default.aspx");

            var serializer = new XmlSerializer(typeof(XMLObject.language));
                        var loadStream = new FileStream(HttpRuntime.AppDomainAppPath + "\\file2.xml", FileMode.Open, FileAccess.Read);
                        var loadedObject = (XMLObject.language) serializer.Deserialize(loadStream);
            var FROM = new List<string>();
            var GROUP_BY = new List<string>();
            var SELECT = new List<string>();
            var WHERE = new List<string>();
            FROM.Add("MY_TABLE??");
            string period = ""; 
            foreach (var item in list)
            {

                if (item.Item1.Equals("START_PERIOD"))
                {
                    period = "PERIOD_ID BETWEEN " + item.Item2 + " AND ";
      
                }
                //   Console.WriteLine(loadedObject.dimensions.product.);
                if (item.Item1.Equals("END_PERIOD"))
                {
                    period += item.Item2;
                }
                
            }
            WHERE.Add(period);
            foreach (var item in list)
            {
                if (!item.Item1.Equals("START_PERIOD") && !item.Item1.Equals("END_PERIOD") &&
                    !item.Item1.Equals("PRESENTATION_TYPE") && !item.Item1.Equals("SELECT CLAUSE"))
                {
                    if (item.Item1.Equals("Function"))
                    {
                        SELECT.Add(item.Item2);
                    }
                    else if (!SELECT.Contains(item.Item1))
                    {
                        SELECT.Add(item.Item1);
                        GROUP_BY.Add(item.Item1);
                        string tmp = "";
                        tmp = item.Item1 + "='" + item.Item2 + "'";
                        WHERE.Add(tmp);
                    }
                       
                    
                }

              
            }

            string query = "SELECT ";
            string select = "";
            var last = SELECT.Last();
            foreach (var VARIABLE in SELECT)
            {
               // query += VARIABLE;
                if (VARIABLE.Equals(last))
                {
                    select += VARIABLE + " ";
                }
                else select += VARIABLE + ", ";
            }

            loadedObject.result.SELECT = select;
            query += select;

            query += "\nFROM ";
            string from = "";
            last = FROM.Last();
            foreach (var VARIABLE in FROM)
            {
             //   query += VARIABLE;
                if (VARIABLE.Equals(last))
                {
                    from += VARIABLE + " ";
                }
                else from += VARIABLE + ", ";
            }

            query += from;
            loadedObject.result.FROM = from;
            query += "\nWHERE ";
            last = WHERE.Last();
            string where = "";
            foreach (var VARIABLE in WHERE)
            {
             //   query += VARIABLE;
                if (VARIABLE.Equals(last))
                {
                    where += VARIABLE + " ";
                }
                else where += VARIABLE + " AND ";
            }

            query += where;
            loadedObject.result.WHERE = where;

            query += "\nGROUP BY ";
            last = GROUP_BY.Last();
            string groupby = "";
            foreach (var VARIABLE in GROUP_BY)
            {
            //    query += VARIABLE;
                if (VARIABLE.Equals(last))
                {
                    groupby += VARIABLE + " ";
                }
                else groupby += VARIABLE + ", ";

            }

            query += groupby;

            loadedObject.result.GROUP_BY = groupby;

            using (var writer = new StreamWriter(HttpRuntime.AppDomainAppPath + "\\result.xml"))
            {
                serializer.Serialize(writer, loadedObject);
                writer.Flush();
            }


            multitxt.Text = query;
        }

    }
}