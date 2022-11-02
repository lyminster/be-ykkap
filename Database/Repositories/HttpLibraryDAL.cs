using Database.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;



namespace Database.Repositories
{
    public class HttpLibraryDAL
    {
        public HttpLibraryDAL()
        {
        }
        public HttpResponseMessage SentRequest(String JsonData, String URL, String Method, Boolean isAuthorize, String Token, out string ResultJson)
        {
            ResultJson = "";


            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod(Method), URL))
                {
                    try
                    {
                        if (isAuthorize)
                        {
                            request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + Token);

                        }


                        request.Content = new StringContent(JsonData);
                        request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                        var response = httpClient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
                        string result = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                        ResultJson = result;
                        return response;



                    }
                    catch (Exception ex)
                    {
                        return null;
                        //throw ex;
                    }
                }



            }
        }


        public void ExampleMethodRequest()
        {
            try
            {
                JsonApproval jsonApproval = new JsonApproval();


                //1 = approve 2= reject 3= revised
                jsonApproval.DocNo = "DOCNOEXAMPLE";
                //nama project
                jsonApproval.IDClient = "Papperless";
                jsonApproval.Username = "testing";
                //doc no yg di generate di tiap sistem
                jsonApproval.DocNo = "DOCNOEXAMPLE";
                //Id document yg di submit
                jsonApproval.ID = "3bf7984f-4ee5-4cb4-bc96-589cb1357b8b";
                jsonApproval.DocTypeName = "Paper less Archiving";
                //requestor email
                jsonApproval.SenderEmail = "wenpy_natawijaya@yahoo.com";
                //user id atau employee ID tergantung di table masing2
                jsonApproval.SenderID = "EMP0001";
                //nama requestor 
                jsonApproval.SenderName = "Wen";
                //position requostor jika pakai position
                jsonApproval.SenderPositionID = "POS001";
                //postion name
                jsonApproval.SenderPositionName = "Karyawan Derik";

                List<JsonApprovalDetail> ListApprover = new List<JsonApprovalDetail>();
                ListApprover.Add(new JsonApprovalDetail
                {
                    //yg wajib di isi yg ini aja ya 
                    levelApproval = 1,
                    approvalEmail = "wenpynatawijaya@gmail.com",
                    approvalID = "EMP0002",
                    approvalName = "Approval level 1",
                    approvalPositionID = "POSAPP001",
                    approvalPositionName = "Bos Derik",

                });
                ListApprover.Add(new JsonApprovalDetail
                {
                    //yg wajib di isi yg ini aja ya 
                    levelApproval = 2,
                    approvalEmail = "derikwildy@gmail.com",
                    approvalID = "EMP0003",
                    approvalName = "Approval level 2",
                    approvalPositionID = "POSAPP002",
                    approvalPositionName = "Bos Derik 2",

                });
                ListApprover.Add(new JsonApprovalDetail
                {
                    //yg wajib di isi yg ini aja ya 
                    levelApproval = 3,
                    approvalEmail = "natawijaya.aaron@gmail.com",
                    approvalID = "EMP0004",
                    approvalName = "Approval level 3",
                    approvalPositionID = "POSAPP001",
                    approvalPositionName = "Bos Derik 3",

                });

                jsonApproval.ListApprovalDetail = ListApprover;
                //ambil dari Webconfig baiknya
                String URL = "https://localhost:44344/SubmitDocument";

                string JsonData = JsonConvert.SerializeObject(jsonApproval);
                string result = "";
                var httpstatus = SentRequest(JsonData, URL, "POST", false, null, out result);
                if (httpstatus.StatusCode == HttpStatusCode.OK)
                {
                    //ini udah dpt respon sukses tapi tetap harus baca result nya

                    if (result.ToUpper() == "OK")
                    {
                        //ini udah oke then ngapain terserah kalian
                    }


                }
                else
                {
                    //ini gagal artinya


                }


            }
            catch (Exception)
            {
                //return null;
                //throw;
            }
        }
    }
}
