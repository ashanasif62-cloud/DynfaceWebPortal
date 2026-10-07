<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="ESSHRPersonalDetails.aspx.cs" Inherits="DynamicsPortal.ESSHRPersonalDetails" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        label {
            display: inline-block;
            margin-bottom: 0px;
        }

        .tabs {
            left: 50%;
            -webkit-transform: translateX(-50%);
            transform: translateX(-50%);
            position: relative;
            padding: 10px;
        }

            .tabs input[name="tab-control"] {
                display: none;
            }

            .tabs .content section h2,
            .tabs ul li label {
                /* font-size: 18px; */
                /*color: #428BFF;*/
            }

            .tabs ul {
                list-style-type: none;
                padding-left: 0;
                display: inline-flex;
                flex-direction: row;
                margin-bottom: 2px;
                justify-content: space-between;
                align-items: flex-end;
                flex-wrap: wrap;
            }

                .tabs ul li {
                    box-sizing: border-box;
                    flex: 1;
                    padding-right: 15px;
                    text-align: left;
                    /* width: 25%; */
                    /* font-size: 12px; */
                }

                    .tabs ul li label {
                        transition: all 0.3s ease-in-out;
                        color: #2f2f2f;
                        padding: 5px auto;
                        overflow: hidden;
                        text-overflow: ellipsis;
                        display: block;
                        cursor: pointer;
                        transition: all 0.05s ease-in-out;
                        white-space: nowrap;
                        -webkit-touch-callout: none;
                        -webkit-user-select: none;
                        -moz-user-select: none;
                        -ms-user-select: none;
                        user-select: none;
                    }

                        .tabs ul li label:hover, .tabs ul li label:focus, .tabs ul li label:active {
                            outline: 0;
                            color: #bec5cf;
                        }

            .tabs .slider {
                position: relative;
                width: 25%;
                transition: all 0.33s cubic-bezier(0.38, 0.8, 0.32, 1.07);
            }

                .tabs .slider .indicator {
                    position: relative;
                    width: 50px;
                    max-width: 100%;
                    margin: 0 auto;
                    height: 4px;
                    background: #428BFF;
                    border-radius: 1px;
                }

            .tabs .content {
                margin-top: 10px;
            }

                .tabs .content section {
                    display: none;
                    -webkit-animation-name: content;
                    animation-name: content;
                    -webkit-animation-direction: normal;
                    animation-direction: normal;
                    -webkit-animation-duration: 0.3s;
                    animation-duration: 0.3s;
                    -webkit-animation-timing-function: ease-in-out;
                    animation-timing-function: ease-in-out;
                    -webkit-animation-iteration-count: 1;
                    animation-iteration-count: 1;
                    line-height: 1.4;
                }

                    .tabs .content section h2 {
                        /*color: #428BFF;*/
                        display: none;
                    }

                        .tabs .content section h2::after {
                            content: "";
                            position: relative;
                            display: block;
                            width: 30px;
                            height: 3px;
                            /*background: #428BFF;*/
                            margin-top: 5px;
                            left: 1px;
                        }

            .tabs input[name="tab-control"]:nth-of-type(1):checked ~ ul > li:nth-child(1) > label {
                cursor: default;
                color: #474747;
                border-bottom: 2px solid black;
            }


            .tabs input[name="tab-control"]:nth-of-type(1):checked ~ .content > section:nth-child(1) {
                display: block;
            }

            .tabs input[name="tab-control"]:nth-of-type(2):checked ~ ul > li:nth-child(2) > label {
                cursor: default;
                /*color: #428BFF;*/
                border-bottom: 2px solid black;
            }

            .tabs input[name="tab-control"]:nth-of-type(2):checked ~ .content > section:nth-child(2) {
                display: block;
            }

            .tabs input[name="tab-control"]:nth-of-type(3):checked ~ ul > li:nth-child(3) > label {
                cursor: default;
                /*color: #428BFF;*/
                border-bottom: 2px solid black;
            }


            .tabs input[name="tab-control"]:nth-of-type(3):checked ~ .content > section:nth-child(3) {
                display: block;
            }

            .tabs input[name="tab-control"]:nth-of-type(4):checked ~ ul > li:nth-child(4) > label {
                cursor: default;
                /*color: #428BFF;*/
                border-bottom: 2px solid black;
            }

            .tabs input[name="tab-control"]:nth-of-type(4):checked ~ .content > section:nth-child(4) {
                display: block;
            }

            .tabs input[name="tab-control"]:nth-of-type(5):checked ~ ul > li:nth-child(5) > label {
                cursor: default;
                /*color: #428BFF;*/
                border-bottom: 2px solid black;
            }

            .tabs input[name="tab-control"]:nth-of-type(5):checked ~ .content > section:nth-child(5) {
                display: block;
            }
            
            .tabs input[name="tab-control"]:nth-of-type(6):checked ~ ul > li:nth-child(6) > label {
                cursor: default;
                /*color: #428BFF;*/
                border-bottom: 2px solid black;
            }

            .tabs input[name="tab-control"]:nth-of-type(6):checked ~ .content > section:nth-child(6) {
                display: block;
            }

        @-webkit-keyframes content {
            from {
                opacity: 0;
                -webkit-transform: translateY(5%);
                transform: translateY(5%);
            }

            to {
                opacity: 1;
                -webkit-transform: translateY(0%);
                transform: translateY(0%);
            }
        }

        @keyframes content {
            from {
                opacity: 0;
                -webkit-transform: translateY(5%);
                transform: translateY(5%);
            }

            to {
                opacity: 1;
                -webkit-transform: translateY(0%);
                transform: translateY(0%);
            }
        }

        iframe {
            border: medium none;
            width: 100%;
            height: 60vh;
            pointer-events: auto;
        }
    </style>
