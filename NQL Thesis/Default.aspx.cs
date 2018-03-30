using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.sun.crypto.provider;
using java.util.function;

namespace NQL_Thesis
{

    public partial class _Default : Page
    {
        private PageState _pageState;
        private int count;

        protected void Page_Load(object sender, EventArgs e)
        {
            
            _pageState = ViewState["pageState"] as PageState ?? new PageState();

            if (Convert.ToString(ViewState["Generated"]) == "true")
                GenerateDynamicControls();

        }
//        protected void btnGDynamicCont_Click(object sender, EventArgs e)
//        {
//            if (Convert.ToString(ViewState["Generated"]) != "true")
//            {
//                GenerateDynamicControls();
//                ViewState["Generated"] = "true";
//            }
//            else
//            {
//                Response.Write("<h2>Controls are already exist in page</h2>");
//            }
//        }

        public void GenerateDynamicControls()
        {
            var modifiedDictionary = new Dictionary<string, List<string>>();
            foreach (var keyValuePair in _pageState.MyDictionary)
            {
                var list = keyValuePair.Value.ToList();
                list = list.Distinct().ToList();
                modifiedDictionary.Add(keyValuePair.Key, list);
            }

            foreach (var keyValuePair in modifiedDictionary)
            {
                foreach (var v in keyValuePair.Value)
                {
                    //DEBUG 
                    System.Diagnostics.Debug.WriteLine(v + " " + keyValuePair.Key);
                }

            }

            foreach (var keyValuePair in modifiedDictionary)
            {
                if (keyValuePair.Value.Count > 1)

                {
                     if (!(keyValuePair.Value.ElementAt(0).Equals("START_PERIOD") && keyValuePair.Value.ElementAt(1).Equals("END_PERIOD")))
                        {
                        List<string> tempList = new List<string>();
                        foreach (var listItem in keyValuePair.Value)
                        {
                            string append = keyValuePair.Key + " as " + listItem;
                            tempList.Add(append);
                        }
                        //if value has two values eg. duplicate was found, create dynamic dropwdowns
                        DropDownList ddl = new DropDownList();
                        ddl.DataSource = tempList;
                        ddl.CssClass = "form-control";
                        ddl.DataBind();
                        ddl.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                        ddl.ID = "ddl" + count;

                        Panel1.Controls.Add(ddl);
                        count++;
                            _pageState.DuplicatesExist = true;
                        }
                     else
                     {
                         _pageState.DisplayList.Add(new Tuple<string, string>(keyValuePair.Value.ElementAt(0), keyValuePair.Key));
                         _pageState.DisplayList.Add(new Tuple<string, string>(keyValuePair.Value.ElementAt(1), keyValuePair.Key));
                    }
                }
                else
                {
                    _pageState.DisplayList.Add(new Tuple<string, string>(keyValuePair.Value.ElementAt(0), keyValuePair.Key));
                }

            }

            if (_pageState.DuplicatesExist) Panel1.Visible = true;
        }

  
   

        protected void QueryButtonSubmit(object sender, EventArgs e)
        {
            //multitxt.Text = "";
            string query = txtquery.Text;

            myParser parser = new myParser();
            //_pagestate is the presistant version of the dictionary returned from parser. Survives through post-backs
            _pageState.MyDictionary = new Dictionary<string, List<string>>();
            //clear if not null
            _pageState.MyDictionary?.Clear();

            _pageState.MyDictionary = parser.Main(query);
            System.Diagnostics.Debug.WriteLine("THIS IS DONE");


            _pageState.DisplayList = new List<Tuple<string, string>>();
            count = 0;
     
            string a = "";
            //GenerateDynamicControls();
            Panel1.Visible = false;
            
            //no presenation type found errors exist
            foreach (var tuple in _pageState.DisplayList)
            {
                if (tuple.Item1.Equals("PRESENTATION_TYPE"))
                {
                    _pageState.Errors = true;
                    break;
                }
            }
            //make presentation type div visible
            if (_pageState.Errors== false) presentation.Visible = true;

            if (Convert.ToString(ViewState["Generated"]) != "true")
            {
                GenerateDynamicControls();
                ViewState["Generated"] = "true";
            }
            else
            {
                Response.Write("<h2>Controls are already exist in page</h2>");
            }
            foreach (var tuple in _pageState.DisplayList)
            {
                a = a + tuple.Item1 + " " + tuple.Item2 + "\n";

            }

            multitxt.Text = "";
            multitxt.Text = a;
        }

       

        protected void updateParameters(object sender, EventArgs e)
        {

      
            if (_pageState.Errors== false)
            {
                _pageState.DisplayList.Add(new Tuple<string, string>("PRESENTATION_TYPE",
                    presentationDropDown.SelectedValue));
            }

            var appList = new List<Tuple<string,string>>();
            if(Panel1.Controls.Count > 0)
            {
                foreach (Control item in Panel1.Controls)
                {
                    if (item is DropDownList)
                    {
                        string temp = ((System.Web.UI.WebControls.ListControl) item).SelectedValue;
                        var splitTemp = temp.Split(new string[] { " as " }, StringSplitOptions.None);
                        appList.Add(new Tuple<string, string>(splitTemp[1],splitTemp[0]));
                    }//string bb= item.FindControl()
                       
                }
            }

            foreach (var item in appList)
            {   
                //add items selected
                _pageState.DisplayList.Add(item);
            }
           
            _pageState.DisplayList = _pageState.DisplayList.Distinct().ToList();
            string a = "";
            foreach (var tuple in _pageState.DisplayList)
            {
                a = a + tuple.Item1 + " " + tuple.Item2 + "\n";

            }
            System.Diagnostics.Debug.WriteLine(a);

            multitxt.Text = "";
            multitxt.Text = a;
            presentation.Visible = false;
            Panel1.Visible = false;


        }
        protected void Page_PreRender(object sender, EventArgs e)
        {
            ViewState["pageState"] = _pageState;
        }
    }
    [Serializable]
    public class PageState
    {
        public Dictionary<string, List<string>> MyDictionary;
        public List<Tuple<string, string>> DisplayList;

        //public int dropDowns;
        public bool DuplicatesExist;
        public bool Errors;
        //    public List<DropDownList> dropDownLists;
        // public List<Label> labels;
    }
}