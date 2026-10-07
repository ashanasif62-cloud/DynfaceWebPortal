<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ESSEmployeeSurvay_InterviewQuestion.aspx.cs" Inherits="DynamicsPortal.ESS.HR.ESSEmployeeSurvay_InterviewQuestion" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ActionPanel" runat="server">

    <style>
        body {
            background-color: #fff;
        }

        .section-title {
            font-size: 16px;
            font-weight: 600;
            color: #333;
            margin-bottom: 10px;
            padding-bottom: 5px;
            border-bottom: 2px solid #0d6efd;
        }

        .table thead th {
            background-color: #f1f1f1;
            border-bottom: 1px solid #dee2e6;
            font-weight: 600;
            white-space: nowrap;
        }

        .table td {
            vertical-align: middle;
        }

        .round-checkbox {
            display: inline-block;
            width: 16px;
            height: 16px;
            border: 2px solid #666;
            border-radius: 50%;
            background-color: #fff;
            cursor: pointer;
            vertical-align: middle;
        }

        .round-checkbox.selected {
            background-color: #0d6efd;
            border: 2px solid #0d6efd;
        }

        .form-label {
            font-weight: 500;
            margin-bottom: 6px;
            display: block;
        }

        .grid-container {
            overflow-x: auto;
        }
    </style>

    <div class="action-items">

        <asp:LinkButton
            ID="btnSave"
            runat="server"
            OnClick="btnSave_Click">

            <i class="mdi mdi-content-save"></i> Save

        </asp:LinkButton>

    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageContent" runat="server">

    <div class="container-fluid py-3">

      

        <div class="row mb-4">

          

            <div class="col-md-8">

                <div class="grid-container">

                    <asp:GridView
                        ID="gvQuestions"
                        runat="server"
                        AutoGenerateColumns="false"
                        CssClass="table table-sm table-bordered"
                        DataKeyNames="question,empQuestionRecId"
                        OnRowCommand="gvQuestions_RowCommand"
                        ShowHeaderWhenEmpty="true"
                        EmptyDataText="No questions found.">

                        <Columns>

                         

                            <asp:TemplateField
                                HeaderText=""
                                ItemStyle-Width="45px"
                                ItemStyle-HorizontalAlign="Center">

                                <ItemTemplate>

                                    <asp:LinkButton
                                        ID="btnSelectQuestion"
                                        runat="server"
                                        CommandName="SelectRow"
                                        CommandArgument='<%# Container.DataItemIndex %>'
                                        CssClass="round-checkbox">
                                    </asp:LinkButton>

                                </ItemTemplate>

                            </asp:TemplateField>


                         

                            <asp:TemplateField
                                HeaderText="Question Type"
                                ItemStyle-Width="150px">

                                <ItemTemplate>

                                    <asp:Label
                                        ID="lblQuestionType"
                                        runat="server"
                                        Text='<%# Eval("QuestionType") %>'>
                                    </asp:Label>

                                </ItemTemplate>

                            </asp:TemplateField>

                         

                            <asp:TemplateField HeaderText="Question">

                                <ItemTemplate>

                                    <asp:Label
                                        ID="lblQuestion"
                                        runat="server"
                                        Text='<%# Eval("questionDescription") %>'>
                                    </asp:Label>

                                </ItemTemplate>

                            </asp:TemplateField>



                            <asp:TemplateField
                                HeaderText="RecId"
                                Visible="false">

                                <ItemTemplate>

                                    <asp:Label
                                        ID="lblQuestionRecId"
                                        runat="server"
                                        Text='<%# Eval("question") %>'>
                                    </asp:Label>

                                </ItemTemplate>

                            </asp:TemplateField>


                            <!-- EMPLOYEE QUESTION RECID -->

                            <asp:TemplateField
                                HeaderText="Employee Question RecId"
                                Visible="false">

                                <ItemTemplate>

                                    <asp:Label
                                        ID="lblEmpQuestionRecId"
                                        runat="server"
                                        Text='<%# Eval("empQuestionRecId") %>'>
                                    </asp:Label>

                                </ItemTemplate>

                            </asp:TemplateField>

                        </Columns>

                    </asp:GridView>

                </div>

            </div>


        

            <div class="col-md-4">

                <label class="form-label">
                    Answer
                </label>

                <asp:TextBox
                    ID="txtAnswer"
                    runat="server"
                    TextMode="MultiLine"
                    CssClass="form-control"
                    Rows="8"
                    placeholder="Type your answer here...">
                </asp:TextBox>

            </div>

        </div>


     

        <div class="row">

            <div class="col-12">

                <div class="section-title">
                    MCQs
                </div>

            </div>

        </div>


        <div class="row">

         

            <div class="col-md-8">

                <div class="grid-container">

                    <asp:GridView
                        ID="gvMcqs"
                        runat="server"
                        AutoGenerateColumns="false"
                        CssClass="table table-sm table-bordered"
                        DataKeyNames="mcqRecId"
                        OnRowCommand="gvMcqs_RowCommand"
                        OnRowDataBound="gvMcqs_RowDataBound"
                        ShowHeaderWhenEmpty="true"
                        EmptyDataText="No multiple choice options available.">

                        <Columns>

                          

                            <asp:TemplateField
                                HeaderText=""
                                ItemStyle-Width="45px"
                                ItemStyle-HorizontalAlign="Center">

                                <ItemTemplate>

                                    <asp:LinkButton
                                        ID="btnSelectMcq"
                                        runat="server"
                                        CommandName="SelectMcq"
                                        CommandArgument='<%# Container.DataItemIndex %>'
                                        CssClass="round-checkbox">
                                    </asp:LinkButton>

                                </ItemTemplate>

                            </asp:TemplateField>


                          

                            <asp:TemplateField
                                HeaderText="Multiple Choice">

                                <ItemTemplate>

                                    <asp:Label
                                        ID="lblMcqDescription"
                                        runat="server"
                                        Text='<%# Eval("mcqDescription") %>'>
                                    </asp:Label>

                                </ItemTemplate>

                            </asp:TemplateField>


                          

                            <asp:TemplateField
                                HeaderText="Answer"
                                ItemStyle-Width="90px"
                                ItemStyle-HorizontalAlign="Center">

                                <ItemTemplate>

                                    <asp:CheckBox
                                        ID="chkAnswer"
                                        runat="server"
                                        AutoPostBack="true"
                                        OnCheckedChanged="chkAnswer_CheckedChanged" />

                                </ItemTemplate>

                            </asp:TemplateField>

                        </Columns>

                    </asp:GridView>

                </div>

            </div>


          

            <div class="col-md-4">

                <label class="form-label">
                    Description
                </label>

                <asp:TextBox
                    ID="txtDescription"
                    runat="server"
                    TextMode="MultiLine"
                    CssClass="form-control"
                    Rows="8"
                    placeholder="Enter description (if required)...">
                </asp:TextBox>

            </div>

        </div>

    </div>

</asp:Content>