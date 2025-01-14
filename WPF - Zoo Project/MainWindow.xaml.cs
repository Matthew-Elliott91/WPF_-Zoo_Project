using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
namespace WPF___Zoo_Project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SqlConnection sqlConnection;

        public MainWindow()
        {
            InitializeComponent();

            string connectionString = ConfigurationManager
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
                string query = "SELECT * FROM Zoo";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);

                using (sqlDataAdapter)
                {
                    DataTable zooTable = new DataTable();
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
                string query =
                    "SELECT * FROM Animal a inner join ZooAnimal za on a.id = za.AnimalId where za.Zooid = @zooID;";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("@zooID", listZoos.SelectedValue);

                    DataTable associatedAnimalsTable = new DataTable();
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
                string query = "Select * From Animal";



                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);

                using (sqlDataAdapter)
                {


                    DataTable showAllAnimalsTable = new DataTable();
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
                string query = "DELETE FROM Zoo WHERE Id = @ZooID";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
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
                string query = "INSERT INTO Zoo VALUES (@Location)";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
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
                string query = "Insert into ZooAnimal (ZooID, AnimalId) Values (@SelectedZoo, @SelectedAnimal);";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
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
                string query = "DELETE FROM ZooAnimal WHERE AnimalId = @AnimalId AND ZooId = @ZooId";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
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
                string query = "Insert into Animal (Name) values (@input);";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
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
                string query = "DELETE FROM Animal WHERE Id = @AnimalId";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
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
                string query =
                    "SELECT location from zoo where Id = @ZooId;";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("@ZooID", listZoos.SelectedValue);

                    DataTable zooDataTable = new DataTable();
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
                string query =
                    "SELECT Name from Animal where Id = @AnimalId;";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("@AnimalId", listAllAnimals.SelectedValue);

                    DataTable animalDataTable = new DataTable();
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
                string query = "Update Zoo Set Location = @InputBox where Id = @SelectedValue";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
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
                string query = "Update Animal Set Name = @InputBox where  Id = @SelectedValue";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
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
                
          


