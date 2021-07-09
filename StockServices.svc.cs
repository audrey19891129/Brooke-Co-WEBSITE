using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using WebApplicationBrookeAndCo.Classes;

namespace WebApplicationBrookeAndCo
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class StockServices
    {
        // Pour utiliser HTTP GET, ajoutez l'attribut [WebGet]. (ResponseFormat par défaut=WebMessageFormat.Json)
        // Pour créer une opération qui renvoie du code XML,
        //     ajoutez [WebGet(ResponseFormat=WebMessageFormat.Xml)],
        //     et incluez la ligne suivante dans le corps de l'opération :
        //         WebOperationContext.Current.OutgoingResponse.ContentType = "text/xml";

        [OperationContract, WebGet(ResponseFormat = WebMessageFormat.Json)]
        public List<Product> Get_ListProduct()
        {
            try
            {
                List<Product> list = Product.GetProducts();
                return list;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        [OperationContract, WebInvoke(
              Method = "POST",
              BodyStyle = WebMessageBodyStyle.WrappedRequest,
              RequestFormat = WebMessageFormat.Json,
              ResponseFormat = WebMessageFormat.Json
          )]
        public string Post_AddBook(string pcode, string category, double price, string picture, string title,
            string genre, int warehousecode, int minimum, string authors, string pubCo, string pubDate)
        {
            try
            {
                Book bk = new Book(pcode, "book" ,category, price, picture, title, genre,
                    warehousecode, minimum, authors, pubCo, pubDate);
                bk.Saving();
                return "The new Book is Added, but state => INACTIVE <= until you make a purchase of the Product ! >> GO In Section Inventory => [INACTIVE] << ";
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        [OperationContract, WebGet(ResponseFormat = WebMessageFormat.Json)]
        public List<Book> Get_ListBook()
        {
            try
            {
                List<Book> list = Book.GetBooks();
                return list;
            }
            catch(Exception e)
            {
                return null;
            }
           
        }

        [OperationContract, WebGet(ResponseFormat = WebMessageFormat.Json)]
        public List<Book> Get_TopTenBooks()
        {
            try
            {
                List<Book> list = Book.GetTopTenBooks();
                return list;
            }
            catch (Exception e)
            {
                return null;
            }

        }

        [OperationContract, WebGet(ResponseFormat = WebMessageFormat.Json)]
        public List<Movie> Get_ListMovie()
        {
            try
            {
                List<Movie> list = Movie.GetMovies();
                return list;
            }
            catch(Exception e)
            {
                return null;
            }
        }

        [OperationContract, WebGet(ResponseFormat = WebMessageFormat.Json)]
        public List<Movie> Get_TopTenMovies()
        {
            try
            {
                List<Movie> list = Movie.GetTopTenMovies();
                return list;
            }
            catch (Exception e)
            {
                return null;
            }

        }

        [OperationContract, WebInvoke(
             Method = "POST",
             BodyStyle = WebMessageBodyStyle.WrappedRequest,
             RequestFormat = WebMessageFormat.Json,
             ResponseFormat = WebMessageFormat.Json
         )]
        public string Post_AddMovie(string pcode, string category, double price, string picture, string title,
            string genre, int warehousecode, int minimum, string director, string actors, string relyear)
        {
            try
            {
                Movie mv = new Movie(pcode, "movie", category, price, picture, title, genre, warehousecode, minimum, director, actors, relyear);
                mv.Saving();
                return "The new Movie is Added, but state => INACTIVE <= until you make a purchase of the Product ! >> GO In Section Inventory => [INACTIVE] << ";
            }
            catch(Exception e)
            {
                return e.Message;
            }
        }

        [OperationContract, WebGet(ResponseFormat = WebMessageFormat.Json)]
        public List<Game> Get_ListGame()
        {
            try
            {
                List<Game> list = Game.GetGames();
                return list;
            }
            catch(Exception e)
            {
                return null;
            }
        }

        [OperationContract, WebGet(ResponseFormat = WebMessageFormat.Json)]
        public List<Game> Get_TopTenGames()
        {
            try
            {
                List<Game> list = Game.GetTopTenGames();
                return list;
            }
            catch (Exception e)
            {
                return null;
            }

        }

        [OperationContract,WebInvoke(
              Method = "POST",
              BodyStyle = WebMessageBodyStyle.WrappedRequest,
              RequestFormat = WebMessageFormat.Json,
              ResponseFormat = WebMessageFormat.Json
          )]
        public string Post_AddGame(string pcode, string category, double price, string picture, string title,
            string genre, int warehousecode, int minimum, string console, string company, string reldate)
        {
            try
            {
                Game gm = new Game(pcode, "game", category, price, picture, title, genre, warehousecode, minimum, console, company, reldate);
                gm.Saving();
                return "The new Game is Added, but state => INACTIVE <= until you make a purchase of the Product ! >> GO In Section Inventory => [INACTIVE] << ";
            }
            catch(Exception e)
            {
                return e.Message;
            }
        }

        [OperationContract, WebGet(ResponseFormat = WebMessageFormat.Json)]
        public List<Employee> Get_ListEmployee()
        {
            try
            {
                List<Employee> list = Employee.GetEmployees();
                return list;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        [OperationContract, WebInvoke(
              Method = "POST",
              BodyStyle = WebMessageBodyStyle.WrappedRequest,
              RequestFormat = WebMessageFormat.Json,
              ResponseFormat = WebMessageFormat.Json
          )]
        public string Post_AddPurchase(int employee_id)
        {
            try
            {
                /*Purchase.Adding(employee_id);*/
                return "Purchase Pending Success !";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        [OperationContract, WebInvoke(
              Method = "POST",
              BodyStyle = WebMessageBodyStyle.WrappedRequest,
              RequestFormat = WebMessageFormat.Json,
              ResponseFormat = WebMessageFormat.Json
          )]
        public string Post_AddToStock_In(string pcode, int purchase_id, double unit_price_bought)
        {
            try
            {
                /*Stock_In.Adding(pcode, purchase_id, unit_price_bought);*/
                return "Added to The Stock_In Empty Until Purchase !";
            }
            catch (Exception e)
            {
                return e.Message;
            }

        }

        [OperationContract, WebInvoke(
              Method = "POST",
              BodyStyle = WebMessageBodyStyle.WrappedRequest,
              RequestFormat = WebMessageFormat.Json,
              ResponseFormat = WebMessageFormat.Json
          )]
        public string Post_AddToStock_Out(int id_stock, int quantity)
        {
            try
            {
                Stock_Out.Adding(id_stock, quantity);
                return "Added to The Stock_Out Empty Until Purchase !";
            }
            catch (Exception e)
            {
                return e.Message;
            }

        }

        // Ajoutez des opérations supplémentaires ici et marquez-les avec [OperationContract]
    }
}
