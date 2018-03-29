<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="NQL_Thesis._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>Best NQL 5/7</h1>
        <p class="lead">Type your query here:</p>
        
       
       
        <div class="form-group">
            <asp:TextBox x-webkit-speech id="txtquery" ClientIDMode="Static" runat="server" CssClass="form-control" /> 
<%--            <button id="start-record-btn" title="Start Recording">Start Recognition</button>--%>
<%--            <button id="pause-record-btn" title="Pause Recording">Pause Recognition</button>--%>
<%--            <p id="recording-instructions">Press the <strong>Start Recognition</strong> button and allow access.</p>--%>
            <asp:Button runat="server" Text="Record" ID="startRecognition" CssClass="btn btn-info btn-sm" OnClientClick="myFunction(); return false;" />
            <asp:Button runat="server" Text="Stop" ID="stopRecognition" CssClass="btn btn-danger btn-sm" OnClientClick="myFunction2(); return false;" />
           
            <p id="recording-instructions">Press the <strong>Start Recognition</strong> button and allow access.</p>
            
            <span class="input-group-btn">
                <asp:Button CssClass="btn btn-default" runat="server" Text="Go" OnClick="OnClick"/>
            </span>
        </div><!-- /input-group -->
     
        <br /><br />
        
        <asp:TextBox ID="multitxt" runat="server" TextMode="MultiLine" CssClass="form-control" Rows="10"></asp:TextBox>
        
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
            <asp:Panel ID="Panel1" runat="server">
            </asp:Panel>
            <asp:Button runat="server" CssClass="btn btn-primary" Text="Submit" OnClick="updateParameters"/>
        </div>
       
    </div>

