<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LinkDevice.aspx.cs" Inherits="DynamicsPortal.ESS.LinkDevice" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8" />
    <title>Link a device</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <style>
        /* Basic responsive styling inspired by WhatsApp's Link Device screen */
        :root {
            --accent: #25D366;
            --muted: #6b6b6b;
            --bg: #f8f9fa;
            --card: #ffffff;
            --max-width: 980px;
        }
        body {
            margin: 0;
            font-family: "Segoe UI", Roboto, "Helvetica Neue", Arial, sans-serif;
            background: var(--bg);
            color: #111;
            -webkit-font-smoothing: antialiased;

            /* Center the content */
            display: flex;
            align-items: center;
            justify-content: center;
            min-height: 100vh; /* full height of viewport */
        }

        .wrap {
            max-width: var(--max-width);
            width: 100%;
            background: var(--card);
            display: grid;
            grid-template-columns: 1fr 420px;
            gap: 24px;
            box-shadow: 0 6px 24px rgba(0,0,0,0.08);
            border-radius: 12px;
            overflow: hidden;
        }
        header.h {
            display:flex;
            gap:12px;
            align-items:center;
            padding: 20px 28px;
            border-bottom: 1px solid #eee;
        }
        .logo {
            display:flex;
            align-items:center;
            gap:12px;
        }
        .logo .dot {
            width:36px;height:36px;border-radius:8px;background:var(--accent);
            display:inline-block;
            box-shadow: 0 2px 6px rgba(37,211,102,0.18);
        }
        .logo h1 { font-size:16px;margin:0; }
        .left {
            padding: 28px;
        }
        .left h2 { margin: 0 0 8px; font-size:20px; }
        .left p { color:var(--muted); margin: 0 0 18px; line-height:1.45; }
        .steps { margin-top: 12px; }
        .step {
            display:flex; gap:12px; align-items:flex-start; margin-bottom:14px;
        }
        .step .num {
            background:#f1f1f1;border-radius:50%;width:32px;height:32px;display:flex;align-items:center;justify-content:center;font-weight:600;
        }
        .right {
            background: linear-gradient(180deg, #ffffff, #fbfdff);
            padding: 28px;
            display:flex;
            flex-direction:column;
            align-items:center;
            justify-content:center;
        }
        .qr-card {
            width: 260px;
            height: 260px;
            border-radius: 12px;
            background: #fafafa;
            display:flex;
            align-items:center;
            justify-content:center;
            border:1px solid #eee;
            box-shadow: 0 8px 20px rgba(16,24,40,0.04);
        }
        .qr-card img { max-width: 220px; max-height: 220px; }
        .hint { margin-top:18px; color:var(--muted); text-align:center; font-size:14px; }
        .small { font-size:13px; color:var(--muted); margin-top:8px; text-align:center; }
        .btn {
            display:inline-block;
            padding:10px 14px; border-radius:8px; margin-top:14px;
            background:var(--accent); color:#fff; text-decoration:none; font-weight:600;
        }
        @media (max-width:880px) {
            .wrap { grid-template-columns: 1fr; margin: 18px; }
            .right { order: -1; padding-top:20px; padding-bottom:20px; }
        }
    </style>
</head>
<body>
    <div class="wrap">
        <div>
            <header class="h">
                <div class="logo">
<%--                    <span class="dot"></span>--%>
                    <div>
                        <h1>Connect Dynaface App</h1>
                        <div style="font-size:12px;color:var(--muted)">Add configuration from this web app</div>
                    </div>
                </div>
            </header>

            <div class="left">
                <h2>Adding  Configuration</h2>
                <p>Open Dynaface App on your phone, On Login Page <strong>Tap on the QR Icon</strong>, and tap on add Configuration.</p>

                <div class="steps">
                    <div class="step">
                        <div class="num">1</div>
                        <div>
                            <div style="font-weight:600">Open Dynaface App on your phone</div>
                            <div class="small">Android: QR Icon → Add Configuration.</div>
                        </div>
                    </div>

                    <div class="step">
                        <div class="num">2</div>
                        <div>
                            <div style="font-weight:600">Scan this QR code</div>
                            <div class="small">Point your phone camera at the QR to add it.</div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="right">
            <div class="qr-card">
                <!-- server sets the image src to data URL -->
                <asp:Image ID="imgQr" runat="server" AlternateText="QR Code not Available" />
            </div>

            <div class="hint">Scan with your phone's Dynaface App</div>
        </div>
    </div>

    <script>
        // Small client-side nicety: click btn to focus (server does regenerate)
    </script>
</body>
</html>
