using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using Newtonsoft.Json;

namespace WebApplicationBrookeAndCo.Classes
{
    public class Gmail
    {
        public string sendEmail(string receiver, int orderId, double subtotal, double GST, double QST, double total, double delivFee, string delivDate, DeliveryAdress delivAddress)
        {   
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress("lachapelle.audrey.1989@gmail.com");
            msg.To.Add(receiver);
            msg.Subject = "BROOKE AND CO > Confirmation notice for order no : " + orderId;
            msg.IsBodyHtml = true;
            msg.Body =
              "<table width='700px' style='background-color:#f2f2f2;border:1.5px solid teal;font-size:20px;'>" +
                "<thead>" +
                    "<tr style='background-color: teal;'>" +
                        "<th colspan='2' height='90px' style='text-align:center; vertical-align:central'><span style='font-size:25pt; color:white; font-weight:bold;'>ORDER CONFIRMATION</span></th>" +
                    "</tr>" +
                "</thead>" +
                "<tbody>" +
                    "<tr><td width='50%' style='padding-left:15px; padding-top:10px'>Order Id :</td><td>"+orderId+"</td></tr>" +
                    "<tr><td width='50%' style='padding-left:15px; padding-top:10px'>Subtotal :</td><td>"+subtotal+" $</td></tr>" +
                    "<tr><td width='50%' style='padding-left:15px; padding-top:10px'>Delivery fee :</td><td>"+delivFee+" $</td></tr>" +
                    "<tr><td width='50%' style='padding-left:15px; padding-top:10px'>GTS :</td><td>"+GST+" $</td></tr>" +
                    "<tr><td width='50%' style='padding-left:15px; padding-top:10px'>QST :</td><td>" + QST + " $</td></tr>" +
                    "<tr><td width='50%' style='padding-left:15px; padding-top:10px'>Total :</td><td>" + total + " $</td></tr>" +
                    "<tr><td width='50%' style='padding-left:15px; padding-top:10px'>Expected delivery date :</td><td>" + delivDate + "</td></tr>" +
                    "<tr><td width='50%' style='padding-left:15px; padding-top:10px'>Delivery Address :</td><td>" + delivAddress.civicnumber + "-"+ delivAddress.appartment + " " + delivAddress.street +" "+ delivAddress.city + ", " + delivAddress.province +", " + delivAddress.country + ", " + delivAddress.zipcode + "</td></tr>" +
                    "<tr><td td colspan='2'></td></tr>" +
                "</tbody>" +
                "<tfoot>" +
                    "<tr><td style='padding-top:20px;text-align:center;'  td colspan='2'>Thank you for your purchase!</td></tr>" +
                    "<tr><td td colspan='2' style='text-align:center;'>For any question about your order, call us at : 1-800-123-4567</td></tr>" +
                "</tfoot>" +
            "</table>";

            SmtpClient client = new SmtpClient();
            client.UseDefaultCredentials = true;
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Credentials = new NetworkCredential("lachapelle.audrey.1989@gmail.com", "PASSWORD");
            client.Timeout = 20000;
            try
            {
                client.Send(msg);
                return "Mail has been successfully sent!";
            }
            catch (Exception ex)
            {
                return "Fail Has error" + ex.Message;
            }
            finally
            {
                msg.Dispose();
            }
        }
    }
}
