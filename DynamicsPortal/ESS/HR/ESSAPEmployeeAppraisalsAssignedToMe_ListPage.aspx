<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="ESSAPEmployeeAppraisalsAssignedToMe_ListPage.aspx.cs" Inherits="DynamicsPortal.ESSAPEmployeeAppraisalsAssignedToMe_ListPage" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="actionPanel" ContentPlaceHolderID="ActionPanel" runat="server">
    <div class="action-items">
        <asp:LinkButton ID="btnSave" runat="server" OnClick="btnSave_Click"><i class="mdi mdi-content-save"></i>Save</asp:LinkButton>
    </div>
    <div class="action-items">
        <asp:LinkButton ID="btnReviewed" runat="server" OnClick="btnReviewed_Click"><i class="mdi mdi-account-check"></i>Reviewed</asp:LinkButton>
    </div>
    <div class="action-items">
            <asp:LinkButton ID="btnSendToLineManager" runat="server"   OnClick="btnSendToLineManager_Click"  ></asp:LinkButton>        
<%--   Original         <asp:LinkButton ID="btnSendToLineManager" runat="server"   OnClick="btnSendToLineManager_Click"  ><i class="mdi mdi-send"></i>Send To Line Manager</asp:LinkButton>        --%>
    </div>
    <%--<div class="action-items">
        <asp:LinkButton ID="btnView" runat="server"><i class="mdi mdi-eye"></i>View</asp:LinkButton>
    </div>
    <div class="action-items">
        <asp:LinkButton ID="btnSubmit" runat="server" OnClick="btnSubmit_Click"><i class="mdi mdi-shape-plus"></i>Submit</asp:LinkButton>
    </div>--%>
