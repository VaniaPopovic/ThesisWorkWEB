using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Serialization;
using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using Newtonsoft.Json;

namespace NQL_Thesis
{
    public partial class Results : System.Web.UI.Page
    {

        private DataTable GetData(string query)
        {
            using (var conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;
                conn.Open();

                SqlCommand command = new SqlCommand(query, conn);



                var dataTable = new DataTable();
                var da = new SqlDataAdapter(command);
                // this will query your database and return the result to your datatable

                //// this will query your database and return the result to your datatable
                da.Fill(dataTable);
                return dataTable;
            }

            // Modified your method, since I don't have access to your db, so I created one manually
            // Here we create a DataTable with four columns.
//                DataTable table = new DataTable();
//            table.Columns.Add("Dosage", typeof(int));
//            table.Columns.Add("Drug", typeof(string));
//            table.Columns.Add("Patient", typeof(string));
//            table.Columns.Add("Date", typeof(DateTime));
//
//            // Here we add five DataRows.
//            table.Rows.Add(25, "Indocin", "David", DateTime.Now);
//            table.Rows.Add(50, "Enebrel", "Sam", DateTime.Now);
//            table.Rows.Add(10, "Hydralazine", "Christoff", DateTime.Now);
//            table.Rows.Add(21, "Combivent", "Janet", DateTime.Now);
//            table.Rows.Add(100, "Dilantin", "Melanie", DateTime.Now);
            //  return table;
        }

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
                (List<Tuple<string, string>>) HttpContext.Current.Session["Pairs"];

            if (list == null) Response.Redirect("Default.aspx");

            var serializer = new XmlSerializer(typeof(XMLObject.language));
            var loadStream = new FileStream(HttpRuntime.AppDomainAppPath + "\\file2.xml", FileMode.Open,
                FileAccess.Read);
            var loadedObject = (XMLObject.language) serializer.Deserialize(loadStream);
            var FROM = new List<string>();
            var GROUP_BY = new List<string>();
            var SELECT = new List<string>();
            var WHERE = new List<string>();
            FROM.Add("[NQL].[dbo].[PROCESSED_DATA]");
            string ptype = "";
            string period = "";
            foreach (var item in list)
            {

                if (item.Item1.Equals("START_PERIOD"))
                {
                    period = "PERIOD_ID BETWEEN " + "[NQL].[dbo].[GET_PERIOD_ID]('" + item.Item2 + "')" + " AND ";

                }

                //   Console.WriteLine(loadedObject.dimensions.product.);
                if (item.Item1.Equals("END_PERIOD"))
                {
                    period += "[NQL].[dbo].[GET_PERIOD_ID]('" + item.Item2 + "')";
                }

                if (item.Item1.Equals("PRESENTATION_TYPE"))
                {
                    ptype = item.Item2;
                }

            }

