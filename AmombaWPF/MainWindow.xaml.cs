using Microsoft.VisualBasic;
using Microsoft.Win32;
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
using System.IO;

namespace AmombaWPF;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public static string[,]? jatekter;
    public static string jatekos = "X";
    public bool jatekVege = false;
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
                        IsEnabled = true
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

    private void Letilt()
    {
       foreach (var btn in gridJatekter.Children)
        {
            if (btn is Button)
            {
                (btn as Button).IsEnabled = false;
            }
        }
        jatekVege = true;
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
                    Letilt();
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
                    Letilt();
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
                    Letilt();
                    return true;
                }
            }
        }

        return false;
    }

    private void FajlMentese(object sender, RoutedEventArgs e)
    {
        SaveFileDialog saveFileDialog = new SaveFileDialog();

        saveFileDialog.Filter = "Szövegfájlok (*.txt)|*.txt|Minden fájl (*.*)|*.*";
        saveFileDialog.DefaultExt = "txt";
        saveFileDialog.FileName = "amoba_mentett_jatek";

        if (saveFileDialog.ShowDialog() == true)
        {
            try
            {
                using (StreamWriter ir = new(saveFileDialog.FileName))
                {
                    if (jatekter == null)
                    {
                        MessageBox.Show("Nincs feltöltve a játéktér!");
                        return;
                    }

                    ir.WriteLine($"{jatekVege}");
                    ir.WriteLine($"Győztes játékos: {jatekos}");

                    for (int i = 0; i < jatekter.GetLength(0); i++)
                    {
                        for (int j = 0; j < jatekter.GetLength(1); j++)
                        {
                            if (!string.IsNullOrEmpty(jatekter[i, j]))
                            {
                                ir.Write($"{jatekter[i, j]}|");
                            }
                            else
                            {
                                ir.Write(" |");
                            }
                        }
                        ir.Write("\n");
                    }
                    MessageBox.Show("Fájl sikeresen mentve: " + saveFileDialog.FileName);
                }
            } catch(Exception ex)
            {
                MessageBox.Show("Sikertelen mentés! " + ex);
            } 
        }
    }

    private void FajlBetoltese(object sender, RoutedEventArgs e)
    {
        if (jatekter == null)
        {
            MessageBox.Show("Nincs feltöltve a játéktér!");
            return;
        }


        OpenFileDialog fileDialog = new();
        fileDialog.Title = "Fájl megnyitása";
        fileDialog.Filter = "Szöveges dokumentum (.txt)| *.txt| Minden fájl| *.*";

        bool? kapottErtek = fileDialog.ShowDialog();

        if (kapottErtek == true)
        {
            try
            {
                using (StreamReader olvas = new(fileDialog.FileName))
                {

                    var xd = olvas.ReadLine();

                    olvas.ReadLine(); // Győztes játékos

                    int i = 0;
                    int j = 0;
                    while (!olvas.EndOfStream)
                    {
                        string[] adatok = olvas.ReadLine().Split('|');
                        for (int k = 0; k < adatok.Length - 1; k++)
                        {
                            jatekter[i, j] = adatok[k];
                            j++;
                        }
                        i++;
                        j = 0;
                    }
                    JatekterKiir();

                    if (bool.Parse(xd))
                    {
                        Letilt();
                    }
                    else
                    {
                        MessageBox.Show("A játék folytatódik!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nem sikerült beolvasni! " + ex);
            }
        }
        else
        {
            return;
        }
       
    }


    private void Kilepes(object sender, RoutedEventArgs e)
    {
        if (MessageBox.Show("Biztos ki akarsz lépni?", "Kilépés", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
        {
            Application.Current.Shutdown();
        }
    }
}