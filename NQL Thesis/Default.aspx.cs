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
        bool presentationType = false;
        string selectedvalue = "";

        private int count;

        protected void Page_Load(object sender, EventArgs e)
        {
            
            _pageState = ViewState["pageState"] as PageState ?? new PageState();

            if (Convert.ToString(ViewState["Generated"]) == "true")
                GenerateDynamicControls();

        }
        protected void btnGDynamicCont_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(ViewState["Generated"]) != "true")
            {
                GenerateDynamicControls();
                ViewState["Generated"] = "true";
            }
            else
            {
                Response.Write("<h2>Controls are already exist in page</h2>");
            }
        }

        public void GenerateDynamicControls()
        {
            var modifiedDictionary = new Dictionary<string, List<string>>();
            foreach (var VARIABLE in _pageState.myList)
            {
                var list = VARIABLE.Value.ToList();
                list = list.Distinct().ToList();
                modifiedDictionary.Add(VARIABLE.Key, list);
            }

            foreach (var VARIABLE in modifiedDictionary)
            {
                foreach (var v in VARIABLE.Value)
                {
                    System.Diagnostics.Debug.WriteLine(v + " " + VARIABLE.Key);
                }

            }
            foreach (var VARIABLE in modifiedDictionary)
            {
                if (VARIABLE.Value.Count > 1)

                {
                     if (!(VARIABLE.Value.ElementAt(0).Equals("START_PERIOD") && VARIABLE.Value.ElementAt(1).Equals("END_PERIOD")))
                        {
                        List<string> tempList = new List<string>();
                        foreach (var listItem in VARIABLE.Value)
                        {
                            string append = VARIABLE.Key + " as " + listItem;
                            tempList.Add(append);
                        }

                        DropDownList ddl = new DropDownList();
                        ddl.DataSource = tempList;
                            ddl.CssClass = "form-control";
                        ddl.DataBind();
                        ddl.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                        ddl.ID = "ddl" + count;

                        Panel1.Controls.Add(ddl);
                        count++;
                    }
                     else
                     {
                         _pageState.DisplayList.Add(new Tuple<string, string>(VARIABLE.Value.ElementAt(0), VARIABLE.Key));
                         _pageState.DisplayList.Add(new Tuple<string, string>(VARIABLE.Value.ElementAt(1), VARIABLE.Key));
                    }
                }
                else
                {
                    _pageState.DisplayList.Add(new Tuple<string, string>(VARIABLE.Value.ElementAt(0), VARIABLE.Key));
                }

            }
        }

  
   

        protected void OnClick(object sender, EventArgs e)
        {
            multitxt.Text = "";
            string query = txtquery.Text;

            myParser parser = new myParser();
            _pageState.myList = new Dictionary<string, List<string>>();
            _pageState.myList?.Clear();

            _pageState.myList = parser.Main(query);
            System.Diagnostics.Debug.WriteLine("THIS IS DONE");



          
            //            foreach (var VARIABLE in _pageState.myList)
            //            {
            //                a = a + VARIABLE.Value + "  " + VARIABLE.Key + " \n";
            //            }
            //         System.Diagnostics.Debug.WriteLine(a);
            
            _pageState.DisplayList = new List<Tuple<string, string>>();
            count = 0;
      //     _pageState.labels = new List<Label>();
         //   _pageState.dropDownLists = new List<DropDownList>();
          

            _pageState.dropDowns = count;
            string a = "";
            foreach (var VARIABLE in _pageState.DisplayList)
            {
                a = a + VARIABLE.Item1 + " " + VARIABLE.Item2 + "\n";

            }

            multitxt.Text = "";
            multitxt.Text = a;

            foreach (var VARIABLE in _pageState.DisplayList)
            {
                if (VARIABLE.Item1.Equals("PRESENTATION_TYPE"))
                {
                    _pageState.Errors = true;
                    break;
                }
            }

            if (!_pageState.Errors) presentation.Visible = true;

            if (Convert.ToString(ViewState["Generated"]) != "true")
            {
                GenerateDynamicControls();
                ViewState["Generated"] = "true";
            }
            else
            {
                Response.Write("<h2>Controls are already exist in page</h2>");
            }
        }

       

        protected void updateParameters(object sender, EventArgs e)
        {

      
            if (!_pageState.Errors)
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

            foreach (var VARIABLE in appList)
            {
                _pageState.DisplayList.Add(VARIABLE);
            }
            //  DropDownList list =(DropDownList) FindControl("ddl0");
            _pageState.DisplayList = _pageState.DisplayList.Distinct().ToList();
            string a = "";
            foreach (var VARIABLE in _pageState.DisplayList)
            {
                a = a + VARIABLE.Item1 + " " + VARIABLE.Item2 + "\n";

            }
            System.Diagnostics.Debug.WriteLine(a);

            multitxt.Text = "";
            multitxt.Text = a;
            presentation.Visible = false;
   

        }
        protected void Page_PreRender(object sender, EventArgs e)
        {
            ViewState["pageState"] = _pageState;
        }
    }
    [Serializable]
    public class PageState
    {
        public Dictionary<string, List<string>> myList;
        public List<Tuple<string, string>> DisplayList;

        public int dropDowns;

        public bool Errors;
        //    public List<DropDownList> dropDownLists;
        // public List<Label> labels;
    }
}