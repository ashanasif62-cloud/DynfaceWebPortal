using BussinessObject;
using System;
using System.Collections;
using System.Web.UI;

namespace GeneralAuxiliary
{
    public class NotificationMessage
    {
        public static void show(string message)
        {
            Page currentPage = System.Web.HttpContext.Current.CurrentHandler as Page;
            /*
                Request for 'Default.aspx' but an error and we do 'Response.Transfer' to custom 'ErrorHandler.aspx' page.
                'HttpContext.Current.CurrentHandler' will return an instance of 'ErrorHandler.aspx' (if called after the error)
                'HttpContext.Current.Handler'       would return an instance of 'Default.aspx'
             */
            if (currentPage != null && !string.IsNullOrEmpty(message))
            {
                message = message.Replace('\'', ' ').Replace('\"', ' ');
                currentPage.ClientScript.RegisterClientScriptBlock(currentPage.GetType(), "Alert", "alert('" + message + "')", true);

                //ScriptManager.RegisterClientScriptBlock(currentPage, currentPage.GetType(), "Alert", "alert('" + message + "')", true);
                //currentPage.ClientScript.RegisterStartupScript(currentPage.GetType(), "Message", "<script language='javascript'>alert('" + message + "');</script>", true);
            }
        }

        public static void showMessage(AlertType _alertType, string _message, bool _notifyParent = false)
        {
            Page currentPage = System.Web.HttpContext.Current.CurrentHandler as Page;
            string alertType = _alertType.ToString();
            string message = _message;
            bool notifyParent = _notifyParent;

            if (currentPage != null && !string.IsNullOrEmpty(message))
            {
                message = message.Replace("\\n", "<br/>");
                message = message.Replace("\n", "<br/>");
                message = message.Replace('\'', ' ');
                message = message.Replace('\"', ' ');
                message = message.Replace('`', ' ');
                //message = message.Replace('\\', ' ');
                //message = message.Replace('/', ' ');
                //message = message.Replace("\\n", "<br/>").Replace("\n", "<br/>").Replace('\'', ' ').Replace('\"', ' ');//.Replace('\\', ' ').Replace('/', ' ')
                //showNotificationMessage('20 Record(s) are deleted.', 'success')
                //currentPage.ClientScript.RegisterStartupScript(currentPage.GetType(), "Pop", "showNotificationMessage('" + message + "','" + alertType + "', '" + notifyParent + "');", true);
                ScriptManager.RegisterStartupScript(currentPage, currentPage.GetType(), "Pop", "showNotificationMessage('" + message + "','" + alertType + "', '" + notifyParent + "');", true);
            }

        }

        public static void showNotifications(AlertType _alertType, string message, string _title, bool _nonBlock = true, bool _hide = false)
        {
            Page currentPage = System.Web.HttpContext.Current.CurrentHandler as Page;
            AlertType alertType = _alertType;
            string title = _title;
            bool nonBlock = _nonBlock;
            bool hide = _hide;
            string notificationClass = string.Empty;
            string addclass = "dark";
            string type = string.Empty;
            //info,success,warning,error,info+addclass: 'dark'
            switch (alertType)
            {
                case AlertType.Success:
                    type = "success";
                    break;
                case AlertType.Warning:
                    type = "warning";
                    break;
                case AlertType.Information:
                    type = "info";
                    break;
                case AlertType.Error:
                    type = "error";
                    break;
            }

            //if (addclass)
            if (currentPage != null)
                currentPage.ClientScript.RegisterStartupScript(currentPage.GetType(), "Notifications", "" +
                    //"<script language='javascript'>" +
                    "new PNotify({" +
                    "title: '" + title + "'," +
                    "text: '" + message + "'," +
                    "type: '" + type + "'," +
                    //"nonblock: {" +
                    //"nonblock: " + nonBlock.ToString().ToLower() + "}," +
                    "hide: " + hide.ToString().ToLower() + "," +
                    "styling: 'bootstrap3'," +
                    "addclass: '" + addclass + "'" +
                    "});"
                    //+ "" 
                    //+ "</script>"
                    , true
                    );
            //currentPage.ClientScript.RegisterStartupScript(currentPage.GetType(), "Pop", "showErrorMessage('" + message + "', true,'" + alertType + "');", true);
            /*
             "new PNotify({
                title: 'Non-Blocking Notice',
                type: 'info',
                text: 'When you hover over me I\'ll fade to show the elements underneath. Feel free to click any of them just like I wasn\'t even here.',
                nonblock: {
                nonblock: true
                },
                styling: 'bootstrap3',
                addclass: 'dark'
                });"
                "new PNotify({
                                  title: 'Sticky Success',
                                  text: 'Sticky success... I\'m not even gonna make a joke.',
                                  type: 'info',
                                  hide: false,
                                  styling: 'bootstrap3',
                                  addclass: 'dark'
                              });"
             */
        }

        public static bool showMessage(string _xmlMessage)
        {
            bool result = false;
            string xmlMessage = _xmlMessage;
            string message = string.Empty;
            string type = string.Empty;
            AlertType alertType;

            if (!string.IsNullOrEmpty(xmlMessage))
            {
                Hashtable hTable = GetResults.getResultAttributes(xmlMessage);
                if (hTable != null)
                {
                    message = hTable["Message"].ToString();
                    type = hTable["Type"].ToString();
                    result = Convert.ToBoolean(hTable["Result"]);
                    alertType = (AlertType)Enum.Parse(typeof(AlertType), type);
                    showMessage(alertType, message);
                }
                else
                {
                    alertType = AlertType.Error;
                    message = "Something went wrong. Please wait and try again.";
                    showMessage(alertType, message);
                }
            }
            return result;
        }

        public static void showMessage(SysOperationResult_BOL _objBOL, bool _redirectToParent = false)
        {
            SysOperationResult_BOL objBOL = _objBOL;
            string message;
            AlertType alertType;
            bool redirectToParent = _redirectToParent;
            if (objBOL != null && objBOL.AlertType != null)
            {
                alertType = (AlertType)Enum.Parse(typeof(AlertType), objBOL.AlertType);
                message = objBOL.Message;
            }
            else
            {
                alertType = AlertType.Error;
                message = "Something went wrong. Please wait and try again.";
            }
            //if (redirectToParent)
            //{
            //    redirectToParentPage();
            //}

            showMessage(alertType, message, redirectToParent);

        }

        private static void redirectToParentPage()
        {
            Page currentPage = System.Web.HttpContext.Current.CurrentHandler as Page;

            //currentPage.ClientScript.RegisterStartupScript(currentPage.GetType(), "closeDialog", "closeDialog();", true);
            currentPage.ClientScript.RegisterStartupScript(currentPage.GetType(), "refreshParent", "setInterval(function(){window.parent.location.reload();}, 2000);", true);
        }

        public static void showInvalidRecord()
        {
            showMessage(AlertType.Error, "Unable to retrieve required information. Please select a valid record.");
        }
    }
}