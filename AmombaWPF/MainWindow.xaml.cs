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
                        VerticalAlignment = VerticalAlignment.Top,
                    };
                    button.Click += Lepes; //*nevetés Üdvözöllek a Béke Szigetén!
                    gridJatekter.Children.Add(button);
                }
            }
        }

        
    }

    private void Lepes(object sender, RoutedEventArgs e)
    {
        int sorszam = int.Parse((sender as Button).Name.Split('_')[1]);
        int oszlopSzam = int.Parse((sender as Button).Name.Split('_')[2]);

        //if (jatekter[sorszam, oszlopSzam] == "O" && jatekos == "O" || jatekter[sorszam, oszlopSzam] == "X" && jatekos == "X")
        //{
        //    MessageBox.Show("Ez a mező már foglalt!");
        //}
        //else
        //{
            
        //    jatekter[sorszam, oszlopSzam] = jatekos;
        //    jatekos = jatekos == "X" ? "O" : "X";
        //    JatekterKiir();
        //}

        if (jatekter[sorszam, oszlopSzam] == "O" && jatekos == "O")
        {
            MessageBox.Show("Ezt a mezőt már kiválaszottad");
        }
        else if (jatekter[sorszam, oszlopSzam] == "O" && jatekos == "X")
        {
            MessageBox.Show("Ez a mező már foglalt");
        }
        else if (jatekter[sorszam, oszlopSzam] == "X" && jatekos == "O")
        {
            MessageBox.Show("Ez a mező már foglalt");
        }
        else if (jatekter[sorszam, oszlopSzam] == "X" && jatekos == "X")
        {
            MessageBox.Show("Ezt a mezőt már kiválaszottad");
        }
        else
        {
            jatekter[sorszam, oszlopSzam] = jatekos;

            JatekterKiir();
            if (Gyozelem())
            {
                MessageBox.Show("Győzelem!");
            }
            jatekos = jatekos == "X" ? "O" : "X";
            
        }
    }

    private bool Gyozelem()
    {
        // Vízszintes ellenőrzés
        for (int i = 0; i < jatekter.GetLength(0); i++)
        {
            for (int j = 0; j < jatekter.GetLength(1) - 4; j++)
            {
                if (jatekter[i, j] == jatekos && jatekter[i, j + 1] == jatekos && jatekter[i, j + 2] == jatekos)
                {
                    return true;
                }
            }
        }
        // Függőleges ellenőrzés
        for (int i = 0; i < jatekter.GetLength(0) - 4; i++)
        {
            for (int j = 0; j < jatekter.GetLength(1); j++)
            {
                if (jatekter[i, j] == jatekos && jatekter[i + 1, j] == jatekos && jatekter[i + 2, j] == jatekos)
                {
                    return true;
                }
            }
        }
        // Átlós ellenőrzés
        for (int i = 0; i < jatekter.GetLength(0) - 4; i++)
        {
            for (int j = 0; j < jatekter.GetLength(1) - 4; j++)
            {
                if (jatekter[i, j] == jatekos && jatekter[i + 1, j + 1] == jatekos && jatekter[i + 2, j + 2] == jatekos)
                {
                    return true;
                }
            }
        }

        return false;
    }


    private void Kilepes(object sender, RoutedEventArgs e)
    {
        if (MessageBox.Show("Biztos ki akarsz lépni?", "Kilépés", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
        {
            Application.Current.Shutdown();
        }
    }
}

// Házi feladat:
// Győzelem megnézése
// Mentés (txt)
// Betöltés