</asp:Content>
<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">
    <div>
        <asp:GridView ID="gridView_Appraisals"  OnSelectedIndexChanged = "OnSelectedIndexChange"  runat="server" data="searchable" CssClass="table table-condensed no-border table-hover sortable" ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found."
            DataKeyNames="RecId" AutoGenerateColumns="false">
            <Columns>
                <asp:TemplateField HeaderText="Employee Name">
                    <ItemTemplate>
                        <asp:Label ID="lblEmployeeName" runat="server" Text='<%# Bind("EmployeeName") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Position">
                    <ItemTemplate>
                        <asp:Label ID="lblPosition" runat="server" Text='<%# Bind("Position") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Department">
                    <ItemTemplate>
                        <asp:Label ID="lblDepartment" runat="server" Text='<%# Bind("Department") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Appraisal Code">
                    <ItemTemplate>
                        <asp:Label ID="lblAppraisalCode" runat="server" Text='<%# Bind("AppraisalCode") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Performance Period">
                    <ItemTemplate>
                        <asp:Label ID="lblPerformancePeriod" runat="server" Text='<%# Bind("PerformancePeriod") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Start Date">
                    <ItemTemplate>
                        <asp:Label ID="lblStartDate" runat="server" Text='<%# Bind("StartDate") %>' masktype="date"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="End Date">
                    <ItemTemplate>
                        <asp:Label ID="lblEndDate" runat="server" Text='<%# Bind("EndDate") %>' masktype="date" ></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Review Type">
                    <ItemTemplate>
                        <asp:Label ID="lblReviewType" runat="server" Text='<%# Bind("ReviewType") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
             <%--   <asp:TemplateField HeaderText="Total Score">
                    <ItemTemplate>
                        <asp:Label ID="lblTotalScore" runat="server" Text='<%# Bind("TotalScore") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>--%>
                <asp:TemplateField HeaderText="Score" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblScore" runat="server" Text='<%# Bind("Score") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Weightage" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblWeightage" runat="server" Text='<%# Bind("Weightage") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>


                <asp:TemplateField HeaderText="Employee" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblEmployee" runat="server" Text='<%# Bind("Employee") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="AppraisalStatus" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblAppraisalStatus" runat="server" Text='<%# Bind("AppraisalStatus") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Branch" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblBranch" runat="server" Text='<%# Bind("Branch") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="GenerationType" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblGenerationType" runat="server" Text='<%# Bind("GenerationType") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Grade" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblGrade" runat="server" Text='<%# Bind("Grade") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Location" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblLocation" runat="server" Text='<%# Bind("Location") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="MarkingFrequency" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblMarkingFrequency" runat="server" Text='<%# Bind("MarkingFrequency") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="EndDateTime" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblEndDateTime" runat="server" Text='<%# Bind("EndDateTime") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="StartDateTime" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblStartDateTime" runat="server" Text='<%# Bind("StartDateTime") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="RecId" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblRecId" runat="server" Text='<%# Bind("RecId") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderStyle-CssClass="sorttable_nosort"  >
                    <ItemTemplate>
                        <asp:LinkButton CssClass="grid-img-btn btn-details" ID ="gridView_KPIAppraisals" ToolTip="Lines" Text="Lines" runat="server" CommandName="Select" OnClick="Lines_Click" />                            
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="RecVersion" HeaderText="RecVersion" SortExpression="RecVersion" Visible="false" />
                <asp:BoundField DataField="ModifiedDateTime" HeaderText="ModifiedDateTime" SortExpression="ModifiedDateTime" Visible="false" />
                <asp:BoundField DataField="ModifiedBy" HeaderText="ModifiedBy" SortExpression="ModifiedBy" Visible="false" />
                <asp:BoundField DataField="CreatedDateTime" HeaderText="CreatedDateTime" SortExpression="CreatedDateTime" Visible="false" />
                <asp:BoundField DataField="CreatedBy" HeaderText="CreatedBy" SortExpression="CreatedBy" Visible="false" />
                <asp:BoundField DataField="DataAreaId" HeaderText="DataAreaId" SortExpression="DataAreaId" Visible="false" />
                <asp:BoundField DataField="Partition" HeaderText="Partition" SortExpression="Partition" Visible="false" />
            </Columns>
        </asp:GridView>
    </div>
    <div class="divTable" style="margin-top: 30px;">
        <div class="divTableBody">
            <div class="divTableRow">
                <div class="divTableCell" style="width: 65%;">
                    <div class="card card-header">
                        <div class="page-title" runat="server">KPI Appraisals</div>
                    </div>
                    <asp:GridView ID="gridView_KPIAppraisals" runat="server" CssClass="table table-condensed no-border table-hover sortable" ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found."
                        OnRowDataBound="gridView_KPIAppraisals_RowDataBound" DataKeyNames="RecId" AutoGenerateColumns="false">
                        <Columns>
                             <asp:TemplateField HeaderText="KPI">
                                <ItemTemplate>
                                    <asp:Label ID="lblKPI" runat="server" Text='<%# Bind("KPI") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="KPI Description">
                                <ItemTemplate>
                                    <asp:Label ID="lblKPIDescription" runat="server" Text='<%# Bind("KPIDescription") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Weightage">
                                <ItemTemplate>
                                    <asp:Label ID="lblKPIWeightage" runat="server"  Text='<%# Bind("KPIWeightage") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Net Amount">
                                <ItemTemplate>
                                    <asp:Label ID="lblNetAmount" runat="server"  Text='<%# Bind("NetAmount") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                              <asp:TemplateField HeaderText="Targets">
                                <ItemTemplate>
                                    <asp:Label ID="lblTargets" runat="server"  Text='<%# Bind("Targets") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Score">
                                <ItemTemplate>
                                          <%--  <input id="value" name="value" type="text" value="'<%#Eval("Score") %>" onkeyup="this.value = minmax(this.value)" />
                                            <label id="error" class="error"></label>--%>
                               <asp:TextBox ID="txtScore" MaxLength="1" runat="server" Text='<%# Bind("Score") %>' Width="50" masktype="number">  </asp:TextBox>  
                                  <asp:RangeValidator ID="txtScoreRangeValidator" ControlToValidate="txtScore"
             Type="Integer" MinimumValue="0" MaximumValue="4" Display="Dynamic" 
             ErrorMessage=" Enter Score 1-4"
             runat="server"/>
                                </ItemTemplate>
                               <%-- <EditItemTemplate>
                                    <asp:Label ID="lblScore" runat="server" Text='<%# Bind("Score") %>'></asp:Label>
                                </EditItemTemplate>--%>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Final Score">
                                <ItemTemplate>
                                    <asp:Label ID="lblFinalScore" runat="server" Text='<%# Bind("FinalScore") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Comments">
                                <ItemTemplate>
                                    <%--<asp:Label ID="lblComments" runat="server" Text='<%# Bind("Comments") %>'></asp:Label>--%>
                                   <asp:TextBox ID="txtComments" runat="server" Text='<%# Bind("Comments") %>' Width="200"> </asp:TextBox><%--<input type="text" name="Score" id="txtScore"  title="Error Message" pattern="[1-5]{1}[0-5]{5}"style="width:50px";>--%> 

                                </ItemTemplate>
                                <%-- <EditItemTemplate>--%>
