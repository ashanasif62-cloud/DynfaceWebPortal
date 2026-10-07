<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserLogin.aspx.cs" Inherits="DynamicsPortal.Administration.UserLogin" %>

<!DOCTYPE html>
<html>

<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <!-- Favicon icon -->
    <link rel="shortcut icon" type="image/png" sizes="16x16" href="/distribution/img/favicon.png" />
    <title id="siteTitle" runat="server">Maison Consulting & Solutions</title>
    <meta name="description" content="" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <meta name="robots" content="all,follow" />

    <!-- Bootstrap CSS-->
    <link rel="stylesheet" href="/distribution/vendor/bootstrap/css/bootstrap.min.css">
    <!-- Font Awesome CSS-->
    <link rel="stylesheet" href="/distribution/vendor/font-awesome/css/font-awesome.min.css">
    <link rel="stylesheet" href="/distribution/icons-reference/MaterialDesign-Webfont-master/css/materialdesignicons.css" />
    <!-- Custom CSS-->
    <link href="/distribution/css/custom.css" rel="stylesheet" />
    <link href="/distribution/css/login-page.css" rel="stylesheet" />



    <!-- JavaScript files-->
    <script src="/distribution/js/jquery-3.3.1.min.js"></script>

    <script>
        $(document).ready(function () {

            /*=============================[ Show pass ]=====================================*/
            var showPass = 0;
            $('.btn-show-pass').on('click', function () {
                if (showPass === 0) {
                    $(this).next('input').attr('type', 'text');
                    $(this).find('i').removeClass('fa-eye');
                    $(this).find('i').addClass('fa-eye-slash');
                    showPass = 1;
                }
                else {
                    $(this).next('input').attr('type', 'password');
                    $(this).find('i').removeClass('fa-eye-slash');
                    $(this).find('i').addClass('fa-eye');
                    showPass = 0;
                }
            });

        });


        function showNotificationMessage(_errorMsg, _alertType) {
            var errorMsg = _errorMsg;
            var alertType = _alertType.toLowerCase();
            var $notificationPanel = $('#notificationPanel');
            var cssclass;
            switch (alertType) {
                case 'success':
                    cssclass = ' success';
                    break;
                case 'error':
                    cssclass = ' error';
                    break;
                case 'warning':
                    cssclass = ' warning';
                    break;
                default:
                    cssclass = ' info';
            }

            $notificationPanel.show();
            $notificationPanel.addClass('' + cssclass + '');
            var msgBox = $notificationPanel.find('p');
            msgBox.html(errorMsg);
            //var oldMsg = msgBox.html();
            //msgBox.html(oldMsg + errorMsg);
            //$('#notificationPanel .notification-container').append('<p>Test</p>');
        }
    </script>

    <style>
    </style>

</head>

<body>
    <div class="limiter">
        <div class="container-login100">
            <div class="wrap-login100">
                <div>
                    <img src="/distribution/img/Stylo Logo.jpg" alt="person" style="width: 100%; margin-bottom: 35px;" />
                </div>

                <!-- Notification Panel-->
                <div id="notificationPanel" style="display: none; margin-bottom: 5px;" class="notification-panel">
                    <div class="notification-container float-left">
                        <p></p>
                    </div>
                    <div class="float-right">
                        <div class="action-items" data-toggle="tooltip" title="Close"><a onclick="$('#notificationPanel').hide();"><i class="mdi mdi-close"></i></a></div>
                    </div>
                </div>

                <!-- Login Form -->
                <form runat="server" class="login100-form validate-form flex-sb flex-w">
                    <div style="width: 100%;">
                        <%--<span class="login100-form-title">Account Login</span>--%>
                        <span class="txt1">User Name</span>
                        <div class="wrap-input100 validate-input" data-validate="Username is required">
                            <input id="txtUserId" type="text" title="User Id" runat="server" class="input100">
                            <span class="focus-input100"></span>
                        </div>
                        <span class="txt1">Password</span>
                        <div class="wrap-input100 validate-input m-b-12" data-validate="Password is required">
                            <span class="btn-show-pass">
                                <i class="fa fa-eye"></i>
                            </span>
                            <input id="txtPassword" type="password" title="Password" runat="server" class="input100">
                            <span class="focus-input100"></span>
                        </div>
                        <div class="login100-form-btns w-full">
                            <%--<div class="contact100-form-checkbox">
                                <input class="input-checkbox100" id="ckb1" type="checkbox" name="remember-me">
                                <label class="label-checkbox100" for="ckb1">
                                    Remember me
                                </label>
                            </div>--%>
                            <div class="contact100-form-checkbox">
                                <asp:Button ID="btnLogin" runat="server" class="login100-form-btn" Text="Log in" OnClick="btnLogin_Click" />
                            </div>
                           <%-- <div>
                                <a href="#" class="txt3">Forgot Password?</a>
                            </div>--%>
                        </div>
                    </div>
                </form>
            </div>


            <footer class="main-footer">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-sm-12">
                            <p>&copy; Maison Consulting & Solutions <%= DateTime.Now.Year %> | All Rights Reserved.</p>
                        </div>
                    </div>
                </div>
            </footer>
        </div>
    </div>

</body>


</html>
