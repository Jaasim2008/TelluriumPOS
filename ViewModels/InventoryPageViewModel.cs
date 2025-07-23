using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Data.Sqlite;
using System.Collections.ObjectModel;
using System.Diagnostics;
using TelluriumPOS.Catalysts;

namespace TelluriumPOS.ViewModels
{
    public partial class InventoryPageViewModel : ObservableObject
    {
        //Init logger
        private readonly ILoggerCatalyst _logger = new Logger();

        [ObservableProperty]
        ObservableCollection<Product> products = new();
        [ObservableProperty]
        ObservableCollection<Category> categories = new();
        //Show Sections Variables
        [ObservableProperty]
        private bool showAddItem = false;
        [ObservableProperty]
        private bool showAddCategory = false;
        [ObservableProperty]
        private bool showModifyCategory = false;
        [ObservableProperty]
        private bool showRemoveCategory = false;
        [ObservableProperty]
        private string usernameValue;
        //Add Item Binding Variables
        [ObservableProperty]
        private int addItemCategoryid = 0; //max hardcoded to 10,000
        [ObservableProperty]
        private int addItemVATPercentage; //max hardcoded to 100%
        [ObservableProperty]
        private string addItemName = "";
        [ObservableProperty]
        private string addItemBarcode = "";
        [ObservableProperty]
        private string addItemPricePlusVAT = ""; //checked in addItem() if int
        [ObservableProperty]
        private string addItemCostPrice = ""; //checked in addItem() if int
        [ObservableProperty]
        private string addItemStock = ""; //checked in addItem() if int
        //Category Actions Binding Variables
        [ObservableProperty]
        private string addCategoryName = "";
        [ObservableProperty]
        private string modifyCategoryId = ""; //this is not type of int because it may crash when use enters alpha
        [ObservableProperty]
        private string modifyCategoryName = "";
        [ObservableProperty]
        private string removeCategoryId = "";

