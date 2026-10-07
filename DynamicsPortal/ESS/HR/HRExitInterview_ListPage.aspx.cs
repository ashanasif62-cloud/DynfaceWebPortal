using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using GeneralAuxiliary;
using PortalIntegration;

namespace DynamicsPortal.ESS.HR
{
    public partial class HRExitInterview_ListPage : MainForm
    {
        private HRExitInteviewsSvc _svc = new HRExitInteviewsSvc();


        #region Selected Values

        private long SelectedQuestionRecId
        {
            get
            {
                return ViewState["SelectedQuestionRecId"] != null
                    ? Convert.ToInt64(ViewState["SelectedQuestionRecId"])
                    : 0;
            }
            set
            {
                ViewState["SelectedQuestionRecId"] = value;
            }
        }


        private long SelectedEmpQuestionRecId
        {
            get
            {
                return ViewState["SelectedEmpQuestionRecId"] != null
                    ? Convert.ToInt64(ViewState["SelectedEmpQuestionRecId"])
                    : 0;
            }
            set
            {
                ViewState["SelectedEmpQuestionRecId"] = value;
            }
        }


        private string SelectedQuestionType
        {
            get
            {
                return ViewState["SelectedQuestionType"] as string ?? "";
            }
            set
            {
                ViewState["SelectedQuestionType"] = value;
            }
        }


        private int SelectedMcqIndex
        {
            get
            {
                return ViewState["SelectedMcqIndex"] != null
                    ? (int)ViewState["SelectedMcqIndex"]
                    : -1;
            }
            set
            {
                ViewState["SelectedMcqIndex"] = value;
            }
        }

        #endregion


        #region Page Load

        protected override void Page_Load(object sender, EventArgs e)
        {
            pageMenuId = "HRExitInterView_ListPage";

            if (!IsPostBack)
            {
                Page.Title = "Exit Interview";

                var titleDiv =
                    Master.FindControl("pageTitle")
                    as System.Web.UI.HtmlControls.HtmlGenericControl;

                if (titleDiv != null)
                {
                    titleDiv.InnerText = "Exit Interview";
                    titleDiv.Style["font-weight"] = "bold";
                }

                BindQuestions();
            }
        }

        #endregion


        #region Questions

        private void BindQuestions()
        {
            try
            {
                DataTable dt = _svc.retrieveQuestions();

                if (dt != null && dt.Rows.Count > 0)
                {
                    gvQuestions.DataSource = dt;
                    gvQuestions.DataBind();

                    SessionVariables.setSessionDataTable(dt);
                }
                else
                {
                    gvQuestions.DataSource = null;
                    gvQuestions.DataBind();

                    SessionVariables.setSessionDataTable(null);
                }

                ClearAnswerPanel();
                ClearMcqPanel();
            }
            catch (Exception ex)
            {
                gvQuestions.DataSource = null;
                gvQuestions.DataBind();

                SysErrorLog objErrorLog = new SysErrorLog();

                objErrorLog.write(
                    $"{this.GetType().FullName}.{nameof(BindQuestions)}",
                    ex);
            }
        }


        private void BindMcqs(string reference)
        {
            try
            {
                DataTable mcqTable =
                    _svc.retrieveQuestions(reference);

                if (mcqTable != null && mcqTable.Rows.Count > 0)
                {
                    gvMcqs.DataSource = mcqTable;
                    gvMcqs.DataBind();
                }
                else
                {
                    gvMcqs.DataSource = null;
                    gvMcqs.DataBind();
                }
            }
            catch (Exception ex)
            {
                gvMcqs.DataSource = null;
                gvMcqs.DataBind();

                SysErrorLog objErrorLog = new SysErrorLog();

                objErrorLog.write(
                    $"{this.GetType().FullName}.{nameof(BindMcqs)}",
                    ex);
            }
        }


        private void ClearAnswerPanel()
        {
            txtAnswer.Text = string.Empty;
        }


        private void ClearMcqPanel()
        {
            gvMcqs.DataSource = null;
            gvMcqs.DataBind();

            txtDescription.Text = string.Empty;

            SelectedMcqIndex = -1;
        }

        #endregion


        #region Question Grid