</asp:Content>

<asp:Content ID="actionPanel" ContentPlaceHolderID="ActionPanel" runat="server">
</asp:Content>

<asp:Content ID="pageContent" ContentPlaceHolderID="PageContent" runat="server">
    <div>
        <div class="tabs">
            <input type="radio" id="tab1" name="tab-control" checked>
            <input type="radio" id="tab2" name="tab-control">
            <input type="radio" id="tab3" name="tab-control">
            <input type="radio" id="tab4" name="tab-control">
            <input type="radio" id="tab5" name="tab-control">
            <%--<input type="radio" id="tab5" name="tab-control">--%>
            <ul>
                <li title="Contact Details">
                    <label for="tab1" role="button"><span>Contact Details</span></label></li>
                <li title="Identification Number">
                    <label for="tab2" role="button"><span>Identification Number</span></label></li>
                <li title="Education">
                    <label for="tab3" role="button"><span>Education</span></label></li>
                <li title="Sub Department">
                    <label for="tab4" role="button"><span>Sub Department</span></label></li>
                <li title="Image">
                    <label for="tab5" role="button"><span>Image</span></label></li>
                <%--<li title="Addresses">
                    <label for="tab6" role="button"><span>Addresses</span></label></li>--%>
            </ul>
            <div class="content">
                <section>
                    <iframe runat="server" id="logisticsElectronicAddress" src="#" />
                </section>
                <section>
                    <iframe runat="server" id="hcmPersonIdentificationNumber" src="#" />
                </section>
                <section>
                    <iframe runat="server" id="hcmPersonEducation" src="#" />
                </section>
                <section>
                    <iframe runat="server" id="hrSubDepartment" src="#" />
                </section>
                <section>
                    <iframe runat="server" id="hcmPersonImage" src="#" />
                </section>
                <%--<section>
                    <iframe runat="server" id="logisticsLocation" />
                </section>--%>
            </div>
        </div>
    </div>
</asp:Content>
