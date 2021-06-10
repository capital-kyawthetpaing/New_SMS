<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RunFile.aspx.cs" Inherits="MenuHosting.RunnerGlobe.RunFile" EnableEventValidation="false" %> 
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1"   %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta  http-equiv="Content-Type" content="text/html"; charset="utf-8"/>
    <title> Runner</title>
    <style>
        .form-group {
        
        float:left;
        }
        .float-container {
    border: 3px solid #fff;
    padding: 20px;
}

.float-child {
    width: 100%;
    float: left;
    padding: 20px;
    border: 2px solid red;
}  
    </style> 

    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.1/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-+0n0xVW2eSR5OomGNYDnhzAbDsOXxcvSN1TPprVMTNDbiYZCxYbOOl7+AMvyTG2x" crossorigin="anonymous">

<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.min.css" integrity="sha384-Gn5384xqQ1aoWXA+058RXPxPg6fy4IWvTNh0E263XmFcJlSAwiGgFAW/dAiS6JXm" crossorigin="anonymous">

  
</head>
<body>
     <form runat="server"> 
    <cc1:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
</cc1:ToolkitScriptManager>
         <div runat="server" class="Container border border-primary" style="padding-top:25px;">
        <div class="col-12">
  <div class="form-inline">
      
 <%-- <a class="btn btn-primary" data-toggle="collapse" href="#collapseExample" role="button" aria-expanded="false" aria-controls="collapseExample">
    Link with href
  </a>--%>
  <button class="btn btn-primary" type="button" data-toggle="collapse" data-target="#collapseExample" aria-expanded="false" aria-controls="collapseExample">
    Batch Destination
  </button>
          
      <div class="form-inline" style="padding-left:10px">
  
<select class="custom-select my-1 mr-sm-2" id="sl_Project"  runat="server"   > 
    <option value="SMS" selected>SMS</option>
    <option value="Haspo" >Haspo</option>
    <option value="Tennic">Tennic</option>
     <option value="Shinyoh">Shinyoh</option>
  </select>
</div>
</div>
<div class="collapse border-info" id="collapseExample">
   
   <%-- <form runat="server" class="form-inline">--%>
        <%-- <asp:ScriptManager ID="scriptmanager1" runat="server">  
</asp:ScriptManager>  --%>

  <div class="form-group mb-2">
    <input type="text" readonly class="form-control-plaintext" id="" value="Batch Path">
  </div>
  <div class="form-group mx-sm-3 mb-2">
    <input type="text" readonly class="form-control" id="bthPath" placeholder="C:\run.bat" runat="server" style="width: 500px;"/>
  </div>
        <asp:UpdatePanel ID="updatepnl" runat="server">  
<ContentTemplate>  
      <button type="submit" class="btn btn-primary mb-2" runat="server" onserverclick="UpdateClick"  style="cursor: not-allowed; opacity:0.6; "> Use Default Path </button>
     </ContentTemplate>  
</asp:UpdatePanel>  

    <div class="form-group mb-2">
    <input type="text" readonly class="form-control-plaintext" id="staticEmail2" value="Compiled Path">
  </div>
  <div class="form-group mx-sm-3 mb-2">
    <input type="text" class="form-control" id="cmpPath" readonly placeholder="C:\Errors.log" runat="server" style="width: 500px;"/>
  </div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">  
<ContentTemplate>  
      <button type="submit" class="btn btn-primary mb-2" runat="server" onserverclick="UpdateClick"  style="cursor: not-allowed; opacity:0.6; "> Use Default Path </button>


    </ContentTemplate>  
</asp:UpdatePanel>  
           
    <div class="form-group mb-2">
    <input type="text" readonly class="form-control-plaintext" id="staticEmail2" value="Execution Path"/>
  </div>
  <div class="form-group mx-sm-3 mb-2">
    <input type="text" class="form-control" id="execuPath" readonly placeholder="C:\SP.log" runat="server" style="width: 500px;"/>
  </div>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">  
<ContentTemplate>  
      <button type="submit" class="btn btn-primary mb-2" runat="server" onserverclick="UpdateClick"  style="cursor: not-allowed; opacity:0.6; "> Use Default Path </button>


    </ContentTemplate>  