        protected void gvQuestions_RowCommand(
            object sender,
            GridViewCommandEventArgs e)
        {
            if (e.CommandName != "SelectRow")
                return;

            try
            {
                int rowIndex =
                    Convert.ToInt32(e.CommandArgument);

                if (rowIndex < 0 ||
                    rowIndex >= gvQuestions.Rows.Count)
                {
                    return;
                }


                // Highlight selected question
                HighlightSelectedQuestion(rowIndex);


                GridViewRow selectedRow =
                    gvQuestions.Rows[rowIndex];


                Label lblQuestionRecId =
                    selectedRow.FindControl(
                        "lblQuestionRecId") as Label;


                Label lblEmpQuestionRecId =
                    selectedRow.FindControl(
                        "lblEmpQuestionRecId") as Label;


                Label lblQuestionType =
                    selectedRow.FindControl(
                        "lblQuestionType") as Label;


                if (lblQuestionRecId == null ||
                    string.IsNullOrWhiteSpace(
                        lblQuestionRecId.Text))
                {
                    NotificationMessage.showMessage(
                        "Question RecId not found.");

                    return;
                }


                if (lblEmpQuestionRecId == null ||
                    string.IsNullOrWhiteSpace(
                        lblEmpQuestionRecId.Text))
                {
                    NotificationMessage.showMessage(
                        "Employee question RecId not found.");

                    return;
                }


                // Master question RecId
                SelectedQuestionRecId =
                    Convert.ToInt64(
                        lblQuestionRecId.Text);


                // Employee question RecId
                SelectedEmpQuestionRecId =
                    Convert.ToInt64(
                        lblEmpQuestionRecId.Text);


                SelectedQuestionType =
                    lblQuestionType?.Text?.Trim() ?? "";


                // Clear previous answer
                ClearAnswerPanel();

                // Clear previous MCQs
                ClearMcqPanel();


                // Determine question type
                SetTextBoxesByQuestionType(
                    SelectedQuestionType);


                /*
                 * Load MCQs only for Multiple Choice.
                 */
                bool isMultipleChoice =
                    SelectedQuestionType.IndexOf(
                        "Multiple",
                        StringComparison.OrdinalIgnoreCase) >= 0;


                if (isMultipleChoice)
                {
                    string reference = "";

                    BindMcqs(reference);
                }


                // Load existing answer
                LoadExistingAnswer(rowIndex);
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog =
                    new SysErrorLog();

                objErrorLog.write(
                    $"{this.GetType().FullName}.{nameof(gvQuestions_RowCommand)}",
                    ex);
            }
        }


        private void HighlightSelectedQuestion(
            int selectedIndex)
        {
            foreach (GridViewRow row in gvQuestions.Rows)
            {
                LinkButton btn =
                    row.FindControl(
                        "btnSelectQuestion") as LinkButton;

                if (btn != null)
                {
                    btn.CssClass =
                        row.RowIndex == selectedIndex
                            ? "round-checkbox selected"
                            : "round-checkbox";
                }
            }
        }

        #endregion


        #region Existing Answer

        private void LoadExistingAnswer(int rowIndex)
        {
            try
            {
                DataTable dt =
                    SessionVariables.getSessionDataTable();


                if (dt == null ||
                    dt.Rows.Count == 0)
                {
                    dt = _svc.retrieveQuestions();
                }


                if (dt == null ||
                    rowIndex >= dt.Rows.Count)
                {
                    return;
                }


                DataRow row =
                    dt.Rows[rowIndex];


                string existingAnswer = "";


                if (dt.Columns.Contains("answer"))
                {
                    existingAnswer =
                        row["answer"] == DBNull.Value
                            ? ""
                            : row["answer"].ToString();
                }


                bool isMultipleChoice =
                    SelectedQuestionType.IndexOf(
                        "Multiple",
                        StringComparison.OrdinalIgnoreCase) >= 0;


                if (isMultipleChoice)
                {
                    txtDescription.Text =
                        existingAnswer;

                    txtAnswer.Text =
                        string.Empty;
                }
                else
                {
                    txtAnswer.Text =
                        existingAnswer;

                    txtDescription.Text =
                        string.Empty;
                }
            }
            catch (Exception ex)
            {
                txtAnswer.Text = string.Empty;
                txtDescription.Text = string.Empty;

                SysErrorLog objErrorLog =
                    new SysErrorLog();

                objErrorLog.write(
                    $"{this.GetType().FullName}.{nameof(LoadExistingAnswer)}",
                    ex);
            }
        }

        #endregion


        #region Question Type

        private void SetTextBoxesByQuestionType(
            string questionType)
        {
            bool isMultipleChoice =
                !string.IsNullOrEmpty(questionType) &&
                questionType.IndexOf(
                    "Multiple",
                    StringComparison.OrdinalIgnoreCase) >= 0;


            if (isMultipleChoice)
            {
                // MCQ
                txtAnswer.Enabled = false;
                txtAnswer.Text = string.Empty;

                txtDescription.Enabled = true;
            }
            else
            {
                // Normal question
                txtAnswer.Enabled = true;

                txtDescription.Enabled = false;
                txtDescription.Text = string.Empty;
            }
        }

        #endregion


        #region MCQ

        protected void gvMcqs_RowCommand(
            object sender,
            GridViewCommandEventArgs e)
        {
            if (e.CommandName != "SelectMcq")
                return;


            int rowIndex =
                Convert.ToInt32(e.CommandArgument);


            SelectedMcqIndex =
                rowIndex;


            HighlightSelectedMcq(
                rowIndex);
        }