<%--                                    <asp:TextBox ID="txtComments" runat="server"  Text='<%# Bind("Comments") %>'></asp:TextBox>--%>
                                <%-- </EditItemTemplate>--%>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Appraisers" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblAppraisers" runat="server" Text='<%# Bind("Appraisers") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Weightage" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblWeightage" runat="server" Text='<%# Bind("Weightage") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="AssignedTo" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblAssignedTo" runat="server" Text='<%# Bind("AssignedTo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Name" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblName" runat="server" Text='<%# Bind("Name") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="AppraiserStatus" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblAppraiserStatus" runat="server" Text='<%# Bind("AppraiserStatus") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="RefRecId" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblRefRecId" runat="server" Text='<%# Bind("RefRecId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="RecId" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblRecId" runat="server" Text='<%# Bind("RecId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:BoundField DataField="RecVersion" HeaderText="RecVersion" SortExpression="RecVersion" Visible="false" />
                            <asp:BoundField DataField="ModifiedDateTime" HeaderText="ModifiedDateTime" SortExpression="ModifiedDateTime" Visible="false" />
                            <asp:BoundField DataField="ModifiedBy" HeaderText="ModifiedBy" SortExpression="ModifiedBy" Visible="false" />
                            <asp:BoundField DataField="CreatedDateTime" HeaderText="CreatedDateTime" SortExpression="CreatedDateTime" Visible="false" />
                            <asp:BoundField DataField="CreatedBy" HeaderText="CreatedBy" SortExpression="CreatedBy" Visible="false" />
                            <asp:BoundField DataField="DataAreaId" HeaderText="DataAreaId" SortExpression="DataAreaId" Visible="false" />
                            <asp:BoundField DataField="Partition" HeaderText="Partition" SortExpression="Partition" Visible="false" />
                        </Columns>
                    </asp:GridView>
                </div>
                <div class="divTableCell" style="width: 20%; padding-left: 55px; cursor: grab;">
                    <div id="mydiv">
                        <div class="card card-header">
                            <div class="page-title" runat="server">Rating</div>
                            <asp:GridView ID="gridView_Rating" runat="server" CssClass="table table-condensed no-border table-hover sortable" ShowHeaderWhenEmpty="true" EmptyDataText="No Record Found."
                                DataKeyNames="RecId" AutoGenerateColumns="false">
                                <Columns>
                                    <asp:TemplateField HeaderText="From">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFromRange" runat="server" Text='<%# Bind("FromRange") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="To">
                                        <ItemTemplate>
                                            <asp:Label ID="lblToRange" runat="server" Text='<%# Bind("ToRange") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Rating">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRating" runat="server" Text='<%# Bind("Rating") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="AppraisalCode" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAppraisalCode" runat="server" Text='<%# Bind("AppraisalCode") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Description" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDescription" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Grade" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblGrade" runat="server" Text='<%# Bind("Grade") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="RatingScale" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRatingScale" runat="server" Text='<%# Bind("RatingScale") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="RecId" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRecId" runat="server" Text='<%# Bind("RecId") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:BoundField DataField="RecVersion" HeaderText="RecVersion" SortExpression="RecVersion" Visible="false" />
                                    <asp:BoundField DataField="ModifiedDateTime" HeaderText="ModifiedDateTime" SortExpression="ModifiedDateTime" Visible="false" />
                                    <asp:BoundField DataField="ModifiedBy" HeaderText="ModifiedBy" SortExpression="ModifiedBy" Visible="false" />
                                    <asp:BoundField DataField="CreatedDateTime" HeaderText="CreatedDateTime" SortExpression="CreatedDateTime" Visible="false" />
                                    <asp:BoundField DataField="CreatedBy" HeaderText="CreatedBy" SortExpression="CreatedBy" Visible="false" />
                                    <asp:BoundField DataField="DataAreaId" HeaderText="DataAreaId" SortExpression="DataAreaId" Visible="false" />
                                    <asp:BoundField DataField="Partition" HeaderText="Partition" SortExpression="Partition" Visible="false" />
                                </Columns>
                            </asp:GridView>                             
                        </div>
                          <div class="divTableCell" style="width: 20%; padding-left: 55px; cursor: grab;">
                    <div id="mydiv1">
                        <div class="card card-header">
                            <div class="page-title" runat="server">Total Final Score</div>
                                
                                    <asp:Label ID="lblTotalFinalScore" runat="server"/>                           
                                                
                        </div>
                        <script>
        const minmax = (txtscore, min = 1, max = 4) => {
            if(txtscore == '-') {
                document.getElementById('error').innerHTML = ''
                return '';
            }
            else if(parseInt(txtscore) < min || (isNaN(parseInt(txtscore)) && value !== '') || parseInt(txtscore) > max) {
                document.getElementById('error').innerHTML = ''
                return '';
            }
            document.getElementById('error').innerHTML = ''
            return txtscore;
                            }
                            //Button
                           
    </script>
                       <%-- <script>
                            function change(txtScore) {
                                var score = document.getElementById('<%=txtScore.ClientID%>);
                                if (score.value.length >= 1)
                                    document.getElementById(txtScore).disabled = true; 
                                else
                                    document.getElementById(txtScore).disabled = false; 
                            }
                        </script>--%>
                

                    </div>
                </div>
            </div>
        </div>
    </div>
   
    <style>
        .error {
            color: red;
        }
    </style>

</asp:Content>
