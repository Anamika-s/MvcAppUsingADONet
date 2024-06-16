using Microsoft.AspNetCore.Mvc;
using MvcAppUsingADONet.Models;
using System.Data.SqlClient;

namespace MvcAppUsingADONet.Controllers
{
    public class InventoryController : Controller
    {
        IConfiguration _configuration;
        public InventoryController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        SqlConnection GetConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("MyConnection"));
        }
        public IActionResult Index()
        {
            List<Inventory> inventories = new List<Inventory>();
            using (SqlConnection connection = GetConnection())
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.CommandText = "Select * from Inventory";
                    command.Connection = connection;
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            // Object Initializer
                            Inventory inventory = new Inventory()
                            {
                                InventoryId = (int)reader[0],
                                ProductName = reader[1].ToString(),
                                QtyInStock = (int)reader[2],
                                ReorderLevel = (int)reader[3],
                                AddedOn = (DateTime)reader[4]
                            };
                            inventories.Add(inventory);
                        }
                        connection.Close();
                        return View(inventories);
                    }
                    else
                    {
                        ViewBag.msg = "There are no records";
                        return View();
                    }
                }


            }


        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Inventory inventory)
        {
            using (SqlConnection connection = GetConnection())
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.CommandText = "insert into inventory (productName, qtyInStock,reorderLevel,AddedOn) values (@productName, @qtyInStock,@reorderLevel,@AddedOn)";
                    command.Parameters.AddWithValue("@productName", inventory.ProductName);
                    command.Parameters.AddWithValue("@qtyInStock", inventory.QtyInStock);
                    command.Parameters.AddWithValue("@reorderLevel", inventory.ReorderLevel);
                    command.Parameters.AddWithValue("@AddedOn", inventory.AddedOn);

                    command.Connection = connection;
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }

            return RedirectToAction("Index");
        }

        Inventory Search(int id)
        {
            Inventory inventory = null;
            using (SqlConnection connection = GetConnection())
            {
                using (SqlCommand command = new SqlCommand())

                {
                    command.CommandText = "select * from inventory where Inventory_id =@id";
                    command.Parameters.AddWithValue("@id", id);
                    command.Connection = connection;
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        inventory = new Inventory()
                        {

                            InventoryId = (int)reader[0],
                            ProductName = reader[1].ToString(),
                            QtyInStock = (int)reader[2],
                            ReorderLevel = (int)reader[3],
                            AddedOn = (DateTime)reader[4]
                        };
                        connection.Close();
                    }
                }
            }
                        return inventory;
                    }

                    public IActionResult Details(int id)
                    {
                        Inventory inventory = Search(id);
                       if (inventory != null)
                        return View(inventory);
                    
                    else
                    {
                        ViewBag.msg = "There is no such Record with this ID";
                        return View();
                    }
                }
               
        
         public IActionResult Delete(int id)
        {
            Inventory inventory = Search(id);
            if (inventory != null)
                return View(inventory);

            else
            {
                ViewBag.msg = "There is no such Record with this ID";
                return View();
            }

        }
        [HttpPost]
        public IActionResult Deleted(int id) {
        using (SqlConnection connection = GetConnection())
            {
                using (SqlCommand command = new SqlCommand())
               
                {
                    command.CommandText = "delete from inventory where inventory_Id = @id";
                    command.Parameters.AddWithValue("@id", id);
                    command.Connection = connection;
                    connection.Open();
                     
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            Inventory inventory = Search(id);
            if (inventory != null)
                return View(inventory);

            else
            {
                ViewBag.msg = "There is no such Record with this ID";
                return View();
            }

        }
        [HttpPost]
        public IActionResult Edit(int id, Inventory inventory)
        {
            using (SqlConnection connection = GetConnection())
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.CommandText = "update inventory set qtyInStock=@qtyInStock , reorderLevel=@reorderLevel,  AddedOn=@AddedOn where inventory_id=@id";
                    command.Parameters.AddWithValue("@qtyInStock", inventory.QtyInStock);
                    command.Parameters.AddWithValue("@reorderLevel", inventory.ReorderLevel);
                    command.Parameters.AddWithValue("@AddedOn", inventory.AddedOn);
                    command.Parameters.AddWithValue("@id", id);
                    command.Connection = connection;
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }

            }
            return RedirectToAction("Index");
        }
    }

   
    }
