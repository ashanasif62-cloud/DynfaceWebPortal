<%@ Page Title="Outstanding Advances" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ESSPROutstandingAdvances_ListPage.aspx.cs" Inherits="DynamicsPortal.ESSPROutstandingAdvances_ListPage"
    %>

    <asp:Content ID="Content1" ContentPlaceHolderID="PageContent" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <div class="row p-2">
            <div class="col-md-12">
                <asp:UpdatePanel ID="upnlGrid" runat="server">
                    <ContentTemplate>
                        <div class="table-responsive" style="text-align:center; vertical-align:middle">
                            <asp:GridView ID="gvOutstanding" runat="server" AutoGenerateColumns="False"
                                CssClass="table table-striped table-bordered table-hover"
                                EmptyDataText="No Outstanding Advances Found" ShowFooter="true">
                                <Columns>
                                    <asp:BoundField DataField="AdvanceTypeCode" HeaderText="Type" />
                                    <asp:BoundField DataField="PayPeriodCode" HeaderText="Period" />
                                    <asp:BoundField DataField="PaymentDate" HeaderText="Payment Date"
                                        DataFormatString="{0:dd-MM-yyyy}" />

                                    <asp:TemplateField HeaderText="Advance Amount" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <span style="padding-right: 100px; text-align:center; vertical-align:middle">
                                                <asp:Label ID="lblAdv" runat="server"
                                                    Text='<%# Eval("AdvanceAmount", "{0:N2}") %>' />
                                            </span>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Total Recovery" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <span style="padding-right: 100px; text-align:center; vertical-align:middle">
                                                <asp:Label ID="lblRec" runat="server"
                                                    Text='<%# Eval("TotalRecoveryAmount", "{0:N2}") %>' />
                                            </span>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Remaining" ItemStyle-HorizontalAlign="Right"
                                        HeaderStyle-ForeColor="Red">
                                        <ItemTemplate>
                                            <span style="padding-right: 100px; text-align:center; vertical-align:middle">
                                                <asp:Label ID="lblRem" runat="server" Font-Bold="true"
                                                    Text='<%# Eval("RemainingAmount", "{0:N2}") %>' />
                                            </span>
                                        </ItemTemplate>
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