        public async Task OnMount()
        {
            await GetUsernameSetting();
            await GetProductsTable();
            await GetCategoriesTable();
            await GetDefaultVATPercentageSetting();
        }
        private async Task GetUsernameSetting()
        {
            string selectQuery = "SELECT value FROM preferences WHERE option = 'UserName';";
            try
            {
                using var conn = new SqliteConnection(@"Data Source = C:\ProgramData\TelluriumPOS\displayPreferences.db");
                await conn.OpenAsync();
                using var command = new SqliteCommand(selectQuery, conn);
                var result = await command.ExecuteScalarAsync();
                UsernameValue = result?.ToString() ?? "";
                await conn.CloseAsync();
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message, SignificanceLevels.CRITICAL, UsernameValue ?? "Unknown", "GetUsernameSetting");
            }
        }
        private async Task GetProductsTable()
        {
            const string selectQuery = "SELECT * FROM Products;";
            try
            {
                using var conn = new SqliteConnection(@"Data Source = C:\ProgramData\TelluriumPOS\backend.db");
                await conn.OpenAsync();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = selectQuery;
                using var reader = await cmd.ExecuteReaderAsync(); // async reader :contentReference[oaicite:4]{index=4}
                Products.Clear();
                while (await reader.ReadAsync())
                {
                    var product = new Product
                    {
                        Id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                        Name = reader.IsDBNull(1) ? "" : reader.GetString(1),
                        Barcode = reader.IsDBNull(2) ? "" : reader.GetString(2),
                        CategoryId = reader.IsDBNull(3) ? 0 : reader.GetInt32(3),
                        Price = reader.IsDBNull(4) ? 0 : reader.GetDouble(4),
                        CostPrice = reader.IsDBNull(5) ? 0 : reader.GetDouble(5),
                        Stock = reader.IsDBNull(6) ? 0 : reader.GetInt32(6),
                        VAT = reader.IsDBNull(7) ? 0 : reader.GetDouble(7),
                        IsActive = reader.IsDBNull(8) ? false : reader.GetBoolean(8)
                    };

                    Products.Add(product);
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message, SignificanceLevels.CRITICAL, UsernameValue ?? "Unknown", "GetProductsTable");
            }
        }
        private async Task GetCategoriesTable()
        {
            const string selectQuery = "SELECT * FROM Categories;";
            try
            {
                using var conn = new SqliteConnection(@"Data Source = C:\ProgramData\TelluriumPOS\backend.db");
                await conn.OpenAsync();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = selectQuery;
                using var reader = await cmd.ExecuteReaderAsync();
                Categories.Clear();
                while (await reader.ReadAsync())
                {
                    var category = new Category
                    {
                        Id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                        Name = reader.IsDBNull(1) ? "" : reader.GetString(1),
                    };

                    Categories.Add(category);
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message, SignificanceLevels.CRITICAL, UsernameValue ?? "Unknown", "GetCategoriesTable");
            }
        }
        private async Task GetDefaultVATPercentageSetting()
        {
            string selectQuery = "SELECT value FROM preferences WHERE option = 'DefaultVATPercentage';";
            try
            {
                using var conn = new SqliteConnection(@"Data Source = C:\ProgramData\TelluriumPOS\displayPreferences.db");
                await conn.OpenAsync();
                using var command = new SqliteCommand(selectQuery, conn);
                var result = await command.ExecuteScalarAsync();
                Debug.WriteLine("result?.ToString() -> " + result?.ToString());
                AddItemVATPercentage = Convert.ToInt32(result?.ToString());
                await conn.CloseAsync();
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message, SignificanceLevels.CRITICAL, UsernameValue ?? "Unknown", "GetDefaultVATPercentageSetting");
            }
        }
        [RelayCommand]
        private void ShowAddItemSection()
        {
            ShowAddItem = true;
        }
        [RelayCommand]
        private void HideAddItemSection()
        {
            AddItemCategoryid = 0;
            AddItemName = AddItemBarcode = AddItemPricePlusVAT = AddItemCostPrice = AddItemStock = "";
            ShowAddItem = false;
        }
        [RelayCommand]
        private void ShowAddCategorySection()
        {
            HideModifyCategorySection();
            HideRemoveCategorySection();
            ShowAddCategory = true;
        }
        [RelayCommand]
        private void HideAddCategorySection()
        {
            AddCategoryName = "";
            ShowAddCategory = false;
        }
        [RelayCommand]
        private void ShowModifyCategorySection()
        {
            HideAddCategorySection();
            HideRemoveCategorySection();
            ShowModifyCategory = true;
        }
        [RelayCommand]
        private void HideModifyCategorySection()
        {
            ModifyCategoryId = "";
            ModifyCategoryName = "";
            ShowModifyCategory = false;
        }
        [RelayCommand]
        private void ShowRemoveCategorySection()
        {
            HideAddCategorySection();
            HideModifyCategorySection();
            ShowRemoveCategory = true;
        }
        [RelayCommand]
        private void HideRemoveCategorySection()
        {
            RemoveCategoryId = "";
            ShowRemoveCategory = false;
        }
        [RelayCommand]
        private async Task AddNewCategory()
        {
            //input validation
            if (AddCategoryName.Length < 1)
            {
                _logger.Log("Category Name's Length must be greater than 1", SignificanceLevels.WARNING, UsernameValue ?? "Unknown", "AddNewCategory");
                return;
            }
            else if (AddCategoryName.Length > 1000)
            {
                _logger.Log("Category Name's Length must be lesser than 1000", SignificanceLevels.WARNING, UsernameValue ?? "Unknown", "AddNewCategory");
                return;
            }
            //after validation pass:
            const string insertQuery = "INSERT INTO Categories (name) VALUES (@name);";
            try
            {
                using var conn = new SqliteConnection(@"Data Source = C:\ProgramData\TelluriumPOS\backend.db");
                await conn.OpenAsync();
                using var cmd = new SqliteCommand(insertQuery, conn);
                cmd.Parameters.AddWithValue("@name", AddCategoryName);
                cmd.ExecuteNonQuery();
                HideAddCategorySection();
                await GetCategoriesTable();
                await Application.Current.MainPage.DisplayAlert("Success", "Successfully Added New Category", "Ok");
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message, SignificanceLevels.CRITICAL, UsernameValue ?? "Unknown", "AddNewCategory");
            }
        }
        [RelayCommand]
        private async Task ModifyCategory()
        {
            //input validation
            if (!int.TryParse(s: ModifyCategoryId, out int parsedId))
            {
                _logger.Log("You have not entered an integer in Category Id", SignificanceLevels.WARNING, UsernameValue ?? "Unknown", "ModifyCategory");
                return;
            } //is ModifyCategoryId an integer?
            //check if entered Category Id even exist in database
            const string checkQuery = "SELECT name FROM Categories WHERE id = @id;";
            try
            {
                using var conn = new SqliteConnection(@"Data Source = C:\ProgramData\TelluriumPOS\backend.db");
                await conn.OpenAsync();
                using var cmd = new SqliteCommand(checkQuery, conn);
                cmd.Parameters.AddWithValue("@id", parsedId);

                var result = await cmd.ExecuteScalarAsync();
                if (result == null)
                {
                    _logger.Log("The entered Category Id doesn't exist!", SignificanceLevels.WARNING, UsernameValue ?? "Unknown", "ModifyCategory");
                    return;
                }

                //proceed with checking ModifyCategoryName
                if (ModifyCategoryName.Length < 1)
                {
                    _logger.Log("New Category Name's Length must be greater than 1", SignificanceLevels.WARNING, UsernameValue ?? "Unknown", "ModifyCategory");
                    return;
                }
                else if (ModifyCategoryName.Length > 10000)
                {
                    _logger.Log("New Category Name's Length must be lesser than 1000", SignificanceLevels.WARNING, UsernameValue ?? "Unknown", "ModifyCategory");
                    return;
                }

                //if everything is okay:
                const string updateQuery = "UPDATE Categories SET name = @name WHERE id = @id;";
                try
                {
                    using var cmd2 = new SqliteCommand(updateQuery, conn);
                    cmd2.Parameters.AddWithValue("@name", ModifyCategoryName);
                    cmd2.Parameters.AddWithValue("@id", parsedId);
                    await cmd2.ExecuteNonQueryAsync();
                    HideModifyCategorySection();
                    await GetCategoriesTable();
                    await Application.Current.MainPage.DisplayAlert("Success", "Successfully Modified Category", "Ok");
                }
                catch (Exception ex)
                {
                    _logger.Log(ex.Message, SignificanceLevels.CRITICAL, UsernameValue ?? "Unknown", "ModifyCategory");
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message, SignificanceLevels.CRITICAL, UsernameValue ?? "Unknown", "ModifyCategory");
            }
        }
        [RelayCommand]
        private async Task RemoveCategory()
        {
            //input validation
            if (!int.TryParse(s: RemoveCategoryId, out int parsedId))
            {
                _logger.Log("You have not entered an integer in Category Id", SignificanceLevels.WARNING, UsernameValue ?? "Unknown", "RemoveCategory");
                return;
            } //is RemoveCategoryId an integer?
            //check if entered Category Id even exist in database
            const string checkQuery = "SELECT name FROM Categories WHERE id = @id;";
            try
            {
                using var conn = new SqliteConnection(@"Data Source = C:\ProgramData\TelluriumPOS\backend.db");
                await conn.OpenAsync();
                using var cmd = new SqliteCommand(checkQuery, conn);
                cmd.Parameters.AddWithValue("@id", parsedId);

                var result = await cmd.ExecuteScalarAsync();
                if (result == null)
                {
                    _logger.Log("The entered Category Id doesn't exist!", SignificanceLevels.WARNING, UsernameValue ?? "Unknown", "RemoveCategory");
                    return;
                }

                //if everything is okay:
                const string deleteQuery = "DELETE FROM Categories WHERE id = @id;";
                try
                {
                    using var cmd2 = new SqliteCommand(deleteQuery, conn);
                    cmd2.Parameters.AddWithValue("@id", parsedId);
                    await cmd2.ExecuteNonQueryAsync();
                    HideRemoveCategorySection();
                    await GetCategoriesTable();
                    await Application.Current.MainPage.DisplayAlert("Success", "Successfully Removed Category", "Ok");
                }
                catch (Exception ex)
                {
                    _logger.Log(ex.Message, SignificanceLevels.CRITICAL, UsernameValue ?? "Unknown", "RemoveCategory");
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message, SignificanceLevels.CRITICAL, UsernameValue ?? "Unknown", "RemoveCategory");
            }
        }
        [RelayCommand]
        private async Task AddItem()
        {
            //validate binding variables which are supposed to be int/double
            if (!double.TryParse(AddItemPricePlusVAT, out double parsedPricePlusVAT))
            {
                _logger.Log("Invalid Price (not an double)", SignificanceLevels.WARNING, UsernameValue ?? "Unknown", "AddItem");
                return;
            }
            if (!int.TryParse(AddItemStock, out int parsedStock))
            {
                _logger.Log("Invalid Stock count (not an integer)", SignificanceLevels.WARNING, UsernameValue ?? "Unknown", "AddItem");
                return;
            }
            if (!double.TryParse(AddItemCostPrice, out double parsedCostPrice))
            {
                _logger.Log("Invalid Cost Price (not an integer)", SignificanceLevels.WARNING, UsernameValue ?? "Unknown", "AddItem");
                return;
            }
            //no negative values please
            if (parsedStock < 0 || parsedPricePlusVAT < 0 || parsedCostPrice < 0)
            {
                _logger.Log("Numeric fields cannot be negative", SignificanceLevels.WARNING, UsernameValue ?? "Unknown", "AddItem");
                return;
            }
            //validate new item's name and barcode
            if (AddItemName.Length < 1)
            {
                _logger.Log("New item's name length must be greater than one", SignificanceLevels.WARNING, UsernameValue ?? "Unknown", "AddItem");
                return;
            }
            if (AddItemName.Length > 10000)
            {
                _logger.Log("New item's name length should be under 10000", SignificanceLevels.WARNING, UsernameValue ?? "Unknown", "AddItem");
                return;
            }
            if (AddItemBarcode.Length < 1)
            {
                _logger.Log("New item's barcode length must be greater than one", SignificanceLevels.WARNING, UsernameValue ?? "Unknown", "AddItem");
                return;
            }
            if (AddItemBarcode.Length > 10000)
            {
                _logger.Log("New item's barcode length must be under 10000", SignificanceLevels.WARNING, UsernameValue ?? "Unknown", "AddItem");
                return;
            }
            //check if unique barcode
            const string checkQuery = "SELECT name FROM Products WHERE barcode = @barcode;";
            try
            {
                using var conn = new SqliteConnection(@"Data Source = C:\ProgramData\TelluriumPOS\backend.db");
                await conn.OpenAsync();
                using var cmd = new SqliteCommand(checkQuery, conn);
                cmd.Parameters.AddWithValue("@barcode", AddItemBarcode);
                var result = await cmd.ExecuteScalarAsync();
                if (result != null)
                {
                    _logger.Log("The barcode is already used by another product!", SignificanceLevels.WARNING, UsernameValue ?? "Unknown", "AddItem");
                    return;
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message, SignificanceLevels.CRITICAL, UsernameValue ?? "Unknown", "AddItem");
            }
            //subtract VAT% from AddItemPricePlusVAT
            double addItemPriceMinusVAT = parsedPricePlusVAT - (parsedPricePlusVAT * AddItemVATPercentage / 100);
            //after all that validation
            const string insertQuery = "INSERT INTO Products (name, barcode, category_id, price, cost_price, stock, vat_percent) VALUES (@name, @barcode, @categoryId, @price, @costPrice, @stock, @vatPercent);";
            try
            {
                using var conn = new SqliteConnection(@"Data Source = C:\ProgramData\TelluriumPOS\backend.db");
                await conn.OpenAsync();
                using var cmd2 = new SqliteCommand(insertQuery, conn);
                cmd2.Parameters.AddWithValue("@name", AddItemName);
                cmd2.Parameters.AddWithValue("@barcode", AddItemBarcode);
                cmd2.Parameters.AddWithValue("@categoryId", AddItemCategoryid);
                cmd2.Parameters.AddWithValue("@price", addItemPriceMinusVAT);
                cmd2.Parameters.AddWithValue("@costPrice", parsedCostPrice);
                cmd2.Parameters.AddWithValue("@stock", parsedStock);
                cmd2.Parameters.AddWithValue("@vatPercent", AddItemVATPercentage);
                await cmd2.ExecuteNonQueryAsync();
                HideAddItemSection();
                await GetProductsTable();
                await Application.Current.MainPage.DisplayAlert("Success", "Successfully Added New Item", "Ok");
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message, SignificanceLevels.CRITICAL, UsernameValue ?? "Unknown", "AddItem");
            }
        }
    }
}