<%--    <div class="row">--%>
<%--        <div class="col-md-4">--%>
<%--            <h2>Getting started</h2>--%>
<%--            <p>--%>
<%--                ASP.NET Web Forms lets you build dynamic websites using a familiar drag-and-drop, event-driven model.--%>
<%--            A design surface and hundreds of controls and components let you rapidly build sophisticated, powerful UI-driven sites with data access.--%>
<%--            </p>--%>
<%--            <p>--%>
<%--                <a class="btn btn-default" href="http://go.microsoft.com/fwlink/?LinkId=301948">Learn more &raquo;</a>--%>
<%--            </p>--%>
<%--        </div>--%>
<%--        <div class="col-md-4">--%>
<%--            <h2>Get more libraries</h2>--%>
<%--            <p>--%>
<%--                NuGet is a free Visual Studio extension that makes it easy to add, remove, and update libraries and tools in Visual Studio projects.--%>
<%--            </p>--%>
<%--            <p>--%>
<%--                <a class="btn btn-default" href="http://go.microsoft.com/fwlink/?LinkId=301949">Learn more &raquo;</a>--%>
<%--            </p>--%>
<%--        </div>--%>
<%----%>
<%--        <div class="col-md-4">--%>
<%--            <h2>Web Hosting</h2>--%>
<%--            <p>--%>
<%--                You can easily find a web hosting company that offers the right mix of features and price for your applications.--%>
<%--            </p>--%>
<%--            <p>--%>
<%--                <a class="btn btn-default" href="http://go.microsoft.com/fwlink/?LinkId=301950">Learn more &raquo;</a>--%>
<%--            </p>--%>
<%--        </div>--%>
<%--    </div>--%>
<%--    <div class="container">--%>
<%----%>
<%--        <h1>Voice Controlled Notes App</h1>--%>
<%--        <p class="page-description">A tiny app that allows you to take notes by recording your voice</p>--%>
<%--        <p><a class="tz-link" href="https://tutorialzine.com/2017/08/converting-from-speech-to-text-with-javascript">Read the full article on Tutorialzine »</a></p>--%>
<%----%>
<%--        <h3 class="no-browser-support">Sorry, Your Browser Doesn't Support the Web Speech API. Try Opening This Demo In Google Chrome.</h3>--%>
<%----%>
<%--        <div class="app"> --%>
<%--            <h3>Add New Note</h3>--%>
<%--            <div class="input-single">--%>
<%--                <textarea id="note-textarea" placeholder="Create a new note by typing or using voice recognition." rows="6"></textarea>--%>
<%--            </div>         --%>
<%--            <button id="start-record-btn" title="Start Recording">Start Recognition</button>--%>
<%--            <button id="pause-record-btn" title="Pause Recording">Pause Recognition</button>--%>
<%--            <button id="save-note-btn" title="Save Note">Save Note</button>   --%>
<%--            <p id="recording-instructions">Press the <strong>Start Recognition</strong> button and allow access.</p>--%>
<%--                --%>
<%--            <h3>My Notes</h3>--%>
<%--            <ul id="notes">--%>
<%--                <li>--%>
<%--                    <p class="no-notes">You don't have any notes.</p>--%>
<%--                </li>--%>
<%--            </ul>--%>
<%----%>
<%--        </div>--%>
<%--        <textarea id="textarea" rows=10 cols=80></textarea>--%>
<%--        <button id="button" onclick="toggleStartStop()"></button>--%>
<%----%>
<%--        <script type="text/javascript">--%>
<%--            var recognizing;--%>
<%--            var recognition = new SpeechRecognition();--%>
<%--            recognition.continuous = true;--%>
<%--            reset();--%>
<%--            recognition.onend = reset;--%>
<%----%>
<%--            recognition.onresult = function (event) {--%>
<%--                for (var i = resultIndex; i < event.results.length; ++i) {--%>
<%--                    if (event.results.final) {--%>
<%--                        textarea.value += event.results[i][0].transcript;--%>
<%--                    }--%>
<%--                }--%>
<%--            }--%>
<%----%>
<%--            function reset() {--%>
<%--                recognizing = false;--%>
<%--                button.innerHTML = "Click to Speak";--%>
<%--            }--%>
<%----%>
<%--            function toggleStartStop() {--%>
<%--                if (recognizing) {--%>
<%--                    recognition.stop();--%>
<%--                    reset();--%>
<%--                } else {--%>
<%--                    recognition.start();--%>
<%--                    recognizing = true;--%>
<%--                    button.innerHTML = "Click to Stop";--%>
<%--                }--%>
<%--            }--%>
<%--        </script>--%>
<%----%>
<%----%>
<%--    </div>--%>
<%-- //   <button id="startRecognition">Start Recognition</button>--%>
<%----%>
<%--    <textarea id="txtArea"></textarea>--%>
<%--    <script>--%>
<%--        $(function () {--%>
<%--            try {--%>
<%--                var recognition = new webkitSpeechRecognition();--%>
<%--            } catch (e) {--%>
<%--                var recognition = Object;--%>
<%--            }--%>
<%--            recognition.continuous = true;--%>
<%--            recognition.interimResults = true;--%>
<%--            recognition.onresult = function (event) {--%>
<%--                var txtRec = '';--%>
<%--                for (var i = event.resultIndex; i < event.results.length; ++i) {--%>
<%--                    txtRec += event.results[i][0].transcript;--%>
<%--                }--%>
<%--                $('#txtArea').val(txtRec);--%>
<%--            };--%>
<%--//            $('#startRecognition').click(function () {--%>
<%--//                $('#txtArea').focus();--%>
<%--//                recognition.start();--%>
<%--//            });--%>
<%--//            $('#stopRecognition').click(function () {--%>
<%--//                recognition.stop();--%>
<%--//            });--%>
<%----%>
<%--        });--%>
<%--        function myFunction() {--%>
<%--            recognition.start();--%>
<%--        }--%>
<%--        function myFunction2() {--%>
<%--            recognition.stop();--%>
<%--        }--%>
<%--    </script>--%>
    <div class="container">

        <h1>Voice Controlled Notes App</h1>
        <p class="page-description">A tiny app that allows you to take notes by recording your voice</p>
        <p><a class="tz-link" href="https://tutorialzine.com/2017/08/converting-from-speech-to-text-with-javascript">Read the full article on Tutorialzine »</a></p>

        <h3 class="no-browser-support">Sorry, Your Browser Doesn't Support the Web Speech API. Try Opening This Demo In Google Chrome.</h3>

        <div class="app"> 
            <h3>Add New Note</h3>
            <div class="input-single">
                <textarea id="note-textarea" placeholder="Create a new note by typing or using voice recognition." rows="6"></textarea>
            </div>         
<%--            <button id="start-record-btn" title="Start Recording">Start Recognition</button>--%>
<%--            <button id="pause-record-btn" title="Pause Recording">Pause Recognition</button>--%>
            
                
            <h3>My Notes</h3>
            <ul id="notes">
                <li>
                    <p class="no-notes">You don't have any notes.</p>
                </li>
            </ul>

        </div>

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
