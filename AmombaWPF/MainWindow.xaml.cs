using Microsoft.VisualBasic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AmombaWPF;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public static string[,]? jatekter;
    public static string jatekos = "X";
    public MainWindow()
    {
        InitializeComponent();
    }

    private void UjJatek(object sender, RoutedEventArgs e)
    {        
        int sorszam = int.Parse(tbxSor.Text);
        int oszlopSzam = int.Parse(tbxOszlop.Text);
        jatekter = new string[sorszam, oszlopSzam];
        for (int i = 0; i < jatekter.GetLength(0); i++)
        {
            for (int j = 0; j < jatekter.GetLength(1); j++)
            {
                jatekter[i, j] = "";
            }
        }

        jatekter[0, 0] = "X";
        JatekterKiir();
    }

    private void JatekterKiir()
    {
        int meret = 20;

        if (jatekter == null)
        {
            MessageBox.Show("Nincs feltöltve a játéktér!");
            return;
        }
        else
        {
            gridJatekter.Children.Clear();
            // A gridJatekter rácshoz gombot adunk
            for (int i = 0; i < jatekter.GetLength(0); i++)
            {
                for (int j = 0; j < jatekter.GetLength(1); j++)
                {
                    Button button = new()
                    {
                        Name = $"btn_{i}_{j}",
                        Content = jatekter[i, j],
                        Width = meret,
                        Height = meret,
                        // A gomb pozícióját a Margin tulajdonsággal állítjuk be
                        Margin = new Thickness(j * meret, i * meret, 0, 0),
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top
                    };
                    button.Click += Lepes;
                    gridJatekter.Children.Add(button);
                }
            }
        }

        
    }

    private void Lepes(object sender, RoutedEventArgs e)
    {
        
    }
}