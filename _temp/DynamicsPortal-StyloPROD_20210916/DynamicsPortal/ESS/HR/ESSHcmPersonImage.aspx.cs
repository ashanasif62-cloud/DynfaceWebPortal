using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using System;
using System.IO;
using System.Web;

namespace DynamicsPortal
{
    public partial class ESSHcmPersonImage : ModalForm
    {
        private HcmPersonImage hcmPersonImage = new HcmPersonImage();

        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                pageMenuId = "ESSHRPersonalDetailsHistory";
                showPageTitle = false;

                base.Page_Load(sender, e);

                if (!isUserAuthenticated)
                    return;

                if (!IsPostBack)
                {
                    bindImage();
                }
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
            }
            finally
            { }
        }
        private void bindImage()
        {
            try
            {
                string imageData = "data:image/png;base64," + ControlsHelper.getCurrentUserImage();
                imgUser.ImageUrl = imageData;
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
            }
            finally
            { }
        }

        private void reBindImage()
        {
            try
            {
                string imageData = "data:image/png;base64," + ControlsHelper.getCurrentUserImage(true);
                imgUser.ImageUrl = imageData;
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
            }
            finally
            { }
        }

        //public str getImageAsBase64png()
        //{
        //    Image imgObj;
        //    BinData bd;
        //    str result;

        //    if (this.Image)
        //    {
        //        imgObj = new Image(this.Image);
        //        imgObj.saveType(ImageSaveType::PNG);

        //        bd = new BinData();
        //        bd.setData(imgObj.getData());
        //        result = bd.base64Encode();
        //    }
        //    else
        //    {
        //        result = "";
        //    }
        //    return result;
        //}

        //private byte[] objectToByteArray(Object obj)
        //{
        //    try
        //    {
        //        if (obj == null)
        //            return null;
        //        BinaryFormatter bf = new BinaryFormatter();
        //        MemoryStream ms = new MemoryStream();
        //        bf.Serialize(ms, obj);
        //        return ms.ToArray();
        //    }
        //    catch (Exception ex)
        //    {
        //        SysErrorLog objErrorLog = new SysErrorLog();
        //        System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
        //        string currentMethodName = currentMethod.DeclaringType.FullName;
        //        objErrorLog.write(currentMethodName, ex);
        //        return null;
        //    }
        //    finally
        //    { }
        //}

        //private Object byteArrayToObject(byte[] arrBytes)
        //{
        //    try
        //    {
        //        //MemoryStream memStream = new MemoryStream();
        //        //BinaryFormatter binForm = new BinaryFormatter();
        //        //memStream.Write(arrBytes, 0, arrBytes.Length);
        //        //memStream.Seek(0, SeekOrigin.Begin);
        //        //Object obj = binForm.Deserialize(memStream);

        //        object[] obj = arrBytes.Cast<object>().ToArray();
        //        return obj;
        //    }
        //    catch (Exception ex)
        //    {
        //        SysErrorLog objErrorLog = new SysErrorLog();
        //        System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
        //        string currentMethodName = currentMethod.DeclaringType.FullName;
        //        objErrorLog.write(currentMethodName, ex);
        //        return null;
        //    }
        //    finally
        //    { }
        //}

        protected void btnNew_Click(object sender, EventArgs e)
        {
            try
            {
                string employeeId = SessionVariables.getCurrentEmployeeId();
                string message = string.Empty;
                //hcmPersonImage.update(employeeId, imageDate);
                if (!userImgUpload.HasFile)
                {
                    message = "Please select an image to upload.";
                    NotificationMessage.showMessage(AlertType.Error, message);
                    return;
                }
                else
                {
                    //string filename = Path.GetFileName(userImgUpload.PostedFile.FileName);
                    HttpPostedFile imageFile = userImgUpload.PostedFile;
                    int imageSize = imageFile.ContentLength;
                    if (imageSize < 5000000)
                    {
                        string imageType = userImgUpload.PostedFile.ContentType;

                        using (Stream imageStream = userImgUpload.PostedFile.InputStream)
                        {
                            using (BinaryReader br = new BinaryReader(imageStream))
                            {
                                byte[] imageBytes = br.ReadBytes((Int32)imageStream.Length);

                                // Convert byte[] to Base64 String
                                string imageData = Convert.ToBase64String(imageBytes);

                                SysOperationResult_BOL operationResult_BOL = hcmPersonImage.update(employeeId, imageData);
                                bool result = operationResults(operationResult_BOL);
                                if (result)
                                {
                                    reBindImage();
                                }
                            }
                        }

                    }
                    else
                    {
                        message = "Image size must be less than 5mb.";
                        NotificationMessage.showMessage(AlertType.Error, message);
                        return;
                    }
                }

            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
            }
            finally
            { }
        }
        
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string employeeId = SessionVariables.getCurrentEmployeeId();

                SysOperationResult_BOL operationResult_BOL = hcmPersonImage.delete(employeeId);
                bool result = operationResults(operationResult_BOL);
                if (result)
                {
                    reBindImage();
                }

            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
            }
            finally
            { }

        }

    }
}