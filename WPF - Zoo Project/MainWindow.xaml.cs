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
                .ConnectionStrings["WPF___Zoo_Project.Properties.Settings.PanjutorialsDBConnectionString"].ConnectionString;
               

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
        private void ShowAssociatedAnimals()
        {
            try
            {
                string query = "SELECT * FROM Animal a inner join ZooAnimal za on a.id = za.AnimalId where za.Zooid = @zooID;";

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
                MessageBox.Show(e.ToString());
            }
            
        }

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

        }

        
    }
}
