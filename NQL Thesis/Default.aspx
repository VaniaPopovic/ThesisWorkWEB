<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="NQL_Thesis._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>NQL Parser</h1>
        <p class="lead">Type your query here:</p>
        
       
       
        <div class="form-group">
            <asp:TextBox id="txtquery" ClientIDMode="Static" runat="server" CssClass="form-control" />             
            <span class="input-group-btn">
                <asp:Button CssClass="btn btn-default" runat="server" Text="Go" OnClick="QueryButtonSubmit"/>
            </span>
        </div><!-- /input-group -->
            <asp:Button runat="server" Text="Record" ID="startRecognition" CssClass="btn btn-info btn-sm" OnClientClick="myFunction(); return false;" />
            <asp:Button runat="server" Text="Stop" ID="stopRecognition" CssClass="btn btn-danger btn-sm" OnClientClick="myFunction2(); return false;" />
            
       
           
            <p id="recording-instructions">Press the <strong>Record</strong> button and allow access.</p>
<%--        <br /><br />--%>
<%--        <div class="btn-group">--%>
<%--            <button type="button" class="btn btn-primary OnClientClick="myFunction2(); return false;">Sample Query 1</button>--%>
<%--            <button type="button" class="btn btn-primary">Sample Query 2</button>--%>
<%--            <button type="button" class="btn btn-primary">Sample Query 3</button>--%>
<%--        </div>--%>

     
        <br /><br />
        
        <asp:TextBox ID="multitxt" runat="server" TextMode="MultiLine" CssClass="form-control" Rows="10"></asp:TextBox>
        <div runat="server" ID="extraControls" Visible="false">
        <div runat="server" Visible="False" ID="presentation" class="alert alert-danger" role="alert">
            No or wrong presentation type selected
            <div class="form-group">
                <label for="exampleFormControlSelect1">Select one</label>
                <asp:DropDownList class="form-control" id="presentationDropDown" runat="server">
                    <asp:ListItem>Table</asp:ListItem>
                    <asp:ListItem>Line Chart</asp:ListItem>
                    <asp:ListItem>Bar Chart</asp:ListItem>
                </asp:DropDownList>
            </div>
           
        </div>
            
            <div runat="server" Visible="False" ID="Div1" class="alert alert-danger" role="alert">
                No function selected
                <div class="form-group">
                    <label for="exampleFormControlSelect1">Select one</label>
                    <asp:DropDownList class="form-control" id="DropDownList1" runat="server">
                        <asp:ListItem>Sales Quantity</asp:ListItem>
                        <asp:ListItem>Sales Volume</asp:ListItem>
                        <asp:ListItem>Sales Value</asp:ListItem>
                    </asp:DropDownList>
                </div>
           
            </div>
<%--    <div runat="server" Visible="True" ID="Div1" class="alert alert-danger" role="alert">--%>
       
        <asp:Panel ID="Panel1" runat="server" CssClass="alert alert-danger">
        </asp:Panel>
            <asp:Button runat="server" CssClass="btn btn-primary" Text="Submit" OnClick="updateParameters"/>
        </div>
      
<%--    </div>--%>
    </div>

    
    <script>
        try {
  var SpeechRecognition = window.SpeechRecognition || window.webkitSpeechRecognition;
  var recognition = new SpeechRecognition();
}
catch(e) {
  console.error(e);
  $('.no-browser-support').show();
  $('.app').hide();
}


        var noteTextarea = $('#txtquery');
        var instructions = $('#recording-instructions');



/*-----------------------------
      Voice Recognition 
------------------------------*/

// If false, the recording will stop after a few seconds of silence.
// When true, the silence period is longer (about 15 seconds),
// allowing us to keep recording even when the user pauses. 
recognition.continuous = true;

// This block is called every time the Speech APi captures a line. 
recognition.onresult = function(event) {

  // event is a SpeechRecognitionEvent object.
  // It holds all the lines we have captured so far. 
  // We only need the current one.
  var current = event.resultIndex;

  // Get a transcript of what was said.
  var transcript = event.results[current][0].transcript;

  // Add the current transcript to the contents of our Note.
  // There is a weird bug on mobile, where everything is repeated twice.
  // There is no official solution so far so we have to handle an edge case.
  var mobileRepeatBug = (current == 1 && transcript == event.results[0][0].transcript);

  if(!mobileRepeatBug) {
    noteContent += transcript;
    noteTextarea.val(noteContent);
  }
};

recognition.onstart = function() { 
  instructions.text('Voice recognition activated. Try speaking into the microphone.');
}

recognition.onspeechend = function() {
  instructions.text('You were quiet for a while so voice recognition turned itself off.');
}

recognition.onerror = function(event) {
  if(event.error == 'no-speech') {
    instructions.text('No speech was detected. Try again.');  
  };
}



/*-----------------------------
      App buttons and input 
------------------------------*/

//$('#start-record-btn').on('click', function(e) {
// 
//  recognition.start();
//});
//
//
//$('#pause-record-btn').on('click', function(e) {
//  recognition.stop();
// 
//});

function myFunction() {
    if (noteContent.length) {
        noteContent += ' ';
    }
            recognition.start();
        };
        function myFunction2() {
            recognition.stop();
            instructions.text('Voice recognition paused.');
        };

// Sync the text inside the text area with the noteContent variable.
        noteTextarea.on('input',
            function() {
                noteContent = $(this).val();
            });





/*-----------------------------
      Helper Functions 
------------------------------*/


</script>
</asp:Content>
