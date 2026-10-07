////using BussinessObject;
////using GeneralAuxiliary;
////using PortalIntegration;
////using System;
////using System.IO;
////using System.Runtime.Serialization.Formatters.Binary;
////using System.Web;

////namespace DynamicsPortal
////{
////    public partial class ESSHcmPersonImage : ModalForm
////    {
////        private HcmPersonImage hcmPersonImage = new HcmPersonImage();

////        protected override void Page_Load(object sender, EventArgs e)
////        {
////            try
////            {
////                pageMenuId = "ESSHRPersonalDetailsHistory";
////                showPageTitle = false;

////                base.Page_Load(sender, e);

////                if (!isUserAuthenticated)
////                    return;

////                if (!IsPostBack)
////                {
////                    bindImage();
////                }
////            }
////            catch (Exception ex)
////            {
////                SysErrorLog objErrorLog = new SysErrorLog();
////                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
////                string currentMethodName = currentMethod.DeclaringType.FullName;
////                objErrorLog.write(currentMethodName, ex);
////            }
////            finally
////            { }
////        }
////        private void bindImage()
////        {
////            try
////            {
////                string imageData = "data:image/png;base64," + ControlsHelper.getCurrentUserImage();
////                imgUser.ImageUrl = imageData;
////            }
////            catch (Exception ex)
////            {
////                SysErrorLog objErrorLog = new SysErrorLog();
////                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
////                string currentMethodName = currentMethod.DeclaringType.FullName;
////                objErrorLog.write(currentMethodName, ex);
////            }
////            finally
////            { }
////        }

////        private void reBindImage()
////        {
////            try
////            {
////                string imageData = "data:image/png;base64," + ControlsHelper.getCurrentUserImage(true);
////                imgUser.ImageUrl = imageData;
////            }
////            catch (Exception ex)
////            {
////                SysErrorLog objErrorLog = new SysErrorLog();
////                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
////                string currentMethodName = currentMethod.DeclaringType.FullName;
////                objErrorLog.write(currentMethodName, ex);
////            }
////            finally
////            { }
////        }

////        //public string getImageAsBase64png()
////        //{
////        //    Image imgObj;
////        //    BinData bd;
////        //    string result;

////        //    if (this.Image)
////        //    {
////        //        imgObj = new Image(this.Image);
////        //        imgObj.saveType(ImageSaveType::PNG);

////        //        bd = new BinData();
////        //        bd.setData(imgObj.getData());
////        //        result = bd.base64Encode();
////        //    }
////        //    else
////        //    {
////        //        result = "";
////        //    }
////        //    return result;
////        //}

////        private byte[] objectToByteArray(Object obj)
////        {
////            try
////            {
////                if (obj == null)
////                    return null;
////                BinaryFormatter bf = new BinaryFormatter();
////                MemoryStream ms = new MemoryStream();
////                bf.Serialize(ms, obj);
////                return ms.ToArray();
////            }
////            catch (Exception ex)
////            {
////                SysErrorLog objErrorLog = new SysErrorLog();
////                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
////                string currentMethodName = currentMethod.DeclaringType.FullName;
////                objErrorLog.write(currentMethodName, ex);
////                return null;
////            }
////            finally
////            { }
////        }

////        //private Object byteArrayToObject(byte[] arrBytes)
////        //{
////        //    try
////        //    {
////        //        MemoryStream memStream = new MemoryStream();
////        //        BinaryFormatter binForm = new BinaryFormatter();
////        //        memStream.Write(arrBytes, 0, arrBytes.Length);
////        //        memStream.Seek(0, SeekOrigin.Begin);
////        //        Object obj = binForm.Deserialize(memStream);

////        //        object[] obj = arrBytes.Cast<object>().ToArray();
////        //        return obj;
////        //    }
////        //    catch (Exception ex)
////        //    {
////        //        SysErrorLog objErrorLog = new SysErrorLog();
////        //        System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
////        //        string currentMethodName = currentMethod.DeclaringType.FullName;
////        //        objErrorLog.write(currentMethodName, ex);
////        //        return null;
////        //    }
////        //    finally
////        //    { }
////        //}

////        protected void btnNew_Click(object sender, EventArgs e)
////        {
////            try
////            {
////                string employeeId = SessionVariables.getCurrentEmployeeId();
////                string message = string.Empty;
////                //hcmPersonImage.update(employeeId, imageDate);
////                if (!userImgUpload.HasFile)
////                {
////                    message = "Please select an image to upload.";
////                    NotificationMessage.showMessage(AlertType.Error, message);
////                    return;
////                }
////                else
////                {
////                    //string filename = Path.GetFileName(userImgUpload.PostedFile.FileName);
////                    HttpPostedFile imageFile = userImgUpload.PostedFile;
////                    int imageSize = imageFile.ContentLength;
////                    if (imageSize < 5000000)
////                    {
////                        string imageType = userImgUpload.PostedFile.ContentType;

