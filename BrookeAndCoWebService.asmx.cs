using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using WebApplicationBrookeAndCo.Classes;

namespace WebApplicationBrookeAndCo
{
    /// <summary>
    /// Summary description for BrookeAndCoWebService
    /// </summary>
    //[WebService(Namespace = "http://tempuri.org/")]
    //[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    //[System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class BrookeAndCoWebService : System.Web.Services.WebService
    {
        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void sendEmail(string receiver, int orderId, double subtotal, double GST, double QST, double total, double delivFee, string delivDate, int delivAddress)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            try
            {
                Gmail email = new Gmail();
                DeliveryAdress Address = new DeliveryAdress(delivAddress);
                email.sendEmail(receiver, orderId, subtotal, GST, QST, total, delivFee, delivDate, Address);
                Context.Response.Write(js.Serialize(email));
            }
            catch (Exception ex)
            {
                Context.Response.Write(js.Serialize(ex.ToString()));
            }
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void getProductByPcode(string pcode)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            try
            {
                Context.Response.Write(js.Serialize(new Product(pcode)));
            }
            catch (Exception ex)
            {
                Context.Response.Write(js.Serialize(ex.ToString()));
            }
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void getProductByTitle(string title)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            try
            {
                string replace = title.Replace("`", "'");
                List<Product> productList = Product.getProductsByTitle(replace);
                if (productList.Count > 0)
                {
                    Product pr = productList[0];
                    if (pr.type == "book")
                    {
                        Book book = new Book(pr.pcode);
                        Context.Response.Write(js.Serialize(book));
                    }
                    else if (pr.type == "game")
                    {
                        Game game = new Game(pr.pcode);
                        Context.Response.Write(js.Serialize(game));
                    }
                    else if (pr.type == "movie")
                    {
                        Movie movie = new Movie(pr.pcode);
                        Context.Response.Write(js.Serialize(movie));
                    }
                }
                else
                {
                    Product pr = new Product();
                    Context.Response.Write(js.Serialize(pr));
                }
            }
            catch (Exception ex)
            {
                Context.Response.Write(js.Serialize(ex.ToString()));
            }
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void modifyProductByPcode(string pcode, int quant)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            try
            {
                Product newProduct = new Product(pcode);
                string request = "update product set sold_quant=(sold_quant + " + quant + ")  where pcode='" + pcode + "';";
                newProduct.modify(request);
                Context.Response.Write(js.Serialize(newProduct));
            }
            catch (Exception ex)
            {
                Context.Response.Write(js.Serialize(ex.ToString()));
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void getTopTen()
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            try
            {
                List<Product> list = Product.getTopTen();
                //Context.Response.ContentType = "application/json";
                Context.Response.Write(js.Serialize(list));
            }
            catch (Exception ex)
            {
                Context.Response.Write(js.Serialize(ex.ToString()));
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void getProducts()
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            try
            {
                List<Product> list = Product.getProducts();
                //Context.Response.ContentType = "application/json";
                Context.Response.Write(js.Serialize(list));
            }
            catch (Exception ex)
            {
                Context.Response.Write(js.Serialize(ex.ToString()));
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void getBooks()
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            try
            {
                List<Book> list = Book.getBookList();
                //Context.Response.ContentType = "application/json";
                Context.Response.Write(js.Serialize(list));
            }
            catch (Exception ex)
            {
                Context.Response.Write(js.Serialize(ex.ToString()));
            }
        }


        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void getBooksByCategory(string category)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            try
            {
                List<Book> list = Book.getBookListByCategory(category);
                Context.Response.Write(js.Serialize(list));
            }
            catch (Exception ex)
            {
                Context.Response.Write(js.Serialize(ex.ToString()));
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void getBook(string pcode)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            try
            {
                Context.Response.Write(js.Serialize(new Book(pcode)));
            }
            catch (Exception ex)
            {
                Context.Response.Write(js.Serialize(ex.ToString()));
            }
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void deleteBook(string pcode)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            try
            {
                Book p = new Book(pcode);
                p.delete();
                Context.Response.Write(js.Serialize("Book successfuly deleted"));
            }
            catch (Exception ex)
            {
                Context.Response.Write(js.Serialize(ex.ToString()));
            }
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void createBook(int id, string pcode, string picture, string type, string title, string category, int inventory, string genre, double price, int bookid, string authors, string pubCo, string pubDate)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            try
            {
                Book p = new Book(pcode, picture, type, title, category, inventory, genre, price, authors, pubCo, pubDate);
                p.save();
                Context.Response.Write(js.Serialize(p));
            }
            catch (Exception ex)
            {
                Context.Response.Write(js.Serialize(ex.ToString()));
            }
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void updateBook(string pcode, string picture, string type, string title, string category, int inventory, string genre, double price, string authors, string pubCo, string pubDate)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            try
            {
                Book p = new Book(pcode);
                p.picture = picture;
                p.type = type;
                p.title = title;
                p.category = category;
                p.inventory = inventory;
                p.genre = genre;
                p.price = price;
                p.authors = authors;
                p.pubCo = pubCo;
                p.pubDate = pubDate;
                p.modify();
                Context.Response.Write(js.Serialize(p));
            }
            catch (Exception ex)
            {
                Context.Response.Write(js.Serialize(ex.ToString()));
            }
        }

        /// <summary>
        /// /////////////////////////////////////////////////////////////////////////////////////
        /// </summary>

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void getGames()
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            try
            {
                List<Game> list = Game.getGameList();
                Context.Response.Write(js.Serialize(list));
            }
            catch (Exception ex)
            {
                Context.Response.Write(js.Serialize(ex.ToString()));
            }
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void getGamesByCategory(string category)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            try
            {
                List<Game> list = Game.getGameListByCategory(category);
                Context.Response.Write(js.Serialize(list));
            }
            catch (Exception ex)
            {
                Context.Response.Write(js.Serialize(ex.ToString()));
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void getGame(string pcode)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            try
            {
                Context.Response.Write(js.Serialize(new Game(pcode)));
            }
            catch (Exception ex)
            {
                Context.Response.Write(js.Serialize(ex.ToString()));
            }
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void deleteGame(string pcode)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            try
            {
                Game p = new Game(pcode);
                p.delete();
                Context.Response.Write(js.Serialize("Game successfuly deleted"));
            }
            catch (Exception ex)
            {
                Context.Response.Write(js.Serialize(ex.ToString()));
            }
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void createGame(string pcode, string picture, string type, string title, string category, int inventory, string genre, double price, string console, string company, string reldate)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            try
            {
                Game p = new Game(pcode, picture, type, title, category, inventory, genre, price, console, company, reldate);
                p.save();
                Context.Response.Write(js.Serialize(p));
            }
            catch (Exception ex)
            {
                Context.Response.Write(js.Serialize(ex.ToString()));
            }
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void updateGame(string pcode, string picture, string type, string title, string category, int inventory, string genre, double price, string console, string company, string reldate)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            try
            {
                Game p = new Game(pcode);
                p.picture = picture;
                p.type = type;
                p.title = title;
                p.category = category;
                p.inventory = inventory;
                p.genre = genre;
                p.price = price;
                p.console = console;
                p.company = company;
                p.reldate = reldate;
                p.modify();
                Context.Response.Write(js.Serialize(p));
            }
            catch (Exception ex)
            {
                Context.Response.Write(js.Serialize(ex.ToString()));
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////


        /// <summary>
        /// /////////////////////////////////////////////////////////////////////////////////////
        /// </summary>

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void getMovies()
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            try
            {
                List<Movie> list = Movie.getMovieList();
                Context.Response.Write(js.Serialize(list));
            }
            catch (Exception ex)
            {
                Context.Response.Write(js.Serialize(ex.ToString()));
            }
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void getMoviesByCategory(string category)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            try
            {
                List<Movie> list = Movie.getMovieListByCategory(category);
                Context.Response.Write(js.Serialize(list));
            }
            catch (Exception ex)
            {
                Context.Response.Write(js.Serialize(ex.ToString()));
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void getMovie(string pcode)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            try
            {
                Context.Response.Write(js.Serialize(new Movie(pcode)));
            }
            catch (Exception ex)
            {
                Context.Response.Write(js.Serialize(ex.ToString()));
            }
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void deleteMovie(string pcode)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            try
            {
                Movie p = new Movie(pcode);
                p.delete();
                Context.Response.Write(js.Serialize("Movie successfuly deleted"));
            }
            catch (Exception ex)
            {
                Context.Response.Write(js.Serialize(ex.ToString()));
            }
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void createMovie(string pcode, string picture, string type, string title, string category, int inventory, string genre, double price, string director, string actors, string relyear)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            try
            {
                Movie p = new Movie(pcode, picture, type, title, category, inventory, genre, price, director, actors, relyear);
                p.save();
                Context.Response.Write(js.Serialize(p));
            }
            catch (Exception ex)
            {
                Context.Response.Write(js.Serialize(ex.ToString()));
            }
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void updateMovie(string pcode, string picture, string type, string title, string category, int inventory, string genre, double price, string director, string actors, string relyear)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            try
            {
                Movie p = new Movie(pcode);
                p.picture = picture;
                p.type = type;
                p.title = title;
                p.category = category;
                p.inventory = inventory;
                p.genre = genre;
                p.price = price;
                p.director = director;
                p.actors = actors;
                p.relyear = relyear;
                p.modify();
                Context.Response.Write(js.Serialize(p));
            }
            catch (Exception ex)
            {
                Context.Response.Write(js.Serialize(ex.ToString()));
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void getClients()
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            try
            {
                List<Client> list = Client.getClientsList();
                List<Client> clientsList = new List<Client>();

                foreach (Client client in list)
                {
                    Client complete = clientConstructor(client);
                    clientsList.Add(complete);
                }

                Context.Response.Write(js.Serialize(clientsList));
            }
            catch (Exception ex)
            {
                Context.Response.Write(js.Serialize(ex.ToString()));
            }
        }


        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void getDeliveriesByOrderId(int orderId)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            try
            {
                List<Delivery> list = Delivery.getDeliveriesListByOrderId(orderId);
                Context.Response.Write(js.Serialize(list));
            }
            catch (Exception ex)
            {
                Context.Response.Write(js.Serialize(ex.ToString()));
            }
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void createShipment(int order_id, int address_id, int service_id, string delivery_date, double total_cost)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            try
            {
                DeliveryAdress delivery_address = new DeliveryAdress();
                Delivery shipment = new Delivery(0, order_id, delivery_address, address_id, service_id, 0, "received", 0, delivery_date, total_cost);
                shipment.save();
                Context.Response.Write(js.Serialize(shipment));
            }
            catch (Exception ex)
            {
                Context.Response.Write(js.Serialize(ex.ToString()));
            }
        }

        public Client clientConstructor(Client client)
        {
            Client newClient = client;
            int clientid = newClient.id;

            List<Order> Orderslist = Order.getOrdersListByClientId(clientid);
            List<DeliveryAdress> Addresseslist = DeliveryAdress.getDeliveryAdressListByClientId(clientid);

            foreach (Order order in Orderslist)
            {
                int orderId = order.order_id;
                List<Entry> Entrieslist = Entry.getEntriesListByOrderId(orderId);

                foreach (Entry entry in Entrieslist)
                {
                    order.addEntryToOrder(entry);
                }
                //newClient.orders.Add(order);

                List<Delivery> PastOrders = Delivery.getDeliveriesListByOrderId(orderId);

                foreach (Delivery delivery in PastOrders)
                {
                    int delAdd = delivery.address_id;
                    DeliveryAdress deliveryAdd = new DeliveryAdress(delAdd);
                    delivery.delivery_address = deliveryAdd;
                    order.deliveries.Add(delivery);
                }
                newClient.orders.Add(order);
            }
            foreach (DeliveryAdress address in Addresseslist)
            {
                newClient.adresses.Add(address);
            }

            return newClient;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void validateClientExists(string username, string password)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            try
            {
                Client newClient = new Client(username, password);
                Client complete = clientConstructor(newClient);
                Context.Response.Write(js.Serialize(complete));
            }
            catch (Exception ex)
            {
                Context.Response.Write(js.Serialize(ex.ToString()));
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void getClient(int id)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            try
            {
                Client newClient = new Client(id);
                Client complete = clientConstructor(newClient);
                Context.Response.Write(js.Serialize(complete));
            }
            catch (Exception ex)
            {
                Context.Response.Write(js.Serialize(ex.ToString()));
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void getClientByEmail(string email)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            try
            {
                String request = "Select * from clients where email='" + email + "'";
                Client newClient = new Client(request);
                if (newClient.id != 0)
                {
                    Client complete = clientConstructor(newClient);
                    Context.Response.Write(js.Serialize(complete));
                }
                else
                {
                    Context.Response.Write(js.Serialize(newClient));
                }

            }
            catch (Exception ex)
            {
                Context.Response.Write(js.Serialize(ex.ToString()));
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void CheckUserNameIsFree(string username)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            try
            {
                String request = "Select * from clients where username='" + username + "'";
                Client newClient = new Client(request);
                bool exists = true;

                if (newClient.username == null)
                {
                    exists = false;
                }
                Context.Response.Write(js.Serialize(exists));
            }
            catch (Exception ex)
            {
                Context.Response.Write(js.Serialize(ex.ToString()));
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void createClient(string email, string username, string password, string name, string lastname, string bday)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            try
            {
                Client newClient = new Client(email, username, password, name, lastname, bday);
                newClient.save();

                Context.Response.Write(js.Serialize(newClient));
            }
            catch (Exception ex)
            {
                Context.Response.Write(js.Serialize(ex.ToString()));
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void modifyClient(int id, string email, string username, string password, string name, string lastname, string bday)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            try
            {
                Client newClient = new Client(id);
                newClient.email = email;
                newClient.username = username;
                newClient.password = password;
                newClient.name = name;
                newClient.lastname = lastname;
                newClient.bday = bday;
                newClient.modify();

                Context.Response.Write(js.Serialize(newClient));
            }
            catch (Exception ex)
            {
                Context.Response.Write(js.Serialize(ex.ToString()));
            }
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void deleteClient(int id)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            try
            {
                Client newClient = new Client(id);
                newClient.delete();
                Context.Response.Write(js.Serialize(newClient));
            }
            catch (Exception ex)
            {
                Context.Response.Write(js.Serialize(ex.ToString()));
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void getOrders()
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            try
            {
                List<Order> list = Order.getOrdersList();
                Context.Response.Write(js.Serialize(list));
            }
            catch (Exception ex)
            {
                Context.Response.Write(js.Serialize(ex.ToString()));
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void deleteAllEntriesFromOrder(int orderId)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            try
            {
                Order order = new Order(orderId);
                order.deleteAllEntries();
            }
            catch (Exception ex)
            {
                Context.Response.Write(js.Serialize(ex.ToString()));
            }
        }


        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void getOrdersByClientId(int clientId)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            try
            {
                List<Order> list = Order.getOrdersListByClientId(clientId);
                Context.Response.Write(js.Serialize(list));
            }
            catch (Exception ex)
            {
                Context.Response.Write(js.Serialize(ex.ToString()));
            }
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void createNewOrder(int clientId)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            try
            {
                Order newOrder = new Order(clientId, "ongoing");
                newOrder.save();
                Context.Response.Write(js.Serialize(newOrder));
            }
            catch (Exception ex)
            {
                Context.Response.Write(js.Serialize(ex.ToString()));
            }
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void modifyOrderStatus(int orderId, string date)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            try
            {
                Order newOrder = new Order(orderId);
                string request = "update brookeandco.order set status='paid', date='" + date + "' where id=" + orderId;
                newOrder.modify(request);
                Context.Response.Write(js.Serialize(newOrder));
            }
            catch (Exception ex)
            {
                Context.Response.Write(js.Serialize(ex.ToString()));
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void getEntries()
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            try
            {
                List<Entry> list = Entry.getEntriesList();
                Context.Response.Write(js.Serialize(list));
            }
            catch (Exception ex)
            {
                Context.Response.Write(js.Serialize(ex.ToString()));
            }
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void addEntryToOrder(int orderId, string pcode, int quantity, double price, int basket)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            try
            {
                double subtotal = Math.Round(quantity * price, 2);
                Entry newEntry = new Entry(0, orderId, pcode, quantity, price, subtotal);
                List<Entry> list = Entry.getEntriesListByOrderId(orderId);
                int isIn = 0;

                foreach (Entry entry in list)
                {
                    if (entry.pcode == pcode)
                    {
                        isIn = 1;

                        if (basket == 0)
                            entry.modifyEntry(entry.quantity + quantity);
                        else
                            entry.modifyEntry(quantity);

                        Context.Response.Write(js.Serialize(entry));
                    }
                }
                if (isIn == 0)
                {
                    newEntry.save();
                    Context.Response.Write(js.Serialize(newEntry));
                }
            }
            catch (Exception ex)
            {
                Context.Response.Write(js.Serialize(ex.ToString()));
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void deleteEntryFromOrder(int orderId, string pcode)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            try
            {
                Order order = new Order(orderId);
                order.deleteEntry(pcode);
                Context.Response.Write("Entry deleted");
            }
            catch (Exception ex)
            {
                Context.Response.Write(js.Serialize(ex.ToString()));
            }
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void deleteEntry(int orderId, string pcode)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            try
            {
                double subtotal = 0;
                int quantity = 0;
                double price = 0;
                Entry newEntry = new Entry(0, orderId, pcode, quantity, price, subtotal);
                List<Entry> list = Entry.getEntriesListByOrderId(orderId);

                foreach (Entry entry in list)
                {
                    if (entry.pcode == pcode)
                    {
                        entry.delete();
                        Context.Response.Write(js.Serialize(entry));
                    }
                }
            }
            catch (Exception ex)
            {
                Context.Response.Write(js.Serialize(ex.ToString()));
            }
        }


        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void getEntriesByOrderId(int orderId)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            try
            {
                List<Entry> list = Entry.getEntriesListByOrderId(orderId);
                Context.Response.Write(js.Serialize(list));
            }
            catch (Exception ex)
            {
                Context.Response.Write(js.Serialize(ex.ToString()));
            }
        }



        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void getDeliveryAdress()
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            try
            {
                List<DeliveryAdress> list = DeliveryAdress.getDeliveryAdressList();
                Context.Response.Write(js.Serialize(list));
            }
            catch (Exception ex)
            {
                Context.Response.Write(js.Serialize(ex.ToString()));
            }
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void getDeliveryAdressesByClientId(int clientId)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            try
            {
                List<DeliveryAdress> list = DeliveryAdress.getDeliveryAdressListByClientId(clientId);
                Context.Response.Write(js.Serialize(list));
            }
            catch (Exception ex)
            {
                Context.Response.Write(js.Serialize(ex.ToString()));
            }
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void getDeliveryAdressById(int id)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            try
            {
                Context.Response.Write(js.Serialize(new DeliveryAdress(id)));
            }
            catch (Exception ex)
            {
                Context.Response.Write(js.Serialize(ex.ToString()));
            }
        }

        /*=======================================================================*/

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void getCardsByClientId(int clientId)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            try
            {
                List<Card> list = Card.getCardsByClientId(clientId);
                Context.Response.Write(js.Serialize(list));
            }
            catch (Exception ex)
            {
                Context.Response.Write(js.Serialize(ex.ToString()));
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void getCardById(int card_id)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            try
            {
                Context.Response.Write(js.Serialize(new Card(card_id)));
            }
            catch (Exception ex)
            {
                Context.Response.Write(js.Serialize(ex.ToString()));
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void modifyClientCard(int id, string holdername, string expiration)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            try
            {
                Card card = new Card(id);
                card.holdername = holdername;
                card.expiration = expiration;
                card.modify();
                Context.Response.Write(js.Serialize(card));
            }
            catch (Exception ex)
            {
                Context.Response.Write(js.Serialize(ex.ToString()));
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void deleteClientCard(int id)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            try
            {
                Card card = new Card(id);
                card.delete();
                Context.Response.Write(js.Serialize(card));
            }
            catch (Exception ex)
            {
                Context.Response.Write(js.Serialize(ex.ToString()));
            }
        }

        public void activateClientCard(int id)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            try
            {
                Card card = new Card(id);
                card.activate();
                Context.Response.Write(js.Serialize(card));
            }
            catch (Exception ex)
            {
                Context.Response.Write(js.Serialize(ex.ToString()));
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void createClientCard(int id, int client_id, string type, string card_number, string security_code, string holdername, string expiration)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            try
            {
                Card card = new Card(id, client_id, type, card_number, security_code, holdername, expiration);
                card.save();
                Context.Response.Write(js.Serialize(card));
            }
            catch (Exception ex)
            {
                Context.Response.Write(js.Serialize(ex.ToString()));
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void createTemporaryClientCard(int id, int client_id, string type, string card_number, string security_code, string holdername, string expiration, string delete_on)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            try
            {
                Card card = new Card(id, client_id, type, card_number, security_code, holdername, expiration);
                card.saveTemporary(delete_on);
                Context.Response.Write(js.Serialize(card));
            }
            catch (Exception ex)
            {
                Context.Response.Write(js.Serialize(ex.ToString()));
            }
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void modifyDeliveryAddress(int id, string country, string province, string city, string street, int civicnumber, string appartment, string zipcode)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            try
            {
                DeliveryAdress address = new DeliveryAdress(id);

                DeliveryAdress compare = new DeliveryAdress(country, province, city, street, civicnumber, appartment, zipcode, address.client_id);
                if (addressExists(compare))
                {
                    address.delete();
                    Context.Response.Write("address has been deleted");
                }
                else
                {
                    address.country = country;
                    address.province = province;
                    address.city = city;
                    address.street = street;
                    address.civicnumber = civicnumber;
                    address.appartment = appartment;
                    address.zipcode = zipcode;
                    address.modify();
                    Context.Response.Write(js.Serialize(address));
                }

            }
            catch (Exception ex)
            {
                Context.Response.Write(js.Serialize(ex.ToString()));
            }
        }

        public Boolean addressExists(DeliveryAdress address)
        {
            bool exists = false;
            List<DeliveryAdress> activeList = DeliveryAdress.getDeliveryAdressListByClientId(address.client_id);

            foreach (DeliveryAdress addr in activeList)
            {
                if (addr.country == address.country &
                           addr.province == address.province &
                           addr.city == address.city &
                           addr.street == address.street &
                           addr.civicnumber == address.civicnumber &
                           addr.appartment == address.appartment &
                           addr.zipcode == address.zipcode)
                    exists = true;
            }
            return exists;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void createDeliveryAddress(string country, string province, string city, string street, int civicnumber, string appartment, string zipcode, int client_id)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            try
            {
                DeliveryAdress address = new DeliveryAdress(0, country, province, city, street, civicnumber, appartment, zipcode, client_id);
                address.save();
                Context.Response.Write(js.Serialize(address));
            }
            catch (Exception ex)
            {
                Context.Response.Write(js.Serialize(ex.ToString()));
            }
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void deleteDeliveryAddress(int id)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            try
            {
                DeliveryAdress address = new DeliveryAdress(id);
                address.delete();
                Context.Response.Write(js.Serialize(address));
            }
            catch (Exception ex)
            {
                Context.Response.Write(js.Serialize(ex.ToString()));
            }
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void getServicesList()
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            try
            {
                List<DeliveryType> list = DeliveryType.getDeliveryTypeList();
                Context.Response.Write(js.Serialize(list));
            }
            catch (Exception ex)
            {
                Context.Response.Write(js.Serialize(ex.ToString()));
            }
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void getServiceById(int id)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            try
            {
                Context.Response.Write(js.Serialize(new DeliveryType(id)));
            }
            catch (Exception ex)
            {
                Context.Response.Write(js.Serialize(ex.ToString()));
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void getClientTransactionById(int id)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            try
            {
                Client_Transaction card = new Client_Transaction(id);
                Context.Response.Write(js.Serialize(card));
            }
            catch (Exception ex)
            {
                Context.Response.Write(js.Serialize(ex.ToString()));
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void createClientTransaction(int id, int card_id, string transaction_confirmation, string status, string validation, double amount, int order_id, string date)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            try
            {
                Client_Transaction card = new Client_Transaction(id, card_id, transaction_confirmation, status, validation, amount, order_id, date);
                card.save();
                Context.Response.Write(js.Serialize(card));
            }
            catch (Exception ex)
            {
                Context.Response.Write(js.Serialize(ex.ToString()));
            }
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void getStockIns()
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            try
            {
                List<StockIn> list = StockIn.getStockInList();
                Context.Response.Write(js.Serialize(list));
            }
            catch (Exception ex)
            {
                Context.Response.Write(js.Serialize(ex.ToString()));
            }
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void getStockInListByPcode(string pcode)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            try
            {
                List<StockIn> list = StockIn.getStockInListByPcode(pcode);
                Context.Response.Write(js.Serialize(list));
            }
            catch (Exception ex)
            {
                Context.Response.Write(js.Serialize(ex.ToString()));
            }
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
        public void getStockOuts()
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            try
            {
                List<StockOut> list = StockOut.getStockOutList();
                Context.Response.Write(js.Serialize(list));
            }
            catch (Exception ex)
            {
                Context.Response.Write(js.Serialize(ex.ToString()));
            }
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void newStockOut(int stock_id, int transaction_id, string type, int quant)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            try
            {
                StockOut stockOut = new StockOut(0, stock_id, transaction_id, type, quant);
                StockIn stockIn = new StockIn(stock_id);
                stockOut.save();
                stockIn.modify("update stock_in set left_quant=(left_quant-" + quant + ") where id=" + stock_id);
                Context.Response.Write("stock out" + js.Serialize(stockOut));
                Context.Response.Write("stock in" + js.Serialize(stockIn));
            }
            catch (Exception ex)
            {
                Context.Response.Write(js.Serialize(ex.ToString()));
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void getSalesList()
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            try
            {
                List<Sale> list = Sale.getSalesList();
                //Context.Response.ContentType = "application/json";
                Context.Response.Write(js.Serialize(list));
            }
            catch (Exception ex)
            {
                Context.Response.Write(js.Serialize(ex.ToString()));
            }
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void getEmployee(string username, string password)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            try
            {
                Employee employee = new Employee(username, password);
                Context.Response.Write(js.Serialize(employee));
            }
            catch (Exception ex)
            {
                Context.Response.Write(js.Serialize(ex.ToString()));
            }
        }
    }
}
