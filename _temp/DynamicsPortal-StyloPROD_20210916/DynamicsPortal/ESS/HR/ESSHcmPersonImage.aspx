<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Modal.Master" CodeBehind="ESSHcmPersonImage.aspx.cs" Inherits="DynamicsPortal.ESSHcmPersonImage" %>


<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">

    <script type="text/javascript" lang="javascript">
        function showBrowseDialog() {
            var userImgUpload = document.getElementById('PageContent_userImgUpload');
            userImgUpload.click();
            return false;
        }
        function upload() {
            var btnUpload = document.getElementById('PageContent_btnUpload');
            btnUpload.click();
        }
    </script>

</asp:Content>

<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">
    <%--    <div>
        <asp:Label ID="lblMessage" runat="server"></asp:Label>
        <input type="button" value="Upload" onclick="return showBrowseDialog();" />
    </div>--%>

    <div class="action-panel-grid">
        <div class="action-items-grid">
            <asp:Button ID="btnUpload" runat="server" Style="display: none" OnClientClick="return confirm('Are you sure to upload this image?')" OnClick="btnNew_Click" />
            <asp:LinkButton ID="btnNew" runat="server" OnClientClick="return showBrowseDialog();"><i class="mdi mdi-plus"></i>Upload new image</asp:LinkButton>
        </div>
        <div class="action-items-grid">
            <asp:LinkButton ID="btnDelete" runat="server" OnClick="btnDelete_Click"><i class="mdi mdi-delete"></i>Remove</asp:LinkButton>
        </div>
        <%--    <div class="action-items">
        <asp:LinkButton ID="btnEdit" runat="server"><i class="mdi mdi-border-color"></i>Edit</asp:LinkButton>
    </div>--%>
        <%--    <div class="action-items">
        <asp:LinkButton ID="btnView" runat="server"><i class="mdi mdi-eye"></i>View</asp:LinkButton>
    </div>
        <div class="action-items">
            <asp:LinkButton ID="btnSubmit" runat="server" OnClick="btnSubmit_Click"><i class="mdi mdi-shape-plus"></i>Submit</asp:LinkButton>
        </div>--%>
    </div>
    <asp:FileUpload ID="userImgUpload" runat="server" accept="image/*" Style="display: none;" onchange="upload();" />
    <asp:Image ID="imgUser" runat="server" Width="300" Height="300" />

    <div>
    </div>
</asp:Content>