////                        using (Stream imageStream = userImgUpload.PostedFile.InputStream)
////                        {
////                            using (BinaryReader br = new BinaryReader(imageStream))
////                            {
////                                byte[] imageBytes = br.ReadBytes((Int32)imageStream.Length);

////                                // Convert byte[] to Base64 String
////                                string imageData = Convert.ToBase64String(imageBytes);

////                                SysOperationResult_BOL operationResult_BOL = hcmPersonImage.update(employeeId, imageData);
////                                bool result = operationResults(operationResult_BOL);
////                                if (result)
////                                {
////                                    reBindImage();
////                                }
////                            }
////                        }

////                    }
////                    else
////                    {
////                        message = "Image size must be less than 5mb.";
////                        NotificationMessage.showMessage(AlertType.Error, message);
////                        return;
////                    }
////                }

////            }
////            catch (Exception ex)
////            {
////                SysErrorLog objErrorLog = new SysErrorLog();
////                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
////                string currentMethodName = currentMethod.DeclaringType.FullName;
////                objErrorLog.write(currentMethodName, ex);
////            }
////            finally
////            { }
////        }

////        protected void btnDelete_Click(object sender, EventArgs e)
////        {
////            try
////            {
////                string employeeId = SessionVariables.getCurrentEmployeeId();

////                SysOperationResult_BOL operationResult_BOL = hcmPersonImage.delete(employeeId);
////                bool result = operationResults(operationResult_BOL);
////                if (result)
////                {
////                    reBindImage();
////                }

////            }
////            catch (Exception ex)
////            {
////                SysErrorLog objErrorLog = new SysErrorLog();
////                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
////                string currentMethodName = currentMethod.DeclaringType.FullName;
////                objErrorLog.write(currentMethodName, ex);
////            }
////            finally
////            { }

////        }

////    }
////}

//using BussinessObject;
//using GeneralAuxiliary;
//using PortalIntegration;
//using PortalIntegration.HcmPersonImageSvcReference;
//using System;
//using System.IO;
//using System.Web;

//namespace DynamicsPortal
//{
//    public partial class ESSHcmPersonImage : ModalForm
//    {
//        private HcmPersonImage hcmPersonImage = new HcmPersonImage();

//        // Session key constants
//        private const string SESSION_SUBMITTED_RECID = "PersonImage_SubmittedRecId";
//        private const string SESSION_SUBMITTED_MODIFIED = "PersonImage_SubmittedModifiedDateTime";

//        protected override void Page_Load(object sender, EventArgs e)
//        {
//            try
//            {
//                pageMenuId = "ESSHRPersonalDetailsHistory";
//                showPageTitle = false;

//                base.Page_Load(sender, e);

//                if (!isUserAuthenticated)
//                    return;

//                if (!IsPostBack)
//                {
//                    bindImage();
//                    checkSubmitStatus();
//                }
//            }
//            catch (Exception ex)
//            {
//                SysErrorLog objErrorLog = new SysErrorLog();
//                string currentMethodName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName;
//                objErrorLog.write(currentMethodName, ex);
//            }
//        }

//        // ── Checks if a pending submission exists and updates UI accordingly ──
//        private void checkSubmitStatus()
//        {
//            try
//            {
//                string employeeId = SessionVariables.getCurrentEmployeeId();
//                HcmPersonImageSvcContract imageContract = hcmPersonImage.retrieveByEmployee(employeeId);

//                if (imageContract == null || imageContract.RecId == 0)
//                {
//                    // No image in DB at all — allow upload, disable submit
//                    btnSubmit.Enabled = false;
//                    lblSubmitStatus.Text = "Please upload an image first.";
//                    return;
//                }

//                // Check if this RecId was previously submitted
//                long? sessionRecId = Session[SESSION_SUBMITTED_RECID] as long?;
//                DateTime? sessionModified = Session[SESSION_SUBMITTED_MODIFIED] as DateTime?;