            WHERE.Add(period);
            string putLast = "";
            foreach (var item in list)
            {
                if (!item.Item1.Equals("START_PERIOD") && !item.Item1.Equals("END_PERIOD") &&
                    !item.Item1.Equals("PRESENTATION_TYPE") && !item.Item1.Equals("SELECT CLAUSE"))
                {
                    if (item.Item1.Equals("FUNCTION"))
                    {
                        putLast = item.Item2;
                        SELECT.Add("PERIOD_NAME");
                        // SELECT.Add(item.Item2);
                        GROUP_BY.Add("PERIOD_NAME");
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

            if (putLast.ToLower().Trim().Equals("sales value"))
            {
                SELECT.Add("SUM(SALES_VALUE) AS SALES_VALUE");
            }

            if (putLast.ToLower().Trim().Equals("sales volume"))
            {
                SELECT.Add("SUM(SALES_VOLUME) AS SALES_VOLUME");
            }

            if (putLast.ToLower().Trim().Equals("sales") || putLast.ToLower().Trim().Equals("sales items"))
            {
                SELECT.Add("SUM(SALES_QUANTITY) AS SALES_QUANTITY");
            }

            SELECT = SELECT.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();
            WHERE = WHERE.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();
            GROUP_BY = GROUP_BY.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();

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
            if (WHERE.Count > 0)
            {
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
            }

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

            loadStream.Close();


            multitxt.Text = query;

            DataTable dt = GetData(query);
            gridView.DataSource = dt;
            gridView.DataBind();
            gridView.UseAccessibleHeader = true;
         //   gridView.HeaderRow.TableSection = TableRowSection.TableHeader;
            DataColumnCollection columns = dt.Columns;
            string myStr = "";
            if (columns.Contains("SALES_VALUE"))
            {
                myStr = "SALES_VALUE";
            }

            if (columns.Contains("SALES_VOLUME"))
            {
                myStr = "SALES_VOLUME";
            }

            if (columns.Contains("SALES_QUANTITY"))
            {
                myStr = "SALES_QUANTITY";
            }
            // SerializedDataOfChart(query);

            DataView dv = new DataView(dt);

            DataTable dtT = dv.ToTable(true, "PERIOD_NAME", myStr);
            List<string> ctgries = new List<string>();
            var values = new List<double>();

            foreach (DataRow row in dtT.Rows)
            {
                ctgries.Add(Convert.ToString(row["PERIOD_NAME"]));
                values.Add(Convert.ToDouble(row[myStr]));
            }
           

            string[] ca = ctgries.ToArray();
                object[] cc = values.Select(d => (object) d).ToArray();
               


            if (ptype.ToLower().Trim().Equals("table"))
            {
                gridView.Visible = true;
            }
            else if (ptype.ToLower().Trim().Equals("line chart"))
            {
                ltrChart.Visible = true;
                DotNet.Highcharts.Highcharts chart = new DotNet.Highcharts.Highcharts("chart").SetXAxis(new XAxis
                {
                    Categories = ca
                }).SetSeries(new Series
                {
                    Data = new Data(cc),
                    Name = myStr
                }).SetTitle(new Title { Text = "Line Chart" });
                ltrChart.Text = chart.ToHtmlString();
            }
            else if (ptype.ToLower().Trim().Equals("bar chart"))
            {
                ltrChart.Visible = true;
                DotNet.Highcharts.Highcharts chart = new DotNet.Highcharts.Highcharts("chart")
                    .InitChart(new Chart {DefaultSeriesType = ChartTypes.Bar})
                    .SetTitle(new Title {Text = "Bar Chart"})
                    .SetXAxis(new XAxis
                    {
                        Categories = ca
                    })
                    .SetYAxis(new YAxis
                    {
                        Min = 0,
                        Title = new YAxisTitle
                        {
                            Text = myStr,
                            Align = AxisTitleAligns.High
                        }
                    })
                    .SetPlotOptions(new PlotOptions
                    {
                        Bar = new PlotOptionsBar
                        {
                            DataLabels = new PlotOptionsBarDataLabels {Enabled = true}
                        }
                    })
//                    .SetLegend(new Legend
//                    {
//                        Layout = Layouts.Vertical,
//                        Align = HorizontalAligns.Right,
//                        VerticalAlign = VerticalAligns.Top,
//                        X = -100,
//                        Y = 100,
//                        Floating = true,
//                        BorderWidth = 1,
//                        BackgroundColor = new BackColorOrGradient(ColorTranslator.FromHtml("#FFFFFF")),
//                        Shadow = true
//                    })
                    .SetSeries(new Series
                    {
                        Data = new Data(cc),
                        Name = myStr
                    });
                ltrChart.Text = chart.ToHtmlString();
            }

            //            foreach (DataRow row in dtT.Rows)
            //            {
            //                ctgries.Add(Convert.ToString(row["PERIOD_NAME"]));
            //                values.Add(Convert.ToDouble(row[myStr]));
            //
            //
            //                string[] ca = ctgries.ToArray();
            //                object[] cc = values.Select(d => (object)d).ToArray();
            //                DotNet.Highcharts.Highcharts chart = new DotNet.Highcharts.Highcharts("barchart").SetXAxis(new XAxis
            //                {
            //                    Categories = ca
            //                }).SetSeries(new Series
            //                {
            //                    Name = myStr,
            //                    Data = new Data(cc)
            //                });
            //                barChart.Text = chart.ToHtmlString();
            //            }

        }
    }
}