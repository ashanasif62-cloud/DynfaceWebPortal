<%@ Page Title="Advances Request" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ESSPRAdvancesRequest_ListPage.aspx.cs" Inherits="DynamicsPortal.ESS.PR.ESSPRAdvancesRequest_ListPage" %>


    <asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
        <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>

    </asp:Content>
    <asp:Content ID="Content1" ContentPlaceHolderID="PageContent" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <div class="row">
            <div class="col-md-12">
                <h3 class="page-header">Advances Request</h3>
                <asp:UpdatePanel ID="upnlGrid" runat="server">
                    <ContentTemplate>
                        <div class="table-responsive">
                            <asp:GridView ID="gvAdvancesRequest" runat="server" AutoGenerateColumns="False" OnRowDataBound="gridView_RowDataBound"
                                CssClass="table table-striped table-bordered table-hover"
                                EmptyDataText="No Advances Request Found" ShowFooter="true">
                                <Columns>
                                    <asp:BoundField DataField="RecoveryDate" HeaderText="Recovery Date"
                                        DataFormatString="{0:dd-MMM-yyyy}" ItemStyle-Width="25%" />
                                    <asp:BoundField DataField="PayGroupCode" HeaderText="Pay Group Code"
                                        ItemStyle-Width="25%" />
                                    <asp:BoundField DataField="AdvanceTypeCode" HeaderText="Advance Type Code"
                                        ItemStyle-Width="25%" />

                                    <asp:TemplateField HeaderText="Recovery Amount" ItemStyle-HorizontalAlign="Right"
                                        ItemStyle-Width="25%">
                                        <ItemTemplate>
                                            <span style="padding-right: 15px;">
                                                <asp:Label ID="lblRecAmt" runat="server"
                                                    Text='<%# Eval("RecoveryAmount", "{0:N2}") %>' />
                                            </span>
                                        </ItemTemplate>
                                        <FooterStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                </Columns>
                                <FooterStyle BackColor="#f5f5f5" Font-Bold="true" />
                            </asp:GridView>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </asp:Content>