//                if (sessionRecId.HasValue &&
//                    sessionRecId.Value == imageContract.RecId &&
//                    sessionModified.HasValue &&
//                    sessionModified.Value == imageContract.ModifiedDateTime)
//                {
//                    // Same image, same timestamp → still pending
//                    btnSubmit.Enabled = false;
//                    lblSubmitStatus.Text = "⚠ Request already submitted. Awaiting approval.";
//                }
//                else
//                {
//                    // Either never submitted, or image was changed/approved (ModifiedDateTime differs)
//                    // Clear old session so they can submit again
//                    if (sessionRecId.HasValue && sessionRecId.Value == imageContract.RecId &&
//                        sessionModified.HasValue && sessionModified.Value != imageContract.ModifiedDateTime)
//                    {
//                        // ModifiedDateTime changed → request was processed (approved/rejected)
//                        Session.Remove(SESSION_SUBMITTED_RECID);
//                        Session.Remove(SESSION_SUBMITTED_MODIFIED);
//                    }

//                    btnSubmit.Enabled = true;
//                    lblSubmitStatus.Text = string.Empty;
//                }
//            }
//            catch (Exception ex)
//            {
//                SysErrorLog objErrorLog = new SysErrorLog();
//                string currentMethodName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName;
//                objErrorLog.write(currentMethodName, ex);
//            }
//        }

//        private void bindImage()
//        {
//            try
//            {
//                string imageData = ControlsHelper.getCurrentUserImage();
//                imgUser.ImageUrl = string.IsNullOrEmpty(imageData)
//                    ? "/distribution/img/User.png"
//                    : "data:image/png;base64," + imageData;
//            }
//            catch (Exception ex)
//            {
//                SysErrorLog objErrorLog = new SysErrorLog();
//                string currentMethodName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName;
//                objErrorLog.write(currentMethodName, ex);
//            }
//        }

//        private void reBindImage()
//        {
//            try
//            {
//                string imageData = ControlsHelper.getCurrentUserImage(true);
//                imgUser.ImageUrl = string.IsNullOrEmpty(imageData)
//                    ? "/distribution/img/User.png"
//                    : "data:image/png;base64," + imageData;
//            }
//            catch (Exception ex)
//            {
//                SysErrorLog objErrorLog = new SysErrorLog();
//                string currentMethodName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName;
//                objErrorLog.write(currentMethodName, ex);
//            }
//        }

//        protected void btnNew_Click(object sender, EventArgs e)
//        {
//            try
//            {
//                string employeeId = SessionVariables.getCurrentEmployeeId();

//                if (!userImgUpload.HasFile)
//                {
//                    NotificationMessage.showMessage(AlertType.Error, "Please select an image to upload.");
//                    return;
//                }

//                HttpPostedFile imageFile = userImgUpload.PostedFile;

//                if (imageFile.ContentLength >= 5000000)
//                {
//                    NotificationMessage.showMessage(AlertType.Error, "Image size must be less than 5mb.");
//                    return;
//                }

//                using (Stream imageStream = imageFile.InputStream)
//                using (BinaryReader br = new BinaryReader(imageStream))
//                {
//                    byte[] imageBytes = br.ReadBytes((int)imageStream.Length);
//                    string imageData = Convert.ToBase64String(imageBytes);

//                    SysOperationResult_BOL operationResult = hcmPersonImage.update(employeeId, imageData);
//                    bool success = operationResults(operationResult);

//                    if (success)
//                    {
//                        // Clear any previous submission session — new image uploaded
//                        Session.Remove(SESSION_SUBMITTED_RECID);
//                        Session.Remove(SESSION_SUBMITTED_MODIFIED);

//                        reBindImage();
//                        checkSubmitStatus(); // Re-evaluate submit button state
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                SysErrorLog objErrorLog = new SysErrorLog();
//                string currentMethodName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName;
//                objErrorLog.write(currentMethodName, ex);
//            }
//        }

//        protected void btnDelete_Click(object sender, EventArgs e)
//        {
//            try
//            {
//                string employeeId = SessionVariables.getCurrentEmployeeId();

//                SysOperationResult_BOL operationResult = hcmPersonImage.delete(employeeId);
//                bool success = operationResults(operationResult);

//                if (success)
//                {
//                    // Clear submission session on delete
//                    Session.Remove(SESSION_SUBMITTED_RECID);
//                    Session.Remove(SESSION_SUBMITTED_MODIFIED);

//                    reBindImage();
//                    checkSubmitStatus();
//                }
//            }
//            catch (Exception ex)
//            {
//                SysErrorLog objErrorLog = new SysErrorLog();
//                string currentMethodName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName;
//                objErrorLog.write(currentMethodName, ex);
//            }
//        }

