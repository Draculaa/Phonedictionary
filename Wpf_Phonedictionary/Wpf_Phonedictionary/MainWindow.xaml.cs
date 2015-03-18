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


namespace Wpf_Phonedictionary
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Wpf_Phonedictionary.Wcf_lib.Service1Client client = new Wpf_Phonedictionary.Wcf_lib.Service1Client();
        //public System.Windows.Data.DataTable DataTable = new System.Windows.Data.DataTable();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Wpf_Phonedictionary.Wcf_lib.Service1Client client = new Wpf_Phonedictionary.Wcf_lib.Service1Client();
            string text = client.GetData(TextBox1.Text);
            TextBox2.Text = text;       

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            Dictionary<string, string>  dic = client.GetAllData();
            DataGrid1.ItemsSource = dic;
            DataGrid1.Items.Refresh();
            
           
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //Wpf_Phonedictionary.Wcf_lib.Service1Client client = new Wpf_Phonedictionary.Wcf_lib.Service1Client();
            client.AddPhone(TextBox3.Text, TextBox4.Text);
            Dictionary<string, string> dic = client.GetAllData();
            DataGrid1.ItemsSource = dic;
            DataGrid1.Items.Refresh();
            //Dictionary<string, string> dic = client.GetAllData();
             
        }
    }
}
