<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserLogin.aspx.cs" Inherits="DynamicsPortal.Administration.UserLogin" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <title id="siteTitle" runat="server">Maison Consulting & Solutions</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="shortcut icon" type="image/png" sizes="16x16" href="/distribution/img/favicon.png" />

    <!-- CSS -->
    <link rel="stylesheet" href="/distribution/vendor/bootstrap/css/bootstrap.min.css" />
    <link rel="stylesheet" href="/distribution/vendor/font-awesome/css/font-awesome.min.css" />
    <link rel="stylesheet" href="/distribution/icons-reference/MaterialDesign-Webfont-master/css/materialdesignicons.css" />
    <link rel="stylesheet" href="/distribution/css/custom.css" />
    <link rel="stylesheet" href="/distribution/css/login-page.css" />

    <!-- JS -->
    <script src="/distribution/js/jquery-3.3.1.min.js"></script>

    <script>
        $(document).ready(function () {
            var showPass = 0;
            $('.btn-show-pass').on('click', function () {
                var $input = $(this).siblings('input');
                var $icon = $(this).find('i');
                if (showPass === 0) {
                    $input.attr('type', 'text');
                    $icon.removeClass('fa-eye').addClass('fa-eye-slash');
                    showPass = 1;
                } else {
                    $input.attr('type', 'password');
                    $icon.removeClass('fa-eye-slash').addClass('fa-eye');
                    showPass = 0;
                }
            });
        });

        function showNotificationMessage(_errorMsg, _alertType) {
            var cssclass;
            switch (_alertType.toLowerCase()) {
                case 'success': cssclass = 'success'; break;
                case 'error': cssclass = 'error'; break;
                case 'warning': cssclass = 'warning'; break;
                default: cssclass = 'info';
            }
            var $panel = $('#notificationPanel');
            $panel.removeClass().addClass('notification-panel ' + cssclass).show();
            $panel.find('p').html(_errorMsg);
        }
    </script>

    <style>
        .Config-Btn {
            background-color: #f8f9fa;
            border: 1px solid #ccc;
            color: #333;
            font-size: 14px;
            font-weight: bold;
            padding: 8px 14px;
            border-radius: 6px;
            cursor: pointer;
            transition: all 0.2s ease-in-out;
        }

        .Config-Btn:hover {
            background-color: #e9ecef;
            border-color: #999;
        }
        .Login-Btn {
            display: flex; align-items: center; justify-content: center; width: 100%; padding: 12px; 
           font-size: 16px; font-weight: bold; border-radius: 6px; text-align: center; 
           text-decoration: none; border: none; cursor: pointer; background-color: #2F2F2F; 
           color: white; margin-bottom: 10px;
        }
        .MS-Login-Btn {
            display: flex; align-items: center; justify-content: center; width: 100%; padding: 12px; 
           font-size: 16px; font-weight: bold; border-radius: 6px; text-align: center; 
           text-decoration: none; border: none; cursor: pointer; background-color: #0047AB; 
           color: white; margin-bottom: 10px;
        }
        .G-Login-Btn {
            display: flex; align-items: center; justify-content: center; width: 100%; padding: 12px; 
           font-size: 16px; font-weight: bold; border-radius: 6px; text-align: center; 
           text-decoration: none; border: none; cursor: pointer; background-color: #DB4437; 
           color: white; margin-bottom: 10px;
        }
        button, input[type="submit"] {
            font-family: 'FontAwesome', 'Segoe UI', sans-serif;
        }
</style>

</head>

<body>
    <div class="limiter">
        <div class="container-login100">
<%--            <div style="position: absolute; top: 15px; right: 20px; z-index: 9999;">
                <button type="button" class="Config-Btn" onclick="location.href='/EditConfig.aspx'">
                    ⚙ Environment Config
                </button>
            </div>--%>
            <div class="wrap-login100">
                <div>
                 <img src="/distribution/img/Maison-Logo.png" alt="logo" style="width: fit-content; display: block; margin: 0 auto 20px;" />
                </div>

                <!-- Notification Panel -->
                <div id="notificationPanel" style="display: none; margin-bottom: 5px;" class="notification-panel">
                    <div class="notification-container float-left">
                        <p></p>
                    </div>
                    <div class="float-right">
                        <div class="action-items" title="Close">
                            <a href="javascript:void(0);" onclick="$('#notificationPanel').hide();"><i class="mdi mdi-close"></i></a>
                        </div>
                    </div>
                </div>
                <!-- Login Form -->
                <form id="form1" runat="server" class="login100-form validate-form flex-sb flex-w">
                    <div style="width: 100%;">
                        <%--<span class="txt1">User Name</span>--%>
                      <%--  <div class="wrap-input100 validate-input" data-validate="Username is required">
                            <input id="txtUserId" runat="server" type="text" title="User Id" class="input100" />
                            <span class="focus-input100"></span>
                        </div>

                        <span class="txt1">Password</span>
                        <div class="wrap-input100 validate-input m-b-12" data-validate="Password is required">
                            <span class="btn-show-pass">
                                <i class="fa fa-eye"></i>
                            </span>
                            <input id="txtPassword" runat="server" type="password" title="Password" class="input100" />
                            <span class="focus-input100"></span>
                        </div>

                        <div class="login100-form-btns w-full">
                            <asp:Button ID="btnLogin" runat="server" CssClass="Login-Btn" Text="&#xf023; Log in" OnClick="btnLogin_Click" />
                        </div>--%>

                        <div class="login100-form-btns w-full">
                            <asp:Button ID="btnMicrosoftLogin" runat="server" CssClass="MS-Login-Btn" Text="&#xf17a; Log in with Microsoft" OnClick="btnMicrosoftLogin_Click" />
                        </div>

                        <div class="login100-form-btns w-full">
                            <asp:Button ID="btnGoogleLogin" runat="server" CssClass="G-Login-Btn" Text="&#xf1a0; Log in with Google" OnClick="btnGoogleLogin_Click" />
                        </div>

                    </div>
                </form>

            </div>
        </div>
    </div>

    <footer class="main-footer mt-4">
        <div class="container-fluid">
            <div class="row">
                <div class="col-sm-12 text-center">
                    <p>&copy; Maison Consulting & Solutions <%= DateTime.Now.Year %> | All Rights Reserved.</p>
                </div>
            </div>
        </div>
    </footer>
</body>
</html>