//        protected void btnSubmit_Click(object sender, EventArgs e)
//        {
//            try
//            {
//                string employeeId = SessionVariables.getCurrentEmployeeId();

//                // Get current image record from DB to retrieve RecId
//                HcmPersonImageSvcContract imageContract = hcmPersonImage.retrieveByEmployee(employeeId);

//                if (imageContract == null || imageContract.RecId == 0)
//                {
//                    NotificationMessage.showMessage(AlertType.Error, "No image found to submit. Please upload an image first.");
//                    return;
//                }

//                // Submit workflow
//                ESSWorkflow workflow = new ESSWorkflow();
//                SysOperationResult_BOL result = workflow.submitEssPersonImage(imageContract.RecId);
//                bool success = operationResults(result);

//                if (success)
//                {
//                    // Save submitted RecId + ModifiedDateTime in Session
//                    Session[SESSION_SUBMITTED_RECID] = imageContract.RecId;
//                    Session[SESSION_SUBMITTED_MODIFIED] = imageContract.ModifiedDateTime;

//                    btnSubmit.Enabled = false;
//                    lblSubmitStatus.Text = "✔ Request submitted successfully. Awaiting approval.";
//                }
//            }
//            catch (Exception ex)
//            {
//                SysErrorLog objErrorLog = new SysErrorLog();
//                string currentMethodName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName;
//                objErrorLog.write(currentMethodName, ex);
//            }
//        }
//    }
//}

using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using PortalIntegration.HcmPersonImageSvcReference;
using System;
using System.IO;
using System.Web;

namespace DynamicsPortal
{
    public partial class ESSHcmPersonImage : ModalForm
    {
        private HcmPersonImage hcmPersonImage = new HcmPersonImage();

        private const string SESSION_PENDING_RECID = "PersonImage_PendingRecId";
        private const string SESSION_PENDING_MODIFIED = "PersonImage_PendingModified";

        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                pageMenuId = "ESSHRPersonalDetailsHistory";
                showPageTitle = false;

                base.Page_Load(sender, e);

                if (!isUserAuthenticated) return;

