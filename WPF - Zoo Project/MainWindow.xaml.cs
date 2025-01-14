using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace WPF___Zoo_Project
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly SqlConnection sqlConnection;

        public MainWindow()
        {
            InitializeComponent();

            var connectionString = ConfigurationManager
                .ConnectionStrings["WPF___Zoo_Project.Properties.Settings.PanjutorialsDBConnectionString"]
                .ConnectionString;


            sqlConnection = new SqlConnection(connectionString);

            ShowZoos();
            ShowAllAnimals();
        }

        private void ShowZoos()
        {
            try
            {
                var query = "SELECT * FROM Zoo";
                var sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);

                using (sqlDataAdapter)
                {
                    var zooTable = new DataTable();
                    sqlDataAdapter.Fill(zooTable);

                    // What info should be shown in the listbox
                    listZoos.DisplayMemberPath = "Location";
                    //What value should be delivered when an item is selected
                    listZoos.SelectedValuePath = "Id";
                    //The reference to the data the listbox should populate
                    listZoos.ItemsSource = zooTable.DefaultView;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        // Showing Animals in selected zoo
        private void ShowAssociatedAnimals()
        {
            try
            {
                var query =
                    "SELECT * FROM Animal a inner join ZooAnimal za on a.id = za.AnimalId where za.Zooid = @zooID;";

                var sqlCommand = new SqlCommand(query, sqlConnection);

                var sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("@zooID", listZoos.SelectedValue);

                    var associatedAnimalsTable = new DataTable();
                    sqlDataAdapter.Fill(associatedAnimalsTable);

                    // What info should be shown in the listbox
                    listAssociatedAnimals.DisplayMemberPath = "Name";
                    //What value should be delivered when an item is selected
                    listAssociatedAnimals.SelectedValuePath = "Id";
                    //The reference to the data the listbox should populate
                    listAssociatedAnimals.ItemsSource = associatedAnimalsTable.DefaultView;
                }
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.ToString());
            }
        }

        //Showing all animals in the database
        private void ShowAllAnimals()
        {
            try
            {
                var query = "Select * From Animal";


                var sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);

                using (sqlDataAdapter)
                {
                    var showAllAnimalsTable = new DataTable();
                    sqlDataAdapter.Fill(showAllAnimalsTable);

                    // What info should be shown in the listbox
                    listAllAnimals.DisplayMemberPath = "Name";
                    //What value should be delivered when an item is selected
                    listAllAnimals.SelectedValuePath = "Id";
                    //The reference to the data the listbox should populate
                    listAllAnimals.ItemsSource = showAllAnimalsTable.DefaultView;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private void listZoos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowAssociatedAnimals();
            ShowSelectedZooInTextBox();
        }

        private void listAllAnimals_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowSelectedAnimalInTextBox();
        }

        // Delete Zoo Button
        private void DeleteZoo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var query = "DELETE FROM Zoo WHERE Id = @ZooID";
                var sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@ZooID", listZoos.SelectedValue);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowZoos();
            }
        }

        // Add Zoo Button
        private void AddZoo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var query = "INSERT INTO Zoo VALUES (@Location)";
                var sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@Location", InputBox.Text);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowZoos();
            }
        }

        // Add animal to zoo button
        private void AddAnimalToZoo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var query = "Insert into ZooAnimal (ZooID, AnimalId) Values (@SelectedZoo, @SelectedAnimal);";
                var sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@SelectedZoo", listZoos.SelectedValue);
                sqlCommand.Parameters.AddWithValue("@SelectedAnimal", listAllAnimals.SelectedValue);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowAssociatedAnimals();
            }
        }

        // Remove Animal Button
        private void RemoveAnimal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var query = "DELETE FROM ZooAnimal WHERE AnimalId = @AnimalId AND ZooId = @ZooId";
                var sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@AnimalId", listAssociatedAnimals.SelectedValue);
                sqlCommand.Parameters.AddWithValue("@ZooId", listZoos.SelectedValue);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowAssociatedAnimals();
            }
        }

        // Add Animal Button
        private void AddAnimal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var query = "Insert into Animal (Name) values (@input);";
                var sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@input", InputBox.Text);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowAllAnimals();
                InputBox.Clear();
            }
        }

        // Delete Animal Button
        private void DeleteAnimal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var query = "DELETE FROM Animal WHERE Id = @AnimalId";
                var sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@AnimalId", listAllAnimals.SelectedValue);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowAllAnimals();
            }
        }

        // Show selected zoo in InputBox
        private void ShowSelectedZooInTextBox()
        {
            try
            {
                var query =
                    "SELECT location from zoo where Id = @ZooId;";

                var sqlCommand = new SqlCommand(query, sqlConnection);

                var sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("@ZooID", listZoos.SelectedValue);

                    var zooDataTable = new DataTable();
                    sqlDataAdapter.Fill(zooDataTable);


                    InputBox.Text = zooDataTable.Rows[0]["Location"].ToString();
                }
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.ToString());
            }
        }

        private void ShowSelectedAnimalInTextBox()
        {
            try
            {
                var query =
                    "SELECT Name from Animal where Id = @AnimalId;";

                var sqlCommand = new SqlCommand(query, sqlConnection);

                var sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("@AnimalId", listAllAnimals.SelectedValue);

                    var animalDataTable = new DataTable();
                    sqlDataAdapter.Fill(animalDataTable);


                    InputBox.Text = animalDataTable.Rows[0]["Name"].ToString();
                }
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.ToString());
            }
        }

        // Update Zoo Button
        private void UpdateZoo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var query = "Update Zoo Set Location = @InputBox where Id = @SelectedValue";
                var sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@InputBox", InputBox.Text);
                sqlCommand.Parameters.AddWithValue("@SelectedValue", listZoos.SelectedValue);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowZoos();
                MessageBox.Show("Zoo updated");
            }
        }

        // Update Animal Button
        private void UpdateAnimal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var query = "Update Animal Set Name = @InputBox where  Id = @SelectedValue";
                var sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@InputBox", InputBox.Text);
                sqlCommand.Parameters.AddWithValue("@SelectedValue", listAllAnimals.SelectedValue);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowAllAnimals();
                MessageBox.Show("Animal updated");
            }
        }
    }
}