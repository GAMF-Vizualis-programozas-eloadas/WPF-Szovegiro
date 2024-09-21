using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Szovegiro
{
	/// <summary>
	/// Egyszerű szövegíró program menüvel és nyomógombokat tartalmazó eszköztárral.
	/// Néhány menüpont/eszköztárgomb esetén az eseménykezelést nem közvetlenül az 
	/// eseménykezelő metódus megadásával oldjuk meg (Click esemény), hanem az előre 
	/// definiált parancskészletből egy parancsot rendelünk a menüponthoz/eszköztárgombhoz, 
	/// majd az ablak konstruktorában eseménykezelőt kapcsolunk a parancshoz.
	/// </summary>
	public partial class MainWindow : Window
  {
    /// <summary>
    /// Adattag, amiben nyilvántartjuk, hogy megváltozott-e a szerkesztőmező 
    /// tartalma a legutóbbi mentés óta.
    /// </summary>
    bool VoltVáltozás;

    /// <summary>
    /// Állománynév tárolására szolgáló adattag.
    /// </summary>
    string ÁllományNév = "valami.txt";

    public MainWindow()
    {
      InitializeComponent();
      // Parancscsatoló objektum létrehozása a mentés funkcióhoz
      var cbMentés = new CommandBinding(
        ApplicationCommands.Save);
      // Eseménykezelő metódus parancshoz rendelése
      cbMentés.Executed += cbMentés_Executed;

      // Parancscsatoló objektum létrehozása a megnyitás funkcióhoz
      CommandBinding cbMegnyitás =
        new CommandBinding(ApplicationCommands.Open);
      // Eseménykezelő metódus parancshoz rendelése
      cbMegnyitás.Executed += cbMegnyitás_Executed;

      // Parancscsatoló objektum létrehozása az új dokumentum funkcióhoz
      CommandBinding cbÚj = new CommandBinding(
          ApplicationCommands.New);
      // Eseménykezelő metódus parancshoz rendelése
      cbÚj.Executed += cbÚj_Executed;

      // Parancscsatoló objektumok felvétele az ablak parancscsatoló listájába
      CommandBindings.Add(cbMentés);
      CommandBindings.Add(cbMegnyitás);
      CommandBindings.Add(cbÚj);
    }

    void cbÚj_Executed(object sender, ExecutedRoutedEventArgs e)
    {
      // Ha megváltozott a szerkesztőmező értéke a legutóbbi mentés óta
      if (VoltVáltozás)
      {
        // Akar-e menteni a felhasználó?
        MessageBoxResult res = MessageBox.Show("Akarsz menteni?", "Mentés kérdés",
          MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (res == MessageBoxResult.Yes)
          Mentés();
      }
      // Szövegmező tartalmának ürítése
      tbSzöveg.Text = "";
      VoltVáltozás = false;
    }

    /// <summary>
    /// Gondoskodik a szövegmező tartalmának lementéséről
    /// </summary>
    private void Mentés()
    {
      // Mentést végző objektum létrehozása, használata, lezárása
      using (StreamWriter sw = new StreamWriter(ÁllományNév))
      {
        // Szövegmező tartalmának mentése
        sw.Write(tbSzöveg.Text);
      }
      VoltVáltozás = false;
      MessageBox.Show("Mentés megtörtént!");
    }

    void cbMegnyitás_Executed(object sender, ExecutedRoutedEventArgs e)
    {
      // Ha megváltozott a szerkesztőmező értéke a legutóbbi mentés óta
      if (VoltVáltozás)
      {
        // Akar-e menteni a felhasználó?
        MessageBoxResult res = MessageBox.Show("Akarsz menteni?", "kérdés",
          MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (res == MessageBoxResult.Yes)
          Mentés();
      }
      // Beolvasást végző objektum létrehozása
      StreamReader sr = new StreamReader("valami.txt");
      // Az állomány teljes tartalmának beolvasása
      tbSzöveg.Text = sr.ReadToEnd();
      // Állomány lezárása
      sr.Close();
      VoltVáltozás = false;
    }

    void cbMentés_Executed(object sender, ExecutedRoutedEventArgs e)
    {
      Mentés();
    }

    private void miKilépés_Click(object sender, RoutedEventArgs e)
    {
      // Ha megváltozott a szerkesztőmező értéke a legutóbbi mentés óta
      if (VoltVáltozás)
      {
        // Akar-e menteni a felhasználó?
        MessageBoxResult res = MessageBox.Show("Akarsz menteni?", "kérdés",
          MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (res == MessageBoxResult.Yes)
          Mentés();

      }
      // Alkalmazás leállítása
      Application.Current.Shutdown();
    }

    private void tbSzöveg_TextChanged(object sender, TextChangedEventArgs e)
    {
      VoltVáltozás = true;

    }
  }

}