                if (!IsPostBack)
                {
                    BindCurrentImage();
                    BindPendingImage();
                }
            }
            catch (Exception ex)
            {
                new SysErrorLog().write(
                    System.Reflection.MethodBase.GetCurrentMethod()
                    .DeclaringType.FullName, ex);
            }
        }

        /* ── Bind the current APPROVED image ── */
        private void BindCurrentImage()
        {
            string employeeId = SessionVariables.getCurrentEmployeeId();
            // This should read from HcmPersonImage (approved image)
            string imageData = ControlsHelper.getCurrentUserImage();
            imgUser.ImageUrl = string.IsNullOrEmpty(imageData)
                ? "/distribution/img/User.png"
                : "data:image/png;base64," + imageData;
        }

        /* ── Bind pending/draft image if one exists ── */
        private void BindPendingImage()
        {
            //try
            //{
            //    string employeeId = SessionVariables.getCurrentEmployeeId();
            //    HcmPersonImageSvcContract contract = hcmPersonImage.retrieveByEmployeePending(employeeId);

            //    // No record at all
            //    if (contract == null || contract.RecId == 0)
            //    {
            //        pnlPending.Visible = false;
            //        return;
            //    }

            //    // Use ApprovalStatus from contract — NOT image comparison
            //    bool isPending = contract.ApprovalStatus != "Completed";

            //    if (isPending)
            //    {
            //        pnlPending.Visible = true;
            //        imgPending.ImageUrl = string.IsNullOrEmpty(contract.StringImage)
            //            ? "/distribution/img/User.png"
            //            : "data:image/png;base64," + contract.StringImage;

            //        // Check session to decide Draft vs Submitted label
            //        long? sessionRecId = Session[SESSION_PENDING_RECID] as long?;

            //        if (sessionRecId.HasValue && sessionRecId.Value == contract.RecId)
            //        {
            //            lblPendingStatus.Text = "Submitted";
            //            btnSubmit.Enabled = false;
            //        }
            //        else
            //        {
            //            lblPendingStatus.Text = contract.ApprovalStatus; // Draft / InReview etc
            //            btnSubmit.Enabled = true;
            //        }
            //    }
            //    else
            //    {
            //        // Approved — clear pending panel and session
            //        pnlPending.Visible = false;
            //        Session.Remove(SESSION_PENDING_RECID);
            //        Session.Remove(SESSION_PENDING_MODIFIED);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    new SysErrorLog().write(
            //        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName, ex);
            //}
        }

        /* ── Upload new image → saves as pending draft ── */
        protected void btnNew_Click(object sender, EventArgs e)
        {
            try
            {
                string employeeId = SessionVariables.getCurrentEmployeeId();

                if (!userImgUpload.HasFile)
                {
                    NotificationMessage.showMessage(
                        AlertType.Error, "Please select an image.");
                    return;
                }

                if (userImgUpload.PostedFile.ContentLength >= 5000000)
                {
                    NotificationMessage.showMessage(
                        AlertType.Error, "Image must be less than 5MB.");
                    return;
                }

                using (Stream stream = userImgUpload.PostedFile.InputStream)
                using (BinaryReader br = new BinaryReader(stream))
                {
                    byte[] bytes = br.ReadBytes((int)stream.Length);
                    string imageData = Convert.ToBase64String(bytes);

                    SysOperationResult_BOL result =
                        hcmPersonImage.update(employeeId, imageData);

                    bool success = operationResults(result);

                    if (success)
                    {
                        // Clear old pending session — new upload resets state
                        Session.Remove(SESSION_PENDING_RECID);
                        Session.Remove(SESSION_PENDING_MODIFIED);

                        lblSubmitStatus.Text = string.Empty;

                        // Refresh both panels
                        BindCurrentImage();
                        BindPendingImage();
                    }
                }
            }
            catch (Exception ex)
            {
                new SysErrorLog().write(
                    System.Reflection.MethodBase.GetCurrentMethod()
                    .DeclaringType.FullName, ex);
            }
        }

        /* ── Delete current approved image ── */
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string employeeId = SessionVariables.getCurrentEmployeeId();

                SysOperationResult_BOL result =
                    hcmPersonImage.delete(employeeId);

                bool success = operationResults(result);

                if (success)
                {
                    Session.Remove(SESSION_PENDING_RECID);
                    Session.Remove(SESSION_PENDING_MODIFIED);

                    BindCurrentImage();
                    BindPendingImage();
                }
            }
            catch (Exception ex)
            {
                new SysErrorLog().write(
                    System.Reflection.MethodBase.GetCurrentMethod()
                    .DeclaringType.FullName, ex);
            }
        }

        /* ── Delete the pending draft image ── */
        protected void btnDeletePending_Click(object sender, EventArgs e)
        {
            try
            {
                string employeeId = SessionVariables.getCurrentEmployeeId();

                // Delete pending = restore current approved image
                // You may need a specific "cancel pending" API call here.
                // For now, re-upload the current approved image as the pending
                // OR simply call delete which removes any pending draft.
                SysOperationResult_BOL result =
                    hcmPersonImage.delete(employeeId);

                bool success = operationResults(result);

                if (success)
                {
                    Session.Remove(SESSION_PENDING_RECID);
                    Session.Remove(SESSION_PENDING_MODIFIED);

                    //pnlPending.Visible = false;
                    lblSubmitStatus.Text = string.Empty;

                    BindCurrentImage();
                    BindPendingImage();
                }
            }
            catch (Exception ex)
            {
                new SysErrorLog().write(
                    System.Reflection.MethodBase.GetCurrentMethod()
                    .DeclaringType.FullName, ex);
            }
        }

        /* ── Submit pending image for workflow approval ── */
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                string employeeId = SessionVariables.getCurrentEmployeeId();

                HcmPersonImageSvcContract contract =
                    hcmPersonImage.retrieveByEmployee(employeeId);

                if (contract == null || contract.RecId == 0)
                {
                    NotificationMessage.showMessage(
                        AlertType.Error,
                        "No image found. Please upload first.");
                    return;
                }

                ESSWorkflow workflow = new ESSWorkflow();
                SysOperationResult_BOL result =
                    workflow.submitEssPersonImage(SessionVariables.getCurrentEmployeePersonId());

                bool success = operationResults(result);

                if (success)
                {
                    // Save to session so we know it's submitted
                    Session[SESSION_PENDING_RECID] = contract.RecId;
                    Session[SESSION_PENDING_MODIFIED] = contract.ModifiedDateTime;

                    //lblPendingStatus.Text = "Submitted";
                    //btnSubmit.Enabled = false;
                    //lblSubmitStatus.Text =
                    //    "Image submitted for approval.";
                }

                NotificationMessage.showMessage(result);
            }
            catch (Exception ex)
            {
                new SysErrorLog().write(
                    System.Reflection.MethodBase.GetCurrentMethod()
                    .DeclaringType.FullName, ex);
            }
        }
    }
}