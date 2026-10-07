using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicsPortal
{
    public partial class WebForm1 : MainForm
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            pageMenuId = "Payroll";

            base.Page_Load(sender, e);

            if (!isUserAuthenticated)
                return;

            if (!IsPostBack)
            {
                gridDataBind();
            }

        }

        private void gridDataBind()
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(
                new DataColumn[] {
                    new DataColumn("ItemId", typeof(int)),
                    new DataColumn("ItemName", typeof(string)),
                    new DataColumn("Desc", typeof(string)),
                    new DataColumn("Stocker", typeof(string)),
                    new DataColumn("ItemQuantity",typeof(string)),
                    new DataColumn("WFStatus",typeof(string))
            });
            for (int i = 1; i < 55; i++)
            {
                if (i == 4)
                {
                    dt.Rows.Add(i, "Flour" + i, "Desc" + i, "Stocker" + i, "500 Kg", "Submited");
                }
                else
                    dt.Rows.Add(i, "Flour" + i, "Desc" + i, "Stocker" + i, "500 Kg", "Draft");
            }
            gv.DataSource = dt;
            gv.DataBind();

            GridView1.DataSource = dt;
            GridView1.DataBind();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            //PortalIntegration.PREarnings pREarnings = new PortalIntegration.PREarnings();


            //DataTable dataTable = pREarnings.ViewData();
            //DataSet dataTable = pREarnings.retrieveRecords();
            //long createResults = pREarnings.createRecord();
            //bool updateResults = pREarnings.updateRecord();
            //bool deleteResults = pREarnings.deleteRecord();
            //DataTable dataTable = pREarnings.retrieveWorkerDetails();

            //MemoryStream memoryStream = pREarnings.ViewReport();
            //MemoryStream memoryStream = pREarnings.RetrieveReport("USMF", "000004", "2018-03");
            ////data:application/vnd.ms-excel; base64,                       //data:application/pdf
            //string inputAsString = "UEsDBBQAAAAIAJtKJ01dm91yxwEAAFUEAAAPABwAeGwvd29ya2Jvb2sueG1sIKIYACigFAAAAAAAAAAAAAAAAAAAAAAAAAAAAJ1SXW+jMBD8K65V9a2BoHxUOaDSKSddX6qoidLHyoeXYNXYlm0K+fdd4EiI1IcoT8t47dnZYeLnppTkC6wTWiV0OgkpAZVpLtQhoZXPH58ocZ4pzqRWkNAjOEqe07jW9vOf1p8E3yuX0MJ7swoClxVQMjfRBhR2cm1L5hHaQ+CMBcZdAeBLGURhuAhKJhTtGVb2Gg6d5yKDtc6qEpTvSSxI5lG9K4RxA1vDr+LjltW46aBnJHHddwa+qzb8SZ0rmAW+Oxo4afvyt5JxnW2sNm5/wcdu2HTsfHaTVxkudnIpjXMhYd/HiDBjXlmJYWkkJZI5/4cLDzyhM4S6hosDW5nflZAtmIcLZApGVOecbSzhkLNK+h0qHPqY2GgWRd2z8800buteQO3OBC0kzbtQXNcJjWYY9OOAphGiugPvgvsCT57CxensL4hDgT9tGc7D8aSWEuFoWBedoRLVmbB52zLJ7HErhZmswYkDOt9deMGlp+jASuCHfeHdGl1nqEiFWwsFvDX0Ev2n/2ikKicbK5T/2Akv21xInTG5HUag5gdmtPv1g5K+cXc/Xd0v42DEnl4gnIyMGD7Slk53NF9G81Zw3xjZkn4DUEsDBAoAAAAAAJtKJ03mOic9QwIAAEMCAAALABwAX3JlbHMvLnJlbHMgohgAKKAUAAAAAAAAAAAAAAAAAAAAAAAAAAAA77u/PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0idXRmLTgiPz48UmVsYXRpb25zaGlwcyB4bWxucz0iaHR0cDovL3NjaGVtYXMub3BlbnhtbGZvcm1hdHMub3JnL3BhY2thZ2UvMjAwNi9yZWxhdGlvbnNoaXBzIj48UmVsYXRpb25zaGlwIFR5cGU9Imh0dHA6Ly9zY2hlbWFzLm9wZW54bWxmb3JtYXRzLm9yZy9vZmZpY2VEb2N1bWVudC8yMDA2L3JlbGF0aW9uc2hpcHMvb2ZmaWNlRG9jdW1lbnQiIFRhcmdldD0iL3hsL3dvcmtib29rLnhtbCIgSWQ9InJJZDIiIC8+PFJlbGF0aW9uc2hpcCBUeXBlPSJodHRwOi8vc2NoZW1hcy5vcGVueG1sZm9ybWF0cy5vcmcvcGFja2FnZS8yMDA2L3JlbGF0aW9uc2hpcHMvbWVhdGFkYXRhL2NvcmUtcHJvcGVydGllcyIgVGFyZ2V0PSIvZG9jUHJvcHMvY29yZS54bWwiIElkPSJySWQ0IiAvPjxSZWxhdGlvbnNoaXAgVHlwZT0iaHR0cDovL3NjaGVtYXMub3BlbnhtbGZvcm1hdHMub3JnL29mZmljZURvY3VtZW50LzIwMDYvcmVsYXRpb25zaGlwcy9leHRlbmRlZC1wcm9wZXJ0aWVzIiBUYXJnZXQ9Ii9kb2NQcm9wcy9hcHAueG1sIiBJZD0icklkNSIgLz48L1JlbGF0aW9uc2hpcHM+UEsDBBQAAAAIAJtKJ01z9d9oWgUAAIs6AAANABwAeGwvc3R5bGVzLnhtbCCiGAAooBQAAAAAAAAAAAAAAAAAAAAAAAAAAADlW/Fv4jYU/leidJo2aS2BUI7cAaeNCumk7XZau2nStB9M4gRrSZw5pgf96+dnJyR0DTYtVQkQldjO8/e+z352TOKOPq6S2LrHLCc0HdvdK8e2cOrTgKTR2F7y8HJoWzlHaYBimuKxvca5bX2cjHK+jvHtAmNuCYQ0H9sLzrP3nU7uL3CC8iua4VRcCSlLEBdZFnXyjGEU5FApiTs9xxl0EkRSWyG8ZyYYNAyJj2+ov0xwyhUIwzHign++IFleoq0CI7yAoa9Ca8mnRvFGXSnxjBQ+xS5fIIaDu3WGN9zu+XPBAup/YTTL/9jCQ89QWm95/1lt5Qthm1aajNJlMkt4bvl0mQp9/U2RpU6fgrE97NmWQpzSQETTX99cOl2n73h/J52gsxYfUauj7J+q7jZVv/jBuXKcD9+p8/cfvv13SXnxvRuy3wx5cSFBL8vUbqBrQ251lCKRT0YhTau2e2erAjHMHqx7FIuB2YVq+cNk5NOYMotF87E9mznyA5dksaCFEqxqTFFM5oxIb6JQAKKExGt1sQfFqkD4gB4vaiUkpUy6koVgJXmo763MXOTFH1EVJQkC8wIj/+BamSqYjJaqLBWzCBQva9qcfbXd4ohi6/dPlbhtZjX3h2HYOzTDQzEbHnvTHQvB12IlTzB+SRxvzX1QMBlliHPM0pnIWEUaZu6qqWoGACZPmqoRQ+tu7/qZtXMakwD4RdO69qEDh5wW1AXR1E0W89LigP7VZ5f/ymK3f3kSPTKnLBDrmrJPuq5dlk1GMQ6h79SJkWgBueLMaSYy8ntOOacJeCwSAUERTRF4qSVLVEN0S66dxjZfiDuwLgTrRMqKAV3OY6yvuj/r/agdXODRtPQBiOzn+0w571fxreQdoJ0PM7COhkhLOrz1nNs/Wl6b82sMrNfgXCTEusTHcXwLDv4MRW4V1n5DOvATMt0kxTqmSKrKKiOwViH438KBnEiUqx0XlqACG2VZvJ5RuQCyi5yArXI/SWSZf5JHV8ejdPJ5mcwxm8lfwDVnZq5l/seYRCk86mhm09vBBpXVrQVl5EHUgIV7hFPMUGzDMy5OfCgSPW1bHK/4b5TL50YS7CtD2Z0olM7hIRBJo19LgkLrBv4tdLt76/ZFAWYtl90UfN0t2Y8k1hW9tYD+mcbr9d66YWZuo2j1RPPcVD8d4r266N6Jh3iTbrdRt1xutFx104zstnBGNuy20w7X/kmHa9OM3Dw5tXVGVq/Gdqs+7ampSfX1KY5sk+4+7aHddCcatOVO1CTgXVsENMXd8CwnV+8sR1vXaUu0NipozcOERgW9tihoWoQ2zxhtvkX39cLPc9Lw2h6vJ7mkNNDdHK4t1m0yTps7/BTGaU21a/bc8jxVn16Qn3p3q025Zye7FuWD44nyTvGOuvYKvNqdZ9dKrVVYqICtlmP7MzRRbNffedfTAjBYVa++5ZtpKJiMOJrHeNuRAA1wiJYxv9tcHNtV+hcckGXibay+kHvKC6sq/TNERhceNXRqPoqtAuJM0gCvcDAtsiya17YyOk61iaC88n+barvjLhs9jrLS2YCVno+esx5HIen56HzduHDstqm2rL6snY+tfYYG7aOs9Lp07TM0wBka4DgGfTGVx8tj3hMfXRt6nusOBvo+nU41nKf6/hoM4E/nS98+gKLnA4xe3u8msWoS86bxrI9Ds5jXt6HJGNS3oUm/g42uvwDF83SxqucDKPrY0I8LYKzjAyNHh+O6EIl6Xfo508TG8/Q2MAp143Qw0PbFAA5d/OjnFtf1PJ0NWOk4u67eBuY6Exs9Z2Ctt3Ef3Zc7j9ZDau8kJKp/I538B1BLAwQUAAAACACbSidNRHFq8MAAAACjAQAAGgAcAHhsL19yZWxzL3dvcmtib29rLnhtbC5yZWxzIKIYACigFAAAAAAAAAAAAAAAAAAAAAAAAAAAALWQSwoCMQyGr1KydzIqiIjVjRu34gVKJ/PAmbY08XU2Fx7JK1hFxAEXblyF/Ek+PnK7XOfLU9eqA0VuvNMwzHJQ5KwvGldp2Es5mMJyMd9QayRtcN0EVunEsYZaJMwQ2dbUGc58IJcmpY+dkdTGCoOxO1MRjvJ8gvGTAX2m2p4D/UL0ZdlYWnm778jJFzCynFtiUFsTKxINeGpfWZZYoNaFhrguxqDwbw5HH3dcE0lf4x0ny0cZ9owmTyPsvXpxB1BLAwQUAAAACACbSidNjUyrBNAAAABsAQAAEQAcAGRvY1Byb3BzL2NvcmUueG1sIKIYACigFAAAAAAAAAAAAAAAAAAAAAAAAAAAAG2Q3WrDMAxGX8XoPlGywhghSV+gg8JudmtsNTWLf7DUpX37uWHLBuul+I4O0tfvr35Wn5TZxTBAWzegKJhoXZgGuMipegHFooPVcww0wI0Y1H7sTepMzHTMMVEWR6yKKHBn0gBnkdQhsjmT11wXIpTwFLPXUsY8YdLmQ0+ET03zjJ5EWy0a78IqbUb4VlqzKdMlz6vAGqSZPAVhbOsWf1mh7Pnhwpr8Ib2TW6KH6E+40Vd2G7gsS73sVrTc3+L76+FtfbVy4V6VIRh7/FfQ+AVQSwMEFAAAAAgAm0onTYTFRkX4AQAARAUAABAAHABkb2NQcm9wcy9hcHAueG1sIKIYACigFAAAAAAAAAAAAAAAAAAAAAAAAAAAAJ1UTY/aMBD9K1HuSwKqqgqZrCqoxKG0K0F3z649Sax1bMseKPTXd2JDCFSVKnJ6b2b8PF8Oez52OjuAD8qaRT6dlHkGRlipTLPI91g/fcqzgNxIrq2BRX6CkGfPFXvx1oFHBSEjBRMWeYvo5kURRAsdDxNyG/LU1nccifqmsHWtBKys2HdgsJiV5ccCjghGgnxyg2CeFOf+UU0PmiPVE1rlBrWj/C896fkvqr3TSSo4D1yGFgBXyXPRe7ji0HIPcndy10oP+KiYtKKfRHi90eMPVNpxZS7nxUO9ElTY0CXaEN5AqEpWJMDerJeRJ8CWFM4F0ur1xhFjX5VJJxMgJc8bz117lhsY22olU+gZsW8WkyEBtrPI9U510NuuhK2VlGCu528422yWmnand1wg2wquYUnNjtcNhK1pQajiF6763A84P4BA67OfPEA/lUV+4F5xg/SQ1G+iszyFJWvE2gX0FTXmPa5aYMVgjHAcO8bqQzWNAQRuA4shEcK3Ke4Uagjfa2ok/iPjePUl3zK/k7sToCm9hx9uZ1cc4Ty2kWE06DeF7dZxAXcjH9nZNj4P2vLY5oGwNSXmda9M50wDMk7tLyP77JxWIr7/aqOEt8HWmH05CtCsGDv7yNf046ums0lJXwy42BhduwWx9wpP/V1jSls4/K6qP1BLAwQUAAAACACbSidNbAge3vwGAAC1IwAAGAAcAHhsL3dvcmtzaGVldHMvc2hlZXQxLnhtbCCiGAAooBQAAAAAAAAAAAAAAAAAAAAAAAAAAACtWltz2joQ/isans55KNjy3UPoACGBOWlP5tDLs2uL4KmxGFmQ0F9/Vrah2JUlYNKHxnh3P+1+u15pMcOPb5sM7QkrUprf9cy+0UMkj2mS5i93vR1fffB7qOBRnkQZzcld70CKHvo4Gr5S9rNYE8IRAOTFXW/N+TYcDIp4TTZR0adbkoNkRdkm4vCRvQyKLSNRUhptsgE2DHewidK8VyGE7BIMulqlMbmn8W5Dcl6BMJJFHNwv1um2OKK9JRfhJSx6hVCP/py5eF9JjngXRSjzrlhHjCRfDlty8m3PbwVLaPzM6Lb41sCLboj0nPn4Jq5iCOzE0mhYkvbMRkO641maE3E5OL8+KZQX31LyWpxdI1FQPyj9KT4skrseFGKxpq+PLE2eAKIQd0bDbZQTdFhusxQ49MrS5FCVK0Z/EYiH0+0TWfEpybK73tgHg4GwOK4usM+vwYGYZtX/aJOKB6CHNtFb+TfeFZxuvqcJX5efX6srbPQ9z7ItzxHoYHhmjWtr3Gnt9B3DCgwXS6yt2trqtDbdfuAbtuvLFrdrc7vT3Oj7julI13ZqY6fT2Op7VtfSbm3tdlp7fde0sG/K1vZqa0/BmmdYUlu/tvU7be0+DnAg9zuorYPubOO+YXeZm8axWgwF5wZuVsugKrmqyUQ8Gg0ZfUWsNFpDVVvQhE9pmJP0Rdw0hTnonZRxpez3La2qVamaVl8KG6OiXBs+prl4Wpec9YTd2AJpCq6WPT4stlEMjxp0yIKwPemNpjTntKBolnPCOPQT0afQ8lBwskFfl+PhgIMnAmAQt1yyj97b+kid2n2vQ7n0H0v8nzpq/5dRFrEDWmbpVuGqWy3vXJIUr9LF/aDbU6v0tDhy7PWqNSWyiUI2VcjuFbKZQvagkD0qZHOFbHGUNXjy65T2PS2lQa3qtzWNI6G2rHQDdepnm21GD4SEzcSXeI4Eb6LBM8Q/WwImc+5eA3YhzEwD8xwd0DNhKU0ujXKhQcSG6X8wrL+sgTkor62BVV39rXiARHO8JYWmofbmc7S5PH86sKfdT4KeSC7ONJemUYd5aR51OCKRj4zuthfnUYf4SHLCokyVNPPGpJmanivOa+U+AVufNH1unTABtB/ZlhkYw8FemwvNuhfn4noceQauwmlSX2/tXh9fyT1+F98nN+DIc/I+OLN3imtxHU4zJ5b2cfCkKdEcoWYRy2F8KiQh+NLUaPAkOEHz3FAinISiN0sSd/0qsuhnOpx7kuzicnLX+v3Q9LspfFQJ562IZSfdxXURN0vD1paGKTudjsFQueYkKtIYVYdUCT+iJMuSsMtG6YgTSLNRtjNvN3iQP7Ian2R+yIKb6YBm/04WFyTdViVdIZy3gnXqNFdcmdZvppq5dG7NpWbUmNNdAc+5Ko1O5Rp22mk03ebp3nR6evo1/jwzuk8TsQ0/7PJE5taRscot23SdDsrcWylz1S5+oj/SjKgYc0vXsJYvt8GX15yHWtKm7eIkbcbs3Rqzp475C4vyYkulR9BT2N7pedfE7Snj9pRxS8c209fHLdu2xqavjvsrT7OUH9A4y+hrlMfStAd1/H5H/Nho9Tv/PEJstthpSnGLnabUaLUllfRRKZ0rvVqcpE3e9UMwtqS8awY6mCqKonuLwXZNeVBS7mHvD86dFudBIzpXusdcP/5i+fFCA/SF8ihDykNGy/+Hlv9OK7Mq6bwl9euMVtzZOOhooFg/HuNAllx8/fhpSY977wU00wF9JhyNN3SXc9ngZ5kVYyXKfuT6ttFFmX447aDs+umug7J3AprpgMbJXvRDFW24pq0amDtONhjfStn1w1cHZe8ENNMBPUeH8guGCyoNaytNP/dJnRxjzUBROYfSXLz3Sgqpl7KWPtEBo2X6BjvpTHiJvqzh3BnlCXqgO4bmcNRjBD6ke4LQ8z//KaYaXE81tvydQaU7OHuHsSHshYj3bsXZNWJkJbahcFYane7/qWKFD5ZaZeqEj45aZRKE00CtMgvCuUZlYhrhtPwGTYUDSnOd0sQ0AUkT+gyU5jqliYkBCeuQMCBplCamFdZfAqiQrHCuU4LJM6yHym4lmCjD+qylWs4O5zokmJBgOU0FwOADSLoyMV1AcnVKHih5OiU/nOqig+NTWB+MVI5DYeqUYMsPp1hXmaA01ynBTghIusoEpblOCTYIQNJVJijNdUrQ2sIFbhfd7+tCvP1/IZ8i9pLmBcrIqmxGiB3bknj7X/79QTn0q+qFKokSUr1bXVHKq8uR+FXACamCXRK+2yLKUtg6yl+TQFOFQYxFKQcUuP+LgiC736ai5UPP3xPG0/jsTo1aAo2G1cIP5ZooytKX/HvK1/WS1fYxONcZDeufVyAWpsldjy2Ssvjqu3B1+sXN6H9QSwMEFAAAAAgAm0onTQWHSMSgAQAAhAQAABgAHAB4bC9kcmF3aW5ncy9kcmF3aW5nMi54bWwgohgAKKAUAAAAAAAAAAAAAAAAAAAAAAAAAAAAnVTbahsxEP0VofdG64SaZrEdQk1LobSmlL4r2tmsQDdG8jr5+46slZ0NgbR+O2cu5+yMpF3dPVnDRsCovVvzxVXDGTjlO+0e13yf+g+fOItJuk4a72DNnyFydrdZPXXYHuIWGfW72BJd8yGl0AoR1QBWxisfwFG292hlIoqPokN5IGVrxHXTLEUMCLKLA0Dalgyf9P5Jzfe9VrD1am/BpSKJYGSiWeKgQ6xq8VK1OEiE7vdzgJPWmC4V67zaoQ/xz0xPXrA3K7Wr/eqizSsa7LTzcprp4D+DMfdODR5LqEdvC1LebJqVqLDGfvb9i3Bmxwz6Qw1nWGMvqidWyNkm+Xftbm5vPi5u/8uTWprl8g3jahe0KsCNO02HNBn+GHfIdEfPgjMnLd1+yqY9AlvwSeFYcy4vzeINOlN+MDp80YYGk0fMsAX7AOSE3zp6cIpeXCK7gNqlbFXKcnlMCEkNGfak8AtUOuZn5FQk5m6ZxZC/QrYBY/oK3rIMyJh6eY7L8XssKmdUayfBIiFmm1NG00XfyiTr/K8Dr+6XqH+QzV9QSwMEFAAAAAgAm0onTeeQLFSuAAAAHQEAACMAHAB4bC93b3Jrc2hlZXRzL19yZWxzL3NoZWV0MS54bWwucmVscyCiGAAooBQAAAAAAAAAAAAAAAAAAAAAAAAAAACNzzEOwjAMBdCrRN6pSwdAqGkXlq6oF4hSN41okyhJoZyNgSNxBbKAQGJgtL/9ZD9u97JeppGdyQdtDYd1lgMjI22njeIwx361g7oqjzSKmCbCoF1gacUEDkOMbo8Y5ECTCJl1ZFLSWz+JmEqv0Al5EoqwyPMN+k8Dvk3WXh39I9q+15IOVs4TmfgDxs6LSzoeWCu8osgBl/HVfKdFllxgTcfBN90WGFYlfj1ZPQFQSwMECgAAAAAAm0onTdPTyicrAAAAKwAAABMAHAB4bC9tZWRpYS9pbWFnZTMuZ2lmIKIYACigFAAAAAAAAAAAAAAAAAAAAAAAAAAAAEdJRjg5YQEAAQDwAADb3+8AAAAh+QQBAAAAACwAAAAAAQABAAACAkQBADtQSwMEFAAAAAgAm0onTVFMRJitAAAAFgEAACMAHAB4bC9kcmF3aW5ncy9fcmVscy9kcmF3aW5nMi54bWwucmVscyCiGAAooBQAAAAAAAAAAAAAAAAAAAAAAAAAAACNj00KwjAUhK8S3t6+qiBFmrpx41Z6gZC+pMHmhySVejYXHskrGHCj4MLlMDPfMM/7oz0sdmJXisl4x2Fd1cDIST8YpznMWa0aOHTtmSaRSyKNJiRWKi5xGHMOe8QkR7IiVT6QK47y0YpcZNQYhLwITbip6x3GTwZ8M1l/C/QP0StlJB29nC25/AOMxpZFYL2ImjIHXCa0NBjxNraVNgrYaeAQT0MDDLsWv951L1BLAwQUAAAACACbSidNOJ3BajkBAAD0AwAAEwAcAFtDb250ZW50X1R5cGVzXS54bWwgohgAKKAUAAAAAAAAAAAAAAAAAAAAAAAAAAAArZM5bsMwEEWvIrANJNougiCw7CJJm7jIBQhyJBHmBg7l5WwpcqRcISN6KQwjsWFX3Ob/N/wEf76+p/ONNcUKImrvajauRqwAJ73Srq1Zn5ryic1n089tACyo1GHNupTCM+coO7ACKx/A0UnjoxWJlrHlQcilaIFPRqNHLr1L4FKZBg82m75CI3qTircNbe+wJGfFy65uQNVMhGC0FImO+cqpE0jpm0ZLUF72liQVhghCYQeQrKnyWFmh3UM25meZEQxeB93fqiJlrsFOB/wL0ermhKDtEEveJ8kH5R61gmIhYnoXlgr4xnBMWwNY3TmUbHrs9hyalIvoA9KLRbgef4hnUJeBjCAmfSmSvG++MAzJK1CXwinqtY/LnBDyPIzvnPrR/79GVBRr+nJ4mExubmRvdATz/Idnv1BLAQItABQAAAAIAJtKJ01dm91yxwEAAFUEAAAPAAAAAAAAAAAAAAAAAAAAAAB4bC93b3JrYm9vay54bWxQSwECLQAKAAAAAACbSidN5jonPUMCAABDAgAACwAAAAAAAAAAAAAAAAAQAgAAX3JlbHMvLnJlbHNQSwECLQAUAAAACACbSidNc/XfaFoFAACLOgAADQAAAAAAAAAAAAAAAACYBAAAeGwvc3R5bGVzLnhtbFBLAQItABQAAAAIAJtKJ01EcWrwwAAAAKMBAAAaAAAAAAAAAAAAAAAAADkKAAB4bC9fcmVscy93b3JrYm9vay54bWwucmVsc1BLAQItABQAAAAIAJtKJ02NTKsE0AAAAGwBAAARAAAAAAAAAAAAAAAAAE0LAABkb2NQcm9wcy9jb3JlLnhtbFBLAQItABQAAAAIAJtKJ02ExUZF+AEAAEQFAAAQAAAAAAAAAAAAAAAAAGgMAABkb2NQcm9wcy9hcHAueG1sUEsBAi0AFAAAAAgAm0onTWwIHt78BgAAtSMAABgAAAAAAAAAAAAAAAAAqg4AAHhsL3dvcmtzaGVldHMvc2hlZXQxLnhtbFBLAQItABQAAAAIAJtKJ00Fh0jEoAEAAIQEAAAYAAAAAAAAAAAAAAAAAPgVAAB4bC9kcmF3aW5ncy9kcmF3aW5nMi54bWxQSwECLQAUAAAACACbSidN55AsVK4AAAAdAQAAIwAAAAAAAAAAAAAAAADqFwAAeGwvd29ya3NoZWV0cy9fcmVscy9zaGVldDEueG1sLnJlbHNQSwECLQAKAAAAAACbSidN09PKJysAAAArAAAAEwAAAAAAAAAAAAAAAAD1GAAAeGwvbWVkaWEvaW1hZ2UzLmdpZlBLAQItABQAAAAIAJtKJ01RTESYrQAAABYBAAAjAAAAAAAAAAAAAAAAAG0ZAAB4bC9kcmF3aW5ncy9fcmVscy9kcmF3aW5nMi54bWwucmVsc1BLAQItABQAAAAIAJtKJ004ncFqOQEAAPQDAAATAAAAAAAAAAAAAAAAAHcaAABbQ29udGVudF9UeXBlc10ueG1sUEsFBgAAAAAMAAwAJgMAAP0bAAAAAA==";
            // "data:application/ms-excel;base64, " + Convert.ToBase64String(memoryStream.ToArray());


            //string src = "data:application/vnd.ms-excel;base64, " + inputAsString;
            //embed01.Attributes.Add("src", src);
            //iframe01.Attributes.Add("src", src);


            //convertExcelToHTML(inputAsString, "");
            //Response.Clear();
            //Response.Buffer = true;
            //Response.ContentType = "application/vnd.ms-excel";
            //Response.AddHeader("content-disposition", "attachment;filename=SalarySlip.xls");
            //Response.Charset = "";
            ////this.EnableViewState = false;
            ////byte[] bytes = memoryStream.ToArray();
            //Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
            //Response.BinaryWrite(Convert.FromBase64String(inputAsString));  //Response.OutputStream.Write(data, 0, data.Length);
            //                                                                //Response.Write(Convert.FromBase64String(inputAsString));

            //Response.End();


            //byte[] rdlBytes = Encoding.UTF8.GetBytes(strRdl);
            //System.IO.MemoryStream stream = new MemoryStream(rdlBytes);
            //ReportViewer1.LocalReport.LoadReportDefinition(memoryStream);
            //ReportViewer1.DataBind();
            //////embed01.DataBind();

        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            //PortalIntegration.ESSWorkflow eSSWorkflow = new PortalIntegration.ESSWorkflow();
            //bool results = eSSWorkflow.retrieveRecords();


        }

        protected void gv_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            //PortalIntegration.OnBehalfof onBehalfof = new PortalIntegration.OnBehalfof();
            //onBehalfof.retrieveRecords("000001");
        }

        protected void gv_RowEditing(object sender, GridViewEditEventArgs e)
        {
            //GridView1.EditIndex = e.NewEditIndex;
            gv.EditIndex = e.NewEditIndex;
            gridDataBind();
        }

        protected void gv_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {

        }

        protected void gv_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridView gridView = sender as GridView;
            GridViewRow gridViewRow = null;
            //ControlCollection controls = null;
            int gridControls = -1;
            int gridSubControls = -1;

            gridViewRow = e.Row as GridViewRow;
            if (gridViewRow.RowType == DataControlRowType.DataRow)
            {

                //gridViewRow.Attributes["NonActionable"]
                gridControls = gridViewRow.Controls.Count - 1;
                if (gridControls > 0)
                {
                    gridSubControls = gridViewRow.Controls[gridControls].Controls.Count;
                    foreach (DataControlFieldCell control in gridViewRow.Controls)
                    {
                        var HeaderText = control.ContainingField.HeaderText;
                        if (HeaderText == "WFStatus")
                        {
                            string value = control.Text;
                            if (value == "Submited")
                            {
                                GridViewRow parent = control.Parent as System.Web.UI.WebControls.GridViewRow;
                                parent.Enabled = false;
                                parent.Attributes.Add("NonActionable", "true");
                            }
                            //((System.Web.UI.WebControls.DataControlFieldCell)(gridViewRow.Controls[7])).ContainingField.HeaderText
                            //string lid = ((Label)GridView1.Rows[e.RowIndex].FindControl("lblLoginId")).Text;
                        }
                    }

                }

                if (e.Row.RowState == DataControlRowState.Edit)
                {

                }
            }

        }


        //public void convertExcelToHTML(string Source, string Target)
        //{
        //    if (MSExcel == null)
        //        MSExcel = new Excel.ApplicationClass();
        //    var unknown = System.Reflection.Missing.Value;
        //    try
        //    {
        //        MSExcel.Visible = false;
        //        MSExcel.Application.Visible = false;
        //        MSExcel.WindowState = Excel.XlWindowState.xlMinimized;

        //        MSExcel.Workbooks.Open(Source, unknown,
        //             unknown, unknown, unknown,
        //             unknown, unknown, unknown,
        //             unknown, unknown, unknown,
        //             unknown, unknown, unknown, unknown);
        //        var sheet = (Excel.Worksheet)MSExcel.Worksheets[1];
        //        object format = Excel.XlFileFormat.xlHtml;

        //        MSExcel.Workbooks[1].SaveAs(Target, format,
        //                unknown, unknown, unknown,
        //                unknown, Excel.XlSaveAsAccessMode.xlExclusive, unknown,
        //                unknown, unknown, unknown,
        //                unknown);
        //    }
        //    catch (Exception e)
        //    {
        //    }
        //    finally
        //    {
        //        if (MSExcel != null)
        //        {
        //            MSExcel.Workbooks.Close();
        //            MSExcel.Quit();
        //        }
        //    }
        //}



    }
}