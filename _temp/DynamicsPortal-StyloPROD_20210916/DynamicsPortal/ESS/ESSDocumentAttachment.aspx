<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Modal.Master" CodeBehind="ESSDocumentAttachment.aspx.cs" Inherits="DynamicsPortal.ESSDocumentAttachment" %>


<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">

    <script type="text/javascript" lang="javascript">
        function showBrowseDialog() {
            var userImgUpload = document.getElementById('PageContent_fileUpload');
            userImgUpload.click();
            return false;
        }
        function upload() {
            var btnUpload = document.getElementById('PageContent_btnUpload');
            btnUpload.click();
        }
    </script>

    <style>
        .download-attachment {
            width: 100px;
            display: inline-grid;
            margin: 5px;
            /* cursor: pointer; */
        }

            .download-attachment div {
                font-size: 10px;
                padding: 0px 3px;
                text-align: center;
                color: #353535;
                line-height: 11px;
                word-wrap: anywhere;
                word-break: break-word;
            }

            .download-attachment a {
                background: #ffffff;
                /* border: solid 1px #f9f9f9; */
                /* border-radius: 2px; */
                display: inline-block;
                height: 50px;
                line-height: 50px;
                position: relative;
                text-align: center;
                vertical-align: middle;
                width: 100px;
            }

                .download-attachment a span {
                    background: #f2594b;
                    border-radius: 4px;
                    color: #ffffff;
                    display: inline-block;
                    font-size: 11px;
                    font-weight: 700;
                    line-height: normal;
                    padding: 5px 10px;
                    position: relative;
                    text-transform: uppercase;
                    z-index: 1;
                }


                .download-attachment a::before,
                .download-attachment a::after {
                    background: #ffffff;
                    border: solid 3px #949596;
                    border-radius: 4px;
                    content: '';
                    display: block;
                    height: 35px;
                    left: 50%;
                    margin: -17px 0 0 -12px;
                    position: absolute;
                    top: 50%;
                    /* transform: translate(-50%,-50%); */
                    width: 25px;
                }

                .download-attachment a:hover:before,
                .download-attachment a:hover:after {
                    /*background: #e2e8f0;*/
                }
                /*a:before{transform:translate(-30%,-60%);}*/

                .download-attachment a:before {
                    margin: -23px 0 0 -5px;
                }

                .download-attachment a:hover {
                    /* background: #e2e8f0; */
                    /* border-color: #9fb4cc; */
                }

                .download-attachment a:active {
                    /*background: #dae0e8;
                    box-shadow: inset 0 2px 2px rgba(0, 0, 0, .25);*/
                }

                .download-attachment a input:first-child {
                    position: absolute;
                    right: 0;
                    border: none;
                }

                .download-attachment a span:nth-child(2) {
                    display: none;
                }

                .download-attachment a:hover span:nth-child(2) {
                    display: inline-block;
                }

                .download-attachment a span:last-child {
                    margin-left: -20px;
                }

                .download-attachment a:hover span:last-child {
                    display: none;
                }
    </style>


</asp:Content>

<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">
    <div class="action-panel-grid">
        <div class="action-items-grid">
            <asp:Button ID="btnUpload" runat="server" Style="display: none" OnClientClick="return confirm('Are you sure to add this attachment?')" OnClick="btnNew_Click" />
            <asp:LinkButton ID="btnNew" runat="server" OnClientClick="return showBrowseDialog();">
                <i class="mdi mdi-plus"></i>New
            </asp:LinkButton>
        </div>
        <div class="action-items-grid">
            <asp:LinkButton ID="btnView" runat="server" OnClientClick="return checkAttachments(event)" OnClick="btnViewAttachment_Click">
                <i class="mdi mdi-eye"></i>View
            </asp:LinkButton>
        </div>
        <div class="action-items-grid">
            <asp:LinkButton ID="btnDownload" runat="server" OnClientClick="return checkAttachments(event)" OnClick="btnDownload_Click">
                <i class="mdi mdi-cloud-download"></i> Download
            </asp:LinkButton>
        </div>
        <div class="action-items-grid">
            <asp:LinkButton ID="btnDelete" runat="server" OnClientClick="return checkAttachments(event)" OnClick="btnDelete_Click">
                <i class="mdi mdi-delete"></i>Delete
            </asp:LinkButton>
        </div>
    </div>

    <div id="fileList" runat="server">
    </div>

    <%-- accept=""--%>
    <asp:FileUpload ID="fileUpload" runat="server" Style="display: none;" accept="application/pdf, application/msword,
        application/vnd.openxmlformats-officedocument.wordprocessingml.document, image/*"
        onchange="upload();" />

    <div style="margin-top: 5px;">
        <%--type="application/pdf" height="700" width="850"--%>
        <embed id="embed01" runat="server" />
    </div>

    <div>
    </div>
    <script>
        function selectAttachment(args) {
            if ($(args).find("input")[0].checked) {
                $(args).find("input").prop("checked", false);
            }
            else {
                $('.download-attachment').each(function () {
                    $(this).find("input").prop("checked", false);
                });
                $(args).find("input").prop("checked", true);
            }
            return false;
        }

        function checkAttachments(event) {
            var checkCount = 0;
            $('.download-attachment').each(function () {
                //$(this).find("input").prop("checked", false);
                if ($(this).find("input")[0].checked) {
                    checkCount = checkCount + 1;
                }

            });

            if (checkCount !== 1) {
                showNotificationMessage('Please select one Attachment File.', 'Error', 'false');
                event.stopPropagation();
                event.preventDefault();
                return false;
            }
            else {
                return true;
            }
        }
    </script>
</asp:Content>