</asp:UpdatePanel>  

      <div class="form-group mb-2">
    <input type="text" readonly class="form-control-plaintext" id="staticEmail2" value="Status Path">
  </div>
  <div class="form-group mx-sm-3 mb-2">
    <input type="text" class="form-control" id="stPath" readonly placeholder="C:\Flg.log" runat="server" style="width: 500px;"/>
  </div>
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">  
<ContentTemplate>  
      <button type="submit" class="btn btn-primary mb-2" runat="server" onserverclick="UpdateClick"  style="cursor: not-allowed; opacity:0.6; "> Use Default Path </button>


    </ContentTemplate>  
</asp:UpdatePanel>  
        <%-- </form>--%>
      </div>
</div>

    </div>
        <br />
       <div runat="server" class="Container border border-primary" style="padding-top:25px;">


        <div class="row" style="width: 100%; margin-left:0px !important;" >
  <div class="form-inline" > 
     <button type="button" class="btn btn-primary btn-danger" data-toggle="modal" data-target="#exampleModal" style="margin-left:30px">
  Trigger
</button>
        <button class="btn btn-primary" type="button" style="margin-left:20px;" runat="server" id="btnDisplayLog" onserverclick="ShowAllLog">
    Current Log
  </button>

      <label runat="server" id="status"  visible="false"  style="color:blue; font-weight:bolder; font-size:large; padding-left:10px;"> </label>
   <div class="spinner-border text-success" role="status" style="margin-left:10px;" runat="server" id="loader" visible="false">
</div>
</div>   
          <div class="float-container" style="width: inherit;">

  <div class="float-child">
      <p> Compilation Log</p>
      <hr />
    <div class="green">
        
     <p id="compiledLog" runat="server">
       


            </p>
    </div>
  </div>
  
  <div class="float-child">
      <p> Execution Log</p>
      <hr />
    <div class="blue">
        <p id="sqlLog" runat="server">
       
            </p>
    </div>
  </div>
  
</div> 
            </div>
           </div> 
         
<div class="modal fade" id="exampleModal" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
  <div class="modal-dialog">
    <div class="modal-content">
        
      <div class="modal-header">
        <h5 class="modal-title" id="exampleModalLabel">Comfirmation!</h5>
        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
          <span aria-hidden="true">&times;</span>
        </button>
      </div>
      <div class="modal-body">
        <span class="active"> Are you sure to run batch compiler?</span>
      </div>
      <div class="modal-footer form-inline" style="justify-content:space-between;">
        <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancel</button>
          <label class="col-form-label"  runat="server"> Key - </label>
    <input type="password" class="form-control" id="txtPass"  maxlength="10"  runat="server" style="width: 200px;"/>
  
<%--        <asp:UpdatePanel ID="UpdatePanel4" runat="server">  
<ContentTemplate> --%>
        <button  type="button" class="btn btn-primary" runat="server" onserverclick="Trigger"> Yes</button>
    <%--  </ContentTemplate>  
</asp:UpdatePanel>  --%>
      </div>
           
    </div>
  </div>
</div>
      <div class="modal fade" id="myModal" role="dialog">
    <div class="modal-dialog">
    
      <!-- Modal content-->
      <div class="modal-content">
        <div class="modal-header">
          <button type="button" class="close" data-dismiss="modal">&times;</button>
          <h4 class="modal-title">Modal Options</h4>
        </div>
        <div class="modal-body">
          <p>Modal content..</p>
        </div>
        <div class="modal-footer">
          <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
        </div>
      </div>
      
    </div>
  </div>
          </form>
    <script type="text/javascript">  
        function openModal() {
    $('#myModal').modal('show');  
}  
</script>
    <script src="https://code.jquery.com/jquery-3.2.1.slim.min.js" integrity="sha384-KJ3o2DKtIkvYIK3UENzmM7KCkRr/rE9/Qpg6aAZGJwFDMVNA/GpGFF93hXpG5KkN" crossorigin="anonymous"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.12.9/umd/popper.min.js" integrity="sha384-ApNbgh9B+Y1QKtv3Rn7W3mgPxhU9K/ScQsAP7hUibX39j7fakFPskvXusvfa0b4Q" crossorigin="anonymous"></script>
<script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/js/bootstrap.min.js" integrity="sha384-JZR6Spejh4U02d8jOt6vLEHfe/JQGiRRSQQxSfFWpi1MquVdAyjUar5+76PVCmYl" crossorigin="anonymous"></script>
</body>
</html>