        private void HighlightSelectedMcq(
            int selectedIndex)
        {
            foreach (GridViewRow row in gvMcqs.Rows)
            {
                LinkButton btn =
                    row.FindControl(
                        "btnSelectMcq") as LinkButton;


                if (btn != null)
                {
                    btn.CssClass =
                        row.RowIndex == selectedIndex
                            ? "round-checkbox selected"
                            : "round-checkbox";
                }
            }
        }


        protected void gvMcqs_RowDataBound(
            object sender,
            GridViewRowEventArgs e)
        {
            if (e.Row.RowType !=
                DataControlRowType.DataRow)
            {
                return;
            }


            CheckBox chk =
                e.Row.FindControl(
                    "chkAnswer") as CheckBox;


            if (chk == null)
                return;


            string answer =
                DataBinder.Eval(
                    e.Row.DataItem,
                    "answer")?.ToString() ?? "";


            if (answer.Equals(
                    "Yes",
                    StringComparison.OrdinalIgnoreCase)
                || answer == "1")
            {
                chk.Checked = true;

                SelectedMcqIndex =
                    e.Row.RowIndex;
            }
        }


        protected void chkAnswer_CheckedChanged(
            object sender,
            EventArgs e)
        {
            CheckBox chk =
                (CheckBox)sender;


            GridViewRow currentRow =
                (GridViewRow)chk.NamingContainer;


            if (!chk.Checked)
                return;


            // Single selection
            foreach (GridViewRow row in gvMcqs.Rows)
            {
                CheckBox otherChk =
                    row.FindControl(
                        "chkAnswer") as CheckBox;


                if (otherChk != null &&
                    otherChk != chk)
                {
                    otherChk.Checked = false;
                }
            }


            SelectedMcqIndex =
                currentRow.RowIndex;


            HighlightSelectedMcq(
                currentRow.RowIndex);
        }

        #endregion


        #region Save

        protected void btnSave_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                /*
                 * Master question RecId
                 */
                if (SelectedQuestionRecId == 0)
                {
                    NotificationMessage.showMessage(
                        "Please select a question first.");

                    return;
                }


                /*
                 * Employee question RecId
                 */
                if (SelectedEmpQuestionRecId == 0)
                {
                    NotificationMessage.showMessage(
                        "Employee question record was not found.");

                    return;
                }


                bool isMultipleChoice =
                    SelectedQuestionType.IndexOf(
                        "Multiple",
                        StringComparison.OrdinalIgnoreCase) >= 0;


                long selectedMcqRecId = 0;

                string answer = "";


                /*
                 * ==============================
                 * MULTIPLE CHOICE
                 * ==============================
                 */
                if (isMultipleChoice)
                {
                    foreach (GridViewRow row in gvMcqs.Rows)
                    {
                        CheckBox chk =
                            row.FindControl(
                                "chkAnswer") as CheckBox;


                        if (chk != null &&
                            chk.Checked)
                        {
                            selectedMcqRecId =
                                Convert.ToInt64(
                                    gvMcqs.DataKeys[
                                        row.RowIndex].Value);

                            break;
                        }
                    }


                    if (selectedMcqRecId == 0)
                    {
                        NotificationMessage.showMessage(
                            "Please select an MCQ option.");

                        return;
                    }


                    /*
                     * X++ does:
                     *
                     * str2Enum(NoYes, _answer)
                     *
                     * Therefore send Yes.
                     */
                    answer = "Yes";
                }


                /*
                 * ==============================
                 * NORMAL QUESTION
                 * ==============================
                 */
                else
                {
                    answer =
                        txtAnswer.Text.Trim();


                    if (string.IsNullOrWhiteSpace(answer))
                    {
                        NotificationMessage.showMessage(
                            "Please enter an answer.");

                        return;
                    }
                }


                /*
                 * Call consumption method
                 *
                 * X++:
                 *
                 * updateAnswerByQuestion(
                 *     _questionRecId,
                 *     _empQuetionRecId,
                 *     _mcqRecId,
                 *     _answer
                 * )
                 */
                bool success =
                    _svc.saveAnswer(
                        SelectedQuestionRecId,
                        SelectedEmpQuestionRecId,
                        selectedMcqRecId,
                        answer);


                if (success)
                {
                    NotificationMessage.showMessage(
                        "Answer saved successfully.");

                    BindQuestions();
                }
                else
                {
                    NotificationMessage.showMessage(
                        "Failed to save the answer.");
                }
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog =
                    new SysErrorLog();

                objErrorLog.write(
                    $"{this.GetType().FullName}.{nameof(btnSave_Click)}",
                    ex);

                NotificationMessage.showMessage(
                    "An error occurred while saving the answer.");
            }
        }

        #endregion
    }
}