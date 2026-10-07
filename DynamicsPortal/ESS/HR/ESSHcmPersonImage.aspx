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
    <asp:UpdatePanel ID="updHelpDeskForm" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
    <ContentTemplate>

        <%-- ── Pending image section (shown when draft exists) ── --%>
<%--        <asp:Panel ID="pnlPending" runat="server" Visible="false">
            <div style="border:1px solid #e0e0e0; border-radius:6px; margin-bottom:16px; overflow:hidden;">
                <div style="background:#f8f8f8; padding:10px 14px; border-bottom:1px solid #e0e0e0; display:flex; align-items:center; justify-content:space-between;">
                    <strong style="font-size:14px;">Pending image change</strong>
                    <div style="display:flex; gap:8px;">
                        <asp:LinkButton ID="btnSubmit" runat="server" OnClick="btnSubmit_Click"
                            Style="color:#0078d4; font-size:13px; text-decoration:none;">
                            <i class="mdi mdi-refresh"></i> Workflow
                        </asp:LinkButton>
                        <asp:LinkButton ID="btnDeletePending" runat="server" OnClick="btnDeletePending_Click"
                            Style="color:#d13438; font-size:13px; text-decoration:none;">
                            <i class="mdi mdi-delete"></i> Delete
                        </asp:LinkButton>
                    </div>
                </div>
                <div style="padding:14px;">
                    <div style="font-size:12px; color:#605e5c; margin-bottom:8px;">Status</div>
                    <div style="background:#f3f2f1; color:#605e5c; font-size:12px; padding:4px 10px; border-radius:3px; display:inline-block; margin-bottom:12px;">
                        <asp:Label ID="lblPendingStatus" runat="server" Text="Draft"></asp:Label>
                    </div>
                    <br />
                    <asp:Image ID="imgPending" runat="server" Width="80" Height="80"
                        Style="border-radius:4px; object-fit:cover; border:1px solid #e0e0e0;" />
                </div>
            </div>
        </asp:Panel>--%>

        <%-- ── Current approved image section ── --%>
        <div style="border:1px solid #e0e0e0; border-radius:6px; overflow:hidden;">
            <div style="background:#f8f8f8; padding:10px 14px; border-bottom:1px solid #e0e0e0; display:flex; align-items:center; justify-content:space-between;">
                <strong style="font-size:14px;">My image</strong>
            </div>
            <div style="padding:14px;">
                <div style="margin-bottom:12px; display:flex; gap:8px;">
                    <asp:LinkButton ID="btnNew" runat="server" OnClientClick="return showBrowseDialog();"
                        Style="color:#0078d4; font-size:13px; text-decoration:none;" Visible="false">
                        <i class="mdi mdi-plus"></i> Upload new image
                    </asp:LinkButton>
                    <asp:LinkButton ID="btnDelete" runat="server" OnClick="btnDelete_Click"
                        Style="color:#605e5c; font-size:13px; text-decoration:none;" Visible="false">
                        <i class="mdi mdi-delete-outline"></i> Remove
                    </asp:LinkButton>
                </div>
                <asp:Image ID="imgUser" runat="server" Width="200" Height="200"
                    Style="border-radius:6px; object-fit:cover; border:1px solid #e0e0e0;" />
            </div>
        </div>

        <asp:Button ID="btnUpload" runat="server" Style="display:none !important;"
            OnClick="btnNew_Click" />
        <asp:FileUpload ID="userImgUpload" runat="server" accept="image/*"
            Style="display:none;" onchange="upload();" />
        <asp:Label ID="lblSubmitStatus" runat="server"
            Style="color:#107c10; font-weight:600; margin-top:8px; display:block;"></asp:Label>

    </ContentTemplate>
           <Triggers>
        <asp:PostBackTrigger ControlID="btnUpload" />
    </Triggers>
    </asp:UpdatePanel>
</asp